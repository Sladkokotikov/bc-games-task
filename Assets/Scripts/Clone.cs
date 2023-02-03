using UnityEngine;

namespace DrawAndRun
{
    public class Clone : Spawnable
    {
        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("buddy"))
                return;
            Controller.AddBuddy(transform.position);
            Destroy(gameObject);
        }
    }
}