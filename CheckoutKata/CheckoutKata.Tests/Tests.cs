namespace CheckoutKata.Tests
{
    using FluentAssertions;
    using Xunit;

    public class When_no_items_are_scanned
    {
        [Fact]
        public void It_should_return_zero()
        {
            var sut = new Checkout();
            double actual = sut.GetTotal();
            actual.Should().Be(0);
        }
    }

    public class When_I_scan_in_item_A
    {
        [Fact]
        public void It_should_return_fifty()
        {
            var sut = new Checkout();
            sut.Scan('A');
            double actual = sut.GetTotal();
            actual.Should().Be(50);
        }
    }

    public class When_I_scan_in_item_B
    {
        [Fact]
        public void It_should_return_thirty()
        {
            var sut = new Checkout();
            sut.Scan('B');
            double actual = sut.GetTotal();
            actual.Should().Be(30);
        }
    }

    public class Item
    {
        public int Price { get; } = 50;
    }

    public class Checkout
    {
        private int _total;
        private readonly Item _a = new Item();

        public double GetTotal()
        {
            return _total;
        }

        public void Scan(char item)
        {
            switch (item)
            {
                case 'A':
                    _total = GetAUnitPrice();
                    break;
                default:
                    _total = GetOtherUnitPrice();
                    break;
            }
        }

        private int GetOtherUnitPrice()
        {
            return 30;
        }

        private int GetAUnitPrice()
        {
            return _a.Price;
        }
    }
}
