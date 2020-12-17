using System;
using DG.Tweening;
using UnityEngine;
using System.Collections;

public class DotweenMove : MonoBehaviour {

    [SerializeField]
    private float duration = 1.0f;
    [SerializeField]
    private float _delay = 0;
    [SerializeField]
    private Vector3 From = new Vector3(1, 1, 1);
    [SerializeField]
    private Vector3 To = new Vector3(1, 1, 1);
    [SerializeField]
    private bool StartOnEnable = true;
    [SerializeField]
    private bool _isWorld = false;
    [SerializeField]
    private bool _loop = false;
    [SerializeField] private Ease EaseType = Ease.InOutSine;
    [SerializeField] private LoopType loopType;
    private Tweener _tween;
    public Action OnMoveComplete;

    void OnEnable()
    {
        if (StartOnEnable)
        {
            StartTween();
        }
    }
    private void OnDisable()
    {
        if(_tween!=null)
        {
            _tween.Kill();
        }
    }
    public void StartTween()
    {
        if (_isWorld)
        {
            _tween = DOTween.To(() => From, x => transform.position = x, To, duration)
                .SetEase(EaseType)
                .SetDelay(_delay)
                .OnComplete(Complete);
        }
        else
        {
            _tween = DOTween.To(() => From, x => transform.localPosition = x, To, duration)
               .SetEase(EaseType)
                .SetDelay(_delay)
               .OnComplete(Complete);
        }
        if (_loop)
        {
            _tween.SetLoops(-1, loopType);
        }
    }
    public void DoBackTween()
    {
        if (_isWorld)
        {
            DOTween.To(() => To , x => transform.position = x, From , duration)
                .SetEase(EaseType)
                 .SetDelay(_delay)
                .OnComplete(Complete);
        }
        else
        {
            DOTween.To(() => To, x => transform.localPosition = x, From , duration)
               .SetEase(EaseType)
                .SetDelay(_delay)
               .OnComplete(Complete);
        }

    }

    public void SetTweemConfigure(Vector3 from, Vector3 to, float twduration,float delay)
    {
        From = from;
        To = to;
        duration = twduration;
        _delay = delay;
    }

    public void SetTweemConfigure( float delay)
    {
        _delay = delay;
    }

    private void Complete()
    {
        if (OnMoveComplete != null)
        {
            OnMoveComplete();
        }
    }

    public void Pause()
    {
        _tween.Pause();
    }

    public void Continue()
    {
        _tween.Play();
    }
}
