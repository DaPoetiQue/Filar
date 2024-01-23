using System;
using TMPro;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class RenderProfileUIHandler : AppData.SelectableWidget
    {
        #region Components

        [Space(5)]
        [SerializeField]
        AppData.UICheckbox<AppData.SceneConfigDataPacket> profileToggleBox = new AppData.UICheckbox<AppData.SceneConfigDataPacket>();

        AppData.NavigationRenderSettingsProfileID profileID;

        #endregion

        #region Main

        public void Initialize(AppData.NavigationRenderSettingsProfileID profileID = AppData.NavigationRenderSettingsProfileID.None)
        {
            this.profileID = profileID;

            if (titleDisplayer != null)
            {
                string title = profileID.ToString();
                title = title.Replace("_", " ");
                titleDisplayer.text = title;
            }
            else
                Debug.LogWarning("--> RG_Unity - Init Failed : Title Displayer Is Null.");

            //Init();
        }

        protected override void OnActionButtonInputs(AppData.UIButton<AppData.ButtonConfigDataPacket> actionButton)
        {
            switch (actionButton.dataPackets.GetAction().GetData())
            {
                case AppData.InputActionButtonType.Edit:

                    Debug.Log("-----------> Edit Render Profile");
                    AppData.ActionEvents.OnNavigationSubTabChangedEvent(actionButton.dataPackets.tabID, profileID);

                    break;

                case AppData.InputActionButtonType.Delete:

                    Debug.Log("-----------> Delete Render Profile");

                    break;
            }
        }

        public override void OnSelect(bool isInitialSelection)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnSetUIWidgetData(AppData.Folder folder)
        {
            throw new System.NotImplementedException();
        }

        public override void OnDeselect()
        {
            throw new System.NotImplementedException();
        }

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

        protected override AppData.CallbackData<AppData.WidgetStatePacket<AppData.SelectableWidgetType, AppData.WidgetType, AppData.Widget>> OnGetState()
        {
            return null;
        }

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket<AppData.SelectableWidgetType, AppData.WidgetType, AppData.Widget>>> callback)
        {
           
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
