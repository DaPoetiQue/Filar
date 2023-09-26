using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SceneAssetTransformPropertiesWidget : AppData.Widget
    {
        #region Components

        [Space(5)]
        [SerializeField]
        AppData.UIInputField<AppData.InputFieldDataPackets> xRotationInputField,
                                                  yRotationInputField,
                                                  zRotationInputField;

        Vector3 propertiesVector = Vector3.zero;

        #endregion

        #region Main

        protected override void Initialize(Action<AppData.CallbackData<AppData.WidgetStatePacket>> callback)
        {
            AppData.CallbackData<AppData.WidgetStatePacket> callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket>();

            callbackResults.SetResult(GetType());

            if (callbackResults.Success())
            {
                OnRegisterWidget(this, onRegisterWidgetCallbackResults =>
                {
                    callbackResults.SetResult(GetType());

                    if (callbackResults.Success())
                    {
                        if (xRotationInputField.value)
                            xRotationInputField.value.onValueChanged.AddListener((value) => OnUIInputFieldActionValueChanged(value, xRotationInputField.dataPackets.action));
                        else
                            Debug.LogWarning("--> X Rotation Input Field Value Is Null.");

                        if (yRotationInputField.value)
                            yRotationInputField.value.onValueChanged.AddListener((value) => OnUIInputFieldActionValueChanged(value, yRotationInputField.dataPackets.action));
                        else
                            Debug.LogWarning("--> Y Rotation Input Field Value Is Null.");

                        if (zRotationInputField.value)
                            zRotationInputField.value.onValueChanged.AddListener((value) => OnUIInputFieldActionValueChanged(value, zRotationInputField.dataPackets.action));
                        else
                            Debug.LogWarning("--> Z Rotation Input Field Value Is Null.");

                        var widgetStatePacket = new AppData.WidgetStatePacket(name: GetName(), type: GetType().data, stateType: AppData.WidgetStateType.Initialized, value: this);

                        callbackResults.result = $"Widget : {GetName()} Of Type : {GetType().data}'s State Packet Has Been Initialized Successfully.";
                        callbackResults.data = widgetStatePacket;
                    }
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                });
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback.Invoke(callbackResults);
        }

        void OnUIInputFieldActionValueChanged(string value, AppData.InputFieldActionType inputFiledAction)
        {
            float valueResults = float.Parse(value);

            switch (inputFiledAction)
            {
                case AppData.InputFieldActionType.XRotationPropertyField:

                    propertiesVector.x = valueResults;

                    break;


                case AppData.InputFieldActionType.YRotationPropertyField:

                    propertiesVector.y = valueResults;

                    break;

                case AppData.InputFieldActionType.ZRotationPropertyField:

                    propertiesVector.z = valueResults;

                    break;
            }

            if (AppDatabaseManager.Instance != null)
            {
                if (AppDatabaseManager.Instance.GetCurrentSceneAsset() != null)
                {
                    AppDatabaseManager.Instance.GetCurrentSceneAsset().assetImportRotation = propertiesVector;
                }
                else
                    Debug.LogWarning("--> On UI Input Field Action Value Changed - Scene Assets Manager Instance's Get Current Scene Asset Is Null / Not Found.");
            }
            else
                Debug.LogWarning("--> On UI Input Field Action Value Changed - Scene Assets Manager Instance Is Not Yet Initialized.");

            Quaternion propertiesQuaternion = Quaternion.Euler(propertiesVector.x, propertiesVector.y, propertiesVector.z);

            AppData.ActionEvents.OnUpdateSceneAssetDefaultRotation(propertiesQuaternion);
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
