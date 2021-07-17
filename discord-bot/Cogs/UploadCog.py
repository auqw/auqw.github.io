from .Base import *
from discord.ext import commands

from pprintpp import pprint
from datetime import date
from bs4 import BeautifulSoup as Soup


class UploadCog(commands.Cog):

    def __init__(self, bot):
        self.bot = bot
        pass

    @commands.command()
    async def verify(self, ctx, role):
        print(type(role))
        if ctx.author.id != "252363724894109700":
            await ctx.send("\>It works!")
        else:
            await ctx.send(f"\>GTFO you're not <@252363724894109700>")
        
        # BaseProgram.settings[]



    @commands.command()
    async def upload(self, ctx, botName, author, tags, desc):
        await ctx.send(f"\> Hello.")


        try:
            attach = ctx.message.attachments[0]
        except:
            await ctx.send("\> Please attach a .txt file.")
            return

        file_n = attach.filename
        if file_n.split(".")[-1] != "txt":
            await ctx.send("\> Only a .txt files are allowed with `;uptext` command.")
            return  



        target_url = attach.url
        print(target_url)
                

        data = await self.get_site_content(URL=target_url, is_soup=False, encoding="cp1252")
        text = str(data).split("\n")

        tags_ = tags.replace("-", ",").capitalize()

        await self.update_portal(botName, author, tags, desc)


    async def update_portal(self, _botname_, _author_, _tags_, _desc_):

        portal_html, sha = BaseProgram.github.read("index.html")

        # root = lh.tostring(sliderRoot) #convert the generated HTML to a string
        soup = Soup(portal_html, 'html.parser')                #make BeautifulSoup
        # prettyHTML = soup.prettify()
        # pprint(prettyHTML)

        div = soup.find("div", {"id": "myModalBoats"}).find("table", {"id":"myTable"}).find("tbody")

        _date_ = date.today().strftime("%d %b %Y")


        # _botname_ = "Text.gbot"
        # _author_ = "weeb"
        # _date_ = "14 jun 2021"
        # _tags_ = "test, test, test"
        # _desc_ = "test desc"


        _download_ = soup.new_tag("a",attrs={"class": "btn2", "href": "./bots/"+_botname_})
        _download_.string = "Download"

        _botLink_ = soup.new_tag("a", attrs={"class": "collapsible", "type": "button"})
        _botLink_.string = _botname_

        _collapse_ = soup.new_tag("div", attrs={"class":"collapsibleContent"})


        # collapse tags
        _collapse_a_ = soup.new_tag("p")
        _collapse_a_.string = _tags_
        _collapse_a_a_ = soup.new_tag("b")
        _collapse_a_a_.string = "Tags: "
        _collapse_a_.string.insert_before(_collapse_a_a_)

        # collapse descs
        _collapse_b_ = soup.new_tag("p")
        _collapse_b_.string = _desc_
        _collapse_b_a_ = soup.new_tag("b")
        _collapse_b_a_.string = "Description: "
        _collapse_b_.string.insert_before(_collapse_b_a_)

        _collapse_.append(_collapse_a_)
        _collapse_.append(_collapse_b_)

        tr = soup.new_tag("tr")

        row_1 = soup.new_tag("td", attrs={"class": "collapsibleInfoName"})
        row_1.append(_botLink_)
        row_1.append(_collapse_)
        tr.append(row_1)

        row_2 = soup.new_tag("td", attrs={"class": "collapsibleInfo"})
        row_2.string = _author_
        tr.append(row_2)

        row_3 = soup.new_tag("td", attrs={"class": "collapsibleInfo"})
        row_3.string = _date_
        tr.append(row_3)

        row_4 = soup.new_tag("td", attrs={"class": "collapsibleInfo"})
        row_4.append(_download_)
        tr.append(row_4)


        div.insert(0, tr)
        print(soup)


    