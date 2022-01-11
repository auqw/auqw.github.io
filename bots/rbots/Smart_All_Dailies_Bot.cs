using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RBot;
using RBot.Options;
using RBot.Items;

public class SmartDailies
{	

	//-----------EDIT BELOW-------------//
	public static int PrivateRoomNumber = 999999;
	public int SaveStateLoops = 8700;
	public int TurnInAttempts = 10;
	public int ScriptDelay = 700;
	public string[] SoloingGear = {"LightCaster"};
	public string[] FarmingGear = {"Vampire Lord"};
	public readonly int[] SoloingSkillOrder = { 3, 1, 2, 4 };
	public readonly int[] FarmingSkillOrder = { 4, 2, 3, 1 };

	// If you dont want the popup menu that comes up, set DisableMenu to true
	// Warning: It wont do any Dailies that require you to select your prefered reward in the menu
	public static bool DisableMenu = false;

	//-----------EDIT ABOVE-------------//

	public string OptionsStorage = "DailyOptionStorage";
	public string[] MultiOptions = {"A_Info", "B_General_Options", "C_Toggle_entire_Catagories", "D_Toggle_specific_Classes", "E_Toggle_specific_Priority_Misc_Items", "F_Toggle_specific_Cosmetics", "G_Toggle_specific_Misc_Items"};
	public List<IOption> A_Info = new List<IOption>() {
		new Option<string>("-", "Close this window in order to start the bot", " ", " ")
	};
	public List<IOption> B_General_Options = new List<IOption>() {
		new Option<bool>("buyGoal", "Buy the Final Product", "If enabled, the bot will automatically buy the final product (when available) of the things it farmed for.", true),
		new Option<bool>("DisableQuantity", "Disable Quantity checks", "If enabled, the bot will continue to farm items even if the goal has already been reached.", false),
		new Option<bool>("PrivateOnly", "Make all rooms private", "If enabled, the bot will make all rooms you go to private. If disabled, it will swap depending on if it's on Farm or Solo mode for that Daily Quest", false),
		new Option<bool>("DisableHunt", "Disable Hunting mode", "Highly recommended to leave this off. Only turn this on if you are having issues with RBot's Hunting feature.", false)
	};
	public List<IOption> C_Toggle_entire_Catagories = new List<IOption>() {
		// Catagories
		new Option<bool>("DisableClasses", "Disable all Classes", "If enabled, the bot will automatically skip the entire 'Classes' section.", false),
		new Option<bool>("DisableCosmetics", "Disable all Cosmetics ", "If enabled, the bot will automatically skip the entire 'Cosmetics' section.", false),
		new Option<bool>("DisablePrioMisc", "Disable all Priority Misc Items", "If enabled, the bot will automatically skip the entire 'Priority Misc. Items' section.", false),
		new Option<bool>("DisableMisc", "Disable all non-Priority Misc Items", "If enabled, the bot will automatically skip the entire 'Misc. Items' section.", false)
	};
	public List<IOption> D_Toggle_specific_Classes = new List<IOption>() {
		// Classes
		new Option<bool>("LordOfOrder", "Lord of Order", "If disabled, the bot will automatically skip the 'Lord of Order' check.\nCompletion of these stories is required for seperate stages:\n - /CitadelRuins + /LivingDungeon + /Drakonnan + /DoomVaultB", true),
		new Option<bool>("Pyromancer", "Pyromancer", "If disabled, the bot will automatically skip the 'Pyromancer' check.\n - Completion of the /XanCave story is required.", true),
		new Option<bool>("Cryomancer", "Cryomancer", "If disabled, the bot will automatically skip the 'Cryomancer' check.", true),
		new Option<bool>("TheCollector", "The Collector", "If disabled, the bot will automatically skip the 'The Collector' check.", true),
		new Option<bool>("ShadowScytheGeneral", "ShadowScythe General", "If disabled, the bot will automatically skip the 'ShadowScythe General' check.", true),
		new Option<bool>("DeathKnightLord", "DeathKnight Lord", "If disabled, the bot will automatically skip the 'DeathKnight Lord' check. \n - Legend-Only.", true),
	};
	public List<IOption> E_Toggle_specific_Priority_Misc_Items = new List<IOption>() {
		// Priority Misc. Items
		new Option<bool>("TreasureChestKeys", "Monthly Treasure Chest Keys", "If disabled, the bot will automatically skip the 'Monthly Treasure Chest Keys' check. \n - Legend-Only.", true),
		new Option<bool>("TheWheelOfDoom", "The Wheel of Doom", "If disabled, the bot will automatically skip the 'The Wheel of Doom' check.", true),
		new Option<BoostEnum>("BoostEnum", "Free Daily Boost", "Select an type of Boost to receive in order to for the 'Free Daily Boost' check.\n - Legend-Only."),
		new Option<bool>("Ballyhoo", "Ballyhoo's Ad Rewards", "If disabled, the bot will automatically skip the 'Ballyhoo's Ad Rewards' check.", true),
		new Option<bool>("EldersBlood", "Void Highlord (Elders' Blood)", "If disabled, the bot will automatically skip the 'Void Highlord (Elders' Blood)' check.", true),
		new Option<bool>("DrakathArmor", "Drakath's Armour", "If disabled, the bot will automatically skip the 'Drakath's Armor' check.", true),
		new Option<MineCraftingEnum>("MineCrafting", "Mine Crafting Ores", "Select an type of Ore to farm in order to enable the 'Mine Crafting Ores' check."),
		new Option<HardCoreMetalsEnum>("HardCoreMetals", "Hard Core Metals", "Select an type of Metal to farm in order to enable the 'Hard Core Metals' check.\n - Legend-Only."),
		new Option<bool>("ArmorOfAwe", "Armor of Awe (Pauldron Relic)", "If disabled, the bot will automatically skip the 'Armor of Awe (Pauldron Relic)' check.\n - Legend-Only.", true)
	};	
	public List<IOption> F_Toggle_specific_Cosmetics = new List<IOption>() {
		// Cosmetics
		new Option<bool>("MadWeaponsmith", "Mad Weaponsmith", "If disabled, the bot will automatically skip the 'Mad Weaponsmith' check.", true),
		new Option<bool>("SUPERHammer", "Cysero's SUPER Hammer", "If disabled, the bot will automatically skip the 'Cysero's SUPER Hammer' check.", true),
		new Option<bool>("BrightKnight", "Bright Knight", "If disabled, the bot will automatically skip the 'Bright Knight' check.", true),
		new Option<bool>("GoldenInquisitor", "Golden Inquisitor of Shadowfall", "If disabled, the bot will automatically skip the 'Golden Inquisitor of Shadowfall' check.", true),
		new Option<bool>("MoglinPets", "Twig, Twilly and Zorbak Pets", "If disabled, the bot will automatically skip the 'Twig, Twilly & Zorbak Pets' check.", true)
	};
	public List<IOption> G_Toggle_specific_Misc_Items = new List<IOption>() {
		// Misc. Items
		new Option<bool>("CryptoToken", "Crypto Tokens", "If disabled, the bot will automatically skip the 'Crypto Tokens' check.", true),
		new Option<bool>("ShadowShroud", "Legion Castle (Shadow Shroud)", "If disabled, the bot will automatically skip the 'Legion Castle (Shadow Shroud)' check.", true),
		new Option<bool>("DesignNotes", "Read the Design Notes!", "If disabled, the bot will automatically skip the 'Read the Design Notes!' check.", true),
		new Option<bool>("PowerGem", "Power Gem", "If disabled, the bot will automatically skip the 'Power Gem' attempt.", true),
		new Option<bool>("GRUMBLE", "GRUMBLE, GRUMBLE...", "If disabled, the bot will automatically skip the 'GRUMBLE, GRUMBLE...' check.\n - Crag & Bamboozle required. ", true)
	};

	public bool DontPreconfigure = true;
	public bool IsMember => ScriptInterface.Instance.Player.IsMember;
	public string[] ItemArray;
	public string[] ItemArrayB;
	public int[] QuantityArray;
	public int[] QuestArray;
	public int SpaceNeeded = 12;
	public int MapNumber = PrivateRoomNumber;
	public bool DisableHunt = false;

	public int FarmLoop;
	public int SavedState;
	public ScriptInterface bot => ScriptInterface.Instance;
	public void ScriptMain(ScriptInterface bot)
	{
		VersionChecker("3.6.3.2");
		if (!DisableMenu) bot.Config.Configure();
		DisableHunt = bot.Config.Get<bool>("B_General_Options", "DisableHunt");


		if (bot.Player.Cell != "Wait") bot.Player.Jump("Wait", "Spawn");

		ConfigureBotOptions();
		ConfigureLiteSettings(CustomDrops: true);

		DeathHandler();

		while (!bot.ShouldExit()) {
			while (!bot.Player.Loaded) { }
			bot.Player.OpenBank();
			bot.Wait.ForBankLoad();

			FormatLog(Text: "Script Started", Title: true);
			
		/// Classes
			if (!bot.Config.Get<bool>("C_Toggle_entire_Catagories", "DisableClasses")) {
				FormatLog(Text: "Classes", Title: true);

			// Lord of Order
				if (bot.Config.Get<bool>("D_Toggle_specific_Classes", "LordOfOrder")) {
					FormatLog(Text: "Lord of Order", Title: true);
					if (CheckStorage("Lord of Order"))
						FormatLog("DailyCheck", "You already own [Lord of Order] x1", Tabs: 1);
				// The Final Challenge (7165)
					else if (bot.Quests.IsUnlocked(7165)) {
						if (bot.Quests.IsDailyComplete(7165))
							FormatLog("DailyCheck", "Daily Quest unavailable", Tabs: 1);
						else {
							FormatLog(Text: "The Final Challenge", Title: true);
							ItemArray = new[] {
								"Lord Of Order", 
								"Champion of Chaos Confronted"
							};
							UnbankList(ItemArray);
							GetDropList(ItemArray);
						// Champion of Chaos Confronted
							SoloMode();
							FormatLog("Farming", "[Champion of Chaos Confronted] x1 from [Champion of Chaos]");
							ItemFarm(
								"Champion of Chaos Confronted", 1,
								Temporary: false,
								HuntFor: !DisableHunt,
								QuestID: 7165,
								MonsterName: "Champion of Chaos",
								MapName: "ultradrakath",
								CellName: "r1",
								PadName: "Left"
							);
							SafeQuestComplete(7165);
							BankArray(ItemArray);
						}
					}

				// Blessing of Order (7164)
					else if (bot.Quests.IsUnlocked(7164)) {
						if (bot.Quests.IsDailyComplete(7164))
							FormatLog("DailyCheck", "Daily Quest unavailable", Tabs: 1);
						else {
							FormatLog(Text: "Blessing of Order", Title: true);
						// You must have completed DoomVaultB
							if (!bot.Quests.IsUnlocked(3004))
								FormatLog("Quest", "Completion of the /DoomVaultB story is required");
							else {
								ItemArray = new[] {
									"Lord Of Order Armor", 
									"Weapon Imprint", 
									"Lure of Order", 
									"Quixotic Mana Essence", 
									"Inversion Infusion"
								};
								UnbankList(ItemArray);
								GetDropList(ItemArray);
							// Weapon Imprint
								SoloMode();
								FormatLog("Farming", "[Weapon Imprint] x15 from [Undead Raxgore]");
								FormatLog("WARNING", "Extremely difficult boss");
								FormatLog(Text: "You might need to farm this manually", Followup: true);
								ItemFarm(
									"Weapon Imprint", 15,
									Temporary: false,
									HuntFor: !DisableHunt,
									QuestID: 7164,
									MonsterName: "Undead Raxgore",
									MapName: "doomvaultb",
									CellName: "r16",
									PadName: "Left"
								);
							// Lure of Order
								if (bot.Player.GetFactionRank("Fishing") < 7) {
									while (bot.Player.GetFactionRank("Fishing") < 7) {
										int i = 0;
										while (!bot.Inventory.Contains("Fishing Dynamite", 10)) {
											ItemFarm(
												"Faith's Fi'shtick", 1,
												Temporary: true,
												HuntFor: !DisableHunt,
												QuestID: 1682,
												MonsterName: "Slime",
												MapName: "greenguardwest",
												CellName: "West4",
												PadName: "Right"
											);
											SafeQuestComplete(1682);
										}
										SafeMapJoin("fishing");
										while (bot.Inventory.Contains("Fishing Dynamite", 1)) {
											bot.SendPacket("%xt%zm%FishCast%1%Dynamite%30%");
											bot.Sleep(3500);
											bot.SendPacket("%xt%zm%getFish%1%false%");
											i++;
											FormatLog("Fishing", $"Fished {i} times");
											bot.Sleep(1500);
										}
									}
								}
								SafePurchase(
									"Lure of Order", 1,
									MapName: "greenguardwest",
									ShopID: 363
								);
							// Quixotic Mana Essence
								SoloMode();
								FormatLog("Farming", "[Quixotic Mana Essence] x10 from [Ultra Xiang]");
								FormatLog("WARNING", "Extremely difficult boss");
								FormatLog(Text: "You might need to farm this manually", Followup: true);
								ItemFarm(
									"Quixotic Mana Essence", 10,
									Temporary: false,
									HuntFor: !DisableHunt,
									QuestID: 7164,
									MonsterName: "Ultra Xiang",
									MapName: "mirrorportal",
									CellName: "r6",
									PadName: "Right"
								);
							// Inversion InfusionSoloMode();
								FormatLog("Farming", "[Inversion Infusion] x5 from [Serepthys]");
								ItemFarm(
									"Inversion Infusion", 5,
									Temporary: false,
									HuntFor: !DisableHunt,
									QuestID: 7164,
									MonsterName: "Serepthys",
									MapName: "yasaris",
									CellName: "r2a",
									PadName: "Bottom"
								);
								SafeQuestComplete(7164);
								BankArray(ItemArray);
							}
						}
					}

				// Axiom (7163)
					else if (bot.Quests.IsUnlocked(7163)) {
						if (bot.Quests.IsDailyComplete(7163))
							FormatLog("DailyCheck", "Daily Quest unavailable", Tabs: 1);
						else {
							FormatLog(Text: "Blessing of Order", Title: true);
							ItemArray = new[] {
								"Lord of Order Polearm", 
								"Law of Nature", 
								"Law of Time", 
								"Law of Gravity", 
								"Law of Relativity",
								"Law of Conservation of Energy",
								"Law of Low Drop Rates"
							};
							UnbankList(ItemArray);
							GetDropList(ItemArray);
						// Law of Nature
							SoloMode();
							FormatLog("Farming", "[Law of Nature] x1 from [monster]");
							ItemFarm(
								"Law of Nature", 1,
								Temporary: false,
								HuntFor: !DisableHunt,
								QuestID: 7163,
								MonsterName: "Guardian Spirit",
								MapName: "elfhame",
								CellName: "r7",
								PadName: "Bottom"
							);
						// Law of Time
							FormatLog("Farming", "[Law of Time] x1 from [Kathool]");
							ItemFarm(
								"Law of Time", 1,
								Temporary: false,
								HuntFor: !DisableHunt,
								QuestID: 7163,
								MonsterName: "Kathool",
								MapName: "deepchaos",
								CellName: "Frame4",
								PadName: "Left"
							);
						// Law of Gravity
							FormatLog("Farming", "[Law of Gravity] x1 from [Shadowstone Support]");
							ItemFarm(
								"Law of Gravity", 1,
								Temporary: false,
								HuntFor: !DisableHunt,
								QuestID: 7163,
								MonsterName: "Shadowstone Support",
								MapName: "necrocavern",
								CellName: "r12",
								PadName: "Left"
							);
						// Law of Relativity
							FormatLog("Farming", "[Law of Relativity] x1 from [Reflecteract]");
							ItemFarm(
								"Law of Relativity", 1,
								Temporary: false,
								HuntFor: !DisableHunt,
								QuestID: 7163,
								MonsterName: "Reflecteract",
								MapName: "blackholesun",
								CellName: "r9",
								PadName: "Left"
							);
						// Law of Conservation of Energy
							FormatLog("Farming", "[Law of Conservation of Energy] x1 from [Tonitru]");
							ItemFarm(
								"Law of Conservation of Energy", 1,
								Temporary: false,
								HuntFor: !DisableHunt,
								QuestID: 7163,
								MonsterName: "Tonitru",
								MapName: "thunderfang",
								CellName: "r8",
								PadName: "Top"
							);
						// Law of Low Drop Rates
							FormatLog("Farming", "[Law of Low Drop Rates] x1 from [Red Dragon]");
							ItemFarm(
								"Law of Low Drop Rates", 1000,
								Temporary: false,
								HuntFor: !DisableHunt,
								QuestID: 7163,
								MonsterName: "Red Dragon",
								MapName: "lair",
								CellName: "End",
								PadName: "Right"
							);
							SafeQuestComplete(7163);
							BankArray(ItemArray);
						}
					}
						
				// Ordinance (7162)
					else if (bot.Quests.IsUnlocked(7162)) {
						if (bot.Quests.IsDailyComplete(7162))
							FormatLog("DailyCheck", "Daily Quest unavailable", Tabs: 1);
						else {
							FormatLog(Text: "Ordinance", Title: true);
							ItemArray = new[] {
								"Lord Of Order Blade", 
								"Acolyte's Braille",
								"Suppressed Drows", 
								"Suppressed Undead", 
								"Suppressed Horcs", 
								"Suppressed Weavers", 
								"Strength of Resilience"
							};
							UnbankList(ItemArray);
							GetDropList(ItemArray);
						// Acolyte's Braille
							FarmMode();
							FormatLog("Farming", "[Acolyte's Braille] x1 from [Chaos Healer]");
							ItemFarm(
								"Acolyte's Braille", 1,
								Temporary: false,
								HuntFor: !DisableHunt,
								QuestID: 7162,
								MonsterName: "Chaos Healer",
								MapName: "newfinale",
								CellName: "r1",
								PadName: "Left"
							);
						// Suppressed Drows
							FarmMode();
							FormatLog("Farming", "[Suppressed Drows] x50 from [Drow Assassin|Drow Soldier]");
							ItemFarm(
								"Suppressed Drows", 50,
								Temporary: false,
								HuntFor: !DisableHunt,
								QuestID: 7162,
								MonsterName: "Drow Assassin|Drow Soldier",
								MapName: "wardwarf",
								CellName: "r2",
								PadName: "Left"
							);
						// Suppressed Undead
							FormatLog("Farming", "[Suppressed Undead] x50 from [Skeletal Fire Mage|Undead Mage|Skeleton]");
							ItemFarm(
								"Suppressed Undead", 50,
								Temporary: false,
								HuntFor: !DisableHunt,
								QuestID: 7162,
								MonsterName: "Skeletal Fire Mage|Undead Mage|Skeleton",
								MapName: "warundead",
								CellName: "r2",
								PadName: "Right"
							);
						// Suppressed Horcs
							FormatLog("Farming", "[Suppressed Horcs] x50 from [Horc Warrior]");
							ItemFarm(
								"Suppressed Horcs", 50,
								Temporary: false,
								HuntFor: !DisableHunt,
								QuestID: 7162,
								MonsterName: "Horc Warrior",
								MapName: "warhorc",
								CellName: "r2",
								PadName: "Left"
							);
						// Suppressed Weavers
							FormatLog("Farming", "[Suppressed Weavers] x50 from [Weaver Queen's Hound]");
							ItemFarm(
								"Suppressed Weavers", 50,
								Temporary: false,
								HuntFor: !DisableHunt,
								QuestID: 7162,
								MonsterName: "Weaver Queen's Hound",
								MapName: "weaverwar",
								CellName: "s1",
								PadName: "Right"
							);
						// Strength of Resilience
							SoloMode();
							FormatLog("Farming", "[Strength of Resilience] x1 from [Xyfrag]");
							ItemFarm(
								"Strength of Resilience", 1,
								Temporary: false,
								HuntFor: !DisableHunt,
								QuestID: 7162,
								MonsterName: "Xyfrag",
								MapName: "thevoid",
								CellName: "r12",
								PadName: "Left"
							);
							SafeQuestComplete(7162);
							BankArray(ItemArray);
						}
					}

				// Harmony (7161)
					else if (bot.Quests.IsUnlocked(7161)) {
						if (bot.Quests.IsDailyComplete(7161))
							FormatLog("DailyCheck", "Daily Quest unavailable", Tabs: 1);
						else {
							FormatLog(Text: "Harmony", Title: true);
							ItemArray = new[] {
								"High Lord Of Order Helm",
								"Unity of Life",
								"Harmony of Solace",
								"Teamwork Observed",
								"Scroll of Enchantment"
								};
							UnbankList(ItemArray);
							GetDropList(ItemArray);
						// Unity of Life
							FarmMode();
							FormatLog("Farming", "[Unity of Life] x1 from [Tree of Destiny]");
							ItemFarm(
								"Unity of Life", 1,
								Temporary: false,
								HuntFor: !DisableHunt,
								QuestID: 7161,
								MonsterName: "Tree of Destiny",
								MapName: "elemental",
								CellName: "r4",
								PadName: "Right"
							);
						// Harmony of Solace
							SoloMode();
							FormatLog("Farming", "[Harmony of Solace] x1 from [Faust]");
							ItemFarm(
								"Harmony of Solace", 1,
								Temporary: false,
								HuntFor: !DisableHunt,
								QuestID: 7161,
								MonsterName: "Faust",
								MapName: "orchestra",
								CellName: "R8",
								PadName: "Down"
							);
						// Teamwork Observed
							FarmMode();
							FormatLog("Farming", "[Teamwork Observed] x100 from [Pactagonal Knight]");
							ItemFarm(
								"Teamwork Observed", 100,
								Temporary: false,
								HuntFor: !DisableHunt,
								QuestID: 7161,
								MonsterName: "Pactagonal Knight",
								MapName: "cathedral",
								CellName: "r8",
								PadName: "Bottom"
							);
						// Scroll of Enchantment
							SoloMode();
							FormatLog("Farming", "[Scroll of Enchantment] x15 from [Queen's ArchSage]");
							ItemFarm(
								"Scroll of Enchantment", 1,
								Temporary: false,
								HuntFor: !DisableHunt,
								QuestID: 7161,
								MonsterName: "Queen's ArchSage",
								MapName: "goose",
								CellName: "r12",
								PadName: "Left"
							);
							SafeQuestComplete(7161);
							BankArray(ItemArray);
						}
					}
						
				// Strike of Order (7160)
					else if (bot.Quests.IsUnlocked(7160)) {
						if (bot.Quests.IsDailyComplete(7160))
							FormatLog("DailyCheck", "Daily Quest unavailable", Tabs: 1);
						else {
							FormatLog(Text: "Strike of Order", Title: true);
						// You must have completed Drakonnan
							if (!bot.Quests.IsUnlocked(6319))
								FormatLog("Quest", "Completion of the /Drakonnan story is required");
							else {
								ItemArray = new[] {
									"Lord of Order Horns",
									"Hanzamune Dragon Koi Blade",
									"The Supreme Arcane Staff",
									"Dragonoid of Hours",
									"Safiria's Spirit Orb",
									"Ice Katana"
								};
								UnbankList(ItemArray);
								GetDropList(ItemArray);
							// Hanzamune Dragon Koi Blade
								SoloMode();
								FormatLog("Farming", "[Hanzamune Dragon Koi Blade] x1 from [Kitsune]");
								ItemFarm(
									"Hanzamune Dragon Koi Blade", 1,
									Temporary: false,
									HuntFor: !DisableHunt,
									QuestID: 7160,
									MonsterName: "kitsune",
									MapName: "Kitsune",
									CellName: "Boss",
									PadName: "Left"
								);
							// The Supreme Arcane Staff
								FormatLog("Farming", "[The Supreme Arcane Staff] x1 from [Monster]");
								ItemFarm(
									"The Supreme Arcane Staff", 1,
									Temporary: false,
									HuntFor: !DisableHunt,
									QuestID: 7160,
									MonsterName: "Ledgermayne",
									MapName: "ledgermayne",
									CellName: "Boss",
									PadName: "Right"
								);
							// Dragonoid of Hours
								FormatLog("Farming", "[Dragonoid of Hours] x1 from [Monster]");
								ItemFarm(
									"Dragonoid of Hours", 1,
									Temporary: false,
									HuntFor: !DisableHunt,
									QuestID: 7160,
									MonsterName: "Dragonoid",
									MapName: "mqlesson",
									CellName: "Boss",
									PadName: "Left"
								);
							// Safiria's Spirit Orb
								FormatLog("Obtaining", "[Safiria's Spirit Orb] x1 from Map maxius", Tabs: 1);
								if (bot.Inventory.Contains("Safiria's Spirit Orb")) {
									SafeMapJoin("maxius");
									bot.Wait.ForMapLoad("maxius");
									bot.SendPacket("%xt%zm%getMapItem%10978%5470%");
								}
							// Ice Katana
								FormatLog("Obtaining", "[Ice Katana] x1 from Quest 6319", Tabs: 1);
								if (bot.Inventory.Contains("Ice Katana") && bot.Inventory.GetItemByName("Ice Katana").ID == 43684)
									FormatLog("Pre-Owned", "You already own [Ice Katana] x1");
								else {
									FarmMode();
									ItemFarm(
										"Inferno Heart", 1,
										Temporary: true,
										HuntFor: !DisableHunt,
										QuestID: 6319,
										MonsterName: "Living Fire",
										MapName: "drakonnan",
										CellName: "r3",
										PadName: "Bottom"
									);
									SafeQuestComplete(6319);
								}
								SafeQuestComplete(7160);
								BankArray(ItemArray);
							}
						}
					}
						
				// Steadfast Will (7159)
					else if (bot.Quests.IsUnlocked(7159)) {
						if (bot.Quests.IsDailyComplete(7159))
							FormatLog("DailyCheck", "Daily Quest unavailable", Tabs: 1);
						else {
							FormatLog(Text: "Steadfast Will", Title: true);
						// You must have completed LivingDungeon
							if (!bot.Quests.IsUnlocked(4362))
								FormatLog("Quest", "Completion of the /LivingDungeon story is required");
							else {
								ItemArray = new[] {
									"Lord of Order Helm",
									"Gaiazor's Cornerstone",
									"Dakka's Crystal",
									"Andre's Necklace Fragment",
									"Desolich's Skull"
								};
								UnbankList(ItemArray);
								GetDropList(ItemArray);
							// Gaiazor's Cornerstone
								SoloMode();
								FormatLog("Farming", "[Gaiazor's Cornerstone] x1 from [Gaiazor]");
								ItemFarm(
									"Gaiazor's Cornerstone", 1,
									Temporary: false,
									HuntFor: !DisableHunt,
									QuestID: 7159,
									MonsterName: "Gaiazor",
									MapName: "gaiazor",
									CellName: "r8",
									PadName: "Left"
								);
							// Dakka's Crystal
								FormatLog("Farming", "[Dakka's Crystal] x1 from [Dakka the Dire Dragon]");
								ItemFarm(
									"Dakka's Crystal", 1,
									Temporary: false,
									HuntFor: !DisableHunt,
									QuestID: 7159,
									MonsterName: "Dakka the Dire Dragon",
									MapName: "treetitanbattle",
									CellName: "Enter",
									PadName: "Spawn"
								);
							// Andre's Necklace Fragment
								FormatLog("Farming", "[Andre's Necklace Fragment] x1 from [Giant Necklace]");
								ItemFarm(
									"Andre's Necklace Fragment", 1,
									Temporary: false,
									HuntFor: !DisableHunt,
									QuestID: 7159,
									MonsterName: "Giant Necklace",
									MapName: "andre",
									CellName: "r2",
									PadName: "Left"
								);
							// Desolich's Skull
								FormatLog("Farming", "[Desolich's Skull] x1 from [Desolich]");
								FormatLog("WARNING", "Extremely difficult boss");
								FormatLog(Text: "You might need to farm this manually", Followup: true);
								ItemFarm(
									"Desolich's Skull", 1,
									Temporary: false,
									HuntFor: !DisableHunt,
									QuestID: 7159,
									MonsterName: "Desolich",
									MapName: "desolich",
									CellName: "r3",
									PadName: "Left"
								);
								SafeQuestComplete(7159);
								BankArray(ItemArray);
							}
						}
					}
						
				// Purification of Chaos (7158)
					else if (bot.Quests.IsUnlocked(7158)) {
						if (bot.Quests.IsDailyComplete(7158))
							FormatLog("DailyCheck", "Daily Quest unavailable", Tabs: 1);
						else {
							FormatLog(Text: "Purification of Chaos", Title: true);
							ItemArray = new[] {
								"Lord of Order Double Wings + Wrap",
								"Chaoroot",
								 "The Supreme Arcane Staff", //For 'Strike of Order'
								"Chaotic War Essence",
								"Chaorrupting Particles",
								"Purified Raindrop"
							};
							UnbankList(ItemArray);
							GetDropList(ItemArray);
						// Chaoroot
							SoloMode();
							FormatLog("Farming", "[Chaoroot] x15 from [Ledgermayne]");
							ItemFarm(
								"Chaoroot", 15,
								Temporary: false,
								HuntFor: !DisableHunt,
								QuestID: 7158,
								MonsterName: "Ledgermayne",
								MapName: "ledgermayne",
								CellName: "Boss",
								PadName: "Right"
							);
						// Chaotic War Essence
							FormatLog("Farming", "[Chaotic War Essence] x15 from [Ultra Chaos Warlord]");
							ItemFarm(
								"Chaotic War Essence", 15,
								Temporary: false,
								HuntFor: !DisableHunt,
								QuestID: 7158,
								MonsterName: "Ultra Chaos Warlord",
								MapName: "chaosboss",
								CellName: "r2",
								PadName: "Spawn"
							);
						// Chaorrupting Particles
							FormatLog("Farming", "[Chaorrupting Particles] x15 from [Chaorruption]");
							ItemFarm(
								"Chaorrupting Particles", 15,
								Temporary: false,
								HuntFor: !DisableHunt,
								QuestID: 7158,
								MonsterName: "Chaorruption",
								MapName: "shadowgates",
								CellName: "r13",
								PadName: "Left"
							);
						// Purified Raindrop
							FormatLog("Farming", "[Purified Raindrop] x45 from [Chaos Lord Lionfang]");
							ItemFarm(
								"Purified Raindrop", 45,
								Temporary: false,
								HuntFor: !DisableHunt,
								QuestID: 7158,
								MonsterName: "Chaos Lord Lionfang",
								MapName: "stormtemple",
								CellName: "r16",
								PadName: "Left"
							);
							SafeQuestComplete(7158);
							BankArray(ItemArray);
						}
					}
						
				// Spirit of Justice (7157)
					else if (bot.Quests.IsUnlocked(7157)) {
						if (bot.Quests.IsDailyComplete(7157))
							FormatLog("DailyCheck", "Daily Quest unavailable", Tabs: 1);
						else {
							FormatLog(Text: "Spirit of Justice", Title: true);
							ItemArray = new[] {
								"Lord Of Order Wings",
								"Lord of Order Wings + Wrap",
								"Warden Elfis Detained",
								"Piggy Drake Punished",
								"Mysterious Stranger Foiled",
								"Calico Cobby Crushed"
							};
							UnbankList(ItemArray);
							GetDropList(ItemArray);
						// Warden Elfis Detained
							SoloMode();
							FormatLog("Farming", "[Warden Elfis Detained] x1 from [Warden Elfis]");
							ItemFarm(
								"Warden Elfis Detained", 1,
								Temporary: false,
								HuntFor: !DisableHunt,
								QuestID: 7157,
								MonsterName: "Warden Elfis",
								MapName: "dwarfprison",
								CellName: "Warden",
								PadName: "Right"
							);
						// Piggy Drake Punished
							FormatLog("Farming", "[Piggy Drake Punished] x1 from [Piggy Drake]");
							ItemFarm(
								"Piggy Drake Punished", 1,
								Temporary: false,
								HuntFor: !DisableHunt,
								QuestID: 7157,
								MonsterName: "Piggy Drake",
								MapName: "prison",
								CellName: "Tax",
								PadName: "Right"
							);
						// Mysterious Stranger Foiled
							FormatLog("Farming", "[Mysterious Stranger Foiled] x1 from [Mysterious Stranger]");
							ItemFarm(
								"Mysterious Stranger Foiled", 1,
								Temporary: false,
								HuntFor: false,
								QuestID: 7157,
								MonsterName: "Mysterious Stranger",
								MapName: "mysteriousdungeon",
								CellName: "r19",
								PadName: "Left"
							);
						// Calico Cobby Crushed
							FormatLog("Farming", "[Calico Cobby Crushed] x1 from [Calico Cobby]");
							ItemFarm(
								"Calico Cobby Crushed", 1,
								Temporary: false,
								HuntFor: !DisableHunt,
								QuestID: 7157,
								MonsterName: "Calico Cobby",
								MapName: "dreammaster",
								CellName: "r5a",
								PadName: "Right"
							);
							SafeQuestComplete(7157);
							BankArray(ItemArray);
						}
					}

				// Heart of Servitude (7156)
					else if (bot.Quests.IsUnlocked(7156)) {
						if (bot.Quests.IsDailyComplete(7156))
							FormatLog("DailyCheck", "Daily Quest unavailable", Tabs: 1);
						else {
							FormatLog(Text: "Heart of Servitude", Title: true);
						// You must have completed CitadelRuins
							if (!bot.Quests.IsUnlocked(6182))
								FormatLog("Quest", "Completion of the /CitadelRuins story is required");
							else {
								ItemArray = new[] {
									"Lord of Order Wrap",
									"Lord of Order Bladed Wrap",
									"Pristine Blades of Order",
									"Dreadrock Donation Receipt",
									"Deadmoor Spirits Helped",
									"Mage's Gratitude",
									"Ravenscar's Truth"
								};
								UnbankList(ItemArray);
								GetDropList(ItemArray);
							// Pristine Blades of Order
								SoloMode();
								FormatLog("Farming", "[Pristine Blades of Order] x1 from [Chaorrupted Knight]");
								ItemFarm(
									"Pristine Blades of Order", 1,
									Temporary: false,
									HuntFor: !DisableHunt,
									QuestID: 7156,
									MonsterName: "Chaorrupted Knight",
									MapName: "watchtower",
									CellName: "Frame3",
									PadName: "Left"
								);
							// Dreadrock Donation Receipt
								FormatLog("Purchasing", "[Dreadrock Donation Receipt] x1 from Shop 1221", Tabs: 1);
								SafePurchase(
									"Dreadrock Donation Receipt", 1,
									MapName: "dreadrock",
									ShopID: 1221
								);
							// Deadmoor Spirits Helped
								FormatLog("Farming", "[Deadmoor Spirits Helped] x1 from [Banshee Mallora]");
								ItemFarm(
									"Deadmoor Spirits Helped", 1,
									Temporary: false,
									HuntFor: !DisableHunt,
									QuestID: 7156,
									MonsterName: "Banshee Mallora",
									MapName: "deadmoor",
									CellName: "r30",
									PadName: "left"
								);
							// Mage's Gratitude
								FormatLog("Obtaining", "[Mage's Gratitude] x1 from Quest 6182", Tabs: 1);
								ItemFarm(
									"Enn'tröpy Defeated", 1,
									Temporary: true,
									HuntFor: !DisableHunt,
									QuestID: 6182,
									MonsterName: "Enn'tröpy",
									MapName: "citadelruins",
									CellName: "r11b",
									PadName: "Bottom"
								);
								SafeQuestComplete(6182);
							// Ravenscar's Truth
								FormatLog("Purchasing", "[Ravenscar's Truth] x1 from Shop 614");
								SafePurchase(
									"Ravenscar's Truth", 1,
									MapName: "ravenscar",
									ShopID: 614
								);
								SafeQuestComplete(7156);
								BankArray(ItemArray);
							}
						}
					}
					else FormatLog("DailyCheck", "Daily Quest unavailable", Tabs: 1);
				}

			// Pyromancer - Shurpu Blaze Token
				if (bot.Config.Get<bool>("D_Toggle_specific_Classes", "Pyromancer")) {
					FormatLog(Text: "Pyromancer", Title: true);
					ItemArray = new[] {"Pyromancer", "Shurpu Blaze Token"};
					QuantityArray = new[] {1, 84};
					QuestArray = new[] {2209, 2210};
					if (bot.Quests.IsUnlocked(2157)) {
						if (DailyCheckANY(QuestArray[0])) {
							UnbankList(ItemArray);
							GetDropList(ItemArray);
							SoloMode();
							if (IsMember) {
								FormatLog($"{ItemArray[0]}", "Doing the Legend-Only Daily Quest", Tabs: 1);
								ItemFarm(
									"Guardian Shale", 1, 
									Temporary: true,
									HuntFor: !DisableHunt,
									QuestID: QuestArray[1],
									MonsterName: "Shurpu Ring Guardian",
									MapName: "xancave", 
									CellName: "r11", 
									PadName: "Left"
								);
								SafeQuestComplete(QuestArray[1]);
							}
							else {
								FormatLog($"{ItemArray[0]}", "Doing the Free-Player Daily Quest", Tabs: 1);
								ItemFarm(
									"Guardian Shale", 1, 
									Temporary: true,
									HuntFor: !DisableHunt,
									QuestID: QuestArray[0],
									MonsterName: "Shurpu Ring Guardian",
									MapName: "xancave", 
									CellName: "r11", 
									PadName: "Left"
								);
								SafeQuestComplete(QuestArray[0]);
							}
							bot.Wait.ForPickup(ItemArray[1]);
							bot.Sleep(ScriptDelay);
							FormatLog($"{ItemArray[0]}", $"You now have [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
						}
						BuyGoal(
							MapName: "xancave",
							ShopID: 447
						);
						BankArray(ItemArray);
					}
					else FormatLog("Quest", "Completion of the /XanCave story is required");
				}

			// Cryomancer - Glacera Ice Token
				if (bot.Config.Get<bool>("D_Toggle_specific_Classes", "Cryomancer")) {
					FormatLog(Text: "Cryomancer", Title: true);
					ItemArray = new[] {"Cryomancer", "Glacera Ice Token"};
					QuantityArray = new[] {1, 84};
					QuestArray = new[] {3966, 3965};
					if (DailyCheckANY(QuestArray[0])) {
						UnbankList(ItemArray);
						GetDropList(ItemArray);
						FarmMode();
						if (IsMember) {
							FormatLog($"{ItemArray[0]}", "Doing the Legend-Only Daily Quest", Tabs: 1);
							ItemFarm(
								"Dark Ice", 1, 
								Temporary: true,
								HuntFor: !DisableHunt,
								QuestID: QuestArray[1],
								MonsterName: "Frost Invader",
								MapName: "frozenruins", 
								CellName: "r6", 
								PadName: "Left"
							);
							SafeQuestComplete(QuestArray[1]);
						}
						else {
							FormatLog($"{ItemArray[0]}", "Doing the Free-Player Daily Quest", Tabs: 1);
							ItemFarm(
								"Dark Ice", 1, 
								Temporary: true,
								HuntFor: !DisableHunt,
								QuestID: QuestArray[0],
								MonsterName: "Frost Invader",
								MapName: "frozenruins", 
								CellName: "r6", 
								PadName: "Left"
							);
							SafeQuestComplete(QuestArray[0]);
						}
						bot.Wait.ForPickup(ItemArray[1]);
						bot.Sleep(ScriptDelay);
						FormatLog($"{ItemArray[0]}", $"You now have [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
					}
					BuyGoal(
						MapName: "frozenruins",
						ShopID: 1056
					);
					BankArray(ItemArray);
				}

			// The Collector - Token of Collection
				if (bot.Config.Get<bool>("D_Toggle_specific_Classes", "TheCollector")) {
					FormatLog(Text: "The Collector", Title: true);
					ItemArray = new[] {"The Collector", "Token of Collection"};
					ItemArrayB = new[] {"This Might Be A Token", "This is Definitely a Token", "This Could Be A Token"};
					QuantityArray = new[] {1, 90};
					QuestArray = new[] {1316, 1331, 1332};
					if (DailyCheckANY(QuestArray[0])) {
						FormatLog("Relogin", "To save you from a potential bug");
						Relogin();
						UnbankList(ItemArray.Concat(ItemArrayB).ToArray());
						GetDropList(ItemArray.Concat(ItemArrayB).ToArray());
						FarmMode();
						if (IsMember) {
							FormatLog($"{ItemArray[0]}", "Doing the Free-Player & Legend-Only Daily Quests", Tabs: 1);
							foreach (int Quest in QuestArray) {
								bot.Quests.EnsureAccept(Quest);
							}
							ItemFarm(
								ItemArrayB[1], 2,
								Temporary: false,
								HuntFor: !DisableHunt,
								QuestID: QuestArray[1],
								MonsterName: "Killer Cricket|Carnivorous Cricket",
								MapName: "terrarium",
								CellName: "r2",
								PadName: "Right"
							);
							SafeQuestComplete(QuestArray[1]);
							ItemFarm(
								ItemArrayB[2], 2,
								Temporary: false,
								HuntFor: !DisableHunt,
								QuestID: QuestArray[2],
								MonsterName: "Killer Cricket|Carnivorous Cricket",
								MapName: "terrarium",
								CellName: "r2",
								PadName: "Right"
							);
							SafeQuestComplete(QuestArray[2]);
						}
						else {
							FormatLog($"{ItemArray[0]}", "Doing the Free-Player Daily Quest", Tabs: 1);
							bot.Quests.EnsureAccept(QuestArray[0]);
						}
						ItemFarm(
							ItemArrayB[0], 2,
							Temporary: false,
							HuntFor: !DisableHunt,
							QuestID: QuestArray[0],
							MonsterName: "Killer Cricket|Carnivorous Cricket",
							MapName: "terrarium",
							CellName: "r2",
							PadName: "Right"
						);
						SafeQuestComplete(QuestArray[0]);
						bot.Wait.ForPickup(ItemArray[1]);
						bot.Sleep(ScriptDelay);
						FormatLog($"{ItemArray[0]}", $"You now have [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
					}
					BuyGoal(
						MapName: "collection",
						ShopID: 324
					);
					BankArray(ItemArray);
				}
				
			// ShadowScythe General - Shadow Shield
				if (bot.Config.Get<bool>("D_Toggle_specific_Classes", "ShadowScytheGeneral")) {
					FormatLog(Text: "ShadowScythe General", Title: true);
					ItemArray = new[] {"ShadowScythe General", "Shadow Shield"};
					QuantityArray = new[] {1, 100};
					QuestArray = new[] {3828, 3827};
					if (DailyCheckANY(QuestArray[0])) {
						UnbankList(ItemArray);
						GetDropList(ItemArray);
						FarmMode();
						if (IsMember) {
							FormatLog("ShadowScy Gen.", "Doing the Free-Player & Legend-Only Daily Quests", Tabs: 1);
							foreach (int Quest in QuestArray) {
								bot.Quests.EnsureAccept(Quest);
							}
							ItemFarm(
								"Broken Blade", 1,
								Temporary: true,
								HuntFor: !DisableHunt,
								QuestID: QuestArray[1],
								MonsterName: "Citadel Crusader|Lightguard Caster|Lightguard Paladin",
								MapName: "lightguardwar",
								CellName: "r2",
								PadName: "Right"
							);
							SafeQuestComplete(QuestArray[1]);
						}
						else {
							FormatLog("ShadowScy Gen.", "Doing the Free-Player Daily Quest", Tabs: 1);
							bot.Quests.EnsureAccept(QuestArray[0]);
						}
						ItemFarm(
							"Broken Blade", 1,
							Temporary: true,
							HuntFor: !DisableHunt,
							QuestID: QuestArray[0],
							MonsterName: "Citadel Crusader|Lightguard Caster|Lightguard Paladin",
							MapName: "lightguardwar",
							CellName: "r2",
							PadName: "Right"
						);
						SafeQuestComplete(QuestArray[0]);
						bot.Wait.ForPickup(ItemArray[1]);
						bot.Sleep(ScriptDelay);
						FormatLog("ShadowScy Gen.", $"You now have [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
					}
					BuyGoal(
						MapName: "shadowfall",
						ShopID: 1644
					);
					BankArray(ItemArray);
				}

			//DeathKnight Lord - Shadow Skull
				if (IsMember && bot.Config.Get<bool>("D_Toggle_specific_Classes", "DeathKnightLord")) {
					FormatLog(Text: "DeathKnight Lord", Title: true);
					ItemArray = new[] {"DeathKnight Lord", "Shadow Skull"};
					QuantityArray = new[] {1, 30};
					QuestArray = new[] {492};
					if (DailyCheckANY(QuestArray[0])) {
						UnbankList(ItemArray);
						GetDropList(ItemArray);
						FarmMode();
						FormatLog($"{ItemArray[0]}", "Doing the Legend-Only Daily Quest", Tabs: 1);
						ItemFarm(
							"Shadow Scales", 5,
							Temporary: true,
							HuntFor: !DisableHunt,
							QuestID: QuestArray[0],
							MonsterName: "Shadow Serpent",
							MapName: "bludrut4",
							CellName: "r5",
							PadName: "Right"
						);
						SafeQuestComplete(QuestArray[0]);
						bot.Wait.ForPickup(ItemArray[1]);
						bot.Sleep(ScriptDelay);
						FormatLog($"{ItemArray[0]}", $"You now have [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
					}
					BuyGoal(
						MapName: "bonecastle",
						ShopID: 1242
					);
					BankArray(ItemArray);
				}
			
			}
		
		/// Priority Misc. Items
			if (!bot.Config.Get<bool>("C_Toggle_entire_Catagories", "DisablePrioMisc")) {
				FormatLog(Text: "Priority Misc. Items", Title: true);
				
			// Mothly Treasure Chest Keys - Magic Treasure Chest Key
				if (IsMember && bot.Config.Get<bool>("E_Toggle_specific_Priority_Misc_Items", "TreasureChestKeys")) {
					FormatLog(Text: "Mothly Treasure Chest Keys", Title: true);
					ItemArray = new[] {"Magic Treasure Chest Key"};
					QuestArray = new[] {1239};
					ExitCombat();
					UnbankList(ItemArray);
					GetDropList(ItemArray);
					if (!bot.Quests.IsDailyComplete(QuestArray[0])) {
						FormatLog("Chest Key", "Doing the Legend-Only Monthly Quest", Tabs: 1);
						SafeQuestComplete(QuestArray[0]);
					}
					else FormatLog("MonthlyCheck", "Monthly Quest unavailable", Tabs: 1);
				}

			// The Wheel of Doom
				if (bot.Config.Get<bool>("E_Toggle_specific_Priority_Misc_Items", "TheWheelOfDoom")) {
					FormatLog(Text: "The Wheel of Doom", Title: true);
					ItemArray = new[] {"Gear of Doom"};
					QuestArray = new[] {3075, 3076};
					List<string> PreQuestInv = GetInvNames();
					ExitCombat();
					if (IsMember && bot.Quests.IsDailyComplete(QuestArray[0]) && !CheckStorage(ItemArray[0], 3))
						FormatLog("Wheel of Doom", "Quests unavailable", Tabs: 1);
					if (IsMember && !bot.Quests.IsDailyComplete(QuestArray[0])) {
						if (CheckStorage(ItemArray[0], 3))
							FormatLog("Wheel of Doom", "Doing the Free-Player Weekly and Legend-Only Daily Quest", Tabs: 1);
						else FormatLog("Wheel of Doom", "Doing the Legend-Only Daily Quest", Tabs: 1);
						int PreDaily = bot.Inventory.GetQuantity("Treasure Potion");
						SafeQuestComplete(QuestArray[0]);
						if (bot.Inventory.GetQuantity("Treasure Potion") != PreDaily) {
							FormatLog("Wheel of Doom", $"You received [Treasure Potion] x{bot.Inventory.GetQuantity("Treasure Potion") - PreDaily}", Tabs: 1);
							FormatLog("Wheel of Doom", $"You now have [Treasure Potion] x{bot.Inventory.GetQuantity("Treasure Potion")}", Tabs: 1);
						}
						bot.Wait.ForDrop("*");
						bot.Player.PickupAll(true);
					}
					if (CheckStorage(ItemArray[0], 3)) {
						if (!IsMember)
							FormatLog("Wheel of Doom", "Doing the Free-Player Weekly Quest", Tabs: 1);
						if (bot.Bank.Contains(ItemArray[0]))
							bot.Bank.ToInventory(ItemArray[0]);
						int PreDaily = bot.Inventory.GetQuantity("Treasure Potion");
						SafeQuestComplete(QuestArray[1]);
						if (bot.Inventory.GetQuantity("Treasure Potion") != PreDaily) {
							FormatLog("Wheel of Doom", $"You received [Treasure Potion] x{bot.Inventory.GetQuantity("Treasure Potion") - PreDaily}", Tabs: 1);
							FormatLog("Wheel of Doom", $"You now have [Treasure Potion] x{bot.Inventory.GetQuantity("Treasure Potion")}", Tabs: 1);
						}
						bot.Wait.ForDrop("*");
						bot.Player.PickupAll(true);
					}
					else if (!IsMember)
						FormatLog("Wheel of Doom", "Quests unavailable", Tabs: 1);
					List<string> PostQuestInv = GetInvNames();
					List<string> RemainderInv = PostQuestInv.Except(PreQuestInv).ToList();
					foreach(string Name in RemainderInv)
						FormatLog("Wheel of Doom", $"You received [{Name}] x{bot.Inventory.GetQuantity(Name)}", Tabs: 1);
					BankArray(RemainderInv.ToArray());
				}

			// Free Daily Boost - XP Boost /  REP Boost / GOLD Boost / Class Boost 
				if (IsMember && bot.Config.Get<BoostEnum>("E_Toggle_specific_Priority_Misc_Items", "BoostEnum").ToString() != "Disabled") {
					FormatLog(Text: "Free Daily Boost", Title: true);
					ItemArray = new[] {$"{bot.Config.Get<BoostEnum>("E_Toggle_specific_Priority_Misc_Items", "BoostEnum").ToString().Replace("_", " ")}! (60 min)"};
					QuantityArray = new[] {500};
					QuestArray = new[] {4069};
					if (DailyCheckANY(QuestArray[0])) {
						FormatLog("Daily Boost", "Doing the Legend-Only Daily Quest", Tabs: 1);
						UnbankList(ItemArray);
						GetDropList(ItemArray);
						SafeQuestComplete(QuestArray[0], (int)bot.Config.Get<BoostEnum>("E_Toggle_specific_Priority_Misc_Items", "BoostEnum"));
						bot.Wait.ForPickup(ItemArray[0]);
						bot.Sleep(ScriptDelay);
						FormatLog("Daily Boost", $"You now have [{ItemArray[0]}] x{bot.Inventory.GetQuantity(ItemArray[0])}", Tabs: 1);
					}
					BankArray(ItemArray);
				}

			// Ballyhoo's Ad Rewards - 500 Gold
				if (bot.Config.Get<bool>("E_Toggle_specific_Priority_Misc_Items", "Ballyhoo")) {
					FormatLog(Text: "Ballyhoo's Ad Rewards", Title: true);
					SafeMapJoin("ballyhoo");
					if (bot.GetGameObject<int>("world.myAvatar.objData.iDailyAds") < 3) {
						FormatLog("Ballyhoo", "Obtaining Ad Rewards");
						while (bot.GetGameObject<int>("world.myAvatar.objData.iDailyAds") < 3) {
							int PreDailyGold = bot.Player.Gold;
							int PreDailyAC = bot.GetGameObject<int>("world.myAvatar.objData.intCoins");
							bot.SendPacket("%xt%zm%getAdReward%7070%");
							bot.Sleep(1000);
							if (bot.Player.Gold != PreDailyGold) 
								FormatLog("Ballyhoo", $"You received {bot.Player.Gold - PreDailyGold} Gold");
							if (bot.GetGameObject<int>("world.myAvatar.objData.intCoins") != PreDailyAC) 
								FormatLog("Ballyhoo", $"YOU RECEIVED {bot.GetGameObject<int>("world.myAvatar.objData.intCoins") - PreDailyGold} AC!!!");
						}
					}
					else FormatLog("Ballyhoo", "Max. amount of Ad Rewards already received today");
				}

			// Void Highlord - Elder's Blood
				if (bot.Config.Get<bool>("E_Toggle_specific_Priority_Misc_Items", "EldersBlood")) {
					FormatLog(Text: "Void Highlord (Elders' Blood)", Title: true);
					ItemArray = new[] {"Void Highlord", "Elders' Blood"};
					QuantityArray = new[] {1, 5};
					QuestArray = new[] {802};
					if (DailyCheckANY(QuestArray[0])) {
						FormatLog($"{ItemArray[0]}", "Doing the Daily Quest", Tabs: 1);
						UnbankList(ItemArray);
						GetDropList(ItemArray);
						FarmMode();
						ItemFarm(
							"Slain Gorillaphant", 50,
							Temporary: true,
							HuntFor: !DisableHunt,
							QuestID: QuestArray[0],
							MonsterName: "Gorillaphant",
							MapName: "arcangrove",
							CellName: "Right",
							PadName: "Right"
						);
						SafeQuestComplete(QuestArray[0]);
						bot.Wait.ForPickup(ItemArray[1]);
						bot.Sleep(ScriptDelay);
						FormatLog($"{ItemArray[0]}", $"You now have [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
					}
					ItemArray = new[] {"Elders' Blood"};
					BankArray(ItemArray);
				}

			// Drakath's Armor - Dage's Scroll Fragment
				if  (bot.Config.Get<bool>("E_Toggle_specific_Priority_Misc_Items", "DrakathArmor")) {
					FormatLog(Text: "Drakath's Armor (Dage's Scroll Fragment)", Title: true);
					ItemArray = new[] {"Get Your Original Drakath's Armor", "Dage's Scroll Fragment"};
					QuantityArray = new[] {1, 13};
					QuestArray = new[] {3596};
					if (DailyCheckANY(QuestArray[0])) {
						FormatLog("Drakath's Armor", "Doing the Daily Quest", Tabs: 1);
						UnbankList(ItemArray);
						GetDropList(ItemArray);
						FarmMode();
						ItemFarm(
							"Chaos Power Increased", 6,
							Temporary: true,
							HuntFor: !DisableHunt,
							QuestID: QuestArray[0],
							MonsterName: "*",
							MapName: "mountdoomskull",
							CellName: "b2",
							PadName: "Left"
						);
						SafeQuestComplete(QuestArray[0]);
						bot.Wait.ForPickup(ItemArray[1]);
						bot.Sleep(ScriptDelay);
						FormatLog("Drakath's Armor", $"You now have [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
					}
					BankArray(ItemArray);
				}

			// Mine Crafting Ores - Aluminum / Barium / Gold / Iron / Copper / Silver / Platinum
				if (bot.Config.Get<MineCraftingEnum>("E_Toggle_specific_Priority_Misc_Items", "MineCrafting").ToString() != "Disabled") {
					FormatLog(Text: "Mine Crafting Ores", Title: true);
					ItemArray = new[] {bot.Config.Get<MineCraftingEnum>("E_Toggle_specific_Priority_Misc_Items", "MineCrafting").ToString()};
					ItemArrayB = new[] {"Axe of the Prospector"};
					QuantityArray = new[] {10};
					QuestArray = new[] {2091};
					if (DailyCheckANY(QuestArray[0])) {
						FormatLog("Mine Crafting", "Doing the Daily Quest", Tabs: 1);
						UnbankList(ItemArray.Concat(ItemArrayB).ToArray());
						GetDropList(ItemArray.Concat(ItemArrayB).ToArray());
						FarmMode();
						ItemFarm(
							"Raw Ore", 30,
							Temporary: true,
							HuntFor: !DisableHunt,
							QuestID: QuestArray[0],
							MonsterName: "Balboa",
							MapName: "stalagbite",
							CellName: "Enter",
							PadName: "Spawn"
						);
						ItemFarm(
							"Axe of the Prospector", 1,
							Temporary: false,
							HuntFor: !DisableHunt,
							QuestID: QuestArray[0],
							MonsterName: "Balboa",
							MapName: "stalagbite",
							CellName: "Enter",
							PadName: "Spawn"
						);
						SafeQuestComplete(QuestArray[0], (int)bot.Config.Get<MineCraftingEnum>("E_Toggle_specific_Priority_Misc_Items", "MineCrafting"));
						bot.Wait.ForPickup(ItemArray[0]);
						bot.Sleep(ScriptDelay);
						FormatLog("Mine Crafting", $"You now have [{ItemArray[0]}] x{bot.Inventory.GetQuantity(ItemArray[0])}", Tabs: 1);
					}
					BankArray(ItemArray);
				}

			// Hard Core Metals - Arsenic / Beryllium / Chromium / Palladium / Rhodium / Thorium / Mercury
				if (IsMember && bot.Config.Get<HardCoreMetalsEnum>("E_Toggle_specific_Priority_Misc_Items", "HardCoreMetals").ToString() != "Disabled") {
					FormatLog(Text: "Hard Core Metals", Title: true);
					ItemArray = new[] {bot.Config.Get<HardCoreMetalsEnum>("E_Toggle_specific_Priority_Misc_Items", "HardCoreMetals").ToString()};
					ItemArrayB = new[] {"Axe of the Prospector"};
					QuantityArray = new[] {10};
					QuestArray = new[] {2098};
					if (DailyCheckANY(QuestArray[0])) {
						FormatLog("Hard Core Metals", "Doing the Daily Quest", Tabs: 1);
						UnbankList(ItemArray.Concat(ItemArrayB).ToArray());
						GetDropList(ItemArray.Concat(ItemArrayB).ToArray());
						FarmMode();
						ItemFarm(
							"Raw Ore", 30,
							Temporary: true,
							HuntFor: !DisableHunt,
							QuestID: QuestArray[0],
							MonsterName: "Balboa",
							MapName: "stalagbite",
							CellName: "Enter",
							PadName: "Spawn"
						);
						ItemFarm(
							"Axe of the Prospector", 1,
							Temporary: false,
							HuntFor: !DisableHunt,
							QuestID: QuestArray[0],
							MonsterName: "Balboa",
							MapName: "stalagbite",
							CellName: "Enter",
							PadName: "Spawn"
						);
						SafeQuestComplete(QuestArray[0], (int)bot.Config.Get<HardCoreMetalsEnum>("E_Toggle_specific_Priority_Misc_Items", "HardCoreMetals"));
						bot.Wait.ForPickup(ItemArray[0]);
						bot.Sleep(ScriptDelay);
						FormatLog("Hard Core Metals", $"You now have [{ItemArray[0]}] x{bot.Inventory.GetQuantity(ItemArray[0])}", Tabs: 1);
					}
					BankArray(ItemArray);
				}

			// Armor of Awe - Pauldron Relic - Pauldron Fragment - Pauldron Shard
				if (IsMember && bot.Config.Get<bool>("E_Toggle_specific_Priority_Misc_Items", "ArmorOfAwe")) {
					FormatLog(Text: "Armor of Awe (Pauldron Relic)", Title: true);
					ItemArray = new[] {"Legendary Awe Pass", "Pauldron Relic", "Pauldron Fragment", "Pauldron Shard"};
					if (!CheckStorage("Armor of Awe")) {
						if (!CheckStorage("Pauldron Relic")) {
							if (!CheckStorage("Pauldron Fragment", 15)) {
								if (!bot.Quests.IsDailyComplete(4160)) {
									UnbankList(ItemArray);
									GetDropList(ItemArray);
									SoloMode();
									ItemFarm(
										"Pauldron Shard", 15,
										Temporary: false,
										HuntFor: !DisableHunt,
										QuestID: 4160,
										MonsterName: "Ultra Akriloth",
										MapName: "gravestrike",
										CellName: "r1",
										PadName: "Left"
									);
									SafeQuestComplete(4160);
								}
								else 
									FormatLog("DailyCheck", $"Daily Quest unavailable", Tabs: 1);
							}
							else {
								if (bot.Bank.Contains("Pauldron Fragment"))
									bot.Bank.ToInventory("Pauldron Fragment");
								SafePurchase(
									"Pauldron Relic", 1,
									MapName: "museum",
									ShopID: 1129
								);
							}
						}
						else
							FormatLog("Pauldron Relic", "You already own [Pauldron Relic] x1", Tabs: 1);
					}
					else
						FormatLog("Armor of Awe", "You already own [Armor of Awe] x1", Tabs: 1);
					BankArray(ItemArray);
				}

			}

		/// Cosmetics
			if (!bot.Config.Get<bool>("C_Toggle_entire_Catagories", "DisableCosmetics")) {
				FormatLog(Text: "Cosmetics", Title: true);

			// Mad Weaponsmith - C-Armor Token
				if (bot.Config.Get<bool>("F_Toggle_specific_Cosmetics", "MadWeaponsmith")) {
					FormatLog(Text: "Mad Weaponsmith", Title: true);
					ItemArray = new[] {"Mad Weaponsmith", "C-Armor Token"};
					QuantityArray = new[] {1, 90};
					QuestArray = new[] {4308, 4309};
					if (DailyCheckANY(QuestArray[0])) {
						UnbankList(ItemArray);
						GetDropList(ItemArray);
						SoloMode();
						if (IsMember) {
							FormatLog($"{ItemArray[0]}", "Doing the Free-Player & Legend-Only Daily Quests", Tabs: 1);
							foreach (int Quest in QuestArray) {
								bot.Quests.EnsureAccept(Quest);
							}
							ItemFarm(
								"Unlucky Horseshoe", 1,
								Temporary: true,
								HuntFor: !DisableHunt,
								QuestID: QuestArray[1],
								MonsterName: "Nightmare",
								MapName: "deadmoor",
								CellName: "r5",
								PadName: "Left"
							);
							SafeQuestComplete(QuestArray[1]);
						}
						else {
							FormatLog($"{ItemArray[0]}", "Doing the Free-Player Daily Quest", Tabs: 1);
							bot.Quests.EnsureAccept(QuestArray[0]);
						}
						ItemFarm(
							"Nightmare Fire", 1,
							Temporary: true,
							HuntFor: !DisableHunt,
							QuestID: QuestArray[0],
							MonsterName: "Nightmare",
							MapName: "deadmoor",
							CellName: "r5",
							PadName: "Left"
						);
						SafeQuestComplete(QuestArray[0]);
						bot.Wait.ForPickup(ItemArray[1]);
						FormatLog($"{ItemArray[0]}", $"You now have [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
					}
					BuyGoal(
						MapName: "deadmoor",
						ShopID: 500
					);
					BankArray(ItemArray);
				}

			// Cysero's SUPER Hammer - C-Hammer Token
				if (bot.Config.Get<bool>("F_Toggle_specific_Cosmetics", "SUPERHammer")) {
					FormatLog(Text: "Cysero's SUPER Hammer", Title: true);
					ItemArray = new[] {"Cysero's SUPER Hammer", "C-Hammer Token"};
					ItemArrayB = new[] {"Mad Weaponsmith"};
					QuantityArray = new[] {1, 90};
					QuestArray = new[] {4310, 4311};
					if (CheckStorage(ItemArrayB[0])) {
						if (DailyCheckANY(QuestArray[0])) {
							UnbankList(ItemArray.Concat(ItemArrayB).ToArray());
							GetDropList(ItemArray.Concat(ItemArrayB).ToArray());
							SoloMode();
							if (IsMember) {
								FormatLog("Cysero's Hammer", "Doing the Free-Player & Legend-Only Daily Quests", Tabs: 1);
								foreach (int Quest in QuestArray) {
									bot.Quests.EnsureAccept(Quest);
								}
								ItemFarm(
									"Geist's Pocket Lint", 1,
									Temporary: true,
									HuntFor: !DisableHunt,
									QuestID: QuestArray[1],
									MonsterName: "Geist",
									MapName: "deadmoor",
									CellName: "r13",
									PadName: "Right"
								);
								SafeQuestComplete(QuestArray[1]);
							}
							else {
								FormatLog("Cysero's Hammer", "Doing the Free-Player Daily Quest", Tabs: 1);
								bot.Quests.EnsureAccept(QuestArray[0]);
							}
							ItemFarm(
								"Geist's Chain Link", 1,
								Temporary: true,
								HuntFor: !DisableHunt,
								QuestID: QuestArray[0],
								MonsterName: "Geist",
								MapName: "deadmoor",
								CellName: "r13",
								PadName: "Right"
							);
							SafeQuestComplete(QuestArray[0]);
							bot.Wait.ForPickup(ItemArray[1]);
							bot.Sleep(ScriptDelay);
							FormatLog("Cysero's Hammer", $"You now have [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
						}
						BuyGoal(
							MapName: "deadmoor",
							ShopID: 500
						);
						BankArray(ItemArray);
					}
					else FormatLog("Cysero's Hammer", $"You don't have [{ItemArrayB[0]}] x1", Tabs: 1);
				}

			// Bright Knight 
				if (bot.Config.Get<bool>("F_Toggle_specific_Cosmetics", "BrightKnight")) {
					FormatLog(Text: "Bright Knight", Title: true);
				// Bright Knight - Seal of Light
					ItemArray = new[] {"Bright Knight", "Seal of Light"};
					QuantityArray = new[] {1, 50};
					QuestArray = new[] {3826};
					if (DailyCheckANY(QuestArray[0])) {
						FormatLog($"{ItemArray[0]}", "Doing the Seal of Light Daily Quest", Tabs: 1);
						UnbankList(ItemArray);
						GetDropList(ItemArray);
						SoloMode();
						ItemFarm(
							"Alteon Defeated", 1,
							Temporary: true,
							HuntFor: false,
							QuestID: QuestArray[0],
							MonsterName: "*",
							MapName: "alteonbattle",
							CellName: "Enter",
							PadName: "Spawn"
						);
						SafeQuestComplete(QuestArray[0]);
						bot.Wait.ForPickup(ItemArray[1]);
						bot.Sleep(ScriptDelay);
						FormatLog($"{ItemArray[0]}", $"You now have [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
					}
					BankArray(ItemArray);

				// Bright Knight - Seal of Darkness
					ItemArray = new[] {"Bright Knight", "Seal of Darkness"};
					QuantityArray = new[] {1, 50};
					QuestArray = new[] {3825};
					if (DailyCheckANY(QuestArray[0])) {
						FormatLog($"{ItemArray[0]}", "Doing the Seal of Darkness Daily Quest", Tabs: 1);
						UnbankList(ItemArray);
						GetDropList(ItemArray);
						SoloMode();
						ItemFarm(
							"Sepulchure Defeated", 1,
							Temporary: true,
							HuntFor: false,
							QuestID: QuestArray[0],
							MonsterName: "*",
							MapName: "sepulchurebattle",
							CellName: "Enter",
							PadName: "Spawn"
						);
						SafeQuestComplete(QuestArray[0]);
						bot.Wait.ForPickup(ItemArray[1]);
						FormatLog($"{ItemArray[0]}", $"You now have [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
					}
					BankArray(ItemArray);
				}

			// Golden Inquisitor of Shadowfall
				if (bot.Config.Get<bool>("F_Toggle_specific_Cosmetics", "GoldenInquisitor")) {
					FormatLog(Text: "Golden Inquisitor of Shadowfall", Title: true);
					ItemArray = new[] {
						"Golden Inquisitor of Shadowfall",
						"Gilded Inquisitor's Female Morph",
						"Gilded Inquisitor's Male Morph",
						"Golden Inquisitor's Locks",
						"Golden Inquisitor's Hair",
						"Golden Inquisitor's Helm",
						"Golden Inquisitor's Crested Helm",
						"Golden Inquisitor's Spear",
						"Golden Inquisitor's Blade",
						"Golden Inquisitor's Wrap",
						"Golden Inquisitor's Back Blade",
						"Golden Inquisitor's Back Blade + Wrap"
					};
					QuantityArray = new[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1};
					QuestArray = new[] {491};
					if (DailyCheckALL(QuestArray[0])) {
						UnbankList(ItemArray);
						GetDropList(ItemArray);
						List<string> PreQuestInv = GetInvNames();
						FarmMode();
						ItemFarm(
							"Inquisitor Contract", 7,
							Temporary: true,
							HuntFor: !DisableHunt,
							QuestID: QuestArray[0],
							MonsterName: "Inquisitor Guard",
							MapName: "citadel",
							CellName: "m1",
							PadName: "Up"
						);
						SafeQuestComplete(QuestArray[0]);
						bot.Wait.ForPickup("*");
						List<string> PostQuestInv = GetInvNames();
						List<string> RemainderInv = PostQuestInv.Except(PreQuestInv).ToList();
						foreach(string Name in RemainderInv)
							FormatLog("Golden Inquisitor", $"You received [{Name}] x{bot.Inventory.GetQuantity(Name)}", Tabs: 1);
					}
					BankArray(ItemArray);
				}

			// Twig, Twilly & Zorbak pet - Moglin MEAL
				if (bot.Config.Get<bool>("F_Toggle_specific_Cosmetics", "MoglinPets")) {
					FormatLog(Text: "Twig, Twilly & Zorbak Pets", Title: true);
				// Twig Pet
					if (!CheckStorage("Twig Pet")) {
						FormatLog(Text: "Twig Pet", Title: true);
						ItemArray = new[] {"Twig Pet", "Moglin MEAL"};
						QuantityArray = new[] {1, 30};
						QuestArray = new[] {4159};
						if (DailyCheckANY(QuestArray[0])) {
							FormatLog($"{ItemArray[0]}", "Doing the Daily Quest", Tabs: 1);
							UnbankList(ItemArray);
							GetDropList(ItemArray);
							FarmMode();
							ItemFarm(
								"Frogzard Meat", 3,
								Temporary: true,
								HuntFor: !DisableHunt,
								QuestID: QuestArray[0],
								MonsterName: "Frogzard",
								MapName: "nexus",
								CellName: "Enter",
								PadName: "Spawn"
							);
							SafeQuestComplete(QuestArray[0]);
							bot.Wait.ForPickup(ItemArray[1]);
							bot.Sleep(ScriptDelay);
							FormatLog($"{ItemArray[0]}", $"You now have [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
						}
						BuyGoal(
							MapName: "ariapet",
							ShopID: 1081
						);
						BankArray(ItemArray);
					}
					// Twilly Pet
					else if (!CheckStorage("Twilly Pet")) {
						FormatLog(Text: "Twilly Pet", Title: true);
						ItemArray = new[] {"Twilly Pet", "Moglin MEAL"};
						if (DailyCheckANY(QuestArray[0])) {
							FormatLog($"{ItemArray[0]}", "Doing the Daily Quest", Tabs: 1);
							UnbankList(ItemArray);
							GetDropList(ItemArray);
							FarmMode();
							ItemFarm(
								"Frogzard Meat", 3,
								Temporary: true,
								HuntFor: !DisableHunt,
								QuestID: QuestArray[0],
								MonsterName: "Frogzard",
								MapName: "nexus",
								CellName: "Enter",
								PadName: "Spawn"
							);
							SafeQuestComplete(QuestArray[0]);
							bot.Wait.ForPickup(ItemArray[1]);
							FormatLog($"{ItemArray[0]}", $"You now have [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
						}
						BuyGoal(
							MapName: "ariapet",
							ShopID: 1081
						);
						BankArray(ItemArray);
					}
					// Zorbak Pet
					else if (!CheckStorage("Zorbak Pet")) {
						FormatLog(Text: "Zorbak Pet", Title: true);
						ItemArray = new[] {"Zorbak Pet", "Moglin MEAL"};
						if (DailyCheckANY(QuestArray[0])) {
							FormatLog($"{ItemArray[0]}", "Doing the Daily Quest", Tabs: 1);
							UnbankList(ItemArray);
							GetDropList(ItemArray);
							FarmMode();
							ItemFarm(
								"Frogzard Meat", 3,
								Temporary: true,
								HuntFor: !DisableHunt,
								QuestID: QuestArray[0],
								MonsterName: "Frogzard",
								MapName: "nexus",
								CellName: "Enter",
								PadName: "Spawn"
							);
							SafeQuestComplete(QuestArray[0]);
							bot.Wait.ForPickup(ItemArray[1]);
							FormatLog($"{ItemArray[0]}", $"You now have [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
						}
						BuyGoal(
							MapName: "ariapet",
							ShopID: 1081
						);
						BankArray(ItemArray);
					}
					else FormatLog("Moglin Pets", "You own all Moglin Pets", Tabs: 1);
				}
			
			}

		/// Misc. Items
			if (!bot.Config.Get<bool>("C_Toggle_entire_Catagories", "DisableMisc")) {
				FormatLog(Text: "Misc. Items", Title: true);

			// Crypto Tokens
				if (bot.Config.Get<bool>("G_Toggle_specific_Misc_Items", "CryptoToken")) {
					FormatLog(Text: "Crypto Tokens", Title: true);
					ItemArray = new[] {"Crypto Token"};
					QuantityArray = new[] {300};
					QuestArray = new[] {6187};
					if (DailyCheckANY(QuestArray[0])) {
						UnbankList(ItemArray);
						GetDropList(ItemArray);
						FarmMode();
						ItemFarm(
							"Metal Ore", 1,
							Temporary: true,
							HuntFor: !DisableHunt,
							QuestID: QuestArray[0],
							MonsterName: "Sneevil",
							MapName: "boxes",
							CellName: "Fort1",
							PadName: "Right"
						);
						SafeQuestComplete(QuestArray[0]);
						bot.Wait.ForPickup(ItemArray[0]);
						bot.Sleep(ScriptDelay);
						FormatLog($"{ItemArray[0]}", $"You now have [{ItemArray[0]}] x{bot.Inventory.GetQuantity(ItemArray[0])}", Tabs: 1);
					}
					BankArray(ItemArray);
				}

			// Legion Castle - Shadow Shroud
				if (bot.Config.Get<bool>("G_Toggle_specific_Misc_Items", "ShadowShroud")) {
					FormatLog(Text: "Legion Castle (Shadow Shroud)", Title: true);
					ItemArray = new[] {"Legion Castle","Shadow Shroud"};
					QuantityArray = new[] {1, 15};
					QuestArray = new[] {486};
					if (DailyCheckANY(QuestArray[0])) {
						UnbankList(ItemArray);
						GetDropList(ItemArray);
						FarmMode();
						ItemFarm(
							"Shadow Canvas", 5,
							Temporary: true,
							HuntFor: !DisableHunt,
							QuestID: QuestArray[0],
							MonsterName: "Shadow Creeper",
							MapName: "bludrut2",
							CellName: "r6",
							PadName: "Up"
						);
						SafeQuestComplete(QuestArray[0]);
						bot.Wait.ForPickup(ItemArray[1]);
						bot.Sleep(ScriptDelay);
						FormatLog($"{ItemArray[0]}", $"You now have [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
					}
					BankArray(ItemArray);
				}
			
			// Read the Design Notes!
				if (bot.Config.Get<bool>("G_Toggle_specific_Misc_Items", "DesignNotes")) {
					FormatLog(Text: "Read the Design Notes!", Title: true);
					QuestArray = new[] {1213};
					if (DailyCheckANY(QuestArray[0])) {
						FormatLog("Design Notes", "Doing the Daily Quest", Tabs: 1);
						SafeQuestComplete(QuestArray[0]);
					}
				}

			// Power Gems
				if (bot.Config.Get<bool>("G_Toggle_specific_Misc_Items", "PowerGem")) {
					FormatLog(Text: "Power Gem", Title: true);
					ItemArray = new[] {"Power Gem"};
					QuantityArray = new[] {1000};
					if (!CheckStorage(ItemArray[0], QuantityArray[0])) {
						UnbankList(ItemArray);
						GetDropList(ItemArray);
						int PreDaily = bot.Inventory.GetQuantity(ItemArray[0]);
						bot.SendPacket("%xt%zm%powergem%11%");
						bot.Wait.ForPickup(ItemArray[0]);
						if (bot.Inventory.GetQuantity(ItemArray[0]) != PreDaily) {
							FormatLog($"{ItemArray[0]}", $"You received [{ItemArray[0]}] x{bot.Inventory.GetQuantity(ItemArray[0]) - PreDaily}", Tabs: 1);
							FormatLog($"{ItemArray[0]}", $"You now have [{ItemArray[0]}] x{bot.Inventory.GetQuantity(ItemArray[0])}", Tabs: 1);
						}
						else FormatLog("WeeklyCheck", "Weekly Gem Unavailable", Tabs: 1);
					}
					else FormatLog("WeeklyCheck", $"You already own [{ItemArray[0]}] x{QuantityArray[0]}", Tabs: 1);
					BankArray(ItemArray);
				}

			// GRUMBLE, GRUMBLE...
				if (CheckStorage("Crag &amp; Bamboozle") && bot.Config.Get<bool>("G_Toggle_specific_Misc_Items", "GRUMBLE")) {
					FormatLog(Text: "GRUMBLE, GRUMBLE...", Title: true);
					ItemArray = new[] {"Diamond of Nulgath", "Blood Gem of the Archfiend"};
					ItemArrayB = new[] {"Crag &amp; Bamboozle"};
					QuantityArray = new[] {1000, 100};
					QuestArray = new[] {592};
					if (DailyCheckANY(QuestArray[0])) {
						UnbankList(ItemArray.Concat(ItemArrayB).ToArray());
						GetDropList(ItemArray.Concat(ItemArrayB).ToArray());
						int PreDaily0 = bot.Inventory.GetQuantity("Diamond of Nulgath");
						int PreDaily1 = bot.Inventory.GetQuantity("Blood Gem of the Archfiend");
						SafeQuestComplete(QuestArray[0]);
						bot.Wait.ForPickup(ItemArray[0]);
						bot.Sleep(ScriptDelay);
						if (bot.Inventory.GetQuantity(ItemArray[0]) != PreDaily0) {
							FormatLog("GRUMBLE", $"You received [{ItemArray[0]}] x{bot.Inventory.GetQuantity(ItemArray[0]) - PreDaily0}", Tabs: 1);
							FormatLog("GRUMBLE", $"You now have [{ItemArray[0]}] x{bot.Inventory.GetQuantity(ItemArray[0])}", Tabs: 1);
						}
						if (bot.Inventory.GetQuantity(ItemArray[1]) != PreDaily1) {
							FormatLog("GRUMBLE", $"You received [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1]) - PreDaily1}", Tabs: 1);
							FormatLog("GRUMBLE", $"You now have [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
						}
					}
					BankArray(ItemArray.Concat(ItemArrayB).ToArray());
				}

			}
			
			StopBot("All (selected) Dailies complete. All materials banked.");
		}
	}

	/*------------------------------------------------------------------------------------------------------------
												 	To-Do
	------------------------------------------------------------------------------------------------------------*/
	//These options are gonna be added at some point in the future. If you have any suggestions for what else should be on this list -
	//message me on discord: Lord Exelot#9674

	/*
		-		Classes

		-		Cosmetics

		-		Priority Misc. Farm

		-		Misc. Farm

		-		QoL

		-		Known glitches
	
	*/

	/*------------------------------------------------------------------------------------------------------------
												 	Daily Quest Template
	------------------------------------------------------------------------------------------------------------*/

	/*
				if (bot.Config.Get<bool>("Catagory", "String")) {
					FormatLog(Text: "String", Title: true);
					ItemArray = new[] {"String", "String"};
					QuantityArray = new[] {0, 0};
					QuestArray = new[] {0000, 0000};
					if (DailyCheckANY(QuestArray[0])) {
						UnbankList(ItemArray);
						GetDropList(ItemArray);
						Farm/SoloMode();
						ItemFarm(
							"String", 0,
							Temporary: true,
							HuntFor: !DisableHunt,
							QuestID: QuestArray[0],
							MonsterName: "*",
							MapName: "battleon",
							CellName: "Enter",
							PadName: "Spawn"
						);
						SafeQuestComplete(QuestArray[0]);
						bot.Wait.ForPickup(ItemArray[1]);
						FormatLog($"{ItemArray[0]}", $"You now have [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
					}
					BuyGoal(
						MapName: "String",
						ShopID: 000
					);
					BankArray(ItemArray);
				}
	*/

	/*------------------------------------------------------------------------------------------------------------
													 Specific Functions
	------------------------------------------------------------------------------------------------------------*/
	//These functions are specific to this bot.

	public void SoloMode()
	{
		EquipList(SoloingGear);
		SkillList(SoloingSkillOrder);
		if (!bot.Config.Get<bool>("B_General_Options", "PrivateOnly"))
			MapNumber = 1;
	}
	public void FarmMode()
	{
		EquipList(FarmingGear);
		SkillList(FarmingSkillOrder);
		MapNumber = PrivateRoomNumber;
	}

	public enum MineCraftingEnum
	{
		Disabled,
		Aluminum = 11608,
		Barium = 11932,
		Gold = 12157,
		Iron = 12263,
		Copper = 12297,
		Silver = 12308,
		Platinum = 12315
	}
	public enum HardCoreMetalsEnum
	{
		Disabled,
		Arsenic = 11287,
		Beryllium = 11534,
		Chromium = 11591,
		Palladium = 11864,
		Rhodium = 12032,
		Thorium = 12075,
		Mercury = 12122
	}
	public enum BoostEnum
	{
		Disabled,
		XP_Boost = 27552,
		REP_Boost = 27553,
		GOLD_Boost = 27554,
		Class_Boost = 27555
	}

	public List<string> GetInvNames() {
		List<string> ItemNamedList = new List<string>();
		foreach(InventoryItem Item in bot.Inventory.Items)
			ItemNamedList.Add(Item.Name);
		return ItemNamedList;
	}

	public void BuyGoal(string MapName, int ShopID) 
	{
		if (bot.Config.Get<bool>("B_General_Options", "buyGoal")) {
			if (!CheckStorage(ItemArray[0], QuantityArray[0]) && CheckStorage(ItemArray[1], QuantityArray[1])) {
				UnbankList(ItemArray);
				SafePurchase(
					ItemArray[0], 
					QuantityArray[0], 
					MapName: MapName, 
					ShopID: ShopID
				);
			}
		}
	}

	public bool DailyCheckANY (int QuestID)
	{
		ExitCombat();
		if (!bot.Config.Get<bool>("B_General_Options", "DisableQuantity")) {
			foreach (string Item in ItemArray)
			{
				int QuantityIndex = Array.IndexOf(ItemArray, Item);
				if (CheckStorage(Item, QuantityArray[QuantityIndex])) {
					FormatLog("DailyCheck", $"You already own [{Item}] x{QuantityArray[QuantityIndex]}", Tabs: 1);
					return false;
				}
			} 
		}
		if (bot.Quests.IsDailyComplete(QuestID) || !bot.Quests.IsUnlocked(QuestID)) {
			FormatLog("DailyCheck", "Daily Quest unavailable", Tabs: 1);
			return false;
		}
		return true;
	}
	public bool DailyCheckALL (int QuestID)
	{
		ExitCombat();
		if (!bot.Config.Get<bool>("B_General_Options", "DisableQuantity")) {
			int count = 0;
			foreach (string Item in ItemArray)
			{
				int QuantityIndex = Array.IndexOf(ItemArray, Item);
				if (CheckStorage(Item, QuantityArray[QuantityIndex]))
					count++;
			}
			if (count == ItemArray.Length) {
				FormatLog("DailyCheck", $"You already own these [{ItemArray}]", Tabs: 1);
				return false;
			}
		}
		if (bot.Quests.IsDailyComplete(QuestID) || !bot.Quests.IsUnlocked(QuestID)) {
			FormatLog("DailyCheck", "Daily Quest unavailable", Tabs: 1);
			return false;
		}
		return true;
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

	public void Relogin()
	{
		bool autoRelogSwitch = bot.Options.AutoRelogin;
		bot.Options.AutoRelogin = false;
		bot.Sleep(2000);
		FormatLog("Relogin", "Started");
		bot.Player.Logout();
		bot.Sleep(5000);
		RBot.Servers.Server server = bot.Options.AutoReloginAny 
				? RBot.Servers.ServerList.Servers.Find(x => x.IP != RBot.Servers.ServerList.LastServerIP) 
				: RBot.Servers.ServerList.Servers.Find(s => s.IP == RBot.Servers.ServerList.LastServerIP) ?? RBot.Servers.ServerList.Servers[0];
		bot.Player.Login(bot.Player.Username, bot.Player.Password);
		bot.Player.Connect(server);
		while(!bot.Player.LoggedIn)
			bot.Sleep(500);
		bot.Sleep(5000);
		FormatLog("Relogin", "Finished");
		bot.Options.AutoRelogin = autoRelogSwitch;
	}

	public void VersionChecker(string TargetVersion)
	{
		int[] TargetVArray = Array.ConvertAll(TargetVersion.Split('.'), int.Parse);
		int[] CurrentVArray = Array.ConvertAll(Forms.Main.Text.Replace("RBot ", "").Split('.'), int.Parse);
		foreach (int Digit in TargetVArray) {
			int Index = Array.IndexOf(TargetVArray, Digit);
			if (Digit > CurrentVArray[Index]) {
				MessageBox.Show($"This script requires RBot {TargetVersion} or above. Stopping the script", "WARNING");
				ScriptManager.StopScript();
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
	}

	/// <summary>
	/// Stops the bot at yulgar if no parameters are set, or your specified map if the parameters are set.
	/// </summary>
	public void StopBot(string Text = "Bot stopped successfully.", string MapName = "yulgar", string CellName = "Enter", string PadName = "Spawn", string Caption = "Stopped", string MessageType = "event")
	{
		//Must have the following functions in your script:
		//SafeMapJoin
		//ExitCombat
		FormatLog(Title: true, Text: "Stopping Script");
		if (bot.Map.Name != MapName.ToLower()) SafeMapJoin(MapName.ToLower(), CellName, PadName);
		if (bot.Player.Cell != CellName) bot.Player.Jump(CellName, PadName);
		bot.Drops.RejectElse = false;
		bot.Options.LagKiller = false;
		bot.Options.AggroMonsters = false;
		FormatLog("Script", "Script Stopped");
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
		bot.Wait.ForCombatExit();
		bot.Sleep(ScriptDelay);
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
		if(bot.Handlers.Any(h => h.Name == "Skill Handler"))
			bot.Handlers.RemoveAll(h => h.Name == "Skill Handler");
		bot.RegisterHandler(1, b => {
			if (bot.Player.InCombat)
			{
				foreach (var Skill in Skillset)
				{
					bot.Player.UseSkill(Skill);
				}
			}
		}, "Skill Handler");
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
	public void CheckSpace()
	{
		int MaxSpace = bot.GetGameObject<int>("world.myAvatar.objData.iBagSlots");
		int FilledSpace = bot.GetGameObject<int>("world.myAvatar.items.length");
		int EmptySpace = MaxSpace - FilledSpace;

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