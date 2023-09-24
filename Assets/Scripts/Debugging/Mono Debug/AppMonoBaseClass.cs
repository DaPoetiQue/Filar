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

        public void Log<T>(AppData.LogInfoChannel logInfoType, string logMessage, T fromClass = null, System.Action logFunc = null) where T : Object => AppMonoDebugManager.Instance?.Log(logInfoType, logMessage, fromClass, logFunc);
        public void Log(AppData.LogInfoChannel logInfoType, string logMessage, string className = null, System.Action logFunc = null) => AppMonoDebugManager.Instance?.Log(logInfoType, logMessage, className, logFunc);

        public void Logger(AppData.LogInfoChannel logInfoType, string logMessage, string className = null, System.Action logFunc = null) => AppMonoDebugManager.Instance?.Log(logInfoType, logMessage, className, logFunc);

        public void DebugLog<T>(AppData.LogInfoChannel logInfoType, string logMessage, T fromClass = null, System.Action logFunc = null) where T : AppData.DataDebugger => AppMonoDebugManager.Instance?.Logger(logInfoType, logMessage, fromClass, logFunc);
        public void DebugLog(AppData.LogInfoChannel logInfoType, string logMessage, string className = null, System.Action logFunc = null) => AppMonoDebugManager.Instance?.Logger(logInfoType, logMessage, className, logFunc);

        public void Log<T, U>(AppData.LogInfoChannel logInfoType, string logMessage, T fromClass = null, System.Action<U> logFunc = null) where T : Object where U : AppData.Callback => AppMonoDebugManager.Instance?.Log(logInfoType, logMessage, fromClass, logFunc);

        public void Logger<T, U>(AppData.LogInfoChannel logInfoType, string logMessage, T fromClass = null, System.Action<U> logFunc = null) where T : AppData.DataDebugger where U : AppData.Callback => AppMonoDebugManager.Instance?.Logger(logInfoType, logMessage, fromClass, logFunc);

        #endregion

        #region Specified Logs

        #region Info Log

        public void LogInfo<T>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : Object => AppMonoDebugManager.Instance?.Log(AppData.LogInfoChannel.Debug, logMessage, fromClass, logFunc);

        public void LogInfo<T, U>(string logMessage, T fromClass = null, System.Action<U> logFunc = null) where T : Object where U : AppData.Callback => AppMonoDebugManager.Instance?.Log(AppData.LogInfoChannel.Debug, logMessage, fromClass, logFunc);
        public void LogInfo(string logMessage, string className = null) => AppMonoDebugManager.Instance?.Log(AppData.LogInfoChannel.Debug, logMessage, className);

        public void DebugLogInfo<T>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : AppData.DataDebugger => AppMonoDebugManager.Instance?.Logger(AppData.LogInfoChannel.Debug, logMessage, fromClass, logFunc);
        public void DebugLogInfo(string logMessage, string className = null, System.Action logFunc = null) => AppMonoDebugManager.Instance?.Logger(AppData.LogInfoChannel.Debug, logMessage, className, logFunc);

        public void LoggerInfo<T, U>(string logMessage, T fromClass = null, System.Action<U> logFunc = null) where T : AppData.DataDebugger where U : AppData.Callback => AppMonoDebugManager.Instance?.Logger(AppData.LogInfoChannel.Debug, logMessage, fromClass, logFunc);
        public void LoggerInfo(string logMessage, string className = null)  => AppMonoDebugManager.Instance?.Logger(AppData.LogInfoChannel.Debug, logMessage, className);

        #endregion

        #region Success Log

        public void LogSuccess(string logMessage, string className = null, System.Action logFunc = null) => AppMonoDebugManager.Instance?.Log(AppData.LogInfoChannel.Success, logMessage, className, logFunc);

        public void LogSuccess<T>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : Object => AppMonoDebugManager.Instance?.Log(AppData.LogInfoChannel.Success, logMessage, fromClass, logFunc);

        public void LogSuccess<T, U>(string logMessage, T fromClass = null, System.Action<U> logFunc = null) where T : Object where U : AppData.Callback => AppMonoDebugManager.Instance?.Log(AppData.LogInfoChannel.Success, logMessage, fromClass, logFunc);

        public void DebugLogSuccess<T>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : AppData.DataDebugger => AppMonoDebugManager.Instance?.Logger(AppData.LogInfoChannel.Success, logMessage, fromClass, logFunc);
        public void DebugLogSuccess(string logMessage, string className = null, System.Action logFunc = null) => AppMonoDebugManager.Instance?.Logger(AppData.LogInfoChannel.Success, logMessage, className, logFunc);

        public void LoggerSuccess<T, U>(string logMessage, T fromClass = null, System.Action<U> logFunc = null) where T : AppData.DataDebugger where U : AppData.Callback => AppMonoDebugManager.Instance?.Logger(AppData.LogInfoChannel.Success, logMessage, fromClass, logFunc);
        public void LoggerSuccess(string logMessage, string className = null) => AppMonoDebugManager.Instance?.Logger(AppData.LogInfoChannel.Success, logMessage, className);

        #endregion

        #region Warning Log

        public void LogWarning(string logMessage, string className = null, System.Action logFunc = null) => AppMonoDebugManager.Instance?.Log(AppData.LogInfoChannel.Warning, logMessage, className, logFunc);

        public void LogWarning<T>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : Object => AppMonoDebugManager.Instance?.Log(AppData.LogInfoChannel.Warning, logMessage, fromClass, logFunc);

        public void LogWarning<T, U>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : Object where U : AppData.Callback => AppMonoDebugManager.Instance?.Log(AppData.LogInfoChannel.Warning, logMessage, fromClass, logFunc);

        public void DebugLogWarning<T>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : AppData.DataDebugger => AppMonoDebugManager.Instance?.Logger(AppData.LogInfoChannel.Warning, logMessage, fromClass, logFunc);
        public void DebugLogWarning(string logMessage, string className = null, System.Action logFunc = null) => AppMonoDebugManager.Instance?.Logger(AppData.LogInfoChannel.Warning, logMessage, className, logFunc);

        public void LoggerWarning<T, U>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : AppData.DataDebugger where U : AppData.Callback => AppMonoDebugManager.Instance?.Logger(AppData.LogInfoChannel.Warning, logMessage, fromClass, logFunc);
        public void LoggerWarning(string logMessage, string className = null, System.Action logFunc = null) => AppMonoDebugManager.Instance?.Logger(AppData.LogInfoChannel.Warning, logMessage, className, logFunc);

        #endregion

        #region Error Log

        public void LogError(string logMessage, string className = null, System.Action logFunc = null) => AppMonoDebugManager.Instance?.Log(AppData.LogInfoChannel.Error, logMessage, className, logFunc);

        public void LogError<T>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : Object => AppMonoDebugManager.Instance?.Log(AppData.LogInfoChannel.Error, logMessage, fromClass, logFunc);

        public void LogError<T, U>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : Object where U : AppData.Callback => AppMonoDebugManager.Instance?.Log(AppData.LogInfoChannel.Error, logMessage, fromClass, logFunc);

        public void DebugLogError<T>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : AppData.DataDebugger => AppMonoDebugManager.Instance?.Logger(AppData.LogInfoChannel.Error, logMessage, fromClass, logFunc);
        public void DebugLogError(string logMessage, string className = null, System.Action logFunc = null) => AppMonoDebugManager.Instance?.Logger(AppData.LogInfoChannel.Error, logMessage, className, logFunc);

        public void LoggerError<T, U>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : AppData.DataDebugger where U : AppData.Callback => AppMonoDebugManager.Instance?.Logger(AppData.LogInfoChannel.Error, logMessage, fromClass, logFunc);

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