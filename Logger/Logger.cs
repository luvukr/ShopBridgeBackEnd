using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Log
{
    public class Logger
    {

        public static void Init()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger.Log("Logging Initialized", LogLevel.INFO);
        }

        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static void Log(string message, string logLevel = LogLevel.DEBUG, Exception exception = null)
        {
            string logMessage = message;


            try
            {
                switch (logLevel)
                {
                    case LogLevel.ERROR:
                        var ex = exception.GetBaseException();
                        
                        log.Error(logMessage, ex);
                        break;
                    case LogLevel.INFO:

                        log.Info(logMessage);
                        break;

                }
            }
            catch (Exception ex)
            {

            }
        }
        public static void Error(string message, Exception exception = null)
        {
            Log(message, LogLevel.ERROR, exception);
        }

    }
}
