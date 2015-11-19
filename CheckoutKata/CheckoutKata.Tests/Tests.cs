namespace CheckoutKata.Tests
{
    using System.Globalization;
    using FluentAssertions;
    using Xunit;
    using Xunit.Abstractions;

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
        [InlineData(4, 180)]
        [InlineData(5, 230)]
        [InlineData(6, 260)]
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

    public class When_I_scan_in_multiple_varying_items
    {
        [Theory]
        [InlineData(new[] { 'A', 'A', 'A', 'B' }, 160)]
        [InlineData(new[] { 'A', 'A', 'A', 'B', 'B' }, 175)]
        [InlineData(new[] { 'A', 'A', 'A', 'B', 'B', 'D' }, 190)]
        [InlineData(new[] { 'D', 'A', 'B', 'A', 'B', 'A' }, 190)]
        public void It_should_return_expectation(char[] items, int expected)
        {
            var sut = new Checkout();
            foreach (var item in items)
            {
                sut.Scan(item);
            }
            double actual = sut.GetTotal();
            actual.Should().Be(expected);
        }
    }
    
    public class When_I_scan_incrementally
    {
        private readonly Checkout _sut = new Checkout();
        private readonly ITestOutputHelper _output;

        public When_I_scan_incrementally(ITestOutputHelper output)
        {
            _output = output;
        }

        // Using theory doesn't allow me to control order the individual tests are run,
        // leaving me with having to do several asserts in a test.
        [Fact]
        public void It_should_totup_correctly()
        {
            _sut.Scan('A');
            OutputAndAssert(50);

            _sut.Scan('B');
            OutputAndAssert(80);

            _sut.Scan('A');
            OutputAndAssert(130);

            _sut.Scan('A');
            OutputAndAssert(160);

            _sut.Scan('B');
            OutputAndAssert(175);
        }

        private void OutputAndAssert(int expected)
        {
            var actual = _sut.GetTotal();
            _output.WriteLine(actual.ToString(CultureInfo.InvariantCulture));
            actual.Should().Be(expected);
        }
    }
}
