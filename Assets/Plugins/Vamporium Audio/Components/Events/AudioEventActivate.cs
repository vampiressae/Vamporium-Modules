using UnityEngine;
using VamporiumAudio;

public class AudioEventActivate : AudioPlayerEvents
{
    [SerializeField] private GameObject _go;
    [SerializeField] private bool _inverted;
    protected override void Refresh(AudioPlayer player)
    {
        var active = player.IsPlaying;
        if (_inverted) active = !active;
        _go.SetActive(active);
    }
}
