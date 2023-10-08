using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Com.RedicalGames.Filar
{
    public class SceneAssetExportWidget : AppData.Widget
    {
        #region Components

        [Space(5)]
        [SerializeField]
        Image thumbnailDisplayer;

        [Space(5)]
        [SerializeField]
        AppData.UIInputField<AppData.SceneDataPackets> exportedAssetNameInputField = new AppData.UIInputField<AppData.SceneDataPackets>();

        [Space(5)]
        [SerializeField]
        AppData.UIDropDown<AppData.DropdownDataPackets> exportExtensionDropDown = new AppData.UIDropDown<AppData.DropdownDataPackets>();

        AppData.AssetExportData assetExportData = new AppData.AssetExportData();

        #endregion

        #region Main

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket>> callback)
        {
            AppData.CallbackData<AppData.WidgetStatePacket> callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket>();

            Init(initializationCallbackResults =>
            {
                callbackResults.SetResultsData(initializationCallbackResults);
            });

            callback.Invoke(callbackResults);
        }

        void InitializeDropDownContent()
        {
            if (exportExtensionDropDown.value != null)
            {
                if (AppDatabaseManager.Instance != null)
                {
                    var content = AppDatabaseManager.Instance.GetDropdownContent<AppData.ExportExtensionType>();

                    if (content.data != null)
                    {
                        exportExtensionDropDown.value.ClearOptions();

                        List<TMP_Dropdown.OptionData> dropdownOption = new List<TMP_Dropdown.OptionData>();

                        foreach (var extension in content.data)
                            dropdownOption.Add(new TMP_Dropdown.OptionData() { text = extension });

                        exportExtensionDropDown.value.AddOptions(dropdownOption);

                        exportExtensionDropDown.value.onValueChanged.AddListener((value) => OnDropDownExtensionsOptions(value));
                    }
                    else
                        Debug.LogWarning("--> SelectedSceneAssetPreviewWidget : Export Extension Drop Down Extensions List Not Found In Scene Assets Manager.");
                }
                else
                    Debug.LogWarning("--> SelectedSceneAssetPreviewWidget : Scene Assets Manager Instance Not Yet Initialized.");
            }
            else
            {
                Debug.LogWarning("--> SelectedSceneAssetPreviewWidget : Export Extension Drop Down Value Missing.");
                return;
            }
        }

        void OnDropDownExtensionsOptions(int dropdownIndex)
        {
            assetExportData.exportExtension = (AppData.ExportExtensionType)dropdownIndex;

            if (AppDatabaseManager.Instance)
                AppDatabaseManager.Instance.SetCurrentAssetExportData(assetExportData);
            else
                Debug.LogWarning("--> RG_Unity - OnDropDownExtensionsOptions Failed : Scene Assets Manager Instance Not Yet Initialized.");
        }

        void OnInputAssetNameFieldValueChangeEvent(string value)
        {
            assetExportData.name = value;

            if (AppDatabaseManager.Instance)
            {
                AppDatabaseManager.Instance.SetCurrentAssetExportData(assetExportData);
            }
            else
                Debug.LogWarning("--> RG_Unity - OnDropDownExtensionsOptions Failed : Scene Assets Manager Instance Not Yet Initialized.");
        }

        protected override void OnScreenWidget()
        {

        }
        void SetWidgetAssetData(AppData.SceneAsset asset)
        {
            AppData.Helpers.ShowImage(asset, thumbnailDisplayer);

            if (titleDisplayer != null && !string.IsNullOrEmpty(asset.name))
                titleDisplayer.text = asset.name;
        }

        protected override void OnShowScreenWidget(AppData.SceneDataPackets dataPackets)
        {
            ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);

            //switch (transitionType)
            //{
            //    case AppData.TransitionType.Default:

            //        if (GetLayoutView(defaultLayoutType).layout)
            //        {
            //            if (exportedAssetNameInputField.value != null)
            //            {
            //                exportedAssetNameInputField.value.text = AppDatabaseManager.Instance.GetCurrentSceneAsset().name;
            //            }
            //            else
            //                Debug.LogWarning("--> OnShowScreenWidget Failed : Exported Asset Name Input Field Value Is Missing / Null.");

            //            if (AppDatabaseManager.Instance)
            //            {
            //                if (AppDatabaseManager.Instance.GetCurrentSceneAsset().sceneObject.value != null)
            //                {
            //                    assetExportData.name = AppDatabaseManager.Instance.GetCurrentSceneAsset().name;
            //                    assetExportData.value = AppDatabaseManager.Instance.GetCurrentSceneAsset().sceneObject.value;
            //                }
            //                else
            //                    Debug.LogWarning("--> RG_Unity - OnShowScreenWidget Failed : Scene Assets Manager Instance's Get Current Scene Asset Scene Object Value Is Missing / Null");

            //                AppDatabaseManager.Instance.SetCurrentAssetExportData(assetExportData);
            //            }
            //            else
            //                Debug.LogWarning("--> RG_Unity - OnDropDownExtensionsOptions Failed : Scene Assets Manager Instance Not Yet Initialized.");
            //        }
            //        else
            //            Debug.LogWarning("--> Pop Up Value Required.");

            //        break;

            //    case AppData.TransitionType.Translate:

            //        break;
            //}

            //if (AppDatabaseManager.Instance)
            //    SetWidgetAssetData(AppDatabaseManager.Instance.GetCurrentSceneAsset());
            //else
            //    Debug.LogWarning("--> Scene Assets Manager Not Yet Initialized.");
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

        protected override AppData.CallbackData<AppData.WidgetStatePacket> OnGetState()
        {
            AppData.CallbackData<AppData.WidgetStatePacket> callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket>(AppData.Helpers.GetAppComponentValid(GetStatePacket(), $"{GetName()} - State Object", "Widget State Object Is Null / Not Yet Initialized In The Base Class."));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(GetType());

                if (callbackResults.Success())
                {
                    var widgetType = GetType().data;

                    callbackResults.SetResult(GetStatePacket().Initialized(widgetType));

                    if (callbackResults.Success())
                    {
                        callbackResults.result = $"Widget : {GetStatePacket().GetName()} Of Type : {GetStatePacket().GetType()} State Is Set To : {GetStatePacket().GetStateType()}";
                        callbackResults.data = GetStatePacket();
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        #endregion
    }
}