using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class NetworkNotificationWidget : AppData.Widget
    {
        #region Components

        #endregion

        #region Unity Callbacks
        void Start() => Init();

        #endregion

        #region Main

        new void Init()
        {
            networkNotificationWidget = this;
            base.Init();
        }

        protected override void OnScreenWidget()
        {

        }

        void SetWidgetAssetData(AppData.SceneAsset asset)
        {
            if (titleDisplayer != null && !string.IsNullOrEmpty(asset.name))
                titleDisplayer.text = asset.name;
        }

        protected override void OnShowScreenWidget(AppData.SceneDataPackets dataPackets)
        {
            ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);

            if (SceneAssetsManager.Instance)
                SetWidgetAssetData(SceneAssetsManager.Instance.GetCurrentSceneAsset());
            else
                Debug.LogWarning("--> Scene Assets Manager Not Yet Initialized.");
        }

        protected override void OnHideScreenWidget()
        {
            HideSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
        }

        protected override void OnInputFieldValueChanged(string value, AppData.InputFieldDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInputFieldValueChanged(int value, AppData.InputFieldDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnSubscribeToActionEvents(bool subscribe)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScrollerValueChanged(Vector2 value)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionButtonEvent(AppData.WidgetType popUpType, AppData.InputActionButtonType actionType, AppData.SceneDataPackets dataPackets)
        {
            if (actionType != AppData.InputActionButtonType.RetryButton)
                return;

            AppData.Callback callbackResults = new AppData.Callback();

            AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, ScreenUIManager.Instance.name, hasScreenUIManagerCallbackResults => 
            {
                callbackResults.SetResult(hasScreenUIManagerCallbackResults);

                if (callbackResults.Success())
                {
                    var screenUIManager = hasScreenUIManagerCallbackResults.data;

                    switch (screenUIManager.GetCurrentUIScreenType())
                    {
                        case AppData.UIScreenType.LoadingScreen:

                            screenUIManager.GetCurrentScreenData().value.HideScreenWidget(this);

                            AppData.SceneDataPackets loadingStateWidgetDataPackets = new AppData.SceneDataPackets
                            {
                                screenType = screenUIManager.GetCurrentUIScreenType(),
                                widgetType = AppData.WidgetType.LoadingWidget
                            };

                            AppData.Helpers.GetAppComponentValid(LoadingManager.Instance, LoadingManager.Instance.name, async hasLoadingManagerCallbackResults =>
                            {
                                callbackResults.SetResult(hasLoadingManagerCallbackResults);

                                if (callbackResults.Success())
                                {
                                    var loadingManager = hasLoadingManagerCallbackResults.data;

                                    if (loadingManager.OnInitialLoad)
                                    {
                                        screenUIManager.GetCurrentScreenData().value.ShowWidget(loadingStateWidgetDataPackets);
                                        await loadingManager.GetLoadingSequence().ProcessQueueSequenceAsync();
                                    }
                                    else
                                        LogWarning("Please Do Network Retry Here.", this);
                                }

                            }, "Loading Manager Instance Is Not Yet Initialized.");

                            break;
                    }
                }
                else
                    Log(callbackResults.ResultCode, callbackResults.Result, this);

            }, "Screen UI Manager Instance Is Not Yet Initialized.");
        }
        
        async void Retry()
        {
         
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void ScrollerPosition(Vector2 position)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
