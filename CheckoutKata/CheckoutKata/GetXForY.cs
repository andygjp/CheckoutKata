namespace CheckoutKata
{
    public class GetXForY : NormalPrice
    {
        private readonly int _x;
        private readonly double _y;

        public GetXForY(char item, double unitPrice, int x, double y) : base(item, unitPrice)
        {
            _x = x;
            _y = y;
        }

        public override double SubTotal(int numberOfUnits)
        {
            return GetPromotionalSubTotal(numberOfUnits) + GetNonPromotionalSubTotal(numberOfUnits);
        }

        private double GetPromotionalSubTotal(int numberOfUnits)
        {
            int x = numberOfUnits/_x;
            return _y*x;
        }

        private double GetNonPromotionalSubTotal(int numberOfUnits)
        {
            return base.SubTotal(GetNumberOfNonPromotionUnits(numberOfUnits));
        }

        private int GetNumberOfNonPromotionUnits(int numberOfUnits)
        {
            return numberOfUnits % _x;
        }
    }
}