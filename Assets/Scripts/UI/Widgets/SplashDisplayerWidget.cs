using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SplashDisplayerWidget : AppData.Widget
    {
        #region Components

        AppData.TransitionableUI transitionable;

        #endregion

        #region Unity Callbacks
        void Start() => Init();

        #endregion

        #region Main

        new void Init()
        {
            splashDisplayerWidget = this;

            AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, databaseManagerCallbackResults =>
            {
                if (databaseManagerCallbackResults.Success())
                {
                    var databaseManager = databaseManagerCallbackResults.data;

                    GetUIImageDisplayerValue(AppData.ScreenImageType.ScreenSnap, imageDisplayerCallbackResults =>
                    {
                        if (imageDisplayerCallbackResults.Success())
                        {
                            var imageDisplayerValue = imageDisplayerCallbackResults.data.value.rectTransform;

                            transitionable = new AppData.TransitionableUI(imageDisplayerValue);
                            transitionable.SetSpeed(databaseManager.GetDefaultExecutionValue(AppData.RuntimeExecution.ScreenWidgetTransitionalSpeed).value);
                        }
                        else
                            Log(imageDisplayerCallbackResults.ResultCode, imageDisplayerCallbackResults.Result, this);
                    });

                    base.Init();
                }
                else
                    Log(databaseManagerCallbackResults.resultCode, databaseManagerCallbackResults.result, this);

            }, "App Database Manager Instance Is Not Yet Initialized.");
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

        protected override void OnScreenWidget()
        {
            
        }

        protected override void OnShowScreenWidget(AppData.SceneDataPackets dataPackets)
        {
            AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, async appDatabaseManagerCallbackResults =>
            {
                if (appDatabaseManagerCallbackResults.Success())
                {
                    var appDatabaseManager = appDatabaseManagerCallbackResults.data;

                    var splashImageURLCallbackResults = await appDatabaseManager.GetRandomSplashImage();

                    if (splashImageURLCallbackResults.Success())
                        SetUIImageDisplayerValue(AppData.ScreenImageType.Splash, splashImageURLCallbackResults.data);
                    else
                        Log(splashImageURLCallbackResults.ResultCode, splashImageURLCallbackResults.Result, this);
                }
                else
                    Log(appDatabaseManagerCallbackResults.ResultCode, appDatabaseManagerCallbackResults.Result, this);
            });

            ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
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