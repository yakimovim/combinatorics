using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EdlinSoftware.Combinatorics.Tests
{
    public class CombinatoricsTests
    {
        [Theory]
        [MemberData("NoSequences")]
        public void GenerateAllSequencesOfLength_ShouldThrowException_IfSequenceIsNullOrEmpty(IReadOnlyList<int> sequence)
        {
            Assert.Throws<ArgumentException>("sequence", () => sequence.GenerateAllSequencesOfLength(1));
        }

        [Fact]
        public void GenerateAllSequencesOfLength_ShouldReturnOneEmptyEnumeration_IfLengthIsZero()
        {
            Assert.Equal(new [] { new int[0] }, new [] { 1 }.GenerateAllSequencesOfLength(0) );
        }

        [Fact]
        public void GenerateAllSequencesOfLength_ShouldReturnEnumerationOfSingleElements_IfLengthIsOne()
        {
            var sequences = Enumerable.Range(1, 5).ToArray().GenerateAllSequencesOfLength(1);

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

        [Fact]
        public void GenerateAllSequencesOfLength_IfLengthIsTwo()
        {
            var sequences = Enumerable.Range(1, 2).ToArray().GenerateAllSequencesOfLength(2);

            var ver = new SequencesVerifier<int>
            {
                { 1, 1 },
                { 1, 2 },
                { 2, 1 },
                { 2, 2 }
            };
            ver.Verify(sequences);
        }

        public static IEnumerable<object[]> NoSequences
        {
            get
            {
                yield return new object[] { null };
                yield return new object[] { new int[0] };
            }
        }
    }
}