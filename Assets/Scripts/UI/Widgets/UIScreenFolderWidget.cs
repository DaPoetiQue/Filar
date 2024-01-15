using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class UIScreenFolderWidget : AppData.SelectableWidget
    {
        #region Components

        #endregion


        #region Unity Callbacks

        void Start() => Initialization();

        #endregion

        #region Main

        void Initialization()
        {
         
        }

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
                case AppData.InputActionButtonType.OpenFolderButton:

                    if (SelectableManager.Instance != null)
                    {
                        if (SelectableManager.Instance.GetCurrentSelectionType() != AppData.FocusedSelectionType.SelectedItem)
                        {
                            if (SelectableManager.Instance.HasActiveSelection())
                                SelectableManager.Instance.OnClearFocusedSelectionsInfo();

                            if (AppDatabaseManager.Instance != null)
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
                SetUITextDisplayerValue(AppData.ScreenTextType.TitleDisplayer, folder.name);

                string fileCountString = folder.GetFileCount().ToString() + " Files";
                SetUITextDisplayerValue(AppData.ScreenTextType.FileCountDisplayer, fileCountString);

                SetUITextDisplayerValue(AppData.ScreenTextType.DateTimeDisplayer, folder?.creationDateTime?.dateTime);

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
                Debug.LogError("=============> Please Fix Here............. Important");
                //SelectableManager.Instance.Select(this, dataPackets, isInitialSelection);
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
