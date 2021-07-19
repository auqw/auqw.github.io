
import os
import re
import json
import pyperclip

from github_contents import GithubContents

import asyncio
import aiosonic
from aiosonic.timeout import Timeouts


from discord.ext import commands
from bs4 import BeautifulSoup as Soup
from dotenv import load_dotenv

from pprintpp import pprint


class BaseProgram:


    github = ""

    def git_prepare(self):
        self.mode_list = ["boats", "settings"]

        self.env_variables()
        self.new_git()

        os.chdir(os.path.dirname(os.path.abspath(__file__)))
        os.chdir('..')
        # self.file_read("all")
        self.git_read("all")

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
        os.chdir(os.path.dirname(os.path.abspath(__file__)))
        load_dotenv()

        BaseProgram.GIT_REPOS = os.getenv('GITHUB_REPOS')
        BaseProgram.GIT_USER = os.getenv('GITHUB_USERNAME')
        BaseProgram.GIT_BLOOM_TOKEN = os.getenv('GITHUB_BLOOMBOT_TOKEN')
        BaseProgram.GITHUB_EMAIL = os.getenv('GITHUB_EMAIL')
        BaseProgram.GITHUB_BLOOMBOT_TOKEN = os.getenv('GITHUB_BLOOMBOT_TOKEN')


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

            content_in_bytes = BaseProgram.github.read(f"discord-bot/Data/{file}.json")[0]
            content_in_dict = json.loads(content_in_bytes.decode('utf-8'))
            
            # print(content_in_dict)
            setattr(BaseProgram, file, content_in_dict)

            print(f"> Finished reading {file}.json")
        return


    def git_save_html(self, git_data, message):
        """ Description: Saves data to github  /discord-bots/"""
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

    def combine_list(self, list_):
        if len(list_) > 1:
            return ', '.join(list_).strip()
        else:
            return str(list_[0])

    def update_portal(self):

        portal_html, sha = BaseProgram.github.read("index.html")
        soup = Soup(portal_html, 'html.parser')
        

        div = soup.find("div", {"id": "myModalBoats"}).find("table", {"id":"myTable"}).find("tbody")
        div.clear()

        for _botname_ in BaseProgram.boats:

            _date_ = BaseProgram.boats[_botname_]["date"]
            _author_ = self.combine_list(BaseProgram.boats[_botname_]["authors"])
            _tags_ = self.combine_list(BaseProgram.boats[_botname_]["tags"])
            _desc_ = BaseProgram.boats[_botname_]["description"].strip()

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
            if _desc_:
                _collapse_b_ = soup.new_tag("p")
                _collapse_b_.string = _desc_
                _collapse_b_a_ = soup.new_tag("b")
                _collapse_b_a_.string = "Description: "
                _collapse_b_.string.insert_before(_collapse_b_a_)

            _collapse_.append(_collapse_a_)

            if _desc_:
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
            print(f"> Done processing {_botname_}")
        # print()
        pyperclip.copy(soup.prettify())

        spam = pyperclip.paste()
        self.git_save_html(soup.prettify(), f"Regenerated the entire shit.")
        print("> Done updating...")




x = BaseProgram()
x.git_prepare()
x.update_portal()