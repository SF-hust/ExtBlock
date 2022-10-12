using ExtBlock.Core.State;
using ExtBlock.Enums;

namespace ExtBlock.Core
{
    public static class BlockStateProperties
    {
        public static readonly EnumStateProperty<Direction> DIRECTION = EnumStateProperty<Direction>.Create("direction");

        public static readonly EnumStateProperty<Direction> FACING = EnumStateProperty<Direction>.Create("facing");
        public static readonly EnumStateProperty<Direction> FACING_HORIZONTAL = EnumStateProperty<Direction>.Create("facing_horizontal",
            new Direction[] { Direction.North, Direction.East, Direction.South, Direction.West });
        public static readonly EnumStateProperty<Direction> FACING_VERTICAL = EnumStateProperty<Direction>.Create("facing_vertical",
            new Direction[] { Direction.Up, Direction.Down });

        public static readonly EnumStateProperty<Axis> AXIS = EnumStateProperty<Axis>.Create("axis");
        public static readonly EnumStateProperty<Axis> AXIS_HORIZONTAL = EnumStateProperty<Axis>.Create("axis_horizontal");

        public static readonly EnumStateProperty<FencePart> FENCE_NORTH = EnumStateProperty<FencePart>.Create("fence_north");
        public static readonly EnumStateProperty<FencePart> FENCE_EAST = EnumStateProperty<FencePart>.Create("fence_east");
        public static readonly EnumStateProperty<FencePart> FENCE_SOUTH = EnumStateProperty<FencePart>.Create("fence_south");
        public static readonly EnumStateProperty<FencePart> FENCE_WEST = EnumStateProperty<FencePart>.Create("fence_west");

        public static readonly BooleanStateProperty NORTH = BooleanStateProperty.Create("north");
        public static readonly BooleanStateProperty EAST = BooleanStateProperty.Create("east");
        public static readonly BooleanStateProperty SOUTH = BooleanStateProperty.Create("south");
        public static readonly BooleanStateProperty WEST = BooleanStateProperty.Create("west");

        public static readonly EnumStateProperty<Direction> TORCH_ATTACH = EnumStateProperty<Direction>.Create("attach",
            new Direction[] { Direction.Down, Direction.North, Direction.East, Direction.South, Direction.West, Direction.Up });

        public static readonly BooleanStateProperty LIGHT = BooleanStateProperty.Create("light");

        public static readonly EnumStateProperty<SlabPart> SLAB_PART = EnumStateProperty<SlabPart>.Create("slab_part");
    }
}
