namespace CheckoutKata
{
    using System.Collections.Generic;
    using System.Linq;

    public class Checkout
    {
        private readonly IDictionary<char, Item> _items;

        public Checkout()
        {
            _items = new Dictionary<char, Item>
            {
                ['A'] = Item.BuyXForY(50, 3, 130),
                ['B'] = Item.BuyXForY(30, 2, 45),
                ['C'] = Item.NonPromotional(20),
                ['D'] = Item.NonPromotional(15)
            };
        }

        public Checkout(IDictionary<char, Item> items)
        {
            _items = items;
        }

        public double GetTotal()
        {
            return _items.Values.Sum(x => x.SubTotal());
        }

        public void Scan(char item)
        {
            _items[item].Scan();
        }
    }
}