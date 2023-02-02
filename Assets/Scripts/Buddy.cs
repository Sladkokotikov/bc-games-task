using DG.Tweening;
using UnityEngine;

namespace DrawAndRun
{
    public class Buddy : MonoBehaviour
    {
        private BuddyBox _box;
        /*[SerializeField] private GameObject explosion;
        [SerializeField] private GameObject gemExplosion;
        [SerializeField] private GameObject fireworks;*/
        [SerializeField] private float drownDuration = 1;
        [SerializeField] private float drownY = -5;

        /*private void OnCollisionEnter(Collision other)
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
                //Instantiate(gemExplosion, transform.position, Quaternion.identity);
                _box.CollectGem();
                Destroy(other.gameObject);
            }
        }

        public void Init(BuddyBox buddyBox)
        {
            _box = buddyBox;
        }*/

        public void Drown()
        {
            transform.DOLocalMoveY(drownY, drownDuration).Play();
        }
    }
}