// Author: Bot By Bloom
// Title: Deluxe Follower Bot
// Instructions: Just run the bot and choose the options in the pop-up menu

using System;
using RBot;
using RBot.Options;
using System.Collections.Generic;
using System.Windows.Forms;

// DO NOT EDIT ANYTHING. JUST RUN THE BOT. YOU'LL EDIT THE SETTINGS IN THE UI
// DO NOT EDIT ANYTHING. JUST RUN THE BOT. YOU'LL EDIT THE SETTINGS IN THE UI
// DO NOT EDIT ANYTHING. JUST RUN THE BOT. YOU'LL EDIT THE SETTINGS IN THE UI
// DO NOT EDIT ANYTHING. JUST RUN THE BOT. YOU'LL EDIT THE SETTINGS IN THE UI
// DO NOT EDIT ANYTHING. JUST RUN THE BOT. YOU'LL EDIT THE SETTINGS IN THE UI


public class Script {
	public ScriptInterface Bot;
	public List<int> SkillArray = new List<int>();
	public string OptionsStorage = "DeluxeFollowerConfig";

    public List<IOption> Options = new List<IOption>() {
        new Option<string>("TargetPlayer", "Target Player", "The Target Player Name", "Artix"),
        new Option<bool>("UseLockZone", "Use LockZone", "Lock zones that the bot will search to follow player.", false),
        new Option<string>("LockZones", "LockZone List", "List of lock zones separated by a comma. \nExample -> shadowrealm-9934, battlegrounda,tercessuinotlim", ""),
        new Option<string>("TargetEnemy", "Target Enemy", "List of Enemies that are priority attacks.\nExample -> Drone, Staff of Escherion,", " "),
        new Option<string>("skillOrder", "Skill Order", "The order of skill execution. Use -> [1, 2, 3, 4, 5, 6] just like the aqw UI skill keys. Add wNumber to delay before executing a skill. Time is in ms, minimum of 10ms. Example -> 5,4,w800,1", "3,w200,4,w500,2,1"),
        new Option<int>("skillWait", "Skill Wait", "Inherent wait between each skill.", 500)
    };

	public void ScriptMain(ScriptInterface bot){
		// Start
		ExitCombat(bot);


		// Options
		bot.Options.SafeTimings = true;
		bot.Options.RestPackets = true;
		bot.Options.PrivateRooms = true;
		bot.Options.InfiniteRange = true;
		bot.Options.PrivateRooms = false;
		bot.Options.SkipCutscenes = true;
		bot.Options.DisableFX = true;
		bot.Options.AggroMonsters = false;
		bot.Options.LagKiller = true;

		// Setup Skills
		string skillOrder = bot.Config.Get<string>("skillOrder");
		foreach (var i in skillOrder.Split(',')) {
			try {
				int a = int.Parse(i.Replace(" ", "").Replace("w", ""));
				if (a >= 1) {
					SkillArray.Add(a-1);
					bot.Log(a.ToString());
				}
			} catch {
				continue;
			}
		}


		// Setup Follower Variables
		string TargetPlayer = bot.Config.Get<string>("TargetPlayer");
		string Cell, Pad;
		int skillWait = bot.Config.Get<int>("skillWait");

		// Setups Lockzone joins
		int GotoTries = 0;
		List<string> LockZones = new List<string>();
		bool UseLockZone = bot.Config.Get<bool>("UseLockZone");
		string _lockzone = bot.Config.Get<string>("LockZones").Replace(" ", "");
		if (!string.IsNullOrEmpty(_lockzone)) {
			foreach (var zone in _lockzone.Split(',')) {
				if (!string.IsNullOrEmpty(zone)) {
					LockZones.Add(zone.ToLower());
				}
			}
		}

		// Setups Monster targetting
		string Target = "*";
		bool emptyMonsterList = false;
		bool emptyMonterTarget = true;
		List<string> TargetEnemy = new List<string>();
		List<string> MonsterList = new List<string>();
		string _targetenemy = bot.Config.Get<string>("TargetEnemy");
		if (!string.IsNullOrEmpty(_targetenemy)) {
			foreach (var enemy in _targetenemy.Split(',')) {
				if (!string.IsNullOrEmpty(enemy)) {
					TargetEnemy.Add(enemy.Trim());
				}
			}
		} else {
			emptyMonsterList = true;
		}


		// Following
		while (!bot.ShouldExit()) {

			// Checks map
			if (!bot.Map.PlayerExists(TargetPlayer)) {
				Target = "*";
				MonsterList.Clear();
				emptyMonterTarget = true;
				GotoTries += 1;
				if (bot.Player.State == 2) {
					bot.Player.Jump("Wait", "Spawn");
					while (bot.Player.State == 2) {}
				}
				bot.SendPacket($"%xt%zm%cmd%1%goto%{TargetPlayer}%");
				bot.Sleep(2500);
				if (GotoTries == 3 && UseLockZone == true) {
					foreach(var zone in LockZones) {
						if (zone == "tercessuinotlim") {
							bot.Player.Jump("m22", "Center");
							bot.Sleep(500);
						}
						bot.Player.Join(zone, "Enter", "Spawn");
						bot.Wait.ForMapLoad(zone);
						if (bot.Map.PlayerExists(TargetPlayer)) {
							GotoTries = 0;
							break;
						}
					}
					GotoTries = 0;
				}
				continue;
			} else {
				GotoTries = 0;
			}


			// Checks Player Cell
			Cell = bot.Map.GetPlayer(TargetPlayer).Cell;
			if (Cell != bot.Player.Cell) {
				Pad = bot.Map.GetPlayer(TargetPlayer).Pad;
				bot.Player.Jump(Cell, Pad);
				Target = "*";
				MonsterList.Clear();
				emptyMonterTarget = true;
			}

			// Check Monsters
			if (emptyMonterTarget) {
				if (!emptyMonsterList) {
					foreach(var mons in bot.Monsters.CurrentMonsters) {
						MonsterList.Add(mons.Name);
					}
					foreach(var mons in TargetEnemy) {
						bot.Log(mons);
						if (MonsterList.Contains(mons)) {
							Target = mons;
							emptyMonterTarget = false;
							break;
						}
					}
				}
			}


			// Kills monsters 
			if (!(bot.Monsters.CurrentMonsters.Count == 0)) {
				SkillAttack(bot, Target);
			}
			bot.Sleep(2500);
		}


	}

	public void SkillAttack(ScriptInterface bot, string Target, int skillWait=500) {
		bot.Player.Attack(Target);
		foreach(var skill in SkillArray) {	
			if (skill > 6) {
				bot.Sleep(skill);
			} else {
				// if (bot.Player.CanUseSkill(skill)) bot.Player.UseSkill(skill);	
				bot.Log($"Skill: {skill.ToString()}");
				bot.Player.UseSkill(skill);
				// bot.Sleep(skillWait);
			}
		}
	}

	public void ExitCombat(ScriptInterface bot) {
		if (bot.Player.State == 2) {
			bot.Player.Jump("Wait", "Spawn");
			while (bot.Player.State == 2) {}
		}
	}


}

// %xt%warning%-1%Cannot goto to player in a Locked Zone.%

