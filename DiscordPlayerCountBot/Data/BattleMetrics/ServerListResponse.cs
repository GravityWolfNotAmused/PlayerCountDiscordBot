namespace PlayerCountBot.Data
{
    public class BattleMetricsServerDetails
    {
        public List<string> modIds { get; set; } = new List<string>();
        public List<string> modHashes { get; set; } = new List<string>();
        public string? map { get; set; }
        public string? time { get; set; }
        public string? time_i { get; set; }
        public bool? official { get; set; }
        public string? gamemode { get; set; }
        public List<string>? modNames { get; set; }
        public bool? pve { get; set; }
        public bool? modded { get; set; }
        public bool? crossplay { get; set; }
        public string? session_flags { get; set; }
        public string? serverSteamId { get; set; }
    }

    public class BattleMetricsServerAttributes
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? address { get; set; }
        public string? ip { get; set; }
        public int? port { get; set; }
        public int? players { get; set; }
        public int? maxPlayers { get; set; }
        public int? rank { get; set; }
        public List<double> location { get; set; } = new List<double>();
        public string? status { get; set; }
        public BattleMetricsServerDetails? details { get; set; }
        public bool? @private { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
        public int? portQuery { get; set; }
        public string? country { get; set; }
        public string? queryStatus { get; set; }
    }

    public class BattleMetricsServerData
    {
        public string? type { get; set; }
        public string? id { get; set; }
        public BattleMetricsServerAttributes? attributes { get; set; }
        public BattleMetricsServerRelationships? relationships { get; set; }
        public BattleMetricsViewModel GetViewModel()
        {
            return new()
            {
                Address = attributes?.address ?? "",
                Port = attributes?.port ?? 0,
                MaxPlayers = attributes?.maxPlayers ?? 0,
                Players = attributes?.players ?? 0,
                Time = attributes?.details?.time ?? "",
                Rank = attributes?.rank ?? 0,
                GameMode = attributes?.details?.gamemode ?? "",
                Map = attributes?.details?.map ?? "",
                QueuedPlayers = 0
            };
        }
    }

    public class BattleMetricsServerGame
    {
        public BattleMetricsServerData? data { get; set; }
    }

    public class BattleMetricsServerRelationships
    {
        public BattleMetricsServerGame? game { get; set; }
    }

    public class Links
    {
        public string? prev { get; set; }
        public string? next { get; set; }
    }

    public class BattleMetricsServerRoot
    {
        public List<BattleMetricsServerData>? data { get; set; }
        public Links? links { get; set; }
        public List<object>? included { get; set; }
    }
}