from .Base import *
from discord.ext import commands

from pprintpp import pprint
from datetime import date
from bs4 import BeautifulSoup as Soup
import discord
import re

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
    async def uploadtest(self, ctx, *, result:str=""):
        res = result.split("-")
        for i in res:
            await ctx.send(i)
            print(i)

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

        data = await self.get_item_content(URL=attach.url, is_soup=False, encoding="cp1252")

        del attach

        result = result.split("-")
        tags_ = result[0].title().strip()
        desc = result[1].strip()
        author = [BaseProgram.settings["verified_list"][str(ctx.author.id)]]

        rejected_author = []
        date_ = date.today().strftime("%d %b %Y")


        if len(result) > 3:
            await ctx.send("\> You used more than 3 delimeter (-). cmd form: `;upload tags, tags... - description - @author2, @author3`")
            return
        print("this: ", result[2])
        if len(result) == 3:
            other_authors = [self.clean_char(x) for x in result[2].strip().split("<@")][1:]

            for othr_author in other_authors:
                if othr_author in BaseProgram.settings["verified_list"]:
                    if othr_author == self.clean_char(ctx.author.id):
                        print(f"other: {othr_author}\tauthor: {author[0]}")
                        continue
                    author.append(BaseProgram.settings["verified_list"][othr_author])
                else:
                    rejected_author.append(f"<@{othr_author.strip()}>")
            del other_authors
        del result
        print("Deez: ",rejected_author)
        exists_already = ""
        if botName in BaseProgram.boats:
            exists_already = "\> Upload Bot overwrite existing bot.\n"

        BaseProgram.boats[botName] = {}
        BaseProgram.boats[botName]["date"] = date_
        if len(author) > 1:
            author = ', '.join(author)
        else:
            author = str(author[0]).strip()
        BaseProgram.boats[botName]["authors"] = author
        BaseProgram.boats[botName]["tags"] = tags_
        BaseProgram.boats[botName]["description"] = desc

        # return
        self.git_save_bots(data, botName, author)
        self.git_save("boats")

        await self.update_portal(botName, date_, author, tags_, desc, exists_already)

        await ctx.send(f"\> Done uploading: `{botName}`.\n{exists_already}\> Please wait 10s-30s for the Portal to update.")
        if rejected_author:
            await ctx.send(f"\> The following author/s were rejected due to being unverified:\n {' '.join(rejected_author)}")
        return

    async def update_portal(self, _botname_, _date_, _author_, _tags_, _desc_, _exists_already_):

        portal_html, sha = BaseProgram.github.read("index.html")

        # root = lh.tostring(sliderRoot) #convert the generated HTML to a string
        soup = Soup(portal_html, 'html.parser')                #make BeautifulSoup
        
        # pprint(prettyHTML)

        # redundancy check


        div = soup.find("div", {"id": "myModalBoats"}).find("table", {"id":"myTable"}).find("tbody")

        if _exists_already_:
            bot = div.find_all("tr", {"id": _botname_})
            for bt in bot:
                bt.decompose()

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

        tr = soup.new_tag("tr", attrs={"name": _author_, "id": _botname_})

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
        # prettyHTML = 
        self.git_save_html(soup.prettify(), _botname_, _author_)


    def clean_char(self, id_):
        return re.sub("[<@!>]", "", str(id_)).strip()