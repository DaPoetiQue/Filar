using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class CreateNewProjectWidget : AppData.Widget
    {
        #region Components


        [Space(5)]
        [SerializeField]
        AppData.FolderStructureData folderStructureDataTemplate = new AppData.FolderStructureData();

        AppData.FolderStructureData newFolderStructureData = new AppData.FolderStructureData();

        #endregion

        #region Unity Callbacks
        void Start() => Init();

        #endregion

        #region Main

        new void Init()
        {
            createNewProjectWidget = this;
            base.Init();
        }

        protected override void OnActionButtonEvent(AppData.WidgetType popUpType, AppData.InputActionButtonType actionType, AppData.SceneDataPackets dataPackets)
        {
            if (popUpType == type)
            {
                switch(actionType)
                {
                    case AppData.InputActionButtonType.Confirm:

                        OnDataValidation(newFolderStructureData, dataValidCallbackResults => 
                        {
                            if (dataValidCallbackResults.Success())
                            {
                                OnInputFieldValidation(AppData.ValidationResultsType.Success, dataValidCallbackResults.data);

                                if (SceneAssetsManager.Instance != null)
                                {
                                    SceneAssetsManager.Instance.CreateNewProjectData(newFolderStructureData, createNewProjectCallbackResults =>
                                    {
                                        if (createNewProjectCallbackResults.Success())
                                        {
                                            if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
                                                ScreenUIManager.Instance.GetCurrentScreenData().value.HideScreenWidget(dataPackets.widgetType, dataPackets);

                                            dataPackets.notification.message = createNewProjectCallbackResults.results;

                                            if (dataPackets.notification.showNotifications)
                                                NotificationSystemManager.Instance.ScheduleNotification(dataPackets.notification);

                                            ScreenUIManager.Instance.Refresh();
                                        }
                                        else
                                            Log(createNewProjectCallbackResults.resultsCode, createNewProjectCallbackResults.results, this);
                                    });
                                }
                                else
                                    LogError("Scene Assets Manager Instance Is Not Yet Initialized.", this);
                            }
                            else
                            {
                                OnInputFieldValidation(AppData.ValidationResultsType.Error, dataValidCallbackResults.data);

                                Log(dataValidCallbackResults.resultsCode, dataValidCallbackResults.results, this);
                            }
                        });

                        break;
                }
            }
        }

        AppData.FolderStructureData CreateNewFolderStructureData()
        {
            return new AppData.FolderStructureData
            {
                rootFolder = folderStructureDataTemplate.rootFolder,
                excludedSystemFiles = folderStructureDataTemplate.excludedSystemFiles,
                excludedSystemFolders = folderStructureDataTemplate.excludedSystemFolders,
                currentLayoutViewType = folderStructureDataTemplate.currentLayoutViewType,
                currentPaginationViewType = folderStructureDataTemplate.currentPaginationViewType,
                layouts = folderStructureDataTemplate.layouts,
            };
        }

        protected override void OnHideScreenWidget()
        {
            HideSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
        }

        protected override void OnInputFieldValueChanged(string value, AppData.InputFieldDataPackets dataPackets)
        {
            switch(dataPackets.action)
            {
                case AppData.InputFieldActionType.AssetNameField:

                    OnInputFieldValidation(AppData.ValidationResultsType.Default, AppData.InputFieldActionType.AssetNameField);

                    if (newFolderStructureData != null)
                        newFolderStructureData.name = value;
                    else
                        LogError("New Folder Structure Data Is Null.", this);

                    break;
            }
        }

        protected override void OnInputFieldValueChanged(int value, AppData.InputFieldDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScreenWidget()
        {
            newFolderStructureData = CreateNewFolderStructureData();
            OnClearInputFieldValidation(AppData.InputFieldActionType.AssetNameField);
        }

        protected override void OnShowScreenWidget(AppData.SceneDataPackets dataPackets) => ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);

        protected override void OnSubscribeToActionEvents(bool subscribe)
        {
            LogInfo($"===============> Subscribe : {subscribe}", this);
        }

        protected override void OnScrollerValueChanged(Vector2 value) => scroller.Update();

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        void OnDataValidation(AppData.FolderStructureData info, Action<AppData.CallbackData<AppData.InputFieldActionType>> callback)
        {
            bool isValidName = !string.IsNullOrEmpty(info.name);

            AppData.CallbackData<AppData.InputFieldActionType> callbackResults = new AppData.CallbackData<AppData.InputFieldActionType>();

            if (isValidName)
            {
                callbackResults.results = "Data Is Valid";
                callbackResults.data = AppData.InputFieldActionType.AssetNameField;
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = "Name Field Is required - Invalid";
                callbackResults.data = AppData.InputFieldActionType.AssetNameField;
                callbackResults.resultsCode = AppData.Helpers.WarningCode;
            }

            callback.Invoke(callbackResults);
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownDataPackets dataPackets)
        {
           switch(dataPackets.action)
            {
                case AppData.InputDropDownActionType.ProjectType:

                    int index = value + 1;
                    var categoryType = (AppData.ProjectCategoryType)index;

                    LogSuccess($"========================> Create Project Type : {categoryType}", this);

                    if (newFolderStructureData != null)
                        newFolderStructureData.projectInfo.projectType = categoryType;
                    else
                        LogError("New Folder Structure Data Is Null.", this);

                    break;

                case AppData.InputDropDownActionType.ProjectTamplate:

                    var templateType = (AppData.ProjectTamplateType)value;

                    if (newFolderStructureData != null)
                        newFolderStructureData.projectInfo.templateType = templateType;
                    else
                        LogError("New Folder Structure Data Is Null.", this);

                    break;
            }
        }

        #endregion

    }
}