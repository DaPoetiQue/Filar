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

            var containerData = SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer;

            containerData.GetPlaceHolder(placeHolder =>
            {
                if (AppData.Helpers.IsSuccessCode(placeHolder.resultsCode))
                {
                    var placeHolderInfo = placeHolder.data.GetInfo();

                    if (placeHolderInfo.isActive)
                    {
                        SetWidgetPosition(placeHolderInfo.worldPosition);
                        SetWidgetSizeDelta(placeHolderInfo.dimensions);
                    }
                    else
                        Debug.LogWarning($"--> GetPlaceHolder Failed - Placeholder Is Not Active In The Scene.");
                }
                else
                    Debug.LogWarning($"--> GetPlaceHolder Failed With Results : {placeHolder.results}");
            });

            SceneAssetsManager.Instance.CreateNewFolderName = SceneAssetsManager.Instance.GetCreateNewFolderTempName();
            SetInputFieldValue(SceneAssetsManager.Instance.CreateNewFolderName, AppData.InputFieldActionType.AssetNameField);

            HighlightInputFieldValue(AppData.InputFieldActionType.AssetNameField);

            ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
        }

        protected override void OnHideScreenWidget()
        {
            if (ScreenUIManager.Instance != null)
            {
                if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
                {
                    if (SceneAssetsManager.Instance)
                    {
                        var widgetContainer = SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer;

                        if (widgetContainer != null)
                        {
                            widgetContainer.GetPlaceHolder(placeholder =>
                            {
                                if (AppData.Helpers.IsSuccessCode(placeholder.resultsCode))
                                {
                                    if (placeholder.data.IsActive())
                                        placeholder.data.ResetPlaceHolder();
                                    else
                                        Debug.LogWarning("--> Reset Place Holder Failed - Plave Holder Is Not Active In The Scene.");
                                }
                                else
                                    Debug.LogWarning($"--> Failed With Results : {placeholder.results}");
                            });
                        }
                        else
                            Debug.LogWarning("--> Get Placeholder Failed : Widgets Container Is Missing / Null.");
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

        #endregion
    }
}
