using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class AppMonoBaseClass : MonoBehaviour
    {
        #region Components

        [Header("::: Component Info")]

        [Space(5)]
        [SerializeField]
        protected string className;

        [Space(5)]
        [Header("::: Component Debug Attributes")]

        [Space(5)]
        [SerializeField]
        protected AppData.DebugMonoLogHeaderAttributes logAttributes = new AppData.DebugMonoLogHeaderAttributes();

        #endregion

        #region Main

        #region Initializations

        public AppData.DebugMonoLogHeaderAttributes GetDebugMonoHeaderAttributes()
        {
            return logAttributes;
        }

        public string DebugLogCat(string log)
        {
            return logAttributes.GetLogCatInfoAttributes().GetDebugLogCat(log);
        }

        public bool HasDebugLogCat()
        {
            return logAttributes.GetLogCatInfoAttributes().HasDebugLogCat();
        }

        public string GetUniqueClassName()
        {
            if (string.IsNullOrEmpty(className))
                className = name;

            return className;
        }

        #endregion

        #region Log Info Functions

        #region Generic Log Functions

        public void Log(AppData.LogInfoType logInfoType, string logMessage, string fromClass = null, System.Action logFunc = null) => AppMonoDebugManager.Instance?.Log(logInfoType, logMessage, fromClass, logFunc);

        public void Logger(AppData.LogInfoType logInfoType, string logMessage, string fromClass = null, System.Action logFunc = null) => AppMonoDebugManager.Instance?.Log(logInfoType, logMessage, fromClass, logFunc);

        public void Log<T>(AppData.LogInfoType logInfoType, string logMessage, T fromClass = null, System.Action logFunc = null) where T : Object => AppMonoDebugManager.Instance?.Log(logInfoType, logMessage, fromClass, logFunc);

        public void DebugLog<T>(AppData.LogInfoType logInfoType, string logMessage, T fromClass = null, System.Action logFunc = null) where T : AppData.DataDebugger => AppMonoDebugManager.Instance?.Logger(logInfoType, logMessage, fromClass, logFunc);

        public void Log<T, U>(AppData.LogInfoType logInfoType, string logMessage, T fromClass = null, System.Action<U> logFunc = null) where T : Object where U : AppData.Callback => AppMonoDebugManager.Instance?.Log(logInfoType, logMessage, fromClass, logFunc);

        public void Logger<T, U>(AppData.LogInfoType logInfoType, string logMessage, T fromClass = null, System.Action<U> logFunc = null) where T : AppData.DataDebugger where U : AppData.Callback => AppMonoDebugManager.Instance?.Logger(logInfoType, logMessage, fromClass, logFunc);

        #endregion

        #region Specified Logs

        #region Info Log

        public void LogInfo(string logMessage, string fromClass = null, System.Action logFunc = null) => AppMonoDebugManager.Instance?.Log(AppData.LogInfoType.Info, logMessage, fromClass, logFunc);

        public void LogInfo<T>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : Object => AppMonoDebugManager.Instance?.Log(AppData.LogInfoType.Info, logMessage, fromClass, logFunc);

        public void LogInfo<T, U>(string logMessage, T fromClass = null, System.Action<U> logFunc = null) where T : Object where U : AppData.Callback => AppMonoDebugManager.Instance?.Log(AppData.LogInfoType.Info, logMessage, fromClass, logFunc);

        public void DebugLogInfo<T>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : AppData.DataDebugger => AppMonoDebugManager.Instance?.Logger(AppData.LogInfoType.Info, logMessage, fromClass, logFunc);

        public void LoggerInfo<T, U>(string logMessage, T fromClass = null, System.Action<U> logFunc = null) where T : AppData.DataDebugger where U : AppData.Callback => AppMonoDebugManager.Instance?.Logger(AppData.LogInfoType.Info, logMessage, fromClass, logFunc);

        #endregion

        #region Success Log

        public void LogSuccess(string logMessage, string fromClass = null, System.Action logFunc = null) => AppMonoDebugManager.Instance?.Log(AppData.LogInfoType.Success, logMessage, fromClass, logFunc);

        public void LogSuccess<T>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : Object => AppMonoDebugManager.Instance?.Log(AppData.LogInfoType.Success, logMessage, fromClass, logFunc);

        public void LogSuccess<T, U>(string logMessage, T fromClass = null, System.Action<U> logFunc = null) where T : Object where U : AppData.Callback => AppMonoDebugManager.Instance?.Log(AppData.LogInfoType.Success, logMessage, fromClass, logFunc);

        public void DebugLogSuccess<T>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : AppData.DataDebugger => AppMonoDebugManager.Instance?.Logger(AppData.LogInfoType.Success, logMessage, fromClass, logFunc);

        public void LoggerSuccess<T, U>(string logMessage, T fromClass = null, System.Action<U> logFunc = null) where T : AppData.DataDebugger where U : AppData.Callback => AppMonoDebugManager.Instance?.Logger(AppData.LogInfoType.Success, logMessage, fromClass, logFunc);

        #endregion

        #region Warning Log

        public void LogWarning(string logMessage, string fromClass = null, System.Action logFunc = null) => AppMonoDebugManager.Instance?.Log(AppData.LogInfoType.Warning, logMessage, fromClass, logFunc);

        public void LogWarning<T>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : Object => AppMonoDebugManager.Instance?.Log(AppData.LogInfoType.Warning, logMessage, fromClass, logFunc);

        public void LogWarning<T, U>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : Object where U : AppData.Callback => AppMonoDebugManager.Instance?.Log(AppData.LogInfoType.Warning, logMessage, fromClass, logFunc);

        public void DebugLogWarning<T>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : AppData.DataDebugger => AppMonoDebugManager.Instance?.Logger(AppData.LogInfoType.Warning, logMessage, fromClass, logFunc);

        public void LoggerWarning<T, U>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : AppData.DataDebugger where U : AppData.Callback => AppMonoDebugManager.Instance?.Logger(AppData.LogInfoType.Warning, logMessage, fromClass, logFunc);

        #endregion

        #region Error Log

        public void LogError(string logMessage, string fromClass = null, System.Action logFunc = null) => AppMonoDebugManager.Instance?.Log(AppData.LogInfoType.Error, logMessage, fromClass, logFunc);

        public void LogError<T>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : Object => AppMonoDebugManager.Instance?.Log(AppData.LogInfoType.Error, logMessage, fromClass, logFunc);

        public void LogError<T, U>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : Object where U : AppData.Callback => AppMonoDebugManager.Instance?.Log(AppData.LogInfoType.Error, logMessage, fromClass, logFunc);

        public void DebugLogError<T>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : AppData.DataDebugger => AppMonoDebugManager.Instance?.Logger(AppData.LogInfoType.Error, logMessage, fromClass, logFunc);

        public void LoggerError<T, U>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : AppData.DataDebugger where U : AppData.Callback => AppMonoDebugManager.Instance?.Logger(AppData.LogInfoType.Error, logMessage, fromClass, logFunc);

        #endregion

        #endregion

        #endregion

        #region Log Exceptions Functions

        #region Generic Exception Functions

        public void ThrowException(AppData.LogExceptionType logExceptionType, System.Exception exception, string fromClass = null, string logFunc = null) => AppMonoDebugManager.Instance?.ThrowException(logExceptionType, exception, fromClass, logFunc);

        public void ThrowException<T>(AppData.LogExceptionType logExceptionType, System.Exception exception, T fromClass = null, string logFunc = null) where T : Object => AppMonoDebugManager.Instance?.ThrowException(logExceptionType, exception, fromClass, logFunc);

        #endregion

        #region Specified Exception Functions

        #endregion

        #endregion

        #endregion
    }
}