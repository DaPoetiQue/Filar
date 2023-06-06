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
            AppData.FolderNavigationCommand previousFolderCommand = new AppData.FolderNavigationCommand(SceneAssetsManager.Instance.GetCurrentFolder(), folderWidgetInfo, structureType);

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

                SceneAssetsManager.Instance.GetDynamicWidgetsContainer(AppData.ContentContainerType.FolderStuctureContent, folder =>
                {
                    if (AppData.Helpers.IsSuccessCode(folder.resultsCode))
                    {
                        if (SelectableManager.Instance != null)
                            SelectableManager.Instance.Select(folderNavigation.folderWidgetInfo.widgetName, AppData.FocusedSelectionType.InteractedItem, selectionCallback => { });
                        else
                        {
                            callbackResults.results = "Selectable Manager Instance Not Yet Initialized.";
                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                            callbackResults.data = default;
                        }
                    }
                    else
                        LogWarning(folder.results, this);
                });

                folderNavigation.Execute();

                callbackResults.results = "Success : Returning From Folder";
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;

                if (ScreenUIManager.Instance != null)
                {
                    if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
                    {
                        if (folderNavigationCommands.Count == 0)
                        {
                            //ScreenUIManager.Instance.GetCurrentScreenData().value.HideScreenWidget(AppData.WidgetType.FolderNavigationWidget);
                            UpdateNavigationRootTitleDisplayer();
                            folderNavigationDataPackets.widgetTitle = SceneAssetsManager.Instance.GetFolderStructureData().GetRootFolder().name;
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
                callbackResults.results = "Failed : There Are No Commands In folderNavigationCommands";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void HighligtEnteredFolder()
        {

        }

        public void UpdateNavigationRootTitleDisplayer()
        {
            string rootFolderName = SceneAssetsManager.Instance.GetFolderStructureData().GetRootFolder().name + "/";
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
                formattedTitle = SceneAssetsManager.Instance.GetFolderStructureData().GetRootFolder().name;

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
                Debug.LogWarning("--> UpdateNavigationRootTitleDisplayer Failed : ScreenUIManager.Instance.GetCurrentScreenData().value Is Missing / Not Found.");
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

        #endregion
    }
}
