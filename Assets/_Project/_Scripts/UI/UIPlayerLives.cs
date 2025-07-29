using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class UIPlayerLives : MonoBehaviour
{
    [SerializeField] private Image uiImage;
    [SerializeField] private Sprite lifeSprite;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnPlayerLivesChange += UpdateLifeAmount;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnPlayerLivesChange -= UpdateLifeAmount;
    }

    private void Start()
    {
        uiImage.sprite = lifeSprite;
    }

    private void UpdateLifeAmount(int amount)
    {
        Vector2 sd = new Vector2(lifeSprite.texture.height * amount, lifeSprite.texture.width);

        rectTransform.sizeDelta = sd;
    }
}
