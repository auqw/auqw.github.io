
import os
import discord

from discord.ext import tasks
from discord import Intents
from discord.ext.commands import CommandNotFound
from pprint import pprint
from datetime import datetime
from pytz import timezone

from lib.Cogs.Base import *
from lib.Cogs.UploadCog import UploadCog

os.chdir(os.path.dirname(os.path.abspath(__file__)))
print("Current Working Dir:", os.curdir)

if os.name == "nt":
    load_dotenv()
    DISCORD_TOKEN = os.getenv('DISCORD_BOT_TOKEN2') # test bot token
    CLIEND_ID = os.getenv("DISCORD_CLIENT_ID2")
    DEPLOY_NAME = os.getenv("DEPLOY_NAME")
    cmd = "'"
else:
    DISCORD_TOKEN = os.environ.get('DISCORD_BOT_TOKEN')
    CLIEND_ID = os.environ.get("DISCORD_CLIENT_ID")
    DEPLOY_NAME = os.environ.get("DEPLOY_NAME")
    cmd = ";"

intents = Intents.all()
intents.presences = True
Bot = commands.Bot(command_prefix=[cmd], description='Bloom Bot Revamped', intents=intents)
Bot.remove_command('help')

BaseStuff = BaseProgram()
BaseStuff.git_prepare()


@Bot.event
async def on_ready():
    print('> Starting AUQW')
    
    await Bot.wait_until_ready()
    deploy_notif = await Bot.fetch_channel(830702959679373352)

    now = datetime.now(timezone('Asia/Manila'))
    current_time = now.strftime("%d %B %Y, %a | %I:%M:%S %p")
 
    await deploy_notif.send(f"**Deployed**: {DEPLOY_NAME} at {current_time}")



# @Bot.event
# async def on_command_error(ctx, error):
#     if isinstance(error, CommandNotFound):
#         print("System: lmao a nigger used", error)
#         return
#     BaseProgram.database_updating = False
#     raise error


# Essential Cog
# Bot.add_cog(BaseCog(Bot))


# Feature Cogs
Bot.add_cog(UploadCog(Bot))

print("> Starting AuQW Bot...")
Bot.run(DISCORD_TOKEN)
