using App.Application.Interfaces;
using App.Common;

namespace AppUI.ViewModels;

/// <summary>
/// ViewModel for the About dialog.
/// </summary>
public partial class AboutViewModel : BaseViewModel
{

    public AboutViewModel(ILocalizationService localizationService)
    {
        LoadMetadata(localizationService);
    }

    protected override void UpdateMetadata(ILocalizationService localizationService)
    {
        base.UpdateMetadata(localizationService);
        Title = GetBrandedText(localizationService.GetString(Constants.Localization.Keys.About));
    }
}
