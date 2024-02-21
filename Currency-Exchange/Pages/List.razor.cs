using Currency_Exchange.Models;
using Microsoft.AspNetCore.Components;
using Services;

namespace Currency_Exchange.Pages
{
    public partial class List
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
                Currencies = await CurrencyService.GetAllCurrencies();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        private async Task UpdateCurrency(Currency currency)
        {
            try
            {
                p_Currency_Update uparam = new();

                uparam.p_Id = null;
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

                Currencies = null;

                StateHasChanged();

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
