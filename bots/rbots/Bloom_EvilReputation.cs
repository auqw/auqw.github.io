using System;
using System.Linq;
using System.Collections.Generic;

using RBot;
using RBot.Options;
using RBot.Skills.UseRules;


public class Script {
   // Sustem variables
   public ScriptInterface bot => ScriptInterface.Instance;
   public bool DebugMode = true;

   // Setup Variables
   public int MapNumber;


   // Monster variables

   // Class & Skill variables
   public string ClassFarm;
   public List<int> SkillFarm = new List<int>();
   public bool EnableSkillRun = true;
   public int SkillWait = 0;

   // Option variables
   public string OptionsStorage = "Legion Revenant";
   public List<IOption> Options = new List<IOption>() {
      new Option<string>("‎space0", "『 Metadata 』", "Bot info.\n(This is just a section header).", ""),
      new Option<string>("author", "Author", "The creator of this bot is bloom.", "Bloom", true),
      new Option<string>("version", "Version", "The bot version.", "v.1.0.0", true),
      new Option<string>("findus", "Find Us", "Join us on discord if you have any question. You can find more bots in the portal link.", "discord.io/AQWBots", true),
      new Option<string>("findus2", " ", "Join us on discord if you have any question. You can find more bots in the portal link.", "https://auqw.tk/", true),
      new Option<string>("‎space1", " ", "", ""),

      new Option<string>("‎space2", "『 Setting 』", "The Setting Section.\n(This is just a section header).", ""),
      new Option<int>("map", "Map", "Map number to join into.", 9999),
      new Option<string>("‎space3", " ", "", ""),

      new Option<string>("‎space6", "『 Classes 』", "The Class Section.\n(This is just a section header).", ""),
      new Option<int>("skillWait", "Skill Wait", "Inherent wait between skills in ms. Example -> 500", 0), 
      new Option<string>("classFarm", "Farming Class", "Class for Farming.\nExample -> Legion Revenant", "Legion Revenant"),
      new Option<string>("skillFarming", "Farming Skills", "Farming Skillset. Use -> [1, 2, 3, 4, 5, 6] just like the aqw UI skill keys. Add wNumber to delay before executing a skill. Time is in ms, minimum of 10ms. Example -> 5,4,w800,1", "1,2,3,4"),

   };
 
   
   public void ScriptMain(ScriptInterface bot) {

      // Var setups
      MapNumber = bot.Config.Get<int>("map");

      // Class setups
      ClassFarm = bot.Config.Get<string>("classFarm");

      // Skill setups
      SkillConvert();
      SkillWait = bot.Config.Get<int>("skillWait");


      // Configurations
      bot.Lite.CustomDropsUI = false;
      bot.Drops.RejectElse = true;
      bot.Drops.Start();
      Configurations();


      /// <remark> MAIN CHECK </remark>
      
      while ((bot.Player.GetFactionRank("Evil") < 10) || DebugMode) {
         QuestRun("newbie", "r2", "Left", 364);
      }
 
   }

   /// <summary>Bot Options</summary>
   public void Configurations() {
      bot.Options.CustomName = "Groomed By Bloom";
      bot.Options.CustomGuild = "<AutisticQuest Worlds>";
      bot.Options.SafeTimings = true;
      bot.Options.RestPackets = true;
      bot.Options.PrivateRooms = true;
      bot.Options.InfiniteRange = true;
      bot.Options.PrivateRooms = false;
      bot.Options.SkipCutscenes = true;
      bot.Options.DisableFX = true;
      bot.Options.AggroMonsters = false;
      bot.Options.AutoRelogin = true;
      bot.Options.LagKiller = true;
      bot.Lite.DisableSkillAnimations = true;
      bot.Lite.CustomDropsUI = false;

   }

   /// <summary>Converts Skill string input into an int List</summary>
   public void SkillConvert() {
      // Setups Skills
      IDictionary<string, List<int>> classes = new Dictionary<string, List<int>>() {
         {"skillFarming", SkillFarm},
      };
      foreach(KeyValuePair<string, List<int>> kvp in classes) {
         foreach (string class_str in bot.Config.Get<string>(kvp.Key).Split(',')) {
            int a = int.Parse(class_str.Replace(" ", "").Replace("w", ""));
            if (a < 1) continue;
            if (a <= 6) {
               kvp.Value.Add(a-1);
               continue;
            } else {
               kvp.Value.Add(a);
            }
         }
      }

   }

   /// <summary>Chooses which skill to use</summary>
   /// <param name="skill">The skill name</param>
   public void SkillUse(string skill, string MonsterTarget="*") {
      if (EnableSkillRun == false) return;
      switch(skill) {
         case "farm":
            ClassEquip(ClassFarm);
            SkillActivate(SkillFarm);
            break;
      }
   }


   /// <summary>Starts attacking the target</summary>
   /// <param name="SkillSet">The skillset int list</param>
   public void SkillActivate(List<int> SkillSet, string MonsterTarget="*") {
      EnableSkillRun = false;

      if (bot.Monsters.Exists(MonsterTarget)) {
         bot.Player.Attack(MonsterTarget);
      } else {
         bot.Player.Attack("*");
      }
      foreach (int skill in SkillSet) {

         if (skill <= 6) {            
            if (bot.Player.CanUseSkill(skill)) bot.Player.UseSkill(skill);   
            if (SkillWait  != 0) bot.Sleep(SkillWait);
            continue;
         } else {
            bot.Sleep(skill);
            continue;
         }
      }
      EnableSkillRun = true;
   }


   public void ClassEquip(string ClassName) {
      if (bot.Inventory.IsEquipped(ClassName)) return;
      if (!bot.Inventory.Contains(ClassName)) return;
      SafeEquip(ClassName);

   }

   /// <summary>Equips an item.</summary>
   public void SafeEquip(string ItemName) {
      while (bot.Inventory.Contains(ItemName) && !bot.Inventory.IsEquipped(ItemName)) {
         ExitCombat();
         bot.Player.EquipItem(ItemName);
      }
   }

   /// <summary>Exits Combat by jumping cells.</summary>
   public void ExitCombat() {
      bot.Options.AggroMonsters = false;
      bot.Player.Jump("Wait", "Spawn");
      while (bot.Player.State == 2) { }
   }


   public void QuestRun(string Map, string Cell, string Pad, int QuestID, string Monster="*") {

      SafeMapJoin(Map, Cell, Pad);
      bot.Quests.EnsureAccept(QuestID);
      
      while (!bot.Quests.CanComplete(QuestID)) SkillUse("farm");

      ExitCombat();

      bot.Quests.Complete(QuestID);
   }


   public void SafeMapJoin(string MapName, string CellName = "Enter", string PadName = "Spawn",bool UseBoss=false)
   {
      //Must have the following functions in your script:
      //ExitCombat
      string mapname = MapName.ToLower();
      int number = MapNumber;

      while (bot.Map.Name != mapname)
      {
         ExitCombat();
         if (mapname == "tercessuinotlim") bot.Player.Jump("m22", "Center");
         bot.Player.Join($"{mapname}-{number}", CellName, PadName);
         bot.Wait.ForMapLoad(mapname);
         bot.Sleep(500);
      }
      if (bot.Player.Cell != CellName) bot.Player.Jump(CellName, PadName);
      bot.Log($"[{DateTime.Now:HH:mm:ss}] Joined map {mapname}-{number}, positioned at the {PadName} side of cell {CellName}.");
   }



}