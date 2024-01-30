using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace CommonFramework.Runtime
{
    public class CoroutineManager : MonoSingleton<CoroutineManager>
    {
        private readonly Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();
        private Coroutine coordinator = null;

        [System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
        public void AddToQueue(IEnumerator coroutine)
        {
            coroutineQueue.Enqueue(coroutine);
            if (coordinator == null)
                coordinator = StartCoroutine(CoroutineCoordinator());
        }

        private IEnumerator CoroutineCoordinator()
        {
            while (coroutineQueue.Count > 0)
                yield return coroutineQueue.Dequeue();
            coordinator = null;
        }
    }
}
