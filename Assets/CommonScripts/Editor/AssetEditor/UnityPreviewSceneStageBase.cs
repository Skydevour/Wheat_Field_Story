using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using CommonFramework.Runtime.Utils;
using CommonFramework.Runtime.AssetEditor;
using CommonFramework.Editor.UndoRedo;

namespace CommonFramework.Editor.AssetEditor
{
    public abstract class UnityPreviewSceneStageBase<U> : PreviewSceneStage, IUndoRedoActionHandler where U : UnityEngine.Object
    {
        private static Scene orePreviewScene;

        public U BackupData;

        protected abstract U SelectedAsset { set; get; }
        private GameObject objectToFocus;
        private GameObject[] sceneObjects;
        private HideFlags focusObjectHideFlags;
        private HideFlags sceneObjectsHideFlags;

        private Action onPreviewSceneGuiEvent;

        private UndoRedoActionHandler handler;

        public static void CreateNewStage<T>(U asset,
                                                                    out T newStage,
                                                                    HideFlags focusObjectHideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector,
                                                                    HideFlags sceneObjectsHideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector)
                                                                    where T : UnityPreviewSceneStageBase<U>
        {
            //Force stage go back to main first, or if we just open another stage, it will be new stage open first, and old one close later
            //This will cause block editor tool being disabled by the later close stage events
            if (StageUtility.GetCurrentStage() != StageUtility.GetMainStage())
                StageUtility.GoToMainStage();

            newStage = CreateInstance<T>();

            newStage.SelectedAsset = asset;
            newStage.focusObjectHideFlags = focusObjectHideFlags;
            newStage.sceneObjectsHideFlags = sceneObjectsHideFlags;

            //Workaround to avoid "Unable to allocate new scene culling mask" when keep creating preview stage            
            if (!orePreviewScene.IsValid())
            {
                Debug.LogWarning("OrePreviewScene Invalid!");
                orePreviewScene = EditorSceneManager.NewPreviewScene();
            }

            Debug.LogWarning("Going to new stage");
            newStage.scene = orePreviewScene;
            StageUtility.GoToStage(newStage, true);
            newStage.SetupScene();
            newStage.OnFirstTimeOpenStageInSceneView(SceneView.lastActiveSceneView);

            SceneView.RepaintAll();
        }

        public U GetSelectedAsset()
        {
            return SelectedAsset;
        }

        public GameObject GetRepresentiveObject()
        {
            return objectToFocus;
        }

        public GameObject[] GetSceneObjects()
        {
            return sceneObjects;
        }

        public void ResetRepresentiveObject(bool resetCameraFocus = true)
        {
            DestroyImmediate(objectToFocus);
            objectToFocus = CreateObjectToFocus();

            GameObjectUtil.SetHideFlagsToAll(objectToFocus, focusObjectHideFlags);

            StageUtility.PlaceGameObjectInCurrentStage(objectToFocus);

            if (resetCameraFocus)
                SetupCameraFocus(objectToFocus);
        }

        public bool Raycast(Ray physicsRay, out RaycastHit hitInfo)
        {
            return scene.GetPhysicsScene().Raycast(physicsRay.origin, physicsRay.direction, out hitInfo);
        }

        public void RegisterPreviewSceneGuiCallback(Action action)
        {
            onPreviewSceneGuiEvent += action;
        }

        public void UnRegisterPreviewSceneGuiCallback(Action action)
        {
            onPreviewSceneGuiEvent -= action;
        }

        public void CameraFocusOnEditingObject()
        {
            if (objectToFocus != null)
                SetupCameraFocus(objectToFocus);
        }

        public void ClearAllUndoRedo()
        {
            if (handler != null)
                handler.ClearAllUndoRedo();
        }

        public void RegisterUndoRedoAction(IUndoRedoAction action)
        {
            if (handler != null)
            {
                handler.RegisterUndoRedoAction(action);
            }
        }

        protected override GUIContent CreateHeaderContent()
        {
            GUIContent headerContent = new GUIContent();

            if (SelectedAsset == null)
                return headerContent;

            headerContent.text = SelectedAsset.name;
            headerContent.image = EditorGUIUtility.IconContent("GameObject Icon").image;

            return headerContent;
        }

        protected override void OnFirstTimeOpenStageInSceneView(SceneView sceneView)
        {
            // Default to not showing skybox if user did not specify a custom environment scene.
            if (string.IsNullOrEmpty(scene.path))
            {
                sceneView.sceneLighting = false;
                sceneView.sceneViewState.showSkybox = false;
            }
        }

        private bool HasAnyActiveLights(Scene scene)
        {
            foreach (var gameObject in scene.GetRootGameObjects())
            {
                if (gameObject.GetComponentsInChildren<Light>(false).Length > 0)
                    return true;
            }

            return false;
        }

        private void SetupScene()
        {
            objectToFocus = CreateObjectToFocus();
            if (objectToFocus != null)
            {
                GameObjectUtil.SetHideFlagsToAll(objectToFocus, focusObjectHideFlags);

                StageUtility.PlaceGameObjectInCurrentStage(objectToFocus);
                SetupCameraFocus(objectToFocus);
            }

            sceneObjects = CreateSceneObjects();
            if (sceneObjects == null || sceneObjects.Length <= 0)
                return;

            for (int i = 0; i < sceneObjects.Length; i++)
            {
                GameObjectUtil.SetHideFlagsToAll(sceneObjects[i], sceneObjectsHideFlags);

                if (sceneObjects[i] != null)
                    StageUtility.PlaceGameObjectInCurrentStage(sceneObjects[i]);
            }

            OnSetupSceneFinished();
        }

        private void SetupCameraFocus(GameObject objectToFocus)
        {
            Selection.activeTransform = objectToFocus.transform;
            SceneView.lastActiveSceneView.FrameSelected();
            Selection.activeObject = SelectedAsset;
        }

        private void OnPreviewSceneGUI(SceneView obj)
        {
            onPreviewSceneGuiEvent?.Invoke();
        }

        protected abstract GameObject CreateObjectToFocus();
        protected virtual GameObject[] CreateSceneObjects()
        {
            GameObject light = new GameObject("Lighting");
            light.AddComponent<EditorCameraLighting>();
            return new GameObject[] { light };
        }

        protected override void OnCloseStage()
        {
            base.OnCloseStage();
            DestroyImmediate(handler);
            SceneView.duringSceneGui -= OnPreviewSceneGUI;
            onPreviewSceneGuiEvent = null;
            OnCloseStageImpl();
            Debug.Log("close stage");
        }

        protected override bool OnOpenStage()
        {
            onPreviewSceneGuiEvent = null;
            SceneView.duringSceneGui += OnPreviewSceneGUI;
            BackupData = Instantiate(SelectedAsset);
            handler = ScriptableObject.CreateInstance<UndoRedoActionHandler>();
            OnOpenStageImpl();
            Debug.Log("open stage");
            return base.OnOpenStage();
        }

        protected virtual void OnOpenStageImpl() { }

        protected virtual void OnCloseStageImpl() { }

        protected virtual void OnSetupSceneFinished() { }
    }
}
