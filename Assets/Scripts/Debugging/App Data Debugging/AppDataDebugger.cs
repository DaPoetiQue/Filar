namespace Com.RedicalGames.Filar
{
    public class AppDataDebugger : AppMonoBaseClass
{
        #region Components

        private static AppDataDebugger instance;

        public static AppDataDebugger Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<AppDataDebugger>();

                return instance;
            }
        }

        #endregion

        #region Main

        public void Log<T>(AppData.LogInfoType infoType, string logMessage, T fromClass) where T : AppData.DataDebugger => DebugLog(infoType, logMessage, fromClass);

        public void LogInfo<T>(string logMessage, T fromClass) where T : AppData.DataDebugger => DebugLogInfo(logMessage, fromClass);

        public void LogSuccess<T>(string logMessage, T fromClass) where T : AppData.DataDebugger => DebugLogSuccess(logMessage, fromClass);

        public void LogWarning<T>(string logMessage, T fromClass) where T : AppData.DataDebugger => DebugLogWarning(logMessage, fromClass);

        public void LogError<T>(string logMessage, T fromClass) where T : AppData.DataDebugger => DebugLogError(logMessage, fromClass);

        #endregion
    }
}