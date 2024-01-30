using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace CommonFramework.Runtime
{
    /// <summary>
    /// Multithread helper manager
    /// </summary>
    public class ThreadManager : MonoSingleton<ThreadManager>
    {
        #region Structs

        public struct DelayedQueueItem
        {
            public Action action;
            public float time;
        }

        #endregion

        #region Private Fields

        private readonly int maxThreads = 8;

        private static int numOfThreads;

        private List<Action> actions = new List<Action>();
        private List<Action> currentActions = new List<Action>();
        private List<DelayedQueueItem> currentDelayed = new List<DelayedQueueItem>();
        private List<DelayedQueueItem> delayed = new List<DelayedQueueItem>();

        #endregion

        #region Public Methods

        /// <summary>
        /// Queue input action to be execute on main unity thread
        /// </summary>
        /// <param name="action">Callback action</param>
        public void QueueOnMainThread(Action action)
        {
            QueueOnMainThread(action, 0f);
        }

        /// <summary>
        /// Queue input action to be execute on main unity thread with a delay of time
        /// </summary>
        /// <param name="action">Callback actions</param>
        /// <param name="time">delayed time</param>
        public void QueueOnMainThread(Action action, float time)
        {
            if (!Mathf.Approximately(time, 0))
            {
                lock (Instance.delayed)
                {
                    Instance.delayed.Add(new DelayedQueueItem { time = Time.time + time, action = action });
                }
            }
            else
            {
                lock (Instance.actions)
                {
                    Instance.actions.Add(action);
                }
            }
        }

        /// <summary>
        /// Queue action callback on Thread pool and run async
        /// </summary>
        /// <param name="action">Callback action</param>
        /// <returns></returns>
        public Thread RunAsync(Action action)
        {
            while (numOfThreads >= maxThreads)
            {
                Thread.Sleep(1);
            }

            Interlocked.Increment(ref numOfThreads);
            ThreadPool.QueueUserWorkItem(RunAction, action);

            return null;
        }

        #endregion

        #region Private Methods

        private void RunAction(object action)
        {
            try
            {
                ((Action)action)();
            }
            finally
            {
                Interlocked.Decrement(ref numOfThreads);
            }
        }

        private void Update()
        {
            lock (actions)
            {
                currentActions.Clear();
                currentActions.AddRange(actions);
                actions.Clear();
            }

            foreach (var action in currentActions)
            {
                action();
            }

            lock (delayed)
            {
                currentDelayed.Clear();
                currentDelayed.AddRange(delayed.Where(d => d.time <= Time.time));

                foreach (var item in currentDelayed)
                    delayed.Remove(item);
            }

            foreach (var delayed in currentDelayed)
            {
                delayed.action();
            }
        }

        #endregion
    }
}
