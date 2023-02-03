using UnityEngine;

namespace DrawAndRun
{
    public class Obstacle : Spawnable
    {
        [SerializeField] private GameObject explosion;
        [SerializeField] private GameObject autoExplosion;
        [SerializeField] private bool autoDestroy;

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("buddy") || other.gameObject == null)
                return;

            Controller.LoseBuddy();

            Instantiate(explosion, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            if (!autoDestroy) 
                return;
            Instantiate(autoExplosion, other.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}