using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SkyboxCreationWidgetUIHandler : AppData.SettingsWidget
    {
        #region Components

        [SerializeField]
        LoadingSpinnerWidget loadingSpinner = null;

        [Space(5)]
        [SerializeField]
        MeshRenderer previewObject = null;

        [Space(5)]
        [SerializeField]
        Material defaultMaterial = null;

        Material previewMaterial;
        Material skyboxMaterial;

        bool initialized;
        bool skyboxCreated = false;

        #endregion

        #region Main

        #endregion

        #region Overrides

        protected override void Init()
        {
            if (GetActive())
            {
                if (loadingSpinner == null)
                    LogWarning("Loading Spinner Is Null.", this, () => Init());

                if (previewObject != null)
                {
                    previewMaterial = previewObject.sharedMaterial;
                    initialized = true;
                }
                else
                    LogWarning("Preview Object Is Missing / Null.", this, () => Init());

                OnActionDropdownInitialized((callbackResults) =>
                {
                    if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                        foreach (var dropdown in actionDropdownList)
                        {
                            switch (dropdown.dataPackets.actionType)
                            {
                                case AppData.InputDropDownActionType.SettingsSelectionType:

                                    List<string> skyboxSettingsTypeList = SceneAssetsManager.Instance.GetFormatedDropDownContentList(SceneAssetsManager.Instance.GetDropDownContentData(AppData.DropDownContentType.SkyboxSettings).data);

                                    if (skyboxSettingsTypeList != null)
                                        OnInitializeDropDown(dropdown, skyboxSettingsTypeList, false);
                                    else
                                        LogError("Color Mode List Is null.", this, () => Init());

                                    break;

                                case AppData.InputDropDownActionType.RotationalDirection:

                                    List<string> rotationalDirectionList = SceneAssetsManager.Instance.GetFormatedDropDownContentList(SceneAssetsManager.Instance.GetDropDownContentData(AppData.DropDownContentType.Directions).data);

                                    if (rotationalDirectionList != null)
                                        OnInitializeDropDown(dropdown, rotationalDirectionList, false);
                                    else
                                        LogError("Color Picker List Is null.", this, () => Init());

                                    break;
                            }
                        }
                    else
                        LogWarning(callbackResults.results, this, () => Init());

                });
            }
        }

        void OnInitializeDropDown(AppData.UIDropDown<AppData.DropdownDataPackets> dropdown, List<string> contentList, bool isUpdate)
        {
            if (contentList != null)
            {
                dropdown.value.ClearOptions();
                dropdown.value.onValueChanged.RemoveAllListeners();

                List<TMP_Dropdown.OptionData> dropdownOption = new List<TMP_Dropdown.OptionData>();

                foreach (var content in contentList)
                    dropdownOption.Add(new TMP_Dropdown.OptionData() { text = content });

                dropdown.value.AddOptions(dropdownOption);

                dropdown.value.onValueChanged.RemoveAllListeners();
                dropdown.value.onValueChanged.AddListener((value) => OnActionDropdownValueChangedEvent(value, contentList, dropdown.dataPackets));

                if (isUpdate)
                {
                    dropdown.value.value = 0;
                    OnActionDropdownValueChangedEvent(dropdown.value.value, contentList, dropdown.dataPackets);
                }
            }
            else
                LogWarning("Dropdown Content Null.", this, () => OnInitializeDropDown(dropdown, contentList, isUpdate));
        }

        void OnWidgetInfoData()
        {
            if (GetActive())
            {
                if (RenderingSettingsManager.Instance.UpdateScreenWidgetInfo)
                {
                    Debug.LogError("==> Update Widget Info");

                    #region Setup Skybox Preview

                    SetupSkyboxHDRI(RenderingSettingsManager.Instance.GetRenderingSettingsData().GetCurrentSkyboxTexture());

                    #endregion
                }
                else
                {
                    // Create New

                    #region Setup Skybox Preview

                    ResetSkyboxHDRI();

                    #endregion
                }
            }
        }

        protected override void OnActionButtonClickedEvent(AppData.ButtonDataPackets dataPackets)
        {
            switch (dataPackets.actionType)
            {
                case AppData.InputActionButtonType.CreateSkyboxButton:

                    skyboxCreated = false;

                    loadingSpinner.SetScreenTextContent("Please Wait - Configuring Lighting Data...", AppData.ScreenUITextType.MessageDisplayer);

                    AppData.ScreenLoadingInitializationData loadingData = new AppData.ScreenLoadingInitializationData();
                    loadingData.duration = SceneAssetsManager.Instance.GetDefaultExecutionValue(AppData.RuntimeValueType.DefaultAssetCreationYieldValue).value;
                    loadingData.autoHide = true;

                    loadingSpinner.AddLoadingData(loadingData);

                    loadingSpinner.Show((spinnerCallbakResults) =>
                    {
                        if (!AppData.Helpers.IsSuccessCode(spinnerCallbakResults.resultsCode))
                            LogWarning(spinnerCallbakResults.results, this, () => OnActionButtonClickedEvent(dataPackets));
                    });

                    break;

                case AppData.InputActionButtonType.OpenFilePicker_HDRI:

                    AppData.ActionEvents.OnActionButtonClicked(AppData.InputActionButtonType.OpenFilePicker_HDRI);

                    break;
            }
        }

        protected override void OnActionCheckboxValueChangedEvent(bool value, AppData.CheckboxDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionDropdownValueChangedEvent(string value, AppData.DropdownDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionDropdownValueChangedEvent(int value, AppData.DropdownDataPackets dataPackets)
        {

        }

        protected override void OnActionDropdownValueChangedEvent(int value, List<string> contentList, AppData.DropdownDataPackets dataPackets)
        {
            switch (dataPackets.actionType)
            {
                case AppData.InputDropDownActionType.SettingsSelectionType:

                    ShowWidgetOnDropDownSelection(AppData.Helpers.GetStringToEnum<AppData.SettingsWidgetTabID>(contentList[value]), dataPackets.actionType);

                    break;

                case AppData.InputDropDownActionType.RotationalDirection:

                    break;

            }
        }


        protected override void OnActionInputFieldValueChangedEvent(string value, AppData.InputFieldDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionSliderValueChangedEvent(float value, AppData.SliderDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInputSliderValueChangedEvent(float value, AppData.InputSliderDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInputSliderValueChangedEvent(string value, AppData.InputSliderDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnResetWidgetData(AppData.SettingsWidgetType widgetType)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnWidgetClosed()
        {
        }

        protected override void OnWidgetOpened() => OnWidgetInfoData();

        protected override void RegisterEventListensers(bool register)
        {
            if (GetActive())
            {
                if (register)
                {
                    AppData.ActionEvents._OnFilePickerDirectoryFieldSelectedEvent += ActionEvents__OnFilePickerDirectoryFieldSelectedEvent;
                    loadingSpinner._OnRefreshInProgress_AddEventListener += OnRefreshInProgress;
                    loadingSpinner._OnRefreshCompleted_AddEventListener += OnRefreshCompleted;
                    loadingSpinner._OnRefreshFailed_AddEventListener += OnRefreshFailed;
                }
                else
                {
                    loadingSpinner._OnRefreshInProgress_AddEventListener -= OnRefreshInProgress;
                    loadingSpinner._OnRefreshCompleted_AddEventListener -= OnRefreshCompleted;
                    loadingSpinner._OnRefreshFailed_AddEventListener -= OnRefreshFailed;
                }
            }
            else
            {
                loadingSpinner._OnRefreshInProgress_AddEventListener -= OnRefreshInProgress;
                loadingSpinner._OnRefreshCompleted_AddEventListener -= OnRefreshCompleted;
                loadingSpinner._OnRefreshFailed_AddEventListener -= OnRefreshFailed;
            }
        }

        void OnRefreshInProgress()
        {
            AppData.SkyboxSettings skyboxSettings = new AppData.SkyboxSettings
            {
                name = "Custom"

            };

            RenderingSettingsManager.Instance.CreateSkyboxSettings(skyboxSettings, (created) =>
            {
                if (AppData.Helpers.IsSuccessCode(created.resultsCode))
                {
                    skyboxCreated = true;
                    Debug.LogError("==> Skybox Created.");
                }
                else
                    LogWarning(created.results, this, () => OnRefreshInProgress());
            });
        }

        void OnRefreshCompleted()
        {
            if (skyboxCreated)
            {
                // Show Notification
                AppData.Notification notification = new AppData.Notification
                {
                    message = "Skybox Created Successfully.",
                    notificationType = AppData.NotificationType.Info,
                    screenType = AppData.UIScreenType.ProjectViewScreen,
                    screenPosition = AppData.SceneAssetPivot.TopCenter,
                    delay = SceneAssetsManager.Instance.GetDefaultExecutionValue(AppData.RuntimeValueType.NotificationDelay).value,
                    duration = SceneAssetsManager.Instance.GetDefaultExecutionValue(AppData.RuntimeValueType.NotificationDuration).value // Get From Value List In Scene Assets Manager.
                };

                NotificationSystemManager.Instance.ScheduleNotification(notification);

                HideWidget();
            }
        }

        void OnRefreshFailed()
        {
            Debug.LogError("==> Refresh Failed.");

            //parentWidget.HideChildWidget(AppData.SettingsWidgetType.LoadingSpinnerWidget);
        }

        private void ActionEvents__OnFilePickerDirectoryFieldSelectedEvent(AppData.AssetFieldType fieldType, AppData.StorageDirectoryData directoryData)
        {
            if (fieldType == AppData.AssetFieldType.HDRI)
            {
                Texture2D skyboxTexture = AppData.Helpers.LoadTextureFile(directoryData.directory);
                SetupSkyboxHDRI(skyboxTexture);
            }
        }

        void SetupSkyboxHDRI(Texture2D hdri)
        {
            if (initialized)
            {
                if (hdri != null)
                {
                    Resources.UnloadUnusedAssets();

                    RenderingSettingsManager.Instance.GetMaterialShader(AppData.ShaderType.Skybox, (shaderResults) =>
                    {
                        if (AppData.Helpers.IsSuccessCode(shaderResults.resultsCode))
                        {
                            skyboxMaterial = new Material(shaderResults.data.value);
                            skyboxMaterial.SetTexture("_MainTex", hdri);

                            previewMaterial.SetTexture("_MainTex", hdri);
                            previewObject.sharedMaterial = previewMaterial;

                            RenderingSettingsManager.Instance.GetRenderingSettingsData().SetCurrentSkybox(skyboxMaterial);
                        }
                        else
                            LogWarning(shaderResults.results, this, () => SetupSkyboxHDRI(hdri));
                    });
                }
                else
                    LogWarning("Texture Failed To Load.", this, () => SetupSkyboxHDRI(hdri));
            }
            else
                LogWarning("PreviewObject Missing / Null", this, () => SetupSkyboxHDRI(hdri));
        }

        void ResetSkyboxHDRI()
        {
            if (initialized)
            {
                if (defaultMaterial != null)
                {
                    LogInfo("Reset Data", this, () => ResetSkyboxHDRI());

                    previewObject.sharedMaterial = defaultMaterial;
                    previewObject.UpdateGIMaterials();

                    RenderingSettingsManager.Instance.GetRenderingSettingsData().SetCurrentSkybox(RenderingSettingsManager.Instance.GetRenderingSettingsData().GetDefaultMaterial());
                }
                else
                    LogWarning("Default Material Is Not Assigned In The Unity Editor Inspector.", this, () => ResetSkyboxHDRI());
            }
            else
                LogWarning("PreviewObject Missing / Null", this, () => ResetSkyboxHDRI());
        }

        #endregion
    }
}