using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EdlinSoftware.Combinatorics.Tests
{
    public class Combinatorics_GenerateOrderedDifferentPermutationsOfLength_Tests
    {
        [Theory]
        [MemberData("NoSequences")]
        public void GenerateOrderedDifferentPermutationsOfLength_ShouldThrowException_IfSequenceIsNullOrEmpty(
            IReadOnlyList<int> sequence,
            Func<IReadOnlyList<int>, uint, IEnumerable<IEnumerable<int>>> generator)
        {
            Assert.Throws<ArgumentException>("sequence", () => generator(sequence, 1));
        }

        [Theory]
        [MemberData("Generators")]
        public void GenerateOrderedDifferentPermutationsOfLength_ShouldThrowException_IfLenghtIsGreaterThanSequence(
            Func<IReadOnlyList<int>, uint, IEnumerable<IEnumerable<int>>> generator)
        {
            Assert.Throws<ArgumentOutOfRangeException>("length", () => generator(new[] { 1, 2 }, 3));
        }

        [Theory]
        [MemberData("Generators")]
        public void GenerateOrderedDifferentPermutationsOfLength_IfLenghtIsZero(
            Func<IReadOnlyList<int>, uint, IEnumerable<IEnumerable<int>>> generator)
        {
            var ver = new SequencesVerifier<int>
            {
                new int[0]
            };

            var sequence = generator(new[] { 1, 2 }, 0);

            ver.Verify(sequence);
        }

        [Theory]
        [MemberData("Generators")]
        public void GenerateOrderedDifferentPermutationsOfLength_IfLenghtIsOne(
            Func<IReadOnlyList<int>, uint, IEnumerable<IEnumerable<int>>> generator)
        {
            var ver = new SequencesVerifier<int>
            {
                1,
                2
            };

            var sequence = generator(new[] { 1, 2 }, 1);

            ver.Verify(sequence);
        }

        [Theory]
        [MemberData("Generators")]
        public void GenerateOrderedDifferentPermutationsOfLength_IfLenghtIsTwo(
            Func<IReadOnlyList<int>, uint, IEnumerable<IEnumerable<int>>> generator)
        {
            var ver = new SequencesVerifier<int>
            {
                { 1, 2 }
            };

            var sequence = generator(new[] { 1, 2 }, 2);

            ver.Verify(sequence);
        }

        [Theory]
        [MemberData("Generators")]
        public void GenerateOrderedDifferentPermutationsOfLength_IfLenghtIsTwo_AndSequenceHasLengthThree(
            Func<IReadOnlyList<int>, uint, IEnumerable<IEnumerable<int>>> generator)
        {
            var ver = new SequencesVerifier<int>
            {
                { 1, 2 },
                { 1, 3 },
                { 2, 3 }
            };

            var sequence = generator(new[] { 1, 2, 3 }, 2);

            ver.Verify(sequence);
        }

        private static IEnumerable<Func<IReadOnlyList<int>, uint, IEnumerable<IEnumerable<int>>>> GetGenerators()
        {
            Func<IReadOnlyList<int>, uint, IEnumerable<IEnumerable<int>>> generator =
                (sequence, length) => sequence.GenerateOrderedDifferentPermutationsOfLength(length);

            yield return generator;
        }

        public static IEnumerable<object[]> NoSequences
        {
            get
            {
                foreach (var generator in GetGenerators())
                {
                    yield return new object[] { null, generator };
                    yield return new object[] { new int[0], generator };
                }
            }
        }

        public static IEnumerable<object[]> Generators
        {
            get
            {
                return GetGenerators().Select(generator => new object[] { generator });
            }
        }
    }
}