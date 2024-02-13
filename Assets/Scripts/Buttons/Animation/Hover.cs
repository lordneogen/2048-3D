using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Hover : MonoBehaviour
{
    public float hoverAlpha = 0.5f;
    public float normalAlpha = 1f;
    private Renderer _renderer;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnMouseEnter()
    {
        _renderer.material.DOFade(hoverAlpha, 0.1f);
    }

    private void OnMouseExit()
    {
        _renderer.material.DOFade(normalAlpha, 0.1f);
    }
}