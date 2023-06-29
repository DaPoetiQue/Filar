using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class UIAssetFileWidget : AppData.UIScreenWidget
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
            Init((initializationCallbackResults) =>
            {
                if (initializationCallbackResults.Success())
                {
                    if (screenManager == null)
                        screenManager = ScreenUIManager.Instance;
                    else
                        Debug.LogWarning($"--> Failed to Initialize Scene Asset UI With Results : {initializationCallbackResults.results}.");
                }
                else
                    LogError("Failed to Initialize Scene Asset UI.", this);
            });
        }

        protected override void OnActionButtonInputs(AppData.UIButton<AppData.ButtonDataPackets> actionButton)
        {
            switch (actionButton.dataPackets.action)
            {
                case AppData.InputActionButtonType.Delete:

                    if (SceneAssetsManager.Instance != null)
                    {
                        SceneAssetsManager.Instance.AddToSelectedSceneAsseList(assetData);
                        SceneAssetsManager.Instance.SetCurrentSceneAsset(assetData);
                    }
                    else
                        Debug.LogWarning("--> Assets Manager Not Initialized.");

                    if (widgetParentScreen != null)
                        widgetParentScreen.ShowWidget(actionButton.dataPackets);

                    break;

                case AppData.InputActionButtonType.Edit:

                    if (screenManager != null)
                    {
                        assetData.currentAssetMode = AppData.SceneAssetModeType.EditMode;

                        actionButton.dataPackets.sceneAsset = assetData;

                        if (SceneAssetsManager.Instance != null)
                            SceneAssetsManager.Instance.OnSceneAssetEditMode(actionButton.dataPackets);
                        else
                            Debug.LogWarning("--> RG_Unity - OnActionButtonInputs Failed : Scene Assets Manager Instance Is Not Yet Initialized.");

                        if (assetData.assetFields != null)
                            screenManager.ShowScreen(actionButton.dataPackets);
                        else
                            Debug.LogWarning("--> Scene Asset Data Invalid.");
                    }
                    else
                        Debug.LogWarning("--> Screen Manager Missing.");

                    break;

                case AppData.InputActionButtonType.OpenSceneAssetPreview:

                    if (SelectableManager.Instance != null)
                    {
                        if (SelectableManager.Instance.HasActiveSelection())
                            SelectableManager.Instance.OnClearFocusedSelectionsInfo();

                        if (screenManager != null)
                        {
                            assetData.currentAssetMode = AppData.SceneAssetModeType.PreviewMode;
                            actionButton.dataPackets.sceneAsset = assetData;

                            //if (SceneAssetsManager.Instance != null)
                            //    SceneAssetsManager.Instance.OnSceneAssetPreviewMode(actionButton.dataPackets);
                            //else
                            //    Debug.LogWarning("--> RG_Unity - OnActionButtonInputs Failed : Scene Assets Manager Instance Is Not Yet Initialized.");

                            LogSuccess($"===================> Show Widget : {actionButton.dataPackets.widgetType} - Blur Screen : {actionButton.dataPackets.blurScreen}", this);

                            if (widgetParentScreen != null)
                            {
                                widgetParentScreen.ShowWidget(actionButton.dataPackets, (results) =>
                                {
                                    if (results)
                                        AppData.ActionEvents.OnScreenChangeEvent(actionButton.dataPackets);
                                    else
                                        Debug.LogWarning("--> Widget Screen Not Showing For Some Reason!");
                                });

                            }
                            else
                                Debug.LogWarning($"--> RG_Unity - OpenSceneAssetPreview Failed : Widget Parent Screen Missing / Null.");

                            Debug.Log($"--> Open Scene Asset Preview : {actionButton.dataPackets.widgetType.ToString()}");

                        }
                        else
                            Debug.LogWarning("--> Screen Manager Missing.");
                    }
                    else
                        Debug.LogWarning("--> OnActionButtonInputs Failed : SelectableManager.Instance Is Not Yet Initialized.");

                    break;

                case AppData.InputActionButtonType.PlaceItemInAR:

                    Debug.Log("--> Placing Asset In AR");

                    if (screenManager != null)
                    {
                        assetData.currentAssetMode = AppData.SceneAssetModeType.ARMode;
                        actionButton.dataPackets.sceneAsset = assetData;

                        if (SceneAssetsManager.Instance != null)
                            SceneAssetsManager.Instance.OnSceneAssetPreviewMode(actionButton.dataPackets);
                        else
                            Debug.LogWarning("--> RG_Unity - OnActionButtonInputs Failed : Scene Assets Manager Instance Is Not Yet Initialized.");

                        Debug.Log($"--> Place Asset : {actionButton.dataPackets.sceneAsset.name} In AR ");

                    }
                    else
                        Debug.LogWarning("--> Screen Manager Missing.");

                    break;
            }
        }

        protected override void OnSetUIWidgetData(AppData.Folder folder)
        {
            throw new System.NotImplementedException();
        }

        public override void OnSelect(bool isInitialSelection)
        {
            if (GetWidgetContainer() != null && GetWidgetContainer().IsContainerActive())
            {
                if (GetActive())

                    if (SelectableManager.Instance != null)
                    {
                        Debug.LogError("===========> Please Fix Selection Here");
                        //SelectableManager.Instance.Select(this, dataPackets, isInitialSelection);
                        //Selected();
                    }
                    else
                        Debug.LogWarning("--> OnSelect Failed :  SelectableManager.Instance Is Not Yet initialized.");
            }
        }

        public override void OnDeselect() => Deselected();

        protected override void OnSetFileData(AppData.SceneAsset assetData)
        {
            LogSuccess("====================> Setting Asset Data");

            if (selectableComponent.selectableAssetType == AppData.SelectableAssetType.File)
            {
                // Set Thumbnail.
                if (assetData.assetFields.Count > 0)
                {
                    foreach (var field in assetData.assetFields)
                    {
                        if (field.fieldType == AppData.AssetFieldType.Thumbnail)
                        {
                            if (field.path != null)
                            {
                                Texture2D thumbnail = AppData.Helpers.LoadTextureFile(field.path);
                                SetUIImageDisplayerValue(AppData.Helpers.Texture2DToSprite(thumbnail), AppData.UIImageDisplayerType.ItemThumbnail);
                            }
                        }
                    }
                }

                // Set Info
                SetUITextDisplayerValue(assetData.name, AppData.ScreenTextType.TitleDisplayer);
                SetUITextDisplayerValue(assetData.description, AppData.ScreenTextType.InfoDisplayer);
                SetUITextDisplayerValue(assetData.creationDateTime.dateTime, AppData.ScreenTextType.TimeDateDisplayer);
            }
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
