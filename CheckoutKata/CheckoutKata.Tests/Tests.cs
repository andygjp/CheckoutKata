﻿namespace CheckoutKata.Tests
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

    public class When_single_item_is_scanned
    {
        [Fact]
        public void It_should_return_expected_value()
        {
            var sut = new Checkout();
            sut.Scan(new Item('A', 50.0));
            double actual = sut.GetTotal();
            actual.Should().Be(0);
        }
    }

    public class Item
    {
        public Item(char name, double price)
        {
            Name = name;
            Price = price;
        }

        public char Name { get; }
        public double Price { get; }
    }

    public class Checkout
    {
        public double GetTotal()
        {
            return 0;
        }

        public void Scan(Item item)
        {
            throw new System.NotImplementedException();
        }
    }
}
