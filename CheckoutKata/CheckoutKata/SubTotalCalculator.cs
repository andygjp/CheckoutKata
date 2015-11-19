namespace CheckoutKata
{
    public abstract class SubTotalCalculator
    {
        protected SubTotalCalculator(char item, double unitPrice)
        {
            Item = item;
            UnitPrice = unitPrice;
        }

        public char Item { get; }
        public double UnitPrice { get; }
        public abstract double SubTotal(int numberOfUnits);
    }
}