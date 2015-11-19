﻿namespace CheckoutKata.Tests
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
        public Checkout Sut =>
                new Checkout(new ItemPrice('A', 50.0, new GetXForY(3, 130.0)), new ItemPrice('B', 30, new GetXForY(2, 45.0)), new ItemPrice('C', 20.0), new ItemPrice('D', 15.0));
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

            int x = numberOfUnits/_x;
            numberOfUnits = numberOfUnits%_x;
            return _y*x;
        }
    }

    public interface IPromotion
    {
        double GetPromotionalPrice(ref int numberOfUnits);
    }

    public class NormalPrice : IPromotion
    {
        public double GetPromotionalPrice(ref int numberOfUnits)
        {
            return 0;
        }
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

    public class ItemPrice : SubTotalCalculator
    {
        private readonly IPromotion _promotion = new NormalPrice();

        public ItemPrice(char item, double unitPrice) : base(item, unitPrice)
        {
        }

        public ItemPrice(char item, double unitPrice, IPromotion promotion) : this(item, unitPrice)
        {
            _promotion = promotion;
        }

        public override double SubTotal(int numberOfUnits)
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
        private readonly Dictionary<char, SubTotalCalculator> _itemPrices;
        private readonly List<SubTotalCalculator> _items = new List<SubTotalCalculator>();
        private Dictionary<char, UnitTally> _unitTallies;

        public Checkout(params SubTotalCalculator[] itemPrices)
        {
            _itemPrices = itemPrices.ToDictionary(x => x.Item);
            _unitTallies = itemPrices.ToDictionary(x => x.Item, x => new UnitTally(x));
        }

        public double GetTotal()
        {
            return _items.GroupBy(x => x.Item).Select(x => _itemPrices[x.Key].SubTotal(x.Count())).Sum();
        }

        private void Scan(SubTotalCalculator itemPrice)
        {
            _items.Add(itemPrice);
        }

        public void Scan(char item)
        {
            Scan(FindItemPrice(item));
        }

        private SubTotalCalculator FindItemPrice(char item)
        {
            return _itemPrices[item];
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
