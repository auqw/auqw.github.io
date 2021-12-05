using System;
using RBot;

public class ExeBot {
	
	/// <summary>
	/// This bot has two modes: 
	/// - If you target a monster in-game before activating the bot, it will hunt that monster all across the map (private room is advised).
	/// - If you dont target a monster in-game before hand, it will attack any monster on screen and will stay on said screen.
	/// Note that the bot will automatically turn in any quests that are completed while the bot is active.
	/// </summary>
	
	
	//-----------EDIT BELOW-------------//
	public int TurnInAttempts = 10;
	public readonly int[] SkillOrder = { 2, 4, 3, 1 };
	//-----------EDIT ABOVE-------------//

	public ScriptInterface bot => ScriptInterface.Instance;
	public string Target;
	public int QuestID;
	public string Cell;
	public string Pad;
	public void ScriptMain(ScriptInterface bot){
		bot.Options.SafeTimings = true;
		bot.Options.RestPackets = true;
		bot.Options.InfiniteRange = false;
		
		SkillList(SkillOrder);
		
		Target = bot.Player.Target.Name;
		if (Target != null) bot.Log($"[{DateTime.Now:HH:mm:ss}] Hunting \t \t [{Target}]");
		else bot.Log($"[{DateTime.Now:HH:mm:ss}] Attacking \t [Everything]");
		while(!bot.ShouldExit())
		{
			if (Target != null) bot.Player.Hunt(Target);
			else
			{
				bot.Player.SetSpawnPoint();
				bot.Player.Attack("*");
			}
			
			if (bot.Quests.ActiveQuests.Count >= 1);
			{
				foreach (var Quest in bot.Quests.ActiveQuests)
				{
					QuestID = Quest.ID;
					if (bot.Quests.CanComplete(QuestID))
					{
						Cell = bot.Player.Cell;
						Pad = bot.Player.Pad;
						SafeQuestComplete(QuestID);
						bot.Player.Jump(Cell, Pad);
					}
				}
			}
		}
	}
	
	/// <summary>
	/// Spams Skills when in combat. You can get in combat by going to a cell with monsters in it with bot.Options.AggroMonsters enabled or using an attack command against one.
	/// </summary>
	public void SkillList(params int[] Skillset)
	{
		bot.RegisterHandler(1, b =>
		{
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
			bot.Log($"[{DateTime.Now:HH:mm:ss}] Failed to turn in Quest {QuestID}. Logging out.");
			bot.Player.Logout();
		}
		bot.Log($"[{DateTime.Now:HH:mm:ss}] Turned In Quest {QuestID} successfully.");
		while (!bot.Quests.IsInProgress(QuestID)) bot.Quests.EnsureAccept(QuestID);
	}
	
	/// <summary>
	/// Exits Combat by jumping cells.
	/// </summary>
	public void ExitCombat()
	{
		bot.Options.AggroMonsters = false;
		bot.Player.Jump("Wait", "Spawn");
		bot.Sleep(1000);
		while (bot.Player.State == 2) { }
	}
}