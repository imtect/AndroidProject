using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BOE.BOEComponent.Uitl
{


    public class MatTilingOffsetTW : MonoBehaviour
    {
        [SerializeField] private Material mat;
        //  [SerializeField] private Vector2 tilingSpeed=new Vector2(1,1);
        [SerializeField] private Vector2 offsetSpeed = new Vector2(1, 1);
        [SerializeField] private float duration = 1.0f;
        [SerializeField] private LoopType loopType = LoopType.Yoyo;
        [SerializeField] private Ease easeType = Ease.Linear;
        [SerializeField] private bool StartOnEnable = false;
        Vector2 offset;

        // Start is called before the first frame update
        void Start()
        {
            if(StartOnEnable)
            {
                DoTw();
            }
       
        }

        // Update is called once per frame
        void Update()
        {
            mat.SetTextureOffset("_MainTex", offset);
        }

        public  void DoTw()
        {

            Tweener tw = DOTween.To(() => Vector2.zero, x => offset = x, offsetSpeed, duration).SetLoops(-1, loopType);
        }
    }
}
