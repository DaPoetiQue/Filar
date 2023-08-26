using System;
using System.Collections;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class PublishingManager : MonoBehaviour
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

        [SerializeField]
        AppData.SceneDataPackets networkInitializationData = new AppData.SceneDataPackets();

        Coroutine networkRoutine;

        #endregion

        #region Unity Callbacks

        #endregion

        #region Main

        public void Publish(Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (networkRoutine != null)
                StopCoroutine(networkRoutine);

            networkRoutine = StartCoroutine(CheckNetworkStatus());

            if (callback != null)
                callback.Invoke(callbackResults);
        }

        IEnumerator CheckNetworkStatus()
        {
            yield return new WaitForSeconds(5.0f);

            Debug.LogError("--> Network Not Found.");

            if (ScreenUIManager.Instance != null)
                ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(networkInitializationData);
            else
                Debug.LogWarning("--> RG_Unity - Check Network Status Failed : Screen UI Manager Instance Is Not Yet Initialized.");
        }

        #endregion
    }
}
