
import os
import re
import json
import github3
from github_contents import GithubContents
import requests
import asyncio
import aiosonic
from aiosonic.timeout import Timeouts
import aiohttp
import discord
import nest_asyncio
import html5lib

from discord.ext import commands
from bs4 import BeautifulSoup as Soup
from dotenv import load_dotenv

from pprintpp import pprint



os.chdir(os.path.dirname(os.path.abspath(__file__)))
os.chdir('..')

class BaseProgram:

    icon_auqw = "https://images-ext-2.discordapp.net/external/HYh_FWKYc_DqZZAmoIg1ZR0sMSB34aDf0YAFGGLFGSE/%3Fsize%3D1024/https/cdn.discordapp.com/icons/782192889723748362/a_d4c8307eb1dc364f207183a2ee144b4d.gif"

    loop = asyncio.get_event_loop()

    asyncio.set_event_loop(loop)
    nest_asyncio.apply(loop)

    # Read the data = True, Dont read = False
    read_true = True
    github = ""
    block_color = 16727357

    def git_prepare(self):
        self.mode_list = ["boats", "settings"]

        self.env_variables()
        # self.og_git()
        self.new_git()

        os.chdir(os.path.dirname(os.path.abspath(__file__)))
        os.chdir('..')
        # self.file_read("all")
        if BaseProgram.read_true:
            self.git_read("all")

    def og_git(self):
        while True:
            try:
                BaseProgram.github = github3.login(token=BaseProgram.GIT_BLOOM_TOKEN)
                BaseProgram.repository = BaseProgram.github.repository(BaseProgram.GIT_USER, BaseProgram.GIT_REPOS)
                print("> Github Connection...Success!")
                break
            except: 
                print("> Failed Connecting to Github... Trying again.")
                print("> Reconnecting...")
                continue


    def new_git(self):
        while True:
            try:
                print("> Starting Github Connection...")
                BaseProgram.github = GithubContents(
                    BaseProgram.GIT_USER,
                    BaseProgram.GIT_REPOS,
                    token=BaseProgram.GIT_BLOOM_TOKEN,
                    branch="master"
                )
                print("> Github Connection...Success!")
                break
            except:
                print("> Failed Connecting to Github... Trying again.")
                print("> Reconnecting...")
                continue

    def env_variables(self):
        if os.name == "nt": # PC Mode

            os.chdir(os.path.dirname(os.path.abspath(__file__)))
            load_dotenv()

            BaseProgram.GIT_REPOS = os.getenv('GITHUB_REPOS')
            BaseProgram.GIT_USER = os.getenv('GITHUB_USERNAME')
            BaseProgram.GIT_BLOOM_TOKEN = os.getenv('GITHUB_BLOOMBOT_TOKEN')
            BaseProgram.GITHUB_EMAIL = os.getenv('GITHUB_EMAIL')
            BaseProgram.GITHUB_BLOOMBOT_TOKEN = os.getenv('GITHUB_BLOOMBOT_TOKEN')

        else:              # Heroku
            BaseProgram.GIT_REPOS = os.environ.get('GITHUB_REPOS')
            BaseProgram.GIT_USER = os.environ.get('GITHUB_USERNAME')
            BaseProgram.GITHUB_EMAIL = os.environ.get('GITHUB_EMAIL')
            BaseProgram.GIT_BLOOM_TOKEN = os.environ.get('GITHUB_BLOOMBOT_TOKEN')
            BaseProgram.GITHUB_BLOOMBOT_TOKEN = os.environ.get('GITHUB_BLOOMBOT_TOKEN')

    def file_read(self, mode):
        """ Description: Reads data from local .json files
            Arguments:
                [mode] - checks to do. accepts: database, guides, settings, classes
                         or any of the their combination delimited by "-"
                    - 'database'> BaseProgram.database
                    - 'guides'> BaseProgram.guides
                    - 'settings'> BaseProgram.settings
                    - 'classes'> BaseProgram.classes
        """

        mode = mode.split("-")
        if mode == ["all"]:
            mode = self.mode_list

        for file in mode:
            with open(f'./Data/{file}.json', 'r', encoding='utf-8') as f:
                setattr(BaseProgram, file, json.load(f))


    def file_save(self, mode:str):
        """ Description: Saves data to local .json files
            Arguments:
                [mode] - checks to do. accepts: database, guides, settings, classes
                         or any of the their combination delimited by "-"
                    - 'database'> BaseProgram.database
                    - 'guides'> BaseProgram.guides
                    - 'settings'> BaseProgram.settings
                    - 'classes'> BaseProgram.classes
        """

        mode = mode.split("-")
        if mode == ["all"]:
            mode = self.mode_list
        for file in mode:
            with open(f'./Data/{file}.json', 'w', encoding='utf-8') as f:
                json.dump(getattr(BaseProgram, file), f, ensure_ascii=False, indent=4)


    def git_save(self, mode:str):
        """ Description: Saves data to github .json files
            Arguments:
                [mode] - checks to do. accepts: database, guides, settings, classes
                         or any of the their combination delimited by "-"
                    - 'database'> BaseProgram.database
                    - 'guides'> BaseProgram.guides
                    - 'settings'> BaseProgram.settings
                    - 'classes'> BaseProgram.classes
        """
        mode = mode.split("-")
        if mode == ["all"]:
            mode = self.mode_list

        for file in mode:
            git_data = json.dumps(getattr(BaseProgram, file), indent=4).encode('utf-8')
            # contents_object = BaseProgram.repository.file_contents(f"./Data/{file}.json")
            # contents_object.update(f"{file} updated", git_data)
            self.file_save(file)
            print("yeah")
            try:
                content_sha, commit_sha = BaseProgram.github.write(
                    filepath=f"lib/Data/{file}.json",
                    content_bytes=git_data,
                    commit_message=f"{file} updated",
                    committer={
                        "name": BaseProgram.GIT_USER,
                        "email": BaseProgram.GITHUB_EMAIL,
                    },
                )
            except Exception as e:
                print(f"> {e}")
            
        return



    def git_read(self, mode:str):
        """ Description: Reads data from github .json files
            Arguments:
                [mode] - checks to do. accepts: database, guides, settings, classes
                         or any of the their combination delimited by "-"
                    - 'database'> BaseProgram.database
                    - 'guides'> BaseProgram.guides
                    - 'settings'> BaseProgram.settings
                    - 'classes'> BaseProgram.classes
        """
        mode = mode.split("-")
        if mode == ["all"]:
            mode = self.mode_list

        for file in mode:
            if file == "update":
                continue
            # git_data = BaseProgram.repository.file_contents(f"./Data/{file}.json").decoded
            # setattr(BaseProgram, file, json.loads(git_data.decode('utf-8')))

            print(f"file: {file}")

            content_in_bytes = BaseProgram.github.read(f"lib/Data/{file}.json")[0]
            content_in_dict = json.loads(content_in_bytes.decode('utf-8'))
            
            # print(content_in_dict)
            setattr(BaseProgram, file, content_in_dict)

            self.file_save(file)
            print(f"> Finished reading {file}.json")
        return

    def git_save_bots(self, git_data, fileName, author):
        """ Description: Saves data to github  /libs/
        """
        try:
            content_sha, commit_sha = BaseProgram.github.write(
                filepath=f"bots/{fileName}",
                content_bytes=git_data,
                commit_message=f"Added {fileName} by {author}",
                committer={
                    "name": BaseProgram.GIT_USER,
                    "email": BaseProgram.GITHUB_EMAIL,
                },
            )
        except Exception as e:
            print(f"> {e}")
            
        return


    def git_save_html(self, git_data, message):
        """ Description: Saves data to github  /libs/
        """
        try:
            content_sha, commit_sha = BaseProgram.github.write(
                filepath=f"index.html",
                content_bytes=str.encode(str(git_data)),
                commit_message=message,
                committer={
                    "name": BaseProgram.GIT_USER,
                    "email": BaseProgram.GITHUB_EMAIL,
                },
            )
        except Exception as e:
            print(f"> {e}")
            
        return

    async def get_site_content(self, URL:str,  mode="aisonic", name="content_get", 
                is_soup:bool=True, parser="html5lib", encoding="utf-8", headers={},
                handle_cookies=False):
        # cp1252
        # client = aiosonic.HTTPClient(handle_cookies=handle_cookies)
        # response = await client.request(URL, headers=headers)
        # print("RESP: ", response)
        # text_ = await response.content()
        # print(f"> Function {name} executed...Success!")
        # if is_soup:
        #     return Soup(text_.decode(encoding), parser)
        # else:
        #     return text_.decode(encoding)
        timeouts = Timeouts(
            sock_read=2,
            # sock_connect=timeout["sock_connect"],
            # pool_acquire=timeout["pool_acquire"],
            # request_timeout=timeout["request_timeout"],
        )
        if mode == "aisonic":
            while True:
                try:
                    client = aiosonic.HTTPClient(handle_cookies=handle_cookies)
                    response = await client.get(URL, headers=headers)

                    text_ = await response.content()

                    print(f"> Function {name} executed...Success!")
                    if is_soup:
                        return Soup(text_.decode(encoding), parser)
                    else:
                        # print(text_)
                        return text_.decode(encoding)
                except:
                    print(f"> Failed Executing {name}... Trying again.")
                    continue

        elif mode == "aiohttp":
            while True:
                print("> Reloading...")
                try:
                    async with aiohttp.ClientSession(trust_env=True) as session:
                        async with session.get(URL, headers=headers) as response:
                            text_ = await response.read()
                            if is_soup:
                                return Soup(text_.decode(encoding), parser)
                            else:
                                return text_.decode(encoding)
                except:
                    print(f"> Failed Executing {name}... Trying again.")
                    continue


    async def get_item_content(self, URL:str, name="content_get", 
                is_soup:bool=True, parser="html5lib", encoding="utf-8", headers={},
                handle_cookies=False):

        while True:
            try:
                client = aiosonic.HTTPClient(handle_cookies=handle_cookies)
                response = await client.get(URL, headers=headers)

                return await response.content()

            except:
                print(f"> Failed Executing {name}... Trying again.")
                continue



class BaseCog(commands.Cog, BaseProgram):
    def __init__(self, bot):
        self.bot = bot
        
    @commands.command()
    async def ahelp(self, ctx):
        embedVar = discord.Embed(title="Command Display", color=BaseProgram.block_color)
        desc = "`;ahelp` ➣ Shows all AuQW Bot Integration commands.\n"\
               "`;delete bot_name.gbot` ➣ Delete a bot associated with you.\n\n"\
               "**Upload Command (Must include bot):**\n"\
               "`;upload tag1, tag2, tag3, etc., - description... - @author2, @author3`\n\n"\
               " - Bot name is file name\n"\
               " - tag and author, delimeted by ` , `\n"\
               " - tag, author, desc, separated by ` - `\n"\
               " - Only accepts .gbot, .zip, and .rar files\n"

        desc2 = "**Example 1:**\n"\
               "➣ `;upload`\n"\
               "➣ __Result__: No tags and no description.\n\n"\
               "**Example 2:**\n"\
               "➣ `;upload Bludrut, Arena, Legion`\n"\
               "➣ __Result__: Bot with tags but no description.\n\n"\
               "**Example 3:**\n"\
               "➣ `;upload Reputation, Story, Grimlite - This bot will Farm Arcangrove`\n"\
               "➣ __Result__: Bot with tag, description and registed author name.\n\n"\
               "**Example 4:\n**"\
               "➣ Bloom uploads a bot and is in Collaboration with Weeb.\n"\
               "➣ `;upload Seks, Story, Grimlite - This bot will seks grimlite - @Weeb`\n"\
               "➣ __Result__: Bot with tags, desc, and multiple authors."
        embedVar.description = desc
        embedVar.add_field(name="\u200b", inline=False, value=desc2)
       

        embedVar.set_author(name="The AutoQuest World's Integration Bot", icon_url=BaseProgram.icon_auqw)
        # embedVar.set_thumbnail(url="https://cdn.discordapp.com/attachments/805367955923533845/866632546854240266/harbor2_-_Copy.jpg")
        await ctx.send(embed=embedVar)
        return