using UnityEngine;

namespace CommonFramework.Runtime
{
    /// <summary>
    /// Inherit from this base class to create a singleton.
    /// e.g. public class MyClassName : Singleton<MyClassName> {}
    /// </summary>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region Private Fields

        // Check to see if we're about to be destroyed.
        private static bool isShuttingDown = false;
        private static object lockObject = new object();
        private static T instance;

        #endregion

        #region Public Fields

        public static bool HasOpenInstances
        {
            get { return instance != null; }
        }

        /// <summary>
        /// Access singleton instance through this propriety.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (isShuttingDown)
                {
                    Debug.LogWarning("[MonoSingleton] Instance '" + typeof(T) +
                        "' already destroyed. Returning null.");
                    return null;
                }

                lock (lockObject)
                {
                    CreateInstance();

                    return instance;
                }
            }
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Call this to initialize manager, please implement all initialization in the Awake
        /// </summary>
        public void Create() { }

        public static void CreateInstance()
        {
            if (instance != null)
                return;

            isShuttingDown = false;

            // Search for existing instance.
            instance = (T)FindObjectOfType(typeof(T));

            // Create new instance if one doesn't already exist.
            if (instance == null)
            {
                // Need to create a new GameObject to attach the singleton to.
                var singletonObject = new GameObject();
                instance = singletonObject.AddComponent<T>();
                singletonObject.name = typeof(T).ToString() + " (MonoSingleton)";

                // Make instance persistent.
                DontDestroyOnLoad(singletonObject);
            }
        }

        public static void DestroyInstance()
        {
            if (instance != null)
            {
                Destroy(instance.gameObject);
            }
        }

        #endregion

        #region Private Methods

        private void OnApplicationQuit()
        {
            ExecuteDestroySequence();
        }


        private void OnDestroy()
        {
            ExecuteDestroySequence();
        }

        private void ExecuteDestroySequence()
        {
            if (!isShuttingDown)
            {
                Debug.LogFormat("[MonoSingleton] {0} is destroying...", typeof(T));
                OnDestroyInstance();
            }
            isShuttingDown = true;
        }

        #endregion

        #region Protected Virtual Methods

        /// <summary>
        /// This method will be called only once either on OnApplicationQuit or OnDestroy 
        /// </summary>
        protected virtual void OnDestroyInstance() { }

        #endregion
    }
}