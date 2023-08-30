using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

            var results = await AppData.Helpers.GetSerializableMeshDataAsync(obj);

            var mesh = results.data.GetMesh();

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();

            GameObject test = new GameObject("Test Object");
            MeshFilter filter = test.AddComponent<MeshFilter>();
            MeshRenderer renderer = test.AddComponent<MeshRenderer>();
            filter.mesh = mesh;

            callbackResults.SetResult(results);

            Log(callbackResults.ResultCode, $" <<<<<<=================>>>>>> On Publish Results : {callbackResults.Result}", this);

            callback?.Invoke(callbackResults);
        }

        #endregion
    }
}
