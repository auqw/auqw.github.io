using System;
using RBot;
using System.Collections.Generic;

public class BluuPurpleTemplate
{
	//-----------EDIT BELOW-------------//
	public int MapNumber = 2142069;
	public readonly int[] SkillOrder = { 3, 1, 2, 4 };
    public readonly int[] SkillOrderSoloClass = { 4, 1, 2 };
    public readonly int[] SkillOrderFarmClass = { 3, 1, 2, 4 };
    public int[] QuestList = { 3064, 3066, 3067 };
	public int SaveStateLoops = 8700;
	public int TurnInAttempts = 10;
	public string[] RequiredItems = { FarmClass, SoloClass };
    public static string FarmClass = "Vampire Lord";
    public static string SoloClass = "Void Highlord";
   	public string[] MaxSetEquip = { "Necrotic Sword of Doom", "Dual Necrotic Swords of Doom", "Polly Roger", "Head of the Legion Beast", "Fire Champion's Armor", "Awescended Omni Wings"  };
       
       //-----------EDIT ABOVE-------------//


	public int FarmLoop;
	public int SavedState;
	public ScriptInterface bot => ScriptInterface.Instance;
	public void ScriptMain(ScriptInterface bot)
	{
		if (bot.Player.Cell != "Wait") bot.Player.Jump("Wait", "Spawn");

		ConfigureBotOptions();
		ConfigureLiteSettings();

		DeathHandler();
		EquipList(MaxSetEquip);
		UnbankList(RequiredItems);
		CheckSpace(RequiredItems);
		GetDropList(RequiredItems);

		while (!bot.ShouldExit())
		{
			while (!bot.Player.Loaded) { }

            while (bot.Player.GetFactionRank("Swordhaven") < 10)
            if (bot.Player.IsMember)
            {
						{
                        SafeEquip(FarmClass);
						bot.Log(" Membership Due Swordhaven Rep");
						ItemFarm(
						"Doppelganger Documents", 1,
						Temporary: true,
						HuntFor: false,
						QuestID: 1310,
						MonsterName: "*",
						MapName: "collectorlab"
						);
						SafeQuestComplete(1310);
                        }
			if (bot.Inventory.Contains("Super-Fan Swag Token A", 1)) {SafeQuestComplete(4343);}
			if (bot.Inventory.Contains("Super-Fan Swag Token C", 200))
							{
								SafePurchase("Super-Fan Swag Token B", 20, "Collection", 325);
								bot.Sleep(700);
								SafePurchase("Super-Fan Swag Token A", 1, "Collection", 325);
							}
            }
            else if (!bot.Player.IsMember)
            {         
            
            while (bot.Player.GetFactionRank("Swordhaven") < 10)
            {
		    SkillList(FarmClass, SkillOrderFarmClass);
            SafeEquip(FarmClass);
			ItemFarm("Dungeon Fiend Hair Bow", 5, true, true, 3065, "Dungeon Fiend", "castle");
			ItemFarm("Dungeon Fiend Bow Tie", 5, true, true, 3065, "Dungeon Fiend", "castle"); 
			ItemFarm("Eradicated Arachnid", 10, true, true, 3066, "Castle Spider", "castle"); 
			ItemFarm("Castle Wraith Defeated", 10, true, true, 3067, "Castle Wraith", "castle"); 
            SafeQuestComplete(3065);
            SafeQuestComplete(3066);
            SafeQuestComplete(3067);
            }

            }
        }
		StopBot($"All Done Boss");
	}

	/*------------------------------------------------------------------------------------------------------------
													 Invokable Functions
	------------------------------------------------------------------------------------------------------------*/

	/*
		*	These functions are used to perform a major action in AQW.
		*	All of them require at least one of the Auxiliary Functions listed below to be present in your script.
		*	Some of the functions require you to pre-declare certain integers under "public class Script"
		*	ItemFarm, MultiQuestFarm and HuntItemFarm will require some Background Functions to be present as well.
		*	All of this information can be found inside the functions. Make sure to read.
		*	ItemFarm("ItemName", ItemQuantity, Temporary, HuntFor, QuestID, "MonsterName", "MapName", "CellName", "PadName");
		*	MultiQuestFarm("MapName", "CellName", "PadName", QuestList[], "MonsterName");
		*	SafeEquip("ItemName");
		*	SafePurchase("ItemName", ItemQuantityNeeded, "MapName", "MapNumber", ShopID)
		*	SafeSell("ItemName", ItemQuantityNeeded)
		*	SafeQuestComplete(QuestID, ItemID)
		*	StopBot("Text", "MapName", "MapNumber", "CellName", "PadName", "Caption")
	*/

	/// <summary>
	/// Farms you the specified quantity of the specified item with the specified quest accepted from specified monsters in the specified location. Saves States every ~5 minutes.
	/// </summary>
	public void ItemFarm(string ItemName, int ItemQuantity, bool Temporary = false, bool HuntFor = false, int QuestID = 0, string MonsterName = "*", string MapName = "Map", string CellName = "Enter", string PadName = "Spawn")
	{
	/*
		*   Must have the following functions in your script:
		*   SafeMapJoin
		*   SmartSaveState
		*   SkillList
		*   ExitCombat
		*   GetDropList OR ItemWhitelist
		*
		*   Must have the following commands under public class Script:
		*   int FarmLoop = 0;
		*   int SavedState = 0;
	*/

	startFarmLoop:
		if (FarmLoop > 0) goto maintainFarmLoop;
		SavedState++;
		FormatLog("Farm", $"Started Farming Loop {SavedState}");
		goto maintainFarmLoop;

	breakFarmLoop:
		SmartSaveState();
		FormatLog("Farm", $"Completed Farming Loop {SavedState}");
		FarmLoop = 0;
		goto startFarmLoop;

	maintainFarmLoop:
		if (Temporary)
		{
			if (HuntFor)
			{
				while (!bot.Inventory.ContainsTempItem(ItemName, ItemQuantity))
				{
					FarmLoop++;
					if (bot.Map.Name != MapName.ToLower()) SafeMapJoin(MapName.ToLower());
					if (QuestID > 0) bot.Quests.EnsureAccept(QuestID);
					bot.Options.AggroMonsters = true;
					AttackType("h", MonsterName);
					if (FarmLoop > SaveStateLoops) goto breakFarmLoop;
				}
			}
			else
			{
				while (!bot.Inventory.ContainsTempItem(ItemName, ItemQuantity))
				{
					FarmLoop++;
					if (bot.Map.Name != MapName.ToLower()) SafeMapJoin(MapName.ToLower(), CellName, PadName);
					if (bot.Player.Cell != CellName) bot.Player.Jump(CellName, PadName);
					if (QuestID > 0) bot.Quests.EnsureAccept(QuestID);
					bot.Options.AggroMonsters = true;
					AttackType("a", MonsterName);
					if (FarmLoop > SaveStateLoops) goto breakFarmLoop;
				}
			}
		}
		else
		{
			if (HuntFor)
			{
				while (!bot.Inventory.Contains(ItemName, ItemQuantity))
				{
					FarmLoop++;
					if (bot.Map.Name != MapName.ToLower()) SafeMapJoin(MapName.ToLower());
					if (QuestID > 0) bot.Quests.EnsureAccept(QuestID);
					bot.Options.AggroMonsters = true;
					AttackType("h", MonsterName);
					if (FarmLoop > SaveStateLoops) goto breakFarmLoop;
				}
			}
			else
			{
				while (!bot.Inventory.Contains(ItemName, ItemQuantity))
				{
					FarmLoop++;
					if (bot.Map.Name != MapName.ToLower()) SafeMapJoin(MapName.ToLower(), CellName, PadName);
					if (bot.Player.Cell != CellName) bot.Player.Jump(CellName, PadName);
					if (QuestID > 0) bot.Quests.EnsureAccept(QuestID);
					bot.Options.AggroMonsters = true;
					AttackType("a", MonsterName);
					if (FarmLoop > SaveStateLoops) goto breakFarmLoop;
				}
			}
		}
	}

	/// <summary>
	/// Farms all the quests in a given string, must all be farmable in the same room and cell.
	/// </summary>
	public void MultiQuestFarm(string MapName, string CellName, string PadName, int[] QuestList, string MonsterName = "*")
	{
	/*
		*   Must have the following functions in your script:
		*   SafeMapJoin
		*   SmartSaveState
		*   SkillList
		*   ExitCombat
		*   GetDropList OR ItemWhitelist
		*
		*   Must have the following commands under public class Script:
		*   int FarmLoop = 0;
		*   int SavedState = 0;
	*/

	startFarmLoop:
		if (FarmLoop > 0) goto maintainFarmLoop;
		SavedState++;
		FormatLog("Farm", $"Started Farming Loop {SavedState}");
		goto maintainFarmLoop;

	breakFarmLoop:
		SmartSaveState();
		FormatLog("Farm", $"Completed Farming Loop {SavedState}");
		FarmLoop = 0;
		goto startFarmLoop;

	maintainFarmLoop:
		FarmLoop++;
		if (bot.Map.Name != MapName.ToLower()) SafeMapJoin(MapName.ToLower(), CellName, PadName);
		if (bot.Player.Cell != CellName) bot.Player.Jump(CellName, PadName);
		foreach (var Quest in QuestList)
		{
			if (!bot.Quests.IsInProgress(Quest)) bot.Quests.EnsureAccept(Quest);
			if (bot.Quests.CanComplete(Quest)) SafeQuestComplete(Quest);
		}
		bot.Options.AggroMonsters = true;
		AttackType("a", MonsterName);
		if (FarmLoop > SaveStateLoops) goto breakFarmLoop;
	}

	/// <summary>
	/// Equips an item.
	/// </summary>
	public void SafeEquip(string ItemName)
	{
		//Must have the following functions in your script:
		//ExitCombat

		while (bot.Inventory.Contains(ItemName) && !bot.Inventory.IsEquipped(ItemName))
		{
			ExitCombat();
			bot.Player.EquipItem(ItemName);
		}
	}

	/// <summary>
	/// Sets attack type to Attack(Attack/A) or Hunt(Hunt/H)
	/// </summary>
	/// <param name="AttackType">Attack/A or Hunt/H</param>
	/// <param name="MonsterName">Name of the monster</param>
	public void AttackType(string AttackType, string MonsterName)
	{
		string attack_ = AttackType.ToLower();

		if (attack_ == "a" || attack_ == "attack")
		{
			bot.Player.Attack(MonsterName);
		}
		else if (attack_ == "h" || attack_ == "hunt")
		{
			bot.Player.Hunt(MonsterName);
		}
	}

	/// <summary>
	/// Purchases the specified quantity of the specified item from the specified shop in the specified map.
	/// </summary>
	public void SafePurchase(string ItemName, int ItemQuantityNeeded, string MapName, int ShopID)
	{
		//Must have the following functions in your script:
		//SafeMapJoin
		//ExitCombat

		while (!bot.Inventory.Contains(ItemName, ItemQuantityNeeded))
		{
			if (bot.Map.Name != MapName.ToLower()) SafeMapJoin(MapName.ToLower(), "Wait", "Spawn");
			ExitCombat();
			bot.Log($"[{DateTime.Now:HH:mm:ss}] Purchasing \t [{ItemName}]");
			bot.Shops.Load(ShopID);
			bot.Log($"[{DateTime.Now:HH:mm:ss}] Shop \t \t Loaded Shop {ShopID}.");
			bot.Shops.BuyItem(ItemName);
			bot.Log($"[{DateTime.Now:HH:mm:ss}] Shop \t \t Purchased {ItemName} from Shop {ShopID}.");
		}
	}

	/// <summary>
	/// Sells the specified item until you have the specified quantity.
	/// </summary>
	public void SafeSell(string ItemName, int ItemQuantityNeeded)
	{
		//Must have the following functions in your script:
		//ExitCombat

		int sellingPoint = ItemQuantityNeeded + 1;
		while (bot.Inventory.Contains(ItemName, sellingPoint))
		{
			ExitCombat();
			bot.Shops.SellItem(ItemName);
		}
	}

	/// <summary>
	/// Attempts to complete the quest with the set amount of {TurnInAttempts}. If it fails to complete, logs out. If it successfully completes, re-accepts the quest and checks if it can be completed again.
	/// </summary>
	public void SafeQuestComplete(int QuestID, int ItemID = -1)
	{
		//Must have the following functions in your script:
		//ExitCombat

		ExitCombat();
		bot.Quests.EnsureAccept(QuestID);
		bot.Quests.EnsureComplete(QuestID, ItemID, tries: TurnInAttempts);
		if (bot.Quests.IsInProgress(QuestID))
		{
			FormatLog("Quest", $"Turning in Quest {QuestID} failed. Logging out");
			bot.Player.Logout();
		}
		FormatLog("Quest", $"Turning in Quest {QuestID} successful.");
		while (!bot.Quests.IsInProgress(QuestID)) bot.Quests.EnsureAccept(QuestID);
	}

	/// <summary>
	/// Stops the bot at yulgar if no parameters are set, or your specified map if the parameters are set.
	/// </summary>
	public void StopBot(string Text = "Bot stopped successfully.", string MapName = "yulgar", string CellName = "Enter", string PadName = "Spawn", string Caption = "Stopped", string MessageType = "event")
	{
		//Must have the following functions in your script:
		//SafeMapJoin
		//ExitCombat
		if (bot.Map.Name != MapName.ToLower()) SafeMapJoin(MapName.ToLower(), CellName, PadName);
		if (bot.Player.Cell != CellName) bot.Player.Jump(CellName, PadName);
		bot.Drops.RejectElse = false;
		bot.Options.LagKiller = false;
		bot.Options.AggroMonsters = false;
		FormatLog(Title: true, Text: "Script Stopped");
		Console.WriteLine(Text);
		SendMSGPacket(Text, Caption, MessageType);
		ScriptManager.StopScript();
	}

	/*------------------------------------------------------------------------------------------------------------
													Auxiliary Functions
	------------------------------------------------------------------------------------------------------------*/

	/*
		*   These functions are used to perform small actions in AQW.
		*   They are usually called upon by the Invokable Functions, but can be used separately as well.
		*   Make sure to have them loaded if your Invokable Function states that they are required.
		*   ExitCombat()
		*   SmartSaveState()
		*   SafeMapJoin("MapName", "CellName", "PadName")
		*	FormatLog("Topic", "Text", Tabs, Title, Followup)
	*/

	/// <summary>
	/// Exits Combat by jumping cells.
	/// </summary>
	public void ExitCombat()
	{
		bot.Options.AggroMonsters = false;
		bot.Player.Jump("Wait", "Spawn");
		while (bot.Player.State == 2) { }
	}

	/// <summary>
	/// Creates a quick Save State by messaging yourself.
	/// </summary>
	public void SmartSaveState()
	{
		bot.SendPacket("%xt%zm%whisper%1%creating save state%" + bot.Player.Username + "%");
		FormatLog("Saving", "Successfully Saved State");
	}

	/// <summary>
	/// Joins the specified map.
	/// </summary>
	public void SafeMapJoin(string MapName, string CellName = "Enter", string PadName = "Spawn")
	{
		//Must have the following functions in your script:
		//ExitCombat
		string mapname = MapName.ToLower();
		while (bot.Map.Name != mapname)
		{
			ExitCombat();
			if (mapname == "tercessuinotlim") bot.Player.Jump("m22", "Center");
			bot.Player.Join($"{mapname}-{MapNumber}", CellName, PadName);
			bot.Wait.ForMapLoad(mapname);
			bot.Sleep(500);
		}
		if (bot.Player.Cell != CellName) bot.Player.Jump(CellName, PadName);
		FormatLog("Joined", $"[{mapname}-{MapNumber}, {CellName}, {PadName}]");
	}
	
	/// <summary>
	/// Logs following a specific format. No more than 3 tabs allowed.
	/// </summary>
	public void FormatLog(string Topic = "FormatLog", string Text = "Missing Input", int Tabs = 2, bool Title = false, bool Followup = false)
	{
		if (Title)
			bot.Log($"[{DateTime.Now:HH:mm:ss}] -----{Text}-----");
		else 
		{
			Tabs = Tabs > 3 ? 3 : Tabs;
			string TabPlace = "";
			for (int i = 0; i < Tabs; i++) 
				TabPlace += "\t";
			if (Followup) 
				bot.Log($"[{DateTime.Now:HH:mm:ss}] ↑ {TabPlace}{Text}");
			else 
				bot.Log($"[{DateTime.Now:HH:mm:ss}] {Topic} {TabPlace}{Text}");
		}
	}

	/*------------------------------------------------------------------------------------------------------------
													Background Functions
	------------------------------------------------------------------------------------------------------------*/

	/*
		*   These functions help you to either configure certain settings or run event handlers in the background.
		*   It is highly recommended to have all these functions present in your script as they are very useful.
		*   Some Invokable Functions may call or require the assistance of some Background Functions as well.
		*   These functions are to be run at the very beginning of the bot under public class Script.
		*   ConfigureBotOptions("PlayerName", "GuildName", LagKiller, SafeTimings, RestPackets, AutoRelogin, PrivateRooms, InfiniteRange, SkipCutscenes, ExitCombatBeforeQuest)
		*   ConfigureLiteSettings(UntargetSelf, UntargetDead, CustomDrops, ReacceptQuest, SmoothBackground, Debugger)
		*   SkillList(int[])
		*   GetDropList(string[])
		*   ItemWhiteList(string[])
		*   EquipList(string[])
		*   UnbankList(string[])
		*   CheckSpace(string[])
		*   SendMSGPacket("Message", "Name", "MessageType")
	*/

	/// <summary>
	/// Change the player's name and guild for your bots specifications.
	/// Recommended Default Bot Configurations.
	/// </summary>
	public void ConfigureBotOptions(string PlayerName = "Tato", string GuildName = "Booba", bool LagKiller = true, bool SafeTimings = true, bool RestPackets = true, bool AutoRelogin = true, bool PrivateRooms = false, bool InfiniteRange = true, bool SkipCutscenes = true, bool ExitCombatBeforeQuest = true, bool HideMonster=true)
	{
		SendMSGPacket("Configuring bot.", "AuQW", "moderator");
		bot.Options.CustomName = PlayerName;
		bot.Options.CustomGuild = GuildName;
		bot.Options.LagKiller = LagKiller;
		bot.Options.SafeTimings = SafeTimings;
		bot.Options.RestPackets = RestPackets;
		bot.Options.AutoRelogin = AutoRelogin;
		bot.Options.PrivateRooms = PrivateRooms;
		bot.Options.InfiniteRange = InfiniteRange;
		bot.Options.SkipCutscenes = SkipCutscenes;
		bot.Options.ExitCombatBeforeQuest = ExitCombatBeforeQuest;
		// bot.Events.PlayerDeath += PD => ScriptManager.RestartScript();
		// bot.Events.PlayerAFK += PA => ScriptManager.RestartScript();
		HideMonsters(HideMonster);
	}

	/// <summary>
	/// Hides the monsters for performance
	/// </summary>
	/// <param name="Value"> true -> hides monsters. false -> reveals them </param>
	public void HideMonsters(bool Value) {
	  switch(Value) {
	     case true:
	        if (!bot.GetGameObject<bool>("ui.monsterIcon.redX.visible")) {
	           bot.CallGameFunction("world.toggleMonsters");
	        }
	        return;
	     case false:
	        if (bot.GetGameObject<bool>("ui.monsterIcon.redX.visible")) {
	           bot.CallGameFunction("world.toggleMonsters");
	        }
	        return;
	  }
	}

	/// <summary>
	/// Gets AQLite Functions
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="optionName"></param>
	/// <returns></returns>
	public T GetLite<T>(string optionName)
	{
		return bot.GetGameObject<T>($"litePreference.data.{optionName}");
	}

	/// <summary>
	/// Sets AQLite Functions
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="optionName"></param>
	/// <param name="value"></param>
	public void SetLite<T>(string optionName, T value)
	{
		bot.SetGameObject($"litePreference.data.{optionName}", value);
	}

	/// <summary>
	/// Allows you to turn on and off AQLite functions.
	/// Recommended Default Bot Configurations.
	/// </summary>
	public void ConfigureLiteSettings(bool UntargetSelf = true, bool UntargetDead = true, bool CustomDrops = true, bool ReacceptQuest = false, bool SmoothBackground = true, bool Debugger = false)
	{
		SetLite("bUntargetSelf", UntargetSelf);
		SetLite("bUntargetDead", UntargetDead);
		SetLite("bCustomDrops", CustomDrops);
		SetLite("bReaccept", ReacceptQuest);
		SetLite("bSmoothBG", SmoothBackground);
		SetLite("bDebugger", Debugger);
	}

	/// <summary>
	/// Spams Skills when in combat. You can get in combat by going to a cell with monsters in it with bot.Options.AggroMonsters enabled or using an attack command against one.
	/// </summary>
	
	string currentSkillSet = "" ;
	public void SkillList(string skillSetName, params int[] Skillset)
	{
		bot.Handlers.RemoveAll(handlers => handlers.Name == currentSkillSet);
		bot.RegisterHandler(1, b => {
			if (bot.Player.InCombat)
			{
				foreach (var Skill in Skillset)
				{
					if (bot.Player.CanUseSkill(Skill))
    				bot.Player.UseSkill(Skill);
				}
			}
		}, skillSetName);
		currentSkillSet = skillSetName;
	}

	/// <summary>
	/// Checks if items in an array have dropped every second and picks them up if so. GetDropList is recommended.
	/// </summary>
	public void GetDropList(params string[] GetDropList)
	{
		bot.RegisterHandler(4, b => {
			foreach (string Item in GetDropList)
			{
				if (bot.Player.DropExists(Item)) bot.Player.Pickup(Item);
			}
			bot.Player.RejectExcept(GetDropList);
		});
	}

	/// <summary>
	/// Pick up items in an array when they dropped. May fail to pick up items that drop immediately after the same item is picked up. GetDropList is preferable instead.
	/// </summary>
	public void ItemWhiteList(params string[] WhiteList)
	{
		foreach (var Item in WhiteList)
		{
			bot.Drops.Add(Item);
		}
		bot.Drops.RejectElse = true;
		bot.Drops.Start();
	}

	/// <summary>
	/// Equips all items in an array.
	/// </summary>
	/// <param name="EquipList"></param>
	public void EquipList(params string[] EquipList)
	{
		foreach (var Item in EquipList)
		{
			if (bot.Inventory.Contains(Item))
			{
				SafeEquip(Item);
			}
		}
	}

	/// <summary>
	/// Unbanks all items in an array after banking every other AC-tagged Misc item in the inventory.
	/// </summary>
	/// <param name="UnbankList"></param>
	public void UnbankList(params string[] UnbankList)
	{
		if (bot.Player.Cell != "Wait") bot.Player.Jump("Wait", "Spawn");
		while (bot.Player.State == 2) { }
		bot.Player.LoadBank();
		List<string> Whitelisted = new List<string>() { "Note", "Item", "Resource", "QuestItem", "ServerUse" };
		foreach (var item in bot.Inventory.Items)
		{
			if (!Whitelisted.Contains(item.Category.ToString())) continue;
			if (item.Name != "Treasure Potion" && item.Coins && !Array.Exists(UnbankList, x => x == item.Name)) bot.Inventory.ToBank(item.Name);
		}
		foreach (var item in UnbankList)
		{
			if (bot.Bank.Contains(item)) bot.Bank.ToInventory(item);
		}
	}

	/// <summary>
	/// Checks the amount of space you need from an array's length/set amount.
	/// </summary>
	/// <param name="ItemList"></param>
	public void CheckSpace(params string[] ItemList)
	{
		int MaxSpace = bot.GetGameObject<int>("world.myAvatar.objData.iBagSlots");
		int FilledSpace = bot.GetGameObject<int>("world.myAvatar.items.length");
		int EmptySpace = MaxSpace - FilledSpace;
		int SpaceNeeded = 0;

		foreach (var Item in ItemList)
		{
			if (!bot.Inventory.Contains(Item)) SpaceNeeded++;
		}

		if (EmptySpace < SpaceNeeded)
		{
			StopBot($"Need {SpaceNeeded} empty inventory slots, please make room for the quest.", bot.Map.Name, bot.Player.Cell, bot.Player.Pad, "Error", "moderator");
		}
	}

	/// <summary>
	/// Sends a message packet to client in chat.
	/// </summary>
	/// <param name="Message"></param>
	/// <param name="Name"></param>
	/// <param name="MessageType">moderator, warning, server, event, guild, zone, whisper</param>
	public void SendMSGPacket(string Message = " ", string Name = "SERVER", string MessageType = "zone")
	{
		// bot.SendClientPacket($"%xt%{MessageType}%-1%{Name}: {Message}%");
		bot.SendClientPacket($"%xt%chatm%0%{MessageType}~{Message}%{Name}%");
	}


	public void DeathHandler() {
      bot.RegisterHandler(2, b => {
         if (bot.Player.State==0) {
            bot.Player.SetSpawnPoint();
            ExitCombat();
            bot.Sleep(12000);
         }
      });
	}



}