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

        #region Unity Callbacks

        void Start() => Init();

        #endregion

        #region Main

        new void Init()
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

            if (DatabaseManager.Instance != null)
            {
                if (DatabaseManager.Instance.GetCurrentSceneAsset() != null)
                {
                    DatabaseManager.Instance.GetCurrentSceneAsset().assetImportRotation = propertiesVector;
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
