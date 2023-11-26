using System;
using CustomSceneSwitcher.Switcher;
using CustomSceneSwitcher.Switcher.Data;
using Cuttables;
using Shop;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private UIManager _uIManager;
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private CuttableSpawner _cuttableSpawner;
        private float currentTimer;
        private int currentLives;
        private int score = 0;
        [SerializeField] private float maxTimer = 60.0f;
        [SerializeField] private float timeBeforeGameStarts = 3.0f;
        [SerializeField] private TextMeshProUGUI timer;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI diamondText;
        private bool isPlaying = false;
        private bool isPaused = false;
       [SerializeField] private int minimunForTrophy = 300;
        private UnityEvent OnGameFinished;
        private int moneyToAdd;

        [SerializeField] private SceneChangeData mainMenu;
        [SerializeField] private SceneChangeData currentScene;


        private void Awake()
        {
            currentTimer = maxTimer;
            diamondText.text = $"Diamonds: {_playerConfig.Money}";
            _uIManager.pauseLevel.onClick.AddListener(PauseLevel);
            _uIManager.continueLevel.onClick.AddListener(PauseLevel);
            _uIManager.restartLevel.onClick.AddListener(RestartLevel);
            _uIManager.restartLevelPause.onClick.AddListener(RestartLevel);
            _uIManager.exitLevelPause.onClick.AddListener(GoToMenu);
            _uIManager.exitLevel.onClick.AddListener(GoToMenu);
            Invoke(nameof(StartGame), timeBeforeGameStarts);
        }

        private void OnDestroy()
        {
            _uIManager.pauseLevel.onClick.RemoveListener(PauseLevel);
            _uIManager.continueLevel.onClick.RemoveListener(PauseLevel);
            _uIManager.restartLevel.onClick.RemoveListener(RestartLevel);
            _uIManager.exitLevel.onClick.RemoveListener(GoToMenu);
            _uIManager.restartLevelPause.onClick.RemoveListener(RestartLevel);
            _uIManager.exitLevelPause.onClick.RemoveListener(GoToMenu);
        }

        private void StartGame()
        {
            isPlaying = true;
        }

        private void Update()
        {
            if (isPlaying)
            {
                currentTimer -= Time.deltaTime;
                TimeSpan timeSpan = TimeSpan.FromSeconds(currentTimer);
                timer.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
                moneyToAdd = score / 2;
                if (currentTimer < 0)
                {
                    GameOver();
                }
            }
        }

        private void GameOver()
        {
            _uIManager.ActivateGameOverCanvas(_playerConfig.Money, moneyToAdd);
            _playerConfig.Money += moneyToAdd;
            if (_playerConfig.Money >= minimunForTrophy)
            {
                GooglePlayController.UnlockAchievement(GPGSIds.achievement_diamond_king);
            }
            if (SystemInfo.supportsVibration)
            {
                Handheld.Vibrate();
            }
            isPlaying = false;
            _cuttableSpawner.enabled = false;
            Time.timeScale = 0;
        }

        public void AddPoint(GameObject cuttableItem)
        {
            score++;
            scoreText.text = $"Score:{score}";
            cuttableItem.GetComponent<CuttableItem>().OnCut.RemoveListener(AddPoint);
        }

        public void Explode(GameObject bomb)
        {
            GameOver();
        }

        private void PauseLevel()
        {
            isPaused = !isPaused;
            _uIManager.TogglePauseMenu(isPaused);
            Time.timeScale = isPaused ? 0 : 1;
        }

        public void RestartLevel()
        {
            Time.timeScale = 1;
            SceneSwitcher.ChangeScene(currentScene);
        }

        public void GoToMenu()
        {
            Time.timeScale = 1;
            SceneSwitcher.ChangeScene(mainMenu);
        }
    }
}