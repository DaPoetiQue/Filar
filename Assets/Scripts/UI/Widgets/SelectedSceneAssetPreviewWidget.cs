using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

namespace Com.RedicalGames.Filar
{
    public class SelectedSceneAssetPreviewWidget : AppData.Widget
    {
        #region Components

        [SerializeField]
        Image thumbnailDisplayer;

        [Space(5)]
        [SerializeField]
        TMP_Text descriptionDisplayer;

        [Space(5)]
        [SerializeField]
        List<AppData.ScreenTogglableWidget<GameObject>> screenTogglableWidgetsList = new List<AppData.ScreenTogglableWidget<GameObject>>();

        AppData.SceneAsset sceneAsset;

        bool canResetAssetPose = false;

        #endregion

        #region Unity Callbacks

        void OnEnable() => OnSubscribeToEvents(true);

        void OnDisable() => OnSubscribeToEvents(false);

        #endregion

        #region Initializations

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

        void OnSubscribeToEvents(bool subscribe)
        {
            if (subscribe)
            {
                AppData.ActionEvents._OnScreenTogglableStateEvent += OnScreenTogglableStateEvent;
                AppData.ActionEvents._OnSceneModelPoseResetEvent += OnAssetPoseReset;
            }
            else
            {
                AppData.ActionEvents._OnScreenTogglableStateEvent -= OnScreenTogglableStateEvent;
                AppData.ActionEvents._OnSceneModelPoseResetEvent -= OnAssetPoseReset;
            }
        }


        #endregion

        #region Main

        void SetWidgetAssetData(AppData.SceneAsset asset)
        {
            AppData.Helpers.ShowImage(asset, thumbnailDisplayer);

            if (titleDisplayer != null && !string.IsNullOrEmpty(asset.name))
                titleDisplayer.text = asset.name;

            if (descriptionDisplayer != null && !string.IsNullOrEmpty(asset.description))
                descriptionDisplayer.text = asset.description;

            if (SelectableManager.Instance != null)
            {
                SelectableManager.Instance.Select(asset, AppData.FocusedSelectionType.InteractedItem, selectionCallback =>
                {
                    if (AppData.Helpers.IsSuccessCode(selectionCallback.resultCode))
                    {
                        LogSuccess(selectionCallback.result, this, () => OnHideScreenWidget());
                    }
                    else
                        LogError(selectionCallback.result, this, () => OnHideScreenWidget());
                });
            }
            else
                LogError("Selectable Manager Instance Is Not Yet Initialized.", this, () => OnHideScreenWidget());
        }

        void OnScreenTogglableStateEvent(AppData.TogglableWidgetType widgetType, bool state = false, bool useInteractability = false)
        {
            if (!state)
            {
                AppData.ScreenTogglableWidget<GameObject> screenTogglableWidget = screenTogglableWidgetsList.Find((x) => x.widgetType == widgetType);

                if (screenTogglableWidget.value != null)
                {
                    if (useInteractability)
                    {
                        screenTogglableWidget.Interactable(state);
                    }
                    else
                    {
                        if (state)
                            screenTogglableWidget.Show();
                        else
                            screenTogglableWidget.Hide();
                    }
                }
                else
                    LogWarning("Screen Togglable Widget Value Is Null.", this, () => OnScreenTogglableStateEvent(widgetType, state = false, useInteractability = false));

                canResetAssetPose = false;
            }
            else
            {
                if (!canResetAssetPose)
                {
                    AppData.ScreenTogglableWidget<GameObject> screenTogglableWidget = screenTogglableWidgetsList.Find((x) => x.widgetType == widgetType);

                    if (screenTogglableWidget.value != null)
                    {
                        if (useInteractability)
                            screenTogglableWidget.Interactable(state);
                        else
                        {
                            if (state)
                                screenTogglableWidget.Show();
                            else
                                screenTogglableWidget.Hide();
                        }

                        canResetAssetPose = true;
                    }
                    else
                        Debug.LogError("--> Screen Togglable Widget Value Is Null.");
                }
            }
        }

        void OnAssetPoseReset()
        {
            OnScreenTogglableStateEvent(AppData.TogglableWidgetType.ResetAssetModelRotationButton, false);
            canResetAssetPose = false;
        }


        #region Overrides

        protected override void OnScreenWidget(AppData.SceneConfigDataPacket configDataPacket, Action<AppData.Callback> callback = null)
        {
          
        }

        //protected override void OnShowScreenWidget(Action<AppData.Callback> callback = null)
        //{
        //    ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);

        //    switch (transitionType)
        //    {
        //        case AppData.TransitionType.Default:

        //            if (GetLayoutView(defaultLayoutType).Success())
        //                OnScreenTogglableStateEvent(AppData.TogglableWidgetType.ResetAssetModelRotationButton, false);
        //            else
        //                LogError("Pop Up Value Required.", this);

        //            break;

        //        case AppData.TransitionType.Translate:

        //            break;
        //    }

        //    if (AppDatabaseManager.Instance)
        //        SetWidgetAssetData(AppDatabaseManager.Instance.GetCurrentSceneAsset());
        //    else
        //        LogWarning("Scene Assets Manager Not Yet Initialized.", this);
        //}

        protected override void OnHideScreenWidget(Action<AppData.Callback> callback = null)
        {
            //await ScreenUIManager.Instance.RefreshAsync();
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

        protected override void OnScreenWidget(Action<AppData.Callback> callback = null)
        {
           
        }

        protected override void OnActionButtonInputs(AppData.UIButton<AppData.ButtonConfigDataPacket> actionButton)
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
    }
}
