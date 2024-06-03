using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class BigFruitsSystem:MonoBehaviour,ISystem
{
    public float Amplitude;
    public float Second;
    public float LoadingTime=2f;

    IEnumerator LoadingTimeWait()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOBlendableMoveBy(new Vector3(0, -Amplitude, 0), 0).SetEase(Ease.InOutSine));
        yield return new WaitForSeconds(LoadingTime);
        sequence.Append(transform.DOBlendableMoveBy(new Vector3(0, Amplitude, 0), Second/2).SetEase(Ease.InOutSine));
        sequence.SetLoops(1);
    }
    private void Start()
    {
        StartCoroutine(LoadingTimeWait());
        EventManager.Instance.Systems.Add(this);
    }
    
    [ContextMenu("UpDown")]
    public void UpDown()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOBlendableMoveBy(new Vector3(0, -Amplitude, 0), Second).SetEase(Ease.InOutSine));
        sequence.Append(transform.DOBlendableMoveBy(new Vector3(0, Amplitude, 0), Second/2).SetEase(Ease.InOutSine));
        sequence.SetLoops(1);
    }
    
    public void Down(Action action)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOBlendableMoveBy(new Vector3(0, -Amplitude, 0), Second).SetEase(Ease.InOutSine));
        // sequence.Append(transform.DOBlendableMoveBy(new Vector3(0, Amplitude, 0), Second/2).SetEase(Ease.InOutSine));
        sequence.SetLoops(1);
    }
    public void Continue()
    {
        UpDown();
    }

    public void GameOver()
    {
        return;
    }

    public void Restart()
    {
        // StartCoroutine(LoadingTimeWait());
        UpDown();
    }
}