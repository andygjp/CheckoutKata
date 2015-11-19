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
                ['A'] = new BuyXForYItem(50, 3, 130),
                ['B'] = new BuyXForYItem(30, 2, 45),
                ['C'] = new Item(20),
                ['D'] = new Item(15)
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