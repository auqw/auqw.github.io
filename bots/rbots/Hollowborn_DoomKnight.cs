using System;
using RBot;
using System.Collections.Generic;
using System.Linq;

public class ExeScript
{
	//-----------EDIT BELOW-------------//
	public readonly int[] SkillOrder = { 3, 1, 2, 4 };
	public int SaveStateLoops = 8700;
	public int TurnInAttempts = 10;
	public string[] FarmGear = { };
	public string[] SoloGear = { };
	//-----------EDIT ABOVE-------------//

	public int MapNumber = 2142069;
	public int[] QuestList = {7553, 7555};
	public string[] AlchemyItems = { "Ice Vapor", "Dragon Scale"};
	public int[] ALchemyNumbers = {30, 30};
	public string[] Contract = { "Lae's Hardcore Contract" };
	public string[] ContractReq = {
		//ForQuest
		"Soul Potion",
		 "Necrot",
		 "Arashtite Ore",
		"Human Soul",
		"Fallen Soul",
		//Target + KeepInv
		"Lae's Hardcore Contract"
	};
    public string[] ADK = {
		//Target
		"Hollowborn DoomKnight Helm",
		"Hollowborn DoomKnight Hood",
		"Hollowborn Doom Cloak",
		//ForQuest + KeepInv
		"Lae's Hardcore Contract",
		"Dark Fragment",
		 "Shadowworn",
		 "Empowered Essence",
		 "Shadowscythe Venom Head",
		 "Hollow Soul",
		"Shadow DoomReaver",
		"Malignant Essence"
    };
    public string[] ADKRises = {
		//Target
		"Classic Hollowborn Doomknight",
		//ForQuest + KeepInv
		"Lae's Hardcore Contract",
		"Dark Fragment",
		 "Shadowworn",
		 "Empowered Essence",
		 "Shadowscythe Venom Head",
		 "Hollow Soul",
		"Doom Fragment",
		 "Doomatter",
		 "Shadow DoomReaver",
		 "Worshipper of Doom",
		 "Ingredients?",
		"Malignant Essence"
	};
	public string[] ADKFalls = {
		//Target
		"Hollowborn Empress' Blade",
		"Hollowborn DoomBlade",
		//ForQuest
		"Unidentified 25",
		 "Unmoulded Fiend Essence",
		"Royal ShadowScythe Blade",
		//KeepInv
		"Lae's Hardcore Contract",
		"Dark Fragment",
		 "Shadowworn",
		 "Empowered Essence",
		 "Shadowscythe Venom Head",
		 "Hollow Soul",
		"Doom Fragment",
		 "Doomatter",
		 "Shadow DoomReaver",
		 "Worshipper of Doom",
		 "Ingredients?",
		"Malignant Essence",
		"Weapon Imprint",
		"(Necro) Scroll of Dark Arts"
	};
	public string[] ADKReturns = {
		//Target
		"Hollowborn DoomKnight",
		"Hollowborn Sepulchure's Helm",
		"Hollowborn Doom Shade",
		"Hollowborn Sword of Doom",
		//ForQuest
		"Hollowborn Shadow of Fate",
		"Lae's Hardcore Contract",
		"Necrotic Sword of Doom",
		"Sepulchure's DoomKnight Armor",
		"Sepulchure's Original Helm",
		"Dark Energy",
		"(Necro) Scroll of Dark Arts",
		"Bones from the Void Realm",
		 "Bone Dust",
		 "Void Aura",
		  "Empowered Essence",
		  "Malignant Essence",
		"Doom Heart",
		"Weapon Imprint",
		"Desolich's Dark Horn",
		"Dark Fragment",
		 "Shadowworn",
		 "Empowered Essence",
		 "Shadowscythe Venom Head",
		 "Hollow Soul",
		"Doom Fragment",
		 "Doomatter",
		 "Shadow DoomReaver",
		 "Worshipper of Doom",
		 "Ingredients?"
	};

	public int FarmLoop;
	public int SavedState;
	public ScriptInterface bot => ScriptInterface.Instance;
	public void ScriptMain(ScriptInterface bot)
	{
		bot.Log($"[{DateTime.Now:HH:mm:ss}] -----Scrip Launched-----");
		if (bot.Player.Cell != "Wait") bot.Player.Jump("Wait", "Spawn");

		ConfigureBotOptions();
		ConfigureLiteSettings();

		DeathHandler();
		SkillList(SkillOrder);
				
		bot.Log($"[{DateTime.Now:HH:mm:ss}] -----Pre Farm Inventory Clean-----");
		BankArray(ADKReturns);
		BankArray(ADKFalls);
		BankArray(ADKRises);
		BankArray(ADK);
		BankArray(Contract);
		
		CheckSpace(ADKReturns);

		while (!bot.ShouldExit())
		{
			while (!bot.Player.Loaded) { }
			if (bot.Player.Level < 65) 
				StopBot("Must be level 65.");
			if (bot.Player.GetFactionRank("evil") < 10) 
				StopBot("Faction Rank 10 Evil required.");
			if (!bot.Quests.IsUnlocked(3004))
				StopBot("Completion of the DoomVault B story is required.");
			
			//Lae's Hardcore Contract	(7556)
			UnbankList(Contract);
			if (!bot.Inventory.Contains("Lae's Hardcore Contract")){
				bot.Log($"[{DateTime.Now:HH:mm:ss}] -----Lae's Hardcore Contract-----");
				bot.Log($"[{DateTime.Now:HH:mm:ss}] Obtaining \t [Lae's Hardcore Contract]");
				UnbankList(ContractReq);
				GetDropList(ContractReq);
				EquipList(FarmGear);
				GetSoulPotion();
				ItemFarm(
					"Human Soul", 50,  
					QuestID: 7556, 
					MapName: "noxustower",
					CellName: "r14",
					PadName: "Left"
				);
				ItemFarm(
					"Fallen Soul", 13, 
					HuntFor: true, 
					QuestID: 7556,
					MonsterName: "Undead Paladin", 
					MapName: "doomwood"
				);
				SafeQuestComplete(7556);
				bot.Log($"[{DateTime.Now:HH:mm:ss}] Check \t \t [Lae's Hardcore Contract]");
			}
			BankArrayUpTo(ContractReq, UpTo: "Fallen Soul");
			
			//A Dark Knight			(8413)
			bot.Log($"[{DateTime.Now:HH:mm:ss}] -----A Dark Knight-----");
			UnbankList(ADK);
			GetDropList(ADK);
			foreach (string Target in ADK) {
				if (Array.IndexOf(ADK, Target) <= 2) {
					bot.Log($"[{DateTime.Now:HH:mm:ss}] Obtaining \t [{Target}]");
					if (!bot.Inventory.Contains(Target))
						ADKFarm(Target);
					bot.Log($"[{DateTime.Now:HH:mm:ss}] Check \t \t [{Target}]");
				}
			}
			BankArrayUpTo(ADK, UpTo: "Hollowborn Doom Cloak");
						
			//A Dark Knight Rises 	(8414)
			bot.Log($"[{DateTime.Now:HH:mm:ss}] -----A Dark Knight Rises-----");
			UnbankList(ADKRises);
			GetDropList(ADKRises);
			
			bot.Log($"[{DateTime.Now:HH:mm:ss}] Obtaining \t [Classic Hollowborn Doomknight]");
			if (!bot.Inventory.Contains("Classic Hollowborn Doomknight"))
				ADKRisesFarm();
			bot.Log($"[{DateTime.Now:HH:mm:ss}] Check \t \t [Classic Hollowborn Doomknight]");
			
			BankArrayUpTo(ADKRises, UpTo: "Classic Hollowborn Doomknight");
			
			//A Dark Knight Falls 	(8415)
			bot.Log($"[{DateTime.Now:HH:mm:ss}] -----A Dark Knight Falls-----");
			UnbankList(ADKFalls);
			GetDropList(ADKFalls);
			
			bot.Log($"[{DateTime.Now:HH:mm:ss}] Obtaining \t [Hollowborn Empress' Blade]");
			bot.Log($"[{DateTime.Now:HH:mm:ss}] ↑ \t \t [Hollowborn DoomBlade]");
			if (
			!bot.Inventory.Contains("Hollowborn Empress' Blade") || 
			!bot.Inventory.Contains("Hollowborn DoomBlade")) {
				if (!bot.Inventory.Contains("Unidentified 25", 1)) {
					GoldCheck(15000100);
					SafePurchase(
						"Unmoulded Fiend Essence", 1,
						MapName: "tercessuinotlim",
						ShopID: 1951
					);
					SafePurchase(
						"Unidentified 25", 1,
						MapName: "tercessuinotlim",
						ShopID: 1951
					);
				}
				EquipList(SoloGear);
				ItemFarm(
					"(Necro) Scroll of Dark Arts", 1,
					HuntFor: true,
					QuestID: 8415, 
					MonsterName: "Ultra Vordred", 
					MapName: "epicvordred"
				);
				GoldCheck(1000000);
				SafePurchase(
					"Royal ShadowScythe Blade", 1,
					MapName: "shadowfall",
					ShopID: 1639
				);
				EquipList(SoloGear);
				MapNumber = 1;
				ItemFarm(
					"Weapon Imprint", 1,
					HuntFor: true,
					QuestID: 8415, 
					MonsterName: "Undead Raxgore",
					MapName: "doomvaultb"
				);
				MapNumber = 2142069;
				while (!bot.Inventory.Contains("Doom Fragment", 5)) 
					ADKRisesFarm();
				while (!bot.Inventory.Contains("Dark Fragment", 20)) 
					ADKFarm();
				SafeQuestComplete(8415);
			}	
			bot.Log($"[{DateTime.Now:HH:mm:ss}] Check \t \t [Hollowborn Empress' Blade]");
			bot.Log($"\n \t \t \t [Hollowborn DoomBlade]");
			BankArrayUpTo(ADKFalls, "Royal ShadowScythe Blade");
			
			//A Dark Knight Returns (8416)
			bot.Log($"[{DateTime.Now:HH:mm:ss}] -----A Dark Knight Returns-----");
			UnbankList(ADKReturns);
			GetDropList(ADKReturns);
			
			if (
				(bot.Player.Level <= 99) || 
				(!bot.Inventory.Contains("Hollowborn Shadow of Fate")) ||
				(!bot.Inventory.Contains("Lae's Hardcore Contract")) ||
				(!bot.Inventory.Contains("Necrotic Sword of Doom")) ||
				(!bot.Inventory.Contains("Sepulchure's DoomKnight Armor")) ||
				(!bot.Inventory.Contains("Sepulchure's Original Helm"))
			)
				StopBot("One or more of the requierments for 'A Dark Knight Returns' are not met. Previous quest items acquired and banked.");
			bot.Log($"[{DateTime.Now:HH:mm:ss}] Checking \t [Hollowborn DoomKnight]");
			bot.Log($"[{DateTime.Now:HH:mm:ss}] ↑ \t \t[Hollowborn Sepulchure's Helm]");
			bot.Log($"[{DateTime.Now:HH:mm:ss}] ↑ \t \t[Hollowborn Doom Shade]");
			bot.Log($"[{DateTime.Now:HH:mm:ss}] ↑ \t \t[Hollowborn Sword of Doom]");
			if (
				!bot.Inventory.Contains("Hollowborn DoomKnight") || 
				!bot.Inventory.Contains("Hollowborn Sepulchure's Helm") ||
				!bot.Inventory.Contains("Hollowborn Doom Shade") ||
				!bot.Inventory.Contains("Hollowborn Sword of Doom")
			) {
				EquipList(FarmGear);
				ItemFarm(
					"Dark Energy", 10000,
					QuestID: 8416,
					MapName: "dwarfhold",
					CellName: "r2",
					PadName: "Left"
				);
				EquipList(SoloGear);
				MapNumber = 1;
				ItemFarm(
					"(Necro) Scroll of Dark Arts", 3,
					HuntFor: true,
					QuestID: 8416,
					MonsterName: "Ultra Vordred", 
					MapName: "epicvordred"
				);
				if (!bot.Inventory.Contains("Bones from the Void Realm")) {
					EquipList(FarmGear);
					ItemFarm(
						"Bone Dust", 50,
						QuestID: 8416,
						MonsterName: "Skeleton Warrior",
						MapName: "battleunderb"
					);
					while (!bot.Inventory.Contains("Void Aura", 50)) {
						EquipList(FarmGear);
						MapNumber = 2142069;
						ItemFarm(
							"Empowered Essence", 50, 
							QuestID: 4439, 
							MapName: "shadowrealmpast"
						);
						EquipList(SoloGear);
						MapNumber = 1;
						ItemFarm(
							"Malignant Essence", 3,
							HuntFor: true,
							QuestID: 4439,
							MonsterName: "Shadow Lord", 
							MapName: "shadowrealmpast"
						);
						SafeQuestComplete(4439);
					}
					SafePurchase(
						"Bones from the Void Realm", 1,
						MapName: "shadowfall",
						ShopID: 793
					);
				}
				EquipList(SoloGear);
				ItemFarm(
					"Doom Heart", 1,
					QuestID: 8416,
					MapName: "sepulchurebattle"
				);
				ItemFarm(
					"Weapon Imprint", 1,
					HuntFor: true,
					QuestID: 8416,
					MonsterName: "Undead Raxgore",
					MapName: "doomvaultb"
				);
				ItemFarm(
					"Desolich's Dark Horn", 3,
					HuntFor: true,
					QuestID: 8416,
					MonsterName: "Desolich",
					MapName: "Desolich"
				);
				MapNumber = 2142069;
				while (!bot.Inventory.Contains("Doom Fragment", 10))
					ADKRisesFarm();
				while (!bot.Inventory.Contains("Dark Fragment", 30))
					ADKFarm();
				SafeQuestComplete(8416);		
			}
			BankArray(ADKReturns);
			bot.Log($"[{DateTime.Now:HH:mm:ss}] Bot \t \t Script stopped successfully.");
			StopBot("All items are obtained and banked.");
		}
	}

	/*------------------------------------------------------------------------------------------------------------
													 Specific Functions
	------------------------------------------------------------------------------------------------------------*/
	//These functions are specific to this bot.
	
	public void GetSoulPotion()
	{
		if (bot.Player.GetFactionRank("alchemy") < 8)
			{
				bot.Log($"[{DateTime.Now:HH:mm:ss}] Alchemy \t \t Rank below 8, starting rep farm");
				AlchemyRanking();
			}
		if (!bot.Inventory.Contains("Soul Potion")) {
			bot.Log($"[{DateTime.Now:HH:mm:ss}] Obtaining \t [Soul Potion]");
			ItemFarm(
				"Necrot", 1,
				HuntFor: true,
				MonsterName: "Skeleton Fighter",
				MapName: "deathsrealm"
			);
			ItemFarm(
				"Arashtite Ore", 1,
				HuntFor: true,
				MonsterName: "Deathmole",
				MapName: "orecavern"
			);
			SafeMapJoin("Alchemy");
            		bot.SendPacket("%xt%zm%crafting%1%getAlchWait%11480%11473%false%Ready to Mix%Necrot%Arashtite Ore%Gebo%Man%");
            		bot.Sleep(15000);
           		bot.SendPacket("%xt%zm%crafting%1%checkAlchComplete%11480%11473%false%Mix Complete%Necrot%Arashtite Ore%Gebo%Man%");
            		bot.Sleep(700);
        	}
	}
	
	public void ADKFarm(string Target = "Dark Fragment")
	{
		bot.Log($"[{DateTime.Now:HH:mm:ss}] Obtaining \t \t [{Target}]");
		EquipList(SoloGear);
		ItemFarm(
			"Shadowworn", 1, 
			HuntFor: true, 
			QuestID: 8413, 
			MonsterName: "Shadow Lord", 
			MapName: "shadowrealmpast"
		);
		EquipList(FarmGear);
		ItemFarm(
			"Empowered Essence", 10, 
			MapName: "shadowrealmpast"
		);
		GoldCheck(100000);
		SafePurchase(
			"Shadowscythe Venom Head", 1, 
			MapName: "shadowfall",
			ShopID: 89
		);
		while (!bot.Inventory.Contains("Hollow Soul", 10)) {
			MultiQuestFarm(
				MapName: "shadowrealm", 
				CellName: "r2", 
				PadName: "Down", 
				QuestList: QuestList
			);
		}
		
		if (Target == "Hollowborn DoomKnight Helm") 
			SafeQuestComplete(8413, 65837);
		else if (Target == "Hollowborn DoomKnight Hood") 
			SafeQuestComplete(8413, 65838);
		else if (Target == "Hollowborn Doom Cloak") 
			SafeQuestComplete(8413, 65839);
		else if (Target == "Dark Fragment")
			SafeQuestComplete(8413, 65837);
		bot.Wait.ForPickup(Target);
		bot.Log($"[{DateTime.Now:HH:mm:ss}] Check \t \t [{Target}]");
	}
	
	public void ADKRisesFarm()
	{
		EquipList(SoloGear);
		ItemFarm(
			"Doomatter", 10,
			HuntFor: true,
			QuestID: 8414,
			MonsterName: "Creature Creation",
			MapName: "maul"
		);
		ItemFarm(
			"Shadow DoomReaver", 1,
			HuntFor: true,
			MonsterName: "Shadow Lord",
			MapName: "shadowrealmpast"
		);
		MapNumber = 1;
		ItemFarm(
			"Worshipper of Doom", 1,
			HuntFor: true,
			MonsterName: "Corrupted Luma",
			MapName: "lumafortress"
		);
		ItemFarm(
			"Ingredients?", 10,
			HuntFor: true,
			QuestID: 7325,
			MonsterName: "Binky",
			MapName: "doomvault"
		);
		MapNumber = 2142069;
		while (!bot.Inventory.Contains("Dark Fragment", 5))
			ADKFarm();
		SafeQuestComplete(8414);
		bot.Wait.ForPickup("Doom Fragment");
	}
	
	public void GoldCheck(int GoldNeeded)
	{
		if (bot.Player.Gold <= GoldNeeded){
			bot.Log($"[{DateTime.Now:HH:mm:ss}] Obtaining \t \t [{GoldNeeded} Gold]");
			EquipList(FarmGear);
			if (bot.Player.Level < 75)
				SafeMapJoin("icestormarena", "r3b", "Top");
			else 
				SafeMapJoin("icestormarena", "r3c", "Top");
			while (bot.Player.Gold <= GoldNeeded)
				bot.Player.Attack("*");
			bot.Player.Jump("Wait", "Enter");
		}
	}
	
	public void AlchemyRanking() //Thank you Tato
	{
		UnbankList(AlchemyItems);
		while (bot.Player.GetFactionRank("alchemy") < 8)
		{
			BankArrayUpTo(AlchemyItems, UpTo: "Ice Vapor");
			bot.Log($"[{DateTime.Now:HH:mm:ss}] AlchemyRanking \t Aquiring Needed Items");
			ItemFarm(
			"Dragon Scale", 30,
			HuntFor: true,
			QuestID: 0,
			MonsterName: "Water Draconian",
			MapName: "lair"
		);

			ItemFarm(
			"Ice Vapor", 30,
			HuntFor: true,
			QuestID: 0,
			MonsterName: "Water Draconian",
			MapName: "lair"
		);

			bot.Log($"[{DateTime.Now:HH:mm:ss}] AlchemyRanking \t Making Potions");			
			bot.Player.Pickup("Dragon Scale", "Ice Vapor");
			
			bot.Player.Join("alchemy-9943199", "Enter", "Spawn");
			bot.Sleep(500);
			while(bot.Inventory.GetQuantity("Dragon Scale") > 1 && bot.Inventory.GetQuantity("Ice Vapor") > 1) 
			{
			bot.SendPacket("%xt%zm%crafting%1%getAlchWait%11475%11478%false%Ready to Mix%Dragon Scale%Ice Vapor%Jera%Moose%");
			bot.Sleep(10000);
			bot.SendPacket("%xt%zm%crafting%1%checkAlchComplete%11475%11475%fa lse%Mix Complete%Dragon Scale%Ice Vapor%Gebo%Moose%");
			bot.Sleep(500);
			}
		}
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
	
	public void BankArrayUpTo(string[] ArrayIn, string UpTo)
	{
		ExitCombat();
		foreach (string Target in ArrayIn)
		{
			if ((Array.IndexOf(ArrayIn, Target)) <= (Array.IndexOf(ArrayIn, UpTo))) {
				if (bot.Inventory.Contains(Target))
				{
					bot.Inventory.ToBank(Target);
					bot.Log($"[{DateTime.Now:HH:mm:ss}] Banked* \t \t [{Target}]");
				}
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
	
		bot.Log($"[{DateTime.Now:HH:mm:ss}] Obtaining \t [{ItemName}]");
	
	startFarmLoop:
		if (FarmLoop > 0) goto maintainFarmLoop;
		SavedState++;
		goto maintainFarmLoop;

	breakFarmLoop:
		SmartSaveState();
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
				bot.Log($"[{DateTime.Now:HH:mm:ss}] Check \t \t [{ItemName}]");
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
				bot.Log($"[{DateTime.Now:HH:mm:ss}] Check \t \t [{ItemName}]");
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
				bot.Log($"[{DateTime.Now:HH:mm:ss}] Check \t \t [{ItemName}]");
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
				bot.Log($"[{DateTime.Now:HH:mm:ss}] Check \t \t [{ItemName}]");
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
		goto maintainFarmLoop;

	breakFarmLoop:
		SmartSaveState();
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
		bot.Log($"[{DateTime.Now:HH:mm:ss}] Save \t \t Successfully Saved State.");
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
		bot.Log($"[{DateTime.Now:HH:mm:ss}] Joined \t \t [{mapname}-{MapNumber}, {PadName}, {CellName}]");
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
		if(bot.Handlers.Any(h => h.Name == "Drop Handler"))
			bot.Handlers.RemoveAll(h => h.Name == "Drop Handler");
		bot.RegisterHandler(4, b => {
			foreach (string Item in GetDropList)
			{
				if (bot.Player.DropExists(Item)) bot.Player.Pickup(Item);
			}
			bot.Player.RejectExcept(GetDropList);
		}, "Drop Handler");
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