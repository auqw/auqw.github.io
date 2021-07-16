import discord
import os

from Cogs.Base import *
from Cogs.UploadCog import UploadCog


os.chdir(os.path.dirname(os.path.abspath(__file__)))


if os.name == "nt":
    load_dotenv()
    BaseProgram.tweet_user = "1349290524901998592"
    DISCORD_TOKEN = os.getenv('DISCORD_BOT_TOKEN2') # test bot token
    CLIEND_ID = os.getenv("DISCORD_CLIENT_ID2")
    DEPLOY_NAME = os.getenv("DEPLOY_NAME")
    cmd = "'"
else:
    BaseProgram.tweet_user = "16480141"
    DISCORD_TOKEN = os.environ.get('DISCORD_BOT_TOKEN')
    CLIEND_ID = os.environ.get("DISCORD_CLIENT_ID")
    DEPLOY_NAME = os.environ.get("DEPLOY_NAME")
    cmd = ";"