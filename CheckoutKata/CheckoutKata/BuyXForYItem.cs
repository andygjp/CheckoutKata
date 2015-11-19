namespace CheckoutKata
{
    public class BuyXForYItem : Item
    {
        private readonly int _x;
        private readonly int _promotion;
        
        public BuyXForYItem(int price, int x, int promotion) : base(price)
        {
            _x = x;
            _promotion = promotion;
        }

        public override int SubTotal()
        {
            return GetPromotionalPrice() + GetNonPromotionalPrice();
        }

        private int GetNonPromotionalPrice()
        {
            return Scanned%_x*Price;
        }

        private int GetPromotionalPrice()
        {
            return Scanned/_x*_promotion;
        }
    }
}