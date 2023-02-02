using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Dreamteck.Splines;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DrawAndRun
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private SplineFollower camSplineFollower, buddyBoxSplineFollower;
        [SerializeField] private LineDrawer drawer;
        [SerializeField] private GameObject drawLabel;
        [SerializeField] private GameObject restartButton;
        [SerializeField] private GameObject nextButton;
        [SerializeField] private TMP_Text gemCounterText;
        [SerializeField] private TMP_Text levelLabel;
        [SerializeField] private List<LevelInfo> levels;
        [SerializeField] private BuddyBox buddyBox;

        private static int _levelIndex;
        private static int _gemCount;

        private float _endPercent;

        private void Start()
        {
            gemCounterText.text = $"{_gemCount}";
            drawer.OnEndDraw += Play;
            levelLabel.text = $"Level {_levelIndex + 1}";
            InitLevel(levels[_levelIndex]);
        }

        private void InitLevel(LevelInfo level)
        {
            buddyBox.Init(level.startBuddyCount);
            
            camSplineFollower.spline = level.computer;
            buddyBoxSplineFollower.spline = level.computer;

            var initializer = GetComponent<LevelInitializer>();
            initializer.InitializeLevel(level);
            _endPercent = initializer.EndPercent;
        }

        private float _followSpeed;

        private float FollowSpeed
        {
            set => camSplineFollower.followSpeed = buddyBoxSplineFollower.followSpeed = value;
        }

        private void Play(List<Vector2> _, Vector2 __)
        {
            drawer.OnEndDraw -= Play;
            drawer = null;
            print("Play!");
            FollowSpeed = 5;
            drawLabel.SetActive(false);
            restartButton.SetActive(false);
            nextButton.SetActive(false);

            StartCoroutine(Delay(() => camSplineFollower.result.percent > _endPercent, Win));
        }

        public void Lose()
        {
            FollowSpeed = 0;
            restartButton.SetActive(true);
        }

        public void RestartGame()
        {
            DOTween.KillAll();
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
            gemCounterText.text = $"{_gemCount}";
            
        }

        private void Win()
        {
            FollowSpeed = 0;
            buddyBox.Win();
            nextButton.SetActive(true);
        }

        private static IEnumerator Delay(Func<bool> pred, Action action)
        {
            yield return new WaitUntil(pred);
            action();
        }


        public void LoseBuddy()
        {
            buddyBox.LoseBuddy();
        }
    }
}