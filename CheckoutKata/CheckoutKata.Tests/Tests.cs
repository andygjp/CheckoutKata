namespace CheckoutKata.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
                new Checkout(new GetXForYProxy('A', 50.0, 3, 130.0), new GetXForYProxy('B', 30, 2, 45.0), new NormalPrice('C', 20.0), new NormalPrice('D', 15.0));
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
            var sut = new GetXForYProxy('A', 50.0, 3, 130.0);
            double actual = sut.SubTotal(numberOfUnits);
            actual.Should().Be(expected);
        }
    }

    public class PromotionResult
    {
        public PromotionResult(double price, int numberOfNonPromotionUnits)
        {
            Price = price;
            NumberOfNonPromotionUnits = numberOfNonPromotionUnits;
        }

        public double Price { get; }
        public int NumberOfNonPromotionUnits { get; }
    }
    
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

    public class GetXForYProxy : SubTotalCalculator
    {
        private readonly int _x;
        private readonly double _y;

        public GetXForYProxy(char item, double unitPrice, int x, double y) : this(item, unitPrice)
        {
            _x = x;
            _y = y;
        }

        private GetXForYProxy(char item, double unitPrice) : base(item, unitPrice)
        {
        }

        public override double SubTotal(int numberOfUnits)
        {
            var promotionalPrice = GetPromotionalPrice(numberOfUnits);
            double subTotal = promotionalPrice.Price;
            int unitCount = 0;
            while (unitCount < promotionalPrice.NumberOfNonPromotionUnits)
            {
                subTotal += UnitPrice;
                unitCount++;
            }
            return subTotal;
        }

        private PromotionResult GetPromotionalPrice(int numberOfUnits)
        {
            int numberOfUnits1 = numberOfUnits;
            var price = 0.0;
            if (numberOfUnits1 >= _x)
            {
                int x = numberOfUnits1 /_x;
                numberOfUnits1 = numberOfUnits1 %_x;
                price = _y * x;
            }
            return new PromotionResult(price, numberOfUnits1);
        }
    }

    public class Checkout
    {
        private readonly Dictionary<char, UnitTally> _unitTallies;

        public Checkout(params SubTotalCalculator[] itemPrices)
        {
            _unitTallies = itemPrices.ToDictionary(x => x.Item, x => new UnitTally(x));
        }

        public double GetTotal()
        {
            return _unitTallies.Values.Sum(x => x.SubTotal());
        }

        public void Scan(char item)
        {
            var tally = FindTally(item);
            tally.AddAnotherUnit();
        }

        private UnitTally FindTally(char item)
        {
            return _unitTallies[item];
        }

        private class UnitTally
        {
            private int _numberOfUnits;
            private readonly SubTotalCalculator _subTotalCalculator;

            public UnitTally(SubTotalCalculator subTotalCalculator)
            {
                _subTotalCalculator = subTotalCalculator;
            }

            public void AddAnotherUnit()
            {
                _numberOfUnits++;
            }

            public double SubTotal()
            {
                return _subTotalCalculator.SubTotal(_numberOfUnits);
            }
        }
    }
}
