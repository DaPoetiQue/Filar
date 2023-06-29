using System.IO;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class UIScreenProjectWidget : AppData.UIScreenWidget
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
                if (AppData.Helpers.IsSuccessCode(callback.resultsCode))
                    if (screenManager == null)
                        screenManager = ScreenUIManager.Instance;
                    else
                        Debug.LogWarning($"--> Failed to Initialize Scene Asset UI With Results : {callback.results}.");
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

                    if (SelectableManager.Instance != null)
                    {
                        if (SelectableManager.Instance.GetCurrentSelectionType() != AppData.FocusedSelectionType.SelectedItem)
                        {
                            if (SelectableManager.Instance.HasActiveSelection())
                                SelectableManager.Instance.OnClearFocusedSelectionsInfo();

                            if (SceneAssetsManager.Instance != null)
                            {
                                SceneAssetsManager.Instance.SetCurrentProjectStructureData(structureData);
                                //ScreenUIManager.Instance.GetCurrentScreenData().value.SetUITextDisplayerValue(AppData.ScreenTextType.TitleDisplayer, "New Project Name");

                                AppData.UIWidgetInfo selectedWidget = new AppData.UIWidgetInfo
                                {
                                    widgetName = name,
                                    position = GetWidgetLocalPosition(),
                                    selectionState = AppData.InputUIState.Highlighted
                                };

                                OnGoToProfile_ActionEvent(actionButton.dataPackets);
                            }
                            else
                                Debug.LogWarning("--> OnActionButtonInputs Failed : SceneAssetsManager.Instance Is Not Yet Initialized.");
                        }
                    }
                    else
                        Debug.LogWarning("--> OnActionButtonInputs Failed : SelectableManager.Instance Is Not Yet Initialized.");

                    break;
            }
        }

        void OnGoToProfile_ActionEvent(AppData.ButtonDataPackets dataPackets)
        {
            if (ScreenUIManager.Instance != null)
                ScreenUIManager.Instance.ShowScreen(dataPackets);
            else
                LogWarning("Screen Manager Missing.", this, () => OnGoToProfile_ActionEvent(dataPackets));
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

        protected override void OnSetFileData(AppData.SceneAsset assetData)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScreenUIRefreshed()
        {

        }

        protected override void OnSetUIWidgetData(AppData.FolderStructureData structureData)
        {
            if (!string.IsNullOrEmpty(structureData.projectInfo.name))
            {
                SetUITextDisplayerValue(structureData?.projectInfo?.name, AppData.ScreenTextType.TitleDisplayer);

                string lastModified = structureData?.creationDateTime.date;
                SetUITextDisplayerValue(lastModified, AppData.ScreenTextType.TimeDateDisplayer);

                string projectType = structureData?.projectInfo?.projectType.ToString().Replace("Project_", "");
                SetUITextDisplayerValue(projectType, AppData.ScreenTextType.TypeDisplayer);
            }
        }

        #endregion
    }
}
