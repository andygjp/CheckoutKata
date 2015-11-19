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

    public class When_I_scan_in_different_items
    {
        [Theory]
        [InlineData('C', 20)]
        [InlineData('D', 15)]
        public void It_should_return_expectation(char item, int expected)
        {
            var sut = new Checkout();
            sut.Scan(item);
            double actual = sut.GetTotal();
            actual.Should().Be(expected);
        }
    }

    public class When_I_scan_in_two_different_items
    {
        [Fact]
        public void It_should_sum_the_items()
        {
            var sut = new Checkout();
            sut.Scan('A');
            sut.Scan('B');
            double actual = sut.GetTotal();
            actual.Should().Be(80);
        }
    }

    public class When_I_scan_in_one_of_each_item
    {
        [Fact]
        public void It_should_sum_the_items()
        {
            var sut = new Checkout();
            sut.Scan('C');
            sut.Scan('D');
            sut.Scan('A');
            sut.Scan('B');
            double actual = sut.GetTotal();
            actual.Should().Be(115);
        }
    }

    public class When_I_scan_in_item_A_multiple_time
    {
        [Theory]
        [InlineData(2, 100)]
        [InlineData(3, 130)]
        public void It_should_return_expectation(int numberOfScans, int expected)
        {
            var sut = new Checkout();
            for (int i = 0; i < numberOfScans; i++)
            {
                sut.Scan('A');
            }
            double actual = sut.GetTotal();
            actual.Should().Be(expected);
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
        private readonly IDictionary<char, Item> _items;

        public Checkout()
        {
            _items = new Dictionary<char, Item>
            {
                ['A'] = new Item(50),
                ['B'] = new Item(30),
                ['C'] = new Item(20),
                ['D'] = new Item(15)
            };
        }

        public Checkout(IDictionary<char, Item> items)
        {
            _items = items;
        }

        public double GetTotal()
        {
            return _total;
        }

        public void Scan(char item)
        {
            _total += GetPrice(item);
        }

        private int GetPrice(char item)
        {
            return _items[item].Price;
        }
    }

    
}
