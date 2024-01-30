using UnityEngine;
using System.Collections.Generic;
using CommonFramework.Runtime.Extensions;

namespace CommonFramework.Runtime.UISystems
{
    /// <summary>
    /// Manager to create/destroy UI views
    /// </summary>
    public class UIManager : MonoSingleton<UIManager>
    {
        private Dictionary<UIType, GameObject> uiDictionary = new Dictionary<UIType, GameObject>();

        /// <summary>
        /// Find UI by UIType and loading them from resource folder
        /// </summary>
        /// <param name="uiType">UI Type</param>
        /// <returns>Game object loaded from resources</returns>
        public GameObject GetSingleUI(UIType uiType)
        {
            if (this.transform == null)
            {
                Debug.LogError("Missing Canvas");
                return null;
            }

            if (uiDictionary.ContainsKey(uiType) == false || uiDictionary[uiType] == null)
            {
                GameObject go = GameObject.Instantiate(Resources.Load<GameObject>(uiType.Path), transform);
                go.name = uiType.Name;
                uiDictionary.AddOrReplace(uiType, go);
                return go;
            }
            return uiDictionary[uiType];
        }

        /// <summary>
        /// Destroy by UI type
        /// </summary>
        /// <param name="uiType">UI Type</param>
        public void DestroySingleUI(UIType uiType)
        {
            if (!uiDictionary.ContainsKey(uiType))
            {
                return;
            }

            if (uiDictionary[uiType] == null)
            {
                uiDictionary.Remove(uiType);
                return;
            }

            GameObject.Destroy(uiDictionary[uiType]);
            uiDictionary.Remove(uiType);
        }

        public void SetActive(bool enable)
        {
            this.transform.gameObject.SetActive(enable);
        }
    }
}
