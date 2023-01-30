using System;
using System.Collections;
using UnityEngine;

namespace InvokeWithDelay
{
    public static class DelayedInvoke
    {
        public static void Invoke(this MonoBehaviour monoBehaviour, Action method, float delay)
        {
            monoBehaviour.StartCoroutine(InvokeRoutine(method, delay));
        }

        private static IEnumerator InvokeRoutine(Action methodToInvoke, float delay)
        {
            yield return new WaitForSeconds(delay);
        
            methodToInvoke();
        }
    }
}