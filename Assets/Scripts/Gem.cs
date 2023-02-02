using UnityEngine;

namespace DrawAndRun
{
    public class Gem: Spawnable
    {
        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("buddy"))
                return;
            Controller.CollectGem();
            Destroy(gameObject);
        }
    }
}