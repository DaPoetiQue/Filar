using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class PublishingManager : AppMonoBaseClass
    {
        #region Static

        private static PublishingManager _instance;

        public static PublishingManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<PublishingManager>();

                return _instance;
            }
        }


        #endregion

        #region Components

        #endregion

        #region Unity Callbacks

        #endregion

        #region Main

        public void Publish(Action<AppData.Callback> callback = null)
        {
            //AppData.Callback callbackResults = new AppData.Callback();

            //if (networkRoutine != null)
            //    StopCoroutine(networkRoutine);

            //networkRoutine = StartCoroutine(CheckNetworkStatus());

            //if (callback != null)
            //    callback.Invoke(callbackResults);
        }

        public async void OnPublish(GameObject obj, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            AppData.Profile profile = new AppData.Profile();

            profile.userName = "Billie";
            profile.userEmail = "Billie@home.com";
            profile.userPassword = "19470302";

            profile.creationDateTime = new AppData.DateTimeComponent(DateTime.Now);

            var serializedMeshDataTaskResults = await AppData.Helpers.GetSerializableMeshDataAsync(obj);

            callbackResults.SetResult(serializedMeshDataTaskResults);

            Log(callbackResults.ResultCode, $" <<<<<<=================>>>>>> On Publish Results : {callbackResults.Result}", this);

            callback?.Invoke(callbackResults);
        }

        #endregion
    }
}
