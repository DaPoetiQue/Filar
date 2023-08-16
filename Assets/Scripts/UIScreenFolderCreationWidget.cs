using System.Collections;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class UIScreenFolderCreationWidget : AppData.Widget
    {
        #region Components

        Coroutine showWidgetRoutine;

        #endregion

        #region Unity Callbacks
        void Start() => Init();

        #endregion

        #region Main

        new void Init()
        {
            folderCreationWidget = this;
            base.Init();
        }

        protected override void OnScreenWidget()
        {
            if (showWidgetRoutine != null)
            {
                StopCoroutine(showWidgetRoutine);
                showWidgetRoutine = null;
            }

            showWidgetRoutine = StartCoroutine(OnShowWidgetAsync());
        }

        // Get Default Folder Name From Scene Assets Folder Lists.
        protected override void OnShowScreenWidget(AppData.SceneDataPackets dataPackets)
        {

        }

        IEnumerator OnShowWidgetAsync()
        {
            yield return new WaitForEndOfFrame();

            DatabaseManager.Instance.GetContentContainer(containerCallbackResults => 
            {
                if (containerCallbackResults.Success())
                {
                    containerCallbackResults.data.GetPlaceHolder(placeHolderCallbackResults =>
                    {
                        if (placeHolderCallbackResults.Success())
                        {
                            var placeHolderInfo = placeHolderCallbackResults.data.GetInfo();

                            if (placeHolderInfo.isActive)
                            {
                                SetWidgetPosition(placeHolderInfo.worldPosition);
                                SetWidgetSizeDelta(placeHolderInfo.dimensions);
                            }
                            else
                                LogWarning($"GetPlaceHolder Failed - Placeholder Is Not Active In The Scene.", this);
                        }
                        else
                            Log(placeHolderCallbackResults.resultCode, placeHolderCallbackResults.result, this);
                    });

                    DatabaseManager.Instance.CreateNewFolderName = DatabaseManager.Instance.GetCreateNewFolderTempName();
                    SetInputFieldValue(AppData.InputFieldActionType.AssetNameField, DatabaseManager.Instance.CreateNewFolderName);

                    HighlightInputFieldValue(AppData.InputFieldActionType.AssetNameField);

                    ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
                }
                else
                    Log(containerCallbackResults.resultCode, containerCallbackResults.result, this);
            });
        }

        protected override void OnHideScreenWidget()
        {
            if (ScreenUIManager.Instance != null)
            {
                if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
                {
                    if (DatabaseManager.Instance)
                    {
                        DatabaseManager.Instance.GetContentContainer(containerCallbackResults => 
                        {
                            if (containerCallbackResults.Success())
                            {
                                containerCallbackResults.data.GetPlaceHolder(placeholderCallbackResults =>
                                {
                                    if (AppData.Helpers.IsSuccessCode(placeholderCallbackResults.resultCode))
                                    {
                                        if (placeholderCallbackResults.data.IsActive())
                                            placeholderCallbackResults.data.ResetPlaceHolder();
                                        else
                                            LogWarning("Reset Place Holder Failed - Plave Holder Is Not Active In The Scene.", this);
                                    }
                                    else
                                        Log(placeholderCallbackResults.resultCode, placeholderCallbackResults.result, this);
                                });
                            }
                            else
                                Log(containerCallbackResults.resultCode, containerCallbackResults.result, this);
                        });
                    }
                    else
                        Debug.LogWarning("--> Get Placeholder Failed : SceneAssetsManager.Instance Is Not Yet Initialized");
                }
                else
                    Debug.LogWarning("--> ScreenUIManager.Instance.GetCurrentScreenData Failed : Value Is Missing / Null.");
            }
            else
                Debug.LogWarning("--> GoToProfile Failed : ScreenUIManager.Instance Is Not Yet Initialized");

            HideSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
        }

        protected override void OnInputFieldValueChanged(string value, AppData.InputFieldDataPackets dataPackets)
        {
            DatabaseManager.Instance.CreateNewFolderName = value;
        }

        protected override void OnInputFieldValueChanged(int value, AppData.InputFieldDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnSubscribeToActionEvents(bool subscribe)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScrollerValueChanged(Vector2 value)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionButtonEvent(AppData.WidgetType popUpType, AppData.InputActionButtonType actionType, AppData.SceneDataPackets dataPackets)
        {
            
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void ScrollerPosition(Vector2 position)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
