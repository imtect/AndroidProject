using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BOE.BOEComponent.Timer
{
    public class CoreTimer
    {
        private float time;
        private float cur;
        private bool timerFlag = false;
        private bool repeat = false;
        private Action callback;
        public CoreTimer(float time)
        {
            cur = 0;
            this.time = time;
        }
        public void AddListener(Action action)
        {
            this.callback = action;
        }
        public void Update()
        {
            if (timerFlag)
            {
                if (cur >= time)
                {
                    cur = 0;
                    callback?.Invoke();
                    if (!repeat)
                    {
                        Stop();
                    }
                }
                else
                {
                    cur += Time.deltaTime;
                }
            }
        }

        public void Start()
        {
            cur = 0;
            repeat = false;
            Resume();
            CoreTimerMgr.Instance.StartTimer(this);
        }
        public void StartRepeat()
        {
            Start();
            repeat = true;
        }
        public void Stop()
        {
            Pause();
            CoreTimerMgr.Instance.StopTimer(this);
        }

        public void Pause()
        {
            this.timerFlag = false;
        }
        public void Resume()
        {
            this.timerFlag = true;
        }
    }

    internal class CoreTimerMgr : DestroyableSingleton<CoreTimerMgr>
    {

        private List<CoreTimer> cache = new List<CoreTimer>();

        private void Update()
        {
            foreach(var t in cache)
            {
                t.Update();
            }
        }
        public void StartTimer(CoreTimer timer)
        {
            if(!cache.Contains(timer))
                cache.Add(timer);

        }
        public void StopTimer(CoreTimer timer)
        {
            if (cache.Contains(timer))
                cache.Remove(timer);
        }
    }
}