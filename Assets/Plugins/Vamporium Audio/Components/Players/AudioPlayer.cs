using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace VamporiumAudio
{
    public class AudioPlayer : MonoBehaviour
    {
        public event System.Action<AudioPlayer> OnPlay, OnStop, OnFinish, OnDestroyed;

        [SerializeField, LabelWidth(100)] private bool _autoDestroy;
        [Space]
        [InlineProperty, HideLabel] public AudioMaterial Material;

        private AudioSource _source;
        private bool _beingDestroyed;

        [HorizontalGroup("info", 120), ShowInInspector, HideInEditorMode, ReadOnly, LabelWidth(100)]
        public bool IsPlaying => Application.isPlaying && Source && Source.isPlaying;
        public bool ShouldDestroy => _autoDestroy && AudioManager.AutoDestroyPlayer;

        public AudioSource Source
        {
            get
            {
                if (_source == null && !_beingDestroyed && gameObject != null)
                    SetSource(gameObject.AddComponent<AudioSource>());
                return _source;
            }
            set => SetSource(value);
        }

        private void OnDestroy()
        {
            _beingDestroyed = true;
            UnscheduleFinish();
            DOTween.Kill(this);
            if (_source) DOTween.Kill(_source);
        }

        public AudioPlayer Play() => Play(Material);

        public AudioPlayer Play(AudioMaterial material)
        {
            if (material.IsValid())
            {
                Material = material;
                Play(Material.GetClip());
            }
            return this;
        }

        public AudioPlayer Play(AudioClip[] clips)
            => Play(clips == null || clips.Length == 0 ? null : clips[Random.Range(0, clips.Length)]);

        public AudioPlayer Play(AudioClip clip)
        {
            if (IsPlaying) Source.Stop();
            Source.clip = clip;

            Source.Play();

            this.SetChannel(Material.Channel);
            this.SetVolume(Material.Volume);
            this.SetPitch(Material.Pitch);
            this.SetSpatialBlend(Material.SpatialBlend);
            this.SetLoop(Material.Loop);
            this.SetTime(0);

            OnPlay?.Invoke(this);
            return this;
        }

        public AudioPlayer Stop()
        {
            Source.Stop();
            OnStop?.Invoke(this);
            if (ShouldDestroy) DestroyMe();
            return this;
        }

        public void ScheduleFinish()
        {
            UnscheduleFinish();
            if (IsPlaying && !Source.loop)
                Invoke(nameof(Finish), Source.clip.length - Source.time);
        }

        private void UnscheduleFinish() => CancelInvoke(nameof(Finish));

        private AudioPlayer Finish()
        {
            Source.Stop();
            OnFinish?.Invoke(this);

            if (ShouldDestroy) DestroyMe();
            return this;
        }

        public void DestroyMe() => DestroyMe(0);
        public void DestroyMe(float delay)
        {
            Destroy(gameObject, delay);
            OnDestroyed?.Invoke(this);
        }

        public void SetSource(AudioSource source)
        {
            _source = source;
            this.SetDefaults();
        }

        public AudioPlayer SetAutoDestroy(bool value)
        {
            _autoDestroy = value;
            return this;
        }


#if UNITY_EDITOR
        [HorizontalGroup("info"), ShowInInspector, HideInEditorMode, HideLabel, PropertyRange(0, "CurrentClipLength")]
        private float PlayTimeInEditor { get => IsPlaying ? Source.time : 0; set => this.SetTime(value); }
        private float CurrentClipLength => IsPlaying ? Source.clip.length : 0;

        [Button("Play", DrawResult = false), HorizontalGroup, HideInEditorMode] private void PlayInEditor() => Source.Play();
        [Button("Stop", DrawResult = false), HorizontalGroup, HideInEditorMode] private void StopInEditor() => Source.Stop();
#endif
    }
}
