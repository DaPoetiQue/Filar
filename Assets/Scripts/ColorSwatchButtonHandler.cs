using System;
using UnityEngine;
using UnityEngine.UI;

namespace Com.RedicalGames.Filar
{
    public class ColorSwatchButtonHandler : AppData.UIScreenWidget
    {
        #region Components

        [Space(5)]
        [SerializeField]
        Image colorSwatchIcon;

        AppData.ColorInfo colorInfo;

        [SerializeField]
        string swatchID;

        #endregion

        #region Components

        public void Initialize(AppData.ColorInfo colorInfo, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            this.colorInfo = colorInfo;

            if (colorSwatchIcon != null)
            {
                Init((callback) =>
                {
                    callbackResults = callback;

                    if (AppData.Helpers.IsSuccessCode(callback.resultsCode))
                    {
                        InitializeColorProperties(colorInfo, (initializationCallback) =>
                        {
                            if (AppData.Helpers.IsSuccessCode(initializationCallback.resultsCode))
                                Debug.Log($"---> Initialize Success With Results :{initializationCallback.results}");
                            else
                                Debug.LogError($"---> Initialize Failed With Results :{initializationCallback.results}");
                        });
                    }
                });
            }
            else
            {
                callbackResults.results = "ColorSwatchButtonHandler Initialization Failed : Color Swatch Icon Missing / Null.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        protected override void OnActionButtonInputs(AppData.UIButton<AppData.ButtonDataPackets> actionButton)
        {
            if (actionButton.dataPackets.action == AppData.InputActionButtonType.ColorPickerButton)
            {
                AppData.ActionEvents.OnSwatchColorPickedEvent(colorInfo, true, false);

                if (SceneAssetsManager.Instance != null)
                    SceneAssetsManager.Instance.GetColorSwatchData((swatchDataResults) =>
                    {
                        if (AppData.Helpers.IsSuccessCode(swatchDataResults.resultsCode))
                            swatchDataResults.data.OnSwatchColorSelection(colorInfo);
                        else
                            Debug.LogError($"--> ColorSwatchButtonHandler OnActionButtonInputs Failed With Results : {swatchDataResults.results}");
                    });
                else
                    Debug.LogWarning("--> OnActionButtonInputs's GetColorSwatchData Failed : SceneAssetsManager.Instance Is Not Yet initialized.");
            }
        }

        void InitializeColorProperties(AppData.ColorInfo colorInfo, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (SceneAssetsManager.Instance != null)
            {
                //Debug.LogError($"-------> Get Hexadecimal : {colorInfo.hexadecimal}");

                SceneAssetsManager.Instance.GetColorFromHexidecimal(colorInfo.hexadecimal, (getColorCallback) =>
                {
                    callbackResults.results = getColorCallback.results;
                    callbackResults.resultsCode = getColorCallback.resultsCode;

                    if (AppData.Helpers.IsSuccessCode(getColorCallback.resultsCode))
                    {
                        colorSwatchIcon.color = getColorCallback.data.color;
                        colorInfo = getColorCallback.data;
                        SetColorInfo(colorInfo);
                    }
                });
            }
            else
            {
                callbackResults.results = "InitializeColorProperties Failed : Scene Assets Manager Instance Is Not Yet Initialized.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void SetInputUIButtonState(AppData.InputActionButtonType type, AppData.InputUIState state)
        {
            var buttonList = GetActionInputUIButtonList();

            var button = buttonList.Find((x) => x.dataPackets.action == type);

            if (button != null)
            {
                button.SetUIInputState(button, state);
            }
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


        #endregion
    }
}
