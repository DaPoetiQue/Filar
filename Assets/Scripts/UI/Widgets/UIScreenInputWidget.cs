using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class UIScreenInputWidget : AppData.UIScreenInputComponent
    {
        #region Components

        [Header("Widget Settings")]
        public AppData.InputType inputType = AppData.InputType.None;

        #region UI View

        [Header("::: UI Text View Info")]

        public AppData.ScreenTextType textView = AppData.ScreenTextType.None;

        [Header("::: UI Image View Info")]
        public AppData.UIImageDisplayerType imagenView = AppData.UIImageDisplayerType.None;

        #endregion

        #region UI Data Component 

        #region Actions Config

        public AppData.UIButton<AppData.ButtonDataPackets> buttonComponentConfig;
        public AppData.UIInputField<AppData.InputFieldDataPackets> inputFieldComponentConfig;
        public AppData.UIInputSlider<AppData.InputSliderDataPackets> inputSliderComponentConfig;
        public AppData.UISlider<AppData.SliderDataPackets> sliderComponentConfig;
        public AppData.UICheckbox<AppData.CheckboxDataPackets> checkboxComponentConfig;
        public AppData.UIDropDown<AppData.DropdownDataPackets> dropdownComponentConfig;

        #endregion

        #region Displayers Config

        public AppData.UIText<AppData.TextDataPackets> textComponentConfig;
        public AppData.UIImageDisplayer<AppData.ImageDataPackets> imageComponentConfig;

        #endregion

        #endregion

        #endregion

        #region Main

        public void Init<T>(Action<AppData.CallbackData<T>> callback = null) where T: AppData.SceneDataPackets
        {
            AppData.CallbackData<T> callbackResults = new AppData.CallbackData<T>();

            switch(inputType)
            {
                case AppData.InputType.Button:

                    GetInputDataPacket<AppData.ButtonDataPackets>(inputCallbackResults =>
                    {
                        callbackResults.data = inputCallbackResults.data as T;
                        callbackResults.result = inputCallbackResults.result;
                        callbackResults.resultCode = inputCallbackResults.resultCode;
                    });

                    break;
            }

            callback?.Invoke(callbackResults);
        }

        public void SetName(string name) => this.name = name;

        public new AppData.CallbackData<AppData.InputType> GetType()
        {
            var callbackResults = new AppData.CallbackData<AppData.InputType>();

            if(inputType != AppData.InputType.None)
            {
                callbackResults.result = $"UI Screen Input Widget : {GetName()} Is Set To Type : {inputType}.";
                callbackResults.data = inputType;
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = $"Get Input Type Failed - Input Type For : {GetName()} Is Set To Default : {inputType} - Invalid Operation";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
            }

            return callbackResults;
        }

        public void GetInputDataPacket<T>(Action<AppData.CallbackData<T>> callback) where T : AppData.SceneDataPackets
        {
            AppData.CallbackData<T> callbackResults = new AppData.CallbackData<T>();

            if(inputType != AppData.InputType.None)
            {
                switch(inputType)
                {
                    case AppData.InputType.Button:

                        if (buttonComponentConfig != null && buttonComponentConfig.value != null)
                        {

                            callbackResults.result = (buttonComponentConfig.value) ? $"Action Name : {buttonComponentConfig.name} - Input Type Is Set To {inputType} Successfully" : $"Action Name : {buttonComponentConfig.name} - Input Type Is Set To {inputType} But Value Is Missing.";
                            callbackResults.data = buttonComponentConfig.dataPackets as T;
                            callbackResults.resultCode = (buttonComponentConfig.value) ? AppData.Helpers.SuccessCode : AppData.Helpers.ErrorCode;
                        }
                        else
                        {
                            callbackResults.result = "Button Component Is Missing / Null / Not Found.";
                            callbackResults.data = default;
                            callbackResults.resultCode = AppData.Helpers.ErrorCode;
                        }

                        break;

                    case AppData.InputType.InputField:

                        if (inputFieldComponentConfig != null && inputFieldComponentConfig.value != null)
                        {
                            callbackResults.result = (inputFieldComponentConfig.value) ? $"Action Name : {inputFieldComponentConfig.name} - Input Type Is Set To {inputType} Successfully" : $"Action Name : {inputFieldComponentConfig.name} - Input Type Is Set To {inputType} But Value Is Missing.";
                            callbackResults.data = inputFieldComponentConfig.dataPackets as T;
                            callbackResults.resultCode = (inputFieldComponentConfig.value) ? AppData.Helpers.SuccessCode : AppData.Helpers.ErrorCode;
                        }
                        else
                        {
                            callbackResults.result = "Input Field Component Is Missing / Null / Not Found.";
                            callbackResults.data = default;
                            callbackResults.resultCode = AppData.Helpers.ErrorCode;
                        }

                        break;

                    case AppData.InputType.InputSlider:

                        if (inputSliderComponentConfig != null && inputSliderComponentConfig.value != null)
                        {
                            callbackResults.result = (inputSliderComponentConfig.value) ? $"Action Name : {inputSliderComponentConfig.name} - Input Type Is Set To {inputType} Successfully" : $"Action Name : {inputSliderComponentConfig.name} - Input Type Is Set To {inputType} But Value Is Missing.";
                            callbackResults.data = inputSliderComponentConfig.dataPackets as T;
                            callbackResults.resultCode = (inputSliderComponentConfig.value) ? AppData.Helpers.SuccessCode : AppData.Helpers.ErrorCode;
                        }
                        else
                        {
                            callbackResults.result = "Input Slider Component Is Missing / Null / Not Found.";
                            callbackResults.data = default;
                            callbackResults.resultCode = AppData.Helpers.ErrorCode;
                        }

                        break;

                    case AppData.InputType.Slider:

                        if (sliderComponentConfig != null && sliderComponentConfig.value != null)
                        {
                            callbackResults.result = (sliderComponentConfig.value) ? $"Action Name : {sliderComponentConfig.name} - Input Type Is Set To {inputType} Successfully" : $"Action Name : {sliderComponentConfig.name} - Input Type Is Set To {inputType} But Value Is Missing.";
                            callbackResults.data = sliderComponentConfig.dataPackets as T;
                            callbackResults.resultCode = (sliderComponentConfig.value) ? AppData.Helpers.SuccessCode : AppData.Helpers.ErrorCode;
                        }
                        else
                        {
                            callbackResults.result = "Slider Component Is Missing / Null / Not Found.";
                            callbackResults.data = default;
                            callbackResults.resultCode = AppData.Helpers.ErrorCode;
                        }

                        break;

                    case AppData.InputType.Checkbox:

                        if (checkboxComponentConfig != null && checkboxComponentConfig.value != null)
                        {
                            callbackResults.result = (checkboxComponentConfig.value) ? $"Action Name : {checkboxComponentConfig.name} - Input Type Is Set To {inputType} Successfully" : $"Action Name : {checkboxComponentConfig.name} - Input Type Is Set To {inputType} But Value Is Missing.";
                            callbackResults.data = checkboxComponentConfig.dataPackets as T;
                            callbackResults.resultCode = (checkboxComponentConfig.value) ? AppData.Helpers.SuccessCode : AppData.Helpers.ErrorCode;
                        }
                        else
                        {
                            callbackResults.result = "Checkbox Component Is Missing / Null / Not Found.";
                            callbackResults.data = default;
                            callbackResults.resultCode = AppData.Helpers.ErrorCode;
                        }

                        break;

                    case AppData.InputType.DropDown:

                        if (dropdownComponentConfig != null && dropdownComponentConfig.value != null)
                        {
                            callbackResults.result = (dropdownComponentConfig.value) ? $"Action Name : {dropdownComponentConfig.name} - Input Type Is Set To {inputType} Successfully" : $"Action Name : {dropdownComponentConfig.name} - Input Type Is Set To {inputType} But Value Is Missing.";
                            callbackResults.data = dropdownComponentConfig.dataPackets as T;
                            callbackResults.resultCode = (dropdownComponentConfig.value) ? AppData.Helpers.SuccessCode : AppData.Helpers.ErrorCode;
                        }
                        else
                        {
                            callbackResults.result = "Dropdown Component Is Missing / Null / Not Found.";
                            callbackResults.data = default;
                            callbackResults.resultCode = AppData.Helpers.ErrorCode;
                        }

                        break;

                    case AppData.InputType.Text:

                        if (textComponentConfig != null && textComponentConfig.value != null)
                        {
                            callbackResults.result = (textComponentConfig.value) ? $"Text Displayer Name : {textComponentConfig.name} - Text Displayer Type Is Set To {inputType} Successfully" : $"Text Displayer Name : {textComponentConfig.name} - Displayer Type Is Set To {inputType} But Value Is Missing.";
                            callbackResults.data = textComponentConfig.dataPackets as T;
                            callbackResults.resultCode = (textComponentConfig.value) ? AppData.Helpers.SuccessCode : AppData.Helpers.ErrorCode;
                        }
                        else
                        {
                            callbackResults.result = "Text Component Is Missing / Null / Not Found.";
                            callbackResults.data = default;
                            callbackResults.resultCode = AppData.Helpers.ErrorCode;
                        }

                        break;

                    case AppData.InputType.Image:

                        if (imageComponentConfig != null && imageComponentConfig.value != null)
                        {
                            callbackResults.result = (imageComponentConfig.value) ? $"Image Displayer Name : {imageComponentConfig.name} - Image Displayer Type Is Set To {inputType} Successfully" : $"Image Displayer Name : {imageComponentConfig.name} - Displayer Type Is Set To {inputType} But Value Is Missing.";
                            callbackResults.data = imageComponentConfig.dataPackets as T;
                            callbackResults.resultCode = (imageComponentConfig.value) ? AppData.Helpers.SuccessCode : AppData.Helpers.ErrorCode;
                        }
                        else
                        {
                            callbackResults.result = "Image Component Is Missing / Null / Not Found.";
                            callbackResults.data = default;
                            callbackResults.resultCode = AppData.Helpers.ErrorCode;
                        }

                        break;
                }
            }
            else
            {
                callbackResults.result = "Input Type Is Set To None";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        public string GetName() => !string.IsNullOrEmpty(name) ? name : "UI Screen Input Widget Name Not Assigned";

        public bool HasComponent(AppData.InputType inputType)
        {
            bool hasComponent = false;

            switch(inputType)
            {
                case AppData.InputType.Button:

                    hasComponent = buttonComponentConfig != null && buttonComponentConfig.value != null;

                    break;

                case AppData.InputType.InputField:

                    hasComponent = inputFieldComponentConfig != null && inputFieldComponentConfig.value != null;

                    break;

                case AppData.InputType.InputSlider:

                    hasComponent = inputSliderComponentConfig != null && inputSliderComponentConfig.value != null;

                    break;

                case AppData.InputType.Slider:

                    hasComponent = sliderComponentConfig != null && sliderComponentConfig.value != null;

                    break;

                case AppData.InputType.Checkbox:

                    hasComponent = checkboxComponentConfig != null && checkboxComponentConfig.value != null;

                    break;

                case AppData.InputType.DropDown:

                    hasComponent = dropdownComponentConfig != null && dropdownComponentConfig.value != null;

                    break;

                case AppData.InputType.Text:

                    hasComponent = textComponentConfig != null && textComponentConfig.value != null;

                    break;

                case AppData.InputType.Image:

                    hasComponent = imageComponentConfig != null && imageComponentConfig.value != null;

                    break;
            }

            return hasComponent;
        }

        #region Check Component Of Type

        #region Actions

        public bool HasComponent(AppData.InputActionButtonType action)
        {
            return buttonComponentConfig != null && buttonComponentConfig.value != null && buttonComponentConfig.dataPackets.action == action;
        }

        public bool HasComponent(AppData.InputFieldActionType action)
        {
            return inputFieldComponentConfig != null && inputFieldComponentConfig.value != null && inputFieldComponentConfig.dataPackets.action == action;
        }

        public bool HasComponent(AppData.InputSliderActionType action)
        {
            return inputSliderComponentConfig != null && inputSliderComponentConfig.value != null && inputSliderComponentConfig.dataPackets.action == action;
        }

        public bool HasComponent(AppData.SliderValueType value)
        {
            return sliderComponentConfig != null && sliderComponentConfig.value != null && sliderComponentConfig.dataPackets.valueType == value;
        }

        public bool HasComponent(AppData.CheckboxInputActionType action)
        {
            return checkboxComponentConfig != null && checkboxComponentConfig.value != null && checkboxComponentConfig.dataPackets.action == action;
        }

        public bool HasComponent(AppData.InputDropDownActionType action)
        {
            return dropdownComponentConfig != null && dropdownComponentConfig.value != null && dropdownComponentConfig.dataPackets.action == action;
        }

        #endregion

        #region Displayers

        public bool HasComponent(AppData.ScreenTextType type)
        {
            return textComponentConfig != null && textComponentConfig.value != null && textComponentConfig.dataPackets.textType == type;
        }

        public bool HasComponent(AppData.ScreenImageType type)
        {
            return imageComponentConfig != null && imageComponentConfig.value != null && imageComponentConfig.dataPackets.imageType == type;
        }

        #endregion

        #endregion

        #region Get Action Components

        public AppData.UIButton<AppData.ButtonDataPackets> GetButtonComponent()
        {
            return buttonComponentConfig;
        }

        public AppData.UIInputField<AppData.InputFieldDataPackets> GetInputFieldComponent()
        {
            return inputFieldComponentConfig;
        }

        public AppData.UIInputSlider<AppData.InputSliderDataPackets> GetInputSliderComponent()
        {
            return inputSliderComponentConfig;
        }

        public AppData.UISlider<AppData.SliderDataPackets> GetSliderComponent()
        {
            return sliderComponentConfig;
        }

        public AppData.UICheckbox<AppData.CheckboxDataPackets> GetCheckboxComponent()
        {
            return checkboxComponentConfig;
        }

        public AppData.UIDropDown<AppData.DropdownDataPackets> GetDropdownComponent()
        {
            return dropdownComponentConfig;
        }

        #endregion

        #region Get Displayer Components

        public AppData.UIText<AppData.TextDataPackets> GetTextComponent()
        {
            return textComponentConfig;
        }

        public AppData.UIImageDisplayer<AppData.ImageDataPackets> GetImageComponent()
        {
            return imageComponentConfig;
        }

        #endregion

        #endregion
    }
}
