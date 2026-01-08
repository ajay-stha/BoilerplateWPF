using App.Common;

namespace App.UnitTests;

public class Helpers
{
    public static string GetBrandedTitle()
    {
        var expectedTitle = $"Localized_{Constants.Localization.Keys.AppTitle}";
#if DEBUG
        expectedTitle = $"{expectedTitle} ({Constants.UI.DebugSuffix})";
#elif QA
        expectedTitle = $"{expectedTitle} ({Constants.UI.QASuffix})";
#endif
        return expectedTitle;
    }
}
