using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class InputActionHandler : AppData.UIScreenInputComponent
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

        public AppData.UIButton<AppData.ButtonConfigDataPacket> buttonComponentConfig;
        public AppData.UIInputField<AppData.InputFieldConfigDataPacket> inputFieldComponentConfig;
        public AppData.UIInputSlider<AppData.InputSliderConfigDataPacket> inputSliderComponentConfig;
        public AppData.UISlider<AppData.SliderConfigDataPacket> sliderComponentConfig;
        public AppData.UICheckbox<AppData.CheckboxConfigDataPacket> checkboxComponentConfig;
        public AppData.UIDropDown<AppData.DropdownConfigDataPacket> dropdownComponentConfig;

        #endregion

        #region Displayers Config

        public AppData.UITextDisplayer<AppData.TextConfigDataPacket> textComponentConfig;
        public AppData.UIImageDisplayer<AppData.ImageConfigDataPacket> imageComponentConfig;

        #endregion

        #region Transitions

        public List<AppData.TransitionableUIMountComponent<AppData.UIMountType>> transitionableUIMounts = new List<AppData.TransitionableUIMountComponent<AppData.UIMountType>>();

        public bool CanTransition { get; private set; }

        #endregion

        #endregion

        #endregion

        #region Main

        public void Init<T>(Action<AppData.CallbackData<T>> callback = null) where T: AppData.SceneConfigDataPacket
        {
            AppData.CallbackData<T> callbackResults = new AppData.CallbackData<T>();

            switch(inputType)
            {
                case AppData.InputType.Button:

                    GetInputDataPacket<AppData.ButtonConfigDataPacket>(inputCallbackResults =>
                    {
                        callbackResults.data = inputCallbackResults.data as T;
                        callbackResults.result = inputCallbackResults.result;
                        callbackResults.resultCode = inputCallbackResults.resultCode;
                    });

                    break;

                case AppData.InputType.InputField:

                    GetInputDataPacket<AppData.InputFieldConfigDataPacket>(inputCallbackResults =>
                    {
                        callbackResults.data = inputCallbackResults.data as T;
                        callbackResults.result = inputCallbackResults.result;
                        callbackResults.resultCode = inputCallbackResults.resultCode;
                    });

                    break;

                case AppData.InputType.InputSlider:

                    GetInputDataPacket<AppData.InputSliderConfigDataPacket>(inputCallbackResults =>
                    {
                        callbackResults.data = inputCallbackResults.data as T;
                        callbackResults.result = inputCallbackResults.result;
                        callbackResults.resultCode = inputCallbackResults.resultCode;
                    });

                    break;

                case AppData.InputType.Slider:

                    GetInputDataPacket<AppData.SliderConfigDataPacket>(inputCallbackResults =>
                    {
                        callbackResults.data = inputCallbackResults.data as T;
                        callbackResults.result = inputCallbackResults.result;
                        callbackResults.resultCode = inputCallbackResults.resultCode;
                    });

                    break;

                case AppData.InputType.DropDown:

                    GetInputDataPacket<AppData.DropdownConfigDataPacket>(inputCallbackResults =>
                    {
                        callbackResults.data = inputCallbackResults.data as T;
                        callbackResults.result = inputCallbackResults.result;
                        callbackResults.resultCode = inputCallbackResults.resultCode;
                    });

                    break;

                case AppData.InputType.Checkbox:

                    GetInputDataPacket<AppData.CheckboxConfigDataPacket>(inputCallbackResults =>
                    {
                        callbackResults.data = inputCallbackResults.data as T;
                        callbackResults.result = inputCallbackResults.result;
                        callbackResults.resultCode = inputCallbackResults.resultCode;
                    });

                    break;

                case AppData.InputType.Text:

                    GetInputDataPacket<AppData.TextConfigDataPacket>(inputCallbackResults =>
                    {
                        callbackResults.data = inputCallbackResults.data as T;
                        callbackResults.result = inputCallbackResults.result;
                        callbackResults.resultCode = inputCallbackResults.resultCode;
                    });

                    break;

                case AppData.InputType.Image:

                    GetInputDataPacket<AppData.ImageConfigDataPacket>(inputCallbackResults =>
                    {
                        callbackResults.data = inputCallbackResults.data as T;
                        callbackResults.result = inputCallbackResults.result;
                        callbackResults.resultCode = inputCallbackResults.resultCode;
                    });

                    break;
            }

            callback?.Invoke(callbackResults);
        }

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

        public void GetInputDataPacket<T>(Action<AppData.CallbackData<T>> callback) where T : AppData.SceneConfigDataPacket
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
            return buttonComponentConfig != null && buttonComponentConfig.value != null && buttonComponentConfig.dataPackets.GetAction().GetData() == action;
        }

        public bool HasComponent(AppData.InputFieldActionType action)
        {
            return inputFieldComponentConfig != null && inputFieldComponentConfig.value != null && inputFieldComponentConfig.dataPackets.GetAction().GetData() == action;
        }

        public bool HasComponent(AppData.InputSliderActionType action)
        {
            return inputSliderComponentConfig != null && inputSliderComponentConfig.value != null && inputSliderComponentConfig.dataPackets.GetAction().GetData() == action;
        }

        public bool HasComponent(AppData.SliderValueType value)
        {
            return sliderComponentConfig != null && sliderComponentConfig.value != null && sliderComponentConfig.dataPackets.GetValueType().GetData() == value;
        }

        public bool HasComponent(AppData.CheckboxInputActionType action)
        {
            return checkboxComponentConfig != null && checkboxComponentConfig.value != null && checkboxComponentConfig.dataPackets.GetAction().GetData() == action;
        }

        public bool HasComponent(AppData.InputDropDownActionType action)
        {
            return dropdownComponentConfig != null && dropdownComponentConfig.value != null && dropdownComponentConfig.dataPackets.GetAction().GetData() == action;
        }

        #endregion

        #region Displayers

        public bool HasComponent(AppData.ScreenTextType type)
        {
            return textComponentConfig != null && textComponentConfig.value != null && textComponentConfig.dataPackets.GetTextType().GetData() == type;
        }

        public bool HasComponent(AppData.ScreenImageType type)
        {
            return imageComponentConfig != null && imageComponentConfig.value != null && imageComponentConfig.dataPackets.GetImageType().GetData() == type;
        }

        #endregion

        #endregion

        #region Get Action Components

        public AppData.CallbackData<AppData.UIButton<AppData.ButtonConfigDataPacket>> GetButtonComponent()
        {
            var callbackResults = new AppData.CallbackData<AppData.UIButton<AppData.ButtonConfigDataPacket>>(AppData.Helpers.GetAppComponentValid(buttonComponentConfig, "Button Component Config",
            $"Button Component Config Not found For : {GetName()} - Invalid Operation.",
            $"Button Component Has Been Successfully Found For : {GetName()}"));

            if (callbackResults.Success())
                callbackResults.data = buttonComponentConfig;
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.CallbackData<AppData.UIInputField<AppData.InputFieldConfigDataPacket>> GetInputFieldComponent()
        {
            var callbackResults = new AppData.CallbackData<AppData.UIInputField<AppData.InputFieldConfigDataPacket>>(AppData.Helpers.GetAppComponentValid(inputFieldComponentConfig, "Input Field Component Config",
            $"Input Field Component Config Not found For : {GetName()} - Invalid Operation.",
            $"Input Field Component Has Been Successfully Found For : {GetName()}"));

            if (callbackResults.Success())
                callbackResults.data = inputFieldComponentConfig;
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.CallbackData<AppData.UIInputSlider<AppData.InputSliderConfigDataPacket>> GetInputSliderComponent()
        {
            var callbackResults = new AppData.CallbackData<AppData.UIInputSlider<AppData.InputSliderConfigDataPacket>>(AppData.Helpers.GetAppComponentValid(inputSliderComponentConfig, "Input Slider Component Config",
            $"Input Slider Component Config Not found For : {GetName()} - Invalid Operation.",
            $"Input Slider Component Has Been Successfully Found For : {GetName()}"));

            if (callbackResults.Success())
                callbackResults.data = inputSliderComponentConfig;
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.CallbackData<AppData.UISlider<AppData.SliderConfigDataPacket>> GetSliderComponent()
        {
            var callbackResults = new AppData.CallbackData<AppData.UISlider<AppData.SliderConfigDataPacket>>(AppData.Helpers.GetAppComponentValid(sliderComponentConfig, "Slider Component Config",
            $"Slider Component Config Not found For : {GetName()} - Invalid Operation.",
            $"Slider Component Has Been Successfully Found For : {GetName()}"));

            if (callbackResults.Success())
                callbackResults.data = sliderComponentConfig;
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.CallbackData<AppData.UICheckbox<AppData.CheckboxConfigDataPacket>> GetCheckboxComponent()
        {
            var callbackResults = new AppData.CallbackData<AppData.UICheckbox<AppData.CheckboxConfigDataPacket>>(AppData.Helpers.GetAppComponentValid(checkboxComponentConfig, "Checkbox Component Config",
            $"Checkbox Component Config Not found For : {GetName()} - Invalid Operation.",
            $"Checkbox Component Has Been Successfully Found For : {GetName()}"));

            if (callbackResults.Success())
                callbackResults.data = checkboxComponentConfig;
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.CallbackData<AppData.UIDropDown<AppData.DropdownConfigDataPacket>> GetDropdownComponent()
        {
            var callbackResults = new AppData.CallbackData<AppData.UIDropDown<AppData.DropdownConfigDataPacket>>(AppData.Helpers.GetAppComponentValid(dropdownComponentConfig, "Dropdown Component Config",
             $"Dropdown Component Config Not found For : {GetName()} - Invalid Operation.",
             $"Dropdown Component Has Been Successfully Found For : {GetName()}"));

            if (callbackResults.Success())
                callbackResults.data = dropdownComponentConfig;
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        #endregion

        #region Get Displayer Components

        public AppData.CallbackData<AppData.UITextDisplayer<AppData.TextConfigDataPacket>> GetTextComponent()
        {
            var callbackResults = new AppData.CallbackData<AppData.UITextDisplayer<AppData.TextConfigDataPacket>>(AppData.Helpers.GetAppComponentValid(textComponentConfig, "Text Component Config",
              $"Text Component Config Not found For : {GetName()} - Invalid Operation.",
              $"Text Component Has Been Successfully Found For : {GetName()}"));

            if (callbackResults.Success())
                callbackResults.data = textComponentConfig;
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.CallbackData<AppData.UIImageDisplayer<AppData.ImageConfigDataPacket>> GetImageComponent()
        {
            var callbackResults = new AppData.CallbackData<AppData.UIImageDisplayer<AppData.ImageConfigDataPacket>>(AppData.Helpers.GetAppComponentValid(imageComponentConfig, "Image Component Config", 
                $"Image Component tConfig Not found For : {GetName()} - Invalid Operation.",
                $"Image Component Has Been Successfully Found For : {GetName()}"));

            if (callbackResults.Success())
                callbackResults.data = imageComponentConfig;
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        #endregion


        #region Transitionable UI Mounts

        public AppData.CallbackDataList<AppData.TransitionableUIMountComponent<AppData.UIMountType>> GetTransitionableUIMounts()
        {
            var callbackResults = new AppData.CallbackDataList<AppData.TransitionableUIMountComponent<AppData.UIMountType>>();

            callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(transitionableUIMounts, "Transitionable UI Mounts", $"Get Transitionable UI Mounts Failed - There Are No Transitionable UI Mounts Initialized For : {GetName()}"));

            if (callbackResults.Success())
                callbackResults.data = transitionableUIMounts;
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        #endregion

        #endregion
    }
}
