using ExtBlock.Math;
using ExtBlock.Game.World;
using ExtBlock.Game.Block;

namespace ExtBlock.Game
{
    public interface IChunk
    {
        /// <summary>
        /// Chunk ������ World
        /// </summary>
        public IWorld World { get; }

        /// <summary>
        /// Chunk ������ World �е�����
        /// </summary>
        public ChunkPos ChunkPos { get; }

        /// <summary>
        /// Chunk ������ World �е�X����
        /// </summary>
        public int X => ChunkPos.x;
        /// <summary>
        /// Chunk ������ World �е�Y����
        /// </summary>
        public int Y => ChunkPos.y;
        /// <summary>
        /// Chunk ������ World �е�Z����
        /// </summary>
        public int Z => ChunkPos.z;

        /// <summary>
        /// ָʾ�ɷ���� Chunk ������
        /// </summary>
        public bool Writable { get; }

        /// <summary>
        /// ָʾ Chunk �Ƿ�Ϊ��
        /// </summary>
        public bool IsEmpty { get; }

        /// <summary>
        /// ��� Chunk ����д, ʲôҲ����
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="blockState"></param>
        public void SetBlockState(int x, int y, int z, BlockState blockState);

        /// <summary>
        /// ��� Chunk ����д, ʲôҲ����
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="blockState"></param>
        public void SetBlockState(BlockPos pos, BlockState blockState)
        {
            SetBlockState(pos.x, pos.y, pos.z, blockState);
        }

        /// <summary>
        /// ��ȡ Chunk �е�ĳ�� BlockState, ����Ϊ Chunk ��������
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public BlockState GetBlockState(int x, int y, int z);

        /// <summary>
        /// ��ȡ Chunk �е�ĳ�� BlockState, ����Ϊ Chunk ��������
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public BlockState GetBlockState(BlockPos pos)
        {
            return GetBlockState(pos.x, pos.y, pos.z);
        }

        /// <summary>
        /// �� Chunk �е� BlockState ���ݴ洢��һ�� BlockState ������
        /// </summary>
        /// <param name="array"></param>
        public void CopyToArray(BlockState[] array);
    }
}