using System;
using System.Collections;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class UIScreenFolderCreationWidget : AppData.Widget
    {
        #region Components

        Coroutine showWidgetRoutine;

        #endregion

        #region Main

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType, AppData.Widget>>> callback)
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType, AppData.Widget>>();

            Init(initializationCallbackResults =>
            {
                callbackResults.SetResultsData(initializationCallbackResults);
            });

            callback.Invoke(callbackResults);
        }

        protected override AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType, AppData.Widget>> OnGetState()
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType, AppData.Widget>>(AppData.Helpers.GetAppComponentValid(GetStatePacket(), $"{GetName()} - State Object", "Widget State Object Is Null / Not Yet Initialized In The Base Class."));

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

        protected override void OnScreenWidget(Action<AppData.Callback> callback = null)
        {

        }

        protected override void OnScreenWidget(AppData.SceneConfigDataPacket configDataPacket, Action<AppData.Callback> callback = null)
        {
            if (showWidgetRoutine != null)
            {
                StopCoroutine(showWidgetRoutine);
                showWidgetRoutine = null;
            }

            showWidgetRoutine = StartCoroutine(OnShowWidgetAsync());
        }

        IEnumerator OnShowWidgetAsync()
        {
            yield return new WaitForEndOfFrame();

            //AppDatabaseManager.Instance.GetDynamicContainer<DynamicWidgetsContainer>(ScreenUIManager.Instance.GetCurrentUIScreenType(), containerCallbackResults => 
            //{
            //    if (containerCallbackResults.Success())
            //    {
            //        containerCallbackResults.data.GetPlaceHolder(placeHolderCallbackResults =>
            //        {
            //            if (placeHolderCallbackResults.Success())
            //            {
            //                var placeHolderInfo = placeHolderCallbackResults.data.GetInfo();

            //                if (placeHolderInfo.isActive)
            //                {
            //                    SetWidgetPosition(placeHolderInfo.worldPosition);
            //                    SetWidgetSizeDelta(placeHolderInfo.dimensions);
            //                }
            //                else
            //                    LogWarning($"GetPlaceHolder Failed - Placeholder Is Not Active In The Scene.", this);
            //            }
            //            else
            //                Log(placeHolderCallbackResults.resultCode, placeHolderCallbackResults.result, this);
            //        });

            //        AppDatabaseManager.Instance.CreateNewFolderName = AppDatabaseManager.Instance.GetCreateNewFolderTempName();
            //        SetInputFieldValue(AppData.InputFieldActionType.AssetNameField, AppDatabaseManager.Instance.CreateNewFolderName);

            //        HighlightInputFieldValue(AppData.InputFieldActionType.AssetNameField);

            //        ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
            //    }
            //    else
            //        Log(containerCallbackResults.resultCode, containerCallbackResults.result, this);
            //});
        }

        protected override void OnHideScreenWidget(Action<AppData.Callback> callback = null)
        {
            //if (ScreenUIManager.Instance != null)
            //{
            //    if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
            //    {
            //        if (AppDatabaseManager.Instance)
            //        {
            //            AppDatabaseManager.Instance.GetDynamicContainer<DynamicWidgetsContainer>(ScreenUIManager.Instance.GetCurrentUIScreenType(), containerCallbackResults => 
            //            {
            //                if (containerCallbackResults.Success())
            //                {
            //                    containerCallbackResults.data.GetPlaceHolder(placeholderCallbackResults =>
            //                    {
            //                        if (AppData.Helpers.IsSuccessCode(placeholderCallbackResults.resultCode))
            //                        {
            //                            if (placeholderCallbackResults.data.IsActive())
            //                                placeholderCallbackResults.data.ResetPlaceHolder();
            //                            else
            //                                LogWarning("Reset Place Holder Failed - Plave Holder Is Not Active In The Scene.", this);
            //                        }
            //                        else
            //                            Log(placeholderCallbackResults.resultCode, placeholderCallbackResults.result, this);
            //                    });
            //                }
            //                else
            //                    Log(containerCallbackResults.resultCode, containerCallbackResults.result, this);
            //            });
            //        }
            //        else
            //            Debug.LogWarning("--> Get Placeholder Failed : SceneAssetsManager.Instance Is Not Yet Initialized");
            //    }
            //    else
            //        Debug.LogWarning("--> ScreenUIManager.Instance.GetCurrentScreenData Failed : Value Is Missing / Null.");
            //}
            //else
            //    Debug.LogWarning("--> GoToProfile Failed : ScreenUIManager.Instance Is Not Yet Initialized");

           
        }

        protected override void OnInputFieldValueChanged(string value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            AppDatabaseManager.Instance.CreateNewFolderName = value;
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
