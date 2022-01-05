using System;
using RBot;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

public class EternalDrakath
{
	//-----------EDIT BELOW-------------//
	public static int PrivateRoomNumber = 999999;
	public int SaveStateLoops = 8700;
	public int TurnInAttempts = 10;
	public string[] SoloingGear = {"LightCaster"};
	public string[] FarmingGear = {"Vampire Lord"};
	public readonly int[] SoloingSkillOrder = { 3, 1, 2, 4 };
	public readonly int[] FarmingSkillOrder = { 4, 2, 3, 1 };

	//-----------EDIT ABOVE-------------//

	public string[] RequieredItems = {
		"Drakath Armor",
		"Drakath's Sword",
		"Spirit Orb",
		 "Blinding Broadsword of Destiny",
		 "Blinding Blade of Destiny",
		  "Blinding Light Fragments",
		 "Bone Dust",
		 "Undead Essence",
		"Crystallized Chaos",
		"Star Fragment",
		 "Supernova Badge",
		 "Living Star Defeated",
		"Death's Oversight",
		"Reality Shard",
		 "Drakath the Eternal",
		 "Drakath the Eternal's Visor",
		 "Eternal Chaos Tassels",
		 "Everlasting Blade of Chaos",
		 "Dual Everlasting Blades of Chaos"
	};

	public int MapNumber = PrivateRoomNumber;
	public int FarmLoop;
	public int SavedState;
	public ScriptInterface bot => ScriptInterface.Instance;
	public void ScriptMain(ScriptInterface bot)
	{
		
		FormatLog(Text: "Script Started", Title: true);
		if (bot.Player.Cell != "Wait") bot.Player.Jump("Wait", "Spawn");

		ConfigureBotOptions();
		ConfigureLiteSettings(CustomDrops: true);

		DeathHandler();

		UnbankList(RequieredItems);
		CheckSpace(RequieredItems);
		GetDropList(RequieredItems);

		while (!bot.ShouldExit())
		{
			while (!bot.Player.Loaded) { }
			FormatLog(Text: "Checking Requierments", Title: true);
		// Checking if you unlocked Drakath's Armor
			if (!CheckStorage("Get Your Original Drakath's Armor") && !CheckStorage("Drakath Armor") && !CheckStorage("Original Drakath Armor")) {
				MessageBox.Show("To run this script you need to have Get Your Original Drakath's Armor. Stopping the script", "WARNING");
				StopBot("To run this script you need to have Get Your Original Drakath's Armor. Stopping the script");
			}
		// No use starting if you dont have this
			else if (!CheckStorage("Supernova Badge")) {
				MessageBox.Show("To run this script you need to have the SuperNova Badge from /Starsinc. Stopping the script", "WARNING");
				StopBot("To run this script you need to have the SuperNova Badge from /Starsinc. Stopping the script");
			}
			else {
		// If you dont own the right version, get it
				FormatLog(Text: "Obtaining correct Drakath Armor", Title: true);
				if (!CheckStorage("Drakath Armor") && CheckStorage("Get Your Original Drakath's Armor")) {
					SafePurchase("Drakath Armor", 1, "battleon", 994);
					bot.Inventory.ToBank("Get Your Original Drakath's Armor");
				}
				else if (!CheckStorage("Drakath Armor") && CheckStorage("Original Drakath Armor"))
					SafePurchase("Drakath Armor", 1, "chaosbeast", 906);
			// Finally actually starting the farm
			// Drakath's Sword
				FormatLog(Text: "Drakath's Sword", Title: true);
				SoloMode();
				ItemFarm(
					"Drakath's Sword", 1,
					HuntFor: true,
					MonsterName: "Champion of Chaos",
					MapName: "ultradrakath"
				);
				FormatLog("Complete", "Drakath's Sword", 1);
				bot.Quests.EnsureAccept(8457);
			// Spirit Orbs
				FormatLog(Text: "Spirit Orbs", Title: true);
				if (!bot.Inventory.Contains("Spirit Orb", 2000)) {
					FarmMode();
					MapNumber = 1;
					if (bot.Inventory.Contains("Blinding Broadsword of Destiny")) {
						FormatLog("Using", "Blinding Broadsword Method");
						while (!bot.Inventory.Contains("Spirit Orb", 2000)) {
							ItemFarm(
								"Blinding Light Fragments", 10,
								QuestID: 2178,
								MapName: "battleunderb"
							);
							SafeQuestComplete(2178);
						}
					}
					else if (bot.Inventory.Contains("Blinding Blade of Destiny")) {
						FormatLog("Using", "Blinding Broadsword Method");
						while (!bot.Inventory.Contains("Spirit Orb", 2000)) {
							ItemFarm(
								"Blinding Light Fragments", 10,
								QuestID: 2179,
								MapName: "battleunderb"
							);
							SafeQuestComplete(2179);
						}
					}
					else {
						FormatLog("Using", "Bone Dust + Undead Essence Method");
						while (!bot.Inventory.Contains("Spirit Orb", 2000)) {
							MultiQuestFarm(
								MapName: "battleunderb",
								CellName: "Enter",
								PadName: "Spawn",
								QuestList: new[] {2082, 2083}
							);
						}
					}
				}
				FormatLog("Complete", "Spirit Orb", 1);
			// Crystallized Chaos
				FormatLog(Text: "Crystallized Chaos", Title: true);
				FarmMode();
				ItemFarm(
					"Crystallized Chaos", 800,
					HuntFor: true,
					MonsterName: "Chaorrupted Moglin",
					MapName: "chaoslab"
				);
				FormatLog("Complete", "Crystallized Chaos", 1);
			// Star Fragment
				FormatLog(Text: "Star Fragment", Title: true);
				while (!bot.Inventory.Contains("Star Fragment", 33)) {
					FarmMode();
					ItemFarm(
						"Living Star Defeated", 30,
						HuntFor: true,
						QuestID: 4413,
						MonsterName: "Living Star",
						MapName: "starsinc"
					);
					SafeQuestComplete(4413);
					bot.Wait.ForPickup("Star Fragment");
				}
				FormatLog("Complete", "Star Fragment", 1);
			// Death's Oversight
				FormatLog(Text: "Death's Oversight", Title: true);
				SoloMode();
				ItemFarm(
					"Death's Oversight", 5,
					HuntFor: true,
					MonsterName: "Death",
					MapName: "shadowattack"
				);
				FormatLog("Complete", "Death's Oversight", 1);
			// Reality Shard
				FormatLog(Text: "Reality Shard", Title: true);
				while (!bot.Inventory.Contains("Reality Shard", 200)) {
					SoloMode();
					ItemFarm(
						"Eternal Drakath Defeated", 1,
						Temporary: true,
						HuntFor: true,
						QuestID: 8455,
						MonsterName: "Eternal Drakath",
						MapName: "eternalchaos"
					);
					SafeQuestComplete(8455);
				}
				FormatLog("Reality Shard", "Death's Oversight", 1);
				FormatLog(Text: "Finalizing", Title: true);
				SafeQuestComplete(8457);
				BankArray(RequieredItems);
				StopBot("All Items farmed and banked");
				
			}
		}
	}


	/*------------------------------------------------------------------------------------------------------------
													 Specific Functions
	------------------------------------------------------------------------------------------------------------*/
	//These functions are specific to this bot.

	public void SoloMode()
	{
		EquipList(SoloingGear);
		SkillList(SoloingSkillOrder);
		MapNumber = 1;
	}
	public void FarmMode()
	{
		EquipList(FarmingGear);
		SkillList(FarmingSkillOrder);
		MapNumber = PrivateRoomNumber;
	}

	public void BankArray(string[] Array)
	{
		ExitCombat();
		foreach (string Item in Array)
		{
			if (bot.Inventory.Contains(Item))
			{
				bot.Inventory.ToBank(Item);
				FormatLog("Banked", $"[{Item}]");
			}
		}
	}

	public bool CheckStorage(string item, int quant = 1)
	{
		if (bot.Bank.Contains(item, quant))
			return true;
		if (bot.Inventory.Contains(item, quant))
			return true;
		if (bot.Inventory.ContainsHouseItem(item))
			return true;
		return false;
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
	public void ConfigureBotOptions(string PlayerName = "Bot By AuQW", string GuildName = "https://auqw.tk/", bool LagKiller = true, bool SafeTimings = true, bool RestPackets = true, bool AutoRelogin = true, bool PrivateRooms = false, bool InfiniteRange = true, bool SkipCutscenes = true, bool ExitCombatBeforeQuest = true, bool HideMonster=true)
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
	public void ConfigureLiteSettings(bool UntargetSelf = true, bool UntargetDead = true, bool CustomDrops = false, bool ReacceptQuest = false, bool SmoothBackground = true, bool Debugger = false)
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
	public void SkillList(params int[] Skillset)
	{
		bot.RegisterHandler(1, b => {
			if (bot.Player.InCombat)
			{
				foreach (var Skill in Skillset)
				{
					bot.Player.UseSkill(Skill);
				}
			}
		});
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