using Newtonsoft.Json.Linq;

namespace FengShuiWeb.Infrastructure.DataProviders
{
    public class FengShuiDataProvider
    {
        private readonly JObject _data;

        public FengShuiDataProvider(IWebHostEnvironment environment)
        {
            var filePath = Path.Combine(environment.ContentRootPath, "Resources", "fengshui_data.json");
            var json = File.ReadAllText(filePath);
            _data = JObject.Parse(json);
        }

        public JObject GetData() => _data;
    }
}
