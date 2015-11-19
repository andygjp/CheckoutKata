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

    public class Checkout
    {
        public double GetTotal()
        {
            return 0;
        }

        public void Scan(char item)
        {
            throw new System.NotImplementedException();
        }
    }
}
