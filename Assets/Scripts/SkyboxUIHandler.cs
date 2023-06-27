using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SkyboxUIHandler : AppData.UIScreenWidget
    {

        #region Components

        [SerializeField]
        AppData.SkyboxSettings skyboxData = new AppData.SkyboxSettings();

        #endregion

        #region Unity Callbacks

        void Start() => Initialization();

        #endregion

        #region Main

        void Initialization()
        {
            // Initialize Assets.
            Init((callback) =>
            {
                if (AppData.Helpers.IsSuccessCode(callback.resultsCode))
                    if (screenManager == null)
                        screenManager = ScreenUIManager.Instance;
                    else
                        Debug.LogWarning($"--> Failed to Initialize Scene Asset UI With Results : {callback.results}.");
                else
                    Debug.LogWarning("--> Failed to Initialize Scene Asset UI.");
            });
        }

        public void SetDataOnInitialization(AppData.SkyboxSettings skyboxData)
        {
            this.skyboxData = skyboxData;

        }

        protected override void OnActionButtonInputs(AppData.UIButton<AppData.ButtonDataPackets> actionButton)
        {
            switch (actionButton.dataPackets.action)
            {
                case AppData.InputActionButtonType.CreateSkyboxButton:

                    break;

                case AppData.InputActionButtonType.SkyboxSelectionButton:

                    RenderingSettingsManager.Instance.ApplySkyboxSettings(skyboxData.lightingSettings, (applied) =>
                    {
                        if (AppData.Helpers.IsSuccessCode(applied.resultsCode))
                            Debug.Log($"==> Selected Skybox : {skyboxData.name}");
                        else
                            Debug.LogWarning($"--> Failed With Results : {applied.results}");
                    });

                    break;
            }
        }

        protected override void OnSetUIWidgetData(AppData.Folder folder)
        {
            throw new System.NotImplementedException();
        }

        public override void OnSelect(bool isInitialSelection)
        {
            throw new System.NotImplementedException();
        }

        public override void OnDeselect()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnSetFileData(AppData.SceneAsset assetData)
        {

        }

        protected override void OnScreenUIRefreshed()
        {

        }

        protected override void OnSetUIWidgetData(AppData.FolderStructureData structureData)
        {
            throw new System.NotImplementedException();
        }


        #endregion
    }
}
