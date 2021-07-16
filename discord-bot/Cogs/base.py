
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

from pprint import pprint



os.chdir(os.path.dirname(os.path.abspath(__file__)))
os.chdir('..')

class BaseProgram:
    # Base
    settings = {}
    priveleged_roles = []

    # BoatCog Vars
    database = {}
    author_list_lowercase = []
    
    # ClassCog Vars
    classes = {}
    class_acronyms = {}
    
    # GuideCog Vars
    guides = {}
    
    # ListenerCog Vars
    reddit_logs = {}
    twitter_logs = {}

    # FarmCog Vars
    streams = []
    status_list = []

    # TextCog Vars
    texts = {}

    # SWFCog Vars
    swf = {}

    mode = ""
    tweet_call = ""
    sqlock = False
    git_already = False
    database_updating = False
    twitter_updating = False

    tweet_user_list = []
    loop = asyncio.get_event_loop()

    asyncio.set_event_loop(loop)
    nest_asyncio.apply(loop)
    
    
    tweet_text = ""
    block_color = 3066993


    url = "https://adventurequest.life/"
    CONSUMER_KEY = ""
    CONSUMER_SECRET = ""
    ACCESS_TOKEN = ""
    ACCESS_TOKEN_SECRET = ""

    DISCORD_TOKEN = ""
    PERMISSIONS = ""
    PORTAL_AGENT = ""

    tweets_listener = ""

    lock_read = False

    icon_bloom = "https://cdn.discordapp.com/attachments/805367955923533845/813066459281489981/icon3.png"
    icon_aqw = "https://cdn.discordapp.com/attachments/805367955923533845/812991601714397194/logo_member.png"
    icon_auqw = "https://images-ext-2.discordapp.net/external/HYh_FWKYc_DqZZAmoIg1ZR0sMSB34aDf0YAFGGLFGSE/%3Fsize%3D1024/https/cdn.discordapp.com/icons/782192889723748362/a_d4c8307eb1dc364f207183a2ee144b4d.gif"
    icon_aqw_g = "https://cdn.discordapp.com/attachments/805367955923533845/813015948256608256/aqw.png"
    icon_google = "https://cdn.discordapp.com/attachments/805367955923533845/813340480330137650/google_chrome_new_logo-512.png"
    icon_spider = "https://cdn.discordapp.com/attachments/805367955923533845/828604515602399262/unknown.png"
    icon_4chan = "https://cdn.discordapp.com/attachments/805367955923533845/828655255821484072/J06iq9EtwExfgF05DQMlokwKnPnFnQRzEpFozGJWT2U.png"
    icon_maids = "https://cdn.discordapp.com/attachments/805367955923533845/828655455705890846/unknown.png"
    icon_dict = {
        "AutoQuestWorlds":icon_auqw,
        "FashionQuestWorlds":icon_aqw,
        "AQW":icon_aqw,
        "133sAppreciationClub":icon_spider,
        "maids": icon_maids,
        "4chan": icon_4chan,
    }


    icons = {
                "auqw": {
                    "title": "AutoQuest Worlds",
                    "icon": icon_auqw
                },
                "aqw": {
                    "title": "AdventureQuest Worlds",
                    "icon": icon_aqw
                }
            }

    usr_agent = {
            'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) '
                          'Chrome/61.0.3163.100 Safari/537.36'}


    tweet_user_list = [
            "16480141", # @Alina
            "135864340", # @Kotaro_AE
            "200782641", # @notdarkon
            "2435624982", # @asukaae
            "2615674874", # @yo_lae
            "989324890204327936", # @arletteaqw
            "1589628840", # @Psi_AE
            "1240767852321390592", # @aqwclass
            "2150245009", # @CaptRhubarb
            "360095665", # @ae_root
            "17190195", # @ArtixKrieger
        ]
    reddit_network = {}
    def git_prepare(self):
        self.mode_list = ["database", "guides", "classes", "settings", "texts", "boaters",
                    "streams", "reddit_logs", "twitter_logs"]

        self.env_variables()
        # self.og_git()
        self.new_git()

        os.chdir(os.path.dirname(os.path.abspath(__file__)))
        os.chdir('..')
        self.file_read("all")
        if not BaseProgram.lock_read:
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
                    "Bloom-Bot",
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
            BaseProgram.DISCORD_TOKEN = os.getenv('DISCORD_BOT_TOKEN2') # test bot token
            BaseProgram.GIT_REPOS = os.getenv('GITHUB_REPOS')
            BaseProgram.GIT_USER = os.getenv('GITHUB_USERNAME')
            BaseProgram.GIT_BLOOM_TOKEN = os.getenv('GITHUB_BLOOMBOT_TOKEN')
            BaseProgram.GITHUB_EMAIL = os.getenv('GITHUB_EMAIL')
            BaseProgram.PERMISSIONS = os.getenv("PRIVILEGED_ROLE").split(',')
            BaseProgram.PORTAL_AGENT = os.getenv('PORTAL_AGENT')

            BaseProgram.CONSUMER_KEY = os.getenv('TWITTER_NOTIFIER_API_KEY')
            BaseProgram.CONSUMER_SECRET = os.getenv('TWITTER_NOTIFIER_API_KEY_SECRET')
            BaseProgram.ACCESS_TOKEN = os.getenv('TWITTER_NOTIFIER_ACCESS_TOKEN')
            BaseProgram.ACCESS_TOKEN_SECRET = os.getenv('TWITTER_NOTIFIER_ACCESS_TOKEN_SECRET')

        else:              # Heroku
            BaseProgram.DISCORD_TOKEN = os.environ.get('DISCORD_BOT_TOKEN')
            BaseProgram.GIT_REPOS = os.environ.get('GITHUB_REPOS')
            BaseProgram.GIT_USER = os.environ.get('GITHUB_USERNAME')
            BaseProgram.GITHUB_EMAIL = os.environ.get('GITHUB_EMAIL')
            BaseProgram.GIT_BLOOM_TOKEN = os.environ.get('GITHUB_BLOOMBOT_TOKEN')
            BaseProgram.PERMISSIONS = os.environ.get("PRIVILEGED_ROLE").split(',')
            BaseProgram.PORTAL_AGENT = os.environ.get("PORTAL_AGENT")

            BaseProgram.CONSUMER_KEY = os.environ.get('TWITTER_NOTIFIER_API_KEY')
            BaseProgram.CONSUMER_SECRET = os.environ.get('TWITTER_NOTIFIER_API_KEY_SECRET')
            BaseProgram.ACCESS_TOKEN = os.environ.get('TWITTER_NOTIFIER_ACCESS_TOKEN')
            BaseProgram.ACCESS_TOKEN_SECRET = os.environ.get('TWITTER_NOTIFIER_ACCESS_TOKEN_SECRET')

    def database_update(self, mode: str):
        """ Description: Updates the database.json
            Arguments:
            [mode] accepts 'web', 'html'
                - 'web': scrapes directly from the BaseProgram.url
                - 'html': uses pre-downloaded html of BaseProgram.url
            Return: Bool
        """

        self.git_read("settings")
        self.file_clear_database()
        BaseProgram.database["sort_by_bot_name"] = {}
        BaseProgram.database["sort_by_bot_authors"] = {}

        if mode == "web":
            # try:
            headers = {
                'User-Agent': "DiscordBot"
            }
            row = []
            try:
                html = requests.get(BaseProgram.url, headers=headers).text
                page_soup = Soup(html, "html.parser")
                body = page_soup.find("table", {"id":"table_id", "class":"display"}).find("tbody")
                row_links = body.find_all("input", {"class":"rainbow"})

                for value in row_links:
                    link = BaseProgram.url + "bots/" + value["value"]
                    row.append(link)

                BaseProgram.settings["latest_update"] = "web"
                BaseProgram.mode = "web"
            except:
                self.git_read("database-settings")
                return False

        elif mode == "html":
            if BaseProgram.settings["latest_update"] == "web":
                # Checks if the latest update method is web, i.e. the latest most way
                # of updating this.
                print("Didn't update. latest update is Web")
                self.git_read("database-settings")
                return False
            else:
                BaseProgram.settings["latest_update"] = "html"
                BaseProgram.mode = "html"
            try:
                soup = Soup(open("./Data/html/aqw.html", encoding="utf8"), "html.parser")
                body = soup.find("table", {"id":"table_id", "class":"display"}).find("tbody")
                row = body.find_all("tr")
            except:
                self.git_read("database-settings")
                return False
        
        for raw_link in row:
            if mode == "web": link = raw_link
            elif mode == "html": link = raw_link.find("td").find("a")["href"]

            item_name = link.split("/")[-1]
            raw_author = item_name
            # Code for finding the bot author. This didn't work easily.
            try:
                raw_data = re.match("(^[a-zA-Z0-9]+[_|-])", item_name)
                raw_author = (re.sub("_|-", "", raw_data[0])).lower()
            except: 
                pass
            if raw_author in BaseProgram.settings["confirmed_authors"]:
                bot_author = raw_author
            else:
                raw_author = item_name
                try:
                    for verified_author in BaseProgram.settings["confirmed_authors"]:
                        for alias in BaseProgram.settings["confirmed_authors"][verified_author]["alias"]:
                            if alias in raw_author:
                                bot_author = verified_author
                                raise BreakProgram
                            else:   # inefficient shit. but it works
                                bot_author = "Unknown"
                except:
                    pass

            # Code for refining bot name.
            if bot_author != "Unknown":
                bot_name = item_name
                alias = [alias for alias in BaseProgram.settings["confirmed_authors"][bot_author]["alias"]]
                for author_nickname in alias:
                    author_replacement = [author_nickname.lower(), author_nickname.capitalize()]
                    for name in author_replacement:
                        bot_name = bot_name.replace(name, "").lstrip()
            else:
                bot_name = item_name
            bot_name = re.sub(r"_|-", " ", bot_name).lstrip().replace("%", "")  # replaces "_" & "-" with spaces
            bot_name = re.sub(r"\s\s\.|\s\.", ".", bot_name)# removes space between file format
            bot_name = re.sub(r"\s\s", " ", bot_name)       # replaces "  " double spaces into single space
            bot_name = re.sub(r"^[s\s]", "", bot_name)      # removes "s " from the name.
            bot_name = re.sub("Non\s|NON\s", "Non-", bot_name)  # replaces "non mem" to "non-mem"
            bot_name = bot_name.lstrip().rstrip()
            bot_author = bot_author.lower()
            BaseProgram.database["sort_by_bot_name"][bot_name] = {}
            BaseProgram.database["sort_by_bot_name"][bot_name]["url"] = link
            BaseProgram.database["sort_by_bot_name"][bot_name]["author"] = bot_author

            # Sort by Author
            if bot_author not in BaseProgram.database["sort_by_bot_authors"]:
                BaseProgram.database["sort_by_bot_authors"][bot_author] = {}
                
            BaseProgram.database["sort_by_bot_authors"][bot_author][bot_name] = {}
            BaseProgram.database["sort_by_bot_authors"][bot_author][bot_name]["url"] = link


        # Saving
        self.file_save("database-settings")
        self.git_save("database-settings")
        print("========DATABASE===========")
        print("===================")
        print("lmao")
        return True

    def file_clear_database(self):
        """Description: Clears the database.json"""
        with open('./Data/database.json', 'w', encoding='utf-8') as f:
            json.dump({}, f, ensure_ascii=False, indent=4)

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
            if file == "classes":
                self.sort_classes_acronym()
            if file == "data" or file == "settings":
                self.sort_privileged_roles()
                self.sort_author_list_lowercase()

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
                    filepath=f"Data/{file}.json",
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

    def git_save_swf(self):
        """ Description: Saves data to github .json files
            Arguments:
                [mode] - checks to do. accepts: database, guides, settings, classes
                         or any of the their combination delimited by "-"
                    - 'database'> BaseProgram.database
                    - 'guides'> BaseProgram.guides
                    - 'settings'> BaseProgram.settings
                    - 'classes'> BaseProgram.classes
        """
        # with open(f'./Data/swf.json', 'r', encoding='utf-8') as f:
        #     json.dumps(BaseProgram.swf)
        #     print(f)
        #     print(type(f))
        git_data = bytearray(BaseProgram.swf)
        self.file_save("swf")
        print("yeah")
        try:
            content_sha, commit_sha = BaseProgram.github.write(
                filepath=f"Data/{file}.json",
                content_bytes=git_data,
                commit_message=f"{file} updated",
                committer={
                    "name": BaseProgram.GIT_USER,
                    "email": BaseProgram.GITHUB_EMAIL,
                },
            )
        except Exception as e:
            print(f"> {e}")

        
            
        # return

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

            content_in_bytes = BaseProgram.github.read(f"Data/{file}.json")[0]
            content_in_dict = json.loads(content_in_bytes.decode('utf-8'))
            
            # print(content_in_dict)
            setattr(BaseProgram, file, content_in_dict)


            if file == "classes":
                self.sort_classes_acronym()
            if file == "data" or file == "settings":
                self.sort_privileged_roles()
                self.sort_author_list_lowercase()
            self.file_save(file)
            print(f"> Finished reading {file}.json")
        return
