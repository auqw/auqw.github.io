text = '''Added “Immediate Login” button (on Tools tab from the main menu). Used for Login (click on button), Connect to Server (select a server on the dropdown list), and Login + Connect to Server (click on dropdown and press Enter key). The Login depends on the Username and the Password from the login screen. The Server is set default to Twilly, can be configured (with the dropdown index) to other servers in the BotClientConfig.cfg file.
Added “Immediate Login” configuration to Actions’ dropdown list in the Hotkeys panel.
Added “Clear Captured” button (in the Packet Tamperer panel). Used for clearing the Captured Packet textbox.
Added “Load Map SWF” button and command. Used for loading a map SWF on client-side.
Added Aura Check statements, consists of Player Aura and Target Aura lists. They’re used for checking ability buff/debuff on the player or the target based on the commands.
Added “Player In Combat” statements. Used for checking a player’s state on combat.
Added “Cancel Target” command. Used for canceling the target out of the (self) player’s focus.
Added “Cancel Auto Attack” command. Used for canceling the (self) player’s auto attack out of combat.
Added “Monster Health” statements. Used for checking a monster’s health (greater/lesser than).
Added “Health Percentage” statements. Used for checking the (self) player’s health percentage (greater/lesser than).
Added “Buy Item By ID” commands. Used for buying an item by Item ID, Shop ID, and Shop Item ID.
Added “Stop Bot With Message” command. Used for stopping the bot automatically by command, attached with a customizable message.
Added “Custom Class” command. Used for changing the (self) player’s class name on client-side.
Added “Provoke All Monster” option and toggle commands. Used for drawing aggro/provoking upon all monster in the map. Provoke packet can be customized in the Custom box.
Added “Skill Available” statements. Used for checking a skill’s availability (by using a skill index), based on the commands.
Added “Player Count in Cell” statements. Used for checking player count in a cell.
Added “Player’s Equipped Class” statements. Used for checking a player’s equipped class.
Added “Get Class with Variable” statement command. Used for getting the (self) player’s equipped class’ name and sets it to a variable.
Added built-in hotkey “CTRL + X”. Used for copying the selected index/command and then removes them from the list.
Added built-in hotkey “CTRL + R” as an alternative for Remove function (CTRL + DEL).
Added “End Description” text into the Quest Grabber.'''


list_ = text.split("\n")
for i in list_:
	print(f"<li> {i}</li>")




