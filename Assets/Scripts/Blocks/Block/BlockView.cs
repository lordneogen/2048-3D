using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BlockView:MonoBehaviour
{
    [Header("Текст")]
    [SerializeField] private List<GameObject> _textMeshPros;
    [Header("Цвета")]
    [SerializeField] private List<Color> _colors;
    [SerializeField] private Color _disable;
    [SerializeField] private int indexColor;
    public BlockAnimation _BlockAnimation;
    public Renderer Renderer;
    public ParticleSystem Freeze;
    private void Start()
    {
        _BlockAnimation.delay = EventManager.Instance.Speed;
        InvokeRepeating("UpdateTextRotation",0,0.1f);
        UpdateColor();
    }

    public void UpdateColor()
    {
        _BlockAnimation._start = GetComponent<MeshRenderer>().material.color;
    }
    private void UpdateTextRotation()
    {
        for (int i = 0; i < _textMeshPros.Count; i++)
        {
                Vector3 result = Camera.main.transform.eulerAngles;
                Vector3 initial = _textMeshPros[i].transform.rotation.eulerAngles;
                result.x = initial.x;
                result.y = initial.y;
                _textMeshPros[i].transform.rotation = Quaternion.Euler(result);
        }
    }

    private void SetTextNumber(string text)
    {
        for (int i = 0; i < _textMeshPros.Count; i++)
        {
            _textMeshPros[i].GetComponent<TextMeshPro>().SetText(text); 
        }
    }

    private void SetColorIndex()
    {
        Renderer.material.color = _colors[indexColor];
    }

    public void IncreaseView(int num)
    {
        indexColor = (int)Math.Log(num, 2);
        UpdateColor();
        SetTextNumber(num.ToString());
        SetColorIndex();
        EventManager.Instance.audioManager.Play(EventManager.Instance.audioManager.ScoreUPClip);
        _BlockAnimation._start=_BlockAnimation._start = GetComponent<MeshRenderer>().material.color;
        _BlockAnimation.Shake(transform,0);
    }

    public void DisableView()
    {
        SetTextNumber("x");
        EventManager.Instance.audioManager.Play(EventManager.Instance.audioManager.FrezeClip);
        Renderer.material.color = _disable;
        _BlockAnimation._start=_BlockAnimation._start = GetComponent<MeshRenderer>().material.color;
        _BlockAnimation.Shake(transform,0);
        Freeze.Play();
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one*0.24f, 0.1f);
    }
}