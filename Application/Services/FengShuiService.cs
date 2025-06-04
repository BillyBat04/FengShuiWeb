using AutoMapper.Internal;
using AutoMapper;
using FengShuiWeb.Application.DTOs;
using FengShuiWeb.Application.Interfaces;
using FengShuiWeb.Domain.Models;
using FengShuiWeb.Infrastructure.DataProviders;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;
using GeminiDotNET;
using GeminiDotNET.ApiModels.ApiRequest;
using GeminiDotNET.ApiModels.Enums;
using GeminiValidator = GeminiDotNET.Validator;

namespace FengShuiWeb.Application.Services
{
    public class FengShuiService : IFengShuiService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFengShuiAnalysisRepository _analysisRepository;
        private readonly FengShuiDataProvider _dataProvider;
        private readonly Generator _geminiGenerator;
        private readonly IMapper _mapper;
        private readonly string _geminiApiKey;

        public FengShuiService(
            IUserRepository userRepository,
            IFengShuiAnalysisRepository analysisRepository,
            FengShuiDataProvider dataProvider,
            IConfiguration configuration,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _analysisRepository = analysisRepository;
            _dataProvider = dataProvider;
            _mapper = mapper;
            _geminiApiKey = configuration["GeminiApiKey"];

            if (!GeminiValidator.CanBeValidApiKey(_geminiApiKey))
            {
                throw new InvalidOperationException("Định dạng API Key không hợp lệ.");
            }

            _geminiGenerator = new Generator(_geminiApiKey);
        }

        public async Task<FengShuiResponseDto> GetFengShuiAnalysisAsync(FengShuiRequestDto dto)
        {
            bool isValidApiKey = await GeminiValidator.IsValidApiKeyAsync(_geminiApiKey);
            if (!isValidApiKey)
            {
                throw new InvalidOperationException("API Key không hợp lệ hoặc không có quyền truy cập Gemini API.");
            }

            if (dto.BirthYear < 1900 || dto.BirthYear > DateTime.UtcNow.Year)
            {
                throw new ArgumentException("Năm sinh không hợp lệ");
            }

            if (dto.Gender != "Male" && dto.Gender != "Female")
            {
                throw new ArgumentException("Giới tính không hợp lệ");
            }

            var validDirections = new[] { "North", "South", "East", "Southeast", "West", "Southwest", "Northwest", "Northeast" };
            if (!validDirections.Contains(dto.MainDoorDirection))
            {
                throw new ArgumentException("Hướng cửa chính không hợp lệ");
            }

            var data = _dataProvider.GetData();
            var kuaNumber = CalculateKuaNumber(dto.BirthYear, dto.Gender);
            var profile = GetProfile(dto.BirthYear, kuaNumber);
            var directions = GetDirections(kuaNumber);
            var layoutTips = GetLayoutTips(kuaNumber, dto.MainDoorDirection);
            var friendlyExplanation = await GetFriendlyExplanationAsync(profile, directions, layoutTips);

            var response = new FengShuiResponseDto
            {
                Profile = profile,
                Directions = directions,
                LayoutTips = layoutTips,
                FriendlyExplanation = friendlyExplanation
            };

            var analysisData = JsonConvert.SerializeObject(response);
            var recommendations = await GetRecommendationsAsync(analysisData);
            response.Recommendations = recommendations;

            return response;
        }

        public async Task<int> SaveAnalysisAsync(int userId, FengShuiAnalysisDto analysis)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("Người dùng không tồn tại");
            }

            var entity = new FengShuiAnalysis
            {
                UserID = userId,
                Label = analysis.Label,
                AnalysisData = analysis.AnalysisData,
                CreatedAt = DateTime.UtcNow
            };

            await _analysisRepository.CreateAsync(entity);
            return entity.Id;
        }

        public async Task<List<FengShuiAnalysisDto>> GetSavedAnalysesAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("Người dùng không tồn tại");
            }

            var analyses = await _analysisRepository.GetByUserIdAsync(userId);
            return _mapper.Map<List<FengShuiAnalysisDto>>(analyses);
        }

        public async Task<ComparisonResponseDto> CompareAnalysesAsync(int userId, ComparisonRequestDto request)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("Người dùng không tồn tại");
            }

            if (request.AnalysisIds == null || request.AnalysisIds.Count < 2)
            {
                throw new ArgumentException("Cần chọn ít nhất 2 bài phân tích để so sánh");
            }

            var analyses = await _analysisRepository.GetByIdsAsync(request.AnalysisIds, userId);
            if (analyses.Count != request.AnalysisIds.Count)
            {
                throw new Exception("Một số bài phân tích không tồn tại hoặc không thuộc về người dùng này");
            }

            var dtos = _mapper.Map<List<FengShuiAnalysisDto>>(analyses);
            var advice = await GenerateComparisonAdviceAsync(analyses);

            return new ComparisonResponseDto
            {
                Analyses = dtos,
                Advice = advice
            };
        }

        public async Task<RecommendationDto> GetRecommendationsAsync(string analysisData)
        {
            // Mock article database (in practice, this would query a real database)
            var articles = new List<(string Title, string Url, string Keyword)>
            {
                ("Phong Thủy Phòng Ngủ", "/articles/phong-ngu", "phòng ngủ"),
                ("Phong Thủy Nhà Bếp", "/articles/nha-bep", "nhà bếp"),
                ("Cải Thiện Tài Lộc", "/articles/tai-loc", "tài lộc")
            };

            var analysis = JsonConvert.DeserializeObject<FengShuiResponseDto>(analysisData);
            var keywords = ExtractKeywords(analysis);

            var recommendedArticles = new List<string>();
            foreach (var article in articles)
            {
                if (keywords.Any(k => article.Keyword.Contains(k)))
                {
                    recommendedArticles.Add($"{article.Title}: {article.Url}");
                }
            }

            var searchKeywords = recommendedArticles.Any() ? null : string.Join(" ", keywords);
            return new RecommendationDto
            {
                Articles = recommendedArticles,
                SearchKeywords = searchKeywords
            };
        }

        private int CalculateKuaNumber(int birthYear, string gender)
        {
            var lastTwoDigits = (birthYear % 100);
            var sum = (lastTwoDigits / 10) + (lastTwoDigits % 10);
            while (sum > 9)
            {
                sum = (sum / 10) + (sum % 10);
            }

            int kua;
            if (gender == "Male")
            {
                kua = 10 - sum;
            }
            else
            {
                kua = sum + 5;
                if (kua > 9) kua = (kua / 10) + (kua % 10);
            }

            if (kua == 5) kua = gender == "Male" ? 2 : 8;
            return kua;
        }

        private FengShuiProfileDto GetProfile(int birthYear, int kuaNumber)
        {
            var data = _dataProvider.GetData();
            var zodiacData = data["elements"][birthYear.ToString()];
            var kuaData = data["kua_numbers"][kuaNumber.ToString()];

            return new FengShuiProfileDto
            {
                BirthYear = $"{birthYear} ({zodiacData["zodiac"]} – {zodiacData["animal"]})",
                DestinyElement = zodiacData["element"].ToString(),
                KuaNumber = kuaNumber,
                KuaGroup = kuaData["group"].ToString() == "East" ? "Đông Tứ Mệnh" : "Tây Tứ Mệnh"
            };
        }

        private DirectionInfoDto[] GetDirections(int kuaNumber)
        {
            var data = _dataProvider.GetData();
            var kuaData = data["kua_numbers"][kuaNumber.ToString()];
            var auspicious = kuaData["auspicious"].ToObject<Dictionary<string, string>>();
            var inauspicious = kuaData["inauspicious"].ToObject<Dictionary<string, string>>();

            var directions = new List<DirectionInfoDto>();

            foreach (var dir in auspicious)
            {
                directions.Add(new DirectionInfoDto
                {
                    Direction = dir.Key,    // Tên hướng
                    Meaning = dir.Value,    // Ý nghĩa
                    IsAuspicious = true     // Hướng tốt
                });
            }

            foreach (var dir in inauspicious)
            {
                directions.Add(new DirectionInfoDto
                {
                    Direction = dir.Key,    // Tên hướng
                    Meaning = dir.Value,    // Ý nghĩa
                    IsAuspicious = false    // Hướng xấu
                });
            }

            return directions.OrderBy(d => d.Direction).ToArray();
        }

        private LayoutTipDto[] GetLayoutTips(int kuaNumber, string mainDoorDirection)
        {
            var data = _dataProvider.GetData();
            var kuaData = data["kua_numbers"][kuaNumber.ToString()];
            var auspiciousDirections = kuaData["auspicious"].ToObject<Dictionary<string, string>>();
            var inauspiciousDirections = kuaData["inauspicious"].ToObject<Dictionary<string, string>>();

            var tips = new List<LayoutTipDto>();

            // Main Door
            var mainDoorTip = new LayoutTipDto { Section = "Cửa Chính" };
            if (auspiciousDirections.ContainsKey(mainDoorDirection))
            {
                mainDoorTip.Tip = $"Cửa chính của bạn hướng {mainDoorDirection} ({auspiciousDirections[mainDoorDirection]}), rất tốt cho sự thịnh vượng và hạnh phúc.";
            }
            else
            {
                mainDoorTip.Tip = $"Cửa chính của bạn hướng {mainDoorDirection} ({inauspiciousDirections[mainDoorDirection]}), không lý tưởng. Hãy cân nhắc sử dụng các biện pháp phong thủy như đặt gương hoặc vật phẩm phong thủy để hóa giải.";
            }
            tips.Add(mainDoorTip);

            // Kitchen
            var kitchenTip = new LayoutTipDto { Section = "Nhà Bếp" };
            var badDirections = inauspiciousDirections.Keys.ToList();
            var goodDirections = auspiciousDirections.Keys.ToList();
            if (badDirections.Any() && goodDirections.Any())
            {
                kitchenTip.Tip = $"Đặt bếp ở {badDirections[0]} ({inauspiciousDirections[badDirections[0]]}) và nhìn về {goodDirections[0]} ({auspiciousDirections[goodDirections[0]]}) để cân bằng năng lượng. Tránh đặt gần bồn rửa, tủ lạnh hoặc cửa sổ phía sau.";
            }
            tips.Add(kitchenTip);

            // Bedroom
            var bedroomTip = new LayoutTipDto { Section = "Phòng Ngủ" };
            if (goodDirections.Any())
            {
                bedroomTip.Tip = $"Đặt ở hướng {goodDirections[1]} ({auspiciousDirections[goodDirections[1]]}) để hỗ trợ sức khỏe. Hướng đầu giường về {goodDirections[0]} để tăng cường năng lượng tích cực. Đặt tủ quần áo ở {badDirections[0]} để trấn áp năng lượng tiêu cực.";
            }
            tips.Add(bedroomTip);

            return tips.ToArray();
        }

        private async Task<string[]> GetFriendlyExplanationAsync(FengShuiProfileDto profile, DirectionInfoDto[] directions, LayoutTipDto[] layoutTips)
        {
            var explanations = new List<string>();

            // Profile Explanation
            var profilePrompt = $"Giải thích ý nghĩa của hồ sơ Phong Thủy sau đây một cách dễ hiểu cho người không biết về Phong Thủy: Năm sinh {profile.BirthYear}, Mệnh {profile.DestinyElement}, Số Kua {profile.KuaNumber}, Nhóm {profile.KuaGroup}.";
            var profileRequest = new ApiRequestBuilder()
                .WithPrompt(profilePrompt)
                .WithDefaultGenerationConfig(temperature: 0.7f, maxOutputTokens: 512)
                .Build();
            var profileExplanation = await CallGeminiAsync(profileRequest);
            explanations.Add(profileExplanation);

            // Directions Explanation
            var directionsPrompt = "Giải thích ý nghĩa của các hướng Phong Thủy sau đây một cách đơn giản và dễ hiểu cho người mới bắt đầu: ";
            foreach (var dir in directions)
            {
                directionsPrompt += $"- Hướng {dir.Direction}: {dir.Meaning}. ";
            }
            var directionsRequest = new ApiRequestBuilder()
                .WithPrompt(directionsPrompt)
                .WithDefaultGenerationConfig(temperature: 0.7f, maxOutputTokens: 512)
                .Build();
            var directionsExplanation = await CallGeminiAsync(directionsRequest);
            explanations.Add(directionsExplanation);

            // Layout Tips Explanation
            var layoutPrompt = "Giải thích các gợi ý bố trí nhà ở sau đây một cách thân thiện và dễ hiểu cho người không rành về Phong Thủy: ";
            foreach (var tip in layoutTips)
            {
                layoutPrompt += $"- {tip.Section}: {tip.Tip}. ";
            }
            var layoutRequest = new ApiRequestBuilder()
                .WithPrompt(layoutPrompt)
                .WithDefaultGenerationConfig(temperature: 0.7f, maxOutputTokens: 512)
                .Build();
            var layoutExplanation = await CallGeminiAsync(layoutRequest);
            explanations.Add(layoutExplanation);

            return explanations.ToArray();
        }

        private async Task<string> GenerateComparisonAdviceAsync(List<FengShuiAnalysis> analyses)
        {
            var analysisResults = analyses.Select(a => JsonConvert.DeserializeObject<FengShuiResponseDto>(a.AnalysisData)).ToList();
            var prompt = "Dựa trên các bài phân tích phong thủy sau đây, hãy đưa ra lời khuyên để người dùng chọn không gian sống tốt nhất: ";
            for (int i = 0; i < analysisResults.Count; i++)
            {
                prompt += $"\nKhông gian {i + 1} (Nhãn: {analyses[i].Label}): ";
                prompt += $"- Hướng cửa chính: {analysisResults[i].Directions.First(d => d.Direction == analysisResults[i].Directions.First().Direction).Meaning}. ";
                foreach (var tip in analysisResults[i].LayoutTips)
                {
                    prompt += $"- {tip.Section}: {tip.Tip}. ";
                }
            }

            var request = new ApiRequestBuilder()
                .WithPrompt(prompt)
                .WithDefaultGenerationConfig(temperature: 0.7f, maxOutputTokens: 512)
                .Build();

            return await CallGeminiAsync(request);
        }

        private List<string> ExtractKeywords(FengShuiResponseDto analysis)
        {
            var keywords = new List<string>();

            // Extract from directions
            foreach (var dir in analysis.Directions)
            {
                if (dir.IsAuspicious && dir.Meaning.Contains("Thịnh Vượng"))
                    keywords.Add("tài lộc");
                if (dir.Meaning.Contains("Sức Khỏe"))
                    keywords.Add("sức khỏe");
            }

            // Extract from layout tips
            foreach (var tip in analysis.LayoutTips)
            {
                if (tip.Section.Contains("Phòng Ngủ"))
                    keywords.Add("phòng ngủ");
                if (tip.Section.Contains("Nhà Bếp"))
                    keywords.Add("nhà bếp");
            }

            return keywords;
        }

        private async Task<string> CallGeminiAsync(ApiRequest request)
        {
            try
            {
                var response = await _geminiGenerator.GenerateContentAsync(request, ModelVersion.Gemini_20_Flash_Lite); 
                return response.Content ?? "Không thể diễn giải kết quả.";
            }
            catch (ArgumentNullException ex)
            {
                return $"Lỗi: Thiếu tham số cần thiết - {ex.ParamName}";
            }
            catch (InvalidOperationException ex)
            {
                return $"Lỗi: Hoạt động không hợp lệ - {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"Lỗi khi gọi Gemini API: {ex.Message}";
            }
        }
    }
}
