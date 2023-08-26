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

        //[SerializeField]
        //AppData.SceneDataPackets networkInitializationData = new AppData.SceneDataPackets();

        //Coroutine networkRoutine;

        [SerializeField]
        GameObject assetToPublish = null;

        #endregion

        #region Unity Callbacks

        private void Start()
        {
            
        }

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

        //private async Task<AppData.Callback> CreatingSerializableMeshDataAsync()
        //{

        //}

        public void PublishTest(GameObject asset)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            AppData.Profile profile = new AppData.Profile();

            profile.userName = "Billie";
            profile.userEmail = "Billie@home.com";
            profile.userPassword = "19470302";

            profile.creationDateTime = new AppData.DateTimeComponent(DateTime.Now);

            var content = AppData.Helpers.GetSerializableMeshData(asset);

            LogSuccess($" <<<<<<=================>>>>>> A New Serializable Mesh Data Has Been Created With : {content.vertices.Length} Vertices : {content.triangles.Length} Triangles : {content.normals.Length} Normals : {content.uvs.Length} UVs And : {content.tangents.Length} Tangengs.", this);
        }

        #endregion
    }
}
