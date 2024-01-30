using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CommonFramework.Editor
{
    /// <summary>
    /// Inherit from this base class to create a EditorWindowSingleton.
    /// e.g. public class MyClassName : EditorWindowSingleton<MyClassName> {}
    /// </summary>
    public class EditorWindowSingleton<T> : EditorWindow where T : EditorWindow
    {
        private static T instance = null;

        /// <summary>
        /// Returns ture if the window is open
        /// </summary>
        public static bool HasOpenInstances => EditorWindow.HasOpenInstances<T>();

        /// <summary>
        /// Use Instance.Show() to get the EditorWindow
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    CheckExcessiveWindow();
                    instance = GetWindow<T>("EditorWindow", true);
                    (instance as EditorWindowSingleton<T>).OnEditorWindowCreate();
                }
                return instance;
            }
        }

        #region Private Methods

        private static void CheckExcessiveWindow()
        {
            var windows = (T[])Resources.FindObjectsOfTypeAll(typeof(T));
            if (windows.Length > 1)
            {
                Debug.LogWarning("More than one EditorWindowSingleton has been found, cleaning...");
                for (int i = 0; i < windows.Length; i++)
                {
                    DestroyImmediate(windows[i]);
                }
            }
        }

        private void OnDestroy()
        {
            ExecuteDestroySequence();
        }

        private void ExecuteDestroySequence()
        {
            OnDestroyInstance();
        }
        #endregion

        #region Protected Virtual Methods

        /// <summary>
        /// This method will be called only once either on OnApplicationQuit or OnDestroy 
        /// </summary>
        protected virtual void OnDestroyInstance() { }

        protected virtual void OnEditorWindowCreate() { }

        #endregion
    }
}
