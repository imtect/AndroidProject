using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace BOE.BOEComponent.Util
{
    public class AudioManager : MonoBehaviour
    {
        private readonly Dictionary<AudioType, AudioClip> _audioClips = new Dictionary<AudioType, AudioClip>();

        private static AudioManager _instance;
        static AudioSource mAudioSource;
        /// <summary>
        /// Singleton,方便各模块访问
        /// </summary>
        public static AudioManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var manager = new GameObject("AudioManager");
                    DontDestroyOnLoad(manager);
                    _instance = manager.AddComponent<AudioManager>();
                    mAudioSource = manager.AddComponent<AudioSource>();
                }

                return _instance;
            }
        }

        public void Play(AudioType audioType)
        {
            //var go = GameObject.Find("One shot audio");
            //if(null!=go)DestroyImmediate(go);
            StartCoroutine(DelayPlay(audioType));
        }

        private IEnumerator DelayPlay(AudioType audioType)
        {
            if (!_audioClips.ContainsKey(audioType))
            {
                var request = Resources.LoadAsync(string.Format("Media/Audio{0}", audioType));
                yield return request;
                var clip = request.asset as AudioClip;
                if (!_audioClips.ContainsKey(audioType))
                {
                    _audioClips.Add(audioType, clip);
                }
            }
            mAudioSource.Stop();
            mAudioSource.clip = _audioClips[audioType];
            mAudioSource.Play();
            //AudioSource.Stop();
            // AudioSource.PlayClipAtPoint(_audioClips[audioType], Vector3.zero);

        }
    }

    public enum AudioType
    {
        Click,
        B,
        C,
    }
}


