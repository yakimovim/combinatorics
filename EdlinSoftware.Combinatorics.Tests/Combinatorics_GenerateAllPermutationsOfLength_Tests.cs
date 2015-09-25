using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EdlinSoftware.Combinatorics.Tests
{
    public class Combinatorics_GenerateAllPermutationsOfLength_Tests
    {
        [Theory]
        [MemberData("NoSequences")]
        public void GenerateAllPermutationsOfLength_ShouldThrowException_IfSequenceIsNullOrEmpty(
            IReadOnlyList<int> sequence,
            Func<IReadOnlyList<int>, uint, IEnumerable<IEnumerable<int>>> generator)
        {
            Assert.Throws<ArgumentException>("sequence", () => generator(sequence, 1));
        }

        [Theory]
        [MemberData("Generators")]
        public void GenerateAllPermutationsOfLength_ShouldThrowException_IfLenghtIsGreaterThanSequence(
            Func<IReadOnlyList<int>, uint, IEnumerable<IEnumerable<int>>> generator)
        {
            Assert.Throws<ArgumentOutOfRangeException>("length", () => generator(new [] { 1, 2 }, 3));
        }

        [Theory]
        [MemberData("Generators")]
        public void GenerateAllPermutationsOfLength_IfLenghtIsZero(
            Func<IReadOnlyList<int>, uint, IEnumerable<IEnumerable<int>>> generator)
        {
            var ver = new SequencesVerifier<int>
            {
                new int[0]
            };

            var sequence = generator(new[] {1, 2}, 0);

            ver.Verify(sequence);
        }

        [Theory]
        [MemberData("Generators")]
        public void GenerateAllPermutationsOfLength_IfLenghtIsOne(
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
        public void GenerateAllPermutationsOfLength_IfLenghtIsTwo(
            Func<IReadOnlyList<int>, uint, IEnumerable<IEnumerable<int>>> generator)
        {
            var ver = new SequencesVerifier<int>
            {
                { 1, 2 },
                { 2, 1 }
            };

            var sequence = generator(new[] { 1, 2 }, 2);

            ver.Verify(sequence);
        }

        private static IEnumerable<Func<IReadOnlyList<int>, uint, IEnumerable<IEnumerable<int>>>> GetGenerators()
        {
            Func<IReadOnlyList<int>, uint, IEnumerable<IEnumerable<int>>> generator =
                (sequence, length) => sequence.GenerateAllPermutationsOfLength(length);
            Func<IReadOnlyList<int>, uint, IEnumerable<IEnumerable<int>>> safeGenerator =
                (sequence, length) => sequence.GenerateAllPermutationsOfLengthStackSafe(length);

            yield return generator;
            yield return safeGenerator;
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