using System;
using CustomSceneSwitcher.Switcher;
using CustomSceneSwitcher.Switcher.Data;
using Cuttables;
using Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private SceneChangeData gameScene;
        [SerializeField] private SceneChangeData debugScene;
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button debugButton;
        [SerializeField] private Button storeButton;
        [SerializeField] private Button backStoreButton;
        [SerializeField] private Button creditsButton;
        [SerializeField] private Button backCreditsButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private CanvasGroup menuCanvas;
        [SerializeField] private CanvasGroup storeCanvas;
        [SerializeField] private CanvasGroup creditsCanvas;
      
        private bool isCreditsActive;
        private bool isStoreActive;

        private void Awake()
        {
            startGameButton.onClick.AddListener(ChangeGameplay);
            debugButton.onClick.AddListener(ChangeToDebug);
            exitButton.onClick.AddListener(ExitGame);
            storeButton.onClick.AddListener(StoreToggle);
            backStoreButton.onClick.AddListener(StoreToggle);
            creditsButton.onClick.AddListener(CreditsToggle);
            backCreditsButton.onClick.AddListener(CreditsToggle);
        }

        private void OnDestroy()
        {
            startGameButton.onClick.RemoveListener(ChangeGameplay);
            debugButton.onClick.RemoveListener(ChangeToDebug);
            exitButton.onClick.RemoveListener(ExitGame);
            storeButton.onClick.RemoveListener(StoreToggle);
            backStoreButton.onClick.RemoveListener(StoreToggle);
            creditsButton.onClick.RemoveListener(CreditsToggle);
            backCreditsButton.onClick.RemoveListener(CreditsToggle);
        }

    

        private void ChangeGameplay()
        {
            SceneSwitcher.ChangeScene(gameScene);
            startGameButton.interactable = false;
        }

        private void ChangeToDebug()
        {
            SceneSwitcher.ChangeScene(debugScene);
            debugButton.interactable = false;
        }

        private void SetCanvasVisibility(CanvasGroup canvas, bool state)
        {
            canvas.alpha = state ? 1 : 0;
            canvas.interactable = state;
            canvas.blocksRaycasts = state;
        }

        private void ExitGame()
        {
            Application.Quit();
        }

        private void StoreToggle()
        {
            isStoreActive = !isStoreActive;
            SetCanvasVisibility(storeCanvas, isStoreActive);
            menuCanvas.blocksRaycasts = !isStoreActive;
        }

        private void CreditsToggle()
        {
            isCreditsActive = !isCreditsActive;
            SetCanvasVisibility(creditsCanvas, isCreditsActive);
            menuCanvas.blocksRaycasts = !isCreditsActive;
        }
    }
}