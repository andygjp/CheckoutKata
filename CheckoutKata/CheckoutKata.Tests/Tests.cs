namespace CheckoutKata.Tests
{
    using System.Collections.Generic;
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
        public Item(int price)
        {
            Price = price;
        }

        public int Price { get; }
    }

    public class Checkout
    {
        private int _total;
        private readonly Dictionary<char, Item> _items = new Dictionary<char, Item> {['A'] = new Item(50), ['x'] = new Item(30)};

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
            return _items['x'].Price;
        }

        private int GetAUnitPrice()
        {
            return _items['A'].Price;
        }
    }

    
}
