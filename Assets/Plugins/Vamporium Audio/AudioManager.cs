using UnityEngine;

namespace VamporiumAudio
{
    public class AudioManager
    {
        public static bool AutoDestroyPlayer = true;
        public static float MaxDistance = 50;

        public static AudioSettings Settings = new();
        public static Transform Parent;
        public static AudioMusic Music;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            Parent = new GameObject("Audio Players").transform;
            Object.DontDestroyOnLoad(Parent);

            Settings.Load();
            Music = new();
        }

        public static AudioPlayer Play(AudioClip[] clips, Transform parent = null) => Play(GetAClip(clips), parent);
        public static AudioPlayer Play(AudioClip clip, Transform parent = null) => PlayWithParent(Play(GetNewPlayer(GetName(clip), true), clip), parent);

        public static AudioPlayer Play(AudioPlayer player, AudioClip[] clips, Transform parent = null) => Play(player, GetAClip(clips), parent);
        public static AudioPlayer Play(AudioPlayer player, AudioClip clip, Transform parent = null) => PlayWithParent(Play(player, clip), parent);

        public static AudioPlayer Play(AudioClip[] clips, Vector3 position) => Play(GetAClip(clips), position);
        public static AudioPlayer Play(AudioClip clip, Vector3 position) => PlayWithPosition(Play(GetNewPlayer(GetName(clip), true), clip), position);

        public static AudioPlayer Play(AudioPlayer player, AudioClip[] clips, Vector3 position) => Play(player, GetAClip(clips), position);
        public static AudioPlayer Play(AudioPlayer player, AudioClip clip, Vector3 position) => PlayWithPosition(Play(player, clip), position);

        private static AudioPlayer Play(AudioPlayer player, AudioClip clip)
        {
            if (player == null) player = GetNewPlayer("Dynamic Audio Player");
            if (player.Material == null) player.SetMaterial(new());
            return player.Play(clip);
        }

        public static AudioPlayer Play(AudioMaterialAsset material, Transform parent = null) => material ? Play(material.Material, parent) : null;
        public static AudioPlayer Play(AudioMaterial material, Transform parent = null) => PlayWithParent(Play(GetNewPlayer(GetName(material), true), material), parent);
        public static AudioPlayer Play(AudioPlayer player, AudioMaterial material, Transform parent = null) => PlayWithParent(Play(player, material), parent);

        public static AudioPlayer Play(AudioMaterialAsset material, Vector3 position) => material ? Play(material.Material, position) : null;
        public static AudioPlayer Play(AudioMaterial material, Vector3 position) => PlayWithPosition(Play(GetNewPlayer(GetName(material), true), material), position);
        public static AudioPlayer Play(AudioPlayer player, AudioMaterial material, Vector3 position) => PlayWithPosition(Play(player, material), position);

        private static AudioPlayer Play(AudioPlayer player, AudioMaterial material)
        {
            material ??= new();
            if (player == null) player = GetNewPlayer(GetName(material));
            return player.Play(material);
        }

        public static AudioPlayer GetNewPlayer(string name, bool autoDestroy = false)
        {
            var go = new GameObject(name);
            go.transform.SetParent(Parent);
            var player = go.AddComponent<AudioPlayer>().SetAutoDestroy(autoDestroy);
            player.Source = go.AddComponent<AudioSource>();
            player.Source.playOnAwake = false;
            return player;
        }

        private static string GetName(AudioMaterial material) => material.IsValid() ? GetName(material.Clips[0]) : "";
        private static string GetName(AudioClip clip) => clip ? clip.name : "";

        private static AudioPlayer PlayWithParent(AudioPlayer player, Transform parent) => player.SetParent(parent).SetSpatialBlend(parent ? 1 : 0);
        private static AudioPlayer PlayWithPosition(AudioPlayer player, Vector3 position) => player.SetPosition(position).SetSpatialBlend(1);

        public static AudioClip GetAClip(AudioClip[] clips) => clips == null || clips.Length == 0 ? null : clips[Random.Range(0, clips.Length)];
        public static float RealVolume(float volume) => Mathf.Log(Mathf.Max(Mathf.Epsilon, volume)) * 15;
    }
}