using System;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class AppServicesManager : AppData.SingletonBaseComponent<AppServicesManager>
    {
        #region Components

        private AppData.AppInfo entry = new AppData.AppInfo();

        [field: SerializeField]
        public bool AppinfoSynced { get; private set; }

        #endregion

        #region Main

        protected override void Init()
        {

        }

        public AppData.CallbackData<AppData.AppInfo> CreateAppDeviceInfo(AppData.Profile newProfile)
        {
            var callbackResults = new AppData.CallbackData<AppData.AppInfo>(AppData.Helpers.GetAppComponentValid(newProfile, "New Profile", "Create App Device Info Failed - New Profile Parameter Value Is Missing / Null - Invalid Operation."));

            if (callbackResults.Success())
            {
                var appKey = AppData.Helpers.GenerateAppKey(5);
                var activationKey = AppData.Helpers.GenerateUniqueIdentifier(5);
                var deviceInfo = AppData.Helpers.GetDeviceInfo();

                AppData.DateTimeComponent activationStartDate = new AppData.DateTimeComponent(DateTime.UtcNow);

                AppData.LicenseKey newLicense = new AppData.LicenseKey(appKey, activationKey, deviceInfo, AppData.LicenseType.Default, AppData.LicenseStatus.Active, activationStartDate);

                AppData.AppInfo appInfo = new AppData.AppInfo(newProfile, newLicense);

                callbackResults.result = $"CreateAppDeviceInfo Success - App Info Have Been Successfully Created.";
                callbackResults.data = appInfo;
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }


        public void CreateNewAppLicense(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback();

            callback?.Invoke(callbackResults);
        }

        public void SyncAppInfo(AppData.AppInfo appInfo,Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(appInfo, "App info", "Sync App Info Failed - App Info Parameter Value Is Null - Invalid Operation."));

            if (callbackResults.Success())
            {
                entry = appInfo;
                AppinfoSynced = true;
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public void GetAppInfo(Action<AppData.CallbackData<AppData.AppInfo>> callback)
        {
            AppData.CallbackData<AppData.AppInfo> callbackResults = new AppData.CallbackData<AppData.AppInfo>();

            if (AppinfoSynced)
            {
                callbackResults.result = "App Info Is Synced.";
                callbackResults.data = entry;
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = "App Info Is Not Synced.";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }


        #endregion
    }
}