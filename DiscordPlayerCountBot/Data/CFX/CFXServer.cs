using Newtonsoft.Json;
using System.Collections.Generic;

namespace DiscordPlayerCountBot.Source.CFX
{
    public class CFXServer
    {
        [JsonProperty("enhancedHostSupport")]
        public bool EnhancedHostSupport { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("resources")]
        public List<string> Resources { get; set; }

        [JsonProperty("server")]
        public string Server { get; set; }

        [JsonProperty("vars")]
        public CFXVars Vars { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        public int GetMaxPlayers()
        {
            return int.Parse(Vars.SvMaxClients);
        }
    }

    public class CFXVars
    {
        [JsonProperty("EssentialModeUUID")]
        public string EssentialModeUUID { get; set; }

        [JsonProperty("EssentialModeVersion")]
        public string EssentialModeVersion { get; set; }

        [JsonProperty("JD_logs")]
        public string JDLogs { get; set; }

        [JsonProperty("banner_connecting")]
        public string BannerConnecting { get; set; }

        [JsonProperty("banner_detail")]
        public string BannerDetail { get; set; }

        [JsonProperty("fivemqueue")]
        public string? Fivemqueue { get; set; } = "0";

        [JsonProperty("gamename")]
        public string Gamename { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("onesync_enabled")]
        public string OnesyncEnabled { get; set; }

        [JsonProperty("sv_enforceGameBuild")]
        public string SvEnforceGameBuild { get; set; }

        [JsonProperty("sv_enhancedHostSupport")]
        public string SvEnhancedHostSupport { get; set; }

        [JsonProperty("sv_lan")]
        public string SvLan { get; set; }

        [JsonProperty("sv_licenseKeyToken")]
        public string SvLicenseKeyToken { get; set; }

        [JsonProperty("sv_maxClients")]
        public string SvMaxClients { get; set; }

        [JsonProperty("sv_projectDesc")]
        public string SvProjectDesc { get; set; }

        [JsonProperty("sv_projectName")]
        public string SvProjectName { get; set; }

        [JsonProperty("sv_scriptHookAllowed")]
        public string SvScriptHookAllowed { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; }

        [JsonProperty("txAdmin-version")]
        public string TxAdminVersion { get; set; }
    }


}
