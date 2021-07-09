using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P0Main
{
    interface IBuissnessModel
    {
        public void Startup();
        public void ChooseLocation();
        public int MainMenuOptions();
        public void BrowseProducts();
        public void BrowseCategories();
        public void AddToCart(int productId, int amountInStore);
        public void checkoutCart();

    }
}
