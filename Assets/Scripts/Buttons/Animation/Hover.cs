using UnityEngine;
using DG.Tweening;

public class Hover : MonoBehaviour
{
    public float hoverAlpha = 0.5f;
    public float normalAlpha = 1f;
    private Renderer _renderer;
    public float duration = 1f;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnMouseEnter()
    {
        _renderer.material.DOFade(hoverAlpha, duration);
    }

    private void OnMouseExit()
    {
        _renderer.material.DOFade(normalAlpha, duration);
    }
}