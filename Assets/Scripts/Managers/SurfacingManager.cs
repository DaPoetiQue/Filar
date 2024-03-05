using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SurfacingManager : AppData.SingletonBaseComponent<ProfileManager>
    {
        #region Components

        private List<AppData.Widget> shownPopUpWidgets = new List<AppData.Widget>();

        #endregion

        #region Main

        protected override void Init()
        {

        }

        #region Data Setters

        private void AddShownPopUpWidget(AppData.Widget shownPopUpWidget, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(shownPopUpWidget, "Shown Pop Up Widget", "Set Shown Pop Up Widget Failed - Shown Pop Up Widget Parameter Value Is Null - Invalid Operation."));

            if(callbackResults.Success())
            {
                callbackResults.SetResult(shownPopUpWidget.Initialized());

                if(callbackResults.Success())
                {
                    callbackResults.SetResult(shownPopUpWidget.WidgetReady());

                    if (callbackResults.Success())
                    {
                        if(!shownPopUpWidgets.Contains(shownPopUpWidget))
                        {
                            shownPopUpWidgets.Add(shownPopUpWidget);

                            callbackResults.result = $"Add Shown Pop Up Widget Success - Pop Up : {shownPopUpWidget.GetName()} - Of Type : {shownPopUpWidget.GetType().GetData()} Has Been Successfully Added To Shown Pop Up Widgets.";
                        }
                        else
                        {
                            callbackResults.result = $"Add Shown Pop Up Widget  Failed - Pop Up : {shownPopUpWidget.GetName()} - Of Type : {shownPopUpWidget.GetType().GetData()} Already Exists In Shown Pop Up Widgets - invalid Operation.";
                            callbackResults.resultCode = AppData.Helpers.WarningCode;
                        }
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        private void RemoveShownPopUpWidget(AppData.Widget shownPopUpWidget, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(shownPopUpWidget, "Shown Pop Up Widget", "Set Shown Pop Up Widget Failed - Shown Pop Up Widget Parameter Value Is Null - Invalid Operation."));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(shownPopUpWidget.Initialized());

                if (callbackResults.Success())
                {
                    callbackResults.SetResult(shownPopUpWidget.WidgetReady());

                    if (callbackResults.Success())
                    {
                        if (shownPopUpWidgets.Contains(shownPopUpWidget))
                        {
                            shownPopUpWidgets.Remove(shownPopUpWidget);

                            callbackResults.result = $"Remove Shown Pop Up Widget Success - Pop Up : {shownPopUpWidget.GetName()} - Of Type : {shownPopUpWidget.GetType().GetData()} Has Been Successfully Removed From Shown Pop Up Widgets.";
                        }
                        else
                        {
                            callbackResults.result = $"Remove Shown Pop Up Widget  Failed - Pop Up : {shownPopUpWidget.GetName()} - Of Type : {shownPopUpWidget.GetType().GetData()} Doesn't Exists In Shown Pop Up Widgets - invalid Operation.";
                            callbackResults.resultCode = AppData.Helpers.WarningCode;
                        }
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        #endregion

        #region Data Getters

        public AppData.CallbackData<AppData.Widget> IsWidgetShown(AppData.Widget shownPopUpWidget)
        {
            var callbackResults = new AppData.CallbackData<AppData.Widget>();

            return callbackResults;
        }

        #endregion

        #region Pop Up System

        #region Action Button Listener

        public void ShowPopUp(AppData.WidgetType popUpType, ConfigMessageDataPacket configMessageData, Action<AppData.CallbackData<AppData.SurfacingResults>> callback = null, AppData.ActionButtonListener primaryButton = null, AppData.ActionButtonListener secondaryButton = null, AppData.LogInfoChannel infoChannel = AppData.LogInfoChannel.None)
        {
            var callbackResults = new AppData.CallbackData<AppData.SurfacingResults>(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Show Pop Up Failed - Screen UI Manager Instance Is Not Initialized Yet - Invalid Operation."));

            if (callbackResults.Success())
            {
                var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                if (callbackResults.Success())
                {
                    var screen = screenUIManagerInstance.GetCurrentScreen().GetData();


                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public void ShowPopUp(AppData.Widget popUp, ConfigMessageDataPacket configMessageData, Action<AppData.CallbackData<AppData.SurfacingResults>> callback = null, AppData.ActionButtonListener primaryButton = null, AppData.ActionButtonListener secondaryButton = null, AppData.LogInfoChannel infoChannel = AppData.LogInfoChannel.None)
        {
            var callbackResults = new AppData.CallbackData<AppData.SurfacingResults>(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Show Pop Up Failed - Screen UI Manager Instance Is Not Initialized Yet - Invalid Operation."));

            if (callbackResults.Success())
            {
                var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                if (callbackResults.Success())
                {
                    var screen = screenUIManagerInstance.GetCurrentScreen().GetData();


                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public void ShowPopUp(AppData.SceneConfigDataPacket popUpConfig, ConfigMessageDataPacket configMessageData, Action<AppData.CallbackData<AppData.SurfacingResults>> callback = null, AppData.ActionButtonListener primaryButton = null, AppData.ActionButtonListener secondaryButton = null, AppData.LogInfoChannel infoChannel = AppData.LogInfoChannel.None)
        {
            var callbackResults = new AppData.CallbackData<AppData.SurfacingResults>(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Show Pop Up Failed - Screen UI Manager Instance Is Not Initialized Yet - Invalid Operation."));

            if (callbackResults.Success())
            {
                var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                if (callbackResults.Success())
                {
                    var screen = screenUIManagerInstance.GetCurrentScreen().GetData();

                    callbackResults.SetResult(popUpConfig.GetReferencedWidgetType());

                    if (callbackResults.Success())
                    {
                        callbackResults.SetResult(screen.GetWidget(popUpConfig.GetReferencedWidgetType().GetData().GetValue().GetData()));

                        if (callbackResults.Success())
                        {
                            var widget = screen.GetWidget(popUpConfig.GetReferencedWidgetType().GetData().GetValue().GetData()).GetData();

                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        #endregion

        #region Action Method

        public void ShowPopUp(AppData.WidgetType popUpType, ConfigMessageDataPacket configMessageData, Action<AppData.CallbackData<AppData.SurfacingResults>> callback = null, Action primaryButton = null, Action secondaryButton = null, AppData.LogInfoChannel infoChannel = AppData.LogInfoChannel.None)
        {
            var callbackResults = new AppData.CallbackData<AppData.SurfacingResults>(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Show Pop Up Failed - Screen UI Manager Instance Is Not Initialized Yet - Invalid Operation."));

            if (callbackResults.Success())
            {
                var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                if (callbackResults.Success())
                {
                    var screen = screenUIManagerInstance.GetCurrentScreen().GetData();


                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public void ShowPopUp(AppData.Widget popUp, ConfigMessageDataPacket configMessageData, Action<AppData.CallbackData<AppData.SurfacingResults>> callback = null, Action primaryButton = null, Action secondaryButton = null, AppData.LogInfoChannel infoChannel = AppData.LogInfoChannel.None)
        {
            var callbackResults = new AppData.CallbackData<AppData.SurfacingResults>(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Show Pop Up Failed - Screen UI Manager Instance Is Not Initialized Yet - Invalid Operation."));

            if (callbackResults.Success())
            {
                var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                if (callbackResults.Success())
                {
                    var screen = screenUIManagerInstance.GetCurrentScreen().GetData();


                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public void ShowPopUp(AppData.SceneConfigDataPacket popUpConfig, ConfigMessageDataPacket configMessageData, Action<AppData.CallbackData<AppData.SurfacingResults>> callback = null, Action primaryButton = null, Action secondaryButton = null, AppData.LogInfoChannel infoChannel = AppData.LogInfoChannel.None)
        {
            var callbackResults = new AppData.CallbackData<AppData.SurfacingResults>(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Show Pop Up Failed - Screen UI Manager Instance Is Not Initialized Yet - Invalid Operation."));

            if (callbackResults.Success())
            {
                var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                if (callbackResults.Success())
                {
                    var screen = screenUIManagerInstance.GetCurrentScreen().GetData();

                    callbackResults.SetResult(popUpConfig.GetReferencedWidgetType());

                    if (callbackResults.Success())
                    {
                        callbackResults.SetResult(screen.GetWidget(popUpConfig.GetReferencedWidgetType().GetData().GetValue().GetData()));

                        if (callbackResults.Success())
                        {
                            var widget = screen.GetWidget(popUpConfig.GetReferencedWidgetType().GetData().GetValue().GetData()).GetData();

                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        #endregion

        #region Action Message Types

        public void ShowPopUp(AppData.WidgetType popUpType, AppData.ConfigMessageType messageType, Action<AppData.CallbackData<AppData.SurfacingResults>> callback = null, (string buttonTitle, Action method) primaryButtonEvent = default, (string buttonTitle, Action method) secondaryButtonEvent = default, params string[] messageOverrides)
        {
            var callbackResults = new AppData.CallbackData<AppData.SurfacingResults>(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Show Pop Up Failed - Screen UI Manager Instance Is Not Initialized Yet - Invalid Operation."));

            if(callbackResults.Success())
            {
                var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                if (callbackResults.Success())
                {
                    var screen = screenUIManagerInstance.GetCurrentScreen().GetData();

                    callbackResults.SetResult(screen.GetWidget(popUpType));

                    if (callbackResults.Success())
                    {
                        var popUpWidget = screen.GetWidget(popUpType).GetData();

                        callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance", "On Tab View Shown Event Failed - App Database Manager Instance Is Not Yet Initialized - Invalid Operation."));

                        if (callbackResults.Success())
                        {
                            var appDatabaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance").GetData();

                            callbackResults.SetResult(appDatabaseManagerInstance.GetAssetBundlesLibrary());

                            if (callbackResults.Success())
                            {
                                var assetBundlesLibrary = appDatabaseManagerInstance.GetAssetBundlesLibrary().GetData();

                                callbackResults.SetResult(assetBundlesLibrary.GetLoadedConfigMessageDataPacket(messageType));

                                if (callbackResults.Success())
                                {
                                    var messageDataObject = assetBundlesLibrary.GetLoadedConfigMessageDataPacket(messageType).GetData();

                                    callbackResults.SetResult(messageDataObject.GetTitle());

                                    popUpWidget.SetUITextDisplayerValue(AppData.ScreenTextType.TitleDisplayer, messageDataObject.GetTitle().GetData(), widgetTitleSetCallbackResults => 
                                    {
                                        callbackResults.SetResult(widgetTitleSetCallbackResults);

                                        if (callbackResults.Success())
                                        {
                                            callbackResults.SetResult(messageDataObject.GetMessage());

                                            if (callbackResults.Success())
                                            {
                                                var popUpMessage = messageDataObject.GetMessage(messageOverrides).GetData();

                                                popUpWidget.SetUITextDisplayerValue(AppData.ScreenTextType.MessageDisplayer, popUpMessage, widgetTitleSetCallbackResults =>
                                                {
                                                    callbackResults.SetResult(widgetTitleSetCallbackResults);

                                                    if (callbackResults.Success())
                                                    {
                                                        var primaryButtonReference = primaryButtonEvent.method != null ? new AppData.ActionButtonListener()
                                                        {
                                                            method = primaryButtonEvent.method,
                                                            action = AppData.InputActionButtonType.ConfirmationButton

                                                        } : null;

                                                        var secondaryButtonReference = secondaryButtonEvent.method != null ? new AppData.ActionButtonListener
                                                        {
                                                            method = secondaryButtonEvent.method,
                                                            action = AppData.InputActionButtonType.Cancel

                                                        } : null;

                                                        popUpWidget.RegisterActionButtonListeners(actionButtonEventsRegisteredCallbackResults =>
                                                        {
                                                            callbackResults.SetResult(actionButtonEventsRegisteredCallbackResults);

                                                            if (callbackResults.Success())
                                                            {
                                                                callbackResults.SetResult(primaryButtonReference.Initialized());

                                                                if (callbackResults.Success())
                                                                {
                                                                    callbackResults.SetResult(AppData.Helpers.GetAppStringValueNotNullOrEmpty(primaryButtonEvent.buttonTitle, "Button Title", "Show Pop Up Set button Title Unsuccessful - Continuing Execution."));

                                                                    if (callbackResults.Success())
                                                                    {
                                                                        popUpWidget.SetActionButtonTitle(AppData.InputActionButtonType.ConfirmationButton, primaryButtonEvent.buttonTitle, buttonTitleUpdatedCallbackResults => 
                                                                        {
                                                                            callbackResults.SetResult(buttonTitleUpdatedCallbackResults);
                                                                        });
                                                                    }
                                                                    else
                                                                    {
                                                                        callbackResults.result = $"There Are No Action Events Registered - Results Code : {callbackResults.GetResultCode} - Results : {callbackResults.GetResult} - Continuing Execution.";
                                                                        callbackResults.resultCode = AppData.Helpers.SuccessCode;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    callbackResults.result = $"There Are No Action Events Registered - Results Code : {callbackResults.GetResultCode} - Results : {callbackResults.GetResult} - Continuing Execution.";
                                                                    callbackResults.resultCode = AppData.Helpers.SuccessCode;
                                                                }

                                                                callbackResults.SetResult(secondaryButtonReference.Initialized());

                                                                if (callbackResults.Success())
                                                                {
                                                                    callbackResults.SetResult(AppData.Helpers.GetAppStringValueNotNullOrEmpty(secondaryButtonEvent.buttonTitle, "Button Title", "Show Pop Up Set button Title Unsuccessful - Continuing Execution."));

                                                                    if (callbackResults.Success())
                                                                    {
                                                                        popUpWidget.SetActionButtonTitle(AppData.InputActionButtonType.Cancel, secondaryButtonEvent.buttonTitle, buttonTitleUpdatedCallbackResults =>
                                                                        {
                                                                            callbackResults.SetResult(buttonTitleUpdatedCallbackResults);
                                                                        });
                                                                    }
                                                                    else
                                                                    {
                                                                        callbackResults.result = $"There Are No Action Events Registered - Results Code : {callbackResults.GetResultCode} - Results : {callbackResults.GetResult} - Continuing Execution.";
                                                                        callbackResults.resultCode = AppData.Helpers.SuccessCode;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    callbackResults.result = $"There Are No Action Events Registered - Results Code : {callbackResults.GetResultCode} - Results : {callbackResults.GetResult} - Continuing Execution.";
                                                                    callbackResults.resultCode = AppData.Helpers.SuccessCode;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                callbackResults.result = $"There Are No Action Events Registered - Results Code : {callbackResults.GetResultCode} - Results : {callbackResults.GetResult} - Continuing Execution.";
                                                                callbackResults.resultCode = AppData.Helpers.SuccessCode;
                                                            }

                                                        }, primaryButtonReference, secondaryButtonReference);

                                                        if(callbackResults.Success())
                                                        {
                                                            screen.ShowWidget(popUpWidget, popUpShownCallbackResults => 
                                                            {
                                                                callbackResults.SetResult(popUpShownCallbackResults);

                                                                if (callbackResults.Success())
                                                                {
                                                                    AddShownPopUpWidget(popUpWidget, popUpShownSetCallbackResults =>
                                                                    {
                                                                        callbackResults.SetResult(popUpShownSetCallbackResults);
                                                                    });
                                                                }
                                                                else
                                                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                            });
                                                        }
                                                        else
                                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                    }
                                                    else
                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                });
                                            }
                                            else
                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                        }
                                        else
                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                    });
                                }
                                else
                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public void ShowPopUp(AppData.Widget popUp, AppData.ConfigMessageType messageType, Action<AppData.CallbackData<AppData.SurfacingResults>> callback = null, Action primaryButton = null, Action secondaryButton = null, AppData.LogInfoChannel infoChannel = AppData.LogInfoChannel.None)
        {
            var callbackResults = new AppData.CallbackData<AppData.SurfacingResults>(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Show Pop Up Failed - Screen UI Manager Instance Is Not Initialized Yet - Invalid Operation."));

            if (callbackResults.Success())
            {
                var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                if (callbackResults.Success())
                {
                    var screen = screenUIManagerInstance.GetCurrentScreen().GetData();


                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public void ShowPopUp(AppData.SceneConfigDataPacket popUpConfig, AppData.ConfigMessageType messageType, Action<AppData.CallbackData<AppData.SurfacingResults>> callback = null, Action primaryButton = null, Action secondaryButton = null, AppData.LogInfoChannel infoChannel = AppData.LogInfoChannel.None)
        {
            var callbackResults = new AppData.CallbackData<AppData.SurfacingResults>(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Show Pop Up Failed - Screen UI Manager Instance Is Not Initialized Yet - Invalid Operation."));

            if (callbackResults.Success())
            {
                var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                if (callbackResults.Success())
                {
                    var screen = screenUIManagerInstance.GetCurrentScreen().GetData();

                    callbackResults.SetResult(popUpConfig.GetReferencedWidgetType());

                    if (callbackResults.Success())
                    {
                        callbackResults.SetResult(screen.GetWidget(popUpConfig.GetReferencedWidgetType().GetData().GetValue().GetData()));

                        if (callbackResults.Success())
                        {
                            var widget = screen.GetWidget(popUpConfig.GetReferencedWidgetType().GetData().GetValue().GetData()).GetData();

                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        #endregion

        public void HidePopUp(AppData.WidgetType popUpType, Action<AppData.CallbackData<AppData.SurfacingResults>> callback = null, AppData.ScreenBlurConfig blurConfig = null)
        {
            var callbackResults = new AppData.CallbackData<AppData.SurfacingResults>(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Show Pop Up Failed - Screen UI Manager Instance Is Not Initialized Yet - Invalid Operation."));

            if (callbackResults.Success())
            {
                var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                if (callbackResults.Success())
                {
                    var screen = screenUIManagerInstance.GetCurrentScreen().GetData();

                    callbackResults.SetResult(screen.GetWidget(popUpType));

                    if(callbackResults.Success())
                    {
                        var widget = screen.GetWidget(popUpType).GetData();

                        widget.UnRegisterActionButtonListeners(actionEventsUnsubscribedCallbackResults => 
                        {
                            callbackResults.SetResult(actionEventsUnsubscribedCallbackResults);

                            if (callbackResults.Success())
                            {
                                screen.HideWidget(popUpType, popUpHiddenCallbackResults =>
                                {
                                    callbackResults.SetResult(popUpHiddenCallbackResults);

                                    if (callbackResults.Success())
                                    {
                                        RemoveShownPopUpWidget(widget, shownPopUpWidgetRemovedCallbackResults => 
                                        {
                                            callbackResults.SetResult(shownPopUpWidgetRemovedCallbackResults);
                                        });
                                    }
                                    else
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                }, blurConfig);
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                        });
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        #endregion

        #region Surfacing Data

        private AppData.CallbackData<AppData.SurfacingData> GetSurfacingData()
        {
            var callbackResults = new AppData.CallbackData<AppData.SurfacingData>();

            return callbackResults;
        }

        #endregion

        #endregion
    }
}