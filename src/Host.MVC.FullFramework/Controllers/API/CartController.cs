using ContosoTravel.Web.Application.Interfaces.MVC;
using ContosoTravel.Web.Application.Models;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace ContosoTravel.Web.Host.MVC.Core.Controllers.Api
{
    [RoutePrefix("api/[controller]")]
    public class CartController : ApiController
    {
        private readonly ICartController _cartController;

        public CartController(ICartController cartController)
        {
            _cartController = cartController;
        }

        [HttpGet]
        public async Task<CartModel> Index(CancellationToken cancellationToken)
        {
            var cart = await _cartController.Index(cancellationToken);

            if (cart == null)
            {
                return null;
            }

            return cart;
        }


        [HttpPost()]
        [Route("purchase")]
        public async Task<bool> Purchase(System.DateTimeOffset PurchasedOn, CancellationToken cancellationToken)
        {
            if (await _cartController.Purchase(PurchasedOn, cancellationToken))
            {
                return true;
            }

            return false;
        }
    }
}