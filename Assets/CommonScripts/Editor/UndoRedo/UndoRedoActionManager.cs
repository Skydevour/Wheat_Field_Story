using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditorUndo = UnityEditor.Undo;

namespace CommonFramework.Editor.UndoRedo
{
    public class UndoRedoActionManager : ScriptableSingleton<UndoRedoActionManager>, IUndoRedoActionHandler
    {
        [SerializeField]
        private UndoRedoActionHandler handler;

        private void Awake()
        {
            Debug.Log("UndoRedoActionManager Awake");
            handler = ScriptableObject.CreateInstance<UndoRedoActionHandler>();
        }

        private void OnDestroy()
        {
            Debug.Log("UndoRedoActionManager OnDestroy");
            DestroyImmediate(handler);
        }

        private void OnValidate()
        {
            if (handler == null)
            {
                Debug.Log("UndoRedoActionManager handler is null, create a new one");
                handler = ScriptableObject.CreateInstance<UndoRedoActionHandler>();
            }
        }

        public void ClearAllUndoRedo()
        {
            OnValidate();
            handler.ClearAllUndoRedo();
        }

        public void RegisterUndoRedoAction(IUndoRedoAction action)
        {
            OnValidate();
            handler.RegisterUndoRedoAction(action);
        }
    }
}
