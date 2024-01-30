using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UIElements;

namespace CommonFramework.Editor.AssetEditor
{
    public abstract class UnityPreviewEditorBase<U, T> : UnityEditor.Editor where T : UnityEngine.Object where U : UnityPreviewSceneStageBase<T>
    {
        protected abstract string OpenAssetString { get; }
        protected virtual HideFlags FocusObjectFlags { get { return HideFlags.NotEditable; } }
        protected virtual HideFlags SceneObjectFlags { get { return HideFlags.HideInHierarchy | HideFlags.HideInInspector; } }
        protected U previewScene;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement rootVisualElement = new VisualElement();

            if (EditorApplication.isPlaying)
            {
                rootVisualElement.Clear();
                Label warningText = new Label { text = "Preview editor is not available during gameplay!" };
                rootVisualElement.Add(warningText);
                return rootVisualElement;
            }

            //rootVisualElement.Add(new IMGUIContainer(OnInspectorGUI));
            SetupOpenCloseButton(rootVisualElement);

            //TODO: Need to find a way to repaint gui
            InspectorGuiImpl(rootVisualElement);



            return rootVisualElement;
        }


        private void SetupOpenCloseButton(VisualElement rootVisualElement)
        {
            T targetAsset = GetTarget(target);
            SyncPreviewWindow();

            Button openCloseButton = new Button();

            if (!IsPreviewSceneLoaded())
            {
                openCloseButton.clickable.clicked += () => { OnOpenButton(targetAsset); ResetInspectorGUI(rootVisualElement); };
                openCloseButton.text = OpenAssetString;
            }
            else
            {
                openCloseButton.clickable.clicked += () => { OnCloseButton(); };
                openCloseButton.text = "Close";
            }
            rootVisualElement.Add(openCloseButton);
        }

        private void OnOpenButton(T targetAsset)
        {
            previewScene = CreatePreviewWindow(targetAsset);

            //Work around when OnEnable is called but we never open for editing
            //We have no chance to register callback, so we reregister again
            previewScene.UnRegisterPreviewSceneGuiCallback(OnPreviewSceneGUI);
            previewScene.RegisterPreviewSceneGuiCallback(OnPreviewSceneGUI);
        }

        private void OnCloseButton()
        {
            bool decision = EditorUtility.DisplayDialog("Warning", "Are you sure?", "Ok", "Cancel");
            if (decision)
            {
                Debug.Log("ChangeEditMode: Object");
                CloseWindow();
            }
        }

        private void ResetInspectorGUI(VisualElement rootVisualElement)
        {
            rootVisualElement.Clear();
            SetupOpenCloseButton(rootVisualElement);
            InspectorGuiImpl(rootVisualElement);
        }

        protected virtual VisualElement InspectorGuiImpl(VisualElement root)
        {
            return root;
        }

        public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width,
            int height)
        {
            if (GenerateThumbnail(width, height, out Texture2D generateThumbnail))
                return generateThumbnail;

            return null;
        }

        private bool GenerateThumbnail(int width, int height, out Texture2D generatedThumbnail)
        {
            generatedThumbnail = null;

            Texture2D img;
            if (GetExternalThumbnail(out img))
            {
                generatedThumbnail = new Texture2D(width, height);
                EditorUtility.CopySerialized(img, generatedThumbnail);
                return true;
            }

            T targetAsset = GetTarget(target);

            if (targetAsset == null)
                return false;

            GameObject representiveObject = CreateRepresentation(targetAsset);
            if (representiveObject == null)
                return false;

            img = AssetPreview.GetAssetPreview(representiveObject);
            if (img == null)
            {
                UnityEngine.Object.DestroyImmediate(representiveObject);
                return false;
            }

            generatedThumbnail = new Texture2D(width, height);
            EditorUtility.CopySerialized(img, generatedThumbnail);

            var data = generatedThumbnail.GetRawTextureData<byte>();
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] << 1 < 0b11111111)
                {
                    data[i] = (byte)(data[i] << 1);
                }
                else
                {
                    data[i] = 0b11111111;
                }
            }

            UnityEngine.Object.DestroyImmediate(representiveObject);

            return true;
        }

        protected virtual bool GetExternalThumbnail(out Texture2D externalThumbnail)
        {
            externalThumbnail = null;
            return false;
        }

        private void OnEnable()
        {
            if (SyncPreviewWindow())
            {
                previewScene.RegisterPreviewSceneGuiCallback(OnPreviewSceneGUI);
            }

            OnEnableImpl();
        }

        private void OnDisable()
        {
            if (SyncPreviewWindow())
            {
                previewScene.UnRegisterPreviewSceneGuiCallback(OnPreviewSceneGUI);
            }

            OnDisableImpl();
        }

        protected virtual void OnEnableImpl() { }

        protected virtual void OnDisableImpl() { }

        protected virtual void OnPreviewSceneGUI()
        {
            if (Event.current == null || Event.current.keyCode == KeyCode.None || Event.current.type != EventType.KeyDown)
            {
                return;
            }

            if (Event.current.keyCode == KeyCode.F)
            {
                previewScene.CameraFocusOnEditingObject();
            }
        }

        private void CloseWindow()
        {
            StageUtility.GoToMainStage();
        }

        protected bool IsPreviewSceneLoaded()
        {
            if (previewScene == null)
                return false;

            if (previewScene.scene.isLoaded == false)
                return false;

            if (StageUtility.GetCurrentStage() != previewScene)
                return false;

            return true;
        }

        protected bool SyncPreviewWindow()
        {
            if (StageUtility.GetCurrentStage() == previewScene)
                return true;
            U currentStage = StageUtility.GetCurrentStage() as U;
            if (currentStage != null
                && currentStage.GetSelectedAsset() == GetTarget(target))
            {
                previewScene = currentStage;
                return true;
            }
            else
                return false;
        }

        protected abstract GameObject CreateRepresentation(T asset);

        protected T GetTarget(UnityEngine.Object item) { return (T)item; }

        private U CreatePreviewWindow(T asset)
        {
            UnityPreviewSceneStageBase<T>.CreateNewStage<U>(asset, out U newStage, FocusObjectFlags, SceneObjectFlags);
            return newStage;
        }

    }
}