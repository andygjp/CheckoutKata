namespace CheckoutKata
{
    using System.Collections.Generic;
    using System.Linq;

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