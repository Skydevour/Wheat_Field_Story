using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditorUndo = UnityEditor.Undo;

namespace CommonFramework.Editor.UndoRedo
{
    public class UndoRedoActionHandler : ScriptableObject, IUndoRedoActionHandler
    {
        //Do we need this ?
        //private readonly int UndoRedoLimit = 200;

        private List<IUndoRedoAction> undoActionMap = new List<IUndoRedoAction>();

        private int currentActionKey = 0;

        [SerializeField]
        private int actionKey = -1;

        public void ClearAllUndoRedo()
        {
            undoActionMap.Clear();
            actionKey = -1;
            UnityEditorUndo.ClearUndo(this);
            Debug.Log("Clear called");
        }

        public void RegisterUndoRedoAction(IUndoRedoAction action)
        {
            int nextIndex = actionKey + 1;

            if (nextIndex < undoActionMap.Count)
            {
                undoActionMap.RemoveRange(nextIndex, undoActionMap.Count - nextIndex);
            }

            undoActionMap.Add(action);

            currentActionKey = nextIndex;

            //We use only action key as properties to registered with Unity undo redo manager
            UnityEditorUndo.RegisterCompleteObjectUndo(this, string.Format("Undo redo registerd: {0}", currentActionKey));
            ++actionKey;
            Debug.LogFormat("undo redo action registered: {0}", actionKey);
        }

        private void Awake()
        {
            Debug.Log("UndoRedoActionHandler Awake");
            UnityEditorUndo.undoRedoPerformed += OnUnityEditoUndoRedoPerformed;
            currentActionKey = actionKey;
        }

        private void OnDestroy()
        {
            Debug.Log("UndoRedoActionHandler OnDestroy");
            UnityEditorUndo.undoRedoPerformed -= OnUnityEditoUndoRedoPerformed;
            ClearAllUndoRedo();
        }

        private void OnUnityEditoUndoRedoPerformed()
        {
            if (actionKey > currentActionKey)
            {
                Redo(actionKey);
            }
            else if (actionKey < currentActionKey)
            {
                Undo(currentActionKey);
            }
            else
            {
                Debug.LogError("No action performed");
            }

            currentActionKey = actionKey;
        }

        private void Undo(int key)
        {
            if (key >= 0 && undoActionMap.Count > 0)
            {
                undoActionMap[key].Undo();
                Debug.LogFormat("Undo {0} preformed", key);
            }
            else
            {
                Debug.LogFormat("Unexpected undo key: {0}", key);
            }
        }

        private void Redo(int key)
        {
            if (key < undoActionMap.Count)
            {
                undoActionMap[key].Redo();
                Debug.LogFormat("Redo {0} preformed", key);
            }
            else
            {
                Debug.LogFormat("Unexpected redo key: {0}", key);
            }
        }
    }
}
