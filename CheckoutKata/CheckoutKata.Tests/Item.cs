namespace CheckoutKata
{
    public class Item
    {
        public Item(int price)
        {
            Price = price;
        }

        protected int Price { get; }
        protected int Scanned { get; private set; }

        public virtual int SubTotal()
        {
            return Price*Scanned;
        }

        public void Scan()
        {
            Scanned++;
        }
    }
}