using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

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

        #endregion
    }
}