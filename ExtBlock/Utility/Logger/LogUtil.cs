namespace ExtBlock.Utility.Logger
{
    public static class LogUtil
    {
        public static LoggerWarp Logger = new LoggerWarp(null);
        public static ILogger GetLogger()
        {
            return Logger;
        }
    }
}
