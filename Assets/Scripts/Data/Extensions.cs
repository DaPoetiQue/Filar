using System.Collections.Generic;
using UnityEngine;

namespace Com.RedicalGames.Filar
{ 
    public static class Extensions
    {

        #region Extension Methods

        public static AppData.SerializableVector ToSerializableVector(this Vector2 vector)
        {
            return new AppData.SerializableVector
            {
                x = vector.x,
                y = vector.y
            };
        }

        public static AppData.SerializableVector ToSerializableVector(this Vector3 vector)
        {
            return new AppData.SerializableVector
            {
                x = vector.x,
                y = vector.y,
                z = vector.z
            };
        }

        public static AppData.SerializableVector ToSerializableVector(this Vector4 vector)
        {
            return new AppData.SerializableVector
            {
                x = vector.x,
                y = vector.y,
                z = vector.z,
                w = vector.w
            };
        }

        #region Mesh

        #endregion

        public static AppData.SerializableMeshData ToSerializableMeshData(this Mesh mesh) => new AppData.SerializableMeshData(mesh);

        #endregion
    }
}