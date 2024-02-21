using Currency_Exchange.Models;
using Microsoft.AspNetCore.Components;
using Services;

namespace Currency_Exchange.Pages
{
    public partial class Exchange
    {
        #region Injectors
        [Inject]
        private HttpClient Http { get; set; }
        [Inject]
        private ICurrencyService CurrencyService { get; set; }
        [Inject]
        private DataService DataService { get; set; }
        [Inject]
        private ExceptionHandlerService ExceptionHandler { get; set; }
        #endregion

        #region Fields & Properties
        public List<Currency> Currencies;
        public List<Currency> AvailableCurrencies;
        private Currency currency = new();
        #endregion

        #region Life Cycle
        protected override async Task OnInitializedAsync()
        {

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Main();
                StateHasChanged();
            }
        }
        #endregion

        #region Main
        public async Task Main()
        {
            try
            {
                await LoadCurrencies();

                AvailableCurrencies = await CurrencyService.GetAllCurrencies();

                currency = new Currency();

                StateHasChanged();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        private async Task LoadCurrencies()
        {
            p_Currency_Select uparam = new();

            Currencies = await DataService.ExecuteStoredProcedure<Currency>("p_Currency_Select", uparam);

            foreach (var currency in Currencies)
            {
                currency.ExchangeRate = await CurrencyService.GetExchangeRate(currency.Code, "ZAR");
            }
        }

        private async Task HandleValidSubmit()
        {
            try
            {
                await UpdateCurrency();

                await Main();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        private void EditCurrency(Currency currencyToEdit)
        {
            currency = currencyToEdit;
        }

        private async Task UpdateCurrency()
        {
            if (AvailableCurrencies.Any(x => x.Code == currency.Code))
            {
                p_Currency_Update uparam = new();

                uparam.p_Id = currency.Id ?? null;
                uparam.p_Code = currency.Code;
                uparam.p_Name = currency.Name;
                uparam.p_MdUserId = 1;
                uparam.p_MdDateTime = DateTime.Now;
                uparam.p_ExchangeRate = await CurrencyService.GetExchangeRate(currency.Code, "ZAR");

                var updateReturn = await DataService.ExecuteStoredProcedure<Currency>("p_Currency_Update", uparam);

                if (updateReturn.Count == 0)
                {
                    throw new Exception("Currency Update Failed.");
                }
            }
            else
            {
                throw new Exception("Currency Code does not Exist");
            }

            await Main();
        }

        private async Task DeleteCurrency(Currency currencyToDelete)
        {
            try
            {
                p_Currency_Delete uparam = new();

                if (currencyToDelete.Id != null)
                {
                    uparam.InputId = currencyToDelete.Id;

                    await DataService.ExecuteStoredProcedure<Currency>("p_Currency_Delete", uparam);
                }
                else
                {
                    throw new Exception("No item to Delete");
                }

                await Main();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
        #endregion
    }
}
