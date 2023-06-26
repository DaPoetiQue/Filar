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
                        callbackResults.results = inputCallbackResults.results;
                        callbackResults.resultsCode = inputCallbackResults.resultsCode;
                    });

                    break;
            }

            callback?.Invoke(callbackResults);
        }

        public AppData.InputType GetInputType()
        {
            return inputType;
        }

        public void GetInputDataPacket<T>(Action<AppData.CallbackData<T>> callback) where T : AppData.SceneDataPackets
        {
            AppData.CallbackData<T> callbackResults = new AppData.CallbackData<T>();

            if(inputType != AppData.InputType.None)
            {
                switch(inputType)
                {
                    case AppData.InputType.Button:

                        callbackResults.results = (buttonComponentConfig.value)? $"Action Name : {buttonComponentConfig.name} - Input Type Is Set To {inputType} Successfully" : $"Action Name : {buttonComponentConfig.name} - Input Type Is Set To {inputType} But Value Is Missing.";
                        callbackResults.data = buttonComponentConfig.dataPackets as T;
                        callbackResults.resultsCode = (buttonComponentConfig.value)? AppData.Helpers.SuccessCode : AppData.Helpers.ErrorCode;

                        break;

                    case AppData.InputType.InputField:

                        callbackResults.results = (inputFieldComponentConfig.value) ? $"Action Name : {inputFieldComponentConfig.name} - Input Type Is Set To {inputType} Successfully" : $"Action Name : {inputFieldComponentConfig.name} - Input Type Is Set To {inputType} But Value Is Missing.";
                        callbackResults.data = inputFieldComponentConfig.dataPackets as T;
                        callbackResults.resultsCode = (inputFieldComponentConfig.value) ? AppData.Helpers.SuccessCode : AppData.Helpers.ErrorCode;

                        break;

                    case AppData.InputType.InputSlider:

                        callbackResults.results = (inputSliderComponentConfig.value) ? $"Action Name : {inputSliderComponentConfig.name} - Input Type Is Set To {inputType} Successfully" : $"Action Name : {inputSliderComponentConfig.name} - Input Type Is Set To {inputType} But Value Is Missing.";
                        callbackResults.data = inputSliderComponentConfig.dataPackets as T;
                        callbackResults.resultsCode = (inputSliderComponentConfig.value) ? AppData.Helpers.SuccessCode : AppData.Helpers.ErrorCode;

                        break;

                    case AppData.InputType.Slider:

                        callbackResults.results = (sliderComponentConfig.value) ? $"Action Name : {sliderComponentConfig.name} - Input Type Is Set To {inputType} Successfully" : $"Action Name : {sliderComponentConfig.name} - Input Type Is Set To {inputType} But Value Is Missing.";
                        callbackResults.data = sliderComponentConfig.dataPackets as T;
                        callbackResults.resultsCode = (sliderComponentConfig.value) ? AppData.Helpers.SuccessCode : AppData.Helpers.ErrorCode;

                        break;

                    case AppData.InputType.Checkbox:

                        callbackResults.results = (checkboxComponentConfig.value) ? $"Action Name : {checkboxComponentConfig.name} - Input Type Is Set To {inputType} Successfully" : $"Action Name : {checkboxComponentConfig.name} - Input Type Is Set To {inputType} But Value Is Missing.";
                        callbackResults.data = checkboxComponentConfig.dataPackets as T;
                        callbackResults.resultsCode = (checkboxComponentConfig.value) ? AppData.Helpers.SuccessCode : AppData.Helpers.ErrorCode;

                        break;

                    case AppData.InputType.DropDown:

                        callbackResults.results = (dropdownComponentConfig.value) ? $"Action Name : {dropdownComponentConfig.name} - Input Type Is Set To {inputType} Successfully" : $"Action Name : {dropdownComponentConfig.name} - Input Type Is Set To {inputType} But Value Is Missing.";
                        callbackResults.data = dropdownComponentConfig.dataPackets as T;
                        callbackResults.resultsCode = (dropdownComponentConfig.value) ? AppData.Helpers.SuccessCode : AppData.Helpers.ErrorCode;

                        break;

                    case AppData.InputType.Text:

                        callbackResults.results = (textComponentConfig.value) ? $"Text Displayer Name : {textComponentConfig.name} - Text Displayer Type Is Set To {inputType} Successfully" : $"Text Displayer Name : {textComponentConfig.name} - Displayer Type Is Set To {inputType} But Value Is Missing.";
                        callbackResults.data = textComponentConfig.dataPackets as T;
                        callbackResults.resultsCode = (textComponentConfig.value) ? AppData.Helpers.SuccessCode : AppData.Helpers.ErrorCode;

                        break;

                    case AppData.InputType.Image:

                        callbackResults.results = (imageComponentConfig.value) ? $"Image Displayer Name : {imageComponentConfig.name} - Image Displayer Type Is Set To {inputType} Successfully" : $"Image Displayer Name : {imageComponentConfig.name} - Displayer Type Is Set To {inputType} But Value Is Missing.";
                        callbackResults.data = imageComponentConfig.dataPackets as T;
                        callbackResults.resultsCode = (imageComponentConfig.value) ? AppData.Helpers.SuccessCode : AppData.Helpers.ErrorCode;

                        break;
                }
            }
            else
            {
                callbackResults.results = "Input Type Is Set To None";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

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
