namespace CheckoutKata.Tests
{
    using System;
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

    public class When_single_item_is_scanned
    {
        [Fact]
        public void It_should_return_expected_value()
        {
            var sut = new Checkout(new NormalPrice('A', 50.0));
            sut.Scan('A');
            double actual = sut.GetTotal();
            actual.Should().Be(50);
        }
    }

    public class DefaultItemPricingFixture
    {
        // Return a new instance
        public Checkout Sut =>
                new Checkout(new GetXForY('A', 50.0, 3, 130.0), new GetXForY('B', 30, 2, 45.0), new NormalPrice('C', 20.0), new NormalPrice('D', 15.0));
    }

    public class When_multiple_items_are_scanned : IClassFixture<DefaultItemPricingFixture>
    {
        private readonly Checkout _sut;

        public When_multiple_items_are_scanned(DefaultItemPricingFixture fixture)
        {
            _sut = fixture.Sut;
        }

        [Theory]
        [InlineData(new[] { 'A', 'B' }, 80.0)]
        [InlineData(new[] { 'C', 'D', 'B', 'A' }, 115.0)]
        [InlineData(new[] { 'A', 'A' }, 100.0)]
        public void It_should_return_expected_value(char[] items, double expected)
        {
            Array.ForEach(items, _sut.Scan);
            double actual = _sut.GetTotal();
            actual.Should().Be(expected);
        }
    }

    public class When_multiples_of_one_items_is_scanned : IClassFixture<DefaultItemPricingFixture>
    {
        private readonly Checkout _sut;

        public When_multiples_of_one_items_is_scanned(DefaultItemPricingFixture fixture)
        {
            _sut = fixture.Sut;
        }

        [Theory]
        [InlineData(new[] { 'B', 'B' }, 45.0)]
        [InlineData(new[] { 'A', 'A', 'A' }, 130.0)]
        [InlineData(new[] { 'A', 'A' }, 100.0)]
        [InlineData(new[] { 'A', 'A', 'A', 'A' }, 180.0)]
        [InlineData(new[] { 'A', 'A', 'A', 'A', 'A' }, 230.0)]
        [InlineData(new[] { 'A', 'A', 'A', 'A', 'A', 'A' }, 260.0)]
        public void It_should_return_promotional_value(char[] items, double expected)
        {
            Array.ForEach(items, _sut.Scan);
            double actual = _sut.GetTotal();
            actual.Should().Be(expected);
        }
    }

    public class When_I_calculate_promotional_price_of_get_x_for_y
    {
        [Theory]
        [InlineData(2, 100.0)]
        [InlineData(3, 130.0)]
        [InlineData(5, 230.0)]
        public void It_should_calculate_correct_price(int numberOfUnits, double expected)
        {
            var sut = new GetXForY('A', 50.0, 3, 130.0);
            double actual = sut.SubTotal(numberOfUnits);
            actual.Should().Be(expected);
        }
    }
}
