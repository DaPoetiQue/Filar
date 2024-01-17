using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class RenderSettingsWidget : AppData.Widget
    {
        #region Components

        [SerializeField]
        AppData.NavigationWidget navigationWidget = new AppData.NavigationWidget();

        bool widgetsInitialized = false;

        #endregion

        #region Main

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType>>> callback)
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType>>();

            Init(initializationCallbackResults =>
            {
                callbackResults.SetResultsData(initializationCallbackResults);
            });

            callback.Invoke(callbackResults);
        }

        protected override AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType>> OnGetState()
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType>>(AppData.Helpers.GetAppComponentValid(GetStatePacket(), $"{GetName()} - State Object", "Widget State Object Is Null / Not Yet Initialized In The Base Class."));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(GetType());

                if (callbackResults.Success())
                {
                    var widgetType = GetType().data;

                    callbackResults.SetResult(GetStatePacket().Initialized(widgetType));

                    if (callbackResults.Success())
                    {
                        callbackResults.result = $"Widget : {GetStatePacket().GetName()} Of Type : {GetStatePacket().GetType()} State Is Set To : {GetStatePacket().GetStateType()}";
                        callbackResults.data = GetStatePacket();
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        void RegisterEventListeners(bool register)
        {
            if (register)
            {
                AppData.ActionEvents._OnNavigationSubTabChangedEvent += ActionEvents__OnNavigationSubTabChangedEvent;
                AppData.ActionEvents._OnNavigationTabWidgetEvent += ActionEvents__OnNavigationTabWidgetEvent;
            }
            else
            {
                AppData.ActionEvents._OnNavigationSubTabChangedEvent -= ActionEvents__OnNavigationSubTabChangedEvent;
                AppData.ActionEvents._OnNavigationTabWidgetEvent -= ActionEvents__OnNavigationTabWidgetEvent;
            }
        }

        private void ActionEvents__OnNavigationSubTabChangedEvent(AppData.NavigationTabID tabID, AppData.NavigationRenderSettingsProfileID navigationSelection)
        {
            Debug.LogError($"--> Navigation Canged For Tab : {tabID} With Selection : {navigationSelection}");

            if (navigationWidget != null)
            {
                AppData.NavigationTab subWidget = navigationWidget.navigationTabsList.Find((x) => x.navigationID == tabID);

                if (subWidget != null)
                {
                    if (subWidget.navigationSubWidget != null)
                        subWidget.navigationSubWidget.OnNavigationTabChangedEvent(navigationSelection);
                }
                else
                    Debug.LogWarning($"--> RG_Unity - ActionEvents__OnCreateNewRenderProfileEvent Failed : Sub Widget Not Found For : {tabID}");
            }
        }

        private void ActionEvents__OnNavigationTabWidgetEvent(AppData.ButtonConfigDataPacket dataPackets)
        {
            if (navigationWidget != null)
            {
                AppData.NavigationTab subWidget = navigationWidget.navigationTabsList.Find((x) => x.navigationID == dataPackets.tabID);

                if (subWidget != null)
                {
                    switch (dataPackets.navigationWidgetVisibilityState)
                    {
                        case AppData.NavigationWidgetVisibilityState.Show:

                            subWidget.ShowWidget(dataPackets.navigationTabWidgetType, (callback) =>
                            {
                                if (!AppData.Helpers.IsSuccessCode(callback.resultCode))
                                    Debug.LogError($"------> Failed To Show Tab Widget : {dataPackets.navigationTabWidgetType} With Error Result : {callback.result}");

                            });

                            break;

                        case AppData.NavigationWidgetVisibilityState.Hide:

                            subWidget.HideWidget(dataPackets.navigationTabWidgetType, (callback) =>
                            {
                                if (!AppData.Helpers.IsSuccessCode(callback.resultCode))
                                    Debug.LogError($"------> Failed : {dataPackets.navigationTabWidgetType} With Error Result : {callback.result}");

                            });

                            break;
                    }
                }
                else
                    Debug.LogWarning($"--> RG_Unity - ActionEvents__OnCreateNewRenderProfileEvent Failed : Sub Widget Not Found For : {dataPackets.navigationTabWidgetType}");
            }
        }

        protected override void OnScreenWidget(Action<AppData.Callback> callback = null)
        {

        }

        protected override void OnScreenWidget(AppData.SceneConfigDataPacket configDataPacket, Action<AppData.Callback> callback = null)
        {

        }


        void SetWidgetAssetData(AppData.SceneAsset asset)
        {
            //AppData.Helpers.ShowImage(asset, thumbnailDisplayer);

            if (titleDisplayer != null && !string.IsNullOrEmpty(asset.name))
                titleDisplayer.text = asset.name;
        }

        //protected override void OnShowScreenWidget(Action<AppData.Callback> callback = null)
        //{
        //    ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);

        //    if (!widgetsInitialized)
        //    {
        //        //InititializeWidget();
        //        widgetsInitialized = true;
        //    }

        //    if (AppDatabaseManager.Instance)
        //        SetWidgetAssetData(AppDatabaseManager.Instance.GetCurrentSceneAsset());
        //    else
        //        Debug.LogWarning("--> Scene Assets Manager Not Yet Initialized.");
        //}

        protected override void OnHideScreenWidget(Action<AppData.Callback> callback = null)
        {
          
        }

        protected override void OnInputFieldValueChanged(string value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInputFieldValueChanged(int value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScrollerValueChanged(Vector2 value)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionButtonEvent(AppData.WidgetType popUpType, AppData.InputActionButtonType actionType, AppData.SceneConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void ScrollerPosition(Vector2 position)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScreenWidget<T>(AppData.ScriptableConfigDataPacket<T> scriptableConfigData, Action<AppData.Callback> callback = null)
        {
            throw new NotImplementedException();
        }

        protected override void OnScreenWidgetShownEvent()
        {
            throw new NotImplementedException();
        }

        protected override void OnScreenWidgetHiddenEvent()
        {
            throw new NotImplementedException();
        }

        protected override void OnScreenWidgetTransitionInProgressEvent()
        {
            throw new NotImplementedException();
        }

        protected override void OnActionButtonInputs(AppData.UIButton<AppData.ButtonConfigDataPacket> actionButton)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}