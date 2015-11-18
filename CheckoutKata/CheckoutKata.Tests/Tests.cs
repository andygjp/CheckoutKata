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

    public class Checkout
    {
        public double GetTotal()
        {
            throw new System.NotImplementedException();
        }
    }
}
