using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BOE.BOEComponent.Uitl
{
    public class ShaderColorTwRepeating : MonoBehaviour
    {

        [SerializeField] private Material mat;
        [SerializeField] private string shaderVariable;

        // Use this for initialization
        [SerializeField] Color From;
        [SerializeField] Color To;
        [SerializeField] private float repeatRate = 2.0f;
        [SerializeField] private float _delay = 0;
        [SerializeField] float duration = 2.0f;
        [SerializeField] private Ease EaseType = Ease.InOutSine;
        //  [SerializeField]
        //   private LoopType loopType = LoopType.Yoyo;

        Color percent;
        void Start()
        {

            InvokeRepeating("DoTw", 0, repeatRate);

        }
        private void OnDisable()
        {
            CancelInvoke();
        }
        // Update is called once per frame
        void Update()
        {
            mat.SetColor(shaderVariable, percent);
        }
        private void DoTw()
        {
            percent = From;
            Tweener tw = DOTween.To(() => From, x => percent = x, To, duration)
                  .SetDelay(_delay)
                .SetEase(EaseType);
            //.SetLoops(-1, loopType);
        }
    }
}
