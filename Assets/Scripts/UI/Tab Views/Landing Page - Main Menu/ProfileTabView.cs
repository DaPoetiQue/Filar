using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class ProfileTabView : AppData.TabView<AppData.WidgetType>
    {
        #region Components

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

                            var confirmationWidgetConfig = new AppData.SceneConfigDataPacket();

                            confirmationWidgetConfig.SetReferencedWidgetType(AppData.WidgetType.ConfirmationPopUpWidget);
                            confirmationWidgetConfig.blurScreen = true;

                            //screen.ShowWidget(confirmationWidgetConfig);

                            // If Cant Show Pop Uo - Go Directly To Screen.
                            GoToProjectHub();
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                    }

                    break;
            }
        }


        private void GoToProjectHub()
        {
            var callbackResults = new AppData.Callback();

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