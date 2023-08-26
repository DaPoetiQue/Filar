using UnityEngine;
using UnityEngine.UI;

namespace Com.RedicalGames.Filar
{
    public class SliderValuePopUpWidget : AppData.Widget
    {
        #region Components

        [Space(5)]
        [SerializeField]
        public Slider slider;

        [Space(5)]
        public float defaultFieldValue = 0.1f;

        [SerializeField]
        bool updateSliderValue;

        #endregion

        #region Unity Callbacks

        void Start() => Init();

        #endregion

        #region Main

        new void Init()
        {
            if (slider != null)
                slider.onValueChanged.AddListener((value) => SetValue(value));
            else
            {
                Debug.LogWarning("--> Slider Required.");
                return;
            }

            sliderWidget = this;

            base.Init();
        }


        public void SetValue(float value)
        {
            Debug.Log($"---> Set Current Slider Value To : {value}");

            if (updateSliderValue)
                return;

            RenderingSettingsManager.Instance.SetMaterialValue(dataPackets.assetFieldConfiguration, value);
        }

        public void SetSliderValue(float value, AppData.SliderValueType sliderType)
        {
            if (slider != null)
            {
                updateSliderValue = true;

                switch (sliderType)
                {
                    case AppData.SliderValueType.MaterialGlossinessValue:

                        slider.value = value;

                        break;

                    case AppData.SliderValueType.MaterialBumpScaleValue:

                        slider.value = value;

                        break;

                    case AppData.SliderValueType.MaterialOcclusionIntensityValue:

                        slider.value = value;

                        break;
                }

                updateSliderValue = false;
            }
            else
            {
                Debug.LogWarning("--> Slider Required.");
                return;
            }
        }

        protected override void OnScreenWidget()
        {

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
            throw new System.NotImplementedException();
        }

        protected override void OnInputFieldValueChanged(int value, AppData.InputFieldDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnSubscribeToActionEvents(bool subscribe)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScrollerValueChanged(Vector2 value)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionButtonEvent(AppData.WidgetType popUpType, AppData.InputActionButtonType actionType, AppData.SceneDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void ScrollerPosition(Vector2 position)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
