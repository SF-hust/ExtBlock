using System.Diagnostics;
using ExtBlock.Core.State;
using ExtBlock.Core.Registry;

namespace ExtBlock.Game
{
    public class Block : IRegistryEntry<Block>,
        IStateDefiner<Block, BlockState>
    {
        // registry entry
        private RegistryEntryInfo<Block>? _regInfo;
        public RegistryEntryInfo<Block> RegEntryInfo { get => _regInfo!; set => _regInfo ??= value; }
        public RegistryEntryInfo UntypedRegInfo => _regInfo!;

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
