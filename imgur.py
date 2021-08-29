
from urllib.request import urlopen, Request, urlretrieve
from urllib.parse import urlencode
from base64 import standard_b64encode
from json import loads
import os 

os.chdir(os.path.dirname(os.path.abspath(__file__)))
print(os.path.abspath(__file__))

url = "https://pbs.twimg.com/media/E5hTaZLWYAMU6ir.jpg"
urlretrieve(url, "daily.jpg")

f = open("./daily.jpg", "rb") 
b64_image = standard_b64encode(f.read())

client_id = "4d8b88de7160b18" # put your client ID here
headers = {'Authorization': 'Client-ID ' + client_id}

data = {'image': b64_image, 'title': 'test'} # create a dictionary.

request = Request(url="https://api.imgur.com/3/upload.json", data=urlencode(data).encode("utf-8"),headers=headers)
print(request)
response = urlopen(request).read()

parse = loads(response)
print(parse['data']['link'])


