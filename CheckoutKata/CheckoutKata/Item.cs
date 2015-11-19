namespace CheckoutKata
{
    public class Item
    {
        private readonly int _price;
        private int _scanned;
        private readonly int _x;
        private readonly int _promotion;

        public static Item NonPromotional(int price)
        {
            return new Item(price, 1, price);
        }

        public static Item BuyXForY(int price, int x, int promotion)
        {
            return new Item(price, x, promotion);
        }

        private Item(int price, int x, int promotion)
        {
            _price = price;
            _x = x;
            _promotion = promotion;
        }

        public int SubTotal()
        {
            return GetPromotionalPrice() + GetNonPromotionalPrice();
        }

        private int GetNonPromotionalPrice()
        {
            return _scanned%_x*_price;
        }

        private int GetPromotionalPrice()
        {
            return _scanned/_x*_promotion;
        }

        public void Scan()
        {
            _scanned++;
        }
    }
}