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
            var sut = new Checkout(new ItemPrice('A', 50.0));
            sut.Scan('A');
            double actual = sut.GetTotal();
            actual.Should().Be(50);
        }
    }

    public class DefaultItemPricingFixture
    {
        // Return a new instance
        public Checkout Sut => new Checkout(new ItemPrice('A', 50.0), new ItemPrice('B', 30), new ItemPrice('C', 20.0), new ItemPrice('D', 15.0));
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
        public void It_should_calculate_correct_price(int numberOfUnits, double expected)
        {
            var sut = new ItemPrice('A', 50.0, new GetXForY(3, 130.0));
            double actual = sut.SubTotal(numberOfUnits);
            actual.Should().Be(expected);
        }
    }

    public class GetXForY : IPromotion
    {
        private readonly int _x;
        private readonly double _y;

        public GetXForY(int x, double y)
        {
            _x = x;
            _y = y;
        }

        public double GetPromotionalPrice(ref int numberOfUnits)
        {
            if (numberOfUnits < _x)
            {
                return 0;
            }
            numberOfUnits = 0;
            return _y;
        }
    }

    public interface IPromotion
    {
        double GetPromotionalPrice(ref int numberOfUnits);
    }

    public class ItemPrice
    {
        private readonly IPromotion _promotion;

        public ItemPrice(char item, double unitPrice)
        {
            Item = item;
            UnitPrice = unitPrice;
        }

        public ItemPrice(char item, double unitPrice, IPromotion promotion) : this(item, unitPrice)
        {
            _promotion = promotion;
        }

        public char Item { get; }
        public double UnitPrice { get; }

        public double SubTotal(int numberOfUnits)
        {
            double subTotal = _promotion.GetPromotionalPrice(ref numberOfUnits);
            int unitCount = 0;
            while (unitCount < numberOfUnits)
            {
                subTotal += UnitPrice;
                unitCount++;
            }
            return subTotal;
        }
    }

    public class Checkout
    {
        private readonly Dictionary<char, ItemPrice> _itemPrices;
        private readonly List<ItemPrice> _items = new List<ItemPrice>();

        public Checkout(params ItemPrice[] itemPrices)
        {
            _itemPrices = itemPrices.ToDictionary(x => x.Item);
        }

        public double GetTotal()
        {
            return _items.Sum(x => x.UnitPrice);
        }

        private void Scan(ItemPrice itemPrice)
        {
            _items.Add(itemPrice);
        }

        public void Scan(char item)
        {
            Scan(FindItemPrice(item));
        }

        private ItemPrice FindItemPrice(char item)
        {
            return _itemPrices[item];
        }
    }
}
