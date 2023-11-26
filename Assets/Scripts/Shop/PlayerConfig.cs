using System;
using UnityEngine;

namespace Shop
{
    [CreateAssetMenu(menuName = "Create PlayerConfig", fileName = "PlayerConfig", order = 0)]
    public class PlayerConfig : ScriptableObject
    {
        public int Money
        {
            get => _money;
            set
            {
                _money = value;
                SaveCurrentMoney();
            }
        }
        [SerializeField] private int currentBlade = 0;
        [SerializeField] private int _money = 200;

        public int CurrentBlade
        {
            get => currentBlade;
            set
            {
                currentBlade = value;
                SaveCurrentBlade();
            }
        }

        public void Init()
        {
            Money = PlayerPrefs.GetInt("Money", Money);
            CurrentBlade = PlayerPrefs.GetInt("CurrentBlade", CurrentBlade);
        }

        private void SaveCurrentMoney()
        {
            if (PlayerPrefs.HasKey("Money"))
            {
                PlayerPrefs.SetInt("Money", Money);
            }
            else
            {
                PlayerPrefs.SetInt("Money", Money);
            }
        }

        private void SaveCurrentBlade()
        {
            PlayerPrefs.SetInt("CurrentBlade", CurrentBlade);
        }
    }
}