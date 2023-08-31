using System.Collections.Generic;
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

        #endregion
    }
}