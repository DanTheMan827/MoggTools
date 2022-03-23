using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MoggTools.Properties
{
    [CompilerGenerated]
    [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed class Settings : ApplicationSettingsBase
    {
        private static Settings defaultInstance;

        public static Settings Default
        {
            get
            {
                return Settings.defaultInstance;
            }
        }

        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        [UserScopedSetting]
        public bool RenameToArtistSongName
        {
            get
            {
                return (bool)this["RenameToArtistSongName"];
            }
            set
            {
                this["RenameToArtistSongName"] = value;
            }
        }

        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        [UserScopedSetting]
        public bool SaveMidiDataInRPP
        {
            get
            {
                return (bool)this["SaveMidiDataInRPP"];
            }
            set
            {
                this["SaveMidiDataInRPP"] = value;
            }
        }

        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        [UserScopedSetting]
        public bool SaveTempoMapInRPP
        {
            get
            {
                return (bool)this["SaveTempoMapInRPP"];
            }
            set
            {
                this["SaveTempoMapInRPP"] = value;
            }
        }

        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        [UserScopedSetting]
        public bool ShowTutorialSongs
        {
            get
            {
                return (bool)this["ShowTutorialSongs"];
            }
            set
            {
                this["ShowTutorialSongs"] = value;
            }
        }

        static Settings()
        {
            Settings.defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
        }

        public Settings()
        {
        }
    }
}