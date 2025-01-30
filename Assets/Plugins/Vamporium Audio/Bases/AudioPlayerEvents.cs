using UnityEngine;
using VamporiumAudio;

public abstract class AudioPlayerEvents : MonoBehaviour
{
    protected IAudioPlayerWithEvents _playerWithEvents;
    protected virtual void Start()
    {
        _playerWithEvents = GetComponent<IAudioPlayerWithEvents>();
        _playerWithEvents.OnPlay += Refresh;
        _playerWithEvents.OnFinish += Refresh;

        Refresh(_playerWithEvents.Player);
    }

    protected virtual void OnDestroy()
    {
        if (_playerWithEvents == null) return;

        _playerWithEvents.OnPlay -= Refresh;
        _playerWithEvents.OnFinish -= Refresh;
    }

    protected abstract void Refresh(AudioPlayer player);
}
