using System;

using ExtBlock.Core.Registry;
using ExtBlock.Core.State;
using ExtBlock.Core.Component;
using ExtBlock.Core.Property;

namespace ExtBlock.Game.Block
{
    /// <summary>
    /// 由于游戏使用面向组件的设计, 不会再为每种 Block 编写一个新的类,
    /// 但为了 Modder 可能有的定制需求, 此类可以被继承
    /// </summary>
    public class Block : IRegistryEntry<Block>,
        IStateDefiner<Block, BlockState>,
        IComponentHolder
    {
        public Block()
        {
            _components = new ComponentHolder(this);
        }
        
        /*
         * 作为 RegistryEntry
         */

        public RegistryEntryInfo<Block> RegEntryInfo { get => _regInfo!; set => _regInfo ??= value; }
        private RegistryEntryInfo<Block>? _regInfo;
        public virtual Type AsEntryType => typeof(Block);

        /*
         * 作为 StateDefiner
         */

        private StateDefinition<Block, BlockState>? _stateDefinition;
        public StateDefinition<Block, BlockState> StateDefinition
        {
            get => _stateDefinition!;
            set => _stateDefinition ??= value;
        }

        /*
         * 作为 ComponentHolder
         */

        public ComponentHolder Components => _components;
        private readonly ComponentHolder _components;

        /*
         * Block 的内置组件
         */

        /// <summary>
        /// Block 的基础属性
        /// </summary>
        public BlockPropertyComponent Properties
        {
            get => _properties!;
            set => _properties ??= value;
        }
        private BlockPropertyComponent? _properties;

        /// <summary>
        /// Block 的额外属性
        /// </summary>
        public ExtraPropertyComponent ExtraProperties
            {
            get => _extraProperties!;
            set => _extraProperties ??= value;
        }
        private ExtraPropertyComponent? _extraProperties;
    }
}
