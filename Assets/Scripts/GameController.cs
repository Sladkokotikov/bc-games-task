using System.Collections.Generic;
using Dreamteck.Splines;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DrawAndRun
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private SplineFollower cam, buddyBox;
        [SerializeField] private LineDrawer drawer;
        [SerializeField] private GameObject drawLabel;
        [SerializeField] private GameObject restart;
        [SerializeField] private GameObject next;
        [SerializeField] private TMP_Text gemCounter;
        [SerializeField] private TMP_Text levelLabel;
        [SerializeField] private List<LevelInfo> levels;
        [SerializeField] private BuddyBox box;

        private static int _levelIndex;
        private static int _gemCount;

        private void Start()
        {
            gemCounter.text = $"{_gemCount}";
            drawer.OnEndDraw += Play;
            levelLabel.text = $"Level {_levelIndex + 1}";
            InitLevel(levels[_levelIndex]);
            
        }

        private void InitLevel(LevelInfo level)
        {
            box.Init(level.startBuddyCount);
            
            cam.spline = level.computer;
            buddyBox.spline = level.computer;
            
            GetComponent<LevelInitializer>().InitializeLevel(level);
        }

        private void Play(List<Vector2> _, Vector2 __)
        {
            drawer.OnEndDraw -= Play;
            drawer = null;
            print("Play!");
            cam.followSpeed = 5;
            buddyBox.followSpeed = 5;
            drawLabel.SetActive(false);
            restart.SetActive(false);
            next.SetActive(false);
        }

        public void Lose()
        {
            cam.enabled = false;
            buddyBox.enabled = false;
            restart.SetActive(true);
        }
        
        public void Win()
        {
            cam.enabled = false;
            buddyBox.enabled = false;
            next.SetActive(true);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(0);
        }

        public void NextLevel()
        {
            if (_levelIndex + 1 < levels.Count)
                _levelIndex++;
            RestartGame();
        }

        public void CollectGem()
        {
            _gemCount++;
            gemCounter.text = $"{_gemCount}";
        }
    }
}