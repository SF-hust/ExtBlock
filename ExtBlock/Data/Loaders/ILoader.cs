namespace ExtBlock.Data.Loaders
{
    public interface ILoader
    {
    }

    public interface ILoader<T>
    {
        public T Load(string path);
    }
}
