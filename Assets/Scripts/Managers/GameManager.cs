using System;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private float currentTimer;
        private int currentLives;
        [SerializeField] private int maxLives = 3;
        [SerializeField] private float maxTimer = 1.0f;
        private bool isPlaying;
        private UnityEvent OnGameFinished;


        private void Awake()
        {
            currentTimer = 0;
            currentLives = maxLives;
        }

        private void Update()
        {
            if (isPlaying)
            {
                currentTimer += Time.deltaTime;
                if (currentTimer > maxTimer)
                {
                    OnGameFinished.Invoke();
                    isPlaying = false;
                }
            }
        }
    }
}