using ExtBlock.Resource;

namespace ExtBlock.Utility
{
    public static class Constants
    {
        public const string DEFAULT_NAMESPACE = "extblock";

        public const string ROOT_REGISTRY_PATH = "registry";

        public static readonly ResourceLocation REGISTRY_LOCATION = ResourceLocation.Create(ROOT_REGISTRY_PATH);

        public const string BLOCK_STRING = "block";
        public const string ITEM_STRING = "item";
        public const string BLOCK_ENTITY_STRING = "blockentity";
        public const string BLOCK_ENTITY_TYPE_STRING = "blockentity_type";
        public const string ENTITY_STRING = "entity";
        public const string ENTITY_TYPE_STRING = "entity_type";
    }
}
