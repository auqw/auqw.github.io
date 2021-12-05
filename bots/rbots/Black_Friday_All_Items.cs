using System;
using RBot;
using System.Collections.Generic;
using System.Linq;

	/*
	This bot gets you the following AC items:
	- All Shadow Dragon Shinobi gear
	- All items dropped by BOGOdrone PRIME
	- All Alpha Hunter/Rogue gear
	- All items present in the EbilCorp Reward Shhop
	- The Dark Hunting Hood
	
	The bot periodically bank the obtained items, so there is no need to bank things every once in a while
	Enjoy!
	~Lord Exelot
	*/

public class ExeScript
{
	//-----------EDIT BELOW-------------//
	public int MapNumber = 2142069;
	public readonly int[] SkillOrder = { 3, 1, 2, 4 };
	public int SaveStateLoops = 8700;
	public int TurnInAttempts = 10;
	public bool TokenCleanup = false; //set this to true if you wish to have any Dragon Shinobi Tokens be removed after the bot is done with it.
	public string[] FarmingGear = { };
	public string[] SoloGear = { };
	//-----------EDIT ABOVE-------------//
	
	public string[] PrimeItems = 
	{ 
		"BOGOdrone Pet",
		"BOGOdrone Prime Pet",
		"Charged Cyber Katana",
		"Chocolate Cake Helm",
		"Cyber Blade",
		"Cyber Dagger",
		"Cyber Katana",
		"Cyber Maximillian",
		"Cyber Maximillian Cape",
		"Cyber Maximillian Helm",
		"Cyber Maximillian Sword + Whip",
		"Cyber PoleAxe",
		"Cyber Sawtooth",
		"Cyber Scythe",
		"Cyber Skewer",
		"Cyber Spear",
		"DealBot 2.0 Pet",
		"Onyx Masqerade Hat",
		"Onyx Masquerade",
		"Onyx Masquerade Fan",
		"Onyx Masquerade Locks",
		"Pink Cake Helm",
		"Rose Glasses",
		"Rose Hunting Hood",
		"Shadow Demonhunter Horns",
		"Shadow Demonhuntress Horns",
		"Shadow Vordred Helm"
	};
	
	public static string[] DSItems = 
	{
		//Token
		"Perfect Orochi Scales",
		"Dragon Shinobi Token",
		//300 Tokens
		"Shadow Dragon Shinobi",
		//100 Tokens
		"Fury of the Shadow Dragon",
		"Wrath of the Shadow Dragon",
		"Shadow Dragon Shinobi Armor",
		//60 Tokens
		"Shadow Dragon Shinobi Guard",
		"Shadow Dragon Shinobi Hood"
	};
	public string[] DSI300 = {DSItems[2]};
	public string[] DSI100 = {DSItems[3], DSItems[4], DSItems[5]};
	public string[] DSI60 = {DSItems[6], DSItems[7]};
	
	public int[] QuestList = {6104, 6105, 6106, 6107};
	public string[] AlphaItems = 
	{
		// (Elite) EbilCorp Keychip
		"Alpha Hunter Helm",
		"Alpha Hunter Hood",
		"Alpha Hunter Hood + Locks",
		"Alpha Hunter Hood + Hair",
		"Alpha Rogue Knife",
		"Alpha Rogue Hood + Locks",
		"Alpha Rogue Hood + Hair",
		"Alpha Rogue Hat",
		"Alpha Rogue Hat + Mask",
		//Ebil eWatches / ePhones
		"Alpha Hunter",
		"Alpha Hunter Rifle",
		"Alpha Rogue",
		"Alpha Rogue Knives"
	};
	
	public string[] DealBotItems = {
		"Dark Hunting Hood",
		"FP5 Moglin Pet"
	};
	
	public string[] DrakathItems = {
		"Black Mirror Drakath"
	};
	
	public string[] Glacialitems = {
		"Black Ice Wizard",
		"Dark Billowing Scarves"
	};

	public int FarmLoop;
	public int SavedState;
	public ScriptInterface bot => ScriptInterface.Instance;
	public void ScriptMain(ScriptInterface bot)
	{
		if (bot.Player.Cell != "Wait") bot.Player.Jump("Wait", "Spawn");

		ConfigureBotOptions();
		ConfigureLiteSettings();

		DeathHandler();
		SkillList(SkillOrder);
		
		SafeMapJoin("blackfridaywar");
		bot.Shops.Load(1498);
		string[] ECRSItems = bot.Shops.ShopItems.Select(i => i.Name).ToArray();
		CheckSpace(PrimeItems);

		while (!bot.ShouldExit())
		{
			while (!bot.Player.Loaded) { }
			
			BankArray(DSItems);
			BankArray(PrimeItems);
			BankArray(AlphaItems);
			BankArray(ECRSItems);
			BankArray(DealBotItems);			
			
			//Shadow Dragon Shinobi
			UnbankList(DSItems);
			GetDropList(DSItems);
			EquipList(FarmingGear);
			
			bot.Log($"[{DateTime.Now:HH:mm:ss}] -----Shadow Dragon Shinobi items-----");
			DSFarmArray(DSI300, 300);
			DSFarmArray(DSI100, 100);
			DSFarmArray(DSI60, 60);
			//Token Cleanup
			if (TokenCleanup)
			{
				while (bot.Inventory.Contains("Dragon Shinobi Token")) bot.Shops.SellItem("Dragon Shinobi Token");
			}
			BankArray(DSItems);
			
			//BOGOdrone PRIME drops
			UnbankList(PrimeItems);
			GetDropList(PrimeItems);
			EquipList(SoloGear);
			
			bot.Log($"[{DateTime.Now:HH:mm:ss}] -----BOGOdrone PRIME drops-----");
			FarmArray(PrimeItems, "BOGOdrone PRIME");
			
			//Alpha Hunter/Rogue
			UnbankList(AlphaItems);
			GetDropList(AlphaItems);
			EquipList(FarmingGear);
			
			bot.Log($"[{DateTime.Now:HH:mm:ss}] -----Alpha Hunter/Rogue items-----");
			foreach (string Target in AlphaItems)
			{
				if (!bot.Inventory.Contains(Target, 1)) bot.Log($"[{DateTime.Now:HH:mm:ss}] Farming \t \t [{Target}]");
				while (!bot.Inventory.Contains(Target, 1))
				{
					MultiQuestFarm(MapName: "blackfridaywar", CellName: "r4", PadName: "Left", QuestList: QuestList, MonsterName: "Hypnotized Shopper");
				}
				bot.Log($"[{DateTime.Now:HH:mm:ss}] Check \t \t [{Target}]");
			}
			BankArray(AlphaItems);
			
			//Deal Bot 2.0 drops
			UnbankList(DealBotItems);
			GetDropList(DealBotItems);
			
			bot.Log($"[{DateTime.Now:HH:mm:ss}] -----Deal Bot 2.0 Items-----");
			FarmArray(DealBotItems, "Deal Bot 2.0");
			
			BankArray(DealBotItems);
			
			//Black Mirror Drakath
			UnbankList(DrakathItems);
			GetDropList(DrakathItems);
			
			bot.Log($"[{DateTime.Now:HH:mm:ss}] -----Black Mirror Drakath-----");
			ItemFarm("Black Mirror Drakath", 1, MapName: "finalbattle", CellName: "r9", PadName: "Left");
			
			BankArray(DrakathItems);
			
			//Glacialitems
			UnbankList(Glacialitems);
			GetDropList(Glacialitems);
		
			bot.Log($"[{DateTime.Now:HH:mm:ss}] -----Glacial Elemental Items-----");
			ItemFarm("Black Ice Wizard", 1, HuntFor: true, MonsterName: "Glacial Elemental", MapName: "cryostorm");
			ItemFarm("Dark Billowing Scarves", 1, HuntFor: true, MonsterName: "Glacial Elemental", MapName: "cryostorm");
			
			BankArray(Glacialitems);
			
			//EbilCorp Reward Shop
			UnbankList(ECRSItems);
			
			bot.Log($"[{DateTime.Now:HH:mm:ss}] -----EbilCorp Reward Shop Items-----");
			bot.Shops.Load(1498);
			foreach (string Target in ECRSItems)
			{
				if (!bot.Inventory.Contains(Target, 1))
				{
					bot.Log($"[{DateTime.Now:HH:mm:ss}] Farming \t \t [{Target}]");
					SafePurchase(Target, 1, MapName: "blackfridaywar", ShopID: 1498);
				}
				bot.Log($"[{DateTime.Now:HH:mm:ss}] Check \t \t [{Target}]");
			}
			BankArray(ECRSItems);

			bot.Log($"[{DateTime.Now:HH:mm:ss}] Bot \t t Script stopped successfully.");
			StopBot("All items are obtained and banked.");
		}
	}
	
	/*------------------------------------------------------------------------------------------------------------
													 Specific Functions
	------------------------------------------------------------------------------------------------------------*/
	//These functions are specific to this bot.
	
	public void DSFarmArray(string[] Array, int ItemQuantity)
	{
		foreach (string Target in Array)
		{
			if (!bot.Inventory.Contains(Target, 1)) bot.Log($"[{DateTime.Now:HH:mm:ss}] Farming \t \t [{Target}]");
			if (!bot.Inventory.Contains(Target, 1))
			{
				DSTokenFarm(ItemQuantity);
				bot.Shops.Load(756);
				SafePurchase(Target, 1, MapName: "blackfridaywar", ShopID: 756);
			}
			bot.Log($"[{DateTime.Now:HH:mm:ss}] Check \t \t [{Target}]");
		}
	}
	
	public void DSTokenFarm(int ItemQuantity)
	{
		while (!bot.Inventory.Contains("Dragon Shinobi Token", ItemQuantity))
		{
			bot.Log($"[{DateTime.Now:HH:mm:ss}] Farming \t \t {ItemQuantity}x Dragon Shinobi Token");
			if (bot.Quests.IsUnlocked(7924))
			{
				ItemFarm("Perfect Orochi Scales", 10, QuestID: 7924, MapName: "shadowfortress", CellName: "r11", PadName: "Bottom");
				SafeQuestComplete(7924);
			}		
			else
			{
				ItemFarm("EbilCorp Bots Battled", 20, Temporary: true, HuntFor: true, QuestID: 7815, MonsterName: "Deal Bot 2.0|BOGOdrone", MapName: "blackfridaywar");
				ItemFarm("EbilCorp Shoppers Saved", 20, Temporary: true, HuntFor: true, QuestID: 7815, MonsterName: "Hypnotized Shopper", MapName: "blackfridaywar");
				SafeQuestComplete(7815);
			}
		}
	}
	
	public void FarmArray(string[] Array, string MonsterName)
	{
		foreach (string Target in Array)
		{
			if (!bot.Inventory.Contains(Target, 1)) bot.Log($"[{DateTime.Now:HH:mm:ss}] Farming \t \t [{Target}]");
			while (!bot.Inventory.Contains(Target, 1))
			{
				ItemFarm(Target, 1, HuntFor: true, MonsterName: MonsterName, MapName: "blackfridaywar");
			}
			bot.Log($"[{DateTime.Now:HH:mm:ss}] Check \t \t [{Target}]");
		}
		BankArray(Array);
	}
	
	public void BankArray(string[] Array)
	{
		ExitCombat();
		foreach (string Target in Array)
		{
			if (bot.Inventory.Contains(Target))
			{
				bot.Inventory.ToBank(Target);
				bot.Log($"[{DateTime.Now:HH:mm:ss}] Banked \t \t [{Target}]");
			}
		}
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
		*	StopBot ("Text", "MapName", "MapNumber", "CellName", "PadName", "Caption")
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
		bot.Log($"[{DateTime.Now:HH:mm:ss}] Farm \t \t Started Farming Loop {SavedState}.");
		goto maintainFarmLoop;

	breakFarmLoop:
		SmartSaveState();
		bot.Log($"[{DateTime.Now:HH:mm:ss}] Farm \t \t Completed Farming Loop {SavedState}.");
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
		bot.Log($"[{DateTime.Now:HH:mm:ss}] Farm \t \t Started Farming Loop {SavedState}.");
		goto maintainFarmLoop;

	breakFarmLoop:
		SmartSaveState();
		bot.Log($"[{DateTime.Now:HH:mm:ss}] Farm \t \t Completed Farming Loop {SavedState}.");
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
			if (!bot.Shops.IsShopLoaded)
			{
				bot.Shops.Load(ShopID);
				bot.Log($"[{DateTime.Now:HH:mm:ss}] Shop \t \t Loaded Shop {ShopID}.");
			}
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
			bot.Log($"[{DateTime.Now:HH:mm:ss}] Quest \t \t Failed to turn in Quest {QuestID}. Logging out.");
			bot.Player.Logout();
		}
		bot.Log($"[{DateTime.Now:HH:mm:ss}] Quest \t \t Turned In Quest {QuestID} successfully.");
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
		bot.Log($"[{DateTime.Now:HH:mm:ss}] Bot \t \t Bot stopped successfully.");
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
		bot.Log($"[{DateTime.Now:HH:mm:ss}] Saving \t \t Successfully Saved State.");
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
		bot.Log($"[{DateTime.Now:HH:mm:ss}] Map \t \t Joined map {mapname}-{MapNumber}, positioned at the {PadName} side of cell {CellName}.");
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