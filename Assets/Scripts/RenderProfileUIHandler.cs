using TMPro;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class RenderProfileUIHandler : AppData.UIScreenWidget
    {
        #region Components

        [SerializeField]
        TMP_Text titleDisplayer = null;

        [Space(5)]
        [SerializeField]
        AppData.UICheckbox<AppData.SceneDataPackets> profileToggleBox = new AppData.UICheckbox<AppData.SceneDataPackets>();

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

        protected override void OnActionButtonInputs(AppData.UIButton<AppData.ButtonDataPackets> actionButton)
        {
            switch (actionButton.dataPackets.action)
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

        protected override void OnSetFileData(AppData.SceneAsset assetData)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScreenUIRefreshed()
        {

        }

        #endregion
    }
}
