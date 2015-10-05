using System;
using System.Collections.Generic;
using System.Linq;

namespace EdlinSoftware.Combinatorics
{
    public static class Combinatorics
    {
        public static IEnumerable<IEnumerable<T>> GenerateAllSequencesOfLength<T>(this IReadOnlyList<T> sequence,
            uint length)
        {
            if((sequence?.Count ?? 0) == 0)
                throw new ArgumentException("Sequence should not be null or empty.", nameof(sequence));

            return GenerateAllSequencesOfLengthInternal(sequence, length);
        }

        public static IEnumerable<IEnumerable<T>> GenerateAllSequencesOfLengthStackSafe<T>(this IReadOnlyList<T> sequence,
            uint length)
        {
            if ((sequence?.Count ?? 0) == 0)
                throw new ArgumentException("Sequence should not be null or empty.", nameof(sequence));

            return GenerateAllSequencesOfLengthInternalStackSafe(sequence, length);
        }

        private static IEnumerable<IEnumerable<T>> GenerateAllSequencesOfLengthInternal<T>(
            IReadOnlyList<T> sequence,
            uint length)
        {
            if (length == 0)
            {
                yield return new T[0];
                yield break;
            }

            foreach (var smallerSequence in GenerateAllSequencesOfLengthInternal(sequence, length - 1))
            {
                var smallerArray = smallerSequence.ToArray();

                foreach (var item in sequence)
                {
                    yield return smallerArray.Concat(new[] {item}).ToArray();
                }
            }
        }

        private static IEnumerable<IEnumerable<T>> GenerateAllSequencesOfLengthInternalStackSafe<T>(
            IReadOnlyList<T> sequence,
            uint length)
        {
            if (length == 0)
            {
                yield return new T[0];
                yield break;
            }

            foreach (var number in new NaryNumbersGenerator((uint)sequence.Count, length).GetAllNumbers())
            {
                yield return number.Select(d => sequence[(int)d]).ToArray();
            }
        }

        public static IEnumerable<IEnumerable<T>> GenerateOrderedDifferentSequencesOfLength<T>(this IReadOnlyList<T> sequence,
            uint length)
        {
            if ((sequence?.Count ?? 0) == 0)
                throw new ArgumentException("Sequence should not be null or empty.", nameof(sequence));

            return GenerateOrderedDifferentSequencesOfLengthInternal(sequence, length);
        }

        private static IEnumerable<IEnumerable<T>> GenerateOrderedDifferentSequencesOfLengthInternal<T>(
            IReadOnlyList<T> sequence,
            uint length)
        {
            if (length == 0)
            {
                yield return new T[0];
                yield break;
            }

            foreach (var number in new NaryNumbersGenerator((uint)sequence.Count, length).GetAllNumbers().Where(IsSorted))
            {
                yield return number.Select(d => sequence[(int)d]).ToArray();
            }
        }

        private static bool IsSorted(uint[] number)
        {
            for(var index = 0; index < number.Length - 1; index++)
            {
                if (number[index] > number[index + 1])
                    return false;
            }

            return true;
        }

        public static IEnumerable<IEnumerable<T>> GenerateAllPermutationsOfLength<T>(this IReadOnlyList<T> sequence,
            uint length)
        {
            if ((sequence?.Count ?? 0) == 0)
                throw new ArgumentException("Sequence should not be null or empty.", nameof(sequence));
            if(length > sequence?.Count)
                throw new ArgumentOutOfRangeException(nameof(length), "Length of permutations can't be greater than length of sequence");

            return GenerateAllPermutationsOfLengthInternal(sequence, length);
        }

        private static IEnumerable<ISet<T>> GenerateAllPermutationsOfLengthInternal<T>(IReadOnlyList<T> sequence, uint length)
        {
            if (length == 0)
            {
                yield return new HashSet<T>();
                yield break;
            }

            foreach (var smallerSequence in GenerateAllPermutationsOfLengthInternal(sequence, length - 1))
            {
                foreach (var item in sequence.Where(i => !smallerSequence.Contains(i)))
                {
                    yield return new HashSet<T>(smallerSequence) { item };
                }
            }
        }

        public static IEnumerable<IEnumerable<T>> GenerateAllPermutationsOfLengthStackSafe<T>(this IReadOnlyList<T> sequence,
            uint length)
        {
            if ((sequence?.Count ?? 0) == 0)
                throw new ArgumentException("Sequence should not be null or empty.", nameof(sequence));
            if (length > sequence?.Count)
                throw new ArgumentOutOfRangeException(nameof(length), "Length of permutations can't be greater than length of sequence");

            return GenerateAllPermutationsOfLengthInternalStackSafe(sequence, length);
        }

        private static IEnumerable<IEnumerable<T>> GenerateAllPermutationsOfLengthInternalStackSafe<T>(IReadOnlyList<T> sequence, uint length)
        {
            if (length == 0)
            {
                yield return new HashSet<T>();
                yield break;
            }

            foreach (var number in new NaryNumbersGenerator((uint)sequence.Count, length)
                .GetAllNumbers()
                .Where(AllDigitsAreDistinct))
            {
                yield return number.Select(d => sequence[(int)d]).ToArray();
            }
        }

        private static bool AllDigitsAreDistinct(uint[] number)
        {
            return new HashSet<uint>(number).Count == number.Length;
        }

        public static IEnumerable<IEnumerable<T>> GenerateOrderedDifferentPermutationsOfLength<T>(this IReadOnlyList<T> sequence,
            uint length)
        {
            if ((sequence?.Count ?? 0) == 0)
                throw new ArgumentException("Sequence should not be null or empty.", nameof(sequence));
            if (length > sequence?.Count)
                throw new ArgumentOutOfRangeException(nameof(length), "Length of permutations can't be greater than length of sequence");

            return GenerateOrderedDifferentPermutationsOfLengthInternal(sequence, length);
        }

        private static IEnumerable<IEnumerable<T>> GenerateOrderedDifferentPermutationsOfLengthInternal<T>(IReadOnlyList<T> sequence, uint length)
        {
            if (length == 0)
            {
                yield return new HashSet<T>();
                yield break;
            }

            foreach (var number in new NaryNumbersGenerator((uint)sequence.Count, length)
                .GetAllNumbers()
                .Where(AllDigitsAreDistinct)
                .Where(IsSorted))
            {
                yield return number.Select(d => sequence[(int)d]).ToArray();
            }
        }
    }
}