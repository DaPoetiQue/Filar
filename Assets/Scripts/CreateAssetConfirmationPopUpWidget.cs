using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

namespace Com.RedicalGames.Filar
{
    public class CreateAssetConfirmationPopUpWidget : AppData.Widget
    {
        #region Components

        [Space(5)]
        [SerializeField]
        Image thumbnailDisplayer;

        [Space(5)]
        [SerializeField]
        TMP_InputField nameInputField;

        [Space(5)]
        [SerializeField]
        TMP_InputField descriptionInputField;

        [Space(5)]
        [SerializeField]
        TMP_Dropdown assetCategoryDropdown;

        [Space(5)]
        [SerializeField]
        TMP_Text submitButtonText;

        [Space(5)]
        [SerializeField]
        string defaultSubmitButtonTitle = "Create",
               updateAssetSubmitButtonTitle = "Update";

        RectTransform screenRect;

        #endregion

        #region Unity Callbacks

        void Start() => Init();

        #endregion

        #region Main

        new void Init()
        {
            createAssetConfirmationWidget = this;

            if (nameInputField != null)
                nameInputField.onValueChanged.AddListener((value) => OnSceneAssetNameFieldValueChanged(value));
            else
                Debug.LogWarning("--> Scene Asset Input Name Field Missing / Not Assigned In The Inspector Panel.");

            if (descriptionInputField != null)
                descriptionInputField.onValueChanged.AddListener((value) => OnSceneAssetDescriptionFieldValueChanged(value));
            else
                Debug.LogWarning("--> Scene Asset Input Name Field Missing / Not Assigned In The Inspector Panel.");

            if (DatabaseManager.Instance != null)
            {
                var content = DatabaseManager.Instance.GetDropdownContent<AppData.AssetCategoryType>();

                if (content.data != null)
                {
                    if (assetCategoryDropdown != null)
                    {
                        List<TMP_Dropdown.OptionData> dropdownOption = new List<TMP_Dropdown.OptionData>();

                        foreach (var category in content.data)
                            dropdownOption.Add(new TMP_Dropdown.OptionData() { text = category });

                        assetCategoryDropdown.AddOptions(dropdownOption);

                        assetCategoryDropdown.onValueChanged.AddListener((value) => OnAssetCategorySelectionDropdownValueChanged(value));
                    }
                    else
                        LogError("Category Drop Down Missing / Not Assigned In The Editor Inspector Panel.", this);
                }
                else
                    LogError("Scene Asset Category Content Missing / Not Found.", this);
            }
            else
                LogError("Scene Asset Manager Instance Not Initialized.", this);

            if (GetLayoutView().layout.GetComponent<RectTransform>())
                screenRect = GetLayoutView().layout.GetComponent<RectTransform>();
            else
                Debug.LogWarning("Init : Value Doesn't Have A Rect Transform Component.");

            base.Init();
        }

        protected override void OnScreenWidget()
        {
            if (DatabaseManager.Instance)
            {
                if (!string.IsNullOrEmpty(DatabaseManager.Instance.GetCurrentSceneAsset().name))
                {
                    if (titleDisplayer)
                        titleDisplayer.text = DatabaseManager.Instance.GetCurrentSceneAsset().name;
                    else
                        Debug.LogWarning("--> Pop Up Title Displayer Missing / Not Assigned In The Inspector Panel.");

                    if (DatabaseManager.Instance.GetCurrentSceneAsset().assetMode == AppData.AssetModeType.CreateMode)
                        ClearInputFields();

                    if (DatabaseManager.Instance.GetCurrentSceneAsset().assetMode == AppData.AssetModeType.EditMode)
                    {
                        if (nameInputField)
                            nameInputField.text = DatabaseManager.Instance.GetCurrentSceneAsset().name;

                        if (descriptionInputField)
                            descriptionInputField.text = DatabaseManager.Instance.GetCurrentSceneAsset().description;

                        if (submitButtonText != null)
                            submitButtonText.text = updateAssetSubmitButtonTitle;

                        if (assetCategoryDropdown != null)
                            assetCategoryDropdown.value = (int)DatabaseManager.Instance.GetCurrentSceneAsset().categoryType;
                    }

                    if (!string.IsNullOrEmpty(DatabaseManager.Instance.GetCurrentSceneAsset().GetAssetField(AppData.AssetFieldType.Thumbnail).path))
                    {
                        if (thumbnailDisplayer)
                        {
                            thumbnailDisplayer.sprite = AppData.Helpers.Texture2DToSprite(AppData.Helpers.LoadTextureFile(DatabaseManager.Instance.GetCurrentSceneAsset().GetAssetField(AppData.AssetFieldType.Thumbnail).path));
                        }
                        else
                            Debug.LogWarning("--> Pop Up Thumbanil Displayer Missing / Not Assigned In The Inspector Panel.");
                    }
                    else
                        Debug.LogWarning("--> Scene Asset Thumnail Not Assigned.");
                }
                else
                    Debug.LogWarning("--> Scene Asset Not Yet Initialized");
            }
            else
                Debug.LogWarning("--> Assets Manager Not Yet Initialized");
        }

        void ClearInputFields()
        {
            if (nameInputField != null)
                nameInputField.text = string.Empty;
            else
                Debug.LogWarning("--> Pop Up Name Input Field Missing / Not Assigned In The Inspector Panel.");

            if (descriptionInputField != null)
                descriptionInputField.text = string.Empty;
            else
                Debug.LogWarning("--> Pop Up Description Input Field Missing / Not Assigned In The Inspector Panel.");

            if (submitButtonText != null)
                submitButtonText.text = defaultSubmitButtonTitle;
            else
                Debug.LogWarning("--> Pop Up Submit Button Text Missing / Not Assigned In The Inspector Panel.");

            if (assetCategoryDropdown != null)
                assetCategoryDropdown.value = 0;
            else
                Debug.LogWarning("--> Pop Up Asset Category Selection Dropdown Missing / Not Assigned In The Inspector Panel.");
        }

        void OnSceneAssetNameFieldValueChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (DatabaseManager.Instance != null)
                {
                    AppData.SceneAsset sceneAsset = DatabaseManager.Instance.GetCurrentSceneAsset();
                    sceneAsset.name = value;

                    AppData.AssetInfoField assetInfoField = sceneAsset.info.GetInfoField(AppData.InfoDisplayerFieldType.Title);
                    assetInfoField.name = value;

                    sceneAsset.info.UpdateInfoField(assetInfoField);

                    DatabaseManager.Instance.SetCurrentSceneAsset(sceneAsset);
                }
                else
                    Debug.LogWarning("--> Asset Manager Not Initialized.");
            }
        }

        void OnSceneAssetDescriptionFieldValueChanged(string value)
        {
            if (DatabaseManager.Instance != null)
            {
                AppData.SceneAsset sceneAsset = DatabaseManager.Instance.GetCurrentSceneAsset();
                sceneAsset.description = value;

                DatabaseManager.Instance.SetCurrentSceneAsset(sceneAsset);
            }
            else
                Debug.LogWarning("--> Asset Manager Not Initialized.");
        }

        void OnAssetCategorySelectionDropdownValueChanged(int dropdownIndex)
        {
            if (DatabaseManager.Instance != null)
            {
                AppData.SceneAsset sceneAsset = DatabaseManager.Instance.GetCurrentSceneAsset();

                AppData.AssetCategoryType selectedCategory = (AppData.AssetCategoryType)dropdownIndex;

                sceneAsset.categoryType = selectedCategory;

                DatabaseManager.Instance.SetCurrentSceneAsset(sceneAsset);
            }
            else
                Debug.LogWarning("--> Asset Manager Not Initialized.");
        }

        protected override void OnShowScreenWidget(AppData.SceneDataPackets dataPackets)
        {
            ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
        }

        protected override void OnHideScreenWidget()
        {
            HideSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
        }

        protected override void OnInputFieldValueChanged(string value, AppData.InputFieldDataPackets dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnInputFieldValueChanged(int value, AppData.InputFieldDataPackets dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnSubscribeToActionEvents(bool subscribe)
        {
            throw new NotImplementedException();
        }

        protected override void OnScrollerValueChanged(Vector2 value)
        {
            throw new NotImplementedException();
        }

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxDataPackets dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnActionButtonEvent(AppData.WidgetType popUpType, AppData.InputActionButtonType actionType, AppData.SceneDataPackets dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownDataPackets dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void ScrollerPosition(Vector2 position)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
