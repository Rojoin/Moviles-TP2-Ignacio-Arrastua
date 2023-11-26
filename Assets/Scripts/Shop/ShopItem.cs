using System;
using Managers;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Shop
{
    public class ShopItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI name;
        [SerializeField] private TextMeshProUGUI buttonText;
        [SerializeField] private Button button;
        public UnityEvent<ShopItem> onClick;
        [SerializeField] private Image buttonIcon;
        [SerializeField] public Image icon;
        private int price;
        public BladeSFX bladeSO;


        private void Awake()
        {
           button.onClick.AddListener(ButtonAction);
        }
        private void OnDestroy()
        {
            button.onClick.RemoveListener(ButtonAction);
        }

        private void ButtonAction()
        {
            onClick.Invoke(this);
        }
        public void Init(BladeSFX bladeSfx)
        {
            bladeSO = bladeSfx;
            name.text = bladeSO.name;
            price = bladeSO.price;
            icon.sprite = bladeSO.image;
        }

        

        public void SetBuyableItem(Sprite image)
        {
            buttonIcon.sprite = image;
            buttonText.text = $"{price}";
            button.interactable = true;
        }

        public void SetEquipable(Sprite image)
        {
            buttonIcon.sprite = image;
            buttonText.text = $"Equip";
            button.interactable = true;
        } 
        public void SetEquipable()
        {
            buttonText.text = $"Equip";
            button.interactable = true;
        }
        public void SetItemUsed(Sprite image)
        {
            buttonIcon.sprite = image;
            buttonText.text = $"Equipped";
            button.interactable = false;
        }
        public void SetItemUsed()
        {
            buttonText.text = $"Equipped";
            button.interactable = false;
        }
    }
}