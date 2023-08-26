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
            var sceneAssetsManagerInstanceCallbackResults = AppData.Helpers.GetAppComponentValid(DatabaseManager.Instance, DatabaseManager.Instance.name, "Scene Assets Manager instance Is Not Yet Initialized.");

            if (sceneAssetsManagerInstanceCallbackResults.Success())
            {
                AppData.CallbackData<AppData.SceneDataPackets> loadingScreenDataPacketsCallbackResults = new AppData.CallbackData<AppData.SceneDataPackets>();

                DatabaseManager.Instance.GetDataPacketsLibrary().GetDataPacket(AppData.UIScreenType.LoadingScreen, getLoadingScreenDataPacketsCallbackResults =>
                {
                    loadingScreenDataPacketsCallbackResults.result = getLoadingScreenDataPacketsCallbackResults.Result;
                    loadingScreenDataPacketsCallbackResults.data = getLoadingScreenDataPacketsCallbackResults.Data.dataPackets;
                    loadingScreenDataPacketsCallbackResults.resultCode = getLoadingScreenDataPacketsCallbackResults.ResultCode;
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

                            int loadingScreenExitDelay = AppData.Helpers.ConvertSecondsFromFloatToMillisecondsInt(DatabaseManager.Instance.GetDefaultExecutionValue(AppData.RuntimeExecution.OnScreenChangedExitDelay).value);
                            await Task.Delay(loadingScreenExitDelay);

                            await ScreenUIManager.Instance.ShowScreenAsync(dataPackets);
                        }
                    }
                    else
                        Log(screenUIManagerInstanceCallbackResults.resultCode, screenUIManagerInstanceCallbackResults.result, this);
                }
                else
                    Log(loadingScreenDataPacketsCallbackResults.resultCode, loadingScreenDataPacketsCallbackResults.result, this);
            }
            else
                Log(sceneAssetsManagerInstanceCallbackResults.resultCode, sceneAssetsManagerInstanceCallbackResults.result, this);
        }

        #endregion
    }
}