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

	public string server = "twig";

	// Place accounts here
	// {"Username", "Password"}
	public Dictionary<string, string> accounts = new Dictionary<string, string>(){
		{"Artix", "daddyArtixPleaseCumInsideMe"},
		{"Alina", "Give me than Thin ASS!"}

        
		
	};

	public void ScriptMain(ScriptInterface bot){
		if (bot.Player.LoggedIn) {
            bot.Player.Logout();
			bot.Sleep(2000);
		}
        
		foreach(var acc in accounts) {
			while (!bot.Player.LoggedIn) {
                bot.CallGameFunction("login", acc.Key, acc.Value);
                while (!bot.Player.Loaded) { }
                while (!bot.Map.Loaded) { }


                string coin = bot.GetGameObject<string>("world.myAvatar.objData.intCoins");
                bot.Log($"[{acc.Key}] {coin} ACs");
				if (bot.Player.LoggedIn) {
                    bot.Player.Logout();
                    bot.Sleep(1500);
					break;
				}
			}

        }
		bot.Log("\n\nDone");
	}



}


