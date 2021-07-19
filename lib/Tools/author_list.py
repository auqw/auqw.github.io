from os import listdir
from os.path import isfile, join
from pprintpp import pprint

import json

boat = "C:\\Users\\Adrian Tagayom\\Google Drive\\Programming\\HTML\\auqw-portal-revised\\discord-bot\\Data\\boats.json"
save = "C:\\Users\\Adrian Tagayom\\Google Drive\\Programming\\HTML\\auqw-portal-revised\\discord-bot\\Data\\author_list.json"
with open(boat, 'r', encoding='utf-8') as f:
    boath = json.load(f)

author_list = {}

for i in boath:
	x = i.split(".")
	if len(x) != 2:
		print(i)
	author = boath[i]["authors"]
	for j in author:
		if j not in author_list:
			author_list[j] = {
				"count": 0,
				"bots": []
			}
		author_list[j]["count"] += 1
		author_list[j]["bots"].append(i)

with open(save, 'w', encoding='utf-8') as f:
    json.dump(author_list, f, ensure_ascii=False, indent=4)

# 
# pprint(author_list)

