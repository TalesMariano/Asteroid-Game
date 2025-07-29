using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IIntangible))]
public class VFXIntangibility : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private IIntangible intangible;
    private void Awake()
    {
        intangible = GetComponent<IIntangible>();
    }

    private void OnEnable()
    {
        intangible.OnChangeIntangible += ChangeVisual;
    }

    private void OnDisable()
    {
        intangible.OnChangeIntangible -= ChangeVisual;
    }

    private void ChangeVisual(bool isIntangible)
    {
        Color color = Color.white;

        color.a = isIntangible ? 0.5f : 1;

        spriteRenderer.color = color;
    }
}
