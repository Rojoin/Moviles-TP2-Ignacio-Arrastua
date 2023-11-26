using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class StoreManager : MonoBehaviour
    {
        [SerializeField] private ShopItem[] _shopItems;
        [SerializeField] private BladeSFX[] _bladeSO;
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private Sprite blockStore;
        [SerializeField] private Sprite availableStore;
        [SerializeField] private Sprite inUseStore;
        [SerializeField] private TextMeshProUGUI diamonds;

        private void Awake()
        {
            diamonds.text = $"{_playerConfig.Money}";
            _playerConfig.Init();
            LoadShopItem();
            var maxBlades = _bladeSO.Length;
            for (int i = 0; i < maxBlades; i++)
            {
                ShopItem currentShopItem = _shopItems[i];
                currentShopItem.Init(_bladeSO[i]);
                if (_bladeSO[i].isAvalaible)
                {
                    currentShopItem.SetEquipable(availableStore);
                    currentShopItem.onClick.AddListener(SelectItem);
                    if (_playerConfig.CurrentBlade == currentShopItem.bladeSO.id)
                    {
                        currentShopItem.SetItemUsed();
                    }
                }
                else
                {
                    currentShopItem.SetBuyableItem(blockStore);
                    currentShopItem.onClick.AddListener(BuyItem);
                }
            }
        }

        private void Start()
        {
            GooglePlayController.UnlockAchievement(GPGSIds.achievement_open_the_game);
        }

        private void OnDestroy()
        {
            foreach (ShopItem shopItem in _shopItems)
            {
                shopItem.onClick.RemoveAllListeners();
            }
        }

        private void LoadShopItem()
        {
            foreach (var blade in _bladeSO)
            {
                if (PlayerPrefs.HasKey(blade.name))
                {
                    blade.isAvalaible = PlayerPrefs.GetInt(blade.name) == 1;
                }
                else
                {
                    PlayerPrefs.SetInt(blade.name, blade.isAvalaible ? 1 : 0);
                }
            }
        }

        private void BuyItem(ShopItem shopItem)
        {
            if (_playerConfig.Money >= shopItem.bladeSO.price)
            {
                _playerConfig.Money -= shopItem.bladeSO.price;
                shopItem.bladeSO.isAvalaible = true;
                PlayerPrefs.SetInt(shopItem.bladeSO.name, 1);
                shopItem.onClick.RemoveListener(BuyItem);
                shopItem.onClick.AddListener(SelectItem);
                shopItem.SetEquipable();

                GooglePlayController.UnlockAchievement(GPGSIds.achievement_first_buy);

                diamonds.text = $"{_playerConfig.Money}";
                foreach (BladeSFX bladeSfxe in _bladeSO)
                {
                    if (!bladeSfxe.isAvalaible)
                    {
                        return;
                    }
                }
                GooglePlayController.UnlockAchievement(GPGSIds.achievement_blade_collector);
            }
        }

        private void SelectItem(ShopItem shopItem)
        {
            foreach (ShopItem item in _shopItems)
            {
                if (item.bladeSO.id == _playerConfig.CurrentBlade)
                {
                    item.onClick.AddListener(SelectItem);
                    item.SetEquipable();
                }
            }

            _playerConfig.CurrentBlade = shopItem.bladeSO.id;
            shopItem.onClick.RemoveListener(SelectItem);
            shopItem.SetItemUsed();
        }
    }
}