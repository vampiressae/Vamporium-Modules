using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace VamporiumAudio
{
    [System.Serializable]
    public class AudioSettings
    {
        public float Master = 1, Effects = 1, UI = 1, Music = 0.5f, Ambient = 0.7f;

        public float GetVolume(string key) => key switch
        {
            "Master" => Master,
            "Effects" => Effects,
            "UI" => UI,
            "Music" => Music,
            "Ambient" => Ambient,
            _ => 0,
        };

        public void SetVolume(string key, float value)
        {
            switch (key)
            {
                case "Master": Master = value; break;
                case "Effects": Effects = value; break;
                case "UI": UI = value; break;
                case "Music": Music = value; break;
                case "Ambient": Ambient = value; break;
            }
            Save();
        }

        public async void SetVolume(AudioMixerGroup channel)
        {
            if (channel == null) return;

            await Task.Yield(); // need this to let the AudioMixer be instantiated.
            SetVolume(channel.audioMixer, "Master");
            SetVolume(channel.audioMixer, channel.name);
        }

        private void SetVolume(AudioMixer mixer, string key) 
            => mixer.SetFloat(key, AudioManager.RealVolume(GetVolume(key)));

        public void Load()
        {
            var json = PlayerPrefs.GetString("AudioManager", string.Empty);
            try { AudioManager.Settings = JsonUtility.FromJson<AudioSettings>(json); }
            catch { }
            finally { AudioManager.Settings ??= new(); }
        }

        public void Save() => PlayerPrefs.SetString("AudioManager", JsonUtility.ToJson(this));
    }
}
