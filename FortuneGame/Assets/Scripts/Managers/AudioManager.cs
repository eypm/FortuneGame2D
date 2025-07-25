using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[DefaultExecutionOrder(-8000)]
public class AudioManager : MonoBehaviour
{
    private static AudioManager sInstance;

    private Dictionary<Audio, AudioSource> mAudioSources;

    public AudioSettings settings;
    public static Sounds Sounds => sInstance.settings.sounds;

    public static AudioManager GetInstance()
    {
        return sInstance;
    }

    public void Awake()
    {
        sInstance = this;
        mAudioSources = new Dictionary<Audio, AudioSource>();
    }

    public void PlaySound(Audio audio)
    {
        if (audio == null) return;
        bool isSoundEnabled = PreferenceHelper.IsSoundEnabled();
        if (!isSoundEnabled) return;

        Play(audio);
    }


    public void PlaySound(Audio audio, float delay = 0.0f, float time = 0.0f)
    {
        if (audio == null) return;

        bool isSoundEnabled = PreferenceHelper.IsSoundEnabled();
        if (!isSoundEnabled)
            return;

        if (delay > 0)
            DOVirtual.DelayedCall(delay, () => Play(audio, time));
        else

            Play(audio, time);
    }

    private void Play(Audio audio, float time = 0.0f)
    {
        if (mAudioSources == null) return;
        if (!mAudioSources.ContainsKey(audio))
        {
            mAudioSources.Add(audio, gameObject.AddComponent<AudioSource>());
            mAudioSources[audio].clip = audio.GetAudioClip();
            PlayAudio();
        }
        else
        {
            PlayAudio();
        }

        void PlayAudio()
        {
            AudioSource audioSource = mAudioSources[audio];
            audioSource.pitch = audio.Pitch;
            audioSource.volume = audio.volume;
            audioSource.loop = audio.loop;
            if (time != 0.0f)
            {
                var length = audioSource.clip.length;
                var pitch = length / time;
                audioSource.pitch = pitch;
            }

            if (audioSource.loop)
            {
                audioSource.Play();
            }
            else
            {
                audioSource.PlayOneShot(audioSource.clip);
            }
        }
    }

    public void PlayIfNotPlaying(Audio audio)
    {
        if (!IsPlaying(audio))
        {
            PlaySound(audio);
        }
    }

    public void StopIfPlaying(Audio audio)
    {
        if (IsPlaying(audio))
        {
            Stop(audio);
        }
    }

    public void VolumeChange(Audio audio, float volume)
    {
        if (!mAudioSources.ContainsKey(audio)) return;
        AudioSource audioSource = mAudioSources[audio];
        if (audioSource == null) return;
        audioSource.volume = volume;
    }

    private void Stop(Audio audio)
    {
        if (!mAudioSources.ContainsKey(audio)) return;
        AudioSource audioSource = mAudioSources[audio];
        audioSource.Stop();
    }

    private bool IsPlaying(Audio audio)
    {
        if (!mAudioSources.ContainsKey(audio)) return false;
        AudioSource audioSource = mAudioSources[audio];
        return audioSource.isPlaying;
    }

    private IEnumerator AfterPlayed(AudioSource audioSource, Action action)
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
        action();
    }
}

[Serializable]
public class Audio : IEquatable<Audio>
{
    public string clipPath;
    public float volume = 1;
    [SerializeField] private float pitch = 1;
    [SerializeField] private bool randomizePitch = false;
    [SerializeField] private float pitchRange = 0.2f;
    public float Pitch => randomizePitch ? UnityEngine.Random.Range(pitch - pitchRange, pitch + pitchRange) : pitch;
    public bool loop;

    public AudioClip GetAudioClip()
    {
        return Resources.Load<AudioClip>(clipPath);
    }

    public bool Equals(Audio other)
    {
        return clipPath == other.clipPath;
    }
}