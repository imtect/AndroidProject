using System;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

namespace BOE.BOEComponent.Util
{
    public class Dotweentool
    {
        public static Tweener CurTweener;
        public static Action OnMoveComplete;
        public static Action OnScaleComplete;
        public static Action OnRotateComplete;
        public static void Move(Transform tweenTransform, bool isworld, Vector3 from, Vector3 to, float duration, float delay, Ease ease = Ease.InOutSine)
        {
            if (isworld)
            {

                CurTweener = DOTween.To(() => from, x => tweenTransform.position = x, to, duration)
                      .SetEase(ease)
                      .SetDelay(delay)
                      .OnComplete(MoveComplete).SetAutoKill();
            }
            else
            {

                CurTweener = DOTween.To(() => from, x => tweenTransform.localPosition = x, to, duration)
                    .SetEase(ease)
                        .SetDelay(delay)
                    .OnComplete(MoveComplete).SetAutoKill();
            }
        }

        private static void MoveComplete()
        {
            if (OnMoveComplete != null)
            {
                OnMoveComplete();
                OnMoveComplete = null;
            }
        }

        public static void Scale(Transform tweenTransform, Vector3 from, Vector3 to, float duration, float delay)
        {

            CurTweener = DOTween.To(() => from, x => tweenTransform.localScale = x, to, duration)
                    .SetEase(Ease.Linear)
                    .SetDelay(delay)
                    .OnComplete(ScaleComplete).SetAutoKill();
        }

        private static void ScaleComplete()
        {
            if (OnScaleComplete != null)
            {
                OnScaleComplete();
                OnScaleComplete = null;
            }
        }


        public static void Rotate(Transform tweenTransform, bool isworld, Vector3 from, Vector3 to, float duration, float delay, bool loop = false, Ease easeType = Ease.Linear)
        {
            
            var _from = new Vector3(from.x % 360, from.y % 360, from.z % 360);
            var _to= new Vector3(to .x % 360, to.y % 360, to .z % 360);
            if (isworld)
            {

                Tweener tw1 = DOTween.To(() => _from, x => tweenTransform.eulerAngles = x, _to, duration)
                    .SetEase(Ease.Linear)
                    .SetDelay(delay)
                    .OnComplete(RotateComplete).SetAutoKill();
                if (loop)
                {
                    tw1.SetLoops(-1, LoopType.Incremental);
                }
                CurTweener = tw1;
            }
            else
            {

                Tweener tw2 = DOTween.To(() => _from, x => tweenTransform.localEulerAngles = x, _to, duration)
                    .SetEase(Ease.Linear)
                    .SetDelay(delay);

                if (loop)
                {
                    tw2.SetLoops(-1, LoopType.Incremental);
                }
                else
                {
                    tw2.OnComplete(RotateComplete).SetAutoKill();
                }
                CurTweener = tw2;
            }

        }

        private static void RotateComplete()
        {
            if (OnRotateComplete != null)
            {
                OnRotateComplete();
                OnRotateComplete = null;
            }
        }
    }
}
