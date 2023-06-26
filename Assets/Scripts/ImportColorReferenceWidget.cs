using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class ImportColorReferenceWidget : AppData.SettingsWidget
    {
        #region Components

        #endregion

        [SerializeField]
        LoadingSpinnerWidget loadingSpinner = null;

        [Space(5)]
        [SerializeField]
        AppData.UIScreenDimensions referenceImageTargetResolution = new AppData.UIScreenDimensions();

        [Space(5)]
        [SerializeField]
        ColorPalletWidgetUIHandler colorPalletWidget = null;

        [Space(5)]
        [SerializeField]
        int displayedPathLength = 50;

        AppData.StorageDirectoryData directoryData = new AppData.StorageDirectoryData();

        [SerializeField]
        List<AppData.ColorInfo> generatedColorInfoList = new List<AppData.ColorInfo>();

        Color[] extractedColors;
        List<Color> loadedColorList = new List<Color>();

        Thread colorGenerationThread;

        Coroutine colorGenerationRoutine;

        bool generatingColors = false;

        AppData.ScreenLoadingInitializationData loadingData = new AppData.ScreenLoadingInitializationData();

        #region Main

        protected override void Init()
        {
            if (colorPalletWidget == null)
            {
                if (GetComponentInParent<ColorPalletWidgetUIHandler>())
                    colorPalletWidget = GetComponentInParent<ColorPalletWidgetUIHandler>();
            }

            SetActionButtonState(AppData.InputActionButtonType.GenerateColorSwatchButton, AppData.InputUIState.Disabled);
        }

        protected override void OnActionButtonClickedEvent(AppData.ButtonDataPackets dataPackets)
        {
            switch (dataPackets.action)
            {
                case AppData.InputActionButtonType.HideScreenWidget:

                    CloseWidget(this);

                    break;

                case AppData.InputActionButtonType.GenerateColorSwatchButton:

                    GenerateColorData();

                    break;

                case AppData.InputActionButtonType.OpenFilePicker_Image:

                    AppData.ActionEvents.OnActionButtonClicked(AppData.InputActionButtonType.OpenFilePicker_Image);

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
            throw new System.NotImplementedException();
        }

        protected override void OnActionInputFieldValueChangedEvent(string value, AppData.InputFieldDataPackets dataPackets)
        {
            if (dataPackets.action == AppData.InputFieldActionType.ColorReferenceImageURLField)
            {
                if (string.IsNullOrEmpty(value))
                    SetActionButtonState(AppData.InputActionButtonType.GenerateColorSwatchButton, AppData.InputUIState.Disabled);
                else
                    SetActionButtonState(AppData.InputActionButtonType.GenerateColorSwatchButton, AppData.InputUIState.Enabled);
            }
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

        protected override void OnWidgetClosed()
        {
            SetInputFieldValue(AppData.InputFieldActionType.ColorReferenceImageURLField, string.Empty, (setValueCallbackResults) =>
            {
                if (!AppData.Helpers.IsSuccessCode(setValueCallbackResults.resultsCode))
                    Debug.LogWarning($"--> ActionEvents__OnFilePickerDirectoryFieldSelectedEvent's SetInputFieldValue Failed With results : {setValueCallbackResults.results}");
            });
        }

        protected override void RegisterEventListensers(bool register)
        {
            if (GetActive())
            {
                if (register)
                {
                    AppData.ActionEvents._OnFilePickerDirectoryFieldSelectedEvent += ActionEvents__OnFilePickerDirectoryFieldSelectedEvent;
                    loadingSpinner._OnRefreshCompleted_AddEventListener += OnRefreshCompleted;
                    loadingSpinner._OnAbortRefresh_AddEventListener += OnRefreshAborted;
                    loadingSpinner._OnRefreshFailed_AddEventListener += OnRefreshFailed;
                }
                else
                {
                    AppData.ActionEvents._OnFilePickerDirectoryFieldSelectedEvent -= ActionEvents__OnFilePickerDirectoryFieldSelectedEvent;
                    loadingSpinner._OnRefreshCompleted_AddEventListener -= OnRefreshCompleted;
                    loadingSpinner._OnAbortRefresh_AddEventListener -= OnRefreshAborted;
                    loadingSpinner._OnRefreshFailed_AddEventListener -= OnRefreshFailed;
                }
            }
            else
            {
                AppData.ActionEvents._OnFilePickerDirectoryFieldSelectedEvent -= ActionEvents__OnFilePickerDirectoryFieldSelectedEvent;
                loadingSpinner._OnRefreshCompleted_AddEventListener -= OnRefreshCompleted;
                loadingSpinner._OnAbortRefresh_AddEventListener -= OnRefreshAborted;
                loadingSpinner._OnRefreshFailed_AddEventListener -= OnRefreshFailed;
            }
        }

        private void ActionEvents__OnFilePickerDirectoryFieldSelectedEvent(AppData.AssetFieldType fieldType, AppData.StorageDirectoryData directoryData)
        {
            if (fieldType == AppData.AssetFieldType.Image)
            {
                string filePathFormatted = string.Empty;

                if (directoryData.directory.Length > displayedPathLength)
                    filePathFormatted = directoryData.directory.Substring(0, displayedPathLength) + "...";
                else
                    filePathFormatted = directoryData.directory;

                SetInputFieldValue(AppData.InputFieldActionType.ColorReferenceImageURLField, filePathFormatted, (setValueCallbackResults) =>
                {
                    if (AppData.Helpers.IsSuccessCode(setValueCallbackResults.resultsCode))
                        this.directoryData = directoryData;
                    else
                        Debug.LogWarning($"--> ActionEvents__OnFilePickerDirectoryFieldSelectedEvent's SetInputFieldValue Failed With results : {setValueCallbackResults.results}");
                });
            }
        }

        void GenerateColorData()
        {
            if (!string.IsNullOrEmpty(directoryData.directory))
            {
                Texture2D texture = AppData.Helpers.LoadTextureFile(directoryData.directory);

                bool isLargeFileSize = (texture.width > referenceImageTargetResolution.width || texture.height > referenceImageTargetResolution.height) ? true : false;

                loadingSpinner.SetScreenTextContent("Please Wait - Extracting Image Color Data...", AppData.ScreenTextType.MessageDisplayer);

                loadingSpinner.SetActionButtonState(AppData.InputActionButtonType.Cancel, AppData.InputUIState.Enabled);

                AppData.ScreenLoadingInitializationData loadingData = new AppData.ScreenLoadingInitializationData();
                loadingData.duration = SceneAssetsManager.Instance.GetDefaultExecutionValue(AppData.RuntimeValueType.DefaultScreenRefreshDuration).value;
                loadingData.isLargeFileSize = isLargeFileSize;

                loadingSpinner.AddLoadingData(loadingData);

                loadingSpinner.Show((spinnerCallbakResults) =>
                {
                    if (!AppData.Helpers.IsSuccessCode(spinnerCallbakResults.resultsCode))
                        Debug.LogWarning($"--> GenerateColorData Failed With Results : {spinnerCallbakResults.results}");
                });

                if (texture != null)
                {
                    extractedColors = texture.GetPixels();


                    if (colorGenerationRoutine != null)
                    {
                        StopAllCoroutines();
                        colorGenerationRoutine = null;
                    }

                    Destroy(texture);

                    Resources.UnloadUnusedAssets();

                    colorGenerationRoutine = StartCoroutine(OnGereratedColorDataSync());
                }
                else
                    Debug.LogWarning("GenerateColorData Failed - Texture Couldn't Load.");
            }
            else
                Debug.LogWarning($"--> GenerateColorData Failed : Directory Data Path Is Null / Empty.");
        }

        IEnumerator OnGereratedColorDataSync()
        {
            generatingColors = true;
            generatedColorInfoList = new List<AppData.ColorInfo>();
            colorGenerationThread = new Thread(GenerateColors);
            colorGenerationThread.Start();

            yield return new WaitUntil(() => generatingColors == false && generatedColorInfoList.Count > 0);

            if (generatedColorInfoList.Count > 0)
                OnCreateGeneratedColorInfo();
        }

        void OnCreateGeneratedColorInfo()
        {
            for (int i = 0; i < generatedColorInfoList.Count; i++)
                SceneAssetsManager.Instance.GetHexidecimalFromColor(generatedColorInfoList[i].color, (getHexCallbackResults) =>
                {
                    if (AppData.Helpers.IsSuccessCode(getHexCallbackResults.resultsCode))
                        generatedColorInfoList[i] = getHexCallbackResults.data;
                    else
                        Debug.LogWarning($"--> OnCreateGeneratedColorInfo - SceneAssetsManager's GetHexidecimalFromColor Failed With Results : {getHexCallbackResults.results}");
                });

            loadingSpinner.SetScreenTextContent("Please Wait - Populating Color Swatch...", AppData.ScreenTextType.MessageDisplayer);
            loadingSpinner.SetActionButtonState(AppData.InputActionButtonType.Cancel, AppData.InputUIState.Disabled);
            loadingData.duration = SceneAssetsManager.Instance.GetDefaultExecutionValue(AppData.RuntimeValueType.DefaultScreenRefreshDuration).value;
            loadingData.isLargeFileSize = false;

            loadingSpinner.AddLoadingData(loadingData);

            if (colorPalletWidget != null)
            {
                colorPalletWidget.OnGenerateNewColorSwatch(generatedColorInfoList, (generatedColorsCallbackResults) =>
                {
                    if (AppData.Helpers.IsSuccessCode(generatedColorsCallbackResults.resultsCode))
                        loadingSpinner.OnLoadingCompleted();
                    else
                        Debug.LogWarning($"--> OnCreateGeneratedColorInfo's OnGenerateNewColorSwatch Failed With Results : {generatedColorsCallbackResults.results}");
                });

                Debug.LogError("==> Populating Swatch.");
            }
            else
                Debug.LogWarning("--> OnCreateGeneratedColorInfo Failed : colorPalletWidget Is Not Assigned In The Editor Inspector Panel.");

            StopAllCoroutines();
        }

        void GenerateColors()
        {
            // Fix Leak

            if (!generatingColors)
            {
                List<Color> loadedColorList = new List<Color>();
                generatingColors = true;

                for (int i = 0; i < extractedColors.Length; i++)
                    if (!loadedColorList.Contains(extractedColors[i]))
                        loadedColorList.Add(extractedColors[i]);

                if (loadedColorList.Count > 0)
                    OnColorGenerated(loadedColorList);
                else
                    Debug.LogWarning("GenerateColorData Failed - Texture pixels Not Loaded.");
            }
        }

        void OnColorGenerated(List<Color> loadedColorList)
        {
            if (loadedColorList.Count > 0)
            {
                foreach (var color in loadedColorList)
                {
                    AppData.ColorInfo colorInfo = new AppData.ColorInfo
                    {
                        color = color,
                    };

                    if (!generatedColorInfoList.Contains(colorInfo))
                        generatedColorInfoList.Add(colorInfo);
                }
            }
            else
                Debug.LogWarning("--> OnColorGenerated Failed : Colors List Is Null / Empty.");

            if (colorGenerationThread.ThreadState == ThreadState.Running)
            {
                generatingColors = false;
                colorGenerationThread.Abort();
            }
        }

        void OnRefreshCompleted()
        {
            Debug.LogError("==> Refresh Completed.");

            //loadingSpinner.SetScreenTextContent("Please Wait - Populating Color Swatch...", AppData.ScreenUITextType.MessageDisplayer);

            //loadingSpinner.SetActionButtonState(AppData.InputActionButtonType.Cancel, AppData.InputUIState.Disabled);

            //loadingSpinner.Show(SceneAssetsManager.Instance.GetDefaultExecutionTimes(AppData.RuntimeValueType.DefaultScreenRefreshDuration).value, false, true, (spinnerCallbakResults) =>
            //{
            //    if (!spinnerCallbakResults.success)
            //        Debug.LogWarning($"--> GenerateColorData Failed With Results : {spinnerCallbakResults.results}");
            //});

            string message = $"{generatedColorInfoList.Count} colors has been generated successfully. Do you wish to generate more colors or proceed?";
            parentWidget.ShowChildWidget(AppData.SettingsWidgetType.ColorGeneratorInfoWidget, message, AppData.ScreenTextType.MessageDisplayer);
        }

        void OnRefreshFailed()
        {
            Debug.LogError("==> Refresh Failed.");
            string message = $"Warning : {Application.productName} has failed to extract color infomation from image. The image used has a resolution that exceeds the target limit of  W : {referenceImageTargetResolution.width} - H : {referenceImageTargetResolution.height}";
            parentWidget.ShowChildWidget(AppData.SettingsWidgetType.ScreenWarningInfoWidget, message, AppData.ScreenTextType.MessageDisplayer);
            //parentWidget.HideChildWidget(AppData.SettingsWidgetType.LoadingSpinnerWidget);
        }

        void OnRefreshAborted()
        {
            Debug.LogError("==> NOTIFICATION : Refresh Aborted.");
            //parentWidget.HideChildWidget(AppData.SettingsWidgetType.LoadingSpinnerWidget);
        }

        protected override void OnResetWidgetData(AppData.SettingsWidgetType widgetType)
        {
            if (widgetType == this.widgetType)
            {
                SetInputFieldValue(AppData.InputFieldActionType.ColorReferenceImageURLField, string.Empty, (setValueCallbackResults) =>
                {
                    if (AppData.Helpers.IsSuccessCode(setValueCallbackResults.resultsCode))
                        parentWidget.HideChildWidget(AppData.SettingsWidgetType.ScreenWarningInfoWidget);
                    else
                        Debug.LogWarning($"--> ActionEvents__OnFilePickerDirectoryFieldSelectedEvent's SetInputFieldValue Failed With results : {setValueCallbackResults.results}");
                });
            }
        }

        protected override void OnWidgetOpened()
        {

        }

        protected override void OnActionDropdownValueChangedEvent(int value, List<string> contentList, AppData.DropdownDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
