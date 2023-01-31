using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace DrawAndRun
{
    public class BuddyBox : MonoBehaviour
    {
        [SerializeField] private Buddy buddyPrefab;
        [SerializeField] private LineDrawer drawer;
        [SerializeField] private int buddyCount;
        [SerializeField] private Transform buddyBox;
        [SerializeField] private float spawnRangeWidth;
        [SerializeField] private float buddyRandomOfset;
        [SerializeField] private Vector3 buddyOffset;
        [SerializeField] private GameController controller;
        private static readonly int Hip = Animator.StringToHash("hip");

        private void OnEnable()
        {
            drawer.OnEndDraw += EndDragHandler;
        }

        private void OnDisable()
        {
            drawer.OnEndDraw -= EndDragHandler;
        }

        private void EndDragHandler(List<Vector2> positions, Vector2 sizeDelta)
        {
            buddyBox.Clear();

            if (buddyCount == 0)
                return;
            print(positions.Count);
            while (positions.Count < buddyCount)
                positions.Add(Random.insideUnitCircle * buddyRandomOfset);
            var step = positions.Count / buddyCount;
            for (var i = 0; i < buddyCount; i++)
            {
                var realPos = positions[i * step];
                realPos.Scale(new Vector2(1 / sizeDelta.x, 1 / sizeDelta.y));
                var newPos = spawnRangeWidth * new Vector3(realPos.x, 0, realPos.y * sizeDelta.y / sizeDelta.x);
                SpawnBuddy(newPos + buddyOffset);
            }
        }

        private void SpawnBuddy(Vector3 pos)
        {
            var buddy = Instantiate(buddyPrefab, buddyBox);
            buddy.transform.localPosition = pos;
            buddy.Init(this);
        }

        public void CountDeath()
        {
            buddyCount--;
            if (buddyCount == 0)
                Die();
        }

        private void Die()
        {
            controller.Lose();
        }

        public void AddClone(Vector3 pos)
        {
            buddyCount++;
            SpawnBuddy(pos + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f)));
        }

        public void CollectGem()
        {
            controller.CollectGem();
        }

        public void Init(int startBuddyCount)
        {
            buddyCount = startBuddyCount;
        }

        public void Win()
        {
            foreach (Transform child in transform)
            {
                child.localScale *= Random.Range(5f, 9f);
                child.GetComponent<Animator>().SetTrigger(Hip);
            }
            controller.Win();
        }
    }
}