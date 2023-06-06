using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class AppMonoBaseClass : MonoBehaviour
    {
        #region Components

        [Header("Debugging Log Info Attributes")]
        [Space(10)]
        [SerializeField]
        protected AppData.LogInfoType enabledInfoLogs;

        [Space(10)]
        [Header("Debugging Log Exceptions Attributes")]
        [Space(10)]
        [SerializeField]
        protected AppData.LogExceptionType enabledExceptionLogs;

        #endregion

        #region Main

        #region Log Info Functions

        #region Generic Log Functions

        public void Log(AppData.LogInfoType logInfoType, string logMessage, string fromClass = null, System.Action logFunc = null) => AppMonoDebugManager.Instance?.Log(logInfoType, logMessage, fromClass, logFunc);

        public void Log<T>(AppData.LogInfoType logInfoType, string logMessage, T fromClass = null, System.Action logFunc = null) where T : Object => AppMonoDebugManager.Instance?.Log(logInfoType, logMessage, fromClass, logFunc);

        public void Log<T, U>(AppData.LogInfoType logInfoType, string logMessage, T fromClass = null, System.Action<U> logFunc = null) where T : Object where U : AppData.Callback => AppMonoDebugManager.Instance?.Log(logInfoType, logMessage, fromClass, logFunc);

        #endregion

        #region Specified Logs

        #region Info Log

        public void LogInfo(string logMessage, string fromClass = null, System.Action logFunc = null) => AppMonoDebugManager.Instance?.Log(AppData.LogInfoType.Info, logMessage, fromClass, logFunc);

        public void LogInfo<T>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : Object => AppMonoDebugManager.Instance?.Log(AppData.LogInfoType.Info, logMessage, fromClass, logFunc);

        public void LogInfo<T, U>(string logMessage, T fromClass = null, System.Action<U> logFunc = null) where T : Object where U : AppData.Callback => AppMonoDebugManager.Instance?.Log(AppData.LogInfoType.Info, logMessage, fromClass, logFunc);

        #endregion

        #region Success Log

        public void LogSuccess(string logMessage, string fromClass = null, System.Action logFunc = null) => AppMonoDebugManager.Instance?.Log(AppData.LogInfoType.Success, logMessage, fromClass, logFunc);

        public void LogSuccess<T>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : Object => AppMonoDebugManager.Instance?.Log(AppData.LogInfoType.Success, logMessage, fromClass, logFunc);

        public void LogSuccess<T, U>(string logMessage, T fromClass = null, System.Action<U> logFunc = null) where T : Object where U : AppData.Callback => AppMonoDebugManager.Instance?.Log(AppData.LogInfoType.Success, logMessage, fromClass, logFunc);

        #endregion

        #region Warning Log

        public void LogWarning(string logMessage, string fromClass = null, System.Action logFunc = null) => AppMonoDebugManager.Instance?.Log(AppData.LogInfoType.Warning, logMessage, fromClass, logFunc);

        public void LogWarning<T>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : Object => AppMonoDebugManager.Instance?.Log(AppData.LogInfoType.Warning, logMessage, fromClass, logFunc);

        public void LogWarning<T, U>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : Object where U : AppData.Callback => AppMonoDebugManager.Instance?.Log(AppData.LogInfoType.Warning, logMessage, fromClass, logFunc);

        #endregion

        #region Error Log

        public void LogError(string logMessage, string fromClass = null, System.Action logFunc = null) => AppMonoDebugManager.Instance?.Log(AppData.LogInfoType.Error, logMessage, fromClass, logFunc);

        public void LogError<T>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : Object => AppMonoDebugManager.Instance?.Log(AppData.LogInfoType.Error, logMessage, fromClass, logFunc);

        public void LogError<T, U>(string logMessage, T fromClass = null, System.Action logFunc = null) where T : Object where U : AppData.Callback => AppMonoDebugManager.Instance?.Log(AppData.LogInfoType.Error, logMessage, fromClass, logFunc);

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