using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class ProfileTabView : AppData.TabView<AppData.WidgetType>
    {
        #region Components

        private AppData.ActionButtonListener onConfirmationButtonEvent = new AppData.ActionButtonListener();

        #endregion

        #region Main

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket<AppData.TabViewType, AppData.TabViewType, AppData.Widget>>> callback)
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.TabViewType, AppData.TabViewType, AppData.Widget>>();

            Init(initializationCallbackResults =>
            {
                callbackResults.SetResultsData(initializationCallbackResults);
            });

            callback.Invoke(callbackResults);
        }

        #region Tab View Overrides

        protected override void OnTabViewShown(Action<AppData.Callback> callback = null)
        {

        }

        protected override void OnTabViewHidden(Action<AppData.Callback> callback = null)
        {

        }

        #endregion

        protected override void OnActionButtonEvent(AppData.TabViewType screenWidgetType, AppData.InputActionButtonType actionType, AppData.SceneConfigDataPacket dataPackets)
        {
            var callbackResults = new AppData.Callback();

            switch (actionType)
            {
                case AppData.InputActionButtonType.OpenProjectButton:

                    callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Screen UI Manager Instance Is Not Yet Initialized."));

                    if (callbackResults.Success())
                    {
                        var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                        callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                        if (callbackResults.Success())
                        {
                            var screen = screenUIManagerInstance.GetCurrentScreen().GetData();

                            callbackResults.SetResult(screen.GetWidget(AppData.WidgetType.ConfirmationPopUpWidget));

                            if (callbackResults.Success())
                            {
                                var confirmationWidget = screen.GetWidget(AppData.WidgetType.ConfirmationPopUpWidget).GetData() as ConfirmationPopUpWidget;

                                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(confirmationWidget, "Confirmation Widget", "Failed To Get Confirmation Widget - Invalid Operation."));

                                if(callbackResults.Success())
                                {
                                    confirmationWidget.UnRegisterActionButtonListeners(eventsUnregisteredCallbackResults => 
                                    {
                                        callbackResults.SetResult(eventsUnregisteredCallbackResults);

                                        if(callbackResults.Success())
                                        {
                                            onConfirmationButtonEvent.SetMethod(GoToProjectHub, methodSetCallbackResults =>
                                            {
                                                callbackResults.SetResult(methodSetCallbackResults);

                                                if (callbackResults.Success())
                                                {
                                                    onConfirmationButtonEvent.SetAction(AppData.InputActionButtonType.ConfirmationButton, actionSetCallbackResults =>
                                                    {
                                                        callbackResults.SetResult(actionSetCallbackResults);

                                                        if (callbackResults.Success())
                                                        {
                                                            confirmationWidget.RegisterActionButtonListeners(onConfirmRegisteredCallbackResults =>
                                                            {
                                                                callbackResults.SetResult(onConfirmRegisteredCallbackResults);

                                                                if (callbackResults.Success())
                                                                {
                                                                    var confirmationWidgetConfig = new AppData.SceneConfigDataPacket();

                                                                    confirmationWidgetConfig.SetReferencedWidgetType(AppData.WidgetType.ConfirmationPopUpWidget);
                                                                    confirmationWidgetConfig.blurScreen = true;

                                                                    screen.ShowWidget(confirmationWidgetConfig);
                                                                }
                                                                else
                                                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                                            }, onConfirmationButtonEvent);
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

                    break;
            }
        }

        private async void GoToProjectHub()
        {
            var callbackResults = new AppData.Callback();

            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Screen UI Manager Instance Is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                if (callbackResults.Success())
                {
                    var screen = screenUIManagerInstance.GetCurrentScreen().GetData();

                    callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance", "App Database Manager Instance Is Not Yet Initialized."));

                    if (callbackResults.Success())
                    {
                        var appDatabaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance").GetData();

                        callbackResults.SetResult(appDatabaseManagerInstance.GetAssetBundlesLibrary());

                        if (callbackResults.Success())
                        {
                            var assetBundles = appDatabaseManagerInstance.GetAssetBundlesLibrary().GetData();

                            callbackResults.SetResult(assetBundles.GetDynamicContainer<DynamicContentContainer>(AppData.ScreenType.LandingPageScreen, AppData.ContentContainerType.SceneContentsContainer, AppData.ContainerViewSpaceType.Scene));

                            if (callbackResults.Success())
                            {
                                await Task.Delay(500);

                                var hideMenuCallbackResults = await screen.HideWidgetAsync(GetParentWidget().GetData());

                                callbackResults.SetResult(hideMenuCallbackResults);

                                if (callbackResults.Success())
                                {
                                    var container = assetBundles.GetDynamicContainer<DynamicContentContainer>(AppData.ScreenType.LandingPageScreen, AppData.ContentContainerType.SceneContentsContainer, AppData.ContainerViewSpaceType.Scene).GetData();

                                    var clearContainerCallbackResultsTask = await container.ClearAsync(true, 0.5f);

                                    callbackResults.SetResult(clearContainerCallbackResultsTask);

                                    if (callbackResults.Success())
                                    {
                                        callbackResults.SetResult(GetParentWidget());

                                        if (callbackResults.Success())
                                        {
                                            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(LoadingManager.Instance, "Loading Manager Instance", "Loading Manager Instance Is Not Yet Initialized."));

                                            if (callbackResults.Success())
                                            {
                                                var loadingManagerInstance = AppData.Helpers.GetAppComponentValid(LoadingManager.Instance, "Loading Manager Instance").GetData();

                                                loadingManagerInstance.LoadSelectedScreen(AppData.ScreenType.ProjectCreationScreen, loadedScreenCallbackResults =>
                                                {
                                                    callbackResults.SetResult(loadedScreenCallbackResults);
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
            }
        }


        protected override void OnActionButtonInputs(AppData.UIButton<AppData.ButtonConfigDataPacket> actionButton)
        {
           
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownConfigDataPacket dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnScrollerValueChanged(Vector2 value)
        {
            throw new NotImplementedException();
        }

        protected override void OnInputFieldValueChanged(string value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnInputFieldValueChanged(int value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxConfigDataPacket dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override AppData.CallbackData<AppData.WidgetStatePacket<AppData.TabViewType, AppData.TabViewType, AppData.Widget>> OnGetState()
        {
            throw new NotImplementedException();
        }

        protected override void OnScreenWidgetShownEvent()
        {
          
        }

        protected override void OnScreenWidgetHiddenEvent()
        {
           
        }

        protected override void OnScreenWidgetTransitionInProgressEvent()
        {
           
        }

        #endregion
    }
}