using System;
using System.Collections.Generic;
using System.Linq;
using RBot;
using RBot.Options;
using System.Windows.Forms;

public class SmartDailies 
{	

	/*
																			READ ME!
		Hi and thanks for helping BETA test this bot. As far as I am aware it's quite stable and it is already a improvement over the old '[Farm] Daily Quest (All).cs'
		The reason I am placing this in open BETA is to give you guys the chance to use the bot whilst its still being developed, and if you run into an issue I will be able to patch them ofcourse.
		My appologies for having the password in there, I just want to make sure people actually read this bit first so that they know that its still in active development and that things are subject to change.
		I am working my butt off to make this bot as smart and as easy to understand as possible, password = "harbor", so if you have any suggestions, let me know. 
		Also, yes I know its cheeky to hide the password in here. A bit lower in the script you can see my todo list, if you have suggestoins, let me know.
		One last thing, I try to test the script a lot, but I cant promise there wont be bugs. Its a BETA after all. 
		It's especially hard to test this bot considering the fact that I can only test it once per day fully, per account.
		Either way, just place the password that is somewhere in this READ ME in the public string below here, ofcourse do remove the bit that says Password first :P
		I hope the bot serves you well thus far, cheers, good luck and have fun. ~ Lord Exelot

		You can contact me with any questions or suggestions via discord at Lord Exelot#9674
	*/
	public string Password = "Password";

	//-----------EDIT BELOW-------------//
	public static int PrivateRoomNumber = 999999;
	public int SaveStateLoops = 8700;
	public int TurnInAttempts = 10;
	public int ExitCombatTimer = 700;
	public string[] SoloingGear = {"LightCaster"};
	public string[] FarmingGear = {"Vampire Lord"};
	public readonly int[] SoloingSkillOrder = { 3, 1, 2, 4 };
	public readonly int[] FarmingSkillOrder = { 4, 2, 3, 1 };


	// If you dont want the popup menu that comes up, set DisableMenu to true		
	// Warning: It wont do any Dailies that require you to select your prefered reward in the menu
	public static bool DisableMenu = false;

	//-----------EDIT ABOVE-------------//


	public string OptionsStorage = "DailyOptionStorage";
	public List<IOption> Options = new List<IOption>() {
		new Option<string>("-", "Close this window in order to start the bot", " ", " "),
		new Option<string>("-", " ", " ", " "),
		new Option<bool>("buyGoal", "Buy the Final Product", "If enabled, the bot will automatically buy the final product (when available) of the things it farmed for.", true),
		new Option<bool>("DisableQuantity", "Disable Quantity checks", "If enabled, the bot will continue to farm items even if the goal has already been reached.", false),
		new Option<bool>("PrivateOnly", "Make all rooms private", "If enabled, the bot will make all rooms you go to private. If disabled, it will swap depending on if it's on Farm or Solo mode for that Daily Quest", false),
		new Option<bool>("DisableHunt", "Disable Hunting mode", "Highly recommended to leave this off. Only turn this on if you are having issues with RBot's Hunting feature.", false),

		// Catagories
		new Option<string>("-", " ", " ", " "),
		new Option<string>("-", "Enable/Disable entire catagories", " ", " "),
		new Option<bool>("DisableClasses", "    Disable all Classes", "If enabled, the bot will automatically skip the entire 'Classes' section.", false),
		new Option<bool>("DisableCosmetics", "    Disable all Cosmetics ", "If enabled, the bot will automatically skip the entire 'Cosmetics' section.", false),
		new Option<bool>("DisablePrioMisc", "    Disable all Priority Misc Items", "If enabled, the bot will automatically skip the entire 'Priority Misc. Items' section.", false),
		new Option<bool>("DisableMisc", "    Disable all non-Priority Misc Items", "If enabled, the bot will automatically skip the entire 'Misc. Items' section.", false),

		// Classes
		new Option<string>("-", " ", " ", " "),
		new Option<string>("-", "Enable/Disable specific Classes", " ", " "),
		new Option<bool>("Pyromancer", "    Pyromancer", "If disabled, the bot will automatically skip the 'Pyromancer' check.", true),
		new Option<bool>("Cryomancer", "    Cryomancer", "If disabled, the bot will automatically skip the 'Cryomancer' check.", true),
		new Option<bool>("TheCollector", "    The Collector", "If disabled, the bot will automatically skip the 'The Collector' check.", true),
		new Option<bool>("ShadowScytheGeneral", "    ShadowScythe General", "If disabled, the bot will automatically skip the 'ShadowScythe General' check.", true),
		new Option<bool>("DeathKnightLord", "    DeathKnight Lord", "If disabled, the bot will automatically skip the 'DeathKnight Lord' check.", true),

		// Cosmetics
		new Option<string>("-", " ", " ", " "),
		new Option<string>("-", "Enable/Disable specific Cosmetics", " ", " "),
		new Option<bool>("MadWeaponsmith", "    Mad Weaponsmith", "If disabled, the bot will automatically skip the 'Mad Weaponsmith' check.", true),
		new Option<bool>("SUPERHammer", "    Cysero's SUPER Hammer", "If disabled, the bot will automatically skip the 'Cysero's SUPER Hammer' check.", true),
		new Option<bool>("BrightKnight", "    Bright Knight", "If disabled, the bot will automatically skip the 'Bright Knight' check.", true),
		new Option<bool>("MoglinPets", "    Twig, Twilly and Zorbak Pets", "If disabled, the bot will automatically skip the 'Twig, Twilly & Zorbak Pets' check.", true),

		// Priority Misc. Items
		new Option<string>("-", " ", " ", " "),
		new Option<string>("-", "Enable/Disable specific Priority Misc. Items", " ", " "),
		new Option<bool>("TreasureChestKeys", "    Monthly Treasure Chest Keys", "If disabled, the bot will automatically skip the 'Monthly Treasure Chest Keys' check.", true),
		new Option<bool>("TheWheelOfDoom", "    The Wheel of Doom", "If disabled, the bot will automatically skip the 'The Wheel of Doom' check.", true),
		new Option<BoostEnum>("BoostEnum", "    Free Daily Boost", "Select an type of Boost to receive in order to for the 'Free Daily Boost' check"),
		new Option<bool>("Ballyhoo", "    Ballyhoo's Ad Rewards", "If disabled, the bot will automatically skip the 'Ballyhoo's Ad Rewards' check.", true),
		new Option<bool>("EldersBlood", "    Void Highlord (Elders' Blood)", "If disabled, the bot will automatically skip the 'Void Highlord (Elders' Blood)' check.", true),
		new Option<bool>("DrakathArmor", "    Drakath's Armour", "If disabled, the bot will automatically skip the 'Drakath's Armor' check.", true),
		new Option<MineCraftingEnum>("MineCrafting", "    Mine Crafting Ores", "Select an type of Ore to farm in order to enable the 'Mine Crafting Ores' check"),
		new Option<HardCoreMetalsEnum>("HardCoreMetals", "    Hard Core Metals", "Select an type of Metal to farm in order to enable the 'Hard Core Metals' check."),
		new Option<bool>("ArmorOfAwe", "    Armor of Awe (Pauldron of Awe)", "If disabled, the bot will automatically skip the 'Armor of Awe (Pauldron of Awe)' check.", true),
		
		// Misc. Items
		new Option<string>("-", " ", " ", " "),
		new Option<string>("-", "Enable/Disable Misc. Items", " ", " "),
		new Option<bool>("CryptoToken", "    Crypto Tokens", "If disabled, the bot will automatically skip the 'Crypto Tokens' check.", true),
		new Option<bool>("ShadowShroud", "    Legion Castle (Shadow Shroud)", "If disabled, the bot will automatically skip the 'Legion Castle (Shadow Shroud)' check.", true),
		new Option<bool>("GRUMBLE", "    GRUMBLE, GRUMBLE...", "If disabled, the bot will automatically skip the 'GRUMBLE, GRUMBLE...' check.", true)
	};

	public bool DontPreconfigure = true;
	public bool IsMember => ScriptInterface.Instance.Player.IsMember;
	public string[] ItemArray;
	public string[] ItemArrayB;
	public int[] QuantityArray;
	public int[] QuestArray;
	public int SpaceNeeded = 0;
	public int MapNumber = PrivateRoomNumber;

	public int FarmLoop;
	public int SavedState;
	public ScriptInterface bot => ScriptInterface.Instance;
	public void ScriptMain(ScriptInterface bot)	
	{

		if (Password != "harbor") {
			MessageBox.Show("Please read the READ-ME at the start of the script and place the password before starting this bot.", "Password Incorrect!");
			ScriptManager.StopScript();
		}

		if (!DisableMenu) bot.Config.Configure();

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
			if (!bot.Config.Get<bool>("DisableClasses")) {
				FormatLog(Text: "Classes", Title: true);

				// Pyromancer - Shurpu Blaze Token
				if (bot.Config.Get<bool>("Pyromancer")) {
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
									HuntFor: !bot.Config.Get<bool>("DisableHunt"),
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
									HuntFor: !bot.Config.Get<bool>("DisableHunt"),
									QuestID: QuestArray[0],
									MonsterName: "Shurpu Ring Guardian",
									MapName: "xancave", 
									CellName: "r11", 
									PadName: "Left"
								);
								SafeQuestComplete(QuestArray[0]);
							}
							bot.Wait.ForPickup(ItemArray[1]);
							FormatLog($"{ItemArray[0]}", $"You now own [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
						}
						BuyGoal(
							MapName: "xancave",
							ShopID: 447
						);
						BankArray(ItemArray);
					}
				}

				// Cryomancer - Glacera Ice Token
				if (bot.Config.Get<bool>("Cryomancer")) {
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
								HuntFor: !bot.Config.Get<bool>("DisableHunt"),
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
								HuntFor: !bot.Config.Get<bool>("DisableHunt"),
								QuestID: QuestArray[0],
								MonsterName: "Frost Invader",
								MapName: "frozenruins", 
								CellName: "r6", 
								PadName: "Left"
							);
							SafeQuestComplete(QuestArray[0]);
						}
						bot.Wait.ForPickup(ItemArray[1]);
						FormatLog($"{ItemArray[0]}", $"You now own [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
					}
					BuyGoal(
						MapName: "frozenruins",
						ShopID: 1056
					);
					BankArray(ItemArray);
				}

				// The Collector - Token of Collection
				if (bot.Config.Get<bool>("TheCollector")) {
					FormatLog(Text: "The Collector", Title: true);
					ItemArray = new[] {"The Collector", "Token of Collection"};
					ItemArrayB = new[] {"This Might Be A Token", "This is Definitely a Token", "This Could Be A Token"};
					QuantityArray = new[] {1, 90};
					QuestArray = new[] {1316, 1331, 1332};
					if (DailyCheckANY(QuestArray[0])) {
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
								HuntFor: !bot.Config.Get<bool>("DisableHunt"),
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
								HuntFor: !bot.Config.Get<bool>("DisableHunt"),
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
							HuntFor: !bot.Config.Get<bool>("DisableHunt"),
							QuestID: QuestArray[0],
							MonsterName: "Killer Cricket|Carnivorous Cricket",
							MapName: "terrarium",
							CellName: "r2",
							PadName: "Right"
						);
						SafeQuestComplete(QuestArray[0]);
						bot.Wait.ForPickup(ItemArray[1]);
						FormatLog($"{ItemArray[0]}", $"You now own [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
					}
					BuyGoal(
						MapName: "collection",
						ShopID: 324
					);
					BankArray(ItemArray);
				}
				
				// ShadowScythe General - Shadow Shield
				if (bot.Config.Get<bool>("ShadowScytheGeneral")) {
					FormatLog(Text: "ShadowScythe General", Title: true);
					ItemArray = new[] {"ShadowScythe General", "Shadow Shield"};
					QuantityArray = new[] {1, 50};
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
								HuntFor: !bot.Config.Get<bool>("DisableHunt"),
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
							HuntFor: !bot.Config.Get<bool>("DisableHunt"),
							QuestID: QuestArray[0],
							MonsterName: "Citadel Crusader|Lightguard Caster|Lightguard Paladin",
							MapName: "lightguardwar",
							CellName: "r2",
							PadName: "Right"
						);
						SafeQuestComplete(QuestArray[0]);
						bot.Wait.ForPickup(ItemArray[1]);
						FormatLog("ShadowScy Gen.", $"You now own [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
					}
					BuyGoal(
						MapName: "shadowfall",
						ShopID: 1644
					);
					BankArray(ItemArray);
				}

				//DeathKnight Lord - Shadow Skull
				if (IsMember && bot.Config.Get<bool>("DeathKnightLord")) {
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
							HuntFor: !bot.Config.Get<bool>("DisableHunt"),
							QuestID: QuestArray[0],
							MonsterName: "Shadow Serpent",
							MapName: "bludrut4",
							CellName: "r5",
							PadName: "Right"
						);
						SafeQuestComplete(QuestArray[0]);
						bot.Wait.ForPickup(ItemArray[1]);
						FormatLog($"{ItemArray[0]}", $"You now own [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
					}
					BuyGoal(
						MapName: "bonecastle",
						ShopID: 1242
					);
					BankArray(ItemArray);
				}
			}

			/// Cosmetics
			if (!bot.Config.Get<bool>("DisableCosmetics")) {
				FormatLog(Text: "Cosmetics", Title: true);

				// Mad Weaponsmith - C-Armor Token
				if (bot.Config.Get<bool>("MadWeaponsmith")) {
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
								HuntFor: !bot.Config.Get<bool>("DisableHunt"),
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
							HuntFor: !bot.Config.Get<bool>("DisableHunt"),
							QuestID: QuestArray[0],
							MonsterName: "Nightmare",
							MapName: "deadmoor",
							CellName: "r5",
							PadName: "Left"
						);
						SafeQuestComplete(QuestArray[0]);
						bot.Wait.ForPickup(ItemArray[1]);
						FormatLog($"{ItemArray[0]}", $"You now own [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
					}
					BuyGoal(
						MapName: "deadmoor",
						ShopID: 500
					);
					BankArray(ItemArray);
				}

				// Cysero's SUPER Hammer - C-Weapon Token
				if (bot.Config.Get<bool>("SUPERHammer")) {
					FormatLog(Text: "Cysero's SUPER Hammer", Title: true);
					ItemArray = new[] {"Cysero's SUPER Hammer", "C-Weapon Token"};
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
									HuntFor: !bot.Config.Get<bool>("DisableHunt"),
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
								HuntFor: !bot.Config.Get<bool>("DisableHunt"),
								QuestID: QuestArray[0],
								MonsterName: "Geist",
								MapName: "deadmoor",
								CellName: "r13",
								PadName: "Right"
							);
							SafeQuestComplete(QuestArray[0]);
							bot.Wait.ForPickup(ItemArray[1]);
							FormatLog("Cysero's Hammer", $"You now own [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
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
				if (bot.Config.Get<bool>("BrightKnight")) {
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
						FormatLog($"{ItemArray[0]}", $"You now own [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
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
						FormatLog($"{ItemArray[0]}", $"You now own [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
					}
					BankArray(ItemArray);
				}

				// Twig, Twilly & Zorbak pet - Moglin MEAL
				if (bot.Config.Get<bool>("MoglinPets")) {
					FormatLog(Text: "Twig, Twilly & Zorbak Pets", Title: true);
					FormatLog("Notice", "The bot can only the quest all these use once a day.");
					FormatLog(Followup: true, Text: "Ignore the extra DailyCheker messages");
				// Twig Pet
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
							HuntFor: !bot.Config.Get<bool>("DisableHunt"),
							QuestID: QuestArray[0],
							MonsterName: "Frogzard",
							MapName: "nexus",
							CellName: "Enter",
							PadName: "Spawn"
						);
						SafeQuestComplete(QuestArray[0]);
						bot.Wait.ForPickup(ItemArray[1]);
						FormatLog($"{ItemArray[0]}", $"You now own [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
					}
					BuyGoal(
						MapName: "Twig Pet",
						ShopID: 1081
					);
					BankArray(ItemArray);

				// Twilly Pet
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
							HuntFor: !bot.Config.Get<bool>("DisableHunt"),
							QuestID: QuestArray[0],
							MonsterName: "Frogzard",
							MapName: "nexus",
							CellName: "Enter",
							PadName: "Spawn"
						);
						SafeQuestComplete(QuestArray[0]);
						bot.Wait.ForPickup(ItemArray[1]);
						FormatLog($"{ItemArray[0]}", $"You now own [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
					}
					BuyGoal(
						MapName: "Twilly Pet",
						ShopID: 1081
					);
					BankArray(ItemArray);

				// Zorbak Pet
					FormatLog(Text: "Zorbak Pets", Title: true);
					ItemArray = new[] {"Zorbak Pet", "Moglin MEAL"};
					if (DailyCheckANY(QuestArray[0])) {
						FormatLog($"{ItemArray[0]}", "Doing the Daily Quest", Tabs: 1);
						UnbankList(ItemArray);
						GetDropList(ItemArray);
						FarmMode();
						ItemFarm(
							"Frogzard Meat", 3,
							Temporary: true,
							HuntFor: !bot.Config.Get<bool>("DisableHunt"),
							QuestID: QuestArray[0],
							MonsterName: "Frogzard",
							MapName: "nexus",
							CellName: "Enter",
							PadName: "Spawn"
						);
						SafeQuestComplete(QuestArray[0]);
						bot.Wait.ForPickup(ItemArray[1]);
						FormatLog($"{ItemArray[0]}", $"You now own [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
					}
					BuyGoal(
						MapName: "Zorbak Pet",
						ShopID: 1081
					);
					BankArray(ItemArray);
				}
			}

			/// Priority Misc. Items
			if (!bot.Config.Get<bool>("DisablePrioMisc")) {
				FormatLog(Text: "Priority Misc. Items", Title: true);
				
				// Mothly Treasure Chest Keys - Magic Treasure Chest Key
				if (IsMember && bot.Config.Get<bool>("TreasureChestKeys")) {
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
				if (bot.Config.Get<bool>("TheWheelOfDoom")) {
					FormatLog(Text: "The Wheel of Doom", Title: true);
					ItemArray = new[] {"Gear of Doom"};
					QuestArray = new[] {3075, 3076};
					ExitCombat();
					if (IsMember && bot.Quests.IsDailyComplete(QuestArray[0]) && !CheckStorage(ItemArray[0], 3))
						FormatLog("Wheel of Doom", "Quests unavailable", Tabs: 1);
					if (IsMember && !bot.Quests.IsDailyComplete(QuestArray[0])) {
						if (CheckStorage(ItemArray[0], 3))
							FormatLog("Wheel of Doom", "Doing the Free-Player Weekly and Legend-Only Daily Quest", Tabs: 1);
						else FormatLog("Wheel of Doom", "Doing the Legend-Only Daily Quest", Tabs: 1);
						SafeQuestComplete(QuestArray[0]);
						bot.Wait.ForDrop("*");
						bot.Player.PickupAll(true);
					}
					if (CheckStorage(ItemArray[0], 3)) {
						if (!IsMember)
							FormatLog("Wheel of Doom", "Doing the Free-Player Weekly Quest", Tabs: 1);
						if (bot.Bank.Contains(ItemArray[0]))
							bot.Bank.ToInventory(ItemArray[0]);
						SafeQuestComplete(QuestArray[1]);
						bot.Wait.ForDrop("*");
						bot.Player.PickupAll(true);
					}
					else if (!IsMember)
						FormatLog("Wheel of Doom", "Quests unavailable", Tabs: 1);
				}

				// Free Daily Boost - XP Boost /  REP Boost /  GOLD Boost / Class Boost 
				if (IsMember && bot.Config.Get<BoostEnum>("BoostEnum").ToString() != "Disabled") {
					FormatLog(Text: "Free Daily Boost", Title: true);
					ItemArray = new[] {$"{bot.Config.Get<BoostEnum>("BoostEnum").ToString().Replace("_", " ")}! (60 min)"};
					QuantityArray = new[] {500};
					QuestArray = new[] {4069};
					if (DailyCheckANY(QuestArray[0])) {
						FormatLog("Daily Boost", "Doing the Legend-Only Daily Quest", Tabs: 1);
						UnbankList(ItemArray);
						GetDropList(ItemArray);
						SafeQuestComplete(QuestArray[0]);
						bot.Wait.ForPickup(ItemArray[0]);
						FormatLog("Daily Boost", $"You now own [{ItemArray[0]}] x{bot.Inventory.GetQuantity(ItemArray[0])}", Tabs: 1);
					}
					BankArray(ItemArray);
				}

				// Ballyhoo's Ad Rewards - 5 AC / 500 Gold
				if (bot.Config.Get<bool>("Ballyhoo")) {
					FormatLog(Text: "Ballyhoo's Ad Rewards", Title: true);
					SafeMapJoin("ballyhoo");
					if (bot.GetGameObject<int>("world.myAvatar.objData.iDailyAds") < 3) {
						FormatLog("Ballyhoo", "Obtaining Ad Rewards");
						int i = 0;
						while (bot.GetGameObject<int>("world.myAvatar.objData.iDailyAds") < 3) {
							bot.SendPacket("%xt%zm%getAdReward%7070%");
							i++;
							FormatLog("Ballyhoo", $"Received Ad Reward {i} time(s)");
							bot.Sleep(700);
						}
					}
					else FormatLog("Ballyhoo", "Max. amount of Ad Rewards already received today");
				}

				// Void Highlord - Elder's Blood
				if (bot.Config.Get<bool>("EldersBlood")) {
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
							HuntFor: !bot.Config.Get<bool>("DisableHunt"),
							QuestID: QuestArray[0],
							MonsterName: "Gorillaphant",
							MapName: "arcangrove",
							CellName: "Right",
							PadName: "Right"
						);
						SafeQuestComplete(QuestArray[0]);
						bot.Wait.ForPickup(ItemArray[1]);
						FormatLog($"{ItemArray[0]}", $"You now own [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
					}
					ItemArray = new[] {"Elders' Blood"};
					BankArray(ItemArray);
				}

				// Drakath's Armor - Dage's Scroll Fragment
				if  (bot.Config.Get<bool>("DrakathArmor")) {
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
							HuntFor: !bot.Config.Get<bool>("DisableHunt"),
							QuestID: QuestArray[0],
							MonsterName: "*",
							MapName: "mountdoomskull",
							CellName: "b2",
							PadName: "Left"
						);
						SafeQuestComplete(QuestArray[0]);
						bot.Wait.ForPickup(ItemArray[1]);
						FormatLog("Drakath's Armor", $"You now own [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
					}
					BankArray(ItemArray);
				}

				// Mine Crafting Ores - Aluminum / Barium / Gold / Iron / Copper / Silver / Platinum
				if (bot.Config.Get<MineCraftingEnum>("MineCrafting").ToString() != "Disabled") {
					FormatLog(Text: "Mine Crafting Ores", Title: true);
					ItemArray = new[] {bot.Config.Get<MineCraftingEnum>("MineCrafting").ToString()};
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
							HuntFor: !bot.Config.Get<bool>("DisableHunt"),
							QuestID: QuestArray[0],
							MonsterName: "Balboa",
							MapName: "stalagbite",
							CellName: "Enter",
							PadName: "Spawn"
						);
						ItemFarm(
							"Axe of the Prospector", 1,
							Temporary: false,
							HuntFor: !bot.Config.Get<bool>("DisableHunt"),
							QuestID: QuestArray[0],
							MonsterName: "Balboa",
							MapName: "stalagbite",
							CellName: "Enter",
							PadName: "Spawn"
						);
						SafeQuestComplete(QuestArray[0], (int)bot.Config.Get<MineCraftingEnum>("MineCrafting"));
						bot.Wait.ForPickup(ItemArray[0]);
						FormatLog("Mine Crafting", $"You now own [{ItemArray[0]}] x{bot.Inventory.GetQuantity(ItemArray[0])}", Tabs: 1);
					}
					BankArray(ItemArray);
				}

				// Hard Core Metals - Arsenic / Beryllium / Chromium / Palladium / Rhodium / Thorium / Mercury
				if (IsMember && bot.Config.Get<HardCoreMetalsEnum>("HardCoreMetals").ToString() != "Disabled") {
					FormatLog(Text: "Hard Core Metals", Title: true);
					ItemArray = new[] {bot.Config.Get<HardCoreMetalsEnum>("HardCoreMetals").ToString()};
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
							HuntFor: !bot.Config.Get<bool>("DisableHunt"),
							QuestID: QuestArray[0],
							MonsterName: "Balboa",
							MapName: "stalagbite",
							CellName: "Enter",
							PadName: "Spawn"
						);
						ItemFarm(
							"Axe of the Prospector", 1,
							Temporary: false,
							HuntFor: !bot.Config.Get<bool>("DisableHunt"),
							QuestID: QuestArray[0],
							MonsterName: "Balboa",
							MapName: "stalagbite",
							CellName: "Enter",
							PadName: "Spawn"
						);
						SafeQuestComplete(QuestArray[0], (int)bot.Config.Get<HardCoreMetalsEnum>("HardCoreMetals"));
						bot.Wait.ForPickup(ItemArray[0]);
						FormatLog("Hard Core Metals", $"You now own [{ItemArray[0]}] x{bot.Inventory.GetQuantity(ItemArray[0])}", Tabs: 1);
					}
					BankArray(ItemArray);
				}

				// Armor of Awe - Pauldron of Awe - Pauldron Fragment - Pauldron Shard
				if (IsMember && bot.Config.Get<bool>("ArmorOfAwe")) {
					FormatLog(Text: "Armor of Awe (Pauldron of Awe)", Title: true);
					ItemArray = new[] {"Legendary Awe Pass", "Pauldron of Awe", "Pauldron Fragment", "Pauldron Shard"};
					if (!CheckStorage("Armor of Awe")) {
						if (!CheckStorage("Pauldron of Awe")) {
							if (!CheckStorage("Pauldron Fragment", 15)) {
								if (!bot.Quests.IsDailyComplete(4160)) {
									UnbankList(ItemArray);
									GetDropList(ItemArray);
									SoloMode();
									ItemFarm(
										"Pauldron Shard", 15,
										Temporary: false,
										HuntFor: !bot.Config.Get<bool>("DisableHunt"),
										QuestID: 4160,
										MonsterName: "Ultra Akriloth",
										MapName: "gravestrike",
										CellName: "r1",
										PadName: "Left"
									);
									SafeQuestComplete(4160);
								}
								else FormatLog("DailyCheck", $"Daily Quest unavailable", Tabs: 1);
							}
							else {
								if (bot.Bank.Contains("Pauldron Fragment"))
									bot.Bank.ToInventory("Pauldron Fragment");
								SafePurchase(
									"Pauldron of Awe", 1,
									MapName: "museum",
									ShopID: 1129
								);
							}
						}
						else
							FormatLog("Pauldron of Awe", "You already own [Pauldron of Awe] x1", Tabs: 1);
					}
					else
						FormatLog("Armor of Awe", "You already own [Armor of Awe] x1", Tabs: 1);
					BankArray(ItemArray);
				}

			}

			/// Misc. Items
			if (!bot.Config.Get<bool>("DisableMisc")) {
				FormatLog(Text: "Misc. Items", Title: true);

				// Crypto Tokens
				if (bot.Config.Get<bool>("CryptoToken")) {
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
							HuntFor: !bot.Config.Get<bool>("DisableHunt"),
							QuestID: QuestArray[0],
							MonsterName: "Sneevil",
							MapName: "boxes",
							CellName: "Fort1",
							PadName: "Right"
						);
						SafeQuestComplete(QuestArray[0]);
						bot.Wait.ForPickup(ItemArray[0]);
						FormatLog($"{ItemArray[0]}", $"You now own [{ItemArray[0]}] x{bot.Inventory.GetQuantity(ItemArray[0])}", Tabs: 1);
					}
					BankArray(ItemArray);
				}

				// Legion Castle - Shadow Shroud
				if (bot.Config.Get<bool>("ShadowShroud")) {
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
							HuntFor: !bot.Config.Get<bool>("DisableHunt"),
							QuestID: QuestArray[0],
							MonsterName: "Shadow Creeper",
							MapName: "bludrut2",
							CellName: "r6",
							PadName: "Up"
						);
						SafeQuestComplete(QuestArray[0]);
						bot.Wait.ForPickup(ItemArray[1]);
						FormatLog($"{ItemArray[0]}", $"You now own [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
					}
					BankArray(ItemArray);
				}

				// GRUMBLE, GRUMBLE...
				if (CheckStorage("Crag &amp; Bamboozle") && bot.Config.Get<bool>("GRUMBLE")) {
					FormatLog(Text: "GRUMBLE, GRUMBLE...", Title: true);
					ItemArray = new[] {"Diamond of Nulgath", "Blood Gem of the Archfiend"};
					ItemArrayB = new[] {"Crag &amp; Bamboozle"};
					QuantityArray = new[] {1000, 100};
					QuestArray = new[] {592};
					if (DailyCheckANY(QuestArray[0])) {
						UnbankList(ItemArray.Concat(ItemArrayB).ToArray());
						GetDropList(ItemArray.Concat(ItemArrayB).ToArray());
						SafeQuestComplete(QuestArray[0]);
						bot.Wait.ForPickup(ItemArray[0]);
						FormatLog("GRUMBLE", $"You now own [{ItemArray[0]}] x{bot.Inventory.GetQuantity(ItemArray[0])}", Tabs: 1);
						FormatLog("GRUMBLE", $"You now own [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
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
		*	Lord of Order

		-		Cosmetics
		*	Golden Inquisitor of Shadowfall

		-		Priority Misc. Farm

		-		Misc. Farm
		*	Power Gems
		*	Realm Gems
		*	Read the Deisgn Notes! (Valencia /Battleon)
		*	GRUMBLE, GRUMBLE..., (Crag & Bamboozle)

		-		PvP
		*	Is this even possible to bot?
		*	1v1 Legion PvP Trophy
		*	​1v1 PvP Trophy
	*/

	/*------------------------------------------------------------------------------------------------------------
												 	Daily Quest Template
	------------------------------------------------------------------------------------------------------------*/

	/*
				if (bot.Config.Get<bool>("String")) {
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
							HuntFor: !bot.Config.Get<bool>("DisableHunt"),
							QuestID: QuestArray[0],
							MonsterName: "*",
							MapName: "battleon",
							CellName: "Enter",
							PadName: "Spawn"
						);
						SafeQuestComplete(QuestArray[0]);
						bot.Wait.ForPickup(ItemArray[1]);
						FormatLog($"{ItemArray[0]}", $"You now own [{ItemArray[1]}] x{bot.Inventory.GetQuantity(ItemArray[1])}", Tabs: 1);
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
		if (!bot.Config.Get<bool>("PrivateOnly"))
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

	public void BuyGoal(string MapName, int ShopID) 
	{
		if (bot.Config.Get<bool>("buyGoal")) {
			if (!CheckStorage(ItemArray[0], QuantityArray[0]) && CheckStorage(ItemArray[1], QuantityArray[1])) {
				SafePurchase(
					ItemArray[0], 
					QuantityArray[0], 
					MapName: MapName, 
					ShopID: ShopID
				);
				FormatLog(ItemArray[0], "Bought", Tabs: 1);
			}
		}
	}

	public bool DailyCheckANY (int QuestID)
	{
		ExitCombat();
		if (!bot.Config.Get<bool>("DisableQuantity")) {
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
		if (!bot.Config.Get<bool>("DisableQuantity")) {
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
		bot.Sleep(ExitCombatTimer);
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