namespace FengShuiWeb.Helpers
{
    public static class ElementHelper
    {
        public static string GetElementFromYear(int year)
        {
            string[] elements = { "Kim", "Thủy", "Hỏa", "Thổ", "Mộc" };
            // Giản lược logic, dùng can chi thực tế nếu muốn chuẩn
            return elements[year % 5];
        }

        public static bool IsCompatible(string e1, string e2)
        {
            return (e1, e2) switch
            {
                ("Mộc", "Hỏa") or ("Hỏa", "Thổ") or ("Thổ", "Kim") or ("Kim", "Thủy") or ("Thủy", "Mộc") => true,
                _ => false
            };
        }
    }
}
