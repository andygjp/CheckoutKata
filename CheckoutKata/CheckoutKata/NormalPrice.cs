namespace CheckoutKata
{
    public class NormalPrice : SubTotalCalculator
    {
        public NormalPrice(char item, double unitPrice) : base(item, unitPrice)
        {
        }

        public override double SubTotal(int numberOfUnits)
        {
            return UnitPrice*numberOfUnits;
        }
    }
}