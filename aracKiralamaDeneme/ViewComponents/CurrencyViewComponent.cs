using Microsoft.AspNetCore.Mvc;
using aracKiralamaDeneme.Services;
using System.Threading.Tasks;

namespace aracKiralamaDeneme.ViewComponents
{
    public class CurrencyViewComponent : ViewComponent
    {
        private readonly CurrencyService _currencyService;

        public CurrencyViewComponent(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var rates = await _currencyService.GetRatesAsync();
            return View(rates);
        }
    }
}
