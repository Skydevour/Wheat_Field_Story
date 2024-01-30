#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace CommonFramework.Runtime.AssetEditor
{
    [ExecuteInEditMode]
    public class EditorCameraLighting : MonoBehaviour
    {
        private Light cameraLight;

        private void Awake()
        {
            cameraLight = this.gameObject.AddComponent<Light>();
            cameraLight.type = LightType.Directional;
        }

        private void Update()
        {
            if (SceneView.lastActiveSceneView == null)
            {
                Debug.LogErrorFormat("SceneView.lastActiveSceneView in {0} is null", this);
                return;
            }
            this.transform.position = SceneView.lastActiveSceneView.camera.transform.position;
            this.transform.rotation = Quaternion.LookRotation(SceneView.lastActiveSceneView.camera.transform.forward);
        }
    }
}

#endif