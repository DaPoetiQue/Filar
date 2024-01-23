using System;
using UnityEngine;
using UnityEngine.UI;

namespace Com.RedicalGames.Filar
{
    public class ColorSwatchButtonHandler : AppData.SelectableWidget
    {
        #region Components

        [Space(5)]
        [SerializeField]
        Image colorSwatchIcon;

        AppData.ColorInfo colorInfo;

        [SerializeField]
        string swatchID;

        #endregion

        #region Main

        public void Initialize(AppData.ColorInfo colorInfo, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            this.colorInfo = colorInfo;

            if (colorSwatchIcon != null)
            {
                Init((callback) =>
                {
                    callbackResults = callback;

                    if (AppData.Helpers.IsSuccessCode(callback.resultCode))
                    {
                        InitializeColorProperties(colorInfo, (initializationCallback) =>
                        {
                            if (AppData.Helpers.IsSuccessCode(initializationCallback.resultCode))
                                Debug.Log($"---> Initialize Success With Results :{initializationCallback.result}");
                            else
                                Debug.LogError($"---> Initialize Failed With Results :{initializationCallback.result}");
                        });
                    }
                });
            }
            else
            {
                callbackResults.result = "ColorSwatchButtonHandler Initialization Failed : Color Swatch Icon Missing / Null.";
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        protected override void OnActionButtonInputs(AppData.UIButton<AppData.ButtonConfigDataPacket> actionButton)
        {
            if (actionButton.dataPackets.GetAction().GetData() == AppData.InputActionButtonType.ColorPickerButton)
            {
                AppData.ActionEvents.OnSwatchColorPickedEvent(colorInfo, true, false);

                if (AppDatabaseManager.Instance != null)
                    AppDatabaseManager.Instance.GetColorSwatchData((swatchDataResults) =>
                    {
                        if (AppData.Helpers.IsSuccessCode(swatchDataResults.resultCode))
                            swatchDataResults.data.OnSwatchColorSelection(colorInfo);
                        else
                            Debug.LogError($"--> ColorSwatchButtonHandler OnActionButtonInputs Failed With Results : {swatchDataResults.result}");
                    });
                else
                    Debug.LogWarning("--> OnActionButtonInputs's GetColorSwatchData Failed : SceneAssetsManager.Instance Is Not Yet initialized.");
            }
        }

        void InitializeColorProperties(AppData.ColorInfo colorInfo, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (AppDatabaseManager.Instance != null)
            {
                //Debug.LogError($"-------> Get Hexadecimal : {colorInfo.hexadecimal}");

                AppDatabaseManager.Instance.GetColorFromHexidecimal(colorInfo.hexadecimal, (getColorCallback) =>
                {
                    callbackResults.result = getColorCallback.result;
                    callbackResults.resultCode = getColorCallback.resultCode;

                    if (AppData.Helpers.IsSuccessCode(getColorCallback.resultCode))
                    {
                        colorSwatchIcon.color = getColorCallback.data.color;
                        colorInfo = getColorCallback.data;
                        SetColorInfo(colorInfo);
                    }
                });
            }
            else
            {
                callbackResults.result = "InitializeColorProperties Failed : Scene Assets Manager Instance Is Not Yet Initialized.";
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void SetInputUIButtonState(AppData.InputActionButtonType type, AppData.InputUIState state)
        {
            var buttonList = GetActionInputUIButtonList();
            var button = buttonList.Find((x) => x.dataPackets.GetAction().GetData() == type);

            if (button != null)
                button.SetUIInputState(state);
        }

        public void SetColorInfo(AppData.ColorInfo info) => colorInfo = info;

        public Color GetColor()
        {
            return colorInfo.color;
        }


        public AppData.ColorInfo GetColorInfo()
        {
            return colorInfo;
        }

        public void SetSwatchID(string id) => swatchID = id;

        public string GetSwatchID()
        {
            return swatchID;
        }

        protected override void OnSetUIWidgetData(AppData.Folder folder)
        {
            throw new NotImplementedException();
        }

        public override void OnSelect(bool isInitialSelection)
        {
            throw new NotImplementedException();
        }

        public override void OnDeselect()
        {
            throw new NotImplementedException();
        }

        protected override void OnSetAssetData(AppData.SceneAsset assetData)
        {
            throw new NotImplementedException();
        }

        protected override void OnScreenUIRefreshed()
        {

        }

        protected override void OnSetUIWidgetData(AppData.ProjectStructureData structureData)
        {
            throw new NotImplementedException();
        }

        protected override void OnSetUIWidgetData(AppData.Post post)
        {
            throw new NotImplementedException();
        }

        protected override AppData.CallbackData<AppData.WidgetStatePacket<AppData.SelectableWidgetType, AppData.WidgetType, AppData.Widget>> OnGetState()
        {
            return null;
        }

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket<AppData.SelectableWidgetType, AppData.WidgetType, AppData.Widget>>> callback)
        {
            
        }

        protected override void OnScreenWidgetShownEvent()
        {
            throw new NotImplementedException();
        }

        protected override void OnScreenWidgetHiddenEvent()
        {
            throw new NotImplementedException();
        }

        protected override void OnScreenWidgetTransitionInProgressEvent()
        {
            throw new NotImplementedException();
        }

        protected override void OnActionButtonEvent(AppData.SelectableWidgetType screenWidgetType, AppData.InputActionButtonType actionType, AppData.SceneConfigDataPacket dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownConfigDataPacket dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnScrollerValueChanged(Vector2 value)
        {
            throw new NotImplementedException();
        }

        protected override void OnInputFieldValueChanged(string value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnInputFieldValueChanged(int value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxConfigDataPacket dataPackets)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
