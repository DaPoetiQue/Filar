using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class AppMonoDebugManager : AppMonoBaseClass
    {
        #region Instance

        private static AppMonoDebugManager instance;

        public static AppMonoDebugManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<AppMonoDebugManager>();

                return instance;
            }

            set
            {
                instance = value;
            }
        }

        #endregion

        #region Components

        [Space(10)]
        [Header("Debug Attributes")]

        [Space(5)]
        [SerializeField]
        List<AppData.MonoLogInfo> logInfoAttributesList = new List<AppData.MonoLogInfo>();

        [Space(5)]
        [SerializeField]
        List<AppData.MonoLogAttributes> logAttributesList = new List<AppData.MonoLogAttributes>();

        #endregion

        #region Unity Callbacks

        void Awake() => Setup();

        #endregion

        #region Initializations

        void Setup()
        {
            if (instance != null && instance != this)
                Destroy(instance);

            instance = this;
        }

        #endregion

        #region Main

        #region Log Info

        #region Generic Log Info Functions

        new public void Log<T>(AppData.LogInfoType logInfoType, string logMessage, T fromClass = null, Action logFunc = null) where T : UnityEngine.Object => Log(logInfoType, logMessage, fromClass?.name, logFunc);

        new public void Log<T, U>(AppData.LogInfoType logInfoType, string logMessage, T fromClass = null, Action<U> logFunc = null) where T : UnityEngine.Object where U : AppData.Callback => Log(logInfoType, logMessage, fromClass?.name, logFunc);

        new public void Log(AppData.LogInfoType logInfoType, string logMessage, string fromClass = null, Action logFunc = null)
        {
            if (GetDebugMonoHeaderAttributes().GetEnabledLogInfoType() == AppData.LogInfoType.None || logInfoType == AppData.LogInfoType.None)
                return;

            if (GetDebugMonoHeaderAttributes().GetEnabledLogInfoType() == AppData.LogInfoType.All || logInfoType == GetDebugMonoHeaderAttributes().GetEnabledLogInfoType())
            {
                #region Symbols Info

                string startBracket = "<color=white><b>[</b></color>";
                string endBracket = "<color=white><b>]</b></color>";

                #endregion

                #region Log Data Info

                var logInfoData = GetLogInfo(logInfoType);
                string logTypeString = (GetLogAttribute(AppData.LogAttributeType.Symbols).isBoldFontWeight) ? $" {startBracket} <color={logInfoData.logColorValue}><b>{logInfoType} Code</b></color> {endBracket}" : $" {startBracket} <color={logInfoData.logColorValue}>{logInfoType}</color> {endBracket}";

                #endregion

                #region Class Info

                string className = (GetLogAttribute(AppData.LogAttributeType.Class).isBoldFontWeight) ? $"<b>Class</b> {startBracket} ~<color={GetLogAttribute(AppData.LogAttributeType.Class).attributeColorValue}><b>{fromClass}</b></color>~ {endBracket} -->" : $"<b>Class</b> {startBracket} ~<color={GetLogAttribute(AppData.LogAttributeType.Class).attributeColorValue}>{fromClass}</color>~ {endBracket} --";
                string classInfo = (fromClass != null || !string.IsNullOrEmpty(fromClass)) ? className : string.Empty;

                #endregion

                #region Function Info

                string functionName = (GetLogAttribute(AppData.LogAttributeType.Function).isBoldFontWeight) ? $"<b>Function</b> {startBracket} ~<color={GetLogAttribute(AppData.LogAttributeType.Function).attributeColorValue}><b>{GetFunctionAttribute(logFunc)}</b></color>~ {endBracket} --" : $"<b>Function</b> {startBracket} ~<color={GetLogAttribute(AppData.LogAttributeType.Function).attributeColorValue}>{GetFunctionAttribute(logFunc)}</color>~ {endBracket} --";
                string functionInfo = (logFunc != null) ? functionName : string.Empty;

                #endregion

                #region Console Log

                string log = (GetLogAttribute(AppData.LogAttributeType.LogMessage).isBoldFontWeight) ? $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}><b>{logMessage}</b></color>" : $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}>{logMessage}</color>";

                string loggedInfo = $"<color=white><b>Debugging</b></color><color=grey> >>> </color> {logTypeString} <color=grey><b>--</b></color> {classInfo} {functionInfo} <color=grey><b>Log Message ==> </b></color> : {log}";

                Debug.Log(loggedInfo);

                #endregion
            }
        }

        public void Log<U>(AppData.LogInfoType logInfoType, string logMessage, string fromClass = null, Action<U> logFunc = null) where U : AppData.Callback
        {
            if (GetDebugMonoHeaderAttributes().GetEnabledLogInfoType() == AppData.LogInfoType.None)
                return;

            if (GetDebugMonoHeaderAttributes().GetEnabledLogInfoType() == AppData.LogInfoType.All || logInfoType == GetDebugMonoHeaderAttributes().GetEnabledLogInfoType())
            {
                #region Symbols Info

                string startBracket = "<color=white><b>[</b></color>";
                string endBracket = "<color=white><b>]</b></color>";

                #endregion

                #region Log Data Info

                var logInfoData = GetLogInfo(logInfoType);
                string logTypeString = (GetLogAttribute(AppData.LogAttributeType.Symbols).isBoldFontWeight) ? $" {startBracket} <color={logInfoData.logColorValue}><b>{logInfoType} Code</b></color> {endBracket}" : $" {startBracket} <color={logInfoData.logColorValue}>{logInfoType}</color> {endBracket}";

                #endregion

                #region Class Info

                string className = (GetLogAttribute(AppData.LogAttributeType.Class).isBoldFontWeight) ? $"<b>Class</b> {startBracket} ~<color={GetLogAttribute(AppData.LogAttributeType.Class).attributeColorValue}><b>{fromClass}</b></color>~ {endBracket} -->" : $"<b>Class</b> {startBracket} ~<color={GetLogAttribute(AppData.LogAttributeType.Class).attributeColorValue}>{fromClass}</color>~ {endBracket} --";
                string classInfo = (fromClass != null || !string.IsNullOrEmpty(fromClass)) ? className : string.Empty;

                #endregion

                #region Function Info

                string functionName = (GetLogAttribute(AppData.LogAttributeType.Function).isBoldFontWeight) ? $"<b>Function</b> {startBracket} ~<color={GetLogAttribute(AppData.LogAttributeType.Function).attributeColorValue}><b>{GetCallbackFunctionAttribute(logFunc)}</b></color>~ {endBracket} --" : $"<b>Function</b> {startBracket} ~<color={GetLogAttribute(AppData.LogAttributeType.Function).attributeColorValue}>{GetCallbackFunctionAttribute(logFunc)}</color>~ {endBracket} --";
                string functionInfo = (logFunc != null) ? functionName : string.Empty;

                #endregion

                #region Console Log

                string log = (GetLogAttribute(AppData.LogAttributeType.LogMessage).isBoldFontWeight) ? $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}><b>{logMessage}</b></color>" : $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}>{logMessage}</color>";

                string loggedInfo = $"<color=white><b>Debugging</b></color><color=grey> >>> </color> {logTypeString} <color=grey><b>--</b></color> {classInfo} {functionInfo} <color=grey><b>Log Message ==> </b></color> : {log}";

                if (GetDebugMonoHeaderAttributes().GetLogCatInfoAttributes().HasDebugLogCat())
                    Debug.Log(GetDebugMonoHeaderAttributes().GetLogCatInfoAttributes().GetDebugLogCat(loggedInfo));
                else
                    Debug.Log(loggedInfo);

                #endregion
            }
        }

        #region Function Name

        public string GetFunctionAttribute(Action logFunc = null)
        {
            return (logFunc != null)? logFunc?.Method.Name + $"({logFunc?.Method.GetParameters().ToString()})" : string.Empty;
        }

        public string GetCallbackFunctionAttribute<U>(Action<U> logFunc = null) where U : AppData.Callback
        {
            return (logFunc != null) ? logFunc?.Method.Name + $"({logFunc?.Method.GetParameters().ToString()})" : string.Empty;
        }

        public string GetDynamicFunctionAttributeName<T>(Action<T> logFunc)
        {
            string attribute = "";

            return attribute;
        }

        public string GetUnityFunctionAttributeName<T>(Action<T> logFunc) where T : UnityEngine.Object
        {
            string attribute = "";

            return attribute;
        }

        #endregion

        #endregion

        #region Specified Logs

        #region Info

        new public void LogInfo(string logMessage, string fromClass = null, Action logFunc = null) => Log(AppData.LogInfoType.Info, logMessage, fromClass, logFunc);

        new public void LogInfo<T>(string logMessage, T fromClass = null, Action logFunc = null) where T : UnityEngine.Object => Log(AppData.LogInfoType.Info, logMessage, fromClass, logFunc);

        #endregion

        #region Success

        new public void LogSuccess(string logMessage, string fromClass = null, Action logFunc = null) => Log(AppData.LogInfoType.Success, logMessage, fromClass, logFunc);

        new public void LogSuccess<T>(string logMessage, T fromClass = null, Action logFunc = null) where T : UnityEngine.Object => Log(AppData.LogInfoType.Success, logMessage, fromClass, logFunc);

        #endregion

        #region Warnings

        new public void LogWarning(string logMessage, string fromClass = null, Action logFunc = null) => Log(AppData.LogInfoType.Warning, logMessage, fromClass, logFunc);

        new public void LogWarning<T>(string logMessage, T fromClass = null, Action logFunc = null) where T : UnityEngine.Object => Log(AppData.LogInfoType.Warning, logMessage, fromClass, logFunc);

        #endregion

        #region Errors

        new public void LogError(string logMessage, string fromClass = null, Action logFunc = null) => Log(AppData.LogInfoType.Error, logMessage, fromClass, logFunc);

        new public void LogError<T>(string logMessage, T fromClass = null, Action logFunc = null) where T : UnityEngine.Object => Log(AppData.LogInfoType.Error, logMessage, fromClass, logFunc);

        #endregion

        #endregion

        #endregion

        #region Log Exceptions

        #region Generic Exception Log Functions

        new public void ThrowException<T>(AppData.LogExceptionType logExceptionType, Exception exception, T fromClass = null, string logFunc = null) where T : UnityEngine.Object => ThrowException(logExceptionType, exception, fromClass?.name, logFunc);

        new public void ThrowException(AppData.LogExceptionType logExceptionType, Exception exception, string fromClass = null, string logFunc = null)
        {
            if (GetDebugMonoHeaderAttributes().GetEnabledLogExceptionType() == AppData.LogExceptionType.None)
                return;

            if (GetDebugMonoHeaderAttributes().GetEnabledLogExceptionType() == AppData.LogExceptionType.All || logExceptionType == GetDebugMonoHeaderAttributes().GetEnabledLogExceptionType())
            { 
                if(logExceptionType == AppData.LogExceptionType.Exception)
                {
                    string log = (GetLogAttribute(AppData.LogAttributeType.LogMessage).isBoldFontWeight) ? $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}><b>{exception}</b></color>" : $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}>{exception}</color>";

                    string loggedMessage = $"Execption Thrown >>>  <b>[{logExceptionType} Code]</b> --> Insided Class [~{fromClass}~] - From Function : {logFunc} Exception : {log}";
                    throw new Exception(loggedMessage);
                }

                if (logExceptionType == AppData.LogExceptionType.NullReference)
                {
                    string log = (GetLogAttribute(AppData.LogAttributeType.LogMessage).isBoldFontWeight) ? $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}><b>{exception}</b></color>" : $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}>{exception}</color>";

                    string loggedMessage = $"Execption Thrown >>>  <b>[{logExceptionType} Code]</b> --> Insided Class [~{fromClass}~] - From Function : {logFunc} Exception : {log}";
                    throw new NullReferenceException(loggedMessage);
                }

                if (logExceptionType == AppData.LogExceptionType.Argument)
                {
                    string log = (GetLogAttribute(AppData.LogAttributeType.LogMessage).isBoldFontWeight) ? $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}><b>{exception}</b></color>" : $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}>{exception}</color>";

                    string loggedMessage = $"Execption Thrown >>>  <b>[{logExceptionType} Code]</b> --> Insided Class [~{fromClass}~] - From Function : {logFunc} Exception : {log}";
                    throw new ArgumentException(loggedMessage);
                }

                if (logExceptionType == AppData.LogExceptionType.ArgumentNull)
                {
                    string log = (GetLogAttribute(AppData.LogAttributeType.LogMessage).isBoldFontWeight) ? $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}><b>{exception}</b></color>" : $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}>{exception}</color>";

                    string loggedMessage = $"Execption Thrown >>>  <b>[{logExceptionType} Code]</b> --> Insided Class [~{fromClass}~] - From Function : {logFunc} Exception : {log}";
                    throw new ArgumentNullException(loggedMessage);
                }

                if (logExceptionType == AppData.LogExceptionType.ArgumentOutOfRange)
                {
                    string log = (GetLogAttribute(AppData.LogAttributeType.LogMessage).isBoldFontWeight) ? $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}><b>{exception}</b></color>" : $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}>{exception}</color>";

                    string loggedMessage = $"Execption Thrown >>>  <b>[{logExceptionType} Code]</b> --> Insided Class [~{fromClass}~] - From Function : {logFunc} Exception : {log}";
                    throw new ArgumentOutOfRangeException(loggedMessage);
                }

                if (logExceptionType == AppData.LogExceptionType.AccessViolation)
                {
                    string log = (GetLogAttribute(AppData.LogAttributeType.LogMessage).isBoldFontWeight) ? $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}><b>{exception}</b></color>" : $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}>{exception}</color>";

                    string loggedMessage = $"Execption Thrown >>>  <b>[{logExceptionType} Code]</b> --> Insided Class [~{fromClass}~] - From Function : {logFunc} Exception : {log}";
                    throw new AccessViolationException(loggedMessage);
                }

                if (logExceptionType == AppData.LogExceptionType.IndexOutOfRange)
                {
                    string log = (GetLogAttribute(AppData.LogAttributeType.LogMessage).isBoldFontWeight) ? $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}><b>{exception}</b></color>" : $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}>{exception}</color>";

                    string loggedMessage = $"Execption Thrown >>>  <b>[{logExceptionType} Code]</b> --> Insided Class [~{fromClass}~] - From Function : {logFunc} Exception : {log}";
                    throw new IndexOutOfRangeException(loggedMessage);
                }

                if (logExceptionType == AppData.LogExceptionType.EntryPointNotFound)
                {
                    string log = (GetLogAttribute(AppData.LogAttributeType.LogMessage).isBoldFontWeight) ? $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}><b>{exception}</b></color>" : $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}>{exception}</color>";

                    string loggedMessage = $"Execption Thrown >>>  <b>[{logExceptionType} Code]</b> --> Insided Class [~{fromClass}~] - From Function : {logFunc} Exception : {log}";
                    throw new EntryPointNotFoundException(loggedMessage);
                }

                if (logExceptionType == AppData.LogExceptionType.NotImplemented)
                {
                    string log = (GetLogAttribute(AppData.LogAttributeType.LogMessage).isBoldFontWeight) ? $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}><b>{exception}</b></color>" : $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}>{exception}</color>";

                    string loggedMessage = $"Execption Thrown >>>  <b>[{logExceptionType} Code]</b> --> Insided Class [~{fromClass}~] - From Function : {logFunc} Exception : {log}";
                    throw new NotImplementedException(loggedMessage);
                }

                if (logExceptionType == AppData.LogExceptionType.MissingField)
                {
                    string log = (GetLogAttribute(AppData.LogAttributeType.LogMessage).isBoldFontWeight) ? $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}><b>{exception}</b></color>" : $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}>{exception}</color>";

                    string loggedMessage = $"Execption Thrown >>>  <b>[{logExceptionType} Code]</b> --> Insided Class [~{fromClass}~] - From Function : {logFunc} Exception : {log}";
                    throw new MissingFieldException(loggedMessage);
                }

                if (logExceptionType == AppData.LogExceptionType.DllNotFound)
                {
                    string log = (GetLogAttribute(AppData.LogAttributeType.LogMessage).isBoldFontWeight) ? $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}><b>{exception}</b></color>" : $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}>{exception}</color>";

                    string loggedMessage = $"Execption Thrown >>>  <b>[{logExceptionType} Code]</b> --> Insided Class [~{fromClass}~] - From Function : {logFunc} Exception : {log}";
                    throw new DllNotFoundException(loggedMessage);
                }

                if (logExceptionType == AppData.LogExceptionType.Aggregate)
                {
                    string log = (GetLogAttribute(AppData.LogAttributeType.LogMessage).isBoldFontWeight) ? $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}><b>{exception}</b></color>" : $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}>{exception}</color>";

                    string loggedMessage = $"Execption Thrown >>>  <b>[{logExceptionType} Code]</b> --> Insided Class [~{fromClass}~] - From Function : {logFunc} Exception : {log}";
                    throw new AggregateException(loggedMessage);
                }

                if (logExceptionType == AppData.LogExceptionType.MissingComponent)
                {
                    string log = (GetLogAttribute(AppData.LogAttributeType.LogMessage).isBoldFontWeight) ? $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}><b>{exception}</b></color>" : $"<color={GetLogAttribute(AppData.LogAttributeType.LogMessage).attributeColorValue}>{exception}</color>";

                    string loggedMessage = $"Execption Thrown >>>  <b>[{logExceptionType} Code]</b> --> Insided Class [~{fromClass}~] - From Function : {logFunc} Exception : {log}";
                    throw new MissingComponentException(loggedMessage);
                }
            }
        }

        #endregion

        #region Specific Exceptions

        #endregion

        #endregion

        #region Log Info Data Accessors

        public AppData.MonoLogInfo GetLogInfo(AppData.LogInfoType logAttribute)
        {
            AppData.MonoLogInfo logInfo = new AppData.MonoLogInfo
            {
                attributeName = "Default",
                logType = AppData.LogInfoType.None,
                logColorValue = "white"
            };

            if (logInfoAttributesList != null && logInfoAttributesList.Count > 0)
            {
                logInfo = logInfoAttributesList.Find(log => log.logType == logAttribute);

                if (!string.IsNullOrEmpty(logInfo.logColorValue) && logInfo.logType == logAttribute)
                    return logInfo;
            }

            return logInfo;
        }


        public AppData.MonoLogAttributes GetLogAttribute(AppData.LogAttributeType attributeType)
        {
            AppData.MonoLogAttributes logAttribute = new AppData.MonoLogAttributes
            {
                attributeName = "Default",
                attributeType = AppData.LogAttributeType.None,
                attributeColorValue = "white"
            };

            if (logAttributesList != null && logAttributesList.Count > 0)
            {
                logAttribute = logAttributesList.Find(log => log.attributeType == attributeType);

                if (!string.IsNullOrEmpty(logAttribute.attributeColorValue) && logAttribute.attributeType == attributeType)
                    return logAttribute;
            }

            return logAttribute;
        }

        #endregion

        #endregion
    }
}