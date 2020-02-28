using System;
using System.Configuration;
using System.Text;


namespace rm2
{
    class RM2Settings : ApplicationSettingsBase
    {
        [UserScopedSetting()]
        [DefaultSettingValue("\\Skins\\Vista-style_ST.skf")]
        public string AppSkinPath
        {
            get
            {
                return ((string)this["AppSkinPath"]);
            }
            set
            {
                this["AppSkinPath"] = (string)value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("false")]
        public bool ShowBallon
        {
            get
            {
                return ((bool)this["ShowBallon"]);
            }
            set
            {
                this["ShowBallon"] = (bool)value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("false")]
        public bool AutoGenScrollLog
        {
            get
            {
                return ((bool)this["AutoGenScrollLog"]);
            }
            set
            {
                this["AutoGenScrollLog"] = (bool)value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("false")]
        public bool AutoRMScrollLog
        {
            get
            {
                return ((bool)this["AutoRMScrollLog"]);
            }
            set
            {
                this["AutoRMScrollLog"] = (bool)value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("5")]
        public int MaxNumOfPeersToShow
        {
            get
            {
                return ((int)this["MaxNumOfPeersToShow"]);
            }
            set
            {
                this["MaxNumOfPeersToShow"] = (int)value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("true")]
        public bool CheckVersion
        {
            get
            {
                return ((bool)this["CheckVersion"]);
            }
            set
            {
                this["CheckVersion"] = (bool)value;
            }
        }
    }
}
