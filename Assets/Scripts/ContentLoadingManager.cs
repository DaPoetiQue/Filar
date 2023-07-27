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

        public async Task LoadScreen(AppData.SceneDataPackets dataPackets)
        {
            var sceneAssetsManagerInstanceCallbackResults = AppData.Helpers.GetAppComponentValid(SceneAssetsManager.Instance, SceneAssetsManager.Instance.name, "Scene Assets Manager instance Is Not Yet Initialized.");

            if (sceneAssetsManagerInstanceCallbackResults.Success())
            {
                AppData.CallbackData<AppData.SceneDataPackets> loadingScreenDataPacketsCallbackResults = new AppData.CallbackData<AppData.SceneDataPackets>();

                SceneAssetsManager.Instance.GetDataPacketsLibrary().GetDataPacket(AppData.UIScreenType.LoadingScreen, getLoadingScreenDataPacketsCallbackResults =>
                {
                    loadingScreenDataPacketsCallbackResults.results = getLoadingScreenDataPacketsCallbackResults.results;
                    loadingScreenDataPacketsCallbackResults.data = getLoadingScreenDataPacketsCallbackResults.data.dataPackets;
                    loadingScreenDataPacketsCallbackResults.resultsCode = getLoadingScreenDataPacketsCallbackResults.resultsCode;
                });

                if (loadingScreenDataPacketsCallbackResults.Success())
                {
                    var screenUIManagerInstanceCallbackResults = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, ScreenUIManager.Instance.name, "Screen UI Manager instance Is Not Yet Initialized.");

                    if (screenUIManagerInstanceCallbackResults.Success())
                    {
                        await ScreenUIManager.Instance.ShowScreenAsync(loadingScreenDataPacketsCallbackResults.data);

                        loadingDuration = tempLoadDuration;

                        while (loadingDuration > 0.0f)
                        {
                            loadingDuration -= 1.0f * 5 * Time.deltaTime;
                            await Task.Yield();
                        }

                        if (loadingDuration <= 0)
                        {
                            await ScreenUIManager.Instance.HideScreenAsync(loadingScreenDataPacketsCallbackResults.data);

                            int loadingScreenExitDelay = AppData.Helpers.ConvertSecondsFromFloatToMillisecondsInt(SceneAssetsManager.Instance.GetDefaultExecutionValue(AppData.RuntimeValueType.OnScreenChangedExitDelay).value);
                            await Task.Delay(loadingScreenExitDelay);

                            await ScreenUIManager.Instance.ShowScreenAsync(dataPackets);
                        }
                    }
                    else
                        Log(screenUIManagerInstanceCallbackResults.resultsCode, screenUIManagerInstanceCallbackResults.results, this);
                }
                else
                    Log(loadingScreenDataPacketsCallbackResults.resultsCode, loadingScreenDataPacketsCallbackResults.results, this);
            }
            else
                Log(sceneAssetsManagerInstanceCallbackResults.resultsCode, sceneAssetsManagerInstanceCallbackResults.results, this);
        }

        #endregion
    }
}