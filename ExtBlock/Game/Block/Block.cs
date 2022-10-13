using System.Diagnostics;
using ExtBlock.Core.State;
using ExtBlock.Core.Registry;

namespace ExtBlock.Core
{
    public class Block : IRegistryEntry<Block>,
        IBlockPropertyProvider,
        IStateDefiner<Block, BlockState>
    {
        // registry entry
        private RegistryInfo<Block>? _regInfo;
        public RegistryInfo<Block> RegInfo { get => _regInfo!; set => _regInfo ??= value; }
        public RegistryInfo UntypedRegInfo => _regInfo!;

        // property
        private BlockProperty _properties;
        public BlockProperty Properties => _properties;

        // state definition
        private StateDefinition<Block, BlockState>? _stateDefinition;
        public StateDefinition<Block, BlockState> StateDef
        {
            get
            {
                Debug.Assert(_stateDefinition != null);
                return _stateDefinition;
            }
            set => _stateDefinition ??= value;
        }


        public Block(BlockProperty properties)
        {
            _properties = properties;
        }
    }
}
