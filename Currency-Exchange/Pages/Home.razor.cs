using Currency_Exchange.Models;
using Microsoft.AspNetCore.Components;
using Services;

namespace Currency_Exchange.Pages
{
    public partial class Home
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
                p_Currency_Select uparam = new();

                Currencies = await DataService.ExecuteStoredProcedure<Currency>("p_Currency_Select", uparam);

                foreach (var currency in Currencies)
                {
                    currency.ExchangeRate = await CurrencyService.GetExchangeRate(currency.Code, "ZAR");
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
        #endregion
    }
}
