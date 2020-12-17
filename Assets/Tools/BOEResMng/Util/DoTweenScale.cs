using System;
using UnityEngine;
using System.Collections;
using  DG.Tweening;
public class DoTweenScale : MonoBehaviour {

    [SerializeField] private float _delay = 0;
    [SerializeField] private  float duration=1.0f;
    [SerializeField] private Vector3 From=new Vector3(1,1,1);
    [SerializeField] private Vector3 To = new Vector3(1, 1, 1);
    [SerializeField] private bool StartOnEnable = true;
    [SerializeField] private bool _loop = false;
    public Action OnScaleComplete;
    [SerializeField] private Ease EaseType = Ease.InOutSine;
    void OnEnable()
    {
        if (StartOnEnable)
        {
             StartTween();
        }
    }

    public  void StartTween()
    {
        Tweener tw = DOTween.To(() => From, x => transform.localScale = x, To, duration)
            .SetDelay(_delay)
            .SetEase(EaseType);
        if (_loop)
        {
            tw.SetLoops(-1,LoopType.Restart);
        }
        else
        {
            tw.OnComplete(Complete);
        }
    }

    public void DoBackTween()
    {
        Tweener tw = DOTween.To(() => To, x => transform.localScale = x, From, duration)
           .SetEase(Ease.Linear);
        if (_loop)
        {
            tw.SetLoops(-1, LoopType.Restart);
        }
        else
        {
            tw.OnComplete(Complete);
        }
    }
    private void Complete()
    {
        if (OnScaleComplete != null)
        {
            OnScaleComplete();
        }
    }
}
