using DG.Tweening;

namespace VamporiumAudio
{
    public static class AudioExtensionsFade
    {
        public static void FadeIn(this AudioPlayer player, float duration, bool fromZero = true)
            => player.FadeIn(duration, player.Material.Volume.GetRandom(), fromZero);

        public static void FadeIn(this AudioPlayer player, float duration, float volume, bool fromZero = true)
        {
            if (player == null) return;
            if (fromZero) player.Source.volume = 0;
            player.Source.DOFade(volume, duration);
        }

        public static void FadeOut(this AudioPlayer player, float duration, bool forceDestroy = false)
        {
            if (player == null) return;
            var tween = player.Source.DOFade(0, duration);
            if (forceDestroy || player.ShouldDestroy) tween.onComplete += player.DestroyMe;
        }
    }
}
