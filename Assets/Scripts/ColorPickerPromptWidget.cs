using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.RedicalGames.Filar
{
    public class ColorPickerPromptWidget : AppData.SettingsWidget
    {
        #region Components

        [SerializeField]
        Image colorResultsDisplayer = null;

        [SerializeField]
        AppData.ColorInfo colorInfo = new AppData.ColorInfo();

        List<string> randomizableColorList = new List<string>();

        int randomValue = 0;
        int previousRandomValue = 9;

        #endregion

        #region Main

        void RandomizeColors()
        {
            if (randomizableColorList.Count > 0)
            {
                do
                {
                    randomValue = Random.Range(0, randomizableColorList.Count - 1);
                }
                while (randomValue == previousRandomValue);

                if (randomValue != previousRandomValue)
                    previousRandomValue = randomValue;

                string randomColor = randomizableColorList[randomValue];

                SetInputFieldValue(AppData.InputFieldActionType.ColorPromptField, randomColor, (colorNameFieldCallbackResults) =>
                {
                    if (!colorNameFieldCallbackResults.Success())
                        Log(colorNameFieldCallbackResults.resultCode, colorNameFieldCallbackResults.result, this);
                });
            }
            else
                Debug.LogWarning("--> RandomizeColors Failed : randomizableColorList Is Null / Empty.");
        }

        void OnResetColorInfo()
        {
            colorInfo.color = Color.clear;
            colorResultsDisplayer.color = colorInfo.color;
        }

        #endregion

        #region Overrides

        protected override void Init()
        {
            if (randomizableColorList.Count <= 0)
                randomizableColorList = new List<string>
        {
            "Clear",
            "White",
            "Red",
            "Green",
            "Blue",
            "Yellow",
            "Purple",
            "Cyan",
            "Magenta",
            "Black"
        };

            OnResetColorInfo();
        }

        protected override void OnActionButtonClickedEvent(AppData.ButtonDataPackets dataPackets)
        {
            switch (dataPackets.action)
            {
                case AppData.InputActionButtonType.CloseButton:

                    CloseWidget(this);

                    break;

                case AppData.InputActionButtonType.ColorPickerButton:

                    if (SceneAssetsManager.Instance != null)
                    {
                        SceneAssetsManager.Instance.GetHexidecimalFromColor(colorInfo.color, (newColorCallbackResults) =>
                        {
                            if (AppData.Helpers.IsSuccessCode(newColorCallbackResults.resultCode))
                            {
                                AppData.ActionEvents.OnSwatchColorPickedEvent(newColorCallbackResults.data, true, false);
                                CloseWidget(this);
                            }
                            else
                                Debug.LogWarning($"--> OnActionButtonClickedEvent's GetHexidecimalFromColor Failed With Results : {newColorCallbackResults.result}");
                        });
                    }
                    else
                        Debug.LogWarning("--> OnActionButtonClickedEvent Failed : SceneAssetsManager.Instance Is Not Yet Initialized.");

                    break;

                case AppData.InputActionButtonType.Randomize:

                    RandomizeColors();

                    break;

                case AppData.InputActionButtonType.VoiceInputButton:

                    AppData.ActionEvents.OnActionButtonClicked(AppData.InputActionButtonType.VoiceInputButton);

                    break;
            }
        }

        protected override void OnActionCheckboxValueChangedEvent(bool value, AppData.CheckboxDataPackets dataPackets)
        {

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
            if (!string.IsNullOrEmpty(value))
            {
                if (colorResultsDisplayer != null)
                {
                    AppData.Helpers.ConvertNameToColor(value, (converColorCallbackResults) =>
                    {
                        if (AppData.Helpers.IsSuccessCode(converColorCallbackResults.resultCode))
                        {
                            colorResultsDisplayer.color = converColorCallbackResults.data;
                            SetColorPickerButtonState(AppData.InputUIState.Enabled);
                        }
                        else
                        {
                            colorResultsDisplayer.color = converColorCallbackResults.data;
                            SetColorPickerButtonState(AppData.InputUIState.Disabled);
                        }

                        colorInfo.color = converColorCallbackResults.data;
                    });
                }
                else
                    Debug.LogWarning($"--> OnActionInputFieldValueChangedEvent Failed : Color : {value} Not Found");
            }
            else
            {
                OnResetColorInfo();
                SetColorPickerButtonState(AppData.InputUIState.Disabled);
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

        protected override void OnResetWidgetData(AppData.SettingsWidgetType widgetType)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnWidgetClosed()
        {
            OnActionInputFieldInitialized((inputFieldInitializedCallbackResults) =>
            {
                if (inputFieldInitializedCallbackResults.Success())
                {
                    SetInputFieldValue(AppData.InputFieldActionType.ColorPromptField, string.Empty, (fieldSetCallbackResults) =>
                    {
                        if (fieldSetCallbackResults.Success())
                            OnResetColorInfo();
                        else
                            Log(fieldSetCallbackResults.resultCode, fieldSetCallbackResults.result, this);
                    });
                }
                else
                    Log(inputFieldInitializedCallbackResults.resultCode, inputFieldInitializedCallbackResults.result, this);
            });
        }

        protected override void OnWidgetOpened() => SetColorPickerButtonState(AppData.InputUIState.Disabled);

        void SetColorPickerButtonState(AppData.InputUIState state)
        {
            if (GetActive())
            {
                OnActionButtonInitialized((buttonsInitializedCallbackResults) =>
                {
                    if (AppData.Helpers.IsSuccessCode(buttonsInitializedCallbackResults.resultCode))
                        SetActionButtonState(AppData.InputActionButtonType.ColorPickerButton, state);
                    else
                        Debug.LogWarning($"--> OnWidgetOpened's OnActionButtonInitialized Failed With Results : {buttonsInitializedCallbackResults.result}");
                });
            }
        }

        protected override void RegisterEventListensers(bool register)
        {
            if (GetActive())
            {
                if (register)
                    AppData.ActionEvents._OnVoiceCommandResultsEvent += ActionEvents__OnVoiceCommandResultsEvent;
                else
                    AppData.ActionEvents._OnVoiceCommandResultsEvent -= ActionEvents__OnVoiceCommandResultsEvent;
            }
            else
                AppData.ActionEvents._OnVoiceCommandResultsEvent -= ActionEvents__OnVoiceCommandResultsEvent;
        }

        private void ActionEvents__OnVoiceCommandResultsEvent(string[] commands)
        {
            List<string> executableCommandsList = new List<string>();

            foreach (var command in commands)
            {

                string prefix = command[0].ToString();
                string formattedCommand = prefix.ToUpper() + command.Substring(1);

                if (randomizableColorList.Contains(formattedCommand))
                    executableCommandsList.Add(formattedCommand);

                Debug.LogError($"==> RG_Unity - Commands : {formattedCommand}");
            }

            if (executableCommandsList.Count > 0)
            {
                if (executableCommandsList.Count == 1)
                {
                    SetInputFieldValue(AppData.InputFieldActionType.ColorPromptField, executableCommandsList[0], (colorNameFieldCallbackResults) =>
                    {
                        if (!colorNameFieldCallbackResults.Success())
                            Log(colorNameFieldCallbackResults.resultCode, colorNameFieldCallbackResults.result, this);
                    });
                }

                if (executableCommandsList.Count == 2)
                {
                    Color color = AppData.Helpers.BlendColors(executableCommandsList[0], executableCommandsList[1], 0.5f);
                    colorResultsDisplayer.color = color;
                }

                if (executableCommandsList.Count > 2)
                {
                    Color color = AppData.Helpers.BlendColors(executableCommandsList);
                    colorResultsDisplayer.color = color;
                }
            }
            else
                Debug.LogWarning("--> RG_Unity - ActionEvents__OnVoiceCommandResultsEvent Failed : No Executable Commands Found.");
        }

        protected override void OnActionDropdownValueChangedEvent(int value, List<string> contentList, AppData.DropdownDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
