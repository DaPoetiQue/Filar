using UnityEngine;
using UnityEngine.UI;
using TMPro;
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

        #region Main

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType>>> callback)
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType>>();

            Init(initializationCallbackResults =>
            {
                callbackResults.SetResultsData(initializationCallbackResults);
            });

            callback.Invoke(callbackResults);
        }

        protected override AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType>> OnGetState()
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType>>(AppData.Helpers.GetAppComponentValid(GetStatePacket(), $"{GetName()} - State Object", "Widget State Object Is Null / Not Yet Initialized In The Base Class."));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(GetType());

                if (callbackResults.Success())
                {
                    var widgetType = GetType().data;

                    callbackResults.SetResult(GetStatePacket().Initialized(widgetType));

                    if (callbackResults.Success())
                    {
                        callbackResults.result = $"Widget : {GetStatePacket().GetName()} Of Type : {GetStatePacket().GetType()} State Is Set To : {GetStatePacket().GetStateType()}";
                        callbackResults.data = GetStatePacket();
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        void ReuseReferenceDataAndDelete()
        {
            //if (callbackResults.Success())
            //{
            //    OnRegisterWidget(this, onRegisterWidgetCallbackResults =>
            //    {
            //        callbackResults.SetResult(GetType());

            //        if (callbackResults.Success())
            //        {
            //            if (nameInputField != null)
            //                nameInputField.onValueChanged.AddListener((value) => OnSceneAssetNameFieldValueChanged(value));
            //            else
            //                Debug.LogWarning("--> Scene Asset Input Name Field Missing / Not Assigned In The Inspector Panel.");

            //            if (descriptionInputField != null)
            //                descriptionInputField.onValueChanged.AddListener((value) => OnSceneAssetDescriptionFieldValueChanged(value));
            //            else
            //                Debug.LogWarning("--> Scene Asset Input Name Field Missing / Not Assigned In The Inspector Panel.");

            //            if (AppDatabaseManager.Instance != null)
            //            {
            //                var content = AppDatabaseManager.Instance.GetDropdownContent<AppData.AssetCategoryType>();

            //                if (content.data != null)
            //                {
            //                    if (assetCategoryDropdown != null)
            //                    {
            //                        List<TMP_Dropdown.OptionData> dropdownOption = new List<TMP_Dropdown.OptionData>();

            //                        foreach (var category in content.data)
            //                            dropdownOption.Add(new TMP_Dropdown.OptionData() { text = category });

            //                        assetCategoryDropdown.AddOptions(dropdownOption);

            //                        assetCategoryDropdown.onValueChanged.AddListener((value) => OnAssetCategorySelectionDropdownValueChanged(value));
            //                    }
            //                    else
            //                        LogError("Category Drop Down Missing / Not Assigned In The Editor Inspector Panel.", this);
            //                }
            //                else
            //                    LogError("Scene Asset Category Content Missing / Not Found.", this);
            //            }
            //            else
            //                LogError("Scene Asset Manager Instance Not Initialized.", this);

            //            if (GetLayoutView().layout.GetComponent<RectTransform>())
            //                screenRect = GetLayoutView().layout.GetComponent<RectTransform>();
            //            else
            //                Debug.LogWarning("Init : Value Doesn't Have A Rect Transform Component.");

            //            var widgetStatePacket = new AppData.WidgetStatePacket(name: GetName(), type: GetType().data, stateType: AppData.WidgetStateType.Initialized, value: this);

            //            callbackResults.result = $"Widget : {GetName()} Of Type : {GetType().data}'s State Packet Has Been Initialized Successfully.";
            //            callbackResults.data = widgetStatePacket;
            //        }
            //        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            //    });
            //}
            //else
            //    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        protected override void OnScreenWidget()
        {
            if (AppDatabaseManager.Instance)
            {
                if (!string.IsNullOrEmpty(AppDatabaseManager.Instance.GetCurrentSceneAsset().name))
                {
                    if (titleDisplayer)
                        titleDisplayer.text = AppDatabaseManager.Instance.GetCurrentSceneAsset().name;
                    else
                        Debug.LogWarning("--> Pop Up Title Displayer Missing / Not Assigned In The Inspector Panel.");

                    if (AppDatabaseManager.Instance.GetCurrentSceneAsset().assetMode == AppData.AssetModeType.CreateMode)
                        ClearInputFields();

                    if (AppDatabaseManager.Instance.GetCurrentSceneAsset().assetMode == AppData.AssetModeType.EditMode)
                    {
                        if (nameInputField)
                            nameInputField.text = AppDatabaseManager.Instance.GetCurrentSceneAsset().name;

                        if (descriptionInputField)
                            descriptionInputField.text = AppDatabaseManager.Instance.GetCurrentSceneAsset().description;

                        if (submitButtonText != null)
                            submitButtonText.text = updateAssetSubmitButtonTitle;

                        if (assetCategoryDropdown != null)
                            assetCategoryDropdown.value = (int)AppDatabaseManager.Instance.GetCurrentSceneAsset().categoryType;
                    }

                    if (!string.IsNullOrEmpty(AppDatabaseManager.Instance.GetCurrentSceneAsset().GetAssetField(AppData.AssetFieldType.Thumbnail).path))
                    {
                        if (thumbnailDisplayer)
                        {
                            thumbnailDisplayer.sprite = AppData.Helpers.Texture2DToSprite(AppData.Helpers.LoadTextureFile(AppDatabaseManager.Instance.GetCurrentSceneAsset().GetAssetField(AppData.AssetFieldType.Thumbnail).path));
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
                if (AppDatabaseManager.Instance != null)
                {
                    AppData.SceneAsset sceneAsset = AppDatabaseManager.Instance.GetCurrentSceneAsset();
                    sceneAsset.name = value;

                    AppData.AssetInfoField assetInfoField = sceneAsset.info.GetInfoField(AppData.InfoDisplayerFieldType.Title);
                    assetInfoField.name = value;

                    sceneAsset.info.UpdateInfoField(assetInfoField);

                    AppDatabaseManager.Instance.SetCurrentSceneAsset(sceneAsset);
                }
                else
                    Debug.LogWarning("--> Asset Manager Not Initialized.");
            }
        }

        void OnSceneAssetDescriptionFieldValueChanged(string value)
        {
            if (AppDatabaseManager.Instance != null)
            {
                AppData.SceneAsset sceneAsset = AppDatabaseManager.Instance.GetCurrentSceneAsset();
                sceneAsset.description = value;

                AppDatabaseManager.Instance.SetCurrentSceneAsset(sceneAsset);
            }
            else
                Debug.LogWarning("--> Asset Manager Not Initialized.");
        }

        void OnAssetCategorySelectionDropdownValueChanged(int dropdownIndex)
        {
            if (AppDatabaseManager.Instance != null)
            {
                AppData.SceneAsset sceneAsset = AppDatabaseManager.Instance.GetCurrentSceneAsset();

                AppData.AssetCategoryType selectedCategory = (AppData.AssetCategoryType)dropdownIndex;

                sceneAsset.categoryType = selectedCategory;

                AppDatabaseManager.Instance.SetCurrentSceneAsset(sceneAsset);
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
