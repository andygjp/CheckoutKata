namespace CheckoutKata.Tests
{
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
            var sut = new Checkout();
            sut.Scan(new ItemPrice('A', 50.0));
            double actual = sut.GetTotal();
            actual.Should().Be(50);
        }
    }

    public class ItemPrice
    {
        public ItemPrice(char item, double unitPrice)
        {
            Item = item;
            UnitPrice = unitPrice;
        }

        public char Item { get; }
        public double UnitPrice { get; }
    }

    public class Checkout
    {
        private readonly List<ItemPrice> _items = new List<ItemPrice>();

        public double GetTotal()
        {
            return _items.Sum(x => x.UnitPrice);
        }

        public void Scan(ItemPrice itemPrice)
        {
            _items.Add(itemPrice);
        }
    }
}
