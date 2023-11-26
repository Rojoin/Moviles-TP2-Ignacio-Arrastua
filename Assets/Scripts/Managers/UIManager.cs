using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup gameOver;
    [SerializeField] private CanvasGroup PauseScreen;
    [SerializeField] private CanvasGroup TutorialScreen;
    [SerializeField] private TextMeshProUGUI diamonds;
    [SerializeField] public Button pauseLevel;
    [SerializeField] public Button continueLevel;
    [SerializeField] public Button restartLevel;
    [SerializeField] public Button exitLevel;
    [SerializeField] public Button restartLevelPause;
    [SerializeField] public Button exitLevelPause;

    public void Init()
    {
        SetCanvasVisibility(gameOver, false);
        SetCanvasVisibility(PauseScreen, false);
    }

    private void SetCanvasVisibility(CanvasGroup canvas, bool state)
    {
        canvas.alpha = state ? 1 : 0;
        canvas.interactable = state;
        canvas.blocksRaycasts = state;
        if (state)
        {
            Button currentbutton = canvas.GetComponentInChildren<Button>();
            if (currentbutton)
            {
                EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(currentbutton.gameObject);
            }
        }
    }

    public void ActivateGameOverCanvas(float currentDiamonds, float newDiamonds)
    {
        SetCanvasVisibility(gameOver, true);
        SetCanvasVisibility(PauseScreen, false);
        StartCoroutine(CountUpNumbers(currentDiamonds, newDiamonds));
    }

    private IEnumerator CountUpNumbers(float initialDiamonds, float newDiamonds)
    {
        float currentDiamonds = initialDiamonds;
        float maxDiamonds = initialDiamonds + newDiamonds;
        string diamondsTextValue = $"{currentDiamonds}";
        diamonds.text = diamondsTextValue;
        while (currentDiamonds < maxDiamonds)
        {
            yield return new WaitForSecondsRealtime(0.2f);
            currentDiamonds++;
            diamondsTextValue = $"{currentDiamonds}";
            diamonds.text = diamondsTextValue;
            yield return null;
        }

        yield return null;
    }

    public void TogglePauseMenu(bool isPaused)
    {
        SetCanvasVisibility(PauseScreen, isPaused);
    }
}