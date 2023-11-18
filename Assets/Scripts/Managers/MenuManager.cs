using System;
using CustomSceneSwitcher.Switcher;
using CustomSceneSwitcher.Switcher.Data;
using Cuttables;
using UnityEngine;

namespace Managers
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private SceneChangeData gameScene;
        [SerializeField]  private CuttableItem _playCuttableItem;

        private void Awake()
        {
                _playCuttableItem.OnCut.AddListener(ChangeToPlayScene);
        }

        private void OnDestroy()
        {
            _playCuttableItem.OnCut.RemoveAllListeners();
        }

        private void ChangeToPlayScene(GameObject arg0)
        {
            Invoke(nameof(ChangeGameplay),2.0f);
        }

        private void ChangeGameplay()
        {
            SceneSwitcher.ChangeScene(gameScene);
        }
    }
}