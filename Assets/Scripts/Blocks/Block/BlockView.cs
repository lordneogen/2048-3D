using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BlockView:MonoBehaviour
{
    [SerializeField] private List<GameObject> _textMeshPros;
    [SerializeField] private List<Color> _colors;
    [SerializeField] private int indexColor = 0;
    [HideInInspector] public Renderer Renderer;

    private void Start()
    {
        Renderer = GetComponent<Renderer>();
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one*0.24f, 0.1f);
    }

    private void Update()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        for (int i = 0; i < _textMeshPros.Count; i++)
        {
            Vector3 result = Camera.main.transform.eulerAngles;
            Vector3 initial = _textMeshPros[i].transform.rotation.eulerAngles;
            result.x = initial.x;
            result.y = initial.y;
            // result.z = initial.z;
            _textMeshPros[i].transform.rotation = Quaternion.Euler(result);
        }
    }

    private void SetText(string text)
    {
        for (int i = 0; i < _textMeshPros.Count; i++)
        { 
            _textMeshPros[i].GetComponent<TextMeshPro>().SetText(text);
        }
    }

    private void SetColor()
    {
        indexColor += 1;
        Renderer.material.color = _colors[indexColor];
        // material.color = _colors[indexColor];
    }

    public void Increase(int num)
    {
        SetText(num.ToString());
        SetColor();
    }
}