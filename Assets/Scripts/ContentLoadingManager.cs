using System;
using System.Collections;
using System.Threading.Tasks;
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

        float loadingDuration = 0.0f;

        #endregion

        #region Main

        public void LoadScreen(AppData.SceneDataPackets dataPackets)
        {
            loadingDuration = tempLoadDuration;

            StartCoroutine(loadScreenAsync(screenLoadedCallbackResults => 
            {
                if (screenLoadedCallbackResults.Success())
                {
                    AppData.Helpers.GetComponent(ScreenUIManager.Instance, validComponentCallback => 
                    {
                        if (validComponentCallback.Success())
                        {
                            SceneAssetsManager.Instance.GetDataPacketsLibrary().GetDataPacket(AppData.UIScreenType.LoadingScreen, async loadingScreenDataPacketsCallbackResults => 
                            {
                                if (loadingScreenDataPacketsCallbackResults.Success())
                                {
                                    await ScreenUIManager.Instance.HideScreenAsync(loadingScreenDataPacketsCallbackResults.data.dataPackets);

                                    int loadingScreenExitDelay = AppData.Helpers.ConvertSecondsFromFloatToMillisecondsInt(SceneAssetsManager.Instance.GetDefaultExecutionValue(AppData.RuntimeValueType.OnScreenChangedExitDelay).value);
                                    await Task.Delay(loadingScreenExitDelay);

                                    await ScreenUIManager.Instance.ShowScreenAsync(dataPackets);
                                }
                                else
                                    Log(loadingScreenDataPacketsCallbackResults.resultsCode, loadingScreenDataPacketsCallbackResults.results, this);
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

            while (loadingDuration > 0)
            {
                loadingDuration -= 1.0f;

                if (loadingDuration <= 0)
                    break;

                yield return null;
            }

            if(loadingDuration <= 0)
            {
                callbackResults.results = "Loading Completed.";
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = "Loading Failed.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);

        }


        #endregion
    }
}