using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ShaderVariableTw : MonoBehaviour
{

    [SerializeField]  private Material mat;
    [SerializeField]  private string shaderVariable;

	// Use this for initialization
   [SerializeField ]    float From=0.0f;
   [SerializeField]     float To=1.0f;
   [SerializeField] float duration=2.0f;
   [SerializeField]
   private LoopType loopType=LoopType.Yoyo ;
        float percent;
	void Start () {
        DoTw();
	}
	
	// Update is called once per frame
	void Update () {
        mat.SetFloat(shaderVariable ,percent);
	}
    private void DoTw()
    {
        Tweener tw = DOTween.To(() => From, x => percent = x, To, duration).SetLoops(-1, loopType);
    }

}
