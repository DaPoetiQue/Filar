using UnityEngine;

public class AssetImportRotateAroundItemHandler : MonoBehaviour
{
    [SerializeField]
    Camera eventCamera = null;

    [SerializeField]
    Transform target = null;

    [Space(5)]
    [SerializeField]
    float rotationSpeed = 10.0f;

    void Start() => Init();

#if UNITY_EDITOR

    void OnMouseDrag()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            float xRot = Input.GetAxis("Mouse X") * rotationSpeed;
            float yRot = Input.GetAxis("Mouse Y") * rotationSpeed;

            Vector3 rightVect = Vector3.Cross(eventCamera.transform.up, this.transform.position - eventCamera.transform.position);
            Vector3 upVect = Vector3.Cross(transform.position - eventCamera.transform.position, rightVect);

            this.transform.rotation = Quaternion.AngleAxis(-xRot, upVect) * this.transform.rotation;
            this.transform.rotation = Quaternion.AngleAxis(yRot, rightVect) * this.transform.rotation;
        }
    }

#endif

    void Update() => RotateSceneViewObject();

    void Init()
    {
        if(eventCamera == null || target == null)
        {
            Debug.LogError("--> Event Cam or Target Missing");
            return;
        }
    }

    void RotateSceneViewObject()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            foreach (Touch touch in Input.touches)
            {
                Ray ray = eventCamera.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, Mathf.Infinity))
                {
                    if (touch.phase == TouchPhase.Moved)
                    {
                        transform.Rotate(touch.deltaPosition.y * rotationSpeed * Time.deltaTime, (-touch.deltaPosition.x) * rotationSpeed * Time.deltaTime, 0.0f, Space.World);
                    }
                }
            }
        }
    }

}
