using System;
using System.Collections;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class ContentLoadingManager : AppMonoBaseClass
    {
        #region Static

        private static ContentLoadingManager _instance;

        public static ContentLoadingManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<ContentLoadingManager>();

                return _instance;
            }
        }

        #endregion

        #region Components

        [SerializeField]
        float tempLoadDuration = 3.0f;

        #endregion

        #region Main

        public void LoadScreen(AppData.SceneDataPackets dataPackets)
        {
            StartCoroutine(loadScreenAsync(screenLoadedCallbackResults => 
            {
                if (screenLoadedCallbackResults.Success())
                {
                    AppData.Helpers.GetComponent(ScreenUIManager.Instance, validComponentCallback => 
                    {
                        if (validComponentCallback.Success())
                        {
                            SceneAssetsManager.Instance.GetDataPacketsLibrary().GetDataPacket(AppData.UIScreenType.LoadingScreen, dataPacketsCallbackResults => 
                            {
                                if (dataPacketsCallbackResults.Success())
                                {
                                    dataPackets.screenTransition = dataPacketsCallbackResults.data.dataPackets.screenTransition;
                                    ScreenUIManager.Instance.ShowScreen(dataPackets);
                                }
                                else
                                    Log(dataPacketsCallbackResults.resultsCode, dataPacketsCallbackResults.results, this);
                            });
                        }
                        else
                            Log(validComponentCallback.resultsCode, "Screen UI Manager Is Not Yet Initialized.", this);
                    });
                }
                else
                    Log(screenLoadedCallbackResults.resultsCode, screenLoadedCallbackResults.results, this);
            }));
        }

        IEnumerator loadScreenAsync(Action<AppData.Callback> callback)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            yield return new WaitForEndOfFrame();

            var duration = tempLoadDuration;

            while (duration > 0)
            {
                duration -= 1.0f * Time.deltaTime;
                yield return null;
            }

            if(duration <= 0)
            {
                callbackResults.results = "Loading Completed.";
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = "Loading Failed.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            yield return new WaitForEndOfFrame();

            callback.Invoke(callbackResults);

        }


        #endregion
    }
}