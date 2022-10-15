using ExtBlock.Core.Registry;
using ExtBlock.Game;
using ExtBlock.Resource;

namespace ExtBlock.CoreMod.Blocks
{
    public static class Blocks
    {
        public static Block Air = new AirBlock(BlockProperty.Builder.Create().SetIsAir(true).SetBreakable(false).Build());

        public static Block Stone = new StoneBlock(BlockProperty.Builder.Create().SetBaseBreakTime(3.0f).Build());


        private static readonly DeferredRegister<Block> BlockRegister = DeferredRegister<Block>.Create(ResourceLocation.DEFAULT_NAMESPACE, Registries.BlockRegistry);
        private static void Register(string name, Block block)
        {
            BlockRegister.Register(name, block);
        }
        public static void Init()
        {
            Register("air", Air);
            Register("stone", Stone);
        }
    }
}
