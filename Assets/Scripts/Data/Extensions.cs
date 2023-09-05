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

        #endregion
    }
}