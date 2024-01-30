using UnityEngine;

namespace CommonFramework.Runtime.Utils
{
    public class GameObjectUtil
    {
        public static void SetHideFlagsToAll(GameObject objectToHide, HideFlags hideFlags)
        {
            Transform[] allTransform = objectToHide.GetComponentsInChildren<Transform>();
            for (int i = 0; i < allTransform.Length; i++)
            {
                allTransform[i].gameObject.hideFlags = hideFlags;
            }
        }
    }
}
