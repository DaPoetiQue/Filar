using System;
using TMPro;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class RenderProfileUIHandler : AppData.UIScreenWidget
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

            Init();
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

        protected override AppData.CallbackData<AppData.WidgetStatePacket<AppData.SelectableWidgetType, AppData.WidgetType>> OnGetState()
        {
            return null;
        }

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket<AppData.SelectableWidgetType, AppData.WidgetType>>> callback)
        {
           
        }

        #endregion
    }
}
