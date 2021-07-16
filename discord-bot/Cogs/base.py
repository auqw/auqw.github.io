
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

    tweet_user_list = []
    loop = asyncio.get_event_loop()

    asyncio.set_event_loop(loop)
    nest_asyncio.apply(loop)

    # Read the data = True, Dont read = False
    read_true = True

    def git_prepare(self):
        self.mode_list = ["boats"]

        self.env_variables()
        # self.og_git()
        self.new_git()

        os.chdir(os.path.dirname(os.path.abspath(__file__)))
        os.chdir('..')
        self.file_read("all")
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
