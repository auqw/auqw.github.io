import os 
from pprintpp import pprint
os.chdir(os.path.dirname(os.path.abspath(__file__)))


with open("./changelog.txt", "r", encoding="utf-8") as f:
	item = f.read().split("\n\n")

link = []
for i in item:
	head = i.split("\n")[0]
	print(f"               <h5 id=\"{head}\">{head}</h5>")
	items = i.split("\n")[1:]
	print("               <ul>")
	for j in items:
		pass
		print(f"                  <li>{j.replace('-', '')}</li>")
	print("               </ul>\n")

	link.append(f"            <a href=\"#{head}\">{head.replace(':', '')}</a> | ")

# for i in link:
# 	print(i)

# pprint(item)