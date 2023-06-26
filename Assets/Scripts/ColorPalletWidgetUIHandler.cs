using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Com.RedicalGames.Filar
{
    public class ColorPalletWidgetUIHandler : AppData.SettingsWidget
    {
        #region Components

        [Space(15)]
        [SerializeField]
        List<Image> selectedColorDisplayerList = new List<Image>();

        [Space(5)]
        [SerializeField]
        int colorValueInputCharacterLimit = 6;

        [Space(5)]
        [SerializeField]
        string newSwatchFileName = "Custom";

        [Space(5)]
        [SerializeField]
        GradientColorPickerHandler gradientColorPicker = null;

        [Space(5)]
        [SerializeField]
        ColorDropPickerHandler dropPickerHandler = null;

        [Space(5)]
        [SerializeField]
        List<AppData.RGBColorValue> colorValueList = new List<AppData.RGBColorValue>();

        [Space(5)]
        [SerializeField]
        bool resetAlphaValueOnModeChanged = true;

        [SerializeField]
        AppData.ColorInfo currentColorInfo = new AppData.ColorInfo();
        AppData.ColorInfo tempColorInfo = new AppData.ColorInfo();

        [SerializeField]
        AppData.ColorSpaceType currentColorMode = AppData.ColorSpaceType.RGBA;

        bool settingsOpen = false;
        bool isRefreshed = false;
        bool isColorDropPickerMode;

        Color hsvColor;
        AppData.HSVColorData colorHSVData = new AppData.HSVColorData();

        List<string> tempCustomSwatchContent = new List<string>();

        int rgbColorValueMultiplier = 255;
        int hsvHueColorValueMultiplier = 360;
        int hsvColorSaturationValueMultiplier = 100;

        #endregion

        #region Main

        protected override void RegisterEventListensers(bool register)
        {
            if (register)
                if (GetActive())
                    AppData.ActionEvents._OnSwatchColorPickedEvent += ActionEvents__OnSwatchColorPickedEvent;
                else
                    AppData.ActionEvents._OnSwatchColorPickedEvent -= ActionEvents__OnSwatchColorPickedEvent;
            else
                AppData.ActionEvents._OnSwatchColorPickedEvent -= ActionEvents__OnSwatchColorPickedEvent;
        }

        protected override void Init()
        {
            if (GetActive())
            {
                InputValueInitialized((callback) =>
                {
                    if (AppData.Helpers.IsSuccessCode(callback.resultsCode))
                    {
                        if (string.IsNullOrEmpty(storageDirectoryData.name))
                            storageDirectoryData.name = "_ColorSettings";

                        OnActionButtonInitialized((callbackResults) =>
                        {
                            if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                            {
                                SetActionButtonState(AppData.InputActionButtonType.CreateNewColorButton, AppData.InputUIState.Hidden);
                                SetActionButtonState(AppData.InputActionButtonType.RevertSettingsButton, AppData.InputUIState.Disabled);
                                SetActionButtonState(AppData.InputActionButtonType.ClearAllButton, AppData.InputUIState.Disabled);
                            }
                            else
                                Debug.LogError($"OnActionButtonInitialized Failed With Error Results : {callbackResults.results}");
                        });

                        OnActionDropdownInitialized((callbackResults) =>
                        {
                            if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                                foreach (var dropdown in actionDropdownList)
                                {
                                    switch (dropdown.dataPackets.action)
                                    {
                                        case AppData.InputDropDownActionType.ColorModeSelection:

                                            List<string> colorModeList = SceneAssetsManager.Instance.GetFormatedDropDownContentList(SceneAssetsManager.Instance.GetDropDownContentData(AppData.DropDownContentType.ColorSpaces).data);

                                            if (colorModeList != null)
                                            {
                                                OnInitializeDropDown(dropdown, colorModeList, false);
                                            }
                                            else
                                                Debug.LogError("--> OnActionDropdownInitialized Failed : Color Mode List Is null.");

                                            break;

                                        case AppData.InputDropDownActionType.ColorPickerSelection:

                                            List<string> colorPickerList = SceneAssetsManager.Instance.GetFormatedDropDownContentList(SceneAssetsManager.Instance.GetDropDownContentData(AppData.DropDownContentType.ColorPickers).data);

                                            if (colorPickerList != null)
                                                OnInitializeDropDown(dropdown, colorPickerList, false);
                                            else
                                                Debug.LogError("--> OnActionDropdownInitialized Failed : Color Picker List Is null.");

                                            break;
                                    }
                                }
                            else
                                Debug.LogWarning($"--> OnActionChecboxInitialized Failed With Results : {callbackResults.results}");

                        });

                        OnActionChecboxInitialized((checkboxInitializedCallbackResults) =>
                        {
                            if (AppData.Helpers.IsSuccessCode(checkboxInitializedCallbackResults.resultsCode))
                            {
                                if (dropPickerHandler)
                                {
                                    dropPickerHandler.Init();
                                    OnColorDropPicker(isColorDropPickerMode);
                                }
                                else
                                    Debug.LogWarning("--> OnActionChecboxInitialized Failed - DropPickerHandler Is Missing.");
                            }
                            else
                                Debug.LogWarning($"--> Init OnActionChecboxInitialized Failed With Results : {checkboxInitializedCallbackResults.results}");
                        });
                    }
                    else
                        Debug.LogError($"--> ActionEvents__OnSwatchColorPickedEvent Failed With Results : {callback.results}");
                });
            }
        }

        void OnResetDropDownSelection(AppData.InputDropDownActionType actionType)
        {
            foreach (var dropdown in actionDropdownList)
                if (dropdown.dataPackets.action == actionType)
                {
                    switch (actionType)
                    {
                        case AppData.InputDropDownActionType.ColorModeSelection:

                            break;

                        case AppData.InputDropDownActionType.ColorPickerSelection:

                            Debug.LogError($"--> OnActionDropdownInitialized Type : {dropdown.dataPackets.action}.");

                            List<string> colorPickerList = SceneAssetsManager.Instance.GetFormatedDropDownContentList(SceneAssetsManager.Instance.GetDropDownContentData(AppData.DropDownContentType.ColorPickers).data);

                            Debug.LogError($"--> OnActionDropdownInitialized Success : Color Picker List Found : {colorPickerList.Count}.");

                            if (colorPickerList != null)
                            {
                                OnInitializeDropDown(dropdown, colorPickerList, true);
                            }
                            else
                                Debug.LogError("--> OnActionDropdownInitialized Failed : Color Picker List Is null.");

                            break;
                    }

                    break;
                }
        }

        void OnSelectDropDownOption(AppData.InputDropDownActionType actionType, int optionID)
        {
            foreach (var dropdown in actionDropdownList)
                if (dropdown.dataPackets.action == actionType)
                {
                    dropdown.value.value = optionID;
                    break;
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

                    Debug.LogError($"-----------------------------> OnInitializeDropDown Success : Color Picker Updated...........................................");
                }
            }
            else
                Debug.LogWarning("OnActionDropdownInitialized Failed : Dropdown Content Null.");
        }

        void ActionEvents__OnSwatchColorPickedEvent(AppData.ColorInfo colorInfo, bool fromButtonClick, bool onOpenColourSettings)
        {
            InputValueInitialized((callback) =>
            {
                if (AppData.Helpers.IsSuccessCode(callback.resultsCode))
                {
                    if (SceneAssetsManager.Instance != null)
                    {
                        if (onOpenColourSettings)
                            SceneAssetsManager.Instance.OnInitializeColorSwatchData(storageDirectoryData.name);

                        SceneAssetsManager.Instance.GetColorSwatchData((callbackResults) =>
                        {
                            if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                            {
                                callbackResults.data.GetSwatchDropDownList((callbackDataResults) =>
                                {
                                    if (AppData.Helpers.IsSuccessCode(callbackDataResults.resultsCode))
                                    {
                                        if (!fromButtonClick)
                                            UpdateDropDownContent(AppData.InputDropDownActionType.SwatchPicker, callbackDataResults.data, true);

                                        if (fromButtonClick)
                                            tempColorInfo = colorInfo;

                                        UpdateActionInputs(colorInfo, fromButtonClick, onOpenColourSettings);
                                    }
                                    else
                                        Debug.LogError($"--> ActionEvents__OnSwatchColorPickedEvent Failed With Results : {callbackDataResults.results}");
                                });
                            }
                            else
                                Debug.LogError($"--> ActionEvents__OnSwatchColorPickedEvent failed With Results : {callbackResults.results}");
                        });
                    }
                    else
                        Debug.LogWarning("--> ActionEvents__OnSwatchColorPickedEvent Failed : SceneAssetsManager.Instance Is Null.");
                }
                else
                    Debug.LogError($"--> ActionEvents__OnSwatchColorPickedEvent Failed With Results : {callback.results}");
            });
        }

        void UpdateActionInputs(AppData.ColorInfo colorInfo, bool fromButtonClick = false, bool onOpenColourSettings = false, bool fromSliderInput = false, bool fromInputField = false)
        {
            if (settingsOpen)
            {
                #region Update Inputs

                #region RGB-A

                if (currentColorMode != AppData.ColorSpaceType.HSV)
                {
                    #region Sliders Update

                    SetAcionInputSliderValue(AppData.InputSliderActionType.RedColorChannelField, colorInfo.color.r);
                    SetAcionInputSliderValue(AppData.InputSliderActionType.GreenColorChannelField, colorInfo.color.g);
                    SetAcionInputSliderValue(AppData.InputSliderActionType.BlueColorChannelField, colorInfo.color.b);
                    SetAcionInputSliderValue(AppData.InputSliderActionType.AlphaColorChannelField, colorInfo.color.a);

                    #endregion

                    #region Inputs Update

                    SetAcionInputSliderValue(AppData.InputSliderActionType.RedColorChannelField, GetInputFieldValue(colorInfo.color.r, rgbColorValueMultiplier));
                    SetAcionInputSliderValue(AppData.InputSliderActionType.GreenColorChannelField, GetInputFieldValue(colorInfo.color.g, rgbColorValueMultiplier));
                    SetAcionInputSliderValue(AppData.InputSliderActionType.BlueColorChannelField, GetInputFieldValue(colorInfo.color.b, rgbColorValueMultiplier));
                    SetAcionInputSliderValue(AppData.InputSliderActionType.AlphaColorChannelField, GetInputFieldValue(colorInfo.color.a, rgbColorValueMultiplier));

                    #endregion
                }

                #endregion

                #region HSV

                if (currentColorMode == AppData.ColorSpaceType.HSV)
                {
                    #region Sliders Update

                    SetAcionInputSliderValue(AppData.InputSliderActionType.RedColorChannelField, colorHSVData.hue);
                    SetAcionInputSliderValue(AppData.InputSliderActionType.GreenColorChannelField, colorHSVData.saturation);
                    SetAcionInputSliderValue(AppData.InputSliderActionType.BlueColorChannelField, colorHSVData.value);

                    #endregion

                    #region Inputs Update

                    SetAcionInputSliderValue(AppData.InputSliderActionType.RedColorChannelField, GetInputFieldValue(colorHSVData.hue, hsvHueColorValueMultiplier));
                    SetAcionInputSliderValue(AppData.InputSliderActionType.GreenColorChannelField, GetInputFieldValue(colorHSVData.saturation, hsvColorSaturationValueMultiplier));
                    SetAcionInputSliderValue(AppData.InputSliderActionType.BlueColorChannelField, GetInputFieldValue(colorHSVData.value, hsvColorSaturationValueMultiplier));
                    SetAcionInputSliderValue(AppData.InputSliderActionType.AlphaColorChannelField, GetInputFieldValue(currentColorInfo.color.a, hsvColorSaturationValueMultiplier));

                    #endregion
                }

                #endregion

                #endregion

                #region Color Displayer Value

                foreach (var colorDisplayer in selectedColorDisplayerList)
                    colorDisplayer.color = colorInfo.color;

                #endregion

                #region Color Info

                currentColorInfo = colorInfo;
                Color.RGBToHSV(colorInfo.color, out colorHSVData.hue, out colorHSVData.saturation, out colorHSVData.value);

                #endregion

                #region Hexadecimal

                OnUpdateHexadecimalField(colorInfo);

                #endregion

                #region Settings

                if (RenderingSettingsManager.Instance != null)
                {
                    RenderingSettingsManager.Instance.GetRenderingSettingsData().GetLightingSettingsData().SetLightColorInfo(colorInfo);
                }

                #endregion

                #region On Create Color Button State

                if (isRefreshed)
                {
                    if (SceneAssetsManager.Instance != null)
                    {
                        SceneAssetsManager.Instance.GetColorSwatchData((callbackResults) =>
                        {
                            if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                            {
                                callbackResults.data.ColorInfoExistsInLibrary(colorInfo, (colorExistsResults) =>
                                {
                                    if (AppData.Helpers.IsSuccessCode(colorExistsResults.resultsCode))
                                        OnCreateColorButtonVisibility(false);
                                    else
                                        OnCreateColorButtonVisibility(true);
                                });
                            }
                            else
                                Debug.LogError($"--> SetInputSliderValueContent On open Settings GetColorSwatchData Failed With Results : {callbackResults.results}");
                        });
                    }
                    else
                        Debug.LogWarning("--> SetInputSliderValueContent On open Settings Failed : SceneAssetsManager.Instance Is Not Yet initialized.");

                }
                else
                {
                    if (fromButtonClick)
                        OnCreateColorButtonVisibility(false);
                    else
                        OnCreateColorButtonVisibility(true);
                }

                #endregion
            }

            #region On Create Color Button State

            if (onOpenColourSettings && !settingsOpen)
            {
                if (SceneAssetsManager.Instance != null)
                {
                    SceneAssetsManager.Instance.GetColorSwatchData((callbackResults) =>
                    {
                        if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                        {
                            tempColorInfo = colorInfo;

                            callbackResults.data.ColorInfoExistsInLibrary(colorInfo, (colorExistsResults) =>
                            {
                                if (!AppData.Helpers.IsSuccessCode(colorExistsResults.resultsCode))
                                    OnCreateColorButtonVisibility(true);
                            });
                        }
                        else
                            Debug.LogError($"--> SetInputSliderValueContent On open Settings GetColorSwatchData Failed With Results : {callbackResults.results}");
                    });
                }
                else
                    Debug.LogWarning("--> SetInputSliderValueContent On open Settings Failed : SceneAssetsManager.Instance Is Not Yet initialized.");

                isRefreshed = true;
                settingsOpen = true;

                Color.RGBToHSV(colorInfo.color, out colorHSVData.hue, out colorHSVData.saturation, out colorHSVData.value);

                #region Gradient Color picker

                if (gradientColorPicker)
                {
                    AppData.ColorInfo color = colorInfo;
                    color.color = Color.red;
                    gradientColorPicker.UpdateGradientInput(color);
                }

                #endregion

                ActionEvents__OnSwatchColorPickedEvent(colorInfo, false, false);
                OnUpdateHexadecimalField(colorInfo);
            }

            #endregion
        }

        string GetInputFieldValue(float inputValue, int multiplier)
        {
            return (GetColorValueInt(inputValue, multiplier) > 0) ? GetColorValueInt(inputValue, multiplier).ToString() : "";
        }

        void OnUpdateHexadecimalField(AppData.ColorInfo colorInfo)
        {
            string colorInfoValue = (colorInfo.hexadecimal.Length > colorValueInputCharacterLimit) ? AppData.Helpers.TrimStringValue(colorInfo.hexadecimal.ToUpper(), colorValueInputCharacterLimit) : colorInfo.hexadecimal;
            SetAcionInputFieldValue(AppData.InputFieldActionType.ColorHexidecimalField, colorInfoValue);
        }

        void InputValueInitialized(Action<AppData.Callback> callback)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (selectedColorDisplayerList.Count > 0)
            {
                foreach (var colorDisplayer in selectedColorDisplayerList)
                {
                    if (colorDisplayer != null)
                        callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                    else
                    {
                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                        break;
                    }
                }
            }

            if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                callbackResults.results = "ColorPalletWidgetUIHandler Init Success : inputSliderValueContent.value Found";
            else
                callbackResults.results = "ColorPalletWidgetUIHandler Init Failed : inputSliderValueContent.value / selectedColorDisplayer Is Null.";

            callback?.Invoke(callbackResults);
        }

        int GetColorValueInt(float value, int multiplier)
        {
            return Mathf.RoundToInt(value * multiplier);
        }

        void UpdateDropDownContent(AppData.InputDropDownActionType actionType, List<string> swatchList, bool isUpdate)
        {
            OnUpdateDropdownSelection(actionType, swatchList, isUpdate, (callbackResults) =>
            {
                if (!AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                    Debug.LogError($"OnActioDropdownInitialized Failed With Error Results : {callbackResults.results}");
            });
        }

        void OnCreateColorButtonVisibility(bool show)
        {
            InputValueInitialized((callbackResults) =>
            {
                OnActionButtonInitialized((initCallbackResults) =>
                {
                    if (AppData.Helpers.IsSuccessCode(initCallbackResults.resultsCode))
                    {
                        if (show)
                        {
                            SetActionButtonState(AppData.InputActionButtonType.CreateNewColorButton, AppData.InputUIState.Shown);
                            SetActionButtonState(AppData.InputActionButtonType.RevertSettingsButton, AppData.InputUIState.Enabled);
                        }
                        else
                        {
                            SetActionButtonState(AppData.InputActionButtonType.CreateNewColorButton, AppData.InputUIState.Hidden);
                            SetActionButtonState(AppData.InputActionButtonType.RevertSettingsButton, AppData.InputUIState.Disabled);
                        }
                    }
                    else
                        Debug.LogError($"--> OnCreateColorButtonVisibility Failed With Results : {initCallbackResults.results}");
                });
            });
        }

        protected override void OnWidgetClosed()
        {
            isRefreshed = false;
            settingsOpen = false;
        }

        #endregion

        #region Actions Events

        protected override void OnActionButtonClickedEvent(AppData.ButtonDataPackets dataPackets)
        {
            switch (dataPackets.action)
            {
                case AppData.InputActionButtonType.CreateNewColorButton:

                    OnCreateButton_Action((createActionCallbackResults) =>
                    {
                        if (AppData.Helpers.IsSuccessCode(createActionCallbackResults.resultsCode))
                        {
                            if (SceneAssetsManager.Instance != null)
                                SceneAssetsManager.Instance.GetColorSwatchData((swatchDataResults) =>
                                {
                                    if (AppData.Helpers.IsSuccessCode(swatchDataResults.resultsCode))
                                        swatchDataResults.data.OnSwatchColorSelection(currentColorInfo);
                                    else
                                        Debug.LogError($"--> ColorSwatchButtonHandler OnActionButtonInputs Failed With Results : {swatchDataResults.results}");
                                });
                            else
                                Debug.LogWarning("--> OnActionButtonInputs's GetColorSwatchData Failed : SceneAssetsManager.Instance Is Not Yet initialized.");

                            OnResetDropDownSelection(AppData.InputDropDownActionType.ColorPickerSelection);
                        }
                    });

                    break;

                case AppData.InputActionButtonType.RevertSettingsButton:

                    OnRevertSettings_Action();

                    break;

                case AppData.InputActionButtonType.ClearAllButton:

                    OnClearAllSwatchColors_Action();

                    break;

                case AppData.InputActionButtonType.ImportColorButton:

                    ShowChildWidget(AppData.SettingsWidgetType.ColorSettingsImportWidget);

                    break;

                case AppData.InputActionButtonType.OpenColorPromptWidget:

                    ShowChildWidget(AppData.SettingsWidgetType.ColorPickerPromptWidget);

                    break;
            }
        }

        protected override void OnActionInputFieldValueChangedEvent(string value, AppData.InputFieldDataPackets dataPackets)
        {
            if (dataPackets.action == AppData.InputFieldActionType.ColorHexidecimalField)
                if (SceneAssetsManager.Instance != null)
                {
                    if (value.Length < (colorValueInputCharacterLimit / 2) || value.Length > (colorValueInputCharacterLimit / 2) && value.Length < colorValueInputCharacterLimit)
                        return;

                    SceneAssetsManager.Instance.GetColorFromHexidecimal(value, (callback) =>
                    {
                        if (AppData.Helpers.IsSuccessCode(callback.resultsCode))
                            UpdateActionInputs(callback.data);
                        else
                            Debug.LogError($"--> OnHexidecimalInputValueChangedEvent Failed With Results : {callback.results}.");
                    });
                }
                else
                    Debug.LogWarning("--> OnHexidecimalInputValueChangedEvent Failed : SceneAssetsManager.Instance Is Not Yet Initialized.");
        }

        protected override void OnActionSliderValueChangedEvent(float value, AppData.SliderDataPackets dataPackets)
        {
            Debug.LogError($"---------------> Slider Value : {value}");
        }

        protected override void OnActionDropdownValueChangedEvent(string value, AppData.DropdownDataPackets dataPackets)
        {

            Debug.LogError($"--> Selection Value : {value} from type : {dataPackets.action}");

            switch (dataPackets.action)
            {
                case AppData.InputDropDownActionType.SwatchPicker:

                    if (!string.IsNullOrEmpty(value))
                    {
                        if (SceneAssetsManager.Instance != null)
                        {
                            SceneAssetsManager.Instance.SelectColorSwatchPallet(storageDirectoryData.name, value, (showPalledResults) =>
                            {
                                if (AppData.Helpers.IsSuccessCode(showPalledResults.resultsCode))
                                {
                                    if (showPalledResults.data == newSwatchFileName)
                                        SetActionButtonState(AppData.InputActionButtonType.ClearAllButton, AppData.InputUIState.Enabled);
                                    else
                                        SetActionButtonState(AppData.InputActionButtonType.ClearAllButton, AppData.InputUIState.Disabled);
                                }
                                else
                                    Debug.LogError($"--> On Color Swatch Picker Value Changed Event Failed With Results : {showPalledResults.results}");
                            });

                        }
                        else
                            Debug.LogWarning("--> OnColorSwatchPickerValueChangedEvent Failed : SceneAssetsManager Instance Is Not Yet Initialized.");
                    }
                    else
                        Debug.LogWarning("--> OnColorSwatchPickerValueChangedEvent Failed : Dropdown ID Is Null.");

                    break;
            }
        }

        protected override void OnActionDropdownValueChangedEvent(int value, AppData.DropdownDataPackets dataPackets)
        {

        }

        protected override void OnActionDropdownValueChangedEvent(int value, List<string> contentList, AppData.DropdownDataPackets dataPackets)
        {
            switch (dataPackets.action)
            {
                case AppData.InputDropDownActionType.ColorModeSelection:

                    currentColorMode = (AppData.ColorSpaceType)value;

                    switch (currentColorMode)
                    {
                        case AppData.ColorSpaceType.RGBA:

                            UpdateInputFieldInfo(AppData.InputSliderActionType.RedColorChannelField, "R", GetRGBColorValueOfType(AppData.ColorValueType.Red));
                            UpdateInputFieldInfo(AppData.InputSliderActionType.GreenColorChannelField, "G", GetRGBColorValueOfType(AppData.ColorValueType.Green));
                            UpdateInputFieldInfo(AppData.InputSliderActionType.BlueColorChannelField, "B", GetRGBColorValueOfType(AppData.ColorValueType.Blue));
                            UpdateInputFieldInfo(AppData.InputSliderActionType.AlphaColorChannelField, "A", GetRGBColorValueOfType(AppData.ColorValueType.Alpha));

                            UpdateInputFieldUIInputState(AppData.InputSliderActionType.AlphaColorChannelField, AppData.InputUIState.Enabled);

                            if (resetAlphaValueOnModeChanged)
                            {
                                float rgbaAlphaValue = currentColorInfo.color.a * 255;
                                ResetInputFieldValue(AppData.InputSliderActionType.AlphaColorChannelField, rgbaAlphaValue);
                            }

                            currentColorInfo.color = Color.HSVToRGB(colorHSVData.hue, colorHSVData.saturation, colorHSVData.value);

                            UpdateActionInputs(currentColorInfo, false, false);

                            break;

                        case AppData.ColorSpaceType.RGB:


                            UpdateInputFieldInfo(AppData.InputSliderActionType.RedColorChannelField, "R", GetRGBColorValueOfType(AppData.ColorValueType.Red));
                            UpdateInputFieldInfo(AppData.InputSliderActionType.GreenColorChannelField, "G", GetRGBColorValueOfType(AppData.ColorValueType.Green));
                            UpdateInputFieldInfo(AppData.InputSliderActionType.BlueColorChannelField, "B", GetRGBColorValueOfType(AppData.ColorValueType.Blue));

                            UpdateInputFieldUIInputState(AppData.InputSliderActionType.AlphaColorChannelField, AppData.InputUIState.Disabled);

                            if (resetAlphaValueOnModeChanged)
                            {
                                float rgbAlphaValue = 1 * 255;
                                ResetInputFieldValue(AppData.InputSliderActionType.AlphaColorChannelField, rgbAlphaValue);
                            }

                            currentColorInfo.color = Color.HSVToRGB(colorHSVData.hue, colorHSVData.saturation, colorHSVData.value);

                            UpdateActionInputs(currentColorInfo, false, false);

                            break;

                        case AppData.ColorSpaceType.HSV:

                            UpdateInputFieldInfo(AppData.InputSliderActionType.RedColorChannelField, "H", currentColorInfo.color);
                            UpdateInputFieldInfo(AppData.InputSliderActionType.GreenColorChannelField, "S", GetRGBColorValueOfType(AppData.ColorValueType.Saturation));
                            UpdateInputFieldInfo(AppData.InputSliderActionType.BlueColorChannelField, "V", GetRGBColorValueOfType(AppData.ColorValueType.Value));

                            UpdateInputFieldUIInputState(AppData.InputSliderActionType.AlphaColorChannelField, AppData.InputUIState.Disabled);

                            if (resetAlphaValueOnModeChanged)
                            {
                                float hsvAlphaValue = currentColorInfo.color.a * 255;
                                ResetInputFieldValue(AppData.InputSliderActionType.AlphaColorChannelField, hsvAlphaValue);
                            }

                            Color.RGBToHSV(currentColorInfo.color, out colorHSVData.hue, out colorHSVData.saturation, out colorHSVData.value);
                            currentColorInfo.color = Color.HSVToRGB(colorHSVData.hue, colorHSVData.saturation, colorHSVData.value);

                            UpdateActionInputs(currentColorInfo, false, false);
                            SetActionDropdownState(AppData.InputDropDownActionType.SwatchPicker, AppData.InputUIState.Disabled);

                            OnSelectDropDownOption(AppData.InputDropDownActionType.ColorPickerSelection, 1);
                            //ShowWidgetOnDropDownSelection(1, AppData.InputDropDownActionType.ColorPickerSelection);

                            break;
                    }

                    break;

                case AppData.InputDropDownActionType.ColorPickerSelection:

                    AppData.ColorPickerType pickerType = (AppData.ColorPickerType)value;

                    if (pickerType == AppData.ColorPickerType.Gradient)
                    {
                        // Disable Swatches Drop Down.
                        SetActionButtonState(AppData.InputActionButtonType.ClearAllButton, AppData.InputUIState.Disabled);
                        SetActionDropdownState(AppData.InputDropDownActionType.SwatchPicker, AppData.InputUIState.Disabled);
                    }
                    else
                    {
                        SetActionButtonState(AppData.InputActionButtonType.ClearAllButton, AppData.InputUIState.Enabled);
                        SetActionDropdownState(AppData.InputDropDownActionType.SwatchPicker, AppData.InputUIState.Enabled);
                    }

                    ShowWidgetOnDropDownSelection(AppData.Helpers.GetStringToEnum<AppData.SettingsWidgetTabID>(contentList[value]), dataPackets.action);

                    break;
            }
        }

        protected override void OnInputSliderValueChangedEvent(float value, AppData.InputSliderDataPackets dataPackets)
        {
            switch (dataPackets.action)
            {
                case AppData.InputSliderActionType.RedColorChannelField:

                    if (currentColorMode == AppData.ColorSpaceType.RGBA || currentColorMode == AppData.ColorSpaceType.RGB)
                        currentColorInfo.color.r = value;

                    if (currentColorMode == AppData.ColorSpaceType.HSV)
                    {
                        Color.RGBToHSV(currentColorInfo.color, out colorHSVData.hue, out colorHSVData.saturation, out colorHSVData.value);
                        colorHSVData.hue = value;

                        currentColorInfo.color = Color.HSVToRGB(colorHSVData.hue, colorHSVData.saturation, colorHSVData.value);

                        UpdateInputFieldInfo(AppData.InputSliderActionType.RedColorChannelField, "H", currentColorInfo.color);
                    }

                    break;

                case AppData.InputSliderActionType.GreenColorChannelField:

                    if (currentColorMode == AppData.ColorSpaceType.RGBA || currentColorMode == AppData.ColorSpaceType.RGB)
                        currentColorInfo.color.g = value;


                    if (currentColorMode == AppData.ColorSpaceType.HSV)
                    {
                        Color.RGBToHSV(currentColorInfo.color, out colorHSVData.hue, out colorHSVData.saturation, out colorHSVData.value);
                        colorHSVData.saturation = value;

                        currentColorInfo.color = Color.HSVToRGB(colorHSVData.hue, colorHSVData.saturation, colorHSVData.value);
                    }

                    break;

                case AppData.InputSliderActionType.BlueColorChannelField:

                    if (currentColorMode == AppData.ColorSpaceType.RGBA || currentColorMode == AppData.ColorSpaceType.RGB)
                        currentColorInfo.color.b = value;


                    if (currentColorMode == AppData.ColorSpaceType.HSV)
                    {
                        Color.RGBToHSV(currentColorInfo.color, out colorHSVData.hue, out colorHSVData.saturation, out colorHSVData.value);
                        colorHSVData.value = value;

                        currentColorInfo.color = Color.HSVToRGB(colorHSVData.hue, colorHSVData.saturation, colorHSVData.value);
                    }

                    break;

                case AppData.InputSliderActionType.AlphaColorChannelField:

                    if (currentColorMode == AppData.ColorSpaceType.RGBA)
                        currentColorInfo.color.a = value;
                    else if (currentColorMode == AppData.ColorSpaceType.HSV)
                        Debug.Log("");

                    break;
            }

            if (SceneAssetsManager.Instance != null)
            {

                if (currentColorMode == AppData.ColorSpaceType.HSV)
                    currentColorInfo.color = Color.HSVToRGB(colorHSVData.hue, colorHSVData.saturation, colorHSVData.value);

                SceneAssetsManager.Instance.GetHexidecimalFromColor(currentColorInfo.color, (callbackResults) =>
                {
                    if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                        UpdateActionInputs(callbackResults.data);
                    else
                        Debug.LogError($"--> OnSliderValueChangedEvent Failed With Results : {callbackResults.results}");
                });
            }
            else
                Debug.LogWarning("--> OnSliderValueChangedEvent Failed : SceneAssetsManager.Instance Is Not Yet initialized.");
        }

        protected override void OnInputSliderValueChangedEvent(string value, AppData.InputSliderDataPackets dataPackets)
        {
            switch (dataPackets.action)
            {
                case AppData.InputSliderActionType.RedColorChannelField:

                    if (!string.IsNullOrEmpty(value))
                    {
                        int redValue;

                        if (int.TryParse(value, out redValue))
                        {
                            #region RGBB-A

                            if (currentColorMode == AppData.ColorSpaceType.RGBA || currentColorMode == AppData.ColorSpaceType.RGB)
                            {
                                Color color = new Color((float)redValue / rgbColorValueMultiplier, (float)GetColorValueInt(currentColorInfo.color.g, rgbColorValueMultiplier) / rgbColorValueMultiplier, (float)GetColorValueInt(currentColorInfo.color.b, rgbColorValueMultiplier) / rgbColorValueMultiplier, (float)GetColorValueInt(currentColorInfo.color.a, rgbColorValueMultiplier) / rgbColorValueMultiplier);

                                if (SceneAssetsManager.Instance != null)
                                {
                                    SceneAssetsManager.Instance.GetHexidecimalFromColor(color, (callbackResults) =>
                                    {
                                        if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                                            UpdateActionInputs(callbackResults.data, false, false);
                                        else
                                            Debug.LogError($"--> OnInputValueChangedEvent Failed With Results : {callbackResults.results}.");
                                    });
                                }
                                else
                                    Debug.LogWarning("--> OnInputValueChangedEvent Failed : Couldn't Try Parse Value To Int.");
                            }

                            #endregion

                            #region HSV

                            if (currentColorMode == AppData.ColorSpaceType.HSV)
                            {
                                Color.RGBToHSV(currentColorInfo.color, out colorHSVData.hue, out colorHSVData.saturation, out colorHSVData.value);
                                colorHSVData.hue = (float)redValue / hsvHueColorValueMultiplier;

                                Color color = Color.HSVToRGB(colorHSVData.hue, colorHSVData.saturation, colorHSVData.value);

                                if (SceneAssetsManager.Instance != null)
                                {
                                    SceneAssetsManager.Instance.GetHexidecimalFromColor(color, (callbackResults) =>
                                    {
                                        if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                                            UpdateActionInputs(callbackResults.data, false, false);
                                        else
                                            Debug.LogError($"--> OnInputValueChangedEvent Failed With Results : {callbackResults.results}.");
                                    });
                                }
                                else
                                    Debug.LogWarning("--> OnInputValueChangedEvent Failed : Couldn't Try Parse Value To Int.");
                            }

                            #endregion
                        }
                        else
                            Debug.LogWarning("--> OnInputValueChangedEvent Failed : Couldn't Try Parse Value To Int.");
                    }
                    else
                    {
                        #region RGB-A

                        if (currentColorMode == AppData.ColorSpaceType.RGBA || currentColorMode == AppData.ColorSpaceType.RGB)
                        {
                            Color color;
                            color = new Color(0.0f, (float)GetColorValueInt(currentColorInfo.color.g, rgbColorValueMultiplier) / rgbColorValueMultiplier, (float)GetColorValueInt(currentColorInfo.color.b, rgbColorValueMultiplier) / rgbColorValueMultiplier, (float)GetColorValueInt(currentColorInfo.color.a, rgbColorValueMultiplier) / rgbColorValueMultiplier);

                            if (SceneAssetsManager.Instance != null)
                            {
                                SceneAssetsManager.Instance.GetHexidecimalFromColor(color, (callbackResults) =>
                                {
                                    if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                                        UpdateActionInputs(callbackResults.data);
                                    else
                                        Debug.LogError($"--> OnInputValueChangedEvent Failed With Results : {callbackResults.results}.");
                                });
                            }
                            else
                                Debug.LogWarning("--> OnInputValueChangedEvent Failed : Couldn't Try Parse Value To Int.");
                        }

                        #endregion

                        #region HSV

                        if (currentColorMode == AppData.ColorSpaceType.HSV)
                        {
                            Color color;
                            color = Color.HSVToRGB(0.0f, colorHSVData.saturation, colorHSVData.value);
                            Color.RGBToHSV(color, out colorHSVData.hue, out colorHSVData.saturation, out colorHSVData.value);

                            if (SceneAssetsManager.Instance != null)
                            {
                                SceneAssetsManager.Instance.GetHexidecimalFromColor(color, (callbackResults) =>
                                {
                                    if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                                        UpdateActionInputs(callbackResults.data);
                                    else
                                        Debug.LogError($"--> OnInputValueChangedEvent Failed With Results : {callbackResults.results}.");
                                });
                            }
                            else
                                Debug.LogWarning("--> OnInputValueChangedEvent Failed : Couldn't Try Parse Value To Int.");
                        }

                        #endregion
                    }

                    break;

                case AppData.InputSliderActionType.GreenColorChannelField:

                    if (!string.IsNullOrEmpty(value))
                    {
                        int greenValue;

                        if (int.TryParse(value, out greenValue))
                        {
                            #region RGB-A

                            if (currentColorMode == AppData.ColorSpaceType.RGBA || currentColorMode == AppData.ColorSpaceType.RGB)
                            {
                                Color color;
                                color = new Color((float)GetColorValueInt(currentColorInfo.color.r, rgbColorValueMultiplier) / rgbColorValueMultiplier, (float)greenValue / rgbColorValueMultiplier, (float)GetColorValueInt(currentColorInfo.color.b, rgbColorValueMultiplier) / rgbColorValueMultiplier, (float)GetColorValueInt(currentColorInfo.color.a, rgbColorValueMultiplier) / rgbColorValueMultiplier);

                                if (SceneAssetsManager.Instance != null)
                                {
                                    SceneAssetsManager.Instance.GetHexidecimalFromColor(color, (callbackResults) =>
                                    {
                                        if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                                            UpdateActionInputs(callbackResults.data, false, false);
                                        else
                                            Debug.LogError($"--> OnInputValueChangedEvent Failed With Results : {callbackResults.results}.");
                                    });
                                }
                                else
                                    Debug.LogWarning("--> OnInputValueChangedEvent Failed : Couldn't Try Parse Value To Int.");
                            }

                            #endregion

                            #region HSV

                            if (currentColorMode == AppData.ColorSpaceType.HSV)
                            {
                                Color.RGBToHSV(currentColorInfo.color, out colorHSVData.hue, out colorHSVData.saturation, out colorHSVData.value);
                                colorHSVData.saturation = (float)greenValue / hsvColorSaturationValueMultiplier;

                                Color color = Color.HSVToRGB(colorHSVData.hue, colorHSVData.saturation, colorHSVData.value);

                                if (SceneAssetsManager.Instance != null)
                                {
                                    SceneAssetsManager.Instance.GetHexidecimalFromColor(color, (callbackResults) =>
                                    {
                                        if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                                            UpdateActionInputs(callbackResults.data, false, false);
                                        else
                                            Debug.LogError($"--> OnInputValueChangedEvent Failed With Results : {callbackResults.results}.");
                                    });
                                }
                                else
                                    Debug.LogWarning("--> OnInputValueChangedEvent Failed : Couldn't Try Parse Value To Int.");
                            }

                            #endregion
                        }
                        else
                            Debug.LogWarning("--> OnInputValueChangedEvent Failed : Couldn't Try Parse Value To Int.");
                    }
                    else
                    {
                        #region RGB-A

                        if (currentColorMode == AppData.ColorSpaceType.RGBA || currentColorMode == AppData.ColorSpaceType.RGB)
                        {
                            Color color;
                            color = new Color((float)GetColorValueInt(currentColorInfo.color.r, rgbColorValueMultiplier) / rgbColorValueMultiplier, 0.0f, (float)GetColorValueInt(currentColorInfo.color.b, rgbColorValueMultiplier) / rgbColorValueMultiplier, (float)GetColorValueInt(currentColorInfo.color.a, rgbColorValueMultiplier) / rgbColorValueMultiplier);

                            if (SceneAssetsManager.Instance != null)
                            {
                                SceneAssetsManager.Instance.GetHexidecimalFromColor(color, (callbackResults) =>
                                {
                                    if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                                        UpdateActionInputs(callbackResults.data);
                                    else
                                        Debug.LogError($"--> OnInputValueChangedEvent Failed With Results : {callbackResults.results}.");
                                });
                            }
                            else
                                Debug.LogWarning("--> OnInputValueChangedEvent Failed : Couldn't Try Parse Value To Int.");
                        }

                        #endregion

                        #region HSV

                        if (currentColorMode == AppData.ColorSpaceType.HSV)
                        {
                            Color color;
                            color = Color.HSVToRGB(colorHSVData.hue, 0.0f, colorHSVData.value);
                            Color.RGBToHSV(color, out colorHSVData.hue, out colorHSVData.saturation, out colorHSVData.value);

                            if (SceneAssetsManager.Instance != null)
                            {
                                SceneAssetsManager.Instance.GetHexidecimalFromColor(color, (callbackResults) =>
                                {
                                    if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                                        UpdateActionInputs(callbackResults.data);
                                    else
                                        Debug.LogError($"--> OnInputValueChangedEvent Failed With Results : {callbackResults.results}.");
                                });
                            }
                            else
                                Debug.LogWarning("--> OnInputValueChangedEvent Failed : Couldn't Try Parse Value To Int.");
                        }

                        #endregion
                    }

                    break;

                case AppData.InputSliderActionType.BlueColorChannelField:

                    if (!string.IsNullOrEmpty(value))
                    {
                        int blueValue;

                        if (int.TryParse(value, out blueValue))
                        {
                            #region RGB-A

                            if (currentColorMode == AppData.ColorSpaceType.RGBA || currentColorMode == AppData.ColorSpaceType.RGB)
                            {
                                Color color;
                                color = new Color((float)GetColorValueInt(currentColorInfo.color.r, rgbColorValueMultiplier) / rgbColorValueMultiplier, (float)GetColorValueInt(currentColorInfo.color.g, rgbColorValueMultiplier) / rgbColorValueMultiplier, (float)blueValue / rgbColorValueMultiplier, (float)GetColorValueInt(currentColorInfo.color.a, rgbColorValueMultiplier) / rgbColorValueMultiplier);

                                if (SceneAssetsManager.Instance != null)
                                {
                                    SceneAssetsManager.Instance.GetHexidecimalFromColor(color, (callbackResults) =>
                                    {
                                        if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                                            UpdateActionInputs(callbackResults.data, false, false);
                                        else
                                            Debug.LogError($"--> OnInputValueChangedEvent Failed With Results : {callbackResults.results}.");
                                    });
                                }
                                else
                                    Debug.LogWarning("--> OnInputValueChangedEvent Failed : Couldn't Try Parse Value To Int.");
                            }

                            #endregion

                            #region HSV

                            if (currentColorMode == AppData.ColorSpaceType.HSV)
                            {
                                Color.RGBToHSV(currentColorInfo.color, out colorHSVData.hue, out colorHSVData.saturation, out colorHSVData.value);
                                colorHSVData.value = (float)blueValue / hsvColorSaturationValueMultiplier;

                                Color color = Color.HSVToRGB(colorHSVData.hue, colorHSVData.saturation, colorHSVData.value);

                                if (SceneAssetsManager.Instance != null)
                                {
                                    SceneAssetsManager.Instance.GetHexidecimalFromColor(color, (callbackResults) =>
                                    {
                                        if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                                            UpdateActionInputs(callbackResults.data, false, false);
                                        else
                                            Debug.LogError($"--> OnInputValueChangedEvent Failed With Results : {callbackResults.results}.");
                                    });
                                }
                                else
                                    Debug.LogWarning("--> OnInputValueChangedEvent Failed : Couldn't Try Parse Value To Int.");
                            }

                            #endregion
                        }
                        else
                            Debug.LogWarning("--> OnInputValueChangedEvent Failed : Couldn't Try Parse Value To Int.");
                    }
                    else
                    {
                        #region RGB-A

                        if (currentColorMode == AppData.ColorSpaceType.RGBA || currentColorMode == AppData.ColorSpaceType.RGB)
                        {
                            Color color;
                            color = new Color((float)GetColorValueInt(currentColorInfo.color.r, rgbColorValueMultiplier) / rgbColorValueMultiplier, (float)GetColorValueInt(currentColorInfo.color.g, rgbColorValueMultiplier) / rgbColorValueMultiplier, 0.0f, (float)GetColorValueInt(currentColorInfo.color.a, rgbColorValueMultiplier) / rgbColorValueMultiplier);

                            if (SceneAssetsManager.Instance != null)
                            {
                                SceneAssetsManager.Instance.GetHexidecimalFromColor(color, (callbackResults) =>
                                {
                                    if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                                        UpdateActionInputs(callbackResults.data);
                                    else
                                        Debug.LogError($"--> OnInputValueChangedEvent Failed With Results : {callbackResults.results}.");
                                });
                            }
                            else
                                Debug.LogWarning("--> OnInputValueChangedEvent Failed : Couldn't Try Parse Value To Int.");
                        }

                        #endregion

                        #region HSV

                        if (currentColorMode == AppData.ColorSpaceType.HSV)
                        {
                            Color color;
                            color = Color.HSVToRGB(colorHSVData.hue, colorHSVData.saturation, 0.0f);
                            Color.RGBToHSV(color, out colorHSVData.hue, out colorHSVData.saturation, out colorHSVData.value);

                            if (SceneAssetsManager.Instance != null)
                            {
                                SceneAssetsManager.Instance.GetHexidecimalFromColor(color, (callbackResults) =>
                                {
                                    if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                                        UpdateActionInputs(callbackResults.data);
                                    else
                                        Debug.LogError($"--> OnInputValueChangedEvent Failed With Results : {callbackResults.results}.");
                                });
                            }
                            else
                                Debug.LogWarning("--> OnInputValueChangedEvent Failed : Couldn't Try Parse Value To Int.");
                        }

                        #endregion
                    }

                    break;

                case AppData.InputSliderActionType.AlphaColorChannelField:

                    if (!string.IsNullOrEmpty(value))
                    {
                        int alphaValue;

                        if (int.TryParse(value, out alphaValue))
                        {
                            #region RGB-A

                            Color color = new Color((float)GetColorValueInt(currentColorInfo.color.r, rgbColorValueMultiplier) / rgbColorValueMultiplier, (float)GetColorValueInt(currentColorInfo.color.g, rgbColorValueMultiplier) / rgbColorValueMultiplier, (float)GetColorValueInt(currentColorInfo.color.b, rgbColorValueMultiplier) / rgbColorValueMultiplier, (float)alphaValue / rgbColorValueMultiplier);

                            if (SceneAssetsManager.Instance != null)
                            {
                                SceneAssetsManager.Instance.GetHexidecimalFromColor(color, (callbackResults) =>
                                {
                                    if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                                        UpdateActionInputs(callbackResults.data);
                                    else
                                        Debug.LogError($"--> OnInputValueChangedEvent Failed With Results : {callbackResults.results}.");
                                });
                            }
                            else
                                Debug.LogWarning("--> OnInputValueChangedEvent Failed : Couldn't Try Parse Value To Int.");

                            #endregion
                        }
                        else
                            Debug.LogWarning("--> OnInputValueChangedEvent Failed : Couldn't Try Parse Value To Int.");
                    }
                    else
                    {
                        #region RGB-A

                        Color color = new Color((float)GetColorValueInt(currentColorInfo.color.r, rgbColorValueMultiplier) / rgbColorValueMultiplier, (float)GetColorValueInt(currentColorInfo.color.g, rgbColorValueMultiplier) / rgbColorValueMultiplier, (float)GetColorValueInt(currentColorInfo.color.b, rgbColorValueMultiplier) / rgbColorValueMultiplier, 0.0f);

                        if (SceneAssetsManager.Instance != null)
                        {
                            SceneAssetsManager.Instance.GetHexidecimalFromColor(color, (callbackResults) =>
                            {
                                if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                                    UpdateActionInputs(callbackResults.data);
                                else
                                    Debug.LogError($"--> OnInputValueChangedEvent Failed With Results : {callbackResults.results}.");
                            });
                        }
                        else
                            Debug.LogWarning("--> OnInputValueChangedEvent Failed : Couldn't Try Parse Value To Int.");

                        #endregion
                    }

                    break;
            }
        }

        Color GetRGBColorValueOfType(AppData.ColorValueType valueType)
        {
            if (colorValueList.Count > 0)
            {
                AppData.RGBColorValue colorValue = colorValueList.Find((x) => x.valueType == valueType);

                if (colorValue != null)
                    return colorValue.GetColor();
                else
                    Debug.LogWarning($"--> GetRGBColorValueOfType Failed : Color Value Of Type : {valueType} Not Found.");
            }

            return Color.clear;
        }

        protected override void OnActionCheckboxValueChangedEvent(bool value, AppData.CheckboxDataPackets dataPackets)
        {
            switch (dataPackets.action)
            {
                case AppData.CheckboxInputActionType.ToggleColorDropPicker:

                    isColorDropPickerMode = !value;

                    AppData.ActionEvents.OnToggleColorDropPickerEvent(isColorDropPickerMode);

                    OnColorDropPicker(isColorDropPickerMode);

                    break;
            }
        }

        #endregion


        void OnCreateButton_Action(Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (SceneAssetsManager.Instance != null)
            {
                SceneAssetsManager.Instance.GetColorSwatchData((getSwatchDataCallbackResults) =>
                {
                    if (AppData.Helpers.IsSuccessCode(getSwatchDataCallbackResults.resultsCode))
                    {
                        getSwatchDataCallbackResults.data.CreateColorInCustomSwatch(storageDirectoryData.name, newSwatchFileName, currentColorInfo, storageDirectoryData.type, (callbackDataResults) =>
                        {
                            if (AppData.Helpers.IsSuccessCode(callbackDataResults.resultsCode))
                            {
                                tempColorInfo = currentColorInfo;
                                UpdateDropDownContent(AppData.InputDropDownActionType.SwatchPicker, callbackDataResults.data, false);
                                OnCreateColorButtonVisibility(false);
                            }
                            else
                                Debug.LogError($"--> OnCreateNewColorButtonClickedEvent Failed With results : {callbackDataResults.results}");
                        });

                    }
                    else
                        Debug.LogError($"--> OnCreateNewColorButtonClickedEvent Failed With results : {getSwatchDataCallbackResults.results}");

                    callbackResults.results = getSwatchDataCallbackResults.results;
                    callbackResults.resultsCode = getSwatchDataCallbackResults.resultsCode;
                });
            }
            else
            {
                callbackResults.results = "OnCreateNewColorButtonClickedEvent Failed: SceneAssetsManager.Instance Is Not Yet Initialized.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void OnGenerateNewColorSwatch(List<AppData.ColorInfo> colorInfoList, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            Debug.LogError($"====> Generate : {colorInfoList.Count} Colors Now Broer.");

            if (colorInfoList.Count > 0)
            {
                int generatedColorsCount = 0;

                for (int i = 0; i < colorInfoList.Count; i++)
                {
                    if (SceneAssetsManager.Instance != null)
                    {
                        SceneAssetsManager.Instance.GetColorSwatchData((getSwatchDataCallbackResults) =>
                        {
                            if (AppData.Helpers.IsSuccessCode(getSwatchDataCallbackResults.resultsCode))
                            {
                                getSwatchDataCallbackResults.data.CreateColorInCustomSwatch(storageDirectoryData.name, newSwatchFileName, colorInfoList[i], storageDirectoryData.type, (callbackDataResults) =>
                                {
                                    if (AppData.Helpers.IsSuccessCode(callbackDataResults.resultsCode))
                                    {
                                        if (generatedColorsCount == 0)
                                        {
                                            tempColorInfo = currentColorInfo;
                                            tempCustomSwatchContent = callbackDataResults.data;
                                        }

                                        generatedColorsCount++;
                                    }
                                    else
                                        Debug.LogError($"--> OnCreateNewColorButtonClickedEvent Failed With results : {callbackDataResults.results}");
                                });

                            }
                            else
                                Debug.LogError($"--> OnCreateNewColorButtonClickedEvent Failed With results : {getSwatchDataCallbackResults.results}");

                            callbackResults.results = getSwatchDataCallbackResults.results;
                            callbackResults.resultsCode = getSwatchDataCallbackResults.resultsCode;
                        });
                    }
                    else
                    {
                        callbackResults.results = "OnCreateNewColorButtonClickedEvent Failed: SceneAssetsManager.Instance Is Not Yet Initialized.";
                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                    }
                }

                if (generatedColorsCount == colorInfoList.Count)
                {
                    callbackResults.results = $"Success - {colorInfoList.Count} Colors Generated Successfully.";
                    callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = $"Failed - Couldn't Populate Swatch With : {colorInfoList.Count} Colors.";
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;

                }
            }

            callback?.Invoke(callbackResults);
        }

        public void SelectGeneratedColorSwatch()
        {
            if (tempCustomSwatchContent != null && tempCustomSwatchContent.Count > 0)
            {
                OnResetDropDownSelection(AppData.InputDropDownActionType.ColorPickerSelection);
                UpdateDropDownContent(AppData.InputDropDownActionType.SwatchPicker, tempCustomSwatchContent, false);
            }
            else
                LogError("Temp Custom Swatch Content Is Nill / Empty.", this, () => SelectGeneratedColorSwatch());
        }

        void OnRevertSettings_Action() => UpdateActionInputs(tempColorInfo, false, true);

        void OnClearAllSwatchColors_Action() => ShowChildWidget(AppData.SettingsWidgetType.CustomSwatchConfirmationWidget);

        public void OnDeselect(BaseEventData eventData)
        {
            Debug.LogError("------------------------> Deselected");
        }

        void OnColorDropPicker(bool enabled)
        {
            if (dropPickerHandler != null)
            {

                if (enabled)
                    dropPickerHandler.Enable();
                else
                    dropPickerHandler.Disable();
            }
            else
                LogError("", this, () => OnColorDropPicker(enabled));
        }

        protected override void OnResetWidgetData(AppData.SettingsWidgetType widgetType)
        {

        }

        protected override void OnWidgetOpened()
        {
        }
    }
}
