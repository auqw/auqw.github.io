using RBot;
using RBot.Flash;
using RBot.Items;
using RBot.Options;
using RBot.Quests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

public class FrostvaleStandalone
{
    /*  
        You can find more scripts made by me on my GitHub: https://github.com/BrenoHenrike/Rbot-Scripts
        If you want to donate me any quantity, you can use this link: https://www.paypal.com/donate/?hosted_button_id=QVQ4Q7XSH9VBY 
    
    --------- EDIT BELLOW --------- */
    public bool PrivateRooms = true;
    public bool AntiLag = true;
    public int HuntDelay = 1500;
    public int ActionDelay = 700;
    public int ExitCombatDelay = 1600;
    /* ! USE ONLY SINGLE TARGET CLASSES ! */
    public string SoloClass = "Generic";
    // Create one using Skills > Advanced
    public string SoloClassSkills = "1 | 2 | 3 | 4 | Mode Optimistic";
    public int SkillTimeout = -1;
    /* --------- EDIT ABOVE --------- */

    public string OptionsStorage = "Frostvale";
    public bool DontPreconfigure = true;

    public List<IOption> Options = new List<IOption>()
    {
        new Option<int>("startQuest", "Quest start index", "This will save the progress through the script.")
    };

    public ScriptInterface Bot => ScriptInterface.Instance;
    public List<ItemBase> CurrentRequirements = new List<ItemBase>();

    public static readonly int[] qIDs =
    {
        //* /Join IceCave
            155, // Rescue Blizzy
            156, // Scary Snow Men
            157, // Moglin Popsicles
            158, // Crystal Spider
            159, // Fluffy Bears
            160, // Blue Eyed Beast
        //* /Join Factory
            161, // Trouble Makers
            162, // Bad Ice Cream
            163, // Greedy Sneevil
            164, // Shadow Figure
        //* /Join Frostvale
            456, // 'Twas the night before Frostval
            457, // Find Page 2
            458, // Find Page 3
            459, // Find Page 4
            460, // Find Page 5
            461, // Find Page 6
            905, // Spirit Abducted
        //* /Join SnowGlobe
            906, // Shaking the Globes
            907, // A Demonstration
            908, // Hearts of Ice
            909, // Defeat Garaja
        //* /Join GoldenRuins
            910, // Springing Traps
            911, // Frost Lions
            912, // Onslaught Keyrings
            913, // Defeat Lionfang
        //* /Join Alpine
            1508, // Snow Way to Know Where to Go
            1509, // Arming the Undead Army
            1510, // Cold As A Corpse
            1511, // Pretty Pretty Undead Princess Decor
            1512, // Deadifying Frost Lions
            1516, // Defiant Undead Deserters
            1513, // Forest Guardian Gauntlet
        //* /Join IceVolcano
            1519, // Snow Turning Back!
            1520, // Venom in Your Veins
            1521, // Song of the Frozen Heart
        //* /Join SnowyVale
            2522, // Locate Kezeroth
            2523, // Chronoton Detection
            2524, // Core Knowledge
            2525, // Temporal Revelation
            2526, // Before the Darkest Hour
        //* /Join FrostDeep
            2527, // Heart of Ice
            2528, // Absolute Zero Success
            2529, // Dirty Secret
            2530, // Frozen Venom
            2531, // Rune-ing His Plan
            2532, // Deadly Beauty
            2533, // Cold-Hearted Trophies
            2534, // Warmth in the Cold
            2535, // Icy Prizes
            2536, // Fading Magic
            2537, // FrostDeep Dwellers
            2538, // A Breather
            2539, // Raiders From ForstDeep
            2540, // 8 Legged Frost Freaks
            2541, // Freezing the Stone
            2542, // Can You Feel the Chill Tonight?
            2543, // Shrouded on Ice
            2544, // Hard Fight for a Cold Truth
            2545, // Sand and Shardin' Bones
            2546, // Older and Colder
            2547, // The Sword Of Hope
        //* /Join IceRise
            2576, // A Little Warmth and Light
            2577, // Behind Locked Doors
            2578, // The Lost Key
            2579, // Uncovering Pages of The Past
            2580, // We Know Where To Look
            2581, // A Terrible Hiding Place
            2582, // Face Kezeroth
    };

    public void ScriptMain(ScriptInterface bot)
    {
        SetOptions();

        int questStart = bot.Config.Get<int>("startQuest");

        for (int i = questStart; i < qIDs.Length; i++)
        {
            if (i != qIDs.Length - 1 && i != 34 && bot.Quests.IsUnlocked(qIDs[i + 1]))
                continue;
            bot.Config.Set("startQuest", i);
            Logger($"Starting {i}");
            EnsureAccept(qIDs[i]);
            switch (i)
            {
                case 0: // Rescue Blizzy
                    SmartKillMonster(qIDs[i], "icecave", "Frosty");
                    break;
                case 1: // Scary Snow Men
                    SmartKillMonster(qIDs[i], "icecave", "Snow Golem");
                    break;
                case 2: // Moglin Popsicles
                    SmartKillMonster(qIDs[i], "icecave", "Frozen Moglin");
                    break;
                case 3: // Crystal Spider
                    SmartKillMonster(qIDs[i], "icecave", "Ice Spider");
                    break;
                case 4: // Fluffy Bears
                    SmartKillMonster(qIDs[i], "icecave", "Polar Bear");
                    break;
                case 5: // Blue Eyed Beast
                    SmartKillMonster(qIDs[i], "icecave", "Frost Dragon");
                    break;
                case 6: // Trouble Makers
                    SmartKillMonster(qIDs[i], "factory", "Sneevil Toy Maker");
                    break;
                case 7: // Bad Ice Cream
                    SmartKillMonster(qIDs[i], "factory", "Snow Golem");
                    break;
                case 8: // Greedy Sneevil
                    SmartKillMonster(qIDs[i], "factory", "Ebilsneezer");
                    break;
                case 9: // Shadow Figure
                    SmartKillMonster(qIDs[i], "frost", "FrostScythe");
                    break;
                case 10: // 'Twas the night before Frostval
                    SmartKillMonster(qIDs[i], "icecave", "Frosty");
                    break;
                case 11: // Find Page 2
                    GetMapItem(85, map: "yulgar");
                    SmartKillMonster(qIDs[i], "icecave", "Frozen Moglin");
                    break;
                case 12: // Find Page 3
                    GetMapItem(86, map: "battleontown");
                    SmartKillMonster(qIDs[i], "icecave", "Frozen Moglin");
                    break;
                case 13: // Find Page 4
                    SmartKillMonster(qIDs[i], "factory", "Sneevil Toy Maker");
                    break;
                case 14: // Find Page 5
                    SmartKillMonster(qIDs[i], "northlandlight", "Santy Claws");
                    break;
                case 15: // Find Page 6
                    GetMapItem(87, map: "battleontown");
                    SmartKillMonster(qIDs[i], "icecave", "Frozen Moglin");
                    break;
                case 16: // Spirit Abducted
                    break;
                case 17: // Shaking the Globes
                    GetMapItem(243, 10, "snowglobe");
                    SmartKillMonster(qIDs[i], "snowglobe", "Snow Golem");
                    break;
                case 18: // A Demonstration
                    SmartKillMonster(qIDs[i], "snowglobe", "Snow Golem");
                    break;
                case 19: // Hearts of Ice
                    SmartKillMonster(qIDs[i], "snowglobe", "Snowman Soldier");
                    break;
                case 20: // Defeat Garaja
                    SmartKillMonster(qIDs[i], "snowglobe", "Garaja");
                    break;
                case 21: // Springing Traps
                    GetMapItem(244, 10, "goldenruins");
                    SmartKillMonster(qIDs[i], "goldenruins", "Golden Warrior");
                    break;
                case 22: // Frost Lions
                    SmartKillMonster(qIDs[i], "goldenruins", "Frost Lion");
                    break;
                case 23: // Onslaught Keyrings
                    SmartKillMonster(qIDs[i], "goldenruins", "Golden Warrior");
                    break;
                case 24: // Defeat Lionfang
                    SmartKillMonster(qIDs[i], "goldenruins", "Maximillian Lionfang");
                    break;
                case 25: // Snow Way to Know Where to Go
                    GetMapItem(758, map: "alpine");
                    break;
                case 26: // Arming the Undead Army
                    SmartKillMonster(qIDs[i], "alpine", "Glacier Mole");
                    break;
                case 27: // Cold As A Corpse
                    GetMapItem(759, 10, "alpine");
                    break;
                case 28: // Pretty Pretty Undead Princess Decor
                    GetMapItem(760, 13, "alpine");
                    break;
                case 29: // Deadfying Frost Lions
                    SmartKillMonster(qIDs[i], "alpine", "Frost Lion");
                    break;
                case 30: // Defiant Undead Deserters
                    SmartKillMonster(qIDs[i], "alpine", "Frozen Deserter");
                    break;
                case 31: // Forest Guadian Gauntlet
                    SmartKillMonster(qIDs[i], "alpine", "Wendigo");
                    break;
                case 32: // Snow Turning Back!
                    GetMapItem(761, 10, "icevolcano");
                    SmartKillMonster(qIDs[i], "icevolcano", new[] { "Snow Golem", "Dead-ly Ice Elemental" });
                    break;
                case 33: // Venom in Your Veins
                    SmartKillMonster(qIDs[i], "icevolcano", "Ice Symbiote");
                    break;
                case 34: // Song of the Frozen Heart
                    SmartKillMonster(qIDs[i], "icevolcano", "Dead Morice");
                    break;
                case 35: // Locate Kezeroth
                    GetMapItem(1584, map: "snowyvale");
                    break;
                case 36: // Chronoton Detection
                    SmartKillMonster(qIDs[i], "snowyvale", "Polar Golem");
                    break;
                case 37: // Core Knowledge
                    GetMapItem(1585, 6, "snowyvale");
                    break;
                case 38: // Temporal Revelation
                    GetMapItem(1586, map: "snowyvale");
                    break;
                case 39: // Before the Darkest Hour
                    GetMapItem(1587, map: "frostdeep");
                    break;
                case 40: // Heart of Ice
                    SmartKillMonster(qIDs[i], "frostdeep", new[] { "Polar Golem", "Polar Elemental" });
                    break;
                case 41: // Absolute Zero Success
                    SmartKillMonster(qIDs[i], "frostdeep", new[] { "Temple Prowler", "Polar Elemental", "Polar Golem" });
                    break;
                case 42: // Dirty Secret
                    SmartKillMonster(qIDs[i], "frostdeep", new[] { "Temple Prowler", "Polar Mole" });
                    break;
                case 43: // Frozen Venom
                    SmartKillMonster(qIDs[i], "frostdeep", new[] { "Polarwyrm Rider", "Polar Spider" });
                    break;
                case 44: // Rune-ing His Plan
                    SmartKillMonster(qIDs[i], "frostdeep", "Ancient Golem");
                    break;
                case 45: // Deadly Beauty
                    SmartKillMonster(qIDs[i], "frostdeep", new[] { "Polar Elemental", "Polar Golem", "Polar Golem" });
                    break;
                case 46: // Cold-Hearted Trophies
                    SmartKillMonster(qIDs[i], "frostdeep", new[] { "Polar Mole", "Temple Prowler", "Temple Prowler" });
                    break;
                case 47: // Warmth in the Cold
                    SmartKillMonster(qIDs[i], "frostdeep", new[] { "Temple Spider", "Temple Maggot" });
                    break;
                case 48: // Icy Prizes
                    SmartKillMonster(qIDs[i], "frostdeep", new[] { "Temple Prowler", "Temple Maggot" });
                    break;
                case 49: // Fading Magic
                    SmartKillMonster(qIDs[i], "frostdeep", new[] { "Ancient Golem", "Ancient Golem" });
                    break;
                case 50: // FrostDeep Dwellers
                    SmartKillMonster(qIDs[i], "frostdeep", new[] { "Polarwyrm Rider", "Polar Mole", "Polar Mole" });
                    break;
                case 51: // A Breather
                    SmartKillMonster(qIDs[i], "frostdeep", new[] { "Polar Mole", "Temple Spider", "Polar Spider" });
                    break;
                case 52: // Raiders From FrostDeep
                    SmartKillMonster(qIDs[i], "frostdeep", new[] { "Polar Draconian", "Temple Maggot" });
                    break;
                case 53: // 8 Legged Frost Freaks
                    SmartKillMonster(qIDs[i], "frostdeep", new[] { "Temple Spider", "Polar Spider" });
                    break;
                case 54: // Freezing the Stone
                    SmartKillMonster(qIDs[i], "frostdeep", new[] { "Ancient Golem", "Ancient Golem" });
                    break;
                case 55: // Can You Feel the Chill Tonight?
                    SmartKillMonster(qIDs[i], "frostdeep", new[] { "Temple Prowler", "Polar Elemental", "Polar Elemental" });
                    break;
                case 56: // Shrouded in Ice
                    SmartKillMonster(qIDs[i], "frostdeep", new[] { "Ancient Maggot", "Ancient Maggot" });
                    break;
                case 57: // Hard Fight for a Cold Truth
                    SmartKillMonster(qIDs[i], "frostdeep", new[] { "Ancient Prowler", "Ancient Prowler" });
                    break;
                case 58: // Sand and Shardin' Bones
                    SmartKillMonster(qIDs[i], "frostdeep", new[] { "Ancient Mole", "Ancient Mole" });
                    break;
                case 59: // Older and Colder
                    SmartKillMonster(qIDs[i], "frostdeep", new[] { "Ancient Mole", "Ancient Prowler", "Ancient Maggot" });
                    break;
                case 60: // The Sword Of Hope
                    SmartKillMonster(qIDs[i], "frostdeep", new[] { "Ancient Terror", "Ancient Terror" });
                    break;
                case 61: // A Little Warmth and Light
                    GetMapItem(1592, 5, "icerise");
                    break;
                case 62: // Behind Locked Doors
                    GetMapItem(1593, map: "icerise");
                    break;
                case 63: // The Lost Key
                    SmartKillMonster(qIDs[i], "icerise", "Polar Golem");
                    break;
                case 64: // Uncovering Pages Of The Past
                    SmartKillMonster(qIDs[i], "icerise", new[] { "Polar Golem", "Polar Elemental", "Arctic Direwolf" });
                    break;
                case 65: // We Know Where To Look
                    SmartKillMonster(qIDs[i], "icerise", new[] { "Polar Golem", "Polar Elemental", "Arctic Direwolf" });
                    break;
                case 66: // A Terrible Hiding Place
                    SmartKillMonster(qIDs[i], "icerise", "Arctic Direwolf");
                    break;
                case 67: // Face Kezeroth!
                    SmartKillMonster(qIDs[i], "icerise", "Kezeroth");
                    break;
            }
            EnsureComplete(qIDs[i]);
            Logger($"Finished {i}");
        }

        SetOptions(false);
    }

    /// <summary>
	/// Set commom bot options to desired value
	/// </summary>
	/// <param name="changeTo">Value the options will be changed to</param>
	public void SetOptions(bool changeTo = true)
    {
        // Common Options
        Bot.Options.PrivateRooms = PrivateRooms;
        Bot.Options.SafeTimings = changeTo;
        Bot.Options.RestPackets = changeTo;
        Bot.Options.AutoRelogin = changeTo;
        Bot.Options.InfiniteRange = changeTo;
        Bot.Options.SkipCutscenes = changeTo;
        Bot.Options.ExitCombatBeforeQuest = changeTo;
        Bot.Drops.RejectElse = changeTo;
        Bot.Lite.UntargetDead = changeTo;
        Bot.Lite.UntargetSelf = changeTo;
        Bot.Lite.ReacceptQuest = false;

        if (changeTo)
        {
            Logger("Bot Started");

            Bot.Options.HuntDelay = HuntDelay;

            Bot.RegisterHandler(2, b =>
            {
                if (b.ShouldExit())
                    StopBot();
            }, "Stop Handler");

            Bot.RegisterHandler(15, b =>
            {
                if (b.Player.Cell.Contains("Cut"))
                {
                    FlashUtil.Call("skipCutscenes");
                    if(b.Options.LagKiller == false)
                    {
                        b.Options.LagKiller = true;
                        b.Options.LagKiller = false;
                    }
                    b.Sleep(1000);
                    b.Player.Jump("Enter", "Spawn");
                }
            }, "Skip Cutscenes");

            Bot.Player.LoadBank();
            Bot.Runtime.BankLoaded = true;
            EquipClass(SoloClass);
            // Anti-lag option
            if (AntiLag)
            {
                Bot.Options.LagKiller = true;
                Bot.SetGameObject("stage.frameRate", 10);
                if (!Bot.GetGameObject<bool>("ui.monsterIcon.redX.visible"))
                    Bot.CallGameFunction("world.toggleMonsters");
            }

            Logger("Bot Configured");
        }
        else
            StopBot(true);
    }

    public void EquipClass(string className)
    {
        if (className.ToLower() != "generic"
            && Bot.Inventory.CurrentClass.Name.ToLower() != className.ToLower())
        {
            JumpWait();
            while (!Bot.Inventory.IsEquipped(className))
            {
                Bot.Player.EquipItem(className);
                Bot.Sleep(ActionDelay);
            }
            Logger($"{className} equipped");
        }
        Bot.Skills.StartAdvanced(SoloClassSkills, SkillTimeout);
    }

    /// <summary>
	/// Ensures you are out of combat before accepting the quest
	/// </summary>
	/// <param name="questID">ID of the quest to accept</param>
	public bool EnsureAccept(int questID)
    {
        if (Bot.Quests.IsInProgress(questID))
            return true;
        if (questID <= 0)
            return false;
        JumpWait();
        Bot.Sleep(ActionDelay);
        return Bot.Quests.EnsureAccept(questID, tries: 5);
    }

    /// <summary>
	/// Ensures you are out of combat before completing the quest
	/// </summary>
	/// <param name="questID">ID of the quest to complete</param>
	/// <param name="itemID">ID of the choosable reward item</param>
	public bool EnsureComplete(int questID, int itemID = -1)
    {
        if (questID <= 0)
            return false;
        JumpWait();
        Bot.Sleep(ActionDelay);
        return Bot.Quests.EnsureComplete(questID, itemID, tries: 5);
    }

    /// <summary>
	/// Jump to wait room and wait <see cref="ExitCombatDelay"/>
	/// </summary>
	public void JumpWait()
    {
        if (Bot.Player.Cell != "Wait")
        {
            Bot.Player.Jump("Wait", "Spawn");
            Bot.Sleep(ExitCombatDelay);
        }
    }

    /// <summary>
	/// Sends a getMapItem packet for the specified item
	/// </summary>
	/// <param name="itemID">ID of the item</param>
	/// <param name="quant">Desired quantity of the item</param>
	/// <param name="map">Map where the item is</param>
	public void GetMapItem(int itemID, int quant = 1, string map = null)
    {
        if (map != null)
            Bot.Player.Join(map);
        Bot.Sleep(ActionDelay);
        for (int i = 0; i < quant; i++)
        {
            Bot.Map.GetMapItem(itemID);
            Bot.Sleep(1000);
        }
        Logger($"Map item {itemID}({quant}) acquired");
    }

    /// <summary>
	/// Kills the specified monsters in the map for the quest requirements
	/// </summary>
	/// <param name="questID">ID of the quest</param>
	/// <param name="map">Map where the <paramref name="monsters"/> are</param>
	/// <param name="monsters">Array of the monsters to kill</param>
	/// <param name="iterations">How many times it should kill the monster until going to the next</param>
	/// <param name="completeQuest">Whether complete the quest after killing all monsters</param>
	public void SmartKillMonster(int questID, string map, string[] monsters, int iterations = 20, bool completeQuest = false)
    {
        EnsureAccept(questID);
        _AddRequirement(questID);
        Bot.Player.Join(map);
        foreach (string monster in monsters)
            _SmartKill(monster, iterations);
        if (completeQuest)
            EnsureComplete(questID);
        CurrentRequirements.Clear();
    }

    private void _AddRequirement(int questID)
    {
        if (questID > 0)
        {
            Quest quest = Bot.Quests.EnsureLoad(questID);
            if (quest == null)
                Logger($"Quest [{questID}] doesn't exist", messageBox: true, stopBot: true);
            List<string> reqItems = new List<string>();
            quest.AcceptRequirements.ForEach(item => reqItems.Add(item.Name));
            quest.Requirements.ForEach(item =>
            {
                if (!CurrentRequirements.Where(i => i.Name == item.Name).Any())
                {
                    if(!item.Temp)
                        reqItems.Add(item.Name);
                    CurrentRequirements.Add(item);
                }
            });
            AddDrop(reqItems.ToArray());
        }
    }

    /// <summary>
	/// Adds drops to the pickup list, unbank the items and restart the Drop Grabber
	/// </summary>
	/// <param name="items">Items to add</param>
	public void AddDrop(params string[] items)
    {
        Bot.Drops.Stop();
        Unbank(items);
        foreach (string item in items)
            Bot.Drops.Add(item);
        Bot.Drops.Start();
    }

    /// <summary>
	/// Check the Bank, Inventory and Temp Inventory for the item
	/// </summary>
	/// <param name="item">Name of the item</param>
	/// <param name="quant">Desired quantity</param>
	/// <param name="toInv">Whether or not send the item to Inventory</param>
	/// <returns>Returns whether the item exists in the desired quantity in the bank and inventory</returns>
	public bool CheckInventory(string item, int quant = 1, bool toInv = true)
    {
        if (Bot.Inventory.ContainsTempItem(item, quant))
            return true;
        if (Bot.Bank.Contains(item))
        {
            if (!toInv)
                return true;
            Unbank(item);
        }
        if (Bot.Inventory.Contains(item, quant))
            return true;
        return false;
    }

    /// <summary>
	/// Move items from bank to inventory
	/// </summary>
	/// <param name="items">Items to move</param>
	public void Unbank(params string[] items)
    {
        JumpWait();
        Bot.Player.OpenBank();
        foreach (string item in items)
        {
            if (Bot.Bank.Contains(item))
            {
                Bot.Sleep(ActionDelay);
                while (!Bot.Inventory.Contains(item))
                {
                    Bot.Bank.ToInventory(item);
                    Bot.Wait.ForBankToInventory(item);
                    Bot.Sleep(ActionDelay);
                }
                Logger($"{item} moved from bank");
            }
        }
    }

    /// <summary>
    /// Kills the specified monsters in the map for the quest requirements
    /// </summary>
    /// <param name="questID">ID of the quest</param>
    /// <param name="map">Map where the <paramref name="monster"/> is</param>
    /// <param name="monster">Monster to kill</param>
    /// <param name="iterations">How many times it should kill the monster until exiting</param>
    /// <param name="completeQuest">Whether complete the quest after killing all monsters</param>
    public void SmartKillMonster(int questID, string map, string monster, int iterations = 20, bool completeQuest = false)
    {
        EnsureAccept(questID);
        _AddRequirement(questID);
        Bot.Player.Join(map);
        _SmartKill(monster, iterations);
        if (completeQuest)
            EnsureComplete(questID);
        CurrentRequirements.Clear();
    }

    private void _SmartKill(string monster, int iterations = 20)
    {
        bool repeat = true;
        for (int j = 0; j < iterations; j++)
        {
            if (CurrentRequirements.Count == 0)
                break;
            if (CurrentRequirements.Count == 1)
            {
                if (_RepeatCheck(ref repeat, 0))
                    break;
                _MonsterHunt(ref repeat, monster, CurrentRequirements[0].Name, CurrentRequirements[0].Quantity, CurrentRequirements[0].Temp, 0);
                break;
            }
            else
            {
                for (int i = CurrentRequirements.Count - 1; i >= 0; i--)
                {
                    if (j == 0 && (CheckInventory(CurrentRequirements[i].Name, CurrentRequirements[i].Quantity)))
                    {
                        CurrentRequirements.RemoveAt(i);
                        continue;
                    }
                    if (j != 0 && CheckInventory(CurrentRequirements[i].Name))
                    {
                        if (_RepeatCheck(ref repeat, i))
                            break;
                        _MonsterHunt(ref repeat, monster, CurrentRequirements[i].Name, CurrentRequirements[i].Quantity, CurrentRequirements[i].Temp, i);
                        break;
                    }
                }
            }
            if (!repeat)
                break;

            Bot.Player.Hunt(monster);
            Bot.Player.Pickup(CurrentRequirements.Where(item => !item.Temp).Select(item => item.Name).ToArray());
            Bot.Sleep(ActionDelay);
        }
    }

    private void _MonsterHunt(ref bool shouldRepeat, string monster, string itemName, int quantity, bool isTemp, int index)
    {
        Logger($"Hunting {monster} for {itemName} ({quantity}) [Temp = {isTemp}]");
        Bot.Player.HuntForItem(monster, itemName, quantity, isTemp);
        CurrentRequirements.RemoveAt(index);
        shouldRepeat = false;
    }

    private bool _RepeatCheck(ref bool shouldRepeat, int index)
    {
        if (CheckInventory(CurrentRequirements[index].Name, CurrentRequirements[index].Quantity))
        {
            CurrentRequirements.RemoveAt(index);
            shouldRepeat = false;
            return true;
        }
        return false;
    }



    /// <summary>
	/// Logs a line of text to the script log with time, method from where it's called and a message
	/// </summary>
	public void Logger(string message = "", [CallerMemberName] string caller = null, bool messageBox = false, bool stopBot = false)
    {
        Bot.Log($"[{DateTime.Now:HH:mm:ss}] ({caller})  {message}");
        if (messageBox)
            Message(message, caller);
        if (stopBot)
            StopBot(true);
    }

    /// <summary>
    /// Creates a Message Box with the desired text and caption
    /// </summary>
    /// <param name="message">Message to display</param>
    /// <param name="caption">Title of the box</param>
    public void Message(string message, string caption)
    {
        if (DialogResult.OK == MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000))
            return;
    }

    public void StopBot(bool removeStopHandler = false)
    {
        if (removeStopHandler)
            Bot.Handlers.RemoveAll(handler => handler.Name == "Stop Handler");
        Bot.Player.Join("battleon");
        if (AntiLag)
        {
            Bot.SetGameObject("stage.frameRate", 60);
            if (Bot.GetGameObject<bool>("ui.monsterIcon.redX.visible"))
                Bot.CallGameFunction("world.toggleMonsters");
        }
        Bot.Options.PrivateRooms = false;
        Bot.Options.AutoRelogin = false;
        Bot.Options.LagKiller = false;
        Bot.Options.LagKiller = true;
        Bot.Options.LagKiller = false;
        Logger("Bot Stopped Successfully", messageBox: true);
        ScriptManager.StopScript();
    }
}