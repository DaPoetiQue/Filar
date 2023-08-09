using System;
using System.Collections;
using System.Collections.Generic;
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
            if (popUpType == widgetType)
            {
                switch(actionType)
                {
                    case AppData.InputActionButtonType.ConfirmationButton:

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
                                            if (SceneAssetsManager.Instance.GetProjectRootStructureData().Success())
                                            {
                                                if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
                                                    ScreenUIManager.Instance.GetCurrentScreenData().value.HideScreenWidget(dataPackets.widgetType, dataPackets);

                                                StartCoroutine(OnCreatedAsync(createNewProjectCallbackResults.data.GetProjectInfo(), createdCallbackResults => 
                                                {
                                                    if (createdCallbackResults.Success())
                                                    {
                                                        dataPackets.notification.message = createNewProjectCallbackResults.result;

                                                        if (dataPackets.notification.showNotifications)
                                                            NotificationSystemManager.Instance.ScheduleNotification(dataPackets.notification);

                                                        ScreenUIManager.Instance.Refresh();
                                                    }
                                                    else
                                                        Log(createdCallbackResults.resultCode, createdCallbackResults.result, this);
                                                }));
                                            }
                                            else
                                                Log(SceneAssetsManager.Instance.GetProjectRootStructureData().resultCode, SceneAssetsManager.Instance.GetProjectRootStructureData().result, this);
                                        }
                                        else
                                            Log(createNewProjectCallbackResults.resultCode, createNewProjectCallbackResults.result, this);
                                    });
                                }
                                else
                                    LogError("Scene Assets Manager Instance Is Not Yet Initialized.", this);
                            }
                            else
                            {
                                OnInputFieldValidation(AppData.ValidationResultsType.Error, dataValidCallbackResults.data);

                                Log(dataValidCallbackResults.resultCode, dataValidCallbackResults.result, this);
                            }
                        });

                        break;
                }
            }
        }

        IEnumerator OnCreatedAsync(AppData.ProjectInfo projectInfo, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            yield return new WaitForEndOfFrame();

            var rootData = SceneAssetsManager.Instance.GetProjectRootStructureData().data;
            rootData.GetProjectCreationTemplateData().SetProjectInfo(projectInfo);

            SceneAssetsManager.Instance.SaveModifiedData(rootData, dataSavedCallbackResults =>
            {
                callbackResults.result = dataSavedCallbackResults.result;
                callbackResults.resultCode = dataSavedCallbackResults.resultCode;
            });

            yield return new WaitForEndOfFrame();

            callback?.Invoke(callbackResults);
        }

        void CreateNewFolderStructureData(Action<AppData.CallbackData<AppData.ProjectStructureData>> callback)
        {
            AppData.CallbackData<AppData.ProjectStructureData> callbackResults = new AppData.CallbackData<AppData.ProjectStructureData>();

            if (SceneAssetsManager.Instance.GetProjectRootStructureData().Success())
            {
                var project = new AppData.ProjectStructureData
                {
                    projectInfo = new AppData.ProjectInfo(string.Empty, SceneAssetsManager.Instance.GetProjectRootStructureData().data.projectCreationTemplateData.GetProjectInfo().GetCategoryType(), SceneAssetsManager.Instance.GetProjectRootStructureData().data.projectCreationTemplateData.GetProjectInfo().GetTamplateType()),
                    rootFolder = folderStructureDataTemplate.rootFolder,
                    excludedSystemFiles = folderStructureDataTemplate.excludedSystemFiles,
                    excludedSystemFolders = folderStructureDataTemplate.excludedSystemFolders,
                    layoutViewType = folderStructureDataTemplate.layoutViewType,
                    paginationViewType = folderStructureDataTemplate.paginationViewType,
                    layouts = folderStructureDataTemplate.layouts,
                };

                callbackResults.result = "New Project Data Created.";
                callbackResults.data = project;
                callbackResults.resultCode = SceneAssetsManager.Instance.GetProjectRootStructureData().resultCode;
            }
            else
            {
                callbackResults.result = SceneAssetsManager.Instance.GetProjectRootStructureData().result;
                callbackResults.data = default;
                callbackResults.resultCode = SceneAssetsManager.Instance.GetProjectRootStructureData().resultCode;
            }

            callback.Invoke(callbackResults);
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

                    if (newProjectStructureData != null)
                    {
                        newProjectStructureData.name = value;

                        OnDataValidation(newProjectStructureData, dataValidCallbackResults => 
                        {
                            if (dataValidCallbackResults.Success())
                            {
                                GetInputField(AppData.InputFieldActionType.AssetNameField, inputFieldCallbackResults =>
                                {
                                    if (inputFieldCallbackResults.Success())
                                    {
                                        if (inputFieldCallbackResults.data.GetValidationStateInfo().Results == AppData.ValidationResultsType.Warning || inputFieldCallbackResults.data.GetValidationStateInfo().Results == AppData.ValidationResultsType.Error)
                                            OnInputFieldValidation(AppData.ValidationResultsType.Success, dataValidCallbackResults.data);
                                    }
                                    else
                                        Log(inputFieldCallbackResults.resultCode, inputFieldCallbackResults.result, this);
                                });
                            }
                            else
                            {
                                GetInputField(AppData.InputFieldActionType.AssetNameField, inputFieldCallbackResults =>
                                {
                                    if (inputFieldCallbackResults.Success())
                                    {
                                        if (inputFieldCallbackResults.data.GetValidationStateInfo().Results == AppData.ValidationResultsType.Success)
                                            OnInputFieldValidation(AppData.ValidationResultsType.Warning, dataValidCallbackResults.data);
                                    }
                                    else
                                        Log(inputFieldCallbackResults.resultCode, inputFieldCallbackResults.result, this);
                                });
                            }
                        });
                    }
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
            CreateNewFolderStructureData(newProjectCallbackResults => 
            {
                if (newProjectCallbackResults.Success())
                {
                    AppManager.Instance.GetAppRestriction(AppData.AppRestrictionType.ProjectSupport, restrictionCallbackResults => 
                    {
                        if (restrictionCallbackResults.Success())
                        {
                            newProjectStructureData = newProjectCallbackResults.data;
         
                            SetInputFieldPlaceHolder(AppData.InputFieldActionType.AssetNameField, "Project Name");

                            OnClearInputFieldValue(AppData.InputFieldActionType.AssetNameField);
                            OnClearInputFieldValidation(AppData.InputFieldActionType.AssetNameField);

                            var projectTypeContentParam = SceneAssetsManager.Instance.GetUIScreenGroupContentTemplate("Project Type Content", AppData.InputType.DropDown, state: AppData.InputUIState.Disabled,  dropdownActionType: AppData.InputDropDownActionType.ProjectType, placeHolder: "Project Type");
                            var projectTemplateContentParam = SceneAssetsManager.Instance.GetUIScreenGroupContentTemplate("Project Template Content", AppData.InputType.DropDown, state: AppData.InputUIState.Enabled, dropdownActionType: AppData.InputDropDownActionType.ProjectTamplate, placeHolder: "Templates", contents: SceneAssetsManager.Instance.GetDropdownContent<AppData.ProjectTamplateType>().data);

                            switch (restrictionCallbackResults.data.GetProjectSupportType())
                            {
                                case AppData.AppProjectSupportType.Supports_3D:

                                    projectTypeContentParam.contents = new List<string> { "3D" };
                                    projectTypeContentParam.SetUIInputState(AppData.InputUIState.Disabled);

                                    break;

                                case AppData.AppProjectSupportType.Supports_AR:

                                    projectTypeContentParam.contents = new List<string> { "3D", "AR" };
                                    projectTypeContentParam.SetUIInputState(AppData.InputUIState.Enabled);

                                    break;

                                case AppData.AppProjectSupportType.Supports_VR:

                                    projectTypeContentParam.contents = new List<string> { "3D", "AR", "VR" };
                                    projectTypeContentParam.SetUIInputState(AppData.InputUIState.Enabled);

                                    break;
                            }

                            SetActionDropdownContent(projectTypeContentParam, projectTemplateContentParam);

                            var projectType = (int)newProjectStructureData.GetProjectInfo().GetCategoryType() - 1;

                            SetActionDropdownSelection(AppData.InputDropDownActionType.ProjectType, projectType);
                            SetActionDropdownSelection(AppData.InputDropDownActionType.ProjectTamplate, (int)newProjectStructureData.GetProjectInfo().GetTamplateType());
                        }
                        else
                            Log(restrictionCallbackResults.resultCode, restrictionCallbackResults.result, this);
                    });
                }
                else
                    Log(newProjectCallbackResults.resultCode, newProjectCallbackResults.result, this);
            });
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
                callbackResults.result = "Data Is Valid";
                callbackResults.data = AppData.InputFieldActionType.AssetNameField;
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = "Name Field Is required - Invalid";
                callbackResults.data = AppData.InputFieldActionType.AssetNameField;
                callbackResults.resultCode = AppData.Helpers.WarningCode;
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

        protected override void ScrollerPosition(Vector2 position)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}