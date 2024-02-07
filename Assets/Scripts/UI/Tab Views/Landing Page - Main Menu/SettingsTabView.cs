using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SettingsTabView : AppData.TabView<AppData.WidgetType>
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