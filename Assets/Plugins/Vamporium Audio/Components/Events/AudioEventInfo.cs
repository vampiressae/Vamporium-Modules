using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

namespace VamporiumAudio
{
    public class AudioEventInfo : AudioPlayerEvents
    {
        private enum InfoType { Nothing, ClipName, ClipLength }

        [System.Serializable]
        private class Module
        {
            [SerializeField] private InfoType _type = InfoType.ClipName;
            [SerializeField] private string _stringFormat = "{0}";
            [SerializeField, ShowIf(nameof(HasFloat))] private string _floatFormat = "f1";

            private bool HasFloat => _type == InfoType.ClipLength;

            public string GetText(AudioPlayer player, string txt)
            {
                var text = GetText(player);

                if (!string.IsNullOrEmpty(text))
                    if (!string.IsNullOrEmpty(_stringFormat))
                        text = string.Format(_stringFormat, text);

                return txt + text;
            }

            private string GetText(AudioPlayer player)
            {
                if (player)
                    if (player.IsPlaying)
                        switch (_type)
                        {
                            case InfoType.ClipName: return player.Source.clip.name;
                            case InfoType.ClipLength: return player.Source.clip.length.ToString(_floatFormat);
                        }
                return string.Empty;
            }
        }

        [SerializeField] private TMP_Text _text;
        [SerializeField] private Module[] _modules;

        protected override void Refresh(AudioPlayer player)
        {
            var text = string.Empty;

            foreach(var module in _modules)
                text = module.GetText(player, text);

            _text.text = text;
        }
    }
}
