#if UNITY_EDITOR
using System.Collections;

namespace VamporiumAudio
{
    public static class AudioEditorExtensionsPresets
    {
        public static IEnumerable MaterialSettingsPresetNames(this AudioMaterialSettings _)
            => new string[] { "2D Effect", "3D Effect" };

        public static string MaterialSettingsPresetChangedInEditor(this AudioMaterialSettings settings, string preset)
        {
            switch (preset)
            {
                case "2D Effect": settings.MaterialSettingsPresetInEditor(false); break;
                case "3D Effect": settings.MaterialSettingsPresetInEditor(true); break;
            }
            return string.Empty;
        }

        public static void MaterialSettingsPresetInEditor(this AudioMaterialSettings settings, bool threeD)
        {
            AudioEditorExtensions.GetChannel("Effect", out settings.Channel);
            settings.SetPitch(new(0.8f, 1)).SetVolume(new(0.9f, 1)).SetSpatialBlend(threeD).SetLoop(false);
        }
    }
}
#endif
