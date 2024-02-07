using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.TMP_InputField;

namespace Com.RedicalGames.Filar
{ 
    public static class Extensions
    {

        #region Extension Methods

        public static AppData.VectorData ToSerializableVector(this Vector2 vector)
        {
            return new AppData.VectorData
            {
                x = vector.x,
                y = vector.y
            };
        }

        public static AppData.VectorData ToSerializableVector(this Vector3 vector)
        {
            return new AppData.VectorData
            {
                x = vector.x,
                y = vector.y,
                z = vector.z
            };
        }

        public static AppData.VectorData ToSerializableVector(this Vector4 vector)
        {
            return new AppData.VectorData
            {
                x = vector.x,
                y = vector.y,
                z = vector.z,
                w = vector.w
            };
        }

        #region Mesh

        #endregion

        //public static AppData.SerializableMeshData ToSerializableMeshData(this Mesh mesh) => new AppData.SerializableMeshData(mesh);

        public static async Task<AppData.MeshData> ToSerializableMeshDataAsync(this Mesh mesh)
        {
            var data = new AppData.MeshData();
            var results = await data.ConvertToSerializableMeshDataAsync(mesh);

            return results;
        }

        public static IEnumerable<string> Split(this string source, int chunkLength)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException("String Source Is Null Or Empty");

            if (chunkLength <= 0)
                throw new ArgumentException("String Split Value Is Set To 0 - Invalid Operation");

            return Enumerable.Range(0, source.Length / chunkLength).Select(x => source.Substring(x * chunkLength, chunkLength));
        }

        public static Vector2 GetWidgetScale(this RectTransform reference) => reference.sizeDelta;
        public static Vector2 GetWidgetPosition(this RectTransform reference) => reference.anchoredPosition;
        public static Vector3 GetWidgetRotationAngle(this RectTransform reference) => reference.localEulerAngles;
        public static Quaternion GetWidgetRotation(this RectTransform reference) => reference.rotation;
        public static Quaternion GetWidgetRotationLocal(this RectTransform reference) => reference.localRotation;

        public static (Vector2 position, Vector2 scale, Vector3 rotationAngle) GetWidgetPoseAngle(this RectTransform reference) => (reference.GetWidgetPosition(), reference.GetWidgetScale(), reference.GetWidgetRotationAngle());
        public static (Vector2 position, Vector2 scale, Quaternion rotation) GetWidgetPose(this RectTransform reference) => (reference.GetWidgetPosition(), reference.GetWidgetScale(), reference.GetWidgetRotation());
        public static (Vector2 position, Vector2 scale, Quaternion rotation) GetWidgetPoseLocal(this RectTransform reference) => (reference.GetWidgetPosition(), reference.GetWidgetScale(), reference.GetWidgetRotationLocal());
        public static void SetWidgetPose(this RectTransform reference, Vector2 position, Vector2 scale, Vector3 rotationAngle)
        {
            reference.SetWidgetPosition(position);
            reference.SetWidgetScale(scale);
            reference.SetWidgetRotation(rotationAngle);
        }

        public static void SetWidgetPose(this RectTransform reference, (Vector2 position, Vector2 scale, Vector3 rotationAngle) pose)
        {
            reference.SetWidgetPosition(pose.position);
            reference.SetWidgetScale(pose.scale);
            reference.SetWidgetRotation(pose.rotationAngle);
        }

        public static void SetWidgetPose(this RectTransform reference, Vector2 position, Vector2 scale, Quaternion rotation)
        {
            reference.SetWidgetPosition(position);
            reference.SetWidgetScale(scale);
            reference.SetWidgetRotation(rotation);
        }

        public static void SetWidgetPoseLocal(this RectTransform reference, Vector2 position, Vector2 scale, Quaternion rotation)
        {
            reference.SetWidgetPosition(position);
            reference.SetWidgetScale(scale);
            reference.SetWidgetRotationLocal(rotation);
        }

        public static void SetWidgetScale(this RectTransform reference, Vector2 scale) => reference.sizeDelta = scale;
        public static void SetWidgetScale(this RectTransform reference, int width, int height) => reference.sizeDelta = new Vector2(width, height);

        public static void SetWidgetPosition(this RectTransform reference, Vector2 position) => reference.anchoredPosition = position;
        public static void SetWidgetRotation(this RectTransform reference, Vector3 rotationAngle) => reference.localEulerAngles = rotationAngle;
        public static void SetWidgetRotationLocal(this RectTransform reference, Quaternion rotation) => reference.localRotation = rotation;
        public static void SetWidgetRotation(this RectTransform reference, Quaternion rotation) => reference.rotation = rotation;

        public static string GetName(this RectTransform reference) => reference.name; 

        public static bool AssignedAndValid(this RectTransform reference) => reference != null;

        public static float ToFloat(this string source) => AppData.Helpers.StringToFloat(source);
        public static int ToInt(this string source) => AppData.Helpers.StringToInt(source);

        public static Vector2 ToVector2(this string source) => AppData.Helpers.StringToVector2(source, " v2|");
        public static string ToStringVector(this Vector2 source) => AppData.Helpers.Vector2ToString(source, " v2|");

        public static Vector3 ToVector3(this string source) => AppData.Helpers.StringToVector3(source, " v3|");
        public static string ToStringVector(this Vector3 source) => AppData.Helpers.Vector3ToString(source, " v3|");

        public static Vector2 ToVector4(this string source) => AppData.Helpers.StringToVector4(source, " v4|");
        public static string ToStringVector(this Vector4 source) => AppData.Helpers.Vector4ToString(source, " v4|");

        public static string TransformToString(this Transform source) => AppData.Helpers.TransformToString(source, " trn|");
        public static (string name, Vector3 localPosition, Vector3 localScale, Vector3 localEulerAngles) ToTransformInfo(this string source) => AppData.Helpers.StringToTransformInfo(source, " trn|");

        public static bool GetActive(this GameObject gameObject) => gameObject.activeSelf && gameObject.activeInHierarchy;
        public static bool GetInActive(this GameObject gameObject) => !gameObject.activeSelf && !gameObject.activeInHierarchy;
        public static void Show(this GameObject gameObject) => gameObject.SetActive(true);
        public static void Hide(this GameObject gameObject) => gameObject.SetActive(false);
        public static void SetName(this GameObject gameObject, string name) => gameObject.name = name;
        public static string GetName(this GameObject gameObject) => gameObject.name;
        public static RectTransform GetWidgetRect(this GameObject gameObject) => gameObject.GetComponent<RectTransform>();

        public static string GetName(this Transform reference) => reference.name;

        public static void SetPose(this Transform reference, (Vector3 position, Vector3 scale, Quaternion rotation) pose)
        {
            reference.position = pose.position;
            reference.localScale = pose.scale;
            reference.rotation = pose.rotation;
        }

        public static void SetLocalPose(this Transform reference, (Vector3 position, Vector3 scale, Quaternion rotation) pose)
        {
            reference.localPosition = pose.position;
            reference.localScale = pose.scale;
            reference.localRotation = pose.rotation;
        }

        public static (Vector3 position, Vector3 scale, Quaternion rotation) GetPose(this Transform reference) => (reference.position, reference.localScale, reference.rotation);
        public static (Vector3 position, Vector3 scale, Quaternion rotation) GetLocalPose(this Transform reference) => (reference.localPosition, reference.localScale, reference.localRotation);

        #region Unity Inputs Extensions

        #region Button

        public static void ShowActionInput(this Button reference) => reference.gameObject.SetActive(true);
        public static void HideActionInput(this Button reference) => reference.gameObject.SetActive(false);

        public static string GetName(this Button reference) => reference.name;

        #endregion

        #region Input Field

        public static void SetValue(this TMP_InputField reference, string value, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppStringValueNotNullOrEmpty(value, "Value", $"Set Value Failed - Value Parameter For {reference.GetName()} Is Not Assigned - Invalid Operation."));

            if (callbackResults.Success())
            {
                reference.text = value;
                callbackResults.result = $"Set Value Success - Value Parameter For {reference.GetName()} Is Set To : {value}.";
            }

            callback?.Invoke(callbackResults);
        }

        public static void ClearValue(this TMP_InputField reference, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppStringValueNotNullOrEmpty(reference.text, "Value", $"Clear Value Failed - There Is No Value To Clear For {reference.GetName()} - Continuing Operation."));

            if (callbackResults.Success())
            {
                reference.text = string.Empty;
                callbackResults.result = $"Clear Value Success - Value For {reference.GetName()} Has Been Successfully Cleared.";
            }
            else
                callbackResults.resultCode = AppData.Helpers.SuccessCode;

            callback?.Invoke(callbackResults);
        }

        public static void SetFieldType(this TMP_InputField reference, ContentType fieldType, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppEnumValueValid(fieldType, "Field Type", $"Set Field Type Failed - Field Type Parameter Value For {reference.GetName()} Is Set To Default : {fieldType} - Invalid Operation."));

            if (callbackResults.Success())
            {
                reference.contentType = fieldType;
                callbackResults.result = $"Set Field Type Success - Field Type Parameter For {reference.GetName()} Is Set To : {fieldType}.";
            }

            callback?.Invoke(callbackResults);
        }

        public static void Refresh(this TMP_InputField reference, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(reference.GetComponent<SelectableInputComponentHandler>(), "Selectable Input Component Handler", $"On Select Failed - SelectableInputComponentHandler Is Missing From Input Field : {reference.GetName()} - Invalid Operation."));

            if (callbackResults.Success())
                reference.ForceLabelUpdate();

            callback?.Invoke(callbackResults);
        }

        public static void OnSelect(this TMP_InputField reference, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(reference.GetComponent<SelectableInputComponentHandler>(), "Selectable Input Component Handler", $"On Select Failed - SelectableInputComponentHandler Is Missing From Input Field : {reference.GetName()} - Invalid Operation."));

            if (callbackResults.Success())
                reference.Select();

            callback?.Invoke(callbackResults);
        }

        public static AppData.CallbackData<string> GetValue(this TMP_InputField reference)
        {
            var callbackResults = new AppData.CallbackData<string>(AppData.Helpers.GetAppStringValueNotNullOrEmpty(reference.text, "Value", $"Get Value Failed - There Is No Value Assigned For {reference.GetName()} - Invalid Operation."));

            if(callbackResults.Success())
            {
                callbackResults.result = $"Get Value Success - Value Found For {reference.GetName()}.";
                callbackResults.data = reference.text;
            }

            return callbackResults;
        }

        public static AppData.CallbackData<int> GetTextLength(this TMP_InputField reference)
        {
            var callbackResults = new AppData.CallbackData<int>(AppData.Helpers.GetAppStringValueNotNullOrEmpty(reference.text, "Value", $"Get Text Length Failed - There Is No Value Assigned For {reference.GetName()} - Invalid Operation."));

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Text Length Success - {reference.GetName()} Has A Text Length Of : {reference.text.Length}.";
                callbackResults.data = reference.text.Length;
            }

            return callbackResults;
        }

        public static void ShowActionInput(this TMP_InputField reference) => reference.gameObject.SetActive(true);
        public static void HideActionInput(this TMP_InputField reference) => reference.gameObject.SetActive(false);

        public static string GetName(this TMP_InputField reference) => reference.name;

        #endregion

        #region Slider

        public static void ShowActionInput(this Slider reference) => reference.gameObject.SetActive(true);
        public static void HideActionInput(this Slider reference) => reference.gameObject.SetActive(false);

        public static string GetName(this Slider reference) => reference.name;

        #endregion

        #region Dropdown

        public static void ShowActionInput(this TMP_Dropdown reference) => reference.gameObject.SetActive(true);
        public static void HideActionInput(this TMP_Dropdown reference) => reference.gameObject.SetActive(false);

        public static string GetName(this TMP_Dropdown reference) => reference.name;

        #endregion

        #region Checkbox

        public static void ShowActionInput(this Toggle reference) => reference.gameObject.SetActive(true);
        public static void HideActionInput(this Toggle reference) => reference.gameObject.SetActive(false);

        public static string GetName(this Toggle reference) => reference.name;

        #endregion

        #region Text

        public static void SetValue(this TMP_Text reference, string value, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppStringValueNotNullOrEmpty(value, "Value", $"Set Value Failed - Value Parameter For {reference.GetName()} Is Not Assigned - Invalid Operation."));

            if (callbackResults.Success())
            {
                reference.text = value;
                callbackResults.result = $"Set Value Success - Value Parameter For {reference.GetName()} Is Set To : {value}.";
            }

            callback?.Invoke(callbackResults);
        }

        public static void ClearValue(this TMP_Text reference, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppStringValueNotNullOrEmpty(reference.text, "Value", $"Clear Value Failed - There Is No Value To Clear For {reference.GetName()} - Continuing Operation."));

            if (callbackResults.Success())
            {
                reference.text = string.Empty;
                callbackResults.result = $"Clear Value Success - Value For {reference.GetName()} Has Been Successfully Cleared.";
            }
            else
                callbackResults.resultCode = AppData.Helpers.SuccessCode;

            callback?.Invoke(callbackResults);
        }

        public static AppData.CallbackData<string> GetValue(this TMP_Text reference)
        {
            var callbackResults = new AppData.CallbackData<string>(AppData.Helpers.GetAppStringValueNotNullOrEmpty(reference.text, "Value", $"Get Value Failed - There Is No Value Assigned For {reference.GetName()} - Invalid Operation."));

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Value Success - Value Found For {reference.GetName()}.";
                callbackResults.data = reference.text;
            }

            return callbackResults;
        }

        public static AppData.CallbackData<int> GetTextLength(this TMP_Text reference)
        {
            var callbackResults = new AppData.CallbackData<int>(AppData.Helpers.GetAppStringValueNotNullOrEmpty(reference.text, "Value", $"Get Text Length Failed - There Is No Value Assigned For {reference.GetName()} - Invalid Operation."));

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Text Length Success - {reference.GetName()} Has A Text Length Of : {reference.text.Length}.";
                callbackResults.data = reference.text.Length;
            }

            return callbackResults;
        }

        public static void ShowActionInput(this TMP_Text reference) => reference.gameObject.SetActive(true);
        public static void HideActionInput(this TMP_Text reference) => reference.gameObject.SetActive(false);

        public static string GetName(this TMP_Text reference) => reference.name;

        #endregion

        #region Image

        public static void ShowActionInput(this Image reference) => reference.gameObject.SetActive(true);
        public static void HideActionInput(this Image reference) => reference.gameObject.SetActive(false);

        public static string GetName(this Image reference) => reference.name;

        #endregion

        #endregion

        #region Container Placements

        /// <summary>
        /// Adds This Object To A Paraent Object Container
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="container">The Parent To Contain This Object As A Transform Type</param>
        /// <param name="keepWorldSpace">Keep The Original Object's Position In World Space</param>
        public static void AddToPlacementContainer(this GameObject gameObject, Transform container, bool keepWorldSpace = false) => gameObject.transform.SetParent(container, keepWorldSpace);

        /// <summary>
        /// Adds This Object To A Paraent Object Container
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="container">The Parent To Contain This Object As A Game Object Type</param>
        /// <param name="keepWorldSpace">Keep The Original Object's Position In World Space</param>
        public static void AddToPlacementContainer(this GameObject gameObject, GameObject container, bool keepWorldSpace = false) => gameObject.transform.SetParent(container.transform, keepWorldSpace);

        /// <summary>
        /// Adds This Object To A Paraent Object Container
        /// </summary>
        /// <param name="gameObject">This Widget Object</param>
        /// <param name="container">The Parent To Contain This Object As A Rect Transform Type</param>
        /// <param name="keepWorldSpace">Keep The Original Object's Position In World Space</param>
        public static void AddToPlacementContainer(this GameObject gameObject, RectTransform container, bool keepWorldSpace = false) => gameObject.transform.SetParent(container.transform, keepWorldSpace);

        #endregion

        #endregion
    }
}