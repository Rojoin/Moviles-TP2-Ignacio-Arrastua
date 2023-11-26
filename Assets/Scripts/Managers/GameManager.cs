using System;
using Cuttables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private float currentTimer;
        private int currentLives;
        private int score = 0;
        [SerializeField] private float maxTimer = 60.0f;
        [SerializeField] private TextMeshProUGUI timer;
        [SerializeField] private TextMeshProUGUI scoreText;
        private bool isPlaying;
        private UnityEvent OnGameFinished;


        private void Awake()
        {
            currentTimer = maxTimer;
            isPlaying = true;
        }

        private void Update()
        {
            if (isPlaying)
            {
                currentTimer -= Time.deltaTime;
                TimeSpan timeSpan = TimeSpan.FromSeconds(currentTimer);
                timer.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
                if (currentTimer < 0)
                {
                    OnGameFinished.Invoke();
                    isPlaying = false;
                }
            }
        }

        public void AddPoint(GameObject cuttableItem)
        {
            score++;
            scoreText.text = $"Score:{score}";
            cuttableItem.GetComponent<CuttableItem>().OnCut.RemoveListener(AddPoint);
        }
        public void Explode(GameObject bomb)
        {
            isPlaying = false;
            OnGameFinished.Invoke();
        }
    }
}