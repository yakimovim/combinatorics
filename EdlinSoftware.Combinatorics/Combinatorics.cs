using System;
using System.Collections.Generic;
using System.Linq;

namespace EdlinSoftware.Combinatorics
{
    public static class Combinatorics
    {
        /// <summary>
        /// Returns all possible samples of length <paramref name="sampleLength"/> from <paramref name="sequence"/> with replacement.
        /// Order of items is important. It means that result can contain samples with the same items but in different order.
        /// </summary>
        /// <typeparam name="T">Type of items.</typeparam>
        /// <param name="sequence">Sequence of all items.</param>
        /// <param name="sampleLength">Length of samples.</param>
        /// <example>
        /// For the sequence { 1, 2 } and length 2 result will be
        /// {
        ///    { 1, 1 },
        ///    { 1, 2 },
        ///    { 2, 1 },
        ///    { 2, 2 }
        /// }
        /// </example>
        public static IEnumerable<IEnumerable<T>> GetOrderedSamplesWithReplacement<T>(this IReadOnlyList<T> sequence,
            uint sampleLength)
        {
            if((sequence?.Count ?? 0) == 0)
                throw new ArgumentException("Sequence should not be null or empty.", nameof(sequence));

            return GetOrderedSamplesWithReplacementInternal(sequence, sampleLength);
        }

        /// <summary>
        /// Returns all possible samples of length <paramref name="sampleLength"/> from <paramref name="sequence"/> with replacement.
        /// Order of items is important. It means that result can contain samples with the same items but in different order.
        /// This method does not throw StackOverflowException if <paramref name="sampleLength"/> is high but works slower.
        /// </summary>
        /// <typeparam name="T">Type of items.</typeparam>
        /// <param name="sequence">Sequence of all items.</param>
        /// <param name="sampleLength">Length of samples.</param>
        /// <example>
        /// For the sequence { 1, 2 } and length 2 result will be
        /// {
        ///    { 1, 1 },
        ///    { 1, 2 },
        ///    { 2, 1 },
        ///    { 2, 2 }
        /// }
        /// </example>
        public static IEnumerable<IEnumerable<T>> GetOrderedSamplesWithReplacementStackSafe<T>(this IReadOnlyList<T> sequence,
            uint sampleLength)
        {
            if ((sequence?.Count ?? 0) == 0)
                throw new ArgumentException("Sequence should not be null or empty.", nameof(sequence));

            return GetOrderedSamplesWithReplacementInternalStackSafe(sequence, sampleLength);
        }

        private static IEnumerable<IEnumerable<T>> GetOrderedSamplesWithReplacementInternal<T>(
            IReadOnlyList<T> sequence,
            uint sampleLength)
        {
            if (sampleLength == 0)
            {
                yield return new T[0];
                yield break;
            }

            foreach (var smallerSequence in GetOrderedSamplesWithReplacementInternal(sequence, sampleLength - 1))
            {
                var smallerArray = smallerSequence.ToArray();

                foreach (var item in sequence)
                {
                    yield return smallerArray.Concat(new[] {item}).ToArray();
                }
            }
        }

        private static IEnumerable<IEnumerable<T>> GetOrderedSamplesWithReplacementInternalStackSafe<T>(
            IReadOnlyList<T> sequence,
            uint sampleLength)
        {
            if (sampleLength == 0)
            {
                yield return new T[0];
                yield break;
            }

            foreach (var number in new NaryNumbersGenerator((uint)sequence.Count, sampleLength).GetAllNumbers())
            {
                yield return number.Select(d => sequence[(int)d]).ToArray();
            }
        }

        /// <summary>
        /// Returns all possible samples of length <paramref name="sampleLength"/> from <paramref name="sequence"/> with replacement.
        /// Order of items is not important. It means that result does not contain samples with the same items but in different order.
        /// </summary>
        /// <typeparam name="T">Type of items.</typeparam>
        /// <param name="sequence">Sequence of all items.</param>
        /// <param name="sampleLength">Length of samples.</param>
        /// <example>
        /// For the sequence { 1, 2 } and length 2 result will be
        /// {
        ///    { 1, 1 },
        ///    { 1, 2 },
        ///    { 2, 2 }
        /// }
        /// </example>
        public static IEnumerable<IEnumerable<T>> GetUnorderedSamplesWithReplacement<T>(this IReadOnlyList<T> sequence,
            uint sampleLength)
        {
            if ((sequence?.Count ?? 0) == 0)
                throw new ArgumentException("Sequence should not be null or empty.", nameof(sequence));

            return GetUnorderedSamplesWithReplacementInternal(sequence, sampleLength);
        }

        private static IEnumerable<IEnumerable<T>> GetUnorderedSamplesWithReplacementInternal<T>(
            IReadOnlyList<T> sequence,
            uint sampleLength)
        {
            if (sampleLength == 0)
            {
                yield return new T[0];
                yield break;
            }

            foreach (var number in new NaryNumbersGenerator((uint)sequence.Count, sampleLength).GetAllNumbers().Where(IsSorted))
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

        /// <summary>
        /// Returns all possible samples of length <paramref name="sampleLength"/> from <paramref name="sequence"/> without replacement.
        /// Order of items is important. It means that result can contain samples with the same items but in different order.
        /// </summary>
        /// <typeparam name="T">Type of items.</typeparam>
        /// <param name="sequence">Sequence of all items.</param>
        /// <param name="sampleLength">Length of samples.</param>
        /// <example>
        /// For the sequence { 1, 2 } and length 2 result will be
        /// {
        ///    { 1, 2 },
        ///    { 2, 1 }
        /// }
        /// </example>
        public static IEnumerable<IEnumerable<T>> GetOrderedSamplesWithoutReplacement<T>(this IReadOnlyList<T> sequence,
            uint sampleLength)
        {
            if ((sequence?.Count ?? 0) == 0)
                throw new ArgumentException("Sequence should not be null or empty.", nameof(sequence));
            if(sampleLength > sequence?.Count)
                throw new ArgumentOutOfRangeException(nameof(sampleLength), "Length of permutations can't be greater than length of sequence");

            return GetOrderedSamplesWithoutReplacementInternal(sequence, sampleLength);
        }

        private static IEnumerable<ISet<T>> GetOrderedSamplesWithoutReplacementInternal<T>(IReadOnlyList<T> sequence, 
            uint sampleLength)
        {
            if (sampleLength == 0)
            {
                yield return new HashSet<T>();
                yield break;
            }

            foreach (var smallerSequence in GetOrderedSamplesWithoutReplacementInternal(sequence, sampleLength - 1))
            {
                foreach (var item in sequence.Where(i => !smallerSequence.Contains(i)))
                {
                    yield return new HashSet<T>(smallerSequence) { item };
                }
            }
        }

        /// <summary>
        /// Returns all possible samples of length <paramref name="sampleLength"/> from <paramref name="sequence"/> without replacement.
        /// Order of items is important. It means that result can contain samples with the same items but in different order.
        /// This method does not throw StackOverflowException if <paramref name="sampleLength"/> is high but works slower.
        /// </summary>
        /// <typeparam name="T">Type of items.</typeparam>
        /// <param name="sequence">Sequence of all items.</param>
        /// <param name="sampleLength">Length of samples.</param>
        /// <example>
        /// For the sequence { 1, 2 } and length 2 result will be
        /// {
        ///    { 1, 2 },
        ///    { 2, 1 }
        /// }
        /// </example>
        public static IEnumerable<IEnumerable<T>> GetOrderedSamplesWithoutReplacementStackSafe<T>(this IReadOnlyList<T> sequence,
            uint sampleLength)
        {
            if ((sequence?.Count ?? 0) == 0)
                throw new ArgumentException("Sequence should not be null or empty.", nameof(sequence));
            if (sampleLength > sequence?.Count)
                throw new ArgumentOutOfRangeException(nameof(sampleLength), "Length of permutations can't be greater than length of sequence");

            return GetOrderedSamplesWithoutReplacementInternalStackSafe(sequence, sampleLength);
        }

        private static IEnumerable<IEnumerable<T>> GetOrderedSamplesWithoutReplacementInternalStackSafe<T>(IReadOnlyList<T> sequence, 
            uint sampleLength)
        {
            if (sampleLength == 0)
            {
                yield return new HashSet<T>();
                yield break;
            }

            foreach (var number in new NaryNumbersGenerator((uint)sequence.Count, sampleLength)
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

        /// <summary>
        /// Returns all possible samples of length <paramref name="sampleLength"/> from <paramref name="sequence"/> without replacement.
        /// Order of items is not important. It means that result does not contain samples with the same items but in different order.
        /// </summary>
        /// <typeparam name="T">Type of items.</typeparam>
        /// <param name="sequence">Sequence of all items.</param>
        /// <param name="sampleLength">Length of samples.</param>
        /// <example>
        /// For the sequence { 1, 2 } and length 2 result will be
        /// {
        ///    { 1, 2 }
        /// }
        /// </example>
        public static IEnumerable<IEnumerable<T>> GetUnorderedSamplesWithoutReplacement<T>(this IReadOnlyList<T> sequence,
            uint sampleLength)
        {
            if ((sequence?.Count ?? 0) == 0)
                throw new ArgumentException("Sequence should not be null or empty.", nameof(sequence));
            if (sampleLength > sequence?.Count)
                throw new ArgumentOutOfRangeException(nameof(sampleLength), "Length of permutations can't be greater than length of sequence");

            return GetUnorderedSamplesWithoutReplacementInternal(sequence, sampleLength);
        }

        private static IEnumerable<IEnumerable<T>> GetUnorderedSamplesWithoutReplacementInternal<T>(IReadOnlyList<T> sequence, uint length)
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