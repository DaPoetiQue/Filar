using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SurfacingManager : AppData.SingletonBaseComponent<SurfacingManager>
    {
        #region Components

        [Space(10)]
        [Header("Surfacing Templates")]

        [SerializeField]
        private AppData.SurfacingTemplateLibrary surfacingTemplateLibrary = new AppData.SurfacingTemplateLibrary();

        [Space(10)]
        [Header("Surfaced Widgets")]

        [Space(5)]
        [SerializeField]
        private List<AppData.Widget> surfacedWidgetList = new List<AppData.Widget>();

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
                        if(!surfacedWidgetList.Contains(shownPopUpWidget))
                        {
                            surfacedWidgetList.Add(shownPopUpWidget);

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
                        if (surfacedWidgetList.Contains(shownPopUpWidget))
                        {
                            surfacedWidgetList.Remove(shownPopUpWidget);

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

        #region Surface Widgets

        public void SurfaceWidget(AppData.WidgetType widgetType, Action<AppData.CallbackData<AppData.SurfacingResults>> callback = null)
        {
            var callbackResults = new AppData.CallbackData<AppData.SurfacingResults>(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Show Pop Up Failed - Screen UI Manager Instance Is Not Initialized Yet - Invalid Operation."));

            if (callbackResults.Success())
            {
                var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                if (callbackResults.Success())
                {
                    var screen = screenUIManagerInstance.GetCurrentScreen().GetData();

                    callbackResults.SetResult(screen.GetWidget(widgetType));

                    if (callbackResults.Success())
                    {
                        var popUpWidget = screen.GetWidget(widgetType).GetData();

                        screen.ShowWidget(popUpWidget, widgetShownCallbackResults =>
                        {
                            callbackResults.SetResult(widgetShownCallbackResults);

                            if (callbackResults.Success())
                            {
                                AddShownPopUpWidget(popUpWidget, widgetShownSetCallbackResults =>
                                {
                                    callbackResults.SetResult(widgetShownSetCallbackResults);
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
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public void SurfaceWidget(AppData.SceneConfigDataPacket configDataPacket, Action<AppData.CallbackData<AppData.SurfacingResults>> callback = null)
        {
            var callbackResults = new AppData.CallbackData<AppData.SurfacingResults>(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Show Pop Up Failed - Screen UI Manager Instance Is Not Initialized Yet - Invalid Operation."));

            if (callbackResults.Success())
            {
                var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                if (callbackResults.Success())
                {
                    var screen = screenUIManagerInstance.GetCurrentScreen().GetData();

                    callbackResults.SetResult(screen.GetWidget(configDataPacket.GetReferencedWidgetType().GetData().GetValue().GetData()));

                    if (callbackResults.Success())
                    {
                        var popUpWidget = screen.GetWidget(configDataPacket.GetReferencedWidgetType().GetData().GetValue().GetData()).GetData();

                        screen.ShowWidget(configDataPacket, widgetShownCallbackResults =>
                        {
                            callbackResults.SetResult(widgetShownCallbackResults);

                            if (callbackResults.Success())
                            {
                                AddShownPopUpWidget(popUpWidget, widgetShownSetCallbackResults =>
                                {
                                    callbackResults.SetResult(widgetShownSetCallbackResults);
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
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        #endregion

        #region Pop Up System

        #region Action Button Listener

        public void ShowPopUp(AppData.WidgetType popUpType, SurfacingTemplateContentConfigDataPacket configMessageData, Action<AppData.CallbackData<AppData.SurfacingResults>> callback = null, AppData.ActionButtonListener primaryButton = null, AppData.ActionButtonListener secondaryButton = null, AppData.LogInfoChannel infoChannel = AppData.LogInfoChannel.None)
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

        public void ShowPopUp(AppData.Widget popUp, SurfacingTemplateContentConfigDataPacket configMessageData, Action<AppData.CallbackData<AppData.SurfacingResults>> callback = null, AppData.ActionButtonListener primaryButton = null, AppData.ActionButtonListener secondaryButton = null, AppData.LogInfoChannel infoChannel = AppData.LogInfoChannel.None)
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

        public void ShowPopUp(AppData.SceneConfigDataPacket popUpConfig, SurfacingTemplateContentConfigDataPacket configMessageData, Action<AppData.CallbackData<AppData.SurfacingResults>> callback = null, AppData.ActionButtonListener primaryButton = null, AppData.ActionButtonListener secondaryButton = null, AppData.LogInfoChannel infoChannel = AppData.LogInfoChannel.None)
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

        public void ShowPopUp(AppData.WidgetType popUpType, SurfacingTemplateContentConfigDataPacket configMessageData, Action<AppData.CallbackData<AppData.SurfacingResults>> callback = null, Action primaryButton = null, Action secondaryButton = null, AppData.LogInfoChannel infoChannel = AppData.LogInfoChannel.None)
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

        public void ShowPopUp(AppData.Widget popUp, SurfacingTemplateContentConfigDataPacket configMessageData, Action<AppData.CallbackData<AppData.SurfacingResults>> callback = null, Action primaryButton = null, Action secondaryButton = null, AppData.LogInfoChannel infoChannel = AppData.LogInfoChannel.None)
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

        public void ShowPopUp(AppData.SceneConfigDataPacket popUpConfig, SurfacingTemplateContentConfigDataPacket configMessageData, Action<AppData.CallbackData<AppData.SurfacingResults>> callback = null, Action primaryButton = null, Action secondaryButton = null, AppData.LogInfoChannel infoChannel = AppData.LogInfoChannel.None)
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

        public void ShowPopUp(AppData.WidgetType popUpType, AppData.SurfacingContentType messageType, Action<AppData.CallbackData<AppData.SurfacingResults>> callback = null, (string buttonTitle, Action method) primaryButtonEvent = default, (string buttonTitle, Action method) secondaryButtonEvent = default, params string[] messageOverrides)
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
                                            callbackResults.SetResult(messageDataObject.GetMessage(messageOverrides));

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

                                                        //popUpWidget.RegisterActionButtonListeners(actionButtonEventsRegisteredCallbackResults =>
                                                        //{
                                                        //    callbackResults.SetResult(actionButtonEventsRegisteredCallbackResults);

                                                        //    if (callbackResults.Success())
                                                        //    {
                                                        //        callbackResults.SetResult(primaryButtonReference.Initialized());

                                                        //        if (callbackResults.Success())
                                                        //        {
                                                        //            callbackResults.SetResult(AppData.Helpers.GetAppStringValueNotNullOrEmpty(primaryButtonEvent.buttonTitle, "Button Title", "Show Pop Up Set button Title Unsuccessful - Continuing Execution."));

                                                        //            if (callbackResults.Success())
                                                        //            {
                                                        //                popUpWidget.SetActionButtonTitle(AppData.InputActionButtonType.ConfirmationButton, primaryButtonEvent.buttonTitle, buttonTitleUpdatedCallbackResults => 
                                                        //                {
                                                        //                    callbackResults.SetResult(buttonTitleUpdatedCallbackResults);
                                                        //                });
                                                        //            }
                                                        //            else
                                                        //            {
                                                        //                callbackResults.result = $"There Are No Action Events Registered - Results Code : {callbackResults.GetResultCode} - Results : {callbackResults.GetResult} - Continuing Execution.";
                                                        //                callbackResults.resultCode = AppData.Helpers.SuccessCode;
                                                        //            }
                                                        //        }
                                                        //        else
                                                        //        {
                                                        //            callbackResults.result = $"There Are No Action Events Registered - Results Code : {callbackResults.GetResultCode} - Results : {callbackResults.GetResult} - Continuing Execution.";
                                                        //            callbackResults.resultCode = AppData.Helpers.SuccessCode;
                                                        //        }

                                                        //        callbackResults.SetResult(secondaryButtonReference.Initialized());

                                                        //        if (callbackResults.Success())
                                                        //        {
                                                        //            callbackResults.SetResult(AppData.Helpers.GetAppStringValueNotNullOrEmpty(secondaryButtonEvent.buttonTitle, "Button Title", "Show Pop Up Set button Title Unsuccessful - Continuing Execution."));

                                                        //            if (callbackResults.Success())
                                                        //            {
                                                        //                popUpWidget.SetActionButtonTitle(AppData.InputActionButtonType.Cancel, secondaryButtonEvent.buttonTitle, buttonTitleUpdatedCallbackResults =>
                                                        //                {
                                                        //                    callbackResults.SetResult(buttonTitleUpdatedCallbackResults);
                                                        //                });
                                                        //            }
                                                        //            else
                                                        //            {
                                                        //                callbackResults.result = $"There Are No Action Events Registered - Results Code : {callbackResults.GetResultCode} - Results : {callbackResults.GetResult} - Continuing Execution.";
                                                        //                callbackResults.resultCode = AppData.Helpers.SuccessCode;
                                                        //            }
                                                        //        }
                                                        //        else
                                                        //        {
                                                        //            callbackResults.result = $"There Are No Action Events Registered - Results Code : {callbackResults.GetResultCode} - Results : {callbackResults.GetResult} - Continuing Execution.";
                                                        //            callbackResults.resultCode = AppData.Helpers.SuccessCode;
                                                        //        }
                                                        //    }
                                                        //    else
                                                        //    {
                                                        //        callbackResults.result = $"There Are No Action Events Registered - Results Code : {callbackResults.GetResultCode} - Results : {callbackResults.GetResult} - Continuing Execution.";
                                                        //        callbackResults.resultCode = AppData.Helpers.SuccessCode;
                                                        //    }

                                                        //}, primaryButtonReference, secondaryButtonReference);

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

        public void ShowPopUp(AppData.Widget popUp, AppData.SurfacingContentType messageType, Action<AppData.CallbackData<AppData.SurfacingResults>> callback = null, Action primaryButton = null, Action secondaryButton = null, AppData.LogInfoChannel infoChannel = AppData.LogInfoChannel.None)
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

        public void ShowPopUp(AppData.SceneConfigDataPacket popUpConfig, AppData.SurfacingContentType messageType, Action<AppData.CallbackData<AppData.SurfacingResults>> callback = null, Action primaryButton = null, Action secondaryButton = null, AppData.LogInfoChannel infoChannel = AppData.LogInfoChannel.None)
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

        #region Surfacing Templates

        public void ShowPopUp(AppData.SurfacingTemplateType templateType, Action<AppData.CallbackData<AppData.SurfacingResults>> callback = null, Action primaryButtonMethodOverride = null, Action secondaryButtonMethodOverride = null)
        {
            var callbackResults = new AppData.CallbackData<AppData.SurfacingResults>(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Show Pop Up Failed - Screen UI Manager Instance Is Not Initialized Yet - Invalid Operation."));

            if (callbackResults.Success())
            {
                var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                if (callbackResults.Success())
                {
                    var screen = screenUIManagerInstance.GetCurrentScreen().GetData();

                    HideInteruptableWidgets(widgetsHiddencallbackResults => 
                    {
                        callbackResults.SetResult(widgetsHiddencallbackResults);

                        if(callbackResults.Success())
                        {
                            callbackResults.SetResult(GetSurfacingTemplateLibrary());

                            if (callbackResults.Success())
                            {
                                callbackResults.SetResult(GetSurfacingTemplateLibrary().GetData().GetSurfacingTemplate(templateType));

                                if (callbackResults.Success())
                                {
                                    var surfacingTemplate = GetSurfacingTemplateLibrary().GetData().GetSurfacingTemplate(templateType).GetData();

                                    callbackResults.SetResult(screen.GetWidget(surfacingTemplate.GetTemplateWidgetType().GetData()));

                                    if (callbackResults.Success())
                                    {
                                        var popUpWidget = screen.GetWidget(surfacingTemplate.GetTemplateWidgetType().GetData()).GetData();

                                        callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance", "On Tab View Shown Event Failed - App Database Manager Instance Is Not Yet Initialized - Invalid Operation."));

                                        if (callbackResults.Success())
                                        {
                                            var appDatabaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance").GetData();

                                            callbackResults.SetResult(appDatabaseManagerInstance.GetAssetBundlesLibrary());

                                            if (callbackResults.Success())
                                            {
                                                var assetBundlesLibrary = appDatabaseManagerInstance.GetAssetBundlesLibrary().GetData();

                                                callbackResults.SetResult(assetBundlesLibrary.GetLoadedConfigMessageDataPacket(surfacingTemplate.GetTemplateContentType().GetData()));

                                                if (callbackResults.Success())
                                                {
                                                    var surfacingTemplateConfigDataObject = assetBundlesLibrary.GetLoadedConfigMessageDataPacket(surfacingTemplate.GetTemplateContentType().GetData()).GetData();

                                                    callbackResults.SetResult(surfacingTemplateConfigDataObject.Initialized());

                                                    if (callbackResults.Success())
                                                    {
                                                        callbackResults.SetResult(surfacingTemplateConfigDataObject.GetTitle());

                                                        popUpWidget.SetUITextDisplayerValue(AppData.ScreenTextType.TitleDisplayer, surfacingTemplateConfigDataObject.GetTitle().GetData(), widgetTitleSetCallbackResults =>
                                                        {
                                                            callbackResults.SetResult(widgetTitleSetCallbackResults);

                                                            if (callbackResults.Success())
                                                            {
                                                                var popUpMessage = string.Empty;

                                                                callbackResults.SetResult(surfacingTemplate.GetMessageOverrides());

                                                                if (callbackResults.Success())
                                                                {
                                                                    callbackResults.SetResult(surfacingTemplateConfigDataObject.GetMessage(surfacingTemplate.GetMessageOverrides().GetData()));

                                                                    if (callbackResults.Success())
                                                                        popUpMessage = surfacingTemplateConfigDataObject.GetMessage(surfacingTemplate.GetMessageOverrides().GetData()).GetData();
                                                                }
                                                                else
                                                                    popUpMessage = surfacingTemplateConfigDataObject.GetMessage().GetData();

                                                                popUpWidget.SetUITextDisplayerValue(AppData.ScreenTextType.MessageDisplayer, popUpMessage, widgetTitleSetCallbackResults =>
                                                                {
                                                                    callbackResults.SetResult(widgetTitleSetCallbackResults);

                                                                    if (callbackResults.Success())
                                                                    {
                                                                        #region Set Icon

                                                                        callbackResults.SetResult(surfacingTemplate.GetIcon());

                                                                        if (callbackResults.Success())
                                                                        {
                                                                            popUpWidget.SetUIImageDisplayer(AppData.ScreenImageType.Icon, surfacingTemplate.GetIcon().GetData(), true, iconSetCallbackResults =>
                                                                            {
                                                                                callbackResults.SetResult(iconSetCallbackResults);
                                                                            });
                                                                        }

                                                                        #endregion

                                                                        #region Set Background image

                                                                        callbackResults.SetResult(surfacingTemplate.GetBackgroundImage());

                                                                        if (callbackResults.Success())
                                                                        {
                                                                            popUpWidget.SetUIImageDisplayer(AppData.ScreenImageType.Background, surfacingTemplate.GetBackgroundImage().GetData(), true, backgroundSetCallbackResults =>
                                                                            {
                                                                                callbackResults.SetResult(backgroundSetCallbackResults);
                                                                            });
                                                                        }

                                                                        #endregion

                                                                        #region Set Buttons Overrides

                                                                        callbackResults.SetResult(surfacingTemplate.GetPrimaryButtonOverride());

                                                                        if (callbackResults.Success())
                                                                        {
                                                                            popUpWidget.SetActionButtonState(AppData.InputActionButtonType.ConfirmationButton, AppData.InputUIState.Shown, buttonShownCallbackResults =>
                                                                            {
                                                                                callbackResults.SetResult(buttonShownCallbackResults);

                                                                                if (callbackResults.Success())
                                                                                {
                                                                                    popUpWidget.SetActionButtonTitle(AppData.InputActionButtonType.ConfirmationButton, surfacingTemplate.GetPrimaryButtonOverride().GetData().GetTitleTextOverride().GetData(), buttonTitleSetCallbackResults =>
                                                                                    {
                                                                                        callbackResults.SetResult(buttonTitleSetCallbackResults);

                                                                                        if (callbackResults.Success())
                                                                                        {
                                                                                            callbackResults.SetResult(surfacingTemplate.GetPrimaryButtonOverride().GetData().GetColorOverride());

                                                                                            if (callbackResults.Success())
                                                                                            {
                                                                                                popUpWidget.SetActionButtonColor(AppData.InputActionButtonType.ConfirmationButton, surfacingTemplate.GetPrimaryButtonOverride().GetData().GetColorOverride().GetData(), colorSetCallbackResults =>
                                                                                                {
                                                                                                    callbackResults.SetResult(colorSetCallbackResults);
                                                                                                });
                                                                                            }

                                                                                            var primaryButtonReference = primaryButtonMethodOverride != null ? new AppData.ActionButtonListener()
                                                                                            {
                                                                                                method = primaryButtonMethodOverride,
                                                                                                action = AppData.InputActionButtonType.ConfirmationButton

                                                                                            } : null;

                                                                                            popUpWidget.RegisterActionButtonListeners(buttonOverrideRegisteredCallbackResults =>
                                                                                            {
                                                                                                callbackResults.SetResult(buttonOverrideRegisteredCallbackResults);

                                                                                            }, primaryButtonReference);
                                                                                        }
                                                                                    });
                                                                                }
                                                                                else
                                                                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                                            });
                                                                        }
                                                                        else
                                                                        {
                                                                            popUpWidget.SetActionButtonState(AppData.InputActionButtonType.ConfirmationButton, AppData.InputUIState.Hidden, buttonHiddenCallbackResults =>
                                                                            {
                                                                                callbackResults.SetResult(buttonHiddenCallbackResults);
                                                                            });
                                                                        }

                                                                        callbackResults.SetResult(surfacingTemplate.GetSecondaryButtonOverride());

                                                                        if (callbackResults.Success())
                                                                        {
                                                                            popUpWidget.SetActionButtonState(AppData.InputActionButtonType.Cancel, AppData.InputUIState.Shown, buttonShownCallbackResults =>
                                                                            {
                                                                                callbackResults.SetResult(buttonShownCallbackResults);

                                                                                if (callbackResults.Success())
                                                                                {
                                                                                    popUpWidget.SetActionButtonTitle(AppData.InputActionButtonType.Cancel, surfacingTemplate.GetSecondaryButtonOverride().GetData().GetTitleTextOverride().GetData(), buttonTitleSetCallbackResults =>
                                                                                    {
                                                                                        callbackResults.SetResult(buttonTitleSetCallbackResults);

                                                                                        if (callbackResults.Success())
                                                                                        {
                                                                                            callbackResults.SetResult(surfacingTemplate.GetPrimaryButtonOverride().GetData().GetColorOverride());

                                                                                            if (callbackResults.Success())
                                                                                            {
                                                                                                popUpWidget.SetActionButtonColor(AppData.InputActionButtonType.Cancel, surfacingTemplate.GetSecondaryButtonOverride().GetData().GetColorOverride().GetData(), colorSetCallbackResults =>
                                                                                                {
                                                                                                    callbackResults.SetResult(colorSetCallbackResults);
                                                                                                });
                                                                                            }

                                                                                            var secondaryButtonReference = secondaryButtonMethodOverride != null ? new AppData.ActionButtonListener
                                                                                            {
                                                                                                method = secondaryButtonMethodOverride,
                                                                                                action = AppData.InputActionButtonType.Cancel

                                                                                            } : null;

                                                                                            popUpWidget.RegisterActionButtonListeners(buttonOverrideRegisteredCallbackResults =>
                                                                                            {
                                                                                                callbackResults.SetResult(buttonOverrideRegisteredCallbackResults);

                                                                                            }, secondaryButtonReference);
                                                                                        }
                                                                                    });
                                                                                }
                                                                                else
                                                                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                                            });
                                                                        }
                                                                        else
                                                                        {
                                                                            popUpWidget.SetActionButtonState(AppData.InputActionButtonType.Cancel, AppData.InputUIState.Hidden, buttonHiddenCallbackResults =>
                                                                            {
                                                                                callbackResults.SetResult(buttonHiddenCallbackResults);
                                                                            });
                                                                        }

                                                                        #endregion


                                                                        #region Constraints

                                                                        callbackResults.SetResult(surfacingTemplate.GetSurfacingTemplateConstraints());

                                                                        if(callbackResults.Success())
                                                                        {
                                                                            popUpWidget.ApplyConstraints(constraintsAppliedCallbackResults =>
                                                                            {
                                                                                callbackResults.SetResult(constraintsAppliedCallbackResults);

                                                                            }, AppData.Helpers.GetArray(surfacingTemplate.GetSurfacingTemplateConstraints().GetData()));
                                                                        }

                                                                        #endregion

                                                                        #region Surface Popup

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

                                                                        #endregion
                                                                    }
                                                                    else
                                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
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
                    });
                 
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

        
        public AppData.Callback HideInteruptableWidgets()
        {
            var callbackResults = new AppData.Callback(GetSurfacedInteruptableWidgets());

            if (callbackResults.Success())
            {
                var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                if (callbackResults.Success())
                {
                    var screen = screenUIManagerInstance.GetCurrentScreen().GetData();

                    for (int i = 0; i < GetSurfacedInteruptableWidgets().GetData().Count; i++)
                    {
                        screen.HideWidget(GetSurfacedInteruptableWidgets().GetData()[i], widgetHiddenCallbackResults =>
                        {
                            callbackResults.SetResult(widgetHiddenCallbackResults);
                        });

                        if (callbackResults.UnSuccessful())
                        {
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                            break;
                        }
                    }
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
            {
                callbackResults.result = "There Are No Interuptable Widgets Found - Successfully Continuing Execution.";
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }

            return callbackResults;
        }

        public void HideInteruptableWidgets(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(GetSurfacedInteruptableWidgets());

            if(callbackResults.Success())
            {
                var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                if (callbackResults.Success())
                {
                    var screen = screenUIManagerInstance.GetCurrentScreen().GetData();

                    for (int i = 0; i < GetSurfacedInteruptableWidgets().GetData().Count; i++)
                    {
                        screen.HideWidget(GetSurfacedInteruptableWidgets().GetData()[i], widgetHiddenCallbackResults => 
                        {
                            callbackResults.SetResult(widgetHiddenCallbackResults);
                        });

                        if(callbackResults.UnSuccessful())
                        {
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                            break;
                        }
                    }
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
            {
                callbackResults.result = "There Are No Interuptable Widgets Found - Successfully Continuing Execution.";
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }

            callback?.Invoke(callbackResults);
        }


        public AppData.CallbackDataList<AppData.Widget> GetSurfacedWidgetList()
        {
            var callbackResults = new AppData.CallbackDataList<AppData.Widget>();

            callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(surfacedWidgetList, "Surfaced Widget List", "Get Surfaced Widget List Failed - There Are No Surfaced Widget List Items Found - Invalid Operation."));

            if(callbackResults.Success())
            {
                callbackResults.result = $"Get Surfaced Widget List Success - There Are : {surfacedWidgetList.Count} Surfaced Widget List Items Found.";
                callbackResults.data = surfacedWidgetList;
            }

            return callbackResults;
        }

        public AppData.CallbackDataList<AppData.Widget> GetSurfacedInteruptableWidgets()
        {
            var callbackResults = new AppData.CallbackDataList<AppData.Widget>(GetSurfacedWidgetList());

            if (callbackResults.Success())
            {
                var interuptableWidgets = GetSurfacedWidgetList().GetData().FindAll(widget => widget.Interuptable().Success());

                callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(interuptableWidgets, "Interuptable Widgets", "Get Surfaced Interuptable Widgets Failed - There Are No Found Interuptable Widgets - Invalid Operation."));

                if(callbackResults.Success())
                {
                    callbackResults.result = $"Get Surfaced Interuptable Widgets Success - There Are : {interuptableWidgets.Count} Interuptable Widgets Found.";
                    callbackResults.data = interuptableWidgets;
                }
            }

            return callbackResults;
        }

        #endregion

        #region Surfacing Data

        private AppData.CallbackData<AppData.SurfacingTemplateLibrary> GetSurfacingTemplateLibrary()
        {
            var callbackResults = new AppData.CallbackData<AppData.SurfacingTemplateLibrary>(AppData.Helpers.GetAppComponentValid(surfacingTemplateLibrary, "Surfacing Template Library", "Get Surfacing Template Library Failed - Surfacing Template Library Is Not Assigned - Invalid Operation."));

            if(callbackResults.Success())
            {
                callbackResults.SetResult(surfacingTemplateLibrary.Initialized());

                if (callbackResults.Success())
                {
                    callbackResults.result = "Get Surfacing Template Library Success - Surfacing Template Library Has Been Successfully initialized.";
                    callbackResults.data = surfacingTemplateLibrary;
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        #endregion

        #endregion
    }
}