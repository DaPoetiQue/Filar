using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class CreateNewProjectWidget : AppData.Widget
    {
        #region Components


        [Space(5)]
        [SerializeField]
        AppData.ProjectStructureData folderStructureDataTemplate = new AppData.ProjectStructureData();

        AppData.ProjectStructureData newProjectStructureData = new AppData.ProjectStructureData();

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

                        OnDataValidation(newProjectStructureData, dataValidCallbackResults => 
                        {
                            if (dataValidCallbackResults.Success())
                            {
                                OnInputFieldValidation(AppData.ValidationResultsType.Success, dataValidCallbackResults.data);

                                if (SceneAssetsManager.Instance != null)
                                {
                                    SceneAssetsManager.Instance.CreateNewProjectStructureData(newProjectStructureData, createNewProjectCallbackResults =>
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

        AppData.ProjectStructureData CreateNewFolderStructureData()
        {
            return new AppData.ProjectStructureData
            {
                projectInfo = new AppData.ProjectInfo(string.Empty, AppData.ProjectCategoryType.Project_3D), 
                rootFolder = folderStructureDataTemplate.rootFolder,
                excludedSystemFiles = folderStructureDataTemplate.excludedSystemFiles,
                excludedSystemFolders = folderStructureDataTemplate.excludedSystemFolders,
                layoutViewType = folderStructureDataTemplate.layoutViewType,
                paginationViewType = folderStructureDataTemplate.paginationViewType,
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

                    if (newProjectStructureData != null)
                        newProjectStructureData.name = value;
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
            newProjectStructureData = CreateNewFolderStructureData();
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

        void OnDataValidation(AppData.ProjectStructureData info, Action<AppData.CallbackData<AppData.InputFieldActionType>> callback)
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

                    int contentIndexA = SceneAssetsManager.Instance.GetDropdownContentCount<AppData.ProjectCategoryType>();
                    int contentIndexB = SceneAssetsManager.Instance.GetDropdownContentCount<AppData.ProjectCategoryType>("Project_", "All");

                    int index = value + SceneAssetsManager.Instance.GetDropdownContentIndex(contentIndexA, contentIndexB);
                    var categoryType = (AppData.ProjectCategoryType)index;

                    int projectIndex = SceneAssetsManager.Instance.GetDropdownContentTypeIndex(categoryType);

                    if (index.Equals(projectIndex))
                    {
                        if (newProjectStructureData != null)
                            newProjectStructureData.GetProjectInfo().SetCategoryType(categoryType);
                        else
                            LogError("New Folder Structure Data Is Null.", this);
                    }
                    else
                        LogWarning($"Project Type Index Is Invalid - Index Value {index} Is Not Equals To Project Type Index Value : {projectIndex}", this);

                    break;

                case AppData.InputDropDownActionType.ProjectTamplate:

                    var templateType = (AppData.ProjectTamplateType)value;

                    int templateIndex = SceneAssetsManager.Instance.GetDropdownContentTypeIndex(templateType);

                    if (value.Equals(templateIndex))
                    {
                        if (newProjectStructureData != null)
                            newProjectStructureData.projectInfo.templateType = templateType;
                        else
                            LogError("New Folder Structure Data Is Null.", this);
                    }
                    else
                        LogWarning($"Project Tamplate Type Index Is Invalid - Index Value {value} Is Not Equals To Project template Type Index Value : {templateIndex}", this);

                    break;
            }
        }

        #endregion

    }
}