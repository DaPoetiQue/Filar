using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SceneAssetTransformPropertiesWidget : AppData.Widget
    {
        #region Components

        [Space(5)]
        [SerializeField]
        AppData.UIInputField<AppData.InputFieldConfigDataPacket> xRotationInputField,
                                                  yRotationInputField,
                                                  zRotationInputField;

        Vector3 propertiesVector = Vector3.zero;

        #endregion

        #region Main

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType>>> callback)
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType>>();

            Init(initializationCallbackResults =>
            {
                callbackResults.SetResultsData(initializationCallbackResults);
            });

            callback.Invoke(callbackResults);
        }

        protected override AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType>> OnGetState()
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType>>(AppData.Helpers.GetAppComponentValid(GetStatePacket(), $"{GetName()} - State Object", "Widget State Object Is Null / Not Yet Initialized In The Base Class."));

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

        void ReferenceAndDelete()
        {
            //if (callbackResults.Success())
            //{
            //    OnRegisterWidget(this, onRegisterWidgetCallbackResults =>
            //    {
            //        callbackResults.SetResult(GetType());

            //        if (callbackResults.Success())
            //        {
            //            if (xRotationInputField.value)
            //                xRotationInputField.value.onValueChanged.AddListener((value) => OnUIInputFieldActionValueChanged(value, xRotationInputField.dataPackets.action));
            //            else
            //                Debug.LogWarning("--> X Rotation Input Field Value Is Null.");

            //            if (yRotationInputField.value)
            //                yRotationInputField.value.onValueChanged.AddListener((value) => OnUIInputFieldActionValueChanged(value, yRotationInputField.dataPackets.action));
            //            else
            //                Debug.LogWarning("--> Y Rotation Input Field Value Is Null.");

            //            if (zRotationInputField.value)
            //                zRotationInputField.value.onValueChanged.AddListener((value) => OnUIInputFieldActionValueChanged(value, zRotationInputField.dataPackets.action));
            //            else
            //                Debug.LogWarning("--> Z Rotation Input Field Value Is Null.");

            //            var widgetStatePacket = new AppData.WidgetStatePacket(name: GetName(), type: GetType().data, stateType: AppData.WidgetStateType.Initialized, value: this);

            //            callbackResults.result = $"Widget : {GetName()} Of Type : {GetType().data}'s State Packet Has Been Initialized Successfully.";
            //            callbackResults.data = widgetStatePacket;
            //        }
            //        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            //    });
            //}
            //else
            //    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

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

        protected override void OnScreenWidget(Action<AppData.Callback> callback = null)
        {

        }

        protected override void OnScreenWidget(AppData.SceneConfigDataPacket configDataPacket, Action<AppData.Callback> callback = null)
        {

        }

        protected override void OnHideScreenWidget(Action<AppData.Callback> callback = null)
        {
            HideSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
        }

        protected override void OnInputFieldValueChanged(string value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInputFieldValueChanged(int value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScrollerValueChanged(Vector2 value)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionButtonEvent(AppData.WidgetType popUpType, AppData.InputActionButtonType actionType, AppData.SceneConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void ScrollerPosition(Vector2 position)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScreenWidget<T>(AppData.ScriptableConfigDataPacket<T> scriptableConfigData, Action<AppData.Callback> callback = null)
        {
            throw new NotImplementedException();
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

        protected override void OnActionButtonInputs(AppData.UIButton<AppData.ButtonConfigDataPacket> actionButton)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
