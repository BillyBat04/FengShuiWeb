    namespace FengShuiWeb.Helpers
{
    public static class LuckyDirectionHelper
    {
        public static string GetDirection(string gender, int birthYear)
        {
            // logic rút gọn mệnh quái → hướng tốt
            // đơn giản hóa: hướng theo mệnh
            int sum = birthYear.ToString().Select(c => int.Parse(c.ToString())).Sum();
            int total = sum;
            if (gender.ToLower() == "nam")
                total = 10 - (sum % 9 == 5 ? 2 : sum % 9);
            else
                total = 5 + (sum % 9 == 5 ? 2 : sum % 9);

            // Tra hướng ví dụ (có thể dùng enum & mapping sau)
            return total switch
            {
                1 => "Bắc",
                2 => "Đông Nam",
                3 => "Đông",
                4 => "Nam",
                6 => "Tây Bắc",
                7 => "Tây",
                8 => "Đông Bắc",
                9 => "Tây Nam",
                _ => "Không xác định"
            };
        }
    }
}
