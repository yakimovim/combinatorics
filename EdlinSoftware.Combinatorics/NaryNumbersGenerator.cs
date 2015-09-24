using System;
using System.Collections.Generic;
using System.Linq;

namespace EdlinSoftware.Combinatorics
{
    public class NaryNumbersGenerator
    {
        private readonly uint _base;
        private readonly uint[] _currentNumber;

        public NaryNumbersGenerator(uint @base, uint length)
        {
            if (@base < 2)
                throw new ArgumentOutOfRangeException(nameof(@base), "Base should be greater that 1.");
            _base = @base;
            _currentNumber = new uint[length];
        }

        public IEnumerable<uint[]> GetAllNumbers()
        {
            while (true)
            {
                yield return _currentNumber.ToArray();

                AddOne();

                if (ZeroAgain())
                    yield break;
            }
        }

        private void AddOne()
        {
            var index = 0;
            while (IncrementAtDigit(index))
            { index++; }
        }

        private bool IncrementAtDigit(int index)
        {
            if (index < 0 || index >= _currentNumber.Length)
                return false;

            _currentNumber[index]++;

            if (MoreThanBase(index))
            {
                _currentNumber[index] = 0;
                return true;
            }
            return false;
        }

        private bool MoreThanBase(int index)
        {
            return _currentNumber[index] >= _base;
        }

        private bool ZeroAgain()
        {
            return _currentNumber.All(i => i == 0);

        }
    }
}