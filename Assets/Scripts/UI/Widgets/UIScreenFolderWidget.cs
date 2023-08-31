using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class UIScreenFolderWidget : AppData.UIScreenWidget
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
            switch (actionButton.dataPackets.action)
            {
                case AppData.InputActionButtonType.OpenFolderButton:

                    if (SelectableManager.Instance != null)
                    {
                        if (SelectableManager.Instance.GetCurrentSelectionType() != AppData.FocusedSelectionType.SelectedItem)
                        {
                            if (SelectableManager.Instance.HasActiveSelection())
                                SelectableManager.Instance.OnClearFocusedSelectionsInfo();

                            if (DatabaseManager.Instance != null)
                            {
                                AppData.UIWidgetInfo selectedWidget = new AppData.UIWidgetInfo
                                {
                                    widgetName = name,
                                    position = GetWidgetLocalPosition(),
                                    selectionState = AppData.InputUIState.Highlighted
                                };

                                Debug.LogError("=============> Please Fix Here............. Important");
                                ScreenNavigationManager.Instance.NavigateToFolder(folderData, selectedWidget, actionButton.dataPackets.folderStructureType);
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

        protected override void OnSetUIWidgetData(AppData.Folder folder)
        {
            if (!string.IsNullOrEmpty(folder.storageData.projectDirectory))
            {
                SetUITextDisplayerValue(folder.name, AppData.ScreenTextType.TitleDisplayer);

                string fileCountString = folder.GetFileCount().ToString() + " Files";
                SetUITextDisplayerValue(fileCountString, AppData.ScreenTextType.FileCountDisplayer);

                SetUITextDisplayerValue(folder?.creationDateTime?.dateTime, AppData.ScreenTextType.TimeDateDisplayer);

                if (folder.GetFileCount() == 0)
                    SetUIImageDisplayerValue(AppData.UIImageDisplayerType.ItemThumbnail, AppData.UIImageType.EmptyFolderIcon);
                else if (folder.GetFileCount() == 1)
                    SetUIImageDisplayerValue(AppData.UIImageDisplayerType.ItemThumbnail, AppData.UIImageType.FolderIcon);
                else
                    SetUIImageDisplayerValue(AppData.UIImageDisplayerType.ItemThumbnail, AppData.UIImageType.MultiFilesFolderIcon);
            }
        }

        public override void OnSelect(bool isInitialSelection = false)
        {
            if (SelectableManager.Instance != null)
            {
                //Debug.LogError("=============> Please Fix Here............. Important");
                SelectableManager.Instance.Select(this, dataPackets, isInitialSelection);
                Selected();
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
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
