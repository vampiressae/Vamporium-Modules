#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;

namespace VamporiumAudio
{
    public static class AudioEditorExtensions
    {
        public static bool GetAsset<T>(string search, out T result) where T : Object
        {
            result = default;
            search += " t:" + typeof(T).Name;

            var guid = AssetDatabase.FindAssets(search).FirstOrDefault();
            if (string.IsNullOrEmpty(guid)) return false;

            var path = AssetDatabase.GUIDToAssetPath(guid);
            result = AssetDatabase.LoadAssetAtPath<T>(path);
            return result != null;
        }

        public static bool GetChannel(string search, out AudioMixerGroup result)
        {
            result = null;
            if (!GetAsset<AudioMixer>("mixer", out var mixer)) return false;
            result = mixer.FindMatchingGroups(search).FirstOrDefault();
            return result != null;
        }

        public static bool GetAudioClipsInEditor(string name, int instanceID, out AudioClip[] clips)
        {
            clips = null;
            name = name.ToLower();
            var list = new List<AudioClip>();

            var guids = AssetDatabase.FindAssets(name, new string[] { AssetDatabase.GetAssetPath(instanceID) });
            if (guids.Length == 0) guids = AssetDatabase.FindAssets(name);
            if (guids.Length == 0) return false;
            foreach (var guid in guids)
            {
                var file = AssetDatabase.LoadAssetAtPath<AudioClip>(AssetDatabase.GUIDToAssetPath(guid));
                if (file == null) continue;
                if (file.name.ToLower() != name)
                {
                    var index = file.name.Replace(name + " ", "", System.StringComparison.OrdinalIgnoreCase);
                    if (!int.TryParse(index, out _)) continue;
                }

                list.Add(file);
            }
            clips = list.ToArray();
            return true;
        }
    }
}
#endif