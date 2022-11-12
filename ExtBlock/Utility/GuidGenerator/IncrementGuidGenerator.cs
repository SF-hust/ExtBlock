using System;
using System.Threading;

namespace ExtBlock.Utility.GuidGenerator
{
    public class IncrementGuidGenerator : IGuidGenerator
    {
        private long _curr;
        private readonly int _source;
        public Guid NextGuid
        {
            get
            {
                Interlocked.Increment(ref _curr);
                return new Guid(_source, 0, 0, BitConverter.GetBytes(_curr));
            }

        }

        public IncrementGuidGenerator(long initialValue, int source)
        {
            _curr = initialValue;
            _source = source;
        }
    }
}
