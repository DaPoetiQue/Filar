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

            if (SceneAssetsManager.Instance != null)
            {
                List<string> categories = SceneAssetsManager.Instance.GetAssetCategoryList();

                if (categories != null)
                {
                    if (assetCategoryDropdown != null)
                    {
                        assetCategoryDropdown.ClearOptions();

                        List<TMP_Dropdown.OptionData> dropdownOption = new List<TMP_Dropdown.OptionData>();

                        foreach (var category in categories)
                        {
                            dropdownOption.Add(new TMP_Dropdown.OptionData() { text = category });
                        }

                        assetCategoryDropdown.AddOptions(dropdownOption);

                        assetCategoryDropdown.onValueChanged.AddListener((value) => OnAssetCategorySelectionDropdownValueChanged(value));
                    }
                    else
                        Debug.LogWarning("--> Scene Asset Input Name Field Missing / Not Assigned In The Inspector Panel.");
                }
                else
                    Debug.LogWarning("--> Asset Manager Asset Category List Not Initialized.");

            }
            else
                Debug.LogWarning("--> Asset Manager Not Initialized.");

            if (GetLayoutView().layout.GetComponent<RectTransform>())
                screenRect = GetLayoutView().layout.GetComponent<RectTransform>();
            else
                Debug.LogWarning("Init : Value Doesn't Have A Rect Transform Component.");

            base.Init();
        }

        protected override void OnScreenWidget()
        {
            if (SceneAssetsManager.Instance)
            {
                if (!string.IsNullOrEmpty(SceneAssetsManager.Instance.GetCurrentSceneAsset().name))
                {
                    if (titleDisplayer)
                        titleDisplayer.text = SceneAssetsManager.Instance.GetCurrentSceneAsset().name;
                    else
                        Debug.LogWarning("--> Pop Up Title Displayer Missing / Not Assigned In The Inspector Panel.");

                    if (SceneAssetsManager.Instance.GetCurrentSceneAsset().currentAssetMode == AppData.SceneAssetModeType.CreateMode)
                        ClearInputFields();

                    if (SceneAssetsManager.Instance.GetCurrentSceneAsset().currentAssetMode == AppData.SceneAssetModeType.EditMode)
                    {
                        if (nameInputField)
                            nameInputField.text = SceneAssetsManager.Instance.GetCurrentSceneAsset().name;

                        if (descriptionInputField)
                            descriptionInputField.text = SceneAssetsManager.Instance.GetCurrentSceneAsset().description;

                        if (submitButtonText != null)
                            submitButtonText.text = updateAssetSubmitButtonTitle;

                        if (assetCategoryDropdown != null)
                            assetCategoryDropdown.value = (int)SceneAssetsManager.Instance.GetCurrentSceneAsset().categoryType;
                    }

                    if (!string.IsNullOrEmpty(SceneAssetsManager.Instance.GetCurrentSceneAsset().GetAssetField(AppData.AssetFieldType.Thumbnail).path))
                    {
                        if (thumbnailDisplayer)
                        {
                            thumbnailDisplayer.sprite = AppData.Helpers.Texture2DToSprite(AppData.Helpers.LoadTextureFile(SceneAssetsManager.Instance.GetCurrentSceneAsset().GetAssetField(AppData.AssetFieldType.Thumbnail).path));
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
                if (SceneAssetsManager.Instance != null)
                {
                    AppData.SceneAsset sceneAsset = SceneAssetsManager.Instance.GetCurrentSceneAsset();
                    sceneAsset.name = value;

                    AppData.AssetInfoField assetInfoField = sceneAsset.info.GetInfoField(AppData.InfoDisplayerFieldType.Title);
                    assetInfoField.name = value;

                    sceneAsset.info.UpdateInfoField(assetInfoField);

                    SceneAssetsManager.Instance.SetCurrentSceneAsset(sceneAsset);
                }
                else
                    Debug.LogWarning("--> Asset Manager Not Initialized.");
            }
        }

        void OnSceneAssetDescriptionFieldValueChanged(string value)
        {
            if (SceneAssetsManager.Instance != null)
            {
                AppData.SceneAsset sceneAsset = SceneAssetsManager.Instance.GetCurrentSceneAsset();
                sceneAsset.description = value;

                SceneAssetsManager.Instance.SetCurrentSceneAsset(sceneAsset);
            }
            else
                Debug.LogWarning("--> Asset Manager Not Initialized.");
        }

        void OnAssetCategorySelectionDropdownValueChanged(int dropdownIndex)
        {
            if (SceneAssetsManager.Instance != null)
            {
                AppData.SceneAsset sceneAsset = SceneAssetsManager.Instance.GetCurrentSceneAsset();

                AppData.SceneAssetCategoryType selectedCategory = (AppData.SceneAssetCategoryType)dropdownIndex;

                sceneAsset.categoryType = selectedCategory;

                SceneAssetsManager.Instance.SetCurrentSceneAsset(sceneAsset);
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

        #endregion
    }
}
