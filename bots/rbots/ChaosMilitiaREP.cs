using System;
using RBot;
using System.Collections.Generic;
public class Script
{
    public string mapNumber = "6969";
    //Make sure to Turn OFF "Reaccept Quests Upon Turnin" and Turn ON "Auto Untarget Dead Targets" + "Auto Untarget Self" from Advanced Settings in AQW.

    public string[] requiredItems = { 
        "Militia Merit",
        "Undead Energy"
    };
    int[] skillOrder = { 3, 1, 2, 4 };
    int QuestCounter = 0;
    public ScriptInterface bot => ScriptInterface.Instance;
    public void ScriptMain(ScriptInterface bot)
    {

        if (bot.Player.Cell != "Wait") bot.Player.Jump("Wait", "Spawn");

        AntiDeathAFK();
        OptionsConfigure();

        SkillList(skillOrder);
        Whitelist(requiredItems);
        Unbank(false, requiredItems);

        while (!bot.ShouldExit())
        {
            while (bot.Player.GetFactionRank("Chaos Militia") < 10)
            {
                TempItemFarm("Doomwood Tabard", 10, "doomwood", mapNumber, "r8", "Right", 5776, "Doomwood Soldier");
                SafeQuestComplete(5776);
            }
        }
        bot.Log($"[{DateTime.Now:HH:mm:ss}] Script stopped successfully.");
        bot.Exit();

    }

    //------------------------------------------------------------------------------------------------------------
    //System Functions
    //------------------------------------------------------------------------------------------------------------

    public void OptionsConfigure(bool LagKiller = true, bool SafeTimings = true, bool RestPackets = true, bool AutoRelogin = true, bool InfiniteRange = true, bool PrivateRooms = false, bool ExitCombatBeforeQuest = true)
    {
        bot.Options.LagKiller = LagKiller;
        bot.Options.SafeTimings = SafeTimings;
        bot.Options.RestPackets = RestPackets;
        bot.Options.AutoRelogin = AutoRelogin;
        bot.Options.PrivateRooms = PrivateRooms;
        bot.Options.InfiniteRange = InfiniteRange;
        bot.Options.ExitCombatBeforeQuest = ExitCombatBeforeQuest;
    }

    public void AntiDeathAFK()
    {
        bot.Events.PlayerDeath += b => {
            ScriptManager.RestartScript();
        };
        bot.Events.PlayerAFK += b => {
            ScriptManager.RestartScript();
        };
    }

    public void Whitelist(params string[] Items)
    {
        bot.RegisterHandler(4, b => {
            foreach (string item in Items)
            {
                if (bot.Player.DropExists(item)) bot.Player.Pickup(item);
            }
            bot.Player.RejectExcept(Items);
        });
    }

    public void Unbank(bool AddToDrops, params string[] Items)
    {
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
            if (AddToDrops) bot.Drops.Add(item);
        }
    }

    public void SkillList(params int[] Skillset)
    {
        bot.RegisterHandler(1, b => {
            if (bot.Player.InCombat == true)
            {
                foreach (var skill in Skillset)
                {
                    bot.Player.UseSkill(skill);
                }
            }
        });
    }

    public void InvItemFarm(string itemName, int itemQuantity, string mapName, string mapNumber, string cellName, string padName, int questID, string monsterName)
    {
        while (!bot.Inventory.Contains(itemName, itemQuantity))
        {
            if (bot.Map.Name != mapName) SafeMapJoin(mapName, mapNumber, cellName, padName);
            if (bot.Player.Cell != cellName) bot.Player.Jump(cellName, padName);
            bot.Quests.EnsureAccept(questID);
            bot.Options.AggroMonsters = true;
            bot.Player.Attack(monsterName);
        }
    }
    public void TempItemFarm(string tempName, int tempQuantity, string mapName, string mapNumber, string cellName, string padName, int questID, string monsterName)
    {
        while (!bot.Inventory.ContainsTempItem(tempName, tempQuantity))
        {
            if (bot.Map.Name != mapName) SafeMapJoin(mapName, mapNumber, cellName, padName);
            if (bot.Player.Cell != cellName) bot.Player.Jump(cellName, padName);
            bot.Quests.EnsureAccept(questID);
            bot.Options.AggroMonsters = true;
            bot.Player.Attack(monsterName);
        }
    }

    public void SafePurchase(string itemName, int itemQuantity, string mapName, string mapNumber, int shopID)
    {
        while (!bot.Inventory.Contains(itemName, itemQuantity))
        {
            bot.Options.AggroMonsters = false;
            if (bot.Map.Name != mapName) SafeMapJoin(mapName, mapNumber, "Wait", "Spawn");
            if (bot.Player.Cell != "Wait") bot.Player.Jump("Wait", "Spawn");
            while (bot.Player.State == 2) { }
            if (bot.Shops.IsShopLoaded != true)
            {
                bot.Shops.Load(shopID);
                bot.Log($"[{DateTime.Now:HH:mm:ss}] Loaded Shop {shopID}.");
            }
            bot.Shops.BuyItem(itemName);
            bot.Log($"[{DateTime.Now:HH:mm:ss}] Purchased {itemName} from Shop {shopID}.");
        }
    }

    public void SafeMapJoin(string mapName, string mapNumber, string cellName, string padName)
    {
    maintainJoinLoop:
        bot.Options.AggroMonsters = false;
        if (bot.Player.Cell != "Wait") bot.Player.Jump("Wait", "Spawn");
        while (bot.Player.State == 2) { }
        bot.Player.Join($"{mapName}-{mapNumber}", cellName, padName);
        bot.Wait.ForMapLoad(mapName);
        if (bot.Map.Name != mapName) goto maintainJoinLoop;
        if (bot.Player.Cell != cellName) bot.Player.Jump(cellName, padName);
        bot.Log($"[{DateTime.Now:HH:mm:ss}] Joined map {mapName}-{mapNumber}, positioned at the {padName} of cell {cellName}.");
    }

    public void SafeQuestComplete(int questID, int itemID = -1)
    {
    maintainCompleteLoop:
        bot.Options.AggroMonsters = false;
        bot.Quests.EnsureAccept(questID);
        bot.Quests.EnsureComplete(questID, itemID, false, 3);
        if (bot.Quests.IsInProgress(questID))
        {
            bot.Log($"[{DateTime.Now:HH:mm:ss}] Failed to turn in Quest {questID}. Logging out.");
            bot.Player.Logout();
        }
        QuestCounter += 1;
        bot.Log($"[{DateTime.Now:HH:mm:ss}] Turned In Quest {questID} successfully {QuestCounter} time(s).");
        bot.Quests.EnsureAccept(questID);
        bot.Sleep(700);
        if (bot.Quests.CanComplete(questID)) goto maintainCompleteLoop;
    }

}