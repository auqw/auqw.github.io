using RBot;
using System;
using System.Collections.Generic;
public class Script 
{
    public ScriptInterface bot => ScriptInterface.Instance;
    public string[] Drops = {"LightningLord","LightningLord Helm","LightningLord Locks","LightningLord Rune","Ancient Hourglass"};
    int[] skillOrder = {3,1,2,4};
    public int QuestCounter = 0;
    public void ScriptMain(ScriptInterface bot)
    {
        //Params : LagKiller,SafeTimings,RestPackets,AutoRelogin,InfiniteRange,PrivateRooms,ExitCombatBeforeQuest
        OptionsConfigure(true,true,true,true,true,false,true);
        Whitelist(Drops);
        SkillList(skillOrder);
        while(!bot.ShouldExit()){
        	if (!bot.Inventory.ContainsTempItem("Jaaku Defeated",5))
            {
                TempItemFarm("Jaaku Defeated",5,"queenreign","2727","r4","left",8326,"Jaaku");
            };
            if (!bot.Inventory.ContainsTempItem("Extriki Defeated",5))
            {
                TempItemFarm("Extriki Defeated",5,"queenreign","2727","r11","left",8326,"Extriki");
            };
            if (!bot.Inventory.ContainsTempItem("Grou'luu Defeated",5))
            {
                TempItemFarm("Grou'luu Defeated",5,"queenreign","2727","r17","left",8326,"Grou'luu");
            };
            if(!bot.Inventory.ContainsTempItem("Sa-Lataan Defeated",5))
            {
                TempItemFarm("Sa-Lataan Defeated",5,"queenreign","2727","r22","left",8326,"Sa-Lataan");
            };
            SafeQuestComplete(8326,-1); 
        }
    }

    public void OptionsConfigure(bool LagKiller,bool SafeTimings,bool RestPackets,bool AutoRelogin,bool InfiniteRange,bool PrivateRooms,bool ExitCombatBeforeQuest)
    {
        bot.Options.LagKiller = LagKiller;
        bot.Options.SafeTimings = SafeTimings;
        bot.Options.RestPackets = RestPackets;
        bot.Options.AutoRelogin = AutoRelogin;
        bot.Options.PrivateRooms = PrivateRooms;
        bot.Options.InfiniteRange = InfiniteRange;
        bot.Options.ExitCombatBeforeQuest = ExitCombatBeforeQuest;
        bot.Events.PlayerDeath += b => {ScriptManager.RestartScript();};
        bot.Events.PlayerAFK += b => {ScriptManager.RestartScript();};
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
    public void SafeQuestComplete(int questID, int itemID )
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

}