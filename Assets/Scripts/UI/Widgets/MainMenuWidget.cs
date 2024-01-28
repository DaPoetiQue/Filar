using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class MainMenuWidget : AppData.Widget
    {
        #region Components

        #endregion

        #region Main

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.TabViewType, AppData.Widget>>> callback)
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.TabViewType, AppData.Widget>>();

            Init(initializationCallbackResults =>
            {
                callbackResults.SetResultsData(initializationCallbackResults);
            });

            callback.Invoke(callbackResults);
        }

        protected override AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.TabViewType, AppData.Widget>> OnGetState()
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.TabViewType, AppData.Widget>>(AppData.Helpers.GetAppComponentValid(GetStatePacket(), $"{GetName()} - State Object", "Widget State Object Is Null / Not Yet Initialized In The Base Class."));

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

        protected override async void OnActionButtonEvent(AppData.WidgetType screenType, AppData.InputActionButtonType actionType, AppData.SceneConfigDataPacket dataPackets)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Screen UI Manager Instance Is Not Initialized Yet."));

            if (callbackResults.Success())
            {
                var screenUIManager = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                callbackResults.SetResult(screenUIManager.GetCurrentScreen());

                if (callbackResults.Success())
                {
                    var screen = screenUIManager.GetCurrentScreen().GetData();

                    callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, "Profile Manager Instance", "Profile Manager Instance Is Not Yet Initialized."));

                    if (callbackResults.Success())
                    {
                        var profileManager = AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, "Profile Manager Instance").GetData();

                        switch (actionType)
                        {
                            case AppData.InputActionButtonType.OpenProfileButton:

                                callbackResults.SetResult(profileManager.GetSignInState());

                                if (callbackResults.Success())
                                {
                                    if (profileManager.GetSignInState().GetData() == AppData.SignInState.SignIn)
                                    {
                                        OpenMainMenu(AppData.MenuType.Profile, screen, menuOpenCallbackResults =>
                                        {
                                            callbackResults.SetResult(menuOpenCallbackResults);

                                            if (callbackResults.Success())
                                            {
                                                SelectTabView(AppData.TabViewType.ProfileView, tabSelectedCallbackResults =>
                                                {
                                                    callbackResults.SetResult(tabSelectedCallbackResults);
                                                });
                                            }
                                            else
                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                        });
                                    }
                                    else
                                    {
                                      
                                    }
                                }

                                break;

                            case AppData.InputActionButtonType.OpenInboxButton:

                                OpenMainMenu(AppData.MenuType.Inbox, screen, menuOpenCallbackResults =>
                                {
                                    callbackResults.SetResult(menuOpenCallbackResults);

                                    if (callbackResults.Success())
                                    {
                                        SelectTabView(AppData.TabViewType.InboxView, tabSelectedCallbackResults =>
                                        {
                                            callbackResults.SetResult(tabSelectedCallbackResults);
                                        });
                                    }
                                    else
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                });

                                break;

                            case AppData.InputActionButtonType.OpenCartButton:

                                OpenMainMenu(AppData.MenuType.Cart, screen, menuOpenCallbackResults =>
                                {
                                    callbackResults.SetResult(menuOpenCallbackResults);


                                    if (callbackResults.Success())
                                    {
                                        SelectTabView(AppData.TabViewType.CartView, tabSelectedCallbackResults =>
                                        {
                                            callbackResults.SetResult(tabSelectedCallbackResults);
                                        });
                                    }
                                    else
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                });

                                break;

                            case AppData.InputActionButtonType.OpenScreenSettingsButton:

                                OpenMainMenu(AppData.MenuType.Settings, screen, menuOpenCallbackResults => 
                                {
                                    callbackResults.SetResult(menuOpenCallbackResults);

                                    if(callbackResults.Success())
                                    {
                                        SelectTabView(AppData.TabViewType.SettingsView, tabSelectedCallbackResults =>
                                        {
                                            callbackResults.SetResult(tabSelectedCallbackResults);
                                        });
                                    }
                                    else
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                });

                                break;
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
        }

        private async void OpenMainMenu(AppData.MenuType menuType, Screen screen, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(screen.GetWidgetOfType(AppData.WidgetType.PostsWidget));

            if (callbackResults.Success())
            {
                var widget = screen.GetWidgetOfType(AppData.WidgetType.PostsWidget).GetData();

                callbackResults.SetResult(screen.IsFocusedWidget(AppData.WidgetType.PostsWidget));

                if (callbackResults.Success())
                {
                    var hideWidgetAsyncCallbackResultsTask = await screen.HideScreenWidgetAsync(AppData.WidgetType.PostsWidget);
                    callbackResults.SetResult(hideWidgetAsyncCallbackResultsTask);

                    if (callbackResults.Success())
                    {
                        await Task.Delay(500);

                        widget.SetActionButtonTitle(AppData.InputActionButtonType.ShowPostsButton, "Close", titleSetCallbackResults =>
                        {
                            callbackResults.SetResult(titleSetCallbackResults);

                            if (callbackResults.Success())
                                screen.ShowWidget(this);
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                        });
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                {
                    callbackResults.SetResult(screen.IsFocusedWidget(this));

                    if (callbackResults.Success())
                    {
                        screen.RemoveFocusedWidget(this, async focusedWidgetRemovedCallbackResults =>
                        {
                            callbackResults.SetResult(focusedWidgetRemovedCallbackResults);

                            if (callbackResults.Success())
                            {
                                //var hideWidgetAsyncCallbackResultsTask = await screen.HideScreenWidgetAsync(AppData.WidgetType.HomeMenuWidget);
                                //callbackResults.SetResult(hideWidgetAsyncCallbackResultsTask);

                                //if (callbackResults.Success())
                                //{
                                //    widget.SetActionButtonTitle(AppData.InputActionButtonType.ShowPostsButton, "Posts", titleSetCallbackResults =>
                                //    {
                                //        callbackResults.SetResult(titleSetCallbackResults);

                                //        if (callbackResults.Success())
                                //            screen.ShowWidget(this);
                                //        else
                                //            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                //    });
                                //}
                                //else
                                //    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                        });
                    }
                    else
                    {
                        widget.SetActionButtonTitle(AppData.InputActionButtonType.ShowPostsButton, "Close", titleSetCallbackResults =>
                        {
                            callbackResults.SetResult(titleSetCallbackResults);

                            if (callbackResults.Success())
                                screen.ShowWidget(this);
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                        });
                    }
                }
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

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

        protected override void OnScreenWidget(AppData.SceneConfigDataPacket configDataPacket, Action<AppData.Callback> callback = null)
        {
        
        }

        protected override void OnScrollerValueChanged(Vector2 value) => scroller.Update();

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxConfigDataPacket dataPackets)
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

        protected override void OnScreenWidget(Action<AppData.Callback> callback = null)
        {

        }

        protected override void OnScreenWidget<T>(AppData.ScriptableConfigDataPacket<T> scriptableConfigData, Action<AppData.Callback> callback = null)
        {
            
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

        protected override void OnActionButtonInputs(AppData.UIButton<AppData.ButtonConfigDataPacket> actionButton)
        {
          
        }

        #endregion
    }
}