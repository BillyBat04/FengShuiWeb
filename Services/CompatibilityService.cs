using FengShuiWeb.Helpers;
using FengShuiWeb.Services;

public class CompatibilityService : ICompatibilityService
{
    public string EvaluateCompatibility(DateTime a, DateTime b)
    {
        var m1 = ElementHelper.GetElementFromYear(a.Year);
        var m2 = ElementHelper.GetElementFromYear(b.Year);

        if (ElementHelper.IsCompatible(m1, m2))
            return $"Hợp phong thủy: {m1} sinh {m2}";
        else
            return $"Không hợp phong thủy: {m1} khắc {m2}";
    }
}