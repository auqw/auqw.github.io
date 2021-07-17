from .Base import *
from discord.ext import commands

from pprintpp import pprint
from datetime import date
from bs4 import BeautifulSoup as Soup
import discord

class UploadCog(commands.Cog, BaseProgram):

    def __init__(self, bot):
        self.bot = bot
        pass

    @commands.command()
    async def verify(self, ctx, name: str, user: discord.User):
        if ctx.author.id != 252363724894109700:
            await ctx.send(f"\> BTFO you're not <@252363724894109700>")
            return

        if not name:
            await ctx.send(f"\> Please enter a name. cmd form: `;verify name, @discord_profile`")
            return

        try:
            id_ = user.id
        except:
            await ctx.send("\> Please mention a real discord user. cmd form: `;verify name, @discord_profile`")
            return

        BaseProgram.settings["verified_list"][user.id] = name 
        self.git_save("settings")
        print(BaseProgram.settings)


    @commands.command()
    async def upload(self, ctx, *, result:str=""):
        if str(ctx.author.id) not in BaseProgram.settings["verified_list"]:
            await ctx.send("\> You are not a verified boat maker")
            return

        try:
            attach = ctx.message.attachments[0]
        except:
            await ctx.send("\> Please attach a .gbot, .zip, .rar file.")
            return

        botName = attach.filename
        if botName.split(".")[-1].lower() not in ["gbot", "zip", "rar"]:
            await ctx.send("\> Only a .gbot, .zip, and .rar files are allowed.")
            return  

        await ctx.send("\> Processing file please wait.")
        target_url = attach.url
        print(target_url)
                

        data = await self.get_item_content(URL=target_url, is_soup=False, encoding="cp1252")

        result = result.split("-")
        tags_ = result[0].title().strip()
        author = BaseProgram.settings["verified_list"][str(ctx.author.id)]

        self.git_save_bots(data, botName, author)

        
        await self.update_portal(botName, author, tags_, result[1].strip())
        await ctx.send(f"\> Done uploading `{botName}` to the Portal.\n\> Please wait 10s-30s for the Portal to update.")
        return

    async def update_portal(self, _botname_, _author_, _tags_, _desc_):

        portal_html, sha = BaseProgram.github.read("index.html")

        # root = lh.tostring(sliderRoot) #convert the generated HTML to a string
        soup = Soup(portal_html, 'html.parser')                #make BeautifulSoup
        # prettyHTML = soup.prettify()
        # pprint(prettyHTML)

        div = soup.find("div", {"id": "myModalBoats"}).find("table", {"id":"myTable"}).find("tbody")

        _date_ = date.today().strftime("%d %b %Y")

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
        
        self.git_save_html(soup, _botname_, _author_)


    