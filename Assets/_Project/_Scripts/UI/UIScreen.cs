using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIScreen : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    [SerializeField] GameState screenGameState;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }


    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChange += SetScreenDisplay;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStateChange -= SetScreenDisplay;
    }


    private void SetScreenDisplay(GameState gameState)
    {
        _canvasGroup.alpha = screenGameState == gameState ? 1 : 0;
    }
}
