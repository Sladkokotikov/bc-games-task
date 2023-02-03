using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DrawAndRun
{
    public static class Extensions
    {
        public static void Clear(this Transform target)
        {
            foreach(Transform child in target)
                Object.Destroy(child.gameObject);
        }

        public static void Clear(this GameObject target)
            => target.transform.Clear();

        public static IEnumerator Delay(float duration, Action action)
        {
            yield return new WaitForSeconds(duration);
            action();
        }
        
        public static IEnumerator Delay(Func<bool> pred, Action action)
        {
            yield return new WaitUntil(pred);
            action();
        }
    }
}