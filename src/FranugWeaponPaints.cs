using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Utils;
using Newtonsoft.Json;
using System.Text;
using static CounterStrikeSharp.API.Core.Listeners;

namespace FranugWeaponPaints;


[MinimumApiVersion(142)]
public class FranugWeaponPaints : BasePlugin
{
    public override string ModuleName => "Franug Weapon Paints";
    public override string ModuleAuthor => "Franc1sco Franug";
    public override string ModuleVersion => "0.0.1";

    public static Dictionary<int, string> weaponDefindex { get; } = new Dictionary<int, string>
    {
        { 1, "weapon_deagle" },
        { 2, "weapon_elite" },
        { 3, "weapon_fiveseven" },
        { 4, "weapon_glock" },
        { 7, "weapon_ak47" },
        { 8, "weapon_aug" },
        { 9, "weapon_awp" },
        { 10, "weapon_famas" },
        { 11, "weapon_g3sg1" },
        { 13, "weapon_galilar" },
        { 14, "weapon_m249" },
        { 16, "weapon_m4a1" },
        { 17, "weapon_mac10" },
        { 19, "weapon_p90" },
        { 23, "weapon_mp5sd" },
        { 24, "weapon_ump45" },
        { 25, "weapon_xm1014" },
        { 26, "weapon_bizon" },
        { 27, "weapon_mag7" },
        { 28, "weapon_negev" },
        { 29, "weapon_sawedoff" },
        { 30, "weapon_tec9" },
        { 32, "weapon_hkp2000" },
        { 33, "weapon_mp7" },
        { 34, "weapon_mp9" },
        { 35, "weapon_nova" },
        { 36, "weapon_p250" },
        { 38, "weapon_scar20" },
        { 39, "weapon_sg556" },
        { 40, "weapon_ssg08" },
        { 60, "weapon_m4a1_silencer" },
        { 61, "weapon_usp_silencer" },
        { 63, "weapon_cz75a" },
        { 64, "weapon_revolver" },
        { 500, "weapon_bayonet" },
        { 503, "weapon_knife_css" },
        { 505, "weapon_knife_flip" },
        { 506, "weapon_knife_gut" },
        { 507, "weapon_knife_karambit" },
        { 508, "weapon_knife_m9_bayonet" },
        { 509, "weapon_knife_tactical" },
        { 512, "weapon_knife_falchion" },
        { 514, "weapon_knife_survival_bowie" },
        { 515, "weapon_knife_butterfly" },
        { 516, "weapon_knife_push" },
        { 517, "weapon_knife_cord" },
        { 518, "weapon_knife_canis" },
        { 519, "weapon_knife_ursus" },
        { 520, "weapon_knife_gypsy_jackknife" },
        { 521, "weapon_knife_outdoor" },
        { 522, "weapon_knife_stiletto" },
        { 523, "weapon_knife_widowmaker" },
        { 525, "weapon_knife_skeleton" }
    };

    internal static readonly Dictionary<string, string> weaponList = new()
    {
        {"weapon_deagle", "Desert Eagle"},
        {"weapon_elite", "Dual Berettas"},
        {"weapon_fiveseven", "Five-SeveN"},
        {"weapon_glock", "Glock-18"},
        {"weapon_ak47", "AK-47"},
        {"weapon_aug", "AUG"},
        {"weapon_awp", "AWP"},
        {"weapon_famas", "FAMAS"},
        {"weapon_g3sg1", "G3SG1"},
        {"weapon_galilar", "Galil AR"},
        {"weapon_m249", "M249"},
        {"weapon_m4a1", "M4A1"},
        {"weapon_mac10", "MAC-10"},
        {"weapon_p90", "P90"},
        {"weapon_mp5sd", "MP5-SD"},
        {"weapon_ump45", "UMP-45"},
        {"weapon_xm1014", "XM1014"},
        {"weapon_bizon", "PP-Bizon"},
        {"weapon_mag7", "MAG-7"},
        {"weapon_negev", "Negev"},
        {"weapon_sawedoff", "Sawed-Off"},
        {"weapon_tec9", "Tec-9"},
        {"weapon_hkp2000", "P2000"},
        {"weapon_mp7", "MP7"},
        {"weapon_mp9", "MP9"},
        {"weapon_nova", "Nova"},
        {"weapon_p250", "P250"},
        {"weapon_scar20", "SCAR-20"},
        {"weapon_sg556", "SG 553"},
        {"weapon_ssg08", "SSG 08"},
        {"weapon_m4a1_silencer", "M4A1-S"},
        {"weapon_usp_silencer", "USP-S"},
        {"weapon_cz75a", "CZ75-Auto"},
        {"weapon_revolver", "R8 Revolver"},
        { "weapon_knife", "Default Knife" },
        { "weapon_knife_m9_bayonet", "M9 Bayonet" },
        { "weapon_knife_karambit", "Karambit" },
        { "weapon_bayonet", "Bayonet" },
        { "weapon_knife_survival_bowie", "Bowie Knife" },
        { "weapon_knife_butterfly", "Butterfly Knife" },
        { "weapon_knife_falchion", "Falchion Knife" },
        { "weapon_knife_flip", "Flip Knife" },
        { "weapon_knife_gut", "Gut Knife" },
        { "weapon_knife_tactical", "Huntsman Knife" },
        { "weapon_knife_push", "Shadow Daggers" },
        { "weapon_knife_gypsy_jackknife", "Navaja Knife" },
        { "weapon_knife_stiletto", "Stiletto Knife" },
        { "weapon_knife_widowmaker", "Talon Knife" },
        { "weapon_knife_ursus", "Ursus Knife" },
        { "weapon_knife_css", "Classic Knife"},
        { "weapon_knife_cord", "Paracord Knife" },
        { "weapon_knife_canis", "Survival Knife" },
        { "weapon_knife_outdoor", "Nomad Knife" },
        { "weapon_knife_skeleton", "Skeleton Knife" }
    };

    private Dictionary<string, ushort> weaponListDef = new()
    {
        {"weapon_deagle", (ushort)ItemDefinition.DESERT_EAGLE},
        {"weapon_elite", (ushort)ItemDefinition.DUAL_BERETTAS},
        {"weapon_fiveseven", (ushort)ItemDefinition.FIVE_SEVEN},
        {"weapon_glock", (ushort)ItemDefinition.GLOCK_18},
        {"weapon_ak47", (ushort)ItemDefinition.AK_47},
        {"weapon_aug", (ushort)ItemDefinition.AUG},
        {"weapon_awp", (ushort)ItemDefinition.AWP},
        {"weapon_famas", (ushort)ItemDefinition.FAMAS},
        {"weapon_g3sg1", (ushort)ItemDefinition.G3SG1},
        {"weapon_galilar", (ushort)ItemDefinition.GALIL_AR},
        {"weapon_m249", (ushort)ItemDefinition.M249},
        {"weapon_m4a1", (ushort)ItemDefinition.M4A4},
        {"weapon_mac10", (ushort)ItemDefinition.MAC_10},
        {"weapon_p90", (ushort)ItemDefinition.P90},
        {"weapon_mp5sd", (ushort)ItemDefinition.MP5_SD},
        {"weapon_ump45", (ushort)ItemDefinition.UMP_45},
        {"weapon_xm1014", (ushort)ItemDefinition.XM1014},
        {"weapon_bizon", (ushort)ItemDefinition.PP_BIZON},
        {"weapon_mag7", (ushort)ItemDefinition.MAG_7},
        {"weapon_negev", (ushort)ItemDefinition.NEGEV},
        {"weapon_sawedoff", (ushort)ItemDefinition.SAWED_OFF},
        {"weapon_tec9", (ushort)ItemDefinition.TEC_9},
        {"weapon_hkp2000", (ushort)ItemDefinition.P2000},
        {"weapon_mp7", (ushort)ItemDefinition.MP7},
        {"weapon_mp9", (ushort)ItemDefinition.MP9},
        {"weapon_nova", (ushort)ItemDefinition.NOVA},
        {"weapon_p250", (ushort)ItemDefinition.P250},
        {"weapon_scar20", (ushort)ItemDefinition.SCAR_20},
        {"weapon_sg556", (ushort)ItemDefinition.SG_553},
        {"weapon_ssg08", (ushort)ItemDefinition.SSG_08},
        {"weapon_m4a1_silencer", (ushort)ItemDefinition.M4A1_S},
        {"weapon_usp_silencer", (ushort)ItemDefinition.USP_S},
        {"weapon_cz75a", (ushort)ItemDefinition.CZ75_AUTO},
        {"weapon_revolver", (ushort)ItemDefinition.R8_REVOLVER},
        { "weapon_knife", (ushort)ItemDefinition.KNIFE_CT}, // REVISAR
        { "weapon_knife_m9_bayonet", (ushort)ItemDefinition.M9_BAYONET},
        { "weapon_knife_karambit", (ushort)ItemDefinition.KARAMBIT},
        { "weapon_bayonet", (ushort)ItemDefinition.BAYONET},
        { "weapon_knife_survival_bowie", (ushort)ItemDefinition.SURVIVAL_KNIFE},
        { "weapon_knife_butterfly", (ushort)ItemDefinition.BUTTERFLY_KNIFE},
        { "weapon_knife_falchion", (ushort)ItemDefinition.FALCHION_KNIFE},
        { "weapon_knife_flip", (ushort)ItemDefinition.FLIP_KNIFE},
        { "weapon_knife_gut", (ushort)ItemDefinition.GUT_KNIFE},
        { "weapon_knife_tactical", (ushort)ItemDefinition.HUNTSMAN_KNIFE},
        { "weapon_knife_push", (ushort)ItemDefinition.SHADOW_DAGGERS},
        { "weapon_knife_gypsy_jackknife", (ushort)ItemDefinition.NAVAJA_KNIFE},
        { "weapon_knife_stiletto", (ushort)ItemDefinition.STILETTO_KNIFE},
        { "weapon_knife_widowmaker", (ushort)ItemDefinition.TALON_KNIFE},
        { "weapon_knife_ursus", (ushort)ItemDefinition.URSUS_KNIFE},
        { "weapon_knife_css", (ushort)ItemDefinition.CLASSIC_KNIFE},
        { "weapon_knife_cord", (ushort)ItemDefinition.PARACORD_KNIFE},
        { "weapon_knife_canis", (ushort)ItemDefinition.SURVIVAL_KNIFE},
        { "weapon_knife_outdoor", (ushort)ItemDefinition.NOMAD_KNIFE},
        { "weapon_knife_skeleton", (ushort)ItemDefinition.SKELETON_KNIFE}
    };

    internal static readonly Dictionary<string, string> knifeList = new()
    {
        { "weapon_knife", "Default Knife" },
        { "weapon_knife_m9_bayonet", "M9 Bayonet" },
        { "weapon_knife_karambit", "Karambit" },
        { "weapon_bayonet", "Bayonet" },
        { "weapon_knife_survival_bowie", "Bowie Knife" },
        { "weapon_knife_butterfly", "Butterfly Knife" },
        { "weapon_knife_falchion", "Falchion Knife" },
        { "weapon_knife_flip", "Flip Knife" },
        { "weapon_knife_gut", "Gut Knife" },
        { "weapon_knife_tactical", "Huntsman Knife" },
        { "weapon_knife_push", "Shadow Daggers" },
        { "weapon_knife_gypsy_jackknife", "Navaja Knife" },
        { "weapon_knife_stiletto", "Stiletto Knife" },
        { "weapon_knife_widowmaker", "Talon Knife" },
        { "weapon_knife_ursus", "Ursus Knife" },
        { "weapon_knife_css", "Classic Knife"},
        { "weapon_knife_cord", "Paracord Knife" },
        { "weapon_knife_canis", "Survival Knife" },
        { "weapon_knife_outdoor", "Nomad Knife" },
        { "weapon_knife_skeleton", "Skeleton Knife" }
    };

    private const int SKINS_SEPARATOR = 25;
    private const int SKINS_BLANKS = 20;

    internal static Dictionary<int, string> g_playersKnife = new();
    internal static Dictionary<int, int> g_knifePickupCount = new Dictionary<int, int>();
    internal static Dictionary<int, Dictionary<int, WeaponInfo>> gPlayerWeaponsInfo = new Dictionary<int, Dictionary<int, WeaponInfo>>();
    private readonly Dictionary<int, int> iWSIndex = new();
    private readonly Dictionary<int, string> iWSWeapon = new();



    internal static Dictionary<string, Skin> skinList = new();


    public override void Load(bool hotReload)
    {
        if (hotReload)
        {
            Utilities.GetPlayers().ForEach(player =>
            {
                iWSIndex.Add((int)player.Index, 0);
                iWSWeapon.Add((int)player.Index, "");
                if (!gPlayerWeaponsInfo.TryGetValue((int)player.Index, out _))
                {
                    gPlayerWeaponsInfo[(int)player.Index] = new Dictionary<int, WeaponInfo>();
                }
                g_knifePickupCount[(int)player!.Index] = 0;
            });
        }

        loadList();
        RegisterListener<Listeners.OnEntityCreated>(OnEntityCreated);
        RegisterEventHandler<EventPlayerConnectFull>((@event, info) =>
        {
            var player = @event.Userid;

            if (player.IsBot || !player.IsValid)
            {
                return HookResult.Continue;

            }
            else
            {
                iWSIndex.Add((int)player.Index, 0);
                iWSWeapon.Add((int)player.Index, "");
                if (!gPlayerWeaponsInfo.TryGetValue((int)player.Index, out _))
                {
                    gPlayerWeaponsInfo[(int)player.Index] = new Dictionary<int, WeaponInfo>();
                }
                return HookResult.Continue;
            }
        });

        RegisterEventHandler<EventPlayerDisconnect>((@event, info) =>
        {
            var player = @event.Userid;

            if (player.IsBot || !player.IsValid)
            {
                return HookResult.Continue;

            }
            else
            {
                if (iWSIndex.ContainsKey((int)player.Index))
                {
                    iWSIndex.Remove((int)player.Index);
                }
                if (iWSWeapon.ContainsKey((int)player.Index))
                {
                    iWSWeapon.Remove((int)player.Index);
                }
                if (gPlayerWeaponsInfo.ContainsKey((int)player.Index))
                {
                    gPlayerWeaponsInfo.Remove((int)player.Index);
                }
                if (g_playersKnife.ContainsKey((int)player.Index))
                {
                    g_playersKnife.Remove((int)player.Index);
                }
                if (g_knifePickupCount.ContainsKey((int)player.Index))
                {
                    g_knifePickupCount.Remove((int)player.Index);
                }
                return HookResult.Continue;
            }
        });

        HookEntityOutput("weapon_knife", "OnPlayerPickup", OnPickup, HookMode.Pre);

        RegisterListener<OnMapStart>(OnMapStart);

        RegisterEventHandler<EventPlayerSpawn>(OnPlayerSpawn);

        RegisterEventHandler<EventRoundStart>(OnRoundStart, HookMode.Pre);

    }

    private HookResult OnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
    {
        CCSPlayerController? player = @event.Userid;
        if (player == null || !player.IsValid || player.IsBot)
        {
            return HookResult.Continue;
        }

        g_knifePickupCount[(int)player.Index] = 0;


        if (!PlayerHasKnife(player))
        {
            AddTimer(0.1f, () => GiveKnifeToPlayer(player));
        }

        return HookResult.Continue;
    }

    private HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        NativeAPI.IssueServerCommand("mp_t_default_melee \"\"");
        NativeAPI.IssueServerCommand("mp_ct_default_melee \"\"");
        NativeAPI.IssueServerCommand("mp_equipment_reset_rounds 0");

        return HookResult.Continue;
    }

    private void OnMapStart(string mapName)
    {

        // TODO
        // needed for now
        AddTimer(2.0f, () =>
        {

            NativeAPI.IssueServerCommand("mp_t_default_melee \"\"");
            NativeAPI.IssueServerCommand("mp_ct_default_melee \"\"");
            NativeAPI.IssueServerCommand("mp_equipment_reset_rounds 0");
        });
    }

        [ConsoleCommand("css_kf", "")]
    [ConsoleCommand("sm_kf", "")]
    [CommandHelper(minArgs: 0, usage: "knifeName]", whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void CommandKnife(CCSPlayerController? player, CommandInfo info)
    {
        if (!string.IsNullOrEmpty(info.GetArg(1)))
        {
            var arg1 = info.GetArg(1);

            if (arg1.Contains("default"))
            {
                if (g_playersKnife[(int)player.Index] == "weapon_knife")
                {
                    player.PrintToConsole("already set default knife");
                    info.ReplyToCommand("already set default knife");
                    return;
                }

                if (g_playersKnife.TryGetValue((int)player.Index, out var knife))
                {
                    g_playersKnife[(int)player.Index] = "weapon_knife";
                    RefreshWeapons(player, knife);
                    player.PrintToConsole("set default knife");
                    info.ReplyToCommand("set default knife");
                    return;
                }
                g_playersKnife[(int)player.Index] = "weapon_knife";
                RefreshWeapons(player, "weapon_knife");

                player.PrintToConsole("set default knife");
                info.ReplyToCommand("set default knife");
                return;
            }

            if (!knifeList.ContainsKey(arg1) && !knifeList.Keys.Any(k => k.Contains(arg1)))
            {
                player.PrintToConsole("invalid knife name");
                info.ReplyToCommand("invalid knife name");
                return;
            }

            var weaponDefIndex = weaponListDef.FirstOrDefault(w => w.Key.Contains(arg1, StringComparison.OrdinalIgnoreCase));

            KeyValuePair<string, string>? weaponName = weaponList.FirstOrDefault(w => w.Key.Contains(arg1, StringComparison.OrdinalIgnoreCase));

            g_playersKnife[(int)player.Index] = weaponName.Value.Key;
            RefreshWeapons(player, weaponName.Value.Key);
            player.PrintToConsole("new knife set to " + weaponName.Value.Value);
            info.ReplyToCommand("new knife set to " + weaponName.Value.Value);
            return;
        } else {

            for (int i = 0; i < SKINS_BLANKS; i++)
            {
                player.PrintToConsole(" ");
            }
            player.PrintToConsole("------------------------------------------------------------------");
            player.PrintToConsole("Knife menu:");
            player.PrintToConsole("------------------------------------------------------------------");
            var count = 0;
            for (int i = 0; i < knifeList.Count; i++)
            {
                StringBuilder sb = new StringBuilder();
                var knife = knifeList.ElementAt(i);
                sb.Append($"{knife.Key} - ★ {knife.Value}");
                player.PrintToConsole(sb.ToString());
                count++;

                if (count >= SKINS_SEPARATOR) break;
            }
            player.PrintToConsole("------------------------------------------------------------------");
        }
    }

    [ConsoleCommand("css_ws", "")]
    [ConsoleCommand("sm_ws", "")]
    [CommandHelper(minArgs: 1, usage: "weaponName skinID]", whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void CommandWs(CCSPlayerController? player, CommandInfo info)
    {
        var arg1 = info.GetArg(1);

        if (!weaponList.ContainsKey(arg1) && !weaponList.Keys.Any(k => k.Contains(arg1)))
        {
            player.PrintToConsole("invalid weapon name");
            info.ReplyToCommand("invalid weapon name");
            return;
        }

        if (!string.IsNullOrEmpty(info.GetArg(2)))
        {
            var arg2 = info.GetArg(2);
            if (!int.TryParse(arg2, out int result2))
            {
                player.PrintToConsole("second arg must be a number");
                info.ReplyToCommand("second arg must be a number");
                return;
            }

            var skinId = int.Parse(arg2);

            if (skinId > skinList.Count-1 || skinId < 0)
            {
                player.PrintToConsole("second arg must be a number and must be between 0-" + (skinList.Count - 1));
                info.ReplyToCommand("second arg must be a number and must be between 0-" + (skinList.Count - 1));
                return;
            }

            var weaponDefIndex = weaponListDef.FirstOrDefault(w => w.Key.Contains(arg1, StringComparison.OrdinalIgnoreCase));

            KeyValuePair<string, string>? weaponName = weaponList.FirstOrDefault(w => w.Key.Contains(arg1, StringComparison.OrdinalIgnoreCase));

            if (skinId == 0)
            {
                gPlayerWeaponsInfo[(int)player.Index].Remove(weaponDefIndex.Value);
                RefreshWeapons(player, weaponName.Value.Key);

                player.PrintToConsole("default skin set for " + weaponName.Value.Value);
                info.ReplyToCommand("default skin set for " + weaponName.Value.Value);
                return;
            }
            for (int i = 0; i < skinList.Count; i++)
            {
                var current = skinList.ElementAt(i);

                if (int.Parse(current.Value.Index) == skinId) break;
            }

            var newSkin = GetSkinSearched(skinId);
            if (newSkin == null)
            {

                player.PrintToConsole("skin id " + skinId + " not found");
                info.ReplyToCommand("skin id " + skinId + " not found");
                return;
            }

            WeaponInfo weaponInfo = new WeaponInfo
            {
                Paint = skinId,
                Seed = 0,
                Wear = 0
            };


            gPlayerWeaponsInfo[(int)player.Index][weaponDefIndex.Value] = weaponInfo;
            RefreshWeapons(player, weaponName.Value.Key);
            player.PrintToConsole("new skin set to " + weaponName.Value.Value + " | " + newSkin.Value.Key);
            info.ReplyToCommand("new skin set to " + weaponName.Value.Value + " | " + newSkin.Value.Key);
            return;
        }

        for (int i = 0; i < SKINS_BLANKS; i++)
        {
            player.PrintToConsole(" ");
        }
        player.PrintToConsole("------------------------------------------------------------------");
        KeyValuePair<string, string>? searchedWeapon = weaponList.FirstOrDefault(w => w.Key.Contains(arg1, StringComparison.OrdinalIgnoreCase));
        player.PrintToConsole($"{searchedWeapon.Value}:");
        player.PrintToConsole("------------------------------------------------------------------");
        var count = 0;
        for (int i = iWSIndex[(int)player.Index]; i < skinList.Count; i += 2)
        {
            StringBuilder sb = new StringBuilder();
            var firstSkin = skinList.ElementAt(i);
            sb.Append($"{firstSkin.Value.Index} - {firstSkin.Key}");

            if (i + 1 < skinList.Count)
            {
                sb.Append("             ");
                var secondSkin = skinList.ElementAt(i + 1);
                sb.Append($"{secondSkin.Value.Index} - {secondSkin.Key}");
            }

            player.PrintToConsole(sb.ToString());
            count++;

            if (count >= SKINS_SEPARATOR) break;
        }
        player.PrintToConsole("------------------------------------------------------------------");
        player.PrintToConsole("<<< sm_ws_back              sm_ws_next >>>");
        player.PrintToConsole("------------------------------------------------------------------");

        iWSWeapon[(int)player.Index] = searchedWeapon.Value.ToString();

    }

    [ConsoleCommand("css_ws_next", "")]
    [ConsoleCommand("sm_ws_next", "")]
    [CommandHelper(minArgs: 0, usage: "", whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void CommandWsNext(CCSPlayerController? player, CommandInfo info)
    {

        if (iWSWeapon[(int)player.Index] == null || iWSWeapon[(int)player.Index] == "")
        {
            player.PrintToConsole("You need to choose weapon first");
            info.ReplyToCommand("You need to choose weapon first");
            return;
        }

        if (iWSIndex[(int)player.Index] + SKINS_SEPARATOR >= skinList.Count)
        {
            player.PrintToConsole("Max paints reached");
            info.ReplyToCommand("Max paints reached");
            return;
        }

        iWSIndex[(int)player.Index] += SKINS_SEPARATOR;

        for (int i = 0; i < SKINS_BLANKS; i++)
        {
            player.PrintToConsole(" ");
        }
        player.PrintToConsole(" ------------------------------------------------------------------");
        player.PrintToConsole($"{iWSWeapon[(int)player.Index]}:");
        player.PrintToConsole(" ------------------------------------------------------------------");
        var count = 0;
        for (int i = iWSIndex[(int)player.Index]; i < skinList.Count; i += 2)
        {
            StringBuilder sb = new StringBuilder();
            var firstSkin = skinList.ElementAt(i);
            sb.Append($"{firstSkin.Value.Index} - {firstSkin.Key}");

            if (i + 1 < skinList.Count)
            {
                sb.Append("             ");
                var secondSkin = skinList.ElementAt(i + 1);
                sb.Append($"{secondSkin.Value.Index} - {secondSkin.Key}");
            }

            player.PrintToConsole(sb.ToString());
            count++;

            if (count >= SKINS_SEPARATOR) break;
        }
        player.PrintToConsole("------------------------------------------------------------------");
        player.PrintToConsole("<<< sm_ws_back              sm_ws_next >>>");
        player.PrintToConsole("------------------------------------------------------------------");

    }

    [ConsoleCommand("css_ws_back", "")]
    [ConsoleCommand("sm_ws_back", "")]
    [CommandHelper(minArgs: 0, usage: "", whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void CommandWsBack(CCSPlayerController? player, CommandInfo info)
    {

        if (iWSWeapon[(int)player.Index] == null || iWSWeapon[(int)player.Index] == "")
        {
            player.PrintToConsole("You need to choose weapon first");
            info.ReplyToCommand("You need to choose weapon first");
            return;
        }

        if (iWSIndex[(int)player.Index] - SKINS_SEPARATOR <= 0)
        {
            player.PrintToConsole("Min paints reached");
            info.ReplyToCommand("Min paints reached");
            return;
        }

        iWSIndex[(int)player.Index] -= SKINS_SEPARATOR;

        for (int i = 0; i < SKINS_BLANKS; i++)
        {
            player.PrintToConsole(" ");
        }
        player.PrintToConsole("------------------------------------------------------------------");
        player.PrintToConsole($"{iWSWeapon[(int)player.Index]}:");
        player.PrintToConsole("------------------------------------------------------------------");
        var count = 0;
        for (int i = iWSIndex[(int)player.Index]; i < skinList.Count; i += 2)
        {
            StringBuilder sb = new StringBuilder();
            var firstSkin = skinList.ElementAt(i);
            sb.Append($"{firstSkin.Value.Index} - {firstSkin.Key}");

            if (i + 1 < skinList.Count)
            {
                sb.Append("             ");
                var secondSkin = skinList.ElementAt(i + 1);
                sb.Append($"{secondSkin.Value.Index} - {secondSkin.Key}");
            }

            player.PrintToConsole(sb.ToString());
            count++;

            if (count >= SKINS_SEPARATOR) break;
        }
        player.PrintToConsole("------------------------------------------------------------------");
        player.PrintToConsole("<<< sm_ws_back              sm_ws_next >>>");
        player.PrintToConsole("------------------------------------------------------------------");
    }

    private void loadList()
    {
        string filePath = Server.GameDirectory + "/csgo/addons/counterstrikesharp/plugins/FranugWeaponPaints/skins_en.json";

        string json = File.ReadAllText(filePath);

        SkinCollection skinCollection = JsonConvert.DeserializeObject<SkinCollection>(json);

        Dictionary<string, Skin> skins = skinCollection.Skins;

        foreach (var skin in skinCollection.Skins.OrderBy(x => int.Parse(x.Value.Index)))
        {
            skinList.Add(skin.Key, skin.Value);
        }

        /*
        Console.WriteLine("Información de Skins:");
        foreach (var skin in skinList)
        {
            Console.WriteLine($"Nombre: {skin.Key}");
            Console.WriteLine($"Index: {skin.Value.Index}");
            Console.WriteLine($"Classes: {skin.Value.Classes}");
            Console.WriteLine();
        }*/
    }

    private void OnEntityCreated(CEntityInstance entity)
    {
        var designerName = entity.DesignerName;
        if (!weaponList.ContainsKey(designerName)) return;
        bool isKnife = false;
        var weapon = new CBasePlayerWeapon(entity.Handle);

        if (designerName.Contains("knife") || designerName.Contains("bayonet"))
        {
            isKnife = true;
        }

        Server.NextFrame(() =>
        {
            try
            {
                if (!weapon.IsValid) return;
                if (weapon.OwnerEntity.Value == null) return;
                if (weapon.OwnerEntity.Index <= 0) return;
                int weaponOwner = (int)weapon.OwnerEntity.Index;
                var pawn = new CBasePlayerPawn(NativeAPI.GetEntityFromIndex(weaponOwner));
                if (!pawn.IsValid) return;

                var playerIndex = (int)pawn.Controller.Index;
                var player = Utilities.GetPlayerFromIndex(playerIndex);
                if (!Utility.IsPlayerValid(player)) return;

                ChangeWeaponAttributes(weapon, player, isKnife);
            }
            catch (Exception) { }
        });
    }

    public void ChangeWeaponAttributes(CBasePlayerWeapon? weapon, CCSPlayerController? player, bool isKnife = false)
    {
        if (player == null || weapon == null || !weapon.IsValid || !Utility.IsPlayerValid(player)) return;

        int playerIndex = (int)player.Index;

        if (!gPlayerWeaponsInfo.ContainsKey(playerIndex)) return;

        if (isKnife && !g_playersKnife.ContainsKey(playerIndex) || isKnife && g_playersKnife[playerIndex] == "weapon_knife") return;

        int weaponDefIndex = weapon.AttributeManager.Item.ItemDefinitionIndex;

        if (isKnife)
        {
            //Console.WriteLine("EntityQuality 3 hecho");
            weapon.AttributeManager.Item.EntityQuality = 3;
        }

        //Console.WriteLine("casi aplicado skin para weaponindex " + weaponDefIndex);
        if (!gPlayerWeaponsInfo[playerIndex].ContainsKey(weaponDefIndex)) return;
        WeaponInfo weaponInfo = gPlayerWeaponsInfo[playerIndex][weaponDefIndex];
        weapon.AttributeManager.Item.ItemID = 16384;
        weapon.AttributeManager.Item.ItemIDLow = 16384 & 0xFFFFFFFF;
        weapon.AttributeManager.Item.ItemIDHigh = weapon.AttributeManager.Item.ItemIDLow >> 32;
        weapon.FallbackPaintKit = weaponInfo.Paint;
        weapon.FallbackSeed = weaponInfo.Seed;
        weapon.FallbackWear = weaponInfo.Wear;
        //Console.WriteLine("aplicado skin " + weaponInfo.Paint);
        if (!isKnife && weapon.CBodyComponent != null && weapon.CBodyComponent.SceneNode != null)
        {
            var skeleton = GetSkeletonInstance(weapon.CBodyComponent.SceneNode);
            skeleton.ModelState.MeshGroupMask = 2;
        }
    }

    private static CSkeletonInstance GetSkeletonInstance(CGameSceneNode node)
    {
        Func<nint, nint> GetSkeletonInstance = VirtualFunction.Create<nint, nint>(node.Handle, 8);
        return new CSkeletonInstance(GetSkeletonInstance(node.Handle));
    }

    private void RefreshWeapons(CCSPlayerController? player, string searched)
    {
        if (player == null || !player.IsValid || player.PlayerPawn.Value == null || !player.PawnIsAlive) return;
        if (player.PlayerPawn.Value.WeaponServices == null || player.PlayerPawn.Value.ItemServices == null) return;

        var weapons = player.PlayerPawn.Value.WeaponServices.MyWeapons;
        if (weapons != null && weapons.Count > 0)
        {
            CCSPlayer_ItemServices service = new(player.PlayerPawn.Value.ItemServices.Handle);

            foreach (var weapon in weapons)
            {
                if (weapon != null && weapon.IsValid && weapon.Value != null && weapon.Value.IsValid)
                {
                    if (searched.Contains("knife") || searched.Contains("bayonet"))
                    {
                        //player.RemoveItemByDesignerName(weapon.Value.DesignerName, true);
                        RemoveKnife(player, true);
                        GiveKnifeToPlayer(player);
                        return;
                    }
                    //player.PrintToConsole("debug - searched " + searched + " and found " + weapon.Value.DesignerName);
                    if (weapon.Index <= 0 || !weapon.Value.DesignerName.Contains("weapon_") || !weapon.Value.DesignerName.Equals(searched)) continue;
                    //if (weapon.Value.AttributeManager.Item.ItemDefinitionIndex == 42 || weapon.Value.AttributeManager.Item.ItemDefinitionIndex == 59)
                    try
                    {
                        //player.PrintToConsole("debug - searched " + searched + " and found " + weapon.Value.DesignerName);
                        if (weapon.Value.DesignerName.Contains("knife") || weapon.Value.DesignerName.Contains("bayonet"))
                        {
                            //player.RemoveItemByDesignerName(weapon.Value.DesignerName, true);
                            RemoveKnife(player, true);
                            GiveKnifeToPlayer(player);
                            return;
                        }
                        else
                        {
                            if (!weaponDefindex.ContainsKey(weapon.Value.AttributeManager.Item.ItemDefinitionIndex)) continue;
                            int clip1, reservedAmmo;

                            clip1 = weapon.Value.Clip1;
                            reservedAmmo = weapon.Value.ReserveAmmo[0];

                            string weaponByDefindex = weaponDefindex[weapon.Value.AttributeManager.Item.ItemDefinitionIndex];
                            player.RemoveItemByDesignerName(weapon.Value.DesignerName, true);
                            CBasePlayerWeapon newWeapon = new(player.GiveNamedItem(weaponByDefindex));

                            Server.NextFrame(() =>
                            {
                                if (newWeapon == null) return;
                                try
                                {
                                    newWeapon.Clip1 = clip1;
                                    newWeapon.ReserveAmmo[0] = reservedAmmo;
                                }
                                catch (Exception)
                                { }
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
    }

    public HookResult OnPickup(CEntityIOOutput output, string name, CEntityInstance activator, CEntityInstance caller, CVariant value, float delay)
    {
        CCSPlayerController? player = Utilities.GetEntityFromIndex<CCSPlayerPawn>((int)activator.Index).OriginalController.Value;

        if (player == null || player.IsBot || player.IsHLTV)
            return HookResult.Continue;

        if (player == null || !player.IsValid || player.AuthorizedSteamID == null ||
             !g_playersKnife.ContainsKey((int)player.Index))
            return HookResult.Continue;

        CBasePlayerWeapon weapon = new(caller.Handle);

        if (weapon.AttributeManager.Item.ItemDefinitionIndex != 42 && weapon.AttributeManager.Item.ItemDefinitionIndex != 59)
            return HookResult.Continue;

        if (g_playersKnife.TryGetValue((int)player.Index, out var knife) && knife != "weapon_knife")
        {
            player.RemoveItemByDesignerName(weapon.DesignerName);
            AddTimer(0.2f, () => GiveKnifeToPlayer(player));
        }

        return HookResult.Continue;
    }

    private void GiveKnifeToPlayer(CCSPlayerController? player)
    {
        g_knifePickupCount[(int)player.Index]++;
        if (g_knifePickupCount[(int)player.Index] > 10) {
            player.PrintToChat("max knife changes reached this round");
            return; 
        }

        if (g_playersKnife.TryGetValue((int)player.Index, out var knife) && knife != "")
        {
            //Console.WriteLine("dado cuchillo "+knife);
            player.GiveNamedItem(knife);
        }
        else
        {
            var defaultKnife = (CsTeam)player.TeamNum == CsTeam.Terrorist ? "weapon_knife_t" : "weapon_knife";
            player.GiveNamedItem(defaultKnife);
        }
    }

    private bool PlayerHasKnife(CCSPlayerController? player)
    {
        if (player == null || !player.IsValid || player.PlayerPawn == null || !player.PlayerPawn.IsValid || !player.PawnIsAlive)
        {
            return false;
        }

        if (player.PlayerPawn?.Value == null || player.PlayerPawn?.Value.WeaponServices == null || player.PlayerPawn?.Value.ItemServices == null)
            return false;

        var weapons = player.PlayerPawn.Value.WeaponServices?.MyWeapons;
        if (weapons == null) return false;
        foreach (var weapon in weapons)
        {
            if (weapon != null && weapon.IsValid && weapon.Value != null && weapon.Value.IsValid)
            {
                if (weapon.Value.DesignerName.Contains("knife") || weapon.Value.DesignerName.Contains("bayonet"))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private KeyValuePair<string, Skin>? GetSkinSearched(int skinIndex)
    {

        for (int i = 0; i < skinList.Count; i++)
        {
            var current = skinList.ElementAt(i);

            if (int.Parse(current.Value.Index) == skinIndex) return current;
        }

        return null;
    }

    private bool RemoveKnife(CCSPlayerController player, bool shouldRemoveEntity)
    {
        CHandle<CBasePlayerWeapon>? item = null;
        if (player.PlayerPawn.Value == null || player.PlayerPawn.Value.WeaponServices == null) return false;

        foreach (var weapon in player.PlayerPawn.Value.WeaponServices.MyWeapons)
        {
            if (weapon is not { IsValid: true, Value.IsValid: true })
                continue;
            if (!weapon.Value.DesignerName.Contains("knife") && !weapon.Value.DesignerName.Contains("bayonet"))
                continue;

            item = weapon;
        }

        if (item != null && item.Value != null)
        {
            player.PlayerPawn.Value.RemovePlayerItem(item.Value);

            if (shouldRemoveEntity)
            {
                item.Value.Remove();
            }

            return true;
        }

        return false;
    }
}

public class WeaponInfo
{
    public int Paint { get; set; }
    public int Seed { get; set; }
    public float Wear { get; set; }
}

internal static class Utility
{
    internal static bool IsPlayerValid(CCSPlayerController? player)
    {
        return (player != null && player.IsValid && !player.IsBot && !player.IsHLTV);
    }
}

public class Skin
{
    public string Index { get; set; }
    public string Classes { get; set; }
}

public class SkinCollection
{
    public Dictionary<string, Skin> Skins { get; set; }
}

