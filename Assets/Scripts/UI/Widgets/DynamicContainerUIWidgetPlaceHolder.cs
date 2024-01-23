using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class DynamicContainerUIWidgetPlaceHolder : AppData.SelectableWidget
    {
        #region Components

        #endregion

        #region Unity Callbacks

        void Start() => Initialization();

        #endregion

        #region Main

        void Initialization()
        {
          
        }

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket<AppData.SelectableWidgetType, AppData.WidgetType, AppData.Widget>>> callback)
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.SelectableWidgetType, AppData.WidgetType, AppData.Widget>>();

            // Initialize Assets.
            Init(initializationCallbackResults =>
            {
                callbackResults.SetResult(initializationCallbackResults);
            });

            callback.Invoke(callbackResults);
        }


        protected override AppData.CallbackData<AppData.WidgetStatePacket<AppData.SelectableWidgetType, AppData.WidgetType, AppData.Widget>> OnGetState()
        {
            return null;
        }

        protected override void OnActionButtonInputs(AppData.UIButton<AppData.ButtonConfigDataPacket> actionButton)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnSetUIWidgetData(AppData.Folder folder)
        {
            throw new System.NotImplementedException();
        }

        public override void OnSelect(bool isInitialSelection)
        {
            throw new System.NotImplementedException();
        }

        public override void OnDeselect() => Deselected();

        protected override void OnSetAssetData(AppData.SceneAsset assetData)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScreenUIRefreshed()
        {

        }

        protected override void OnSetUIWidgetData(AppData.ProjectStructureData structureData)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnSetUIWidgetData(AppData.Post post)
        {
            throw new System.NotImplementedException();
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

        protected override void OnActionButtonEvent(AppData.SelectableWidgetType screenWidgetType, AppData.InputActionButtonType actionType, AppData.SceneConfigDataPacket dataPackets)
        {
            throw new NotImplementedException();
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

        #endregion
    }
}
