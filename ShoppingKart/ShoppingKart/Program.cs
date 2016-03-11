using System;
using System.Linq;
using ShoppingKart.Cashier.Interface;
using ShoppingKart.Repository.Fake;
using ShoppingKart.Repository.Interface;
using ShoppingKart.ShoppingBasket.Fake;
using ShoppingKart.ShoppingBasket.Interface;

namespace ShoppingKart
{
    class Program
    {
        static void Main(string[] args)
        {
            IProductCatalogueRepo catalogueRepo = new ProductCatalogueRepo();
            IInventoryRepo inventory = new InventoryRepo(catalogueRepo);
            ItemOffersRepo offers = new ItemOffersRepo(catalogueRepo);
            foreach (var inv in inventory.GetStocks().Where(i=>i.QuantityHeld > 0))
            {
                Console.WriteLine("{0}\t{1}", inv.Item.Sku, inv.Item.FullRetailPrice.ToString("C"));
                
            }
            foreach (var inv in inventory.GetStocks().Where(i => i.QuantityHeld > 0))
            {
                var firstOrDefault = offers.GetOffer(inv.Item.Sku);
                var offerStr = string.Empty;
                if (firstOrDefault != null && firstOrDefault.Offer != null)
                {
                    offerStr = firstOrDefault.Offer.ToString();
                }
                Console.WriteLine("{0}\t{1}\t{2}", inv.Item.Sku, inv.Item.FullRetailPrice, offerStr);
            }
            IShoppingBasket basket = new FakeShoppingBasket();
            basket.AddItem('A', 1);
            basket.AddItem('B', 2);
            basket.AddItem('A', 1);
            basket.AddItem('C', 1);
            basket.AddItem('D', 5);
            basket.AddItem('A', 1);
            basket.AddItem('A', 1);
            basket.AddItem('B', 2);

            ICashier cashier = new Cashier.Impl.Cashier(catalogueRepo, inventory, offers, new RuleEngine.Impl.RuleEngine());
            var bill = cashier.Checkout(basket.GetItems());
            Console.WriteLine("SKU\tQuantity\tAmount");
            foreach (var item in bill)
            {
                Console.WriteLine("{0}\t{1}\t\t{2}",item.Sku,item.Quantity,item.TotalPrice.ToString("C"));
            }

        }
    }
}
