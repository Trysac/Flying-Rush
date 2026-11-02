using UnityEngine;

namespace FlyingRush.Game
{
    /// <summary>
    /// Cross-fades the title and battle themes, plays interface sounds flat and in-world sounds
    /// from a small pool of 3D sources so you hear where a shot came from.
    /// </summary>
    public sealed class AudioDirector : MonoBehaviour
    {
        const int Voices = 16;
        const float MusicBase = 0.45f;

        AudioSource music;
        AudioSource flat;
        readonly AudioSource[] voices = new AudioSource[Voices];
        int nextVoice;
        AudioClip wanted;
        float fade = 1f;
        bool ducked;

        /// <summary>Master volume 0..1.</summary>
        public float Volume { get; private set; } = 0.8f;
        /// <summary>Music volume 0..1, under the master volume.</summary>
        public float MusicVolume { get; private set; } = 0.6f;

        /// <summary>When true (the demo behind the title), world sounds are not played.</summary>
        public bool Silent { get; set; }

        public AudioClip CurrentMusic => wanted;

        public void Init(float volume, float musicVolume)
        {
            music = gameObject.AddComponent<AudioSource>();
            music.loop = true;
            music.playOnAwake = false;
            flat = gameObject.AddComponent<AudioSource>();
            flat.playOnAwake = false;
            for (int i = 0; i < Voices; i++)
            {
                var host = new GameObject("Voice");
                host.transform.SetParent(transform, false);
                AudioSource source = host.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.spatialBlend = 1f;
                source.rolloffMode = AudioRolloffMode.Linear;
                source.minDistance = 4f;
                source.maxDistance = 90f;
                source.dopplerLevel = 0f;
                voices[i] = source;
            }
            SetVolume(volume, musicVolume);
        }

        public void SetVolume(float volume, float musicVolume)
        {
            Volume = Mathf.Clamp01(volume);
            MusicVolume = Mathf.Clamp01(musicVolume);
            AudioListener.volume = Volume;
        }

        public void PlayMusic(AudioClip clip) => wanted = clip;

        public void SetDucked(bool value) => ducked = value;

        /// <summary>A sound heard as if it were inside your head: your own gun, the HUD.</summary>
        public void Play(AudioClip clip, float volume = 1f, float pitch = 1f, float jitter = 0.04f)
        {
            if (clip == null || flat == null)
            {
                return;
            }
            flat.pitch = pitch * (1f + Random.Range(-jitter, jitter));
            flat.PlayOneShot(clip, volume);
        }

        /// <summary>A sound somewhere in the cage.</summary>
        public void PlayAt(AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f)
        {
            if (clip == null || Silent || music == null)
            {
                return;
            }
            AudioSource voice = voices[nextVoice];
            nextVoice = (nextVoice + 1) % Voices;
            voice.transform.position = position;
            voice.pitch = pitch * (1f + Random.Range(-0.05f, 0.05f));
            voice.clip = clip;
            voice.volume = volume;
            voice.Play();
        }

        void Update()
        {
            if (music == null)
            {
                return;
            }
            if (music.clip != wanted)
            {
                fade = Mathf.MoveTowards(fade, 0f, Time.unscaledDeltaTime * 2.5f);
                if (fade <= 0f || !music.isPlaying)
                {
                    music.clip = wanted;
                    if (wanted != null)
                    {
                        music.Play();
                    }
                }
            }
            else
            {
                fade = Mathf.MoveTowards(fade, 1f, Time.unscaledDeltaTime * 1.5f);
            }
            music.volume = MusicBase * MusicVolume * fade * (ducked ? 0.35f : 1f);
        }
    }
}
