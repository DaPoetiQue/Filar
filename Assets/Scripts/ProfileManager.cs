using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class ProfileManager : AppMonoBaseClass
    {
        #region Static

        private static ProfileManager _instance;

        public static ProfileManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<ProfileManager>();

                return _instance;
            }
        }

        #endregion

        #region Components

        public AppData.StorageDirectoryData profileStorageData = new AppData.StorageDirectoryData();

        public bool SignedIn { get; private set; }
        public bool HasProfile { get; private set; }

        #endregion

        #region Main

        public void GetUserProfile(Action<AppData.CallbackData<AppData.Profile>> callback)
        {
            AppData.CallbackData<AppData.Profile> callbackResults = new AppData.CallbackData<AppData.Profile>();


            callback.Invoke(callbackResults);
        }

        #endregion
    }
}