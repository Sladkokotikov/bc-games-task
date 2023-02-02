using UnityEngine;

namespace DrawAndRun
{
    public class Obstacle : Spawnable
    {
        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("buddy"))
                return;
            Destroy(other.gameObject);
            Controller.LoseBuddy();
        }
    }
}