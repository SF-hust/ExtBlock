using System;
using System.Text.Json.Nodes;

namespace ExtBlock.Game
{
    public class ItemStack
    {
        private readonly Item _item;
        public Item Item => _item;
        public readonly int stackFactor;

        private int _stackSizeCache;
        public int MaxStackSize => _stackSizeCache;
        protected void UpdateStackSize()
        {
            _stackSizeCache = Item.Properties.stackable? Item.Properties.baseStackSize * stackFactor : 1;
        }

        private int _count;
        public int Count
        {
            get => _count;
            set => _count = System.Math.Clamp(value, 0, MaxStackSize);
        }

        public readonly JsonObject ExtraData = new JsonObject();

        public int Increase(int count)
        {
            int oCount = Count;
            Count = Count + count;
            return Count - oCount;
        }

        public int Decrease(int count)
        {
            int oCount = Count;
            Count = Count - count;
            return oCount - Count;
        }

        protected ItemStack(Item item, int factor)
        {
            _item = item;
            stackFactor = factor;
        }
    }
}
