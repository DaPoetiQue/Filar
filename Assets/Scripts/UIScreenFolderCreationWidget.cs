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

            SceneAssetsManager.Instance.GetContentContainer(containerCallbackResults => 
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
                            Log(placeHolderCallbackResults.resultsCode, placeHolderCallbackResults.results, this);
                    });

                    SceneAssetsManager.Instance.CreateNewFolderName = SceneAssetsManager.Instance.GetCreateNewFolderTempName();
                    SetInputFieldValue(AppData.InputFieldActionType.AssetNameField, SceneAssetsManager.Instance.CreateNewFolderName);

                    HighlightInputFieldValue(AppData.InputFieldActionType.AssetNameField);

                    ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
                }
                else
                    Log(containerCallbackResults.resultsCode, containerCallbackResults.results, this);
            });
        }

        protected override void OnHideScreenWidget()
        {
            if (ScreenUIManager.Instance != null)
            {
                if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
                {
                    if (SceneAssetsManager.Instance)
                    {
                        SceneAssetsManager.Instance.GetContentContainer(containerCallbackResults => 
                        {
                            if (containerCallbackResults.Success())
                            {
                                containerCallbackResults.data.GetPlaceHolder(placeholderCallbackResults =>
                                {
                                    if (AppData.Helpers.IsSuccessCode(placeholderCallbackResults.resultsCode))
                                    {
                                        if (placeholderCallbackResults.data.IsActive())
                                            placeholderCallbackResults.data.ResetPlaceHolder();
                                        else
                                            LogWarning("Reset Place Holder Failed - Plave Holder Is Not Active In The Scene.", this);
                                    }
                                    else
                                        Log(placeholderCallbackResults.resultsCode, placeholderCallbackResults.results, this);
                                });
                            }
                            else
                                Log(containerCallbackResults.resultsCode, containerCallbackResults.results, this);
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
            SceneAssetsManager.Instance.CreateNewFolderName = value;
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

        #endregion
    }
}
