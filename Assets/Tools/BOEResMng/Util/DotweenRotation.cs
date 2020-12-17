
using System;
using System.Configuration;
using UnityEngine;
using System.Collections;
using DG.Tweening;
public class DotweenRotation : MonoBehaviour
{
    [SerializeField ]
    private  float duration=0.3f;

    [SerializeField] private float delay = 0;
     [SerializeField]
    private bool aroundx;
    [SerializeField]
    private bool aroundy;
    [SerializeField]
    private bool aroundz;
    [SerializeField]
    private bool reverse;
    [SerializeField]
    private bool loop=false;
    [SerializeField]
    private bool StartOnEnable = false;
    [SerializeField]
    private Vector3 From = Vector3.zero;
      [SerializeField]
    private Vector3 To=Vector3.zero;
    private int factor = 1;
    public Action OnRotateComplete;
    [SerializeField] private Ease EaseType = Ease.InOutSine;
    private Tweener tweener;
    void OnEnable()
	{
        if (tweener != null)
        {
            tweener.Kill();
        }
        if (StartOnEnable)
	    {
            DoForward();
	    }
	  
	}
    private void OnDisable()
    {
        if(tweener!=null)
        {
            tweener.Kill();
        }
    }
    public void DoForward()
    {
        DoRotation(From, To);
    }

    public void DoReverse()
    {
        DoRotation(To , From);
    }
    private void DoRotation(Vector3 from,Vector3 to)
    {
        if (reverse)
        {
            factor = -1;
        }
        else
        {
            factor = 1;
        }
        /*
        if (aroundx)
        {
            To = new Vector3(-180 * factor, 0, 0);
        }
        else if (aroundy)
        {
            To = new Vector3(0, -180 * factor, 0);
        }
        else if (aroundz)
        {
            To = new Vector3(0, 0, -180 * factor);
        }
        */
        Debug.Log(To);
        tweener = DOTween.To(() => from, x => transform.localEulerAngles = x, to , duration)
             .SetEase(EaseType)
             .SetDelay(delay)
             .SetUpdate(true);
         
        if (loop)
        {
            tweener.SetLoops(-1, LoopType.Incremental);
        }
        else
        {
            tweener.OnComplete(OnComplete);
        }
    }
    private void OnComplete()
    {
        if (OnRotateComplete != null)
        {
            OnRotateComplete();
        }
    }
	
}
