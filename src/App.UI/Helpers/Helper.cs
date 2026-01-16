using App.Common;

namespace App.UI.Helpers;

public class Helper
{
    public static string GetConfigSuffix()
    {
#if DEBUG
        return Constants.UI.DebugSuffix;
#elif QA
        return Constants.UI.QASuffix;
#else
        return string.Empty;
#endif
    }
}
