using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Com.RedicalGames.Filar
{
    public class PreviewOrbitWidgetHandler : AppData.Selectable
    {
        #region Components

        [SerializeField]
        Camera eventCamera = null;

        [Space(5)]
        [SerializeField]
        RectTransform previewRect = null;


        [Space(5)]
        [SerializeField]
        LayerMask interactableLayer;

        [Space(5)]
        [SerializeField]
        AppData.PreviewOrbitWidgetType widgetType;

        bool onSelection;

        #endregion

        #region Main

        void OnContentOrbitUpdate()
        {

            if (Input.GetMouseButton(0))
            {
                Vector2 pos;

                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(previewRect, Input.mousePosition, eventCamera, out pos))
                {

                    float xRot = Input.GetAxis("Mouse X") * SelectableManager.Instance.GetAssetPanDragSpeedRuntimeValue(AppData.BuildType.Runtime, AppData.RuntimeExecution.PreviewWidgetOrbitSpeed).value;
                    float yRot = Input.GetAxis("Mouse Y") * SelectableManager.Instance.GetAssetPanDragSpeedRuntimeValue(AppData.BuildType.Runtime, AppData.RuntimeExecution.PreviewWidgetOrbitSpeed).value;

                    Vector3 rightVect = Vector3.Cross(eventCamera.transform.up, transform.position - eventCamera.transform.position);
                    Vector3 upVect = Vector3.Cross(transform.position - eventCamera.transform.position, rightVect);

                    if (widgetType == AppData.PreviewOrbitWidgetType.SceneAssetOrbitWidget)
                    {
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(-xRot, upVect) * transform.rotation, SelectableManager.Instance.GetAssetPanDragSpeedRuntimeValue(AppData.BuildType.Runtime, AppData.RuntimeExecution.PreviewWidgetOrbitSpeed).value * Time.smoothDeltaTime);
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(yRot, rightVect) * transform.rotation, SelectableManager.Instance.GetAssetPanDragSpeedRuntimeValue(AppData.BuildType.Runtime, AppData.RuntimeExecution.PreviewWidgetOrbitSpeed).value * Time.smoothDeltaTime);
                    }

                    if (widgetType == AppData.PreviewOrbitWidgetType.SkyboxOrbitWidget)
                    {
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(-xRot, upVect) * transform.rotation,
                            SelectableManager.Instance.GetAssetPanDragSpeedRuntimeValue(AppData.BuildType.Runtime, AppData.RuntimeExecution.PreviewWidgetOrbitSpeed).value * Time.smoothDeltaTime);

                        float rotationAngle = Mathf.Abs(transform.eulerAngles.y);

                        RenderingSettingsManager.Instance.GetRenderingSettingsData().SetSkyboxRotation(rotationAngle);
                    }
                }
            }
        }

        #region Overrides

        protected override void OnFingerDown(Finger finger)
        {
            Ray ray = eventCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, interactableLayer))
            {
                if (hitInfo.transform.GetComponent<PreviewOrbitWidgetHandler>())
                    onSelection = true;
                else
                    onSelection = false;
            }
        }

        protected override void OnFingerMoved(Finger finger)
        {
            if (onSelection)
                OnContentOrbitUpdate();
        }

        protected override void OnFingerUp(Finger finger) => onSelection = false;

        protected override void OnInitialization()
        {

        }

        protected override void OnSubscribeToActionEvents(bool subscribe)
        {

        }

        protected override void UpdateSelectableStateAction()
        {

        }

        #endregion

        #endregion
    }
}
