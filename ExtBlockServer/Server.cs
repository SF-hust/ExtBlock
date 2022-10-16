using ExtBlock;
using ExtBlock.Core;
using ExtBlock.Core.Registry;
using ExtBlock.CoreMod.Blocks;
using ExtBlock.Game;
using ExtBlock.Server;
using ExtBlock.Utility.Logger;

namespace ExtBlockServer
{
    public class Server
    {
        private readonly static ILogger logger = LogUtil.GetLogger();
        static void Main(string[] args)
        {
            LogUtil.Logger.SetLogger(new ConsoleLogger());
            logger.Info("Server start runing...");
            try
            {
                Blocks.Init();
                Registries.InitInternalRegistries();
                Registries.FireRootRegisterEvent();
                Registries.FireRegisterEvents();
                logger.Info("Server terminate.");
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }
        }
    }
}