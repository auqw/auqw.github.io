using System;
using RBot;
using System.Linq;

public class SmartAttackHunt {
	
	/// This bot has two modes: 
	/// - If you target a monster in-game before activating the bot, it will hunt that monster all across the map (private room is advised).
	/// - If you dont target a monster in-game before hand, it will attack any monster on screen and will stay on said screen.
	/// Note that the bot will automatically turn in any quests that are completed while the bot is active.
	
	//-----------EDIT BELOW-------------//
	public int TurnInAttempts = 10;
	public readonly int[] SkillOrder = { 2, 4, 3, 1 };
	public string[] GrabTheseDrops = {
		"Item Name Here",
		"Item Name Here",
		"Item Name Here",
		"Item Name Here",
		"Item Name Here"
		//If you need more items than 5, just add more items to this list
	 };
	//-----------EDIT ABOVE-------------//

	public ScriptInterface bot => ScriptInterface.Instance;
	public string Target;
	public void ScriptMain(ScriptInterface bot){
		bot.Options.SafeTimings = true;
		bot.Options.RestPackets = true;
		bot.Options.InfiniteRange = false;
		
		//Remove the "//" on the line below to activate the automation of picking up drops.
		//GetDropList(GrabTheseDrops);
		SkillList(SkillOrder);
		AutoQuestComplete();
		DeathHandler();
		
		FormatLog(Text: "Script Started", Title: true);

		// Checking if player should hunt or attack
		if (bot.Player.HasTarget) {
			Target = bot.Player.Target.Name;
			FormatLog("Hunting", $"[{Target}]");
		}
		else FormatLog("Attacking", "[Everything]", Tabs: 1);

		// The Hunting part
		if (bot.Player.HasTarget)
			while(!bot.ShouldExit()) {
				bot.Player.Hunt(Target);
				if (bot.Quests.ActiveQuests.Count >= 1)
					foreach (var Quest in bot.Quests.ActiveQuests) {
						int QuestID = Quest.ID;
						if (bot.Quests.CanComplete(QuestID)) {
							bot.Wait.ForQuestComplete(QuestID);
							bot.Sleep(700);
						}
					}
			}
		
		// The Attacking part
		else while(!bot.ShouldExit()) {
			bot.Player.SetSpawnPoint();
			bot.Player.Attack("*");
		}
	}
	
	/*------------------------------------------------------------------------------------------------------------
													 Required Functions
	------------------------------------------------------------------------------------------------------------*/
	//These functions are required for this bot to function.

	/// <summary>
	/// Constantly checks if there are quests ready for turnin. If that is the case, jump to Wait, Enter 
	/// </summary>
	public void AutoQuestComplete()
	{
		if(bot.Handlers.Any(h => h.Name == "Quest Handler"))
			bot.Handlers.RemoveAll(h => h.Name == "Quest Handler");
		bot.RegisterHandler(4, b => {
			if (bot.Quests.ActiveQuests.Count >= 1) {
				foreach (var Quest in bot.Quests.ActiveQuests) {
					int QuestID = Quest.ID;
					if (bot.Quests.CanComplete(QuestID)) {
						string Cell = bot.Player.Cell;
						string Pad = bot.Player.Pad;
						SafeQuestComplete(QuestID);
						bot.Player.Jump(Cell, Pad);
					}
				}
			}
		}, "Quest Handler");
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
			FormatLog("Quest", $"Turned in Quest {QuestID} unsuccesfully. Logging out");
			bot.Player.Logout();
		}
		FormatLog("Quest", $"Turned in Quest {QuestID}");
		while (!bot.Quests.IsInProgress(QuestID)) bot.Quests.EnsureAccept(QuestID);
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
	
	/// <summary>
	/// Exits Combat by jumping cells.
	/// </summary>
	public void ExitCombat()
	{
		bot.Options.AggroMonsters = false;
		bot.Player.Jump("Wait", "Spawn");
		bot.Wait.ForCombatExit();
		bot.Sleep(2000);
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