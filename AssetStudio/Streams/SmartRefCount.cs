using System;

namespace AssetStudio.Streams
{
    public sealed partial class SmartStream
    {
        private class SmartRefCount
        {
            public void Increase()
            {
                RefCount++;
            }

            public void Decrease()
            {
                if (RefCount == 0)
                {
                    throw new InvalidOperationException("Reference count cannot be negative");
                }
                RefCount--;
            }

            public override string ToString()
            {
                return RefCount.ToString();
            }

            public bool IsZero => RefCount == 0;

            public int RefCount { get; private set; } = 0;
        }
    }
}
