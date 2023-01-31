using UnityEngine;

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
    }
}