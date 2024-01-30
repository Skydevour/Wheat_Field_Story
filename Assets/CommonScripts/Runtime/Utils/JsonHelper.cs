using System;
using System.Text.RegularExpressions;

namespace CommonFramework.Runtime.Utils
{
    /// <summary>
    /// Helper class to handle json array
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// Parsing json string to arrays
        /// </summary>
        /// <typeparam name="T">Type of class</typeparam>
        /// <param name="json">Json strings</param>
        /// <returns></returns>
        public static T[] FromJson<T>(string json)
        {
            string serviceData = "{\"Items\":" + json + "}";
            UnityEngine.Debug.LogFormat("Try parsing {0} to objects", serviceData);

            Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(serviceData);
            return wrapper.Items;
        }

        /// <summary>
        /// Format a list of objects to json strings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;

            string jsonString = UnityEngine.JsonUtility.ToJson(wrapper);

            jsonString = jsonString.TrimEnd('}');
            jsonString = jsonString.Replace("{\"Items\":", string.Empty);

            return jsonString;
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}
