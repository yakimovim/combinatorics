using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EdlinSoftware.Combinatorics.Tests
{
    public class SequencesVerifier<T> : IEnumerable<T[]>
    {
        private readonly LinkedList<T[]> _sequences = new LinkedList<T[]>();

        public void Add(params T[] items)
        {
            _sequences.AddLast(items);
        }

        public IEnumerator<T[]> GetEnumerator()
        {
            return _sequences.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Verify(IEnumerable<IEnumerable<T>> sequences)
        {
            var count = 0;

            foreach (var item in sequences)
            {
                count++;
                var itemArray = item.ToArray();
                Assert.True(IsInSequences(itemArray), $"Sequence [{string.Join(",", itemArray.Select(i => i.ToString()))}] is not expected");
            }

            Assert.Equal(_sequences.Count, count);
        }

        private bool IsInSequences(T[] item)
        {
            return _sequences.Any(i => AreEqual(i, item));
        }

        private bool AreEqual(T[] storedSequence, T[] sequence)
        {
            if (storedSequence.Length != sequence.Length)
                return false;

            for (int i = 0; i < storedSequence.Length; i++)
            {
                var storedItem = storedSequence[i];
                var item = sequence[i];

                if(ReferenceEquals(storedItem, item))
                    continue;

                if (storedItem != null && storedItem.Equals(item))
                    continue;
                if (item != null && item.Equals(storedItem))
                    continue;

                return false;
            }


            return true;
        }
    }
}