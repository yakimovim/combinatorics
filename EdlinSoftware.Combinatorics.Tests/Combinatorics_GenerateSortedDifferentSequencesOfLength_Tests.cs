using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EdlinSoftware.Combinatorics.Tests
{
    public class Combinatorics_GenerateSortedDifferentSequencesOfLength_Tests
    {
        [Theory]
        [MemberData("NoSequences")]
        public void GenerateAllSequencesOfLength_ShouldThrowException_IfSequenceIsNullOrEmpty(
            IReadOnlyList<int> sequence,
            Func<IReadOnlyList<int>, uint, IEnumerable<IEnumerable<int>>> generator)
        {
            Assert.Throws<ArgumentException>("sequence", () => generator(sequence, 1));
        }

        [Theory]
        [MemberData("Generators")]
        public void GenerateAllSequencesOfLength_ShouldReturnOneEmptyEnumeration_IfLengthIsZero(
            Func<IReadOnlyList<int>, uint, IEnumerable<IEnumerable<int>>> generator)
        {
            Assert.Equal(new[] { new int[0] }, generator(new[] { 1 }, 0));
        }

        [Theory]
        [MemberData("Generators")]
        public void GenerateAllSequencesOfLength_ShouldReturnEnumerationOfSingleElements_IfLengthIsOne(
            Func<IReadOnlyList<int>, uint, IEnumerable<IEnumerable<int>>> generator)
        {
            var sequences = generator(Enumerable.Range(1, 5).ToArray(), 1);

            var ver = new SequencesVerifier<int>
            {
                { 1 },
                { 2 },
                { 3 },
                { 4 },
                { 5 }
            };
            ver.Verify(sequences);
        }

        [Theory]
        [MemberData("Generators")]
        public void GenerateAllSequencesOfLength_IfLengthIsTwo(
            Func<IReadOnlyList<int>, uint, IEnumerable<IEnumerable<int>>> generator)
        {
            var sequences = generator(Enumerable.Range(1, 2).ToArray(), 2);

            var ver = new SequencesVerifier<int>
            {
                { 1, 1 },
                { 1, 2 },
                { 2, 2 }
            };
            ver.Verify(sequences);
        }

        [Theory]
        [MemberData("Generators")]
        public void GenerateAllSequencesOfLength_IfLengthIsThree(
            Func<IReadOnlyList<int>, uint, IEnumerable<IEnumerable<int>>> generator)
        {
            var sequences = generator(Enumerable.Range(1, 2).ToArray(), 3);

            var ver = new SequencesVerifier<int>
            {
                { 1, 1, 1 },
                { 1, 1, 2 },
                { 1, 2, 2 },
                { 2, 2, 2 },
            };
            ver.Verify(sequences);
        }

        private static IEnumerable<Func<IReadOnlyList<int>, uint, IEnumerable<IEnumerable<int>>>> GetGenerators()
        {
            Func<IReadOnlyList<int>, uint, IEnumerable<IEnumerable<int>>> generator =
                (sequence, length) => sequence.GenerateSortedDifferentSequencesOfLength(length);

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
                foreach (var generator in GetGenerators())
                {
                    yield return new object[] { generator };
                }
            }
        }
    }
}
