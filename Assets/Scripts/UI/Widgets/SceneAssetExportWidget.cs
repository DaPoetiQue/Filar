using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Com.RedicalGames.Filar
{
    public class SceneAssetExportWidget : AppData.Widget
    {
        #region Components

        [Space(5)]
        [SerializeField]
        Image thumbnailDisplayer;

        [Space(5)]
        [SerializeField]
        AppData.UIInputField<AppData.SceneDataPackets> exportedAssetNameInputField = new AppData.UIInputField<AppData.SceneDataPackets>();

        [Space(5)]
        [SerializeField]
        AppData.UIDropDown<AppData.DropdownDataPackets> exportExtensionDropDown = new AppData.UIDropDown<AppData.DropdownDataPackets>();

        AppData.AssetExportData assetExportData = new AppData.AssetExportData();

        #endregion

        #region Unity Callbacks

        void Start() => Init();

        #endregion

        #region Main

        new void Init()
        {
            sceneAssetExportWidget = this;

            if (exportedAssetNameInputField.value != null)
                exportedAssetNameInputField.value.onValueChanged.AddListener((value) => OnInputAssetNameFieldValueChangeEvent(value));
            else
                Debug.LogWarning("--> OnShowScreenWidget Failed : Exported Asset Name Input Field Value Is Missing / Null.");

            Invoke("InitializeDropDownContent", initializationDelay);

            base.Init();
        }

        void InitializeDropDownContent()
        {
            if (exportExtensionDropDown.value != null)
            {
                if (DatabaseManager.Instance != null)
                {
                    var content = DatabaseManager.Instance.GetDropdownContent<AppData.ExportExtensionType>();

                    if (content.data != null)
                    {
                        exportExtensionDropDown.value.ClearOptions();

                        List<TMP_Dropdown.OptionData> dropdownOption = new List<TMP_Dropdown.OptionData>();

                        foreach (var extension in content.data)
                            dropdownOption.Add(new TMP_Dropdown.OptionData() { text = extension });

                        exportExtensionDropDown.value.AddOptions(dropdownOption);

                        exportExtensionDropDown.value.onValueChanged.AddListener((value) => OnDropDownExtensionsOptions(value));
                    }
                    else
                        Debug.LogWarning("--> SelectedSceneAssetPreviewWidget : Export Extension Drop Down Extensions List Not Found In Scene Assets Manager.");
                }
                else
                    Debug.LogWarning("--> SelectedSceneAssetPreviewWidget : Scene Assets Manager Instance Not Yet Initialized.");
            }
            else
            {
                Debug.LogWarning("--> SelectedSceneAssetPreviewWidget : Export Extension Drop Down Value Missing.");
                return;
            }
        }

        void OnDropDownExtensionsOptions(int dropdownIndex)
        {
            assetExportData.exportExtension = (AppData.ExportExtensionType)dropdownIndex;

            if (DatabaseManager.Instance)
                DatabaseManager.Instance.SetCurrentAssetExportData(assetExportData);
            else
                Debug.LogWarning("--> RG_Unity - OnDropDownExtensionsOptions Failed : Scene Assets Manager Instance Not Yet Initialized.");
        }

        void OnInputAssetNameFieldValueChangeEvent(string value)
        {
            assetExportData.name = value;

            if (DatabaseManager.Instance)
            {
                DatabaseManager.Instance.SetCurrentAssetExportData(assetExportData);
            }
            else
                Debug.LogWarning("--> RG_Unity - OnDropDownExtensionsOptions Failed : Scene Assets Manager Instance Not Yet Initialized.");
        }

        protected override void OnScreenWidget()
        {

        }
        void SetWidgetAssetData(AppData.SceneAsset asset)
        {
            AppData.Helpers.ShowImage(asset, thumbnailDisplayer);

            if (titleDisplayer != null && !string.IsNullOrEmpty(asset.name))
                titleDisplayer.text = asset.name;
        }

        protected override void OnShowScreenWidget(AppData.SceneDataPackets dataPackets)
        {
            ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);

            switch (transitionType)
            {
                case AppData.TransitionType.Default:

                    if (GetLayoutView(defaultLayoutType).layout)
                    {
                        if (exportedAssetNameInputField.value != null)
                        {
                            exportedAssetNameInputField.value.text = DatabaseManager.Instance.GetCurrentSceneAsset().name;
                        }
                        else
                            Debug.LogWarning("--> OnShowScreenWidget Failed : Exported Asset Name Input Field Value Is Missing / Null.");

                        if (DatabaseManager.Instance)
                        {
                            if (DatabaseManager.Instance.GetCurrentSceneAsset().sceneObject.value != null)
                            {
                                assetExportData.name = DatabaseManager.Instance.GetCurrentSceneAsset().name;
                                assetExportData.value = DatabaseManager.Instance.GetCurrentSceneAsset().sceneObject.value;
                            }
                            else
                                Debug.LogWarning("--> RG_Unity - OnShowScreenWidget Failed : Scene Assets Manager Instance's Get Current Scene Asset Scene Object Value Is Missing / Null");

                            DatabaseManager.Instance.SetCurrentAssetExportData(assetExportData);
                        }
                        else
                            Debug.LogWarning("--> RG_Unity - OnDropDownExtensionsOptions Failed : Scene Assets Manager Instance Not Yet Initialized.");
                    }
                    else
                        Debug.LogWarning("--> Pop Up Value Required.");

                    break;

                case AppData.TransitionType.Translate:

                    break;
            }

            if (DatabaseManager.Instance)
                SetWidgetAssetData(DatabaseManager.Instance.GetCurrentSceneAsset());
            else
                Debug.LogWarning("--> Scene Assets Manager Not Yet Initialized.");
        }

        protected override void OnHideScreenWidget()
        {
            HideSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
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

        protected override void ScrollerPosition(Vector2 position)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}