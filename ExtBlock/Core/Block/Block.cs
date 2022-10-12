using System.Diagnostics;
using ExtBlock.Core.State;
using ExtBlock.Core.Registry;

namespace ExtBlock.Core
{
    using BlockStateDefBuilder = StateDefinition<Block, BlockState>.Builder;

    public class Block : IRegistryEntry<Block>,
        IBlockPropertyProvider,
        IStateDefiner<Block, BlockState>
    {
        // registry
        private RegistryInfo<Block>? _regInfo;
        RegistryInfo<Block> IRegistryEntry<Block>.RegInfo { get => _regInfo!; set => _regInfo = value; }
        RegistryInfo IRegistryEntry.UntypedRegInfo => _regInfo!;

        // property
        private BlockPropertyProvider _properties = new BlockPropertyProvider();
        public BlockPropertyProvider Properties => _properties;

        // state
        private StateDefinition<Block, BlockState>? _stateDefinition = null;
        public StateDefinition<Block, BlockState> StateDef
        {
            get
            {
                Debug.Assert(_stateDefinition != null);
                return _stateDefinition;
            }
        }


        public Block()
        {
            BlockStateDefBuilder stateBuilder = BlockStateDefBuilder.Create(this, BlockState.Factory);
            CreateStateDefinition(stateBuilder);
        }

        protected virtual void CreateStateDefinition(BlockStateDefBuilder stateDefBuilder)
        {
        }
    }
}
