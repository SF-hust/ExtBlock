using System;

using ExtBlock.Utility.Container;

namespace ExtBlock.Game.ChunkDataContainer
{
    public class PalletedChunkDataContainer<T> : PalletedContainer<T>, IChunkDataContainer<T> where T : class
    {
        public T Get(int x, int y, int z)
        {
            throw new NotImplementedException();
        }

        public void Set(int x, int y, int z, T value)
        {
            throw new NotImplementedException();
        }
    }
}
