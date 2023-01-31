using UnityEngine;

namespace DrawAndRun
{
    public class Buddy : MonoBehaviour
    {
        private BuddyBox _box;
        [SerializeField] private GameObject explosion;
        [SerializeField] private GameObject gemExplosion;
        [SerializeField] private GameObject fireworks;
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("obstacle"))
            {
                
                Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
                _box.CountDeath();
                return;
            }

            if (other.gameObject.CompareTag("clone"))
            {
                _box.AddClone(transform.localPosition);
                Destroy(other.gameObject);
                return;
            }

            if (other.gameObject.CompareTag("gem"))
            {
                Instantiate(gemExplosion, transform.position, Quaternion.identity);
                _box.CollectGem();
                Destroy(other.gameObject);
                
            }
            
            if (other.gameObject.CompareTag("Finish"))
            {
                //Instantiate(fireworks, transform.position, Quaternion.identity);
                _box.Win();
                return;
            }


        }
        public void Init(BuddyBox buddyBox)
        {
            _box = buddyBox;
        }
    }
}