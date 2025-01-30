using Sirenix.OdinInspector;
using UnityEngine;

namespace VamporiumAudio
{
    public abstract class AudioComponent : MonoBehaviour, IAudioPlayerWithEvents
    {
        public event System.Action<AudioPlayer> OnPlay, OnStop, OnFinish;
        private enum PlayerUsage { Dynamic, Cached, Custom }

        [SerializeField] private PlayerUsage _playerMode = PlayerUsage.Cached;

        [SuffixLabel("$PlayerInfo")]
        [ShowIf(nameof(_playerMode), PlayerUsage.Custom)]
        [SerializeField] protected AudioPlayer _player;

        public AudioPlayer Player => _player;
        protected abstract AudioMaterial Material { get; }

        private bool _active;

        protected virtual void Awake()
        {
            switch (_playerMode)
            {
                case PlayerUsage.Dynamic:
                    // players are dynamically spawned, no need for preparation
                    break;

                case PlayerUsage.Cached:
                    _player = AudioManager.GetNewPlayer(GetType().Name + " : " + name);
                    _player.SetAutoDestroy(false).SetSpatialBlend(1);
                    _player.Material = Material;
                    break;

                case PlayerUsage.Custom:
                    if (_player == null) _player = GetComponentInParent<AudioPlayer>();
                    break;
            }
            Subscribe();
        }

        protected virtual void Start() => _active = true;

        protected virtual void OnDestroy()
        {
            if (_player == null) return;

            if (_playerMode == PlayerUsage.Custom)
                if (_player.transform != transform) return;

            if (_player.IsPlaying && !_player.Source.loop)
                _player.SetAutoDestroy(true);
            else _player.DestroyMe();

            Unsubscribe();
        }

        private void Subscribe()
        {
            if (_player)
            {
                _player.OnPlay += InvokeOnPlay;
                _player.OnStop += InvokeOnStop;
                _player.OnFinish += InvokeOnFinish;
            }
        }

        private void Unsubscribe()
        {
            if (_player)
            {
                _player.OnPlay -= InvokeOnPlay;
                _player.OnStop -= InvokeOnStop;
                _player.OnFinish -= InvokeOnFinish;
            }
        }

        protected virtual AudioPlayer Play()
        {
            if (!isActiveAndEnabled || !_active) return _player;

            switch (_playerMode)
            {
                case PlayerUsage.Dynamic:
                    _player = AudioManager.Play(Material);
                    Unsubscribe();
                    Subscribe();
                    return _player;

                case PlayerUsage.Cached:
                    return _player.Play();

                case PlayerUsage.Custom:
                    return _player.Play(Material);
            }
            return _player;

            //if (_playerMode != PlayerUsage.Dynamic)
            //    if (_playerMode == PlayerUsage.Custom)
            //        return _player.Play(Material);
            //    else return _player.Play();
            //else
            //{
            //    _player = AudioManager.Play(Material);
            //}
        }

        protected virtual AudioPlayer Stop() => _player ? _player.Stop() : null;

        private void InvokeOnPlay(AudioPlayer player) => OnPlay?.Invoke(player);
        private void InvokeOnStop(AudioPlayer player) => OnStop?.Invoke(player);
        private void InvokeOnFinish(AudioPlayer player) => OnFinish?.Invoke(player);

#if UNITY_EDITOR
        protected virtual void OnValidate() { }
        private string PlayerInfo => _player ? string.Empty : "Find in parents";
#endif
    }
}
