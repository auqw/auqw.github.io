

import os
import codecs
from pprintpp import pprint
from datetime import date
from bs4 import BeautifulSoup as Soup


from urllib.request import urlopen, Request, urlretrieve
from urllib.parse import urlencode
from base64 import standard_b64encode
from json import loads

os.chdir(os.path.dirname(os.path.abspath(__file__)))


class HTMLUpdater:

    def __init__(self):

        html = codecs.open("./index.html", 'r', encoding='utf-8')
        self.soup = Soup(html, 'html.parser')

        package1 = {
            "Link": "https://twitter.com/twitter/status/1425838597492539402",
            "Image": "http://pbs.twimg.com/media/E9Rd76oXIAQUUkh.jpg",
            "content": {
                "Location": "/join burningbeach",
                "Enemy":["Shark", "http://aqwwiki.wikidot.com/shark"],
                "Item": "0 AC Last Two New Elemental Avatar Surfboards",
                "Posted": "12 August 2021"
            }
        }


        package2 = {
            "Link": "https://twitter.com/Alina_AE/status/1429820725381275649",
            "Image": "https://pbs.twimg.com/media/E5hTYPNWUAQZrak.jpg",
            "content": {
                "Duration": "48 Hours",
                "Boost": "48 Hour Double Class Points Boost",
                "Posted": "23 August 2021",
                "Ends in": "25 August 2021"
            }
        }

        self.daily_gift(
            mode="daily",
            package=package1,
            )
        self.save()


    def save(self):
        with open('./index.html', 'w+', encoding="utf-8") as f:
            f.write(self.soup.prettify())


    def get_image(self, url) -> str:

        urlretrieve(url, "daily.jpg")

        b64_image = standard_b64encode(open("./daily.jpg", "rb") .read())

        headers = {'Authorization': 'Client-ID ' + "4d8b88de7160b18"}
        data = {'image': b64_image, 'title': 'test'} 

        request = Request(url="https://api.imgur.com/3/upload.json", data=urlencode(data).encode("utf-8"),headers=headers)
        response = loads(urlopen(request).read())

        print(response)
        return response['data']['link']


    def daily_gift(self, mode, package):
        data = {}
        if mode == "daily":
            img = self.soup.find("div", {"id": "dailyGift"}).find("div", {"class":"overlay-red"}).find("img")
            img['src'] = self.get_image(package["Image"])
            print(img['src'])
            print()
            desc = self.soup.find("div", {"id":"dailyDescription"})
            desc.clear()

            title = self.soup.find("h1", {"id": "dailyTitle"})
            title.clear()

            link = self.soup.new_tag("a", attrs={"href": package["Link"], "target": "_blank"})
            bold = self.soup.new_tag("b")
            bold.string = "Daily Gift"
            link.append(bold)
            title.append(link)


            if "Location" in package["content"]:
                data["Location"] = self.soup.new_tag("p")

                bold = self.soup.new_tag("b")
                bold.string = "Location: "
                data["Location"].append(bold)

                span = self.soup.new_tag("span", attrs={"id": "loc"})
                span.string = package["content"]["Location"]
                data["Location"].append(span)

                button = self.soup.new_tag("button", attrs={"class": "btn", "onclick": "copy2clipboard('loc')"})
                button.string = "copy"
                data["Location"].append(button)

                desc.append(data["Location"])

            if "Enemy" in package["content"]:
                data["Enemy"] = self.soup.new_tag("p")

                bold = self.soup.new_tag("b")
                bold.string = "Enemy: "
                data["Enemy"].append(bold)

                link = self.soup.new_tag("a", attrs={"href": package["content"]["Enemy"][1], "target": "_blank"})
                link.string = package["content"]["Enemy"][0]
                data["Enemy"].append(link)

                desc.append(data["Enemy"])

            if "Quest" in package["content"]:
                data["Quest"] = self.soup.new_tag("p")

                bold = self.soup.new_tag("b")
                bold.string = "Quest: "
                data["Quest"].append(bold)

                text = self.soup.new_tag("span")
                text.string = package["content"]["Quest"][0]
                data["Quest"].append(text)

                desc.append(data["Quest"])

            if "Item" in package["content"]:
                data["Item"] = self.soup.new_tag("p")

                bold = self.soup.new_tag("b")
                bold.string = "Item: "
                data["Item"].append(bold)

                text = self.soup.new_tag("span")
                text.string = package["content"]["Item"]
                data["Item"].append(text)

                desc.append(data["Item"])

            if "Posted" in package["content"]:
                data["Posted"] = self.soup.new_tag("p")

                bold = self.soup.new_tag("b")
                bold.string = "Date Posted: "
                data["Posted"].append(bold)

                text = self.soup.new_tag("span")
                text.string = package["content"]["Posted"]
                data["Posted"].append(text)

                desc.append(data["Posted"])

        elif mode == "boost":


            # Boost
            boost = self.soup.find("p", {"id":"serverBoost"})
            boost.clear()

            bold = self.soup.new_tag("b")
            bold.string = "Boost: "
            boost.append(bold)

            span = self.soup.new_tag("span")
            span.string = package["content"]["Boost"]
            boost.append(span)


            # Deadline
            deadline = self.soup.find("p", {"id":"serverDeadline"})
            deadline.clear()

            bold = self.soup.new_tag("b")
            bold.string = "Ends in: "
            deadline.append(bold)

            span = self.soup.new_tag("span")
            span.string = package["content"]["Ends in"]
            deadline.append(span)


        # docs
        self.save()


x = HTMLUpdater()




# d4 = date.today().strftime("%d %b %Y")
# print("d4 =", d4)

# _botname_ = "Text.gbot"
# _author_ = "weeb"
# _date_ = "14 jun 2021"
# _tags_ = "test, test, test"
# _desc_ = "test desc"


# _download_ = self.soup.new_tag("a",attrs={"class": "btn2", "href": "./bots/"+_botname_})
# _download_.string = "Download"

# _botLink_ = self.soup.new_tag("a", attrs={"class": "collapsible", "type": "button"})
# _botLink_.string = _botname_

# _collapse_ = self.soup.new_tag("div", attrs={"class":"collapsibleContent"})


# # collapse tags
# _collapse_a_ = self.soup.new_tag("p")
# _collapse_a_.string = _tags_
# _collapse_a_a_ = self.soup.new_tag("b")
# _collapse_a_a_.string = "Tags: "
# _collapse_a_.string.insert_before(_collapse_a_a_)

# # collapse descs
# _collapse_b_ = self.soup.new_tag("p")
# _collapse_b_.string = _desc_
# _collapse_b_a_ = self.soup.new_tag("b")
# _collapse_b_a_.string = "Description: "
# _collapse_b_.string.insert_before(_collapse_b_a_)

# _collapse_.append(_collapse_a_)
# _collapse_.append(_collapse_b_)




# tr = self.soup.new_tag("tr")

# row_1 = self.soup.new_tag("td", attrs={"class": "collapsibleInfoName"})
# row_1.append(_botLink_)
# row_1.append(_collapse_)
# tr.append(row_1)

# row_2 = self.soup.new_tag("td", attrs={"class": "collapsibleInfo"})
# row_2.string = _author_
# tr.append(row_2)

# row_3 = self.soup.new_tag("td", attrs={"class": "collapsibleInfo"})
# row_3.string = _date_
# tr.append(row_3)

# row_4 = self.soup.new_tag("td", attrs={"class": "collapsibleInfo"})
# row_4.append(_download_)
# tr.append(row_4)


# div.insert(0, tr)
# print(self.soup)

