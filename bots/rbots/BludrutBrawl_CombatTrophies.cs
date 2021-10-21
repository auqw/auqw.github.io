using System;
using RBot;
using System.Collections.Generic;
using System.Windows.Forms;

public class BluuTemplate
{
	//-----------EDIT BELOW-------------//
	public int MapNumber = 2142069;
	public int CombatTrophiesRequired = 500;
	public int MovementDelay = 1337;
	public string[] RequiredItems = { 
		"Combat Trophy",
	};
	public string[] EquippedItems = { };
	public readonly int[] SkillOrder = { 3, 1, 2, 4 };
	public int SaveStateLoops = 8700;
	public int TurnInAttempts = 10;
	//-----------EDIT ABOVE-------------//

	public int FarmLoop;
	public int SavedState;
	public ScriptInterface bot => ScriptInterface.Instance;
	public void ScriptMain(ScriptInterface bot)
	{
		if (bot.Player.Cell != "Wait") bot.Player.Jump("Wait", "Spawn");

		ConfigureBotOptions(LagKiller: false);
		ConfigureLiteSettings();

		SkillList(SkillOrder);
		EquipList(EquippedItems);
		UnbankList(RequiredItems);
		CheckSpace(RequiredItems);
		GetDropList(RequiredItems);

		while (!bot.ShouldExit())
		{
			while (!bot.Player.Loaded) { }
			while (!bot.Inventory.Contains("Combat Trophy", CombatTrophiesRequired))
            {
				SchoolFooting();
				SchoolShooting();
				SchoolLooting();
			}
			StopBot($"Successfully Farmed {CombatTrophiesRequired} Combat Trophies.");
		}
		bot.Log($"[{DateTime.Now:HH:mm:ss}] Script stopped successfully.");
		StopBot();
	}

	/*------------------------------------------------------------------------------------------------------------
														  Specific Functions
	------------------------------------------------------------------------------------------------------------*/

	/*
		*   These functions are specific to this script.
	*/

	public void SchoolFooting()
    {
		ExitCombat();
		while (bot.Player.Cell != "Captain1")
        {
			while (bot.Player.Cell != "Morale1C")
            {
				while (bot.Player.Cell != "Morale1B")
                {
					while (bot.Player.Cell != "Morale1A")
                    {
						while (bot.Player.Cell != "Crosslower")
                        {
							while (bot.Player.Cell != "Morale0A")
                            {
								while (bot.Player.Cell != "Morale0B")
                                {
									while (bot.Player.Cell != "Morale0C")
                                    {
										while (bot.Player.Cell != "Enter0")
                                        {
											SafeMapJoin("bludrutbrawl", "Enter0", "Spawn");
											bot.Wait.ForMapLoad("bludrutbrawl");
										}
										bot.SendPacket($"%xt%zm%mv%121273%828%272%8%");
										bot.Sleep(MovementDelay);
										bot.SendPacket($"%xt%zm%mtcid%121273%5%");
									}
									bot.SendPacket($"%xt%zm%mv%121273%836%276%8%");
									bot.Sleep(MovementDelay);
									bot.SendPacket($"%xt%zm%mtcid%121273%4%");
								}
								bot.SendPacket($"%xt%zm%mv%121273%832%276%8%");
								bot.Sleep(MovementDelay);
								bot.SendPacket($"%xt%zm%mtcid%121273%7%");
							}
							bot.SendPacket($"%xt%zm%mv%121273%832%276%8%");
							bot.Sleep(MovementDelay);
							bot.SendPacket($"%xt%zm%mtcid%121273%9%");
						}
						bot.SendPacket($"%xt%zm%mv%121273%832%276%8%");
						bot.Sleep(MovementDelay);
						bot.SendPacket($"%xt%zm%mtcid%121273%15%");
					}
					bot.SendPacket($"%xt%zm%mv%121273%832%276%8%");
					bot.Sleep(MovementDelay);
					bot.SendPacket($"%xt%zm%mtcid%121273%23%");
				}
				bot.SendPacket($"%xt%zm%mv%121273%832%276%8%");
				bot.Sleep(MovementDelay);
				bot.SendPacket($"%xt%zm%mtcid%121273%25%");
			}
			bot.SendPacket($"%xt%zm%mv%121273%507%255%8%");
			bot.Sleep(MovementDelay);
			bot.SendPacket($"%xt%zm%mtcid%121273%28%");
		}
	}

	public void SchoolShooting()
    {
		while (bot.Monsters.Exists("Team B Captain"))
        {
			foreach (var Skill in SkillOrder)
			{
				bot.Player.UseSkill(Skill);
				bot.Player.Attack("Team B Captain");
			}
		}
	}

	public void SchoolLooting()
    {
		bot.Wait.ForDrop("Combat Trophy");
		while (bot.Player.State == 2) { }
		if (bot.Map.Name != "battleon") bot.SendPacket("%xt%zm%cmd%78664%tfer%" + bot.Player.Username + "%Battleon-" + MapNumber + "%Enter%Spawn%");
	}
	
	/*------------------------------------------------------------------------------------------------------------
													   Invokable Functions
	  ------------------------------------------------------------------------------------------------------------*/

	/*
		*   These functions are used to perform a major action in AQW.
		*   All of them require at least one of the Auxiliary Functions listed below to be present in your script.
		*   Some of the functions require you to pre-declare certain integers under "public class Script"
		*   InvItemFarm and TempItemFarm will require some Background Functions to be present as well.
		*   All of this information can be found inside the functions. Make sure to read.
		*   InvItemFarm("ItemName", ItemQuantity, "MapName", "MapNumber", "CellName", "PadName", QuestID, "MonsterName");
		*   TempItemFarm("TempItemName", TempItemQuantity, "MapName", "MapNumber", "CellName", "PadName", QuestID, "MonsterName");
		*   SafeEquip("ItemName");
		*   SafePurchase("ItemName", ItemQuantityNeeded, "MapName", "MapNumber", ShopID)
		*	SafeSell("ItemName", ItemQuantityNeeded)
		*	SafeQuestComplete(QuestID, ItemID)
		*	StopBot ("Text", "MapName", "MapNumber", "CellName", "PadName")
	*/

	/// <summary>
	/// Farms you the specified quantity of the specified item with the specified quest accepted from specified monsters in the specified location. Saves States every ~5 minutes.
	/// </summary>
	public void InvItemFarm(string ItemName, int ItemQuantity, string MapName, string CellName, string PadName, int QuestID = 0, string MonsterName = "*")
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
		bot.Log($"[{DateTime.Now:HH:mm:ss}] Started Farming Loop {SavedState}.");
		goto maintainFarmLoop;

	breakFarmLoop:
		SmartSaveState();
		bot.Log($"[{DateTime.Now:HH:mm:ss}] Completed Farming Loop {SavedState}.");
		FarmLoop = 0;
		goto startFarmLoop;

	maintainFarmLoop:
		while (!bot.Inventory.Contains(ItemName, ItemQuantity))
		{
			FarmLoop++;
			if (bot.Map.Name != MapName) SafeMapJoin(MapName, CellName, PadName);
			if (bot.Player.Cell != CellName) bot.Player.Jump(CellName, PadName);
			if (QuestID > 0) bot.Quests.EnsureAccept(QuestID);
			bot.Options.AggroMonsters = true;
			bot.Player.Attack(MonsterName);
			if (FarmLoop > SaveStateLoops) goto breakFarmLoop;
		}
	}

	/// <summary>
	/// Farms you the required quantity of the specified temp item with the specified quest accepted from specified monsters in the specified location.
	/// </summary>
	public void TempItemFarm(string TempItemName, int TempItemQuantity, string MapName, string CellName, string PadName, int QuestID = 0, string MonsterName = "*")
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
		bot.Log($"[{DateTime.Now:HH:mm:ss}] Started Farming Loop {SavedState}.");
		goto maintainFarmLoop;

	breakFarmLoop:
		SmartSaveState();
		bot.Log($"[{DateTime.Now:HH:mm:ss}] Completed Farming Loop {SavedState}.");
		FarmLoop = 0;
		goto startFarmLoop;

	maintainFarmLoop:
		while (!bot.Inventory.ContainsTempItem(TempItemName, TempItemQuantity))
		{
			FarmLoop++;
			if (bot.Map.Name != MapName) SafeMapJoin(MapName, CellName, PadName);
			if (bot.Player.Cell != CellName) bot.Player.Jump(CellName, PadName);
			if (QuestID > 0) bot.Quests.EnsureAccept(QuestID);
			bot.Options.AggroMonsters = true;
			bot.Player.Attack(MonsterName);
			if (FarmLoop > SaveStateLoops) goto breakFarmLoop;
		}
	}

	/// <summary>
	/// Equips an item.
	/// </summary>
	public void SafeEquip(string ItemName)
	{
		//Must have the following functions in your script:
		//ExitCombat

		while (!bot.Inventory.IsEquipped(ItemName))
		{
			ExitCombat();
			bot.Player.EquipItem(ItemName);
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
			if (bot.Map.Name != MapName) SafeMapJoin(MapName, "Wait", "Spawn");
			ExitCombat();
			if (!bot.Shops.IsShopLoaded)
			{
				bot.Shops.Load(ShopID);
				bot.Log($"[{DateTime.Now:HH:mm:ss}] Loaded Shop {ShopID}.");
			}
			bot.Shops.BuyItem(ItemName);
			bot.Log($"[{DateTime.Now:HH:mm:ss}] Purchased {ItemName} from Shop {ShopID}.");
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
	/// Attempts to complete the quest thrice. If it fails to complete, logs out. If it successfully completes, re-accepts the quest and checks if it can be completed again.
	/// </summary>
	public void SafeQuestComplete(int QuestID, int ItemID = -1)
	{
	//Must have the following functions in your script:
	//ExitCombat

	maintainCompleteLoop:
		ExitCombat();
		bot.Quests.EnsureAccept(QuestID);
		bot.Quests.EnsureComplete(QuestID, ItemID, tries: TurnInAttempts);
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

	/// <summary>
	/// Stops the bot at yulgar if no parameters are set, or your specified map if the parameters are set.
	/// </summary>
	public void StopBot(string Text = "Bot stopped successfully.", string MapName = "yulgar", string CellName = "Enter", string PadName = "Spawn", string Caption = "Stopped")
	{
		//Must have the following functions in your script:
		//SafeMapJoin
		//ExitCombat

		if (bot.Map.Name != MapName) SafeMapJoin(MapName, CellName, PadName);
		if (bot.Player.Cell != CellName) bot.Player.Jump(CellName, PadName);
		bot.Drops.RejectElse = false;
		bot.Options.LagKiller = false;
		bot.Options.AggroMonsters = false;
		bot.Log($"[{DateTime.Now:HH:mm:ss}] Bot stopped successfully.");
		Console.WriteLine(Text);
		MessageBox.Show(Text, Caption);
		bot.Exit();
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
		bot.SendPacket("%xt%zm%whisper%1% creating save state%" + bot.Player.Username + "%");
		bot.Log($"[{DateTime.Now:HH:mm:ss}] Successfully Saved State.");
	}

	/// <summary>
	/// Joins the specified map.
	/// </summary>
	public void SafeMapJoin(string MapName = "yulgar", string CellName = "Enter", string PadName = "Spawn")
	{
		//Must have the following functions in your script:
		//ExitCombat

		while (bot.Map.Name != MapName)
		{
			ExitCombat();
			if (MapName == "tercessuinotlim")
			{
				while (bot.Map.Name != "citadel")
				{
					bot.Player.Join($"citadel-{MapNumber}", "m22", "Left");
					bot.Wait.ForMapLoad("citadel");
					bot.Sleep(500);
				}
				if (bot.Player.Cell != "m22") bot.Player.Jump("m22", "Left");
			}
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

	/*
		*   These functions help you to either configure certain settings or run event handlers in the background.
		*   It is highly recommended to have all these functions present in your script as they are very useful.
		*   Some Invokable Functions may call or require the assistance of some Background Functions as well.
		*   These functions are to be run at the very beginning of the bot under public class Script.
		*   ConfigureBotOptions("PlayerName", "GuildName", LagKiller, SafeTimings, RestPackets, AutoRelogin, PrivateRooms, InfiniteRange, SkipCutscenes, ExitCombatBeforeQuest)
		*   ConfigureLiteSettings(UntargetSelf, UntargetDead, CustomDrops, ReacceptQuest, SmoothBackground)
		*   SkillList(int[])
		*   GetDropList(string[])
		*   ItemWhiteList(string[])
		*   EquipList(string[])
		*   UnbankList(string[])
	*/

	/// <summary>
	/// Change the player's name and guild for your bots specifications.
	/// Recommended Default Bot Configurations.
	/// </summary>
	public void ConfigureBotOptions(string PlayerName = "Bot By AuQW", string GuildName = "https://auqw.tk/", bool LagKiller = true, bool SafeTimings = true, bool RestPackets = true, bool AutoRelogin = true, bool PrivateRooms = false, bool InfiniteRange = true, bool SkipCutscenes = true, bool ExitCombatBeforeQuest = true)
	{
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
		bot.Events.PlayerDeath += PD => ScriptManager.RestartScript();
		bot.Events.PlayerAFK += PA => ScriptManager.RestartScript();
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
			SafeEquip(Item);
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
	/// <param name="UnbankList"></param>
	public void CheckSpace(params string[] UnbankList)
	{
		int MaxSpace = bot.GetGameObject<int>("world.myAvatar.objData.iBagSlots");
		int FilledSpace = bot.GetGameObject<int>("world.myAvatar.items.length");
		int EmptySpace = MaxSpace - FilledSpace;
		int SpaceNeeded = 0;

		foreach (var Item in UnbankList)
		{
			if (!bot.Inventory.Contains(Item)) SpaceNeeded++;
		}

		if (EmptySpace < SpaceNeeded)
		{
			StopBot($"Need {SpaceNeeded} empty inventory slots, please make room for the quest.", bot.Map.Name, bot.Player.Cell, bot.Player.Pad, "Error");
		}
	}
}