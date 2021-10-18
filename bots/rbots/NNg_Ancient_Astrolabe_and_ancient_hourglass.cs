using System;
using RBot;
using System.Collections.Generic;

//Documentation Website:
/*https://rodit.github.io/rbot-scripts/
*/

public class Script
{
    //Make sure to Turn OFF "Reaccept Quests Upon Turnin" and Turn ON "Auto Untarget Dead Targets" + "Auto Untarget Self" from Advanced Settings in AQW.

    // Set your variables below:
    
    public int ancientHourglassQuantity = 204; // (max stack:300)
    public bool toggleGrindAncientHourGlass = true; //---> false to skip Ancient Hourglass grinding
    public int ancientAstrolabeQuantity = 184; // (max stack:300)
    public bool toggleGrindAncientAstrolabe = true;//---> false to skip Ancient Astrolabe grinding
    public string soloClass = "Void Highlord";//---> edit your solo class
    int[] skillOrder = { 1, 2, 4 }; //solo skillset

    public string mapNumber = "1e99";//---> map number
    public bool getLightningLord = true; // grind for LightningLord Armor, set False to skip
    public bool otherExtrikiDrops = true; // get all drops from Extriki monster: LightningLord Helm, LightningLord Locks, LightningLord Rune, set False to skip

    /*
    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    */




    public string[] RequiredItems = {"Ancient Hourglass", "Ancient Astrolabe", "LightningLord", "LightningLord Helm", "LightningLord Locks", "LightningLord Rune"};
    public string[] EquippedItems = { }; //equip boost
    

    int FarmLoop = 0;
    int SavedState = 0;
    int QuestCounter = 0;
    public ScriptInterface bot => ScriptInterface.Instance;
    public void ScriptMain(ScriptInterface bot)
    {
        mapNumber = MapNumberConverter(mapNumber);
        while (bot.Player.Loaded != true) { }
        if (bot.Player.Cell != "Wait") bot.Player.Jump("Wait", "Spawn");
        ConfigureOptions();
        SkillList(skillOrder);
        EquipList(EquippedItems);
        UnbankList(RequiredItems);
        GetDropList(RequiredItems);

        while (!bot.ShouldExit()){
            //Enter Your Code Here
            //Check the system functions for a list of functions you can use.
            while(!bot.Inventory.Contains("Ancient Hourglass", ancientHourglassQuantity) && toggleGrindAncientHourGlass == true){
                acceptQuest(8326);
                TempItemFarm(soloClass, "Sa-Lataan Defeated", 1, "queenreign", mapNumber, "r22", "Left", 8326, "Sa-Laatan");
                TempItemFarm(soloClass, "Grou'luu Defeated", 1, "queenreign", mapNumber, "r17", "Left", 8326, "Grou'luu");
                TempItemFarm(soloClass, "Extriki Defeated", 1, "queenreign", mapNumber, "r11", "Left", 8326, "Extriki");
                TempItemFarm(soloClass, "Jaaku Defeated", 1, "queenreign", mapNumber, "r4", "Left", 8326, "Jaaku");
                SafeQuestComplete(8326, -1);
            }

            while(!bot.Inventory.Contains("Ancient Astrolabe", ancientAstrolabeQuantity) && toggleGrindAncientAstrolabe == true){
                acceptQuest(8349);
                TempItemFarm(soloClass, "Chamat Defeated", 1, "orbhunt", mapNumber, "r5", "Left", 8349, "Chamat");
                TempItemFarm(soloClass, "Horothotep Defeated", 1, "orbhunt", mapNumber, "r10", "Left", 8349, "Horothotep");
                TempItemFarm(soloClass, "Kolyaban Defeated", 1, "orbhunt", mapNumber, "r15", "Left", 8349, "Kolyaban");
                TempItemFarm(soloClass, "Quetzal Defeated", 1, "orbhunt", mapNumber, "r20", "Right", 8349, "Quetzal");
                SafeQuestComplete(8349, -1);
            }

            if(getLightningLord == true){
            InvItemFarm(soloClass ,"LightningLord", 1, "queenreign", mapNumber, "r11", "Left", 1, "Extriki");
            }
            if(otherExtrikiDrops == true){
                InvItemFarm(soloClass ,"LightningLord Helm", 1, "queenreign", mapNumber, "r11", "Left", 1, "Extriki");
                InvItemFarm(soloClass ,"LightningLord Locks", 1, "queenreign", mapNumber, "r11", "Left", 1, "Extriki");
                InvItemFarm(soloClass ,"LightningLord Rune", 1, "queenreign", mapNumber, "r11", "Left", 1, "Extriki");
            }
            StopBot();
        }
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

    public void InvItemFarm(string class_name ,string itemName, int itemQuantity, string mapName, string mapNumber, string cellName, string padName, int questID, string monsterName)
    {
        //Farms you the specified quantity of the specified item with the specified quest accepted from specified monsters in the specified location. Saves States every ~5 minutes.
        
        //Must have the following functions in your script:
        //SafeMapJoin
        //SaveState
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
            SaveState();
            bot.Log($"[{DateTime.Now:HH:mm:ss}] Saved State {SavedState} time(s).");
            FarmLoop = 0;
            goto startFarmLoop;

        maintainFarmLoop:
            while (!bot.Inventory.Contains(itemName, itemQuantity))
            {
                SafeEquip(class_name, 1);
                if (bot.Map.Name != mapName) SafeMapJoin(mapName, mapNumber, cellName, padName);
                if (bot.Player.Cell != cellName) bot.Player.Jump(cellName, padName);
                bot.Quests.EnsureAccept(questID);
                bot.Options.AggroMonsters = true;
                bot.Player.Attack(monsterName);
                FarmLoop += 1;
                if (FarmLoop > 8700) goto breakFarmLoop;
            }
    }

    public void acceptQuest(int questID){
        if(!bot.Quests.IsInProgress(questID)){
            toRestCell();
            delay(1700);
            bot.Quests.EnsureAccept(questID);
            delay(1400);
        }
        
    }

    public void TempItemFarm(string class_name, string tempName, int tempQuantity, string mapName, string mapNumber, string cellName, string padName, int questID, string monsterName){
        //Farms you the required quantity of the specified temp item with the specified quest accepted from specified monsters in the specified location.

        //Must have the following functions in your script:
        //SafeMapJoin
        //SaveState
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
            SaveState();
            bot.Log($"[{DateTime.Now:HH:mm:ss}] Saved State {SavedState} time(s).");
            FarmLoop = 0;
            goto startFarmLoop;

        maintainFarmLoop:
            while (!bot.Inventory.ContainsTempItem(tempName, tempQuantity)){
                SafeEquip(class_name, 1);
                if (bot.Map.Name != mapName) SafeMapJoin(mapName, mapNumber, cellName, padName);
                if (bot.Player.Cell != cellName) bot.Player.Jump(cellName, padName);
                bot.Quests.EnsureAccept(questID);
                bot.Options.AggroMonsters = true;
                bot.Player.Attack(monsterName);
                FarmLoop += 1;
                if (FarmLoop > 8700) goto breakFarmLoop;
            }
    }

    public void SafeEquip(string itemName, int item_quantity = 1)
    {
        //Equips an item.

        //Must have the following functions in your script:
        //ExitCombat

        while (!bot.Inventory.IsEquipped(itemName) && bot.Inventory.Contains(itemName, item_quantity)){
            toRestCell();
            bot.Player.EquipItem(itemName);
            bot.Options.InfiniteRange = true;
            delay(1200);
        }
    }

    public void SafePurchase(string itemName, int itemQuantityNeeded, string mapName, string mapNumber, int shopID)
    {
        //Purchases the specified quantity of the specified item from the specified shop in the specified map. 

        //Must have the following functions in your script:
        //SafeMapJoin
        //ExitCombat

        while (!bot.Inventory.Contains(itemName, itemQuantityNeeded))
        {
            if (bot.Map.Name != mapName) SafeMapJoin(mapName, mapNumber, "Wait", "Spawn");
            toRestCell();
            if (bot.Shops.IsShopLoaded != true)
            {
                bot.Shops.Load(shopID);
                bot.Log($"[{DateTime.Now:HH:mm:ss}] Loaded Shop {shopID}.");
            }
            bot.Shops.BuyItem(itemName);
            bot.Log($"[{DateTime.Now:HH:mm:ss}] Purchased {itemName} from Shop {shopID}.");
        }
    }

    public void SafeSell(string itemName, int itemQuantityNeeded)
    {
        //Sells the specified item until you have the specified quantity.
        
        //Must have the following functions in your script:
        //ExitCombat

        int sellingPoint = itemQuantityNeeded + 1;
        while (bot.Inventory.Contains(itemName, sellingPoint))
        {
            toRestCell();
            bot.Shops.SellItem(itemName);
        }
    }

    public void SafeQuestComplete(int questID, int itemID = -1)
    {
        //Attempts to complete the quest thrice. If it fails to complete, logs out. If it successfully completes, re-accepts the quest and checks if it can be completed again.

        //Must have the following functions in your script:
        //ExitCombat

        //Must have the following command under public class Script:
        //int QuestCounter = 0;

        maintainCompleteLoop:
            toRestCell();
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

    public void SaveState()
    {
        //Creates a quick Save State by joining a private /yulgar.

        //Must have the following functions in your script:
        //SafeMapJoin
        //ExitCombat

        toRestCell();
        SafeMapJoin("yulgar", "2142069", "Enter", "Spawn");
    }

    public void toRestCell(){
        bot.Options.AggroMonsters = false;
        while(bot.Player.Cell != "Wait"){
            bot.Player.Jump("Wait", "Spawn");
            delay(1200);
        }
        while (bot.Player.State == 2) { }
    }

    public void delay(int time_ms){
        bot.Sleep(time_ms);
    }

    public void SafeMapJoin(string mapName, string mapNumber, string cellName, string padName)
    {
        //Joins the specified map.

        //Must have the following functions in your script:
        //ExitCombat

        maintainJoinLoop:
        if (bot.Map.Name != mapName){
            toRestCell();
            bot.Player.Join($"{mapName}-{mapNumber}", cellName, padName);
            bot.Wait.ForMapLoad(mapName);
            if (bot.Map.Name != mapName) goto maintainJoinLoop;
        }
            if (bot.Player.Cell != cellName) bot.Player.Jump(cellName, padName);
            bot.Log($"[{DateTime.Now:HH:mm:ss}] Joined map {mapName}-{mapNumber}, positioned at the {padName} side of cell {cellName}.");

    }

    /*------------------------------------------------------------------------------------------------------------
                                                    Background Functions
    ------------------------------------------------------------------------------------------------------------*/

    //These functions help you to either configure certain settings or run event handlers in the background.
    //It is highly recommended to have all these functions present in your script as they are very useful.
    //Some Invocable Functions may call or require the assistance of some Background Functions as well.

    public void ConfigureOptions(string playerName = "Bot By AuQW", string guildName = "https://auqw.tk/")
    {
        //Recommended Default Bot Configurations.
        
        bot.Options.CustomName = playerName;
        bot.Options.CustomGuild = guildName;
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

    public void SkillList(params int[] Skillset)
    {
        //Spams Skills when in combat. You can get in combat by going to a cell with monsters in it with bot.Options.AggroMonsters enabled or using an attack command against one. 

        bot.RegisterHandler(1, b => {
            if (bot.Player.InCombat == true)
            {
                foreach (var skill in Skillset){
                    bot.Player.UseSkill(skill);
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
                if (bot.Player.DropExists(Item)){ 
                    bot.Player.Pickup(Item);
                }
            }
            bot.Player.RejectExcept(GetDropList);
        });
    }
    public void ItemWhiteList(params string[] WhiteList)
    {
        //Pick up items in an array when they dropped. May fail to pick up items that drop immediately after the same item is picked up. GetDropList is preferable instead.
        
        foreach (var Item in WhiteList){
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

    public string MapNumberConverter(string mapNumber){
        if(mapNumber=="1e99"){
            Random rnd = new Random();
            int randomDigits = rnd.Next(10000,99999);
            string radomDigitsText = randomDigits.ToString();
            mapNumber = radomDigitsText;
            return mapNumber;
        }
        else{
            return mapNumber;
        }
    }
}