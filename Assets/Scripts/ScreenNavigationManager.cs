using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class ScreenNavigationManager : AppMonoBaseClass
    {
        #region Static


        private static ScreenNavigationManager _instance;

        public static ScreenNavigationManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<ScreenNavigationManager>();

                return _instance;
            }
        }

        #endregion

        #region Components

        [SerializeField]
        Stack<AppData.FolderNavigationCommand> folderNavigationCommands = new Stack<AppData.FolderNavigationCommand>();

        [Space(5)]
        [SerializeField]
        AppData.SceneDataPackets folderNavigationDataPackets = new AppData.SceneDataPackets();

        [Space(5)]
        [SerializeField]
        AppData.SceneDataPackets emptyFolderDataPackets = new AppData.SceneDataPackets();

        [Space(5)]
        [SerializeField]
        AppData.SceneDataPackets pagerNavigationWidgetDataPackets = new AppData.SceneDataPackets();

        [Space(5)]
        [SerializeField]
        AppData.SceneDataPackets scrollerNavigationWidgetDataPackets = new AppData.SceneDataPackets();

        AppData.UIWidgetInfo currentUIFolderWidgetInfo = new AppData.UIWidgetInfo();
        List<string> folderNavigationNameList = new List<string>();

        #endregion

        #region Main

        public void NavigateToFolder(AppData.Folder folder, AppData.UIWidgetInfo folderWidgetInfo, AppData.FolderStructureType structureType)
        {
            // Store Previous Folder
            AppData.FolderNavigationCommand previousFolderCommand = new AppData.FolderNavigationCommand(DatabaseManager.Instance.GetCurrentFolder(), folderWidgetInfo, structureType);

            if (!folderNavigationCommands.Contains(previousFolderCommand))
            {
                folderNavigationCommands.Push(previousFolderCommand);
                folderNavigationNameList.Add(folderWidgetInfo.GetWidgetName().Replace("_FolderData", ""));
            }

            currentUIFolderWidgetInfo = folderWidgetInfo;

            // Open Selected Folder
            AppData.FolderNavigationCommand navigationCommand = new AppData.FolderNavigationCommand(folder, folderWidgetInfo, structureType);
            navigationCommand.Execute();
        }

        public void ReturnFromFolder(Action<AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>>> callback)
        {
            AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>> callbackResults = new AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>>();

            if (folderNavigationCommands.Count > 0)
            {
                // Get Previous Folder
                AppData.FolderNavigationCommand folderNavigation = folderNavigationCommands.Pop();

                if (folderNavigationNameList.Count > 0)
                {
                    string folderName = folderNavigation.folderWidgetInfo.GetWidgetName().Replace("_FolderData", "");

                    if (!string.IsNullOrEmpty(folderName) && folderNavigationNameList.Contains(folderName))
                        folderNavigationNameList.Remove(folderName);

                    SelectableManager.Instance.Select(folderNavigation.folderWidgetInfo.widgetName, AppData.FocusedSelectionType.InteractedItem, selectionCallback => { });
                }

                DatabaseManager.Instance.GetDynamicContainer<DynamicWidgetsContainer>(AppData.ContentContainerType.FolderStuctureContent, folder =>
                {
                    if (AppData.Helpers.IsSuccessCode(folder.resultCode))
                    {
                        if (SelectableManager.Instance != null)
                            SelectableManager.Instance.Select(folderNavigation.folderWidgetInfo.widgetName, AppData.FocusedSelectionType.InteractedItem, selectionCallback => { });
                        else
                        {
                            callbackResults.result = "Selectable Manager Instance Not Yet Initialized.";
                            callbackResults.resultCode = AppData.Helpers.ErrorCode;
                            callbackResults.data = default;
                        }
                    }
                    else
                        LogWarning(folder.result, this);
                });

                folderNavigation.Execute();

                callbackResults.result = "Success : Returning From Folder";
                callbackResults.resultCode = AppData.Helpers.SuccessCode;

                if (ScreenUIManager.Instance != null)
                {
                    if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
                    {
                        if (folderNavigationCommands.Count == 0)
                        {
                            //ScreenUIManager.Instance.GetCurrentScreenData().value.HideScreenWidget(AppData.WidgetType.FolderNavigationWidget);
                            UpdateNavigationRootTitleDisplayer();

                            if (DatabaseManager.Instance.GetProjectStructureData().Success())
                                folderNavigationDataPackets.widgetTitle = DatabaseManager.Instance.GetProjectStructureData().data.GetRootFolder().name;
                            else
                                Log(DatabaseManager.Instance.GetProjectStructureData().resultCode, DatabaseManager.Instance.GetProjectStructureData().result, this);
                        }
                    }
                    else
                        Debug.LogWarning("--> ReturnFromFolder's hide Screen Widget Failed : ScreenUIManager.Instance Current Screen Value Is Missing / Not Found.");
                }
                else
                    Debug.LogWarning("--> ReturnFromFolder's hide Screen Widget Failed : ScreenUIManager.Instance Is Not Yet Initialized.");

                //if(folderNavigationCommands.Count == 0)
                //    ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonUIImageValue(AppData.InputActionButtonType.Return, AppData.UIImageDisplayerType.ButtonIcon, AppData.UIImageType.HomeIcon);
            }
            else
            {
                callbackResults.result = "Failed : There Are No Commands In folderNavigationCommands";
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void HighligtEnteredFolder()
        {

        }

        public void UpdateNavigationRootTitleDisplayer()
        {
            if (DatabaseManager.Instance.GetProjectStructureData().Success())
            {
                string rootFolderName = DatabaseManager.Instance.GetProjectStructureData().data.GetRootFolder().name + "/";
                string formattedTitle = string.Empty;

                if (folderNavigationNameList.Count == 1)
                    formattedTitle = rootFolderName + folderNavigationNameList[0];
                else if (folderNavigationNameList.Count > 1)
                {
                    string folderDirectoryIndexedWidgetName = rootFolderName + folderNavigationNameList[0];

                    for (int i = 0; i < folderNavigationNameList.Count; i++)
                        if (i != 0)
                            folderDirectoryIndexedWidgetName += "/" + folderNavigationNameList[i];

                    formattedTitle = folderDirectoryIndexedWidgetName;
                }
                else
                    formattedTitle = DatabaseManager.Instance.GetProjectStructureData().data.GetRootFolder().name;

                if (formattedTitle.Length > folderNavigationDataPackets.widgetTitleCharacterLimit)
                {
                    string trimmedWidgetTitle = formattedTitle.Substring(formattedTitle.Length - folderNavigationDataPackets.widgetTitleCharacterLimit, folderNavigationDataPackets.widgetTitleCharacterLimit);
                    string output = rootFolderName + ".../" + trimmedWidgetTitle.Substring(trimmedWidgetTitle.IndexOf("/") + 1);
                    formattedTitle = output;
                }

                if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
                {
                    ScreenUIManager.Instance.GetCurrentScreenData().value.SetUITextDisplayerValue(AppData.ScreenTextType.NavigationRootTitleDisplayer, formattedTitle);

                    if (folderNavigationNameList.Count == 0)
                        ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.Return, AppData.InputUIState.Hidden);
                    else
                        ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.Return, AppData.InputUIState.Shown);
                }
                else
                    LogWarning("Update Navigation Root Title Displayer Failed : Screen UI Manager Instance - Get Current Screen Data's Value Is Missing / Not Found.", this);
            }
            else
                Log(DatabaseManager.Instance.GetProjectStructureData().resultCode, DatabaseManager.Instance.GetProjectStructureData().result, this);
        }

        public AppData.SceneDataPackets GetEmptyFolderDataPackets()
        {
            return emptyFolderDataPackets;
        }


        public AppData.SceneDataPackets GetPagerNavigationWidgetDataPackets()
        {
            return pagerNavigationWidgetDataPackets;
        }

        public AppData.SceneDataPackets GetScrollerNavigationWidgetDataPackets()
        {
            return scrollerNavigationWidgetDataPackets;
        }

        public void GetEmptyContentDataPacketsForScreen(AppData.UIScreenType screenType, AppData.Folder contentFolder, Action<AppData.CallbackData<AppData.SceneDataPackets>> callback)
        {
            AppData.CallbackData<AppData.SceneDataPackets> callbackResults = new AppData.CallbackData<AppData.SceneDataPackets>();

            AppData.Helpers.GetComponent(ScreenUIManager.Instance, validComponentCallbackResults => 
            {
                callbackResults.result = validComponentCallbackResults.result;
                callbackResults.resultCode = validComponentCallbackResults.resultCode;
            
                if(callbackResults.Success())
                {
                    callbackResults.result = ScreenUIManager.Instance.HasCurrentScreen().result;
                    callbackResults.resultCode = ScreenUIManager.Instance.HasCurrentScreen().resultCode;

                    if (callbackResults.Success())
                    {
                        if (screenType == ScreenUIManager.Instance.HasCurrentScreen().data.value.GetUIScreenType())
                        {
                            AppData.SceneDataPackets dataPackets = GetEmptyFolderDataPackets();

                            dataPackets.screenType = screenType;
                            dataPackets.isRootFolder = contentFolder.IsRootFolder();

                            switch (screenType)
                            {
                                case AppData.UIScreenType.ProjectCreationScreen:

                                    dataPackets.popUpMessage = (dataPackets.isRootFolder) ? "There Are No Projects Found" : "Project Is Empty";

                                    dataPackets.referencedActionButtonDataList = new List<AppData.ReferencedActionButtonData>
                                    {
                                            new AppData.ReferencedActionButtonData
                                            {
                                                title = (dataPackets.isRootFolder)? "Create New" : "Delete",
                                                type = AppData.InputActionButtonType.FolderActionButton,
                                                state = AppData.InputUIState.Enabled
                                            }
                                    };

                                    break;

                                case AppData.UIScreenType.ProjectDashboardScreen:

                                    dataPackets.popUpMessage = (dataPackets.isRootFolder) ? "There's No Content Found. Create New" : "Folder Is Empty";

                                    dataPackets.referencedActionButtonDataList = new List<AppData.ReferencedActionButtonData>
                                    {
                                            new AppData.ReferencedActionButtonData
                                            {
                                                title = (dataPackets.isRootFolder)? "Create New" : "Delete",
                                                type = AppData.InputActionButtonType.FolderActionButton,
                                                state = AppData.InputUIState.Enabled
                                            }
                                    };

                                    break;
                            }

                            callbackResults.result = $"Empty Content Data Packets For Screen Type : {screenType} Found.";
                            callbackResults.data = dataPackets;
                        }
                        else
                        {
                            callbackResults.result = $"Requested Data Packets For Screen Type : {screenType} Not Found - Scrren Type Mismatched - Current Found Screen Is Of Type : {ScreenUIManager.Instance.HasCurrentScreen().data.value.GetUIScreenType()}";
                            callbackResults.data = default;
                            callbackResults.resultCode = AppData.Helpers.ErrorCode;
                        }
                    }
                }
            });

            callback.Invoke(callbackResults);
        }

        #endregion
    }
}
