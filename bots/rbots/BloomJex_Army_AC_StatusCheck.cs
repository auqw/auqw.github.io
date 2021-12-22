// AC Checker and relogger by BLoom
using System;
using RBot;
using System.Collections.Generic;


// OPEN THE LOGS IF YOU WANT THIS TO WORK
// OPEN THE LOGS IF YOU WANT THIS TO WORK
// OPEN THE LOGS IF YOU WANT THIS TO WORK
// OPEN THE LOGS IF YOU WANT THIS TO WORK
// OPEN THE LOGS IF YOU WANT THIS TO WORK

// THEN CHECK THE SCRIPT TAB OF THE LOGS FOR THE RESULT

public class Script {
	public ScriptInterface bot => ScriptInterface.Instance;

	// Insert the desired server
	public string server = "Twig";

	// Insert a desired room for doom in case of gears
	int R_Number = 999999;

	// Place accounts here
	// {"Username", "Password"}
	public Dictionary<string, string> accounts = new Dictionary<string, string>(){
		{"Username1", "Password"},
		{"Username2", "Password"},
		{"Username3", "Password"},
	}; 
	

	public void ScriptMain(ScriptInterface bot){
		string G_state = "Not Enough"; 
		if (bot.Player.LoggedIn) {
            bot.Player.Logout();
			bot.Sleep(4000);
		}
        
		foreach(var acc in accounts) {
			while (!bot.Player.LoggedIn) {
                bot.CallGameFunction("login", acc.Key, acc.Value);
                bot.Player.Reconnect(server);
                while (!bot.Player.Loaded) { }
                while (!bot.Map.Loaded) { }

				if(bot.Inventory.Contains("Gear of Doom",3) || bot.Player.IsMember){
					bot.Player.Join($"doom-{R_Number}");
					while (!bot.Map.Loaded) { };
					bot.Sleep(1000);
					if(bot.Inventory.Contains("Gear of Doom",3)){bot.SendPacket("%xt%zm%tryQuestComplete%81807%3076%-1%false%wvz%");};
					if(bot.Player.IsMember){bot.SendPacket("%xt%zm%tryQuestComplete%83387%3075%-1%false%wvz%");};
					G_state = "Done";
					bot.Sleep(1000);
				};
				
                string coin = bot.GetGameObject<string>("world.myAvatar.objData.intCoins");
				bot.Log($"Gears: {G_state} || ACs: {coin} <- [ {acc.Key} ]");
				if (bot.Player.LoggedIn) {
                    bot.Player.Logout();
                    bot.Sleep(3000);
					break;
				}
			}

        }
		bot.Log("\n\t\t\tDone");
	}
}
