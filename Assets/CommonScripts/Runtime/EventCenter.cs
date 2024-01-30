using System.Collections.Generic;
using System;

namespace CommonFramework.Runtime
{
    /// <summary>
    /// Event helper class that handles event messages
    /// </summary>
    public static class EventCenter
    {
        #region Private Fields

        private static Dictionary<Type, List<Delegate>> eventList = new Dictionary<Type, List<Delegate>>();

        #endregion

        #region Public Methods

        /// <summary>
        /// Register event listener with callback
        /// </summary>
        /// <typeparam name="T">Type of passing parameters</typeparam>
        /// <param name="callback">Callback functions</param>
        [System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
        public static void StartListenToEvent<T>(Action<T> callback)
        {
            if (eventList.ContainsKey(typeof(T)))
            {
                eventList[typeof(T)].Add(callback);
            }
            else
            {
                List<Delegate> list = new List<Delegate>();
                list.Add(callback);
                eventList.Add(typeof(T), list);
            }
        }

        public static void StartListenToEvent(Action<object> callback, Type type)
        {
            if (eventList.ContainsKey(type))
            {
                eventList[type].Add(callback);
            }
            else
            {
                List<Delegate> list = new List<Delegate>();
                list.Add(callback);
                eventList.Add(type, list);
            }
        }

        /// <summary>
        /// Remove event listener of callbacks
        /// </summary>
        /// <typeparam name="T">Type of passing parameters</typeparam>
        /// <param name="callback">Callback functions</param>
        [System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
        public static void StopListenToEvent<T>(Action<T> callback)
        {
            if (eventList.ContainsKey(typeof(T)))
            {
                eventList[typeof(T)].Remove(callback);
            }
        }

        public static void StopListenToEvent(Action<object> callback, Type type)
        {
            if (eventList.ContainsKey(type))
            {
                eventList[type].Remove(callback);
            }
        }

        /// <summary>
        /// Triggering events to listeners
        /// </summary>
        /// <typeparam name="T">Type of passing parameters</typeparam>
        /// <param name="args">argument</param>
        [System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
        public static void TriggerEvent<T>(T args)
        {
            List<Delegate> subscribers;
            if (eventList.TryGetValue(typeof(T), out subscribers))
            {
                foreach (Delegate action in subscribers.ToArray())
                {
                    (action as Action<T>).Invoke(args);
                }
            }
        }

        [System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
        public static void TriggerEvent(object args, Type type)
        {
            List<Delegate> subscribers;
            if (eventList.TryGetValue(type, out subscribers))
            {
                foreach (Delegate action in subscribers.ToArray())
                {
                    action.DynamicInvoke(args);
                }
            }
        }

        public static void ClearEvents()
        {
            eventList.Clear();
        }

        #endregion
    }
}
