﻿using System;
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
    }
}