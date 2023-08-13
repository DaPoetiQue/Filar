using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class UIScreenPostViewWidget : AppData.UIScreenWidget
    {
        #region Components

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
                if (AppData.Helpers.IsSuccessCode(callback.resultCode))
                    if (screenManager == null)
                        screenManager = ScreenUIManager.Instance;
                    else
                        Debug.LogWarning($"--> Failed to Initialize Scene Asset UI With Results : {callback.result}.");
                else
                    Debug.LogWarning("--> Failed to Initialize Scene Asset UI.");
            });
        }

        protected override void OnActionButtonInputs(AppData.UIButton<AppData.ButtonDataPackets> actionButton)
        {
            LogInfo($"===> Button Action : {actionButton.dataPackets.action}");

            switch (actionButton.dataPackets.action)
            {
                case AppData.InputActionButtonType.OpenProject:

                    
                    break;
            }
        }

        void OnGoToProfile_ActionEvent(AppData.ButtonDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnSetUIWidgetData(AppData.Folder folder)
        {
            throw new System.NotImplementedException();
        }

        public override void OnSelect(bool isInitialSelection = false)
        {
            if (SelectableManager.Instance != null)
            {
                Debug.LogError("===========> Please Fix Selection Here");
                //SelectableManager.Instance.Select(this, dataPackets, isInitialSelection);
                //Selected();
            }
            else
                Debug.LogWarning("--> OnSelect Failed :  SelectableManager.Instance Is Not Yet initialized.");
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
            //if (!string.IsNullOrEmpty(structureData.projectInfo.name))
            //{
            //    SetUITextDisplayerValue(structureData?.projectInfo?.name, AppData.ScreenTextType.TitleDisplayer);

            //    string lastModified = structureData?.creationDateTime.date;
            //    SetUITextDisplayerValue(lastModified, AppData.ScreenTextType.TimeDateDisplayer);

            //    string projectType = structureData?.GetProjectInfo()?.GetCategoryType().ToString().Replace("Project_", "");
            //    SetUITextDisplayerValue(projectType, AppData.ScreenTextType.TypeDisplayer);

            //    SceneAssetsManager.Instance.GetProjectCategoryInfo(structureData.GetProjectInfo().GetCategoryType(), projectInfoCallbackResults =>
            //    {
            //        if (projectInfoCallbackResults.Success())
            //            SetActionButtonColor(AppData.InputActionButtonType.OpenProject, projectInfoCallbackResults.data.color);
            //        else
            //            Log(projectInfoCallbackResults.resultCode, projectInfoCallbackResults.result, this);
            //    });
            //}
        }

        #endregion
    }
}