namespace VamporiumAudio
{
    public interface IAudioPlayerWithEvents
    {
        event System.Action<AudioPlayer> OnPlay;
        event System.Action<AudioPlayer> OnStop;
        event System.Action<AudioPlayer> OnFinish;

        AudioPlayer Player { get; }
    }
}
