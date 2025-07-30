using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScore : MonoBehaviour
{
    private TMP_Text uiText;

    private void Awake()
    {
        uiText = GetComponent<TMP_Text>();
    }


    private void OnEnable()
    {
        GameManager.Instance.OnScore += UpdateTextScore;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnScore += UpdateTextScore;
    }

    void UpdateTextScore(int score)
    {
        uiText.text = score.ToString();
    }
}
