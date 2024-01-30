using System;

namespace CommonFramework.Runtime
{
    /// <summary>
    /// Class singleton
    /// </summary>
    /// <typeparam name="T">Type of class</typeparam>
    public class Singleton<T> where T : class
    {
        private static T instance;

        public void Create()
        {
            if (instance == null)
                instance = (T)Activator.CreateInstance(typeof(T), true);

            return;
        }

        /* Serve the single instance to callers */
        public static T Instance
        {
            get
            {
                if (instance == null)
                    instance = (T)Activator.CreateInstance(typeof(T), true);

                return instance;
            }
        }

        /*  Destroy */
        public void Destroy()
        {

            instance = null;

            return;
        }
    }
}
