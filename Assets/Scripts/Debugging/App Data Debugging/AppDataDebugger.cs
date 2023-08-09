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

        #region Generic Log

        public void Log<T>(AppData.LogInfoChannel infoType, string logMessage, T fromClass = null) where T : AppData.DataDebugger => DebugLog(infoType, logMessage, fromClass);
        public void Log(AppData.LogInfoChannel infoType, string logMessage, string fromClassName = null)  => DebugLog(infoType, logMessage, fromClassName);

        #endregion

        #region Info

        public void LogInfo<T>(string logMessage, T fromClass = null) where T : AppData.DataDebugger => DebugLogInfo(logMessage, fromClass);
        public void LogInfo(string logMessage, string fromClassName = null) => DebugLogInfo(logMessage, fromClassName);

        #endregion

        #region Success

        public void LogSuccess<T>(string logMessage, T fromClass = null) where T : AppData.DataDebugger => DebugLogSuccess(logMessage, fromClass);
        public void LogSuccess(string logMessage, string fromClassName = null) => DebugLogSuccess(logMessage, fromClassName);

        #endregion

        #region Warnings

        public void LogWarning<T>(string logMessage, T fromClass = null) where T : AppData.DataDebugger => DebugLogWarning(logMessage, fromClass);
        public void LogWarning(string logMessage, string fromClassName = null) => DebugLogWarning(logMessage, fromClassName);

        #endregion

        #region Errors

        public void LogError<T>(string logMessage, T fromClass = null) where T : AppData.DataDebugger => DebugLogError(logMessage, fromClass);
        public void LogError(string logMessage, string fromClassName = null)  => DebugLogError(logMessage, fromClassName);

        #endregion

        #endregion
    }
}