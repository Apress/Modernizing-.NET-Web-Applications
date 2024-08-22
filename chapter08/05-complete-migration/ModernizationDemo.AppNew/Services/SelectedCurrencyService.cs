namespace ModernizationDemo.AppNew.Services;

public class SelectedCurrencyService
{
    public string SelectedCurrency { get; private set; } = "USD";

    public event Action? SelectedCurrencyChanged;

    public Task<string> GetCurrency()
    {
        return Task.FromResult(SelectedCurrency);
    }

    public Task SetCurrency(string currency)
    {
        SelectedCurrency = currency;
        SelectedCurrencyChanged?.Invoke();
        return Task.CompletedTask;
    }
}
