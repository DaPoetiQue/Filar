using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{

    public class MainMenuWidget : AppData.Widget
    {
        #region Components

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

        protected override void OnActionButtonEvent(AppData.WidgetType popUpType, AppData.InputActionButtonType actionType, AppData.SceneDataPackets dataPackets)
        {
            AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, ScreenUIManager.Instance.name, screenUIManagerCallbackResults =>
            {
                if (screenUIManagerCallbackResults.Success())
                {
                    var screenUIManager = screenUIManagerCallbackResults.data;

                    switch (actionType)
                    {
                        case AppData.InputActionButtonType.OpenProfileButton:

                            AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, ProfileManager.Instance.name, profileManagerCallbackResults =>
                            {
                                if (profileManagerCallbackResults.Success())
                                {
                                    var profileManager = profileManagerCallbackResults.data;

                                    if (profileManager.SignedIn)
                                    {
                                        LogInfo(" <==========================> Open Profile", this);
                                    }
                                    else
                                    {
                                        screenUIManager.GetCurrentScreen(async currentScreenCallbackResults =>
                                        {
                                            if (currentScreenCallbackResults.Success())
                                            {
                                                var currentScreen = currentScreenCallbackResults.data;

                                                currentScreen.value.GetWidget(AppData.WidgetType.PostsWidget).SetActionButtonState(AppData.InputActionButtonType.HidePostsButton, AppData.InputUIState.Hidden);

                                                currentScreen.value.GetWidget(AppData.WidgetType.PostsWidget).SetActionButtonState(AppData.InputActionButtonType.ShowPostsButton, AppData.InputUIState.Shown);

                                                await currentScreen.value.HideScreenWidgetAsync(AppData.WidgetType.PostsWidget);


                                                AppData.SceneDataPackets dataPackets = new AppData.SceneDataPackets();

                                                dataPackets.SetReferencedScreenType(AppData.ScreenType.LandingPageScreen);
                                                dataPackets.SetReferencedWidgetType(AppData.WidgetType.SignInWidget);
                                                dataPackets.SetScreenBlurState(true);
                                                dataPackets.SetReferencedUIScreenPlacementType(AppData.ScreenUIPlacementType.Background);

                                                currentScreen.value.ShowWidget(dataPackets);
                                            }
                                        });
                                    }
                                }

                            }, "Profile Manager Instance Is Not Yet Initialized.");

                            break;
                    }
                }

            }, "Screen UI Manager Instance Is Not Yet Initialized.");
        }

        protected override void OnHideScreenWidget()
        {
            HideSelectedLayout(defaultLayoutType);
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
            ShowSelectedLayout(defaultLayoutType);
        }

        protected override void OnShowScreenWidget(AppData.SceneDataPackets dataPackets) => ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);

        protected override void OnScrollerValueChanged(Vector2 value) => scroller.Update();

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
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