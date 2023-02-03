using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using Dreamteck.Splines;
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
        [SerializeField] private float buddyRandomOffset;
        [SerializeField] private Vector3 buddyOffset;
        [SerializeField] private GameController controller;

        [SerializeField] private int rowBuddiesCount = 5;
        [SerializeField] private float winScale = 1;
        [SerializeField] private float moveToDanceDuration = 1;

        private bool _won;

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
            if (_won || buddyCount == 0)
                return;
            buddyBox.Clear();
            while (positions.Count < buddyCount)
                positions.Add(Random.insideUnitCircle * buddyRandomOffset);
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
        }

        public void AddClone(Vector3 pos)
        {
            buddyCount++;
            SpawnBuddy(pos - transform.position);
        }

        public void Init(int startBuddyCount)
        {
            buddyCount = startBuddyCount;
        }

        public void Win()
        {
            AlignBuddiesToWin();
            _won = true;
            GetComponent<SplineFollower>().enabled = false;
           transform.localEulerAngles = 180 * Vector3.up;
        }

        public void AlignBuddiesToWin()
        {
            var dx = (1 - rowBuddiesCount % 2) * 0.5f - rowBuddiesCount / 2;
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                child.GetComponent<Animator>().SetTrigger(Hip);
                child
                    .DOLocalMove(winScale * new Vector3(i % rowBuddiesCount + dx, 0, -i / rowBuddiesCount),
                        moveToDanceDuration)
                    .Play();
            }
        }

        public void LoseBuddy()
        {
            buddyCount--;
            if (buddyCount == 0)
                controller.Lose();
        }
    }
}