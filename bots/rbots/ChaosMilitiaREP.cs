using System;
using RBot;
using System.Collections.Generic;
public class Script
{
    //-----------EDIT BELOW-------------//
    public string MapNumber = "69699";
    public string[] RequiredItems = { };
    public string[] EquippedItems = { };
    int[] SkillOrder = { 3, 1, 2, 4 };
    //-----------EDIT ABOVE-------------//

    int FarmLoop = 0;
    int SavedState = 0;
    public ScriptInterface bot => ScriptInterface.Instance;
    public void ScriptMain(ScriptInterface bot)
    {
        while (bot.Player.Loaded != true) { }
        if (bot.Player.Cell != "Wait") bot.Player.Jump("Wait", "Spawn");

        ConfigureBotOptions();
        ConfigureLiteSettings();

        SkillList(SkillOrder);
        EquipList(EquippedItems);
        UnbankList(RequiredItems);
        GetDropList(RequiredItems);

        while (!bot.ShouldExit())
        {
            while (bot.Player.GetFactionRank("Chaos Militia") < 10)
            {
                TempItemFarm("Doomwood Tabard", 10, "doomwood", MapNumber, "r8", "Right", 5776, "Doomwood Soldier");
                SafeQuestComplete(5776);
            }
            StopBot();
        }
        bot.Log($"[{DateTime.Now:HH:mm:ss}] Script stopped successfully.");
        StopBot();
    }

    /*------------------------------------------------------------------------------------------------------------
                                                     Invocable Functions
    ------------------------------------------------------------------------------------------------------------*/

    //These functions are used to perform a major action in AQW. 
    //All of them require at least one of the Auxilliary Functions listed below to be present in your script.
    //Some of the functions require you to pre-declare certain integers under "public class Script"
    //InvItemFarm and TempItemFarm will require some Background Functions to be present as well.
    //All of this information can be found inside the functions. Make sure to read.

    public void InvItemFarm(string ItemName, int ItemQuantity, string MapName, string MapNumber, string CellName, string PadName, int QuestID, string MonsterName)
    {
    //Farms you the specified quantity of the specified item with the specified quest accepted from specified monsters in the specified location. Saves States every ~5 minutes.

    //Must have the following functions in your script:
    //SafeMapJoin
    //SmartSaveState
    //SkillList
    //ExitCombat
    //GetDropList OR ItemWhitelist

    //Must have the following commands under public class Script:
    //int FarmLoop = 0;
    //int SavedState = 0;

    startFarmLoop:
        if (FarmLoop > 0) goto maintainFarmLoop;
        SavedState += 1;
        bot.Log($"[{DateTime.Now:HH:mm:ss}] Started Farming Loop {SavedState}.");
        goto maintainFarmLoop;

    breakFarmLoop:
        SmartSaveState(MapNumber);
        bot.Log($"[{DateTime.Now:HH:mm:ss}] Completed Farming Loop {SavedState}.");
        FarmLoop = 0;
        goto startFarmLoop;

    maintainFarmLoop:
        while (!bot.Inventory.Contains(ItemName, ItemQuantity))
        {
            FarmLoop += 1;
            if (bot.Map.Name != MapName) SafeMapJoin(MapName, MapNumber, CellName, PadName);
            if (bot.Player.Cell != CellName) bot.Player.Jump(CellName, PadName);
            bot.Quests.EnsureAccept(QuestID);
            bot.Options.AggroMonsters = true;
            bot.Player.Attack(MonsterName);
            if (FarmLoop > 8700) goto breakFarmLoop;
        }
    }

    public void TempItemFarm(string TempItemName, int TempItemQuantity, string MapName, string MapNumber, string CellName, string PadName, int QuestID, string MonsterName)
    {
    //Farms you the required quantity of the specified temp item with the specified quest accepted from specified monsters in the specified location.

    //Must have the following functions in your script:
    //SafeMapJoin
    //SmartSaveState
    //ExitCombat
    //SkillList
    //GetDropList OR ItemWhitelist

    //Must have the following commands under public class Script:
    //int FarmLoop = 0;
    //int SavedState = 0;

    startFarmLoop:
        if (FarmLoop > 0) goto maintainFarmLoop;
        SavedState += 1;
        bot.Log($"[{DateTime.Now:HH:mm:ss}] Started Farming Loop {SavedState}.");
        goto maintainFarmLoop;

    breakFarmLoop:
        SmartSaveState(MapNumber);
        bot.Log($"[{DateTime.Now:HH:mm:ss}] Completed Farming Loop {SavedState}.");
        FarmLoop = 0;
        goto startFarmLoop;

    maintainFarmLoop:
        while (!bot.Inventory.ContainsTempItem(TempItemName, TempItemQuantity))
        {
            FarmLoop += 1;
            if (bot.Map.Name != MapName) SafeMapJoin(MapName, MapNumber, CellName, PadName);
            if (bot.Player.Cell != CellName) bot.Player.Jump(CellName, PadName);
            bot.Quests.EnsureAccept(QuestID);
            bot.Options.AggroMonsters = true;
            bot.Player.Attack(MonsterName);
            if (FarmLoop > 8700) goto breakFarmLoop;
        }
    }

    public void SafeEquip(string ItemName)
    {
        //Equips an item.

        //Must have the following functions in your script:
        //ExitCombat

        while (!bot.Inventory.IsEquipped(ItemName))
        {
            ExitCombat();
            bot.Player.EquipItem(ItemName);
        }
    }

    public void SafePurchase(string ItemName, int ItemQuantityNeeded, string MapName, string MapNumber, int ShopID)
    {
        //Purchases the specified quantity of the specified item from the specified shop in the specified map. 

        //Must have the following functions in your script:
        //SafeMapJoin
        //ExitCombat

        while (!bot.Inventory.Contains(ItemName, ItemQuantityNeeded))
        {
            if (bot.Map.Name != MapName) SafeMapJoin(MapName, MapNumber, "Wait", "Spawn");
            ExitCombat();
            if (bot.Shops.IsShopLoaded != true)
            {
                bot.Shops.Load(ShopID);
                bot.Log($"[{DateTime.Now:HH:mm:ss}] Loaded Shop {ShopID}.");
            }
            bot.Shops.BuyItem(ItemName);
            bot.Log($"[{DateTime.Now:HH:mm:ss}] Purchased {ItemName} from Shop {ShopID}.");
        }
    }

    public void SafeSell(string ItemName, int ItemQuantityNeeded)
    {
        //Sells the specified item until you have the specified quantity.

        //Must have the following functions in your script:
        //ExitCombat

        int sellingPoint = ItemQuantityNeeded + 1;
        while (bot.Inventory.Contains(ItemName, sellingPoint))
        {
            ExitCombat();
            bot.Shops.SellItem(ItemName);
        }
    }

    public void SafeQuestComplete(int QuestID, int ItemID = -1)
    {
    //Attempts to complete the quest thrice. If it fails to complete, logs out. If it successfully completes, re-accepts the quest and checks if it can be completed again.

    //Must have the following functions in your script:
    //ExitCombat

    //Must have the following command under public class Script:
    //int QuestCounter = 0;

    maintainCompleteLoop:
        ExitCombat();
        bot.Quests.EnsureAccept(QuestID);
        bot.Quests.EnsureComplete(QuestID, ItemID, false, 3);
        if (bot.Quests.IsInProgress(QuestID))
        {
            bot.Log($"[{DateTime.Now:HH:mm:ss}] Failed to turn in Quest {QuestID}. Logging out.");
            bot.Player.Logout();
        }
        bot.Log($"[{DateTime.Now:HH:mm:ss}] Turned In Quest {QuestID} successfully.");
        bot.Quests.EnsureAccept(QuestID);
        bot.Sleep(1000);
        if (bot.Quests.CanComplete(QuestID)) goto maintainCompleteLoop;
    }

    public void StopBot(string MapName = "yulgar", string MapNumber = "2142069", string CellName = "Enter", string PadName = "Spawn")
    {
        //Stops the bot at yulgar if no parameters are set, or your specified map if the parameters are set.

        //Must have the following functions in your script:
        //SafeMapJoin
        //ExitCombat

        if (bot.Map.Name != MapName) SafeMapJoin(MapName, MapNumber, CellName, PadName);
        if (bot.Player.Cell != CellName) bot.Player.Jump(CellName, PadName);
        bot.Drops.RejectElse = false;
        bot.Options.LagKiller = false;
        bot.Options.AggroMonsters = false;
        bot.Log($"[{DateTime.Now:HH:mm:ss}] Bot stopped successfully.");
        bot.Exit();
    }

    /*------------------------------------------------------------------------------------------------------------
                                                    Auxilliary Functions
    ------------------------------------------------------------------------------------------------------------*/

    //These functions are used to perform small actions in AQW.
    //They are usually called upon by the Invocable Functions, but can be used separately as well.
    //Make sure to have them loaded if your Invocable Function states that they are required.

    public void ExitCombat()
    {
        //Exits Combat.

        bot.Options.AggroMonsters = false;
        bot.Player.Jump(bot.Player.Cell, bot.Player.Pad);
        while (bot.Player.State == 2) { }
    }

    public void SmartSaveState(string MapNumber = "2142069")
    {
        //Creates a quick Save State by joining a private /yulgar.

        //Must have the following functions in your script:
        //SafeMapJoin
        //ExitCombat

        string CurrentMap = bot.Map.Name;
        string CurrentCell = bot.Player.Cell;
        string CurrentPad = bot.Player.Pad;
        if (bot.Map.Name != "yulgar") SafeMapJoin("yulgar", "2142069", "Enter", "Spawn");
        else SafeMapJoin("tavern", "2142069", "Enter", "Spawn");
        SafeMapJoin(CurrentMap, MapNumber, CurrentCell, CurrentPad);
        bot.Log($"[{DateTime.Now:HH:mm:ss}] Successfully Saved State.");
    }

    public void SafeMapJoin(string MapName, string MapNumber, string CellName, string PadName)
    {
        //Joins the specified map.

        //Must have the following functions in your script:
        //ExitCombat

        while (bot.Map.Name != MapName)
        {
            ExitCombat();
            bot.Player.Join($"{MapName}-{MapNumber}", CellName, PadName);
            bot.Wait.ForMapLoad(MapName);
            bot.Sleep(500);
        }
        if (bot.Player.Cell != CellName) bot.Player.Jump(CellName, PadName);
        bot.Log($"[{DateTime.Now:HH:mm:ss}] Joined map {MapName}-{MapNumber}, positioned at the {PadName} side of cell {CellName}.");
    }

    /*------------------------------------------------------------------------------------------------------------
                                                    Background Functions
    ------------------------------------------------------------------------------------------------------------*/

    //These functions help you to either configure certain settings or run event handlers in the background.
    //It is highly recommended to have all these functions present in your script as they are very useful.
    //Some Invocable Functions may call or require the assistance of some Background Functions as well.
    //These functions are to be run at the very beginning of the bot under public class Script.

    public void ConfigureBotOptions(string PlayerName = "Bot By AuQW", string GuildName = "https://auqw.tk/")
    {
        //Recommended Default Bot Configurations.

        bot.Options.CustomName = PlayerName;
        bot.Options.CustomGuild = GuildName;
        bot.Options.LagKiller = true;
        bot.Options.SafeTimings = true;
        bot.Options.RestPackets = true;
        bot.Options.AutoRelogin = true;
        bot.Options.PrivateRooms = false;
        bot.Options.InfiniteRange = true;
        bot.Options.ExitCombatBeforeQuest = true;
        bot.Events.PlayerDeath += b => { ScriptManager.RestartScript(); };
        bot.Events.PlayerAFK += b => { ScriptManager.RestartScript(); };
    }

    public void ConfigureLiteSettings()
    {
        bot.Lite.Set("bUntargetSelf", true);
        bot.Lite.Set("bUntargetDead", true);
        bot.Lite.Set("bCustomDrops", false);
        bot.Lite.Set("bReaccept", false);
        bot.Lite.Set("bSmoothBG", true);
    }

    public void SkillList(params int[] Skillset)
    {
        //Spams Skills when in combat. You can get in combat by going to a cell with monsters in it with bot.Options.AggroMonsters enabled or using an attack command against one. 

        bot.RegisterHandler(1, b => {
            if (bot.Player.InCombat == true)
            {
                foreach (var Skill in Skillset)
                {
                    bot.Player.UseSkill(Skill);
                }
            }
        });
    }

    public void GetDropList(params string[] GetDropList)
    {
        //Checks if items in an array have dropped every second and picks them up if so. GetDropList is recommended.

        bot.RegisterHandler(4, b => {
            foreach (string Item in GetDropList)
            {
                if (bot.Player.DropExists(Item)) bot.Player.Pickup(Item);
            }
            bot.Player.RejectExcept(GetDropList);
        });
    }
    public void ItemWhiteList(params string[] WhiteList)
    {
        //Pick up items in an array when they dropped. May fail to pick up items that drop immediately after the same item is picked up. GetDropList is preferable instead.

        foreach (var Item in WhiteList)
        {
            bot.Drops.Add(Item);
        }
        bot.Drops.RejectElse = true;
        bot.Drops.Start();
    }

    public void EquipList(params string[] EquipList)
    {
        //Equips all items in an array.

        foreach (var Item in EquipList)
        {
            SafeEquip(Item);
        }
    }

    public void UnbankList(params string[] Items)
    {
        //Unbanks all items in an array after banking every other AC-tagged Misc item in the inventory.

        if (bot.Player.Cell != "Wait") bot.Player.Jump("Wait", "Spawn");
        while (bot.Player.State == 2) { }
        bot.Player.LoadBank();
        List<string> Whitelisted = new List<string>() { "Note", "Item", "Resource", "QuestItem", "ServerUse" };
        foreach (var item in bot.Inventory.Items)
        {
            if (!Whitelisted.Contains(item.Category.ToString())) continue;
            if (item.Name != "Treasure Potion" && item.Coins && !Array.Exists(Items, x => x == item.Name)) bot.Inventory.ToBank(item.Name);
        }
        foreach (var item in Items)
        {
            if (bot.Bank.Contains(item)) bot.Bank.ToInventory(item);
        }
    }
}