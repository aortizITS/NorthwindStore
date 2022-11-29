using Northwind.Store.Web.Internet.Extensions;
using Northwind.Store.Web.Internet.ViewModels;

namespace Northwind.Store.Web.Internet.Settings
{
    public class SessionSettings
    {
        private readonly ISession _session;
        public SessionSettings(IHttpContextAccessor hca)
        {
            _session = hca?.HttpContext.Session;
        }
        
        public CartViewModel Cart
        {
            get
            {
                var cart = _session.GetObject<CartViewModel>(nameof(Cart));

                if (cart == null)
                {
                    cart = new CartViewModel();
                }

                return cart;
            }
            set
            {
                _session.SetObject(nameof(Cart), value);
            }
        }
        public void CustomClearSession()
        {
            _session.Clear();
        }
    }
}
