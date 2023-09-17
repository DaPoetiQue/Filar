using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class UITextDisplayerWidget : AppData.Widget
    {
        #region Components

        #endregion

        #region Main

        protected override void Initialize() => textDisplayerWidget = this;

        protected override void OnHideScreenWidget()
        {
            HideSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
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

        }

        protected override void OnShowScreenWidget(AppData.SceneDataPackets dataPackets)
        {
            ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);

            SetUITextDisplayerValue(AppData.ScreenTextType.MessageDisplayer, dataPackets.popUpMessage);

            if (dataPackets.referencedActionButtonDataList.Count > 0)
            {
                foreach (var referencedActionButton in dataPackets.referencedActionButtonDataList)
                {
                    SetActionButtonTitle(referencedActionButton.type, referencedActionButton.title);
                    SetActionButtonState(referencedActionButton.type, referencedActionButton.state);
                }
            }
        }

        protected override void OnSubscribeToActionEvents(bool subscribe)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScrollerValueChanged(Vector2 value)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionButtonEvent(AppData.WidgetType popUpType, AppData.InputActionButtonType actionType, AppData.SceneDataPackets dataPackets)
        {
            
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
