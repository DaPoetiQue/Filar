using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace Com.RedicalGames.Filar
{
    public class SelectedSceneAssetPreviewWidget : AppData.Widget
    {
        #region Components

        [SerializeField]
        Image thumbnailDisplayer;

        [Space(5)]
        [SerializeField]
        TMP_Text descriptionDisplayer;

        [Space(5)]
        [SerializeField]
        List<AppData.ScreenTogglableWidget<GameObject>> screenTogglableWidgetsList = new List<AppData.ScreenTogglableWidget<GameObject>>();

        AppData.SceneAsset sceneAsset;

        bool canResetAssetPose = false;

        #endregion

        #region Unity Callbacks

        void OnEnable() => OnSubscribeToEvents(true);

        void OnDisable() => OnSubscribeToEvents(false);

        void Start() => Init();

        #endregion

        #region Initializations

        new void Init()
        {
            selectedSceneAssetPreviewWidget = this;

            if (thumbnailDisplayer == null)
            {
                LogWarning("Thumbnail Displayer Missing.", this, () => Init());
                return;
            }

            if (titleDisplayer == null)
            {
                LogWarning("Title Displayer Missing.", this, () => Init());
                return;
            }

            if (descriptionDisplayer == null)
            {
                LogWarning("Description Displayer Missing.", this, () => Init());
                return;
            }

            base.Init();
        }

        void OnSubscribeToEvents(bool subscribe)
        {
            if (subscribe)
            {
                AppData.ActionEvents._OnScreenTogglableStateEvent += OnScreenTogglableStateEvent;
                AppData.ActionEvents._OnSceneModelPoseResetEvent += OnAssetPoseReset;
            }
            else
            {
                AppData.ActionEvents._OnScreenTogglableStateEvent -= OnScreenTogglableStateEvent;
                AppData.ActionEvents._OnSceneModelPoseResetEvent -= OnAssetPoseReset;
            }
        }


        #endregion

        #region Main

        void SetWidgetAssetData(AppData.SceneAsset asset)
        {
            AppData.Helpers.ShowImage(asset, thumbnailDisplayer);

            if (titleDisplayer != null && !string.IsNullOrEmpty(asset.name))
                titleDisplayer.text = asset.name;

            if (descriptionDisplayer != null && !string.IsNullOrEmpty(asset.description))
                descriptionDisplayer.text = asset.description;

            if (SelectableManager.Instance != null)
            {
                SelectableManager.Instance.Select(asset, AppData.FocusedSelectionType.InteractedItem, selectionCallback =>
                {
                    if (AppData.Helpers.IsSuccessCode(selectionCallback.resultsCode))
                    {
                        LogSuccess(selectionCallback.results, this, () => OnHideScreenWidget());
                    }
                    else
                        LogError(selectionCallback.results, this, () => OnHideScreenWidget());
                });
            }
            else
                LogError("Selectable Manager Instance Is Not Yet Initialized.", this, () => OnHideScreenWidget());
        }

        void OnScreenTogglableStateEvent(AppData.TogglableWidgetType widgetType, bool state = false, bool useInteractability = false)
        {
            if (!state)
            {
                AppData.ScreenTogglableWidget<GameObject> screenTogglableWidget = screenTogglableWidgetsList.Find((x) => x.widgetType == widgetType);

                if (screenTogglableWidget.value != null)
                {
                    if (useInteractability)
                    {
                        screenTogglableWidget.Interactable(state);
                    }
                    else
                    {
                        if (state)
                            screenTogglableWidget.Show();
                        else
                            screenTogglableWidget.Hide();
                    }
                }
                else
                    LogWarning("Screen Togglable Widget Value Is Null.", this, () => OnScreenTogglableStateEvent(widgetType, state = false, useInteractability = false));

                canResetAssetPose = false;
            }
            else
            {
                if (!canResetAssetPose)
                {
                    AppData.ScreenTogglableWidget<GameObject> screenTogglableWidget = screenTogglableWidgetsList.Find((x) => x.widgetType == widgetType);

                    if (screenTogglableWidget.value != null)
                    {
                        if (useInteractability)
                            screenTogglableWidget.Interactable(state);
                        else
                        {
                            if (state)
                                screenTogglableWidget.Show();
                            else
                                screenTogglableWidget.Hide();
                        }

                        canResetAssetPose = true;
                    }
                    else
                        Debug.LogError("--> Screen Togglable Widget Value Is Null.");
                }
            }
        }

        void OnAssetPoseReset()
        {
            OnScreenTogglableStateEvent(AppData.TogglableWidgetType.ResetAssetModelRotationButton, false);
            canResetAssetPose = false;
        }


        #region Overrides

        protected override void OnScreenWidget()
        {
          
        }

        protected override void OnShowScreenWidget(AppData.SceneDataPackets dataPackets)
        {
            ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);

            switch (transitionType)
            {
                case AppData.TransitionType.Default:

                    if (GetLayoutView(defaultLayoutType).layout)
                        OnScreenTogglableStateEvent(AppData.TogglableWidgetType.ResetAssetModelRotationButton, false);
                    else
                        LogError("Pop Up Value Required.", this, () => OnShowScreenWidget(dataPackets));

                    break;

                case AppData.TransitionType.Translate:

                    break;
            }

            if (SceneAssetsManager.Instance)
                SetWidgetAssetData(SceneAssetsManager.Instance.GetCurrentSceneAsset());
            else
                LogWarning("Scene Assets Manager Not Yet Initialized.", this, () => OnShowScreenWidget(dataPackets));
        }

        protected override void OnHideScreenWidget()
        {
            HideSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);

            ScreenUIManager.Instance.Refresh();
        }

        protected override void OnInputFieldValueChanged(string value, AppData.InputFieldDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #endregion
    }
}
