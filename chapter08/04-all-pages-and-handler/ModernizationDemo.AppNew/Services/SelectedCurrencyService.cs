namespace ModernizationDemo.AppNew.Services;

public class SelectedCurrencyService(IHttpContextAccessor httpContextAccessor)
{
    private string? selectedCurrency;

    public event Action? SelectedCurrencyChanged;

    public async Task<string> GetCurrency()
    {
        if (selectedCurrency == null)
        {
            selectedCurrency = System.Web.HttpContext.Current.Session["SelectedCurrency"] as string ?? "USD";
        }
        return selectedCurrency;
    }

    public async Task SetCurrency(string currency)
    {
        selectedCurrency = currency;
        SelectedCurrencyChanged?.Invoke();

        using (var sessionLock = await httpContextAccessor.HttpContext.AcquireSessionLock())
        {
            // modify the session state here
            System.Web.HttpContext.Current.Session["SelectedCurrency"] = currency;

            await sessionLock.CommitAsync();
        }
    }
}