using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class UIAssetFileWidget : AppData.SelectableWidget
    {
        #region Components

        #endregion

        #region Main

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket<AppData.SelectableWidgetType, AppData.WidgetType>>> callback)
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.SelectableWidgetType, AppData.WidgetType>>();

            // Initialize Assets.
            Init(initializationCallbackResults =>
            {
                callbackResults.SetResult(initializationCallbackResults);
            });

            callback.Invoke(callbackResults);
        }


        protected override AppData.CallbackData<AppData.WidgetStatePacket<AppData.SelectableWidgetType, AppData.WidgetType>> OnGetState()
        {
            return null;
        }


        protected override void OnActionButtonInputs(AppData.UIButton<AppData.ButtonConfigDataPacket> actionButton)
        {
            switch (actionButton.dataPackets.GetAction().GetData())
            {
                case AppData.InputActionButtonType.Delete:

                    if (AppDatabaseManager.Instance != null)
                    {
                        AppDatabaseManager.Instance.AddToSelectedSceneAsseList(assetData);
                        AppDatabaseManager.Instance.SetCurrentSceneAsset(assetData);
                    }
                    else
                        Debug.LogWarning("--> Assets Manager Not Initialized.");

                    if (widgetParentScreen != null)
                        widgetParentScreen.ShowWidget(actionButton.dataPackets);

                    break;

                case AppData.InputActionButtonType.Edit:

                    if (screenManager != null)
                    {
                        assetData.assetMode = AppData.AssetModeType.EditMode;

                        actionButton.dataPackets.sceneAsset = assetData;

                        if (AppDatabaseManager.Instance != null)
                            AppDatabaseManager.Instance.OnSceneAssetEditMode(actionButton.dataPackets);
                        else
                            Debug.LogWarning("--> RG_Unity - OnActionButtonInputs Failed : Scene Assets Manager Instance Is Not Yet Initialized.");

                        //if (assetData.assetFields != null)
                        //    screenManager.ShowScreenAsync(actionButton.dataPackets);
                        //else
                        //    Debug.LogWarning("--> Scene Asset Data Invalid.");
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
                            assetData.assetMode = AppData.AssetModeType.PreviewMode;
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
                        assetData.assetMode = AppData.AssetModeType.ARMode;
                        actionButton.dataPackets.sceneAsset = assetData;

                        if (AppDatabaseManager.Instance != null)
                            AppDatabaseManager.Instance.OnSceneAssetPreviewMode(actionButton.dataPackets);
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
            if (GetWidgetContainer() != null && GetWidgetContainer().GetActive().Success())
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

        protected override void OnSetAssetData(AppData.SceneAsset assetData)
        {
            LogSuccess("====================> Setting Asset Data");

            if (selectableComponent.selectableAssetType == AppData.SelectableWidgetType.Asset)
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
                                SetUIImageDisplayerValue(AppData.Helpers.GetSprite(thumbnail), AppData.UIImageDisplayerType.ItemThumbnail);
                            }
                        }
                    }
                }

                // Set Info
                SetUITextDisplayerValue(AppData.ScreenTextType.TitleDisplayer, assetData.name);
                SetUITextDisplayerValue(AppData.ScreenTextType.InfoDisplayer, assetData.description);
                SetUITextDisplayerValue(AppData.ScreenTextType.DateTimeDisplayer, assetData.creationDateTime.dateTime);
            }
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
