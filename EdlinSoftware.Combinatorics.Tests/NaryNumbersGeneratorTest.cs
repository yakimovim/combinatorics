using System;
using Xunit;

namespace EdlinSoftware.Combinatorics.Tests
{
    public class NaryNumbersGeneratorTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void SmallBase_ShouldThrowException(uint @base)
        {
            Assert.Throws<ArgumentOutOfRangeException>("base", () => new NaryNumbersGenerator(@base, 1));
        }

        [Fact]
        public void GetAllNumbers_ShouldReturnEmptyArray_IfLengthIsZero()
        {
            var generator = new NaryNumbersGenerator(2, 0);

            var numbers = generator.GetAllNumbers();

            var ver = new SequencesVerifier<uint>();
            ver.Add();
            ver.Verify(numbers);
        }

        [Fact]
        public void GetAllNumbers_ShouldReturnCorrectResult_IfLengthIsOne()
        {
            var generator = new NaryNumbersGenerator(2, 1);

            var numbers = generator.GetAllNumbers();

            var ver = new SequencesVerifier<uint>
            {
                { 0 },
                { 1 }
            };
            ver.Verify(numbers);
        }

        [Fact]
        public void GetAllNumbers_ShouldReturnCorrectResult_IfLengthIsTwo()
        {
            var generator = new NaryNumbersGenerator(2, 2);

            var numbers = generator.GetAllNumbers();

            var ver = new SequencesVerifier<uint>
            {
                { 0, 0 },
                { 0, 1 },
                { 1, 0 },
                { 1, 1 }
            };
            ver.Verify(numbers);
        }

        [Fact]
        public void GetAllNumbers_ShouldReturnCorrectResult_IfLengthIsTwo_AndBaseIsThree()
        {
            var generator = new NaryNumbersGenerator(3, 2);

            var numbers = generator.GetAllNumbers();

            var ver = new SequencesVerifier<uint>
            {
                { 0, 0 },
                { 0, 1 },
                { 0, 2 },
                { 1, 0 },
                { 1, 1 },
                { 1, 2 },
                { 2, 0 },
                { 2, 1 },
                { 2, 2 }
            };
            ver.Verify(numbers);
        }
    }
}