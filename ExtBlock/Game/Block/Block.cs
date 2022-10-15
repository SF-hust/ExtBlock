using System.Diagnostics;
using ExtBlock.Core.State;
using ExtBlock.Core.Registry;

namespace ExtBlock.Game
{
    public class Block : IRegistryEntry<Block>,
        IStateDefiner<Block, BlockState>
    {
        // registry entry
        private RegistryInfo<Block>? _regInfo;
        public RegistryInfo<Block> RegInfo { get => _regInfo!; set => _regInfo ??= value; }
        public RegistryInfo UntypedRegInfo => _regInfo!;

        // state definition
        private StateDefinition<Block, BlockState>? _stateDefinition;
        public StateDefinition<Block, BlockState> StateDef
        {
            get => _stateDefinition!;
            set => _stateDefinition ??= value;
        }

        // property
        private BlockProperty _properties;
        public BlockProperty Properties => _properties;

        public Block(BlockProperty properties)
        {
            _properties = properties;
        }
    }
}
