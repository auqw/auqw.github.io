from .Base import *
from discord.ext import commands
from discord.utils import get

from pprintpp import pprint
from datetime import date

import discord
import re

class UploadCog(commands.Cog, BaseProgram):

    def __init__(self, bot):
        self.bot = bot
        pass
    @commands.command()
    async def clearance(self, ctx, user: discord.User):
        if ctx.author.id != 252363724894109700:
            await ctx.send(f"\> BTFO you're not <@252363724894109700>")
            return

        try:
            id_ = user.id
        except:
            await ctx.send(f"\> Please enter a name. cmd form: `;verify @discord_profile`")
            return

        if str(user.id) in BaseProgram.settings["clearance"]:
            await ctx.send(f"\> User `{user.name}` already has clearance. ")
            return
        BaseProgram.settings["clearance"].append(str(user.id))

        self.git_save("settings")
        print(BaseProgram.settings)
        await ctx.send(f"\> Successfully gave clearance to `{user.name}`")

    @commands.command()
    async def verify(self, ctx, name: str = "", user: discord.Member = ""):
        verifyAuthority = [252363724894109700, 781102368787791903]
        if ctx.author.id not in verifyAuthority:
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

        if str(user.id) in BaseProgram.settings["verified_list"]:
            # await ctx.send("\> User already verified, you oaf.")
            # return
            pass
        else:
            BaseProgram.settings["verified_list"][user.id] = name
            if name not in BaseProgram.settings["verified_namelist"]:
                BaseProgram.settings["verified_namelist"].append(name)
            self.git_save("settings")
            self.git_read("settings")
            print(BaseProgram.settings)

        # if os.name == "nt":
        #     guild = self.bot.get_guild(761956630606250005)
        #     role1 = guild.get_role(856385548675317821)
        #     await user.add_roles(role1)
        # else:
            
        guild_gang = self.bot.get_guild(848944006641877023)
        role_gang = guild_gang.get_role(877551430100209755)
        await user.add_roles(role_gang)

        # guild_harbor = self.bot.get_guild(811305081063604284)
        # role_harbor = guild_harbor.get_role(811305081097814073)
        # await user.add_roles(role_harbor)

        await ctx.send(f"\> Successfully verified `{name}`")

    @commands.command()
    async def uploadtest(self, ctx, *, result:str=""):
        res = result.split("-")
        for i in res:
            await ctx.send(i)
            print(i)

    @commands.command()
    async def upload(self, ctx):
        await ctx.send("The command `;upload` no longer works and is replaced by `;uploadr rbot_name` and `;uploadg gbot_name`.")
        return

    @commands.command()
    async def uploadg(self, ctx, *, result:str=""):
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

        result = result.split("-")
        if len(result) >= 1:
            tags_ = result[0].title().strip()
        else:
            tag_ = ""

        if len(result) >=2:
            desc = result[1].strip()
        else:
            desc = ""


        author = [BaseProgram.settings["verified_list"][str(ctx.author.id)]]

        rejected_author = []
        date_ = date.today().strftime("%d %b %Y")


        if len(result) > 3:
            await ctx.send("\> You used more than 3 delimeter (-). cmd form: `;upload tags, tags... - description - @author2, @author3`")
            return
        # print("this: ", result[2])
        if len(result) == 3:
            other_authors = [x.strip().lower() for x in result[2].split(",")]

            for ver_author in BaseProgram.settings["verified_namelist"]:
                if (ver_author.lower() in other_authors) and (ver_author not in author):
                    author.append(ver_author)


        exists_already = ""
        if botName in BaseProgram.gbots:
            exists_already = "\n\> Upload Bot overwrite existing bot."

        BaseProgram.gbots[botName] = {}
        BaseProgram.gbots[botName]["date"] = date_
        if len(author) > 1:
            author_joined = ', '.join(author)
        else:
            author_joined = str(author[0]).strip()
        BaseProgram.gbots[botName]["authors"] = author
        BaseProgram.gbots[botName]["tags"] = [x.title().strip() for x in tags_.strip().split(", ")]
        BaseProgram.gbots[botName]["description"] = desc
        print(author)
        # return
        # if not BaseProgram.debug:
        self.git_save_bots("gbots", data, botName, author_joined)
        self.git_save("gbots")

        await ctx.send(f"\> Successfully Uploaded gbot: `{botName}` by {author_joined}{exists_already}.")
        return

    @commands.command()
    async def uploadr(self, ctx, *, result:str=""):
        if str(ctx.author.id) not in BaseProgram.settings["verified_list"]:
            await ctx.send("\> You are not a verified boat maker")
            return

        try:
            attach = ctx.message.attachments[0]
        except:
            await ctx.send("\> Please attach a .cs, .zip, .rar file.")
            return

        botName = attach.filename
        if botName.split(".")[-1].lower() not in ["cs", "zip", "rar"]:
            await ctx.send("\> Only a .cs, .zip, and .rar files are allowed.")
            return

        await ctx.send("\> Processing file please wait.")

        data = await self.get_item_content(URL=attach.url, is_soup=False, encoding="cp1252")

        result = result.split("-")
        if len(result) >= 1:
            tags_ = result[0].title().strip()
        else:
            tag_ = ""

        if len(result) >=2:
            desc = result[1].strip()
        else:
            desc = ""


        author = [BaseProgram.settings["verified_list"][str(ctx.author.id)]]

        rejected_author = []
        date_ = date.today().strftime("%d %b %Y")


        if len(result) > 3:
            await ctx.send("\> You used more than 3 delimeter (-). cmd form: `;upload tags, tags... - description - @author2, @author3`")
            return
        # print("this: ", result[2])
        if len(result) == 3:
            other_authors = [x.strip().lower() for x in result[2].split(",")]

            for ver_author in BaseProgram.settings["verified_namelist"]:
                if (ver_author.lower() in other_authors) and (ver_author not in author):
                    author.append(ver_author)


        if len(author) > 1:
            author_joined = ', '.join(author)
        else:
            author_joined = str(author[0]).strip()
        

        exists_already = ""
        if botName in BaseProgram.rbots:
            exists_already = "\n\> Upload Bot overwrite existing bot."
            if author:
                BaseProgram.rbots[botName]["authors"] = author
            if tags_:
                BaseProgram.rbots[botName]["tags"] = [x.title().strip() for x in tags_.strip().split(", ")]
            if desc:
                BaseProgram.rbots[botName]["description"] = desc
        else:
            BaseProgram.rbots[botName] = {}
            BaseProgram.rbots[botName]["authors"] = author
            BaseProgram.rbots[botName]["tags"] = [x.title().strip() for x in tags_.strip().split(", ")]
            BaseProgram.rbots[botName]["description"] = desc


        BaseProgram.rbots[botName]["date"] = date_

        # return
        self.git_save_bots("rbots", data, botName, author_joined)
        self.git_save("rbots")

        await ctx.send(f"\> Successfully Uploaded rbot: `{botName}` by {author_joined}.{exists_already}")
        return


    @commands.command()
    async def delete(self, ctx):
        await ctx.send("The command `;delete` no longer works and is replaced by `;deleter rbot_name` and `;deleteg gbot_name`.")
        return

    @commands.command()
    async def deleteg(self, ctx, *, botName):
        botName = botName.strip()
        user = self.clean_char(ctx.author.id)

        if user not in BaseProgram.settings["verified_list"]:
            await ctx.send("\> Sorry. User is not a verified boat maker.")
            return

        if botName not in BaseProgram.gbots:
            await ctx.send("\> The gbot you passed cannot be deleted: `Does not Exists`.")
            return

        if user not in BaseProgram.settings["clearance"]:
            if BaseProgram.settings["verified_list"][user] not in BaseProgram.gbots[botName]["authors"]:
                if len(BaseProgram.gbots[botName]["authors"]) > 1:
                    await ctx.send("\> You're not one of the bot's authors.")
                    return
                await ctx.send("\> Error. You're not the author of this bot.")
                return


        await ctx.send("\> Processing deletion. Please wait...")


        if len(BaseProgram.gbots[botName]["authors"]) > 1:
            author_joined = ', '.join(BaseProgram.gbots[botName]["authors"]).strip()
        else:
            author_joined = str(BaseProgram.gbots[botName]["authors"][0]).strip()

        BaseProgram.gbots.pop(botName, None)

        self.git_save("gbots")

        await ctx.send(f"\> Successfully deleted `{botName}` by {author_joined}.")
        return

    @commands.command()
    async def deleter(self, ctx, *, botName):
        botName = botName.strip()
        user = self.clean_char(ctx.author.id)

        if user not in BaseProgram.settings["verified_list"]:
            await ctx.send("\> Sorry. User is not a verified boat maker.")
            return

        if botName not in BaseProgram.rbots:
            await ctx.send("\> The rbot you passed cannot be deleted: `Does not Exists`.")
            return

        if user not in BaseProgram.settings["clearance"]:
            if BaseProgram.settings["verified_list"][user] not in BaseProgram.rbots[botName]["authors"]:
                if len(BaseProgram.rbots[botName]["authors"]) > 1:
                    await ctx.send("\> You're not one of the bot's authors.")
                    return
                await ctx.send("\> Error. You're not the author of this bot.")
                return


        await ctx.send("\> Processing deletion. Please wait...")


        if len(BaseProgram.rbots[botName]["authors"]) > 1:
            author_joined = ', '.join(BaseProgram.rbots[botName]["authors"]).strip()
        else:
            author_joined = str(BaseProgram.rbots[botName]["authors"][0]).strip()

        BaseProgram.rbots.pop(botName, None)

        self.git_save("rbots")

        await ctx.send(f"\> Successfully deleted `{botName}` by {author_joined}.")
        return


    # @commands.command()
    # async def massdelete(self, ctx, *, botName):
    #     botNames = [x.strip() for x in botName.split(",")]
    #     user = self.clean_char(ctx.author.id)

    #     if user not in BaseProgram.settings["verified_list"]:
    #         await ctx.send("\> Sorry. User is not a verified boat maker.")
    #         return

    #     if not botNames:
    #         await ctx.send("\> Enter bot names (rbot or gbot) separated by comma. ( , )")
    #         return

    #     gbot_lists = []
    #     rbot_lists = []
    #     for botName in botNames:
    #         if botName in BaseProgram.gbots:
    #             BaseProgram.gbots.pop(botName, None)
    #             gbot_lists.append(botName)
    #             continue

    #         if botName in BaseProgram.rbots:
    #             BaseProgram.rbots.pop(botName, None)
    #             rbot_lists.append(botName)

    #     if gbot_lists:
    #         self.git_save("gbots")
    #     if rbot_lists:
    #         self.git_save("rbots")



    #     await ctx.send(f"\> Successfully mass-deleted the following.```{gbot_lists.join(", ")}\n{rbot_lists.join(", ")}```")
    #     return

    def clean_char(self, id_):
        return re.sub("[<@!>]", "", str(id_)).strip()
