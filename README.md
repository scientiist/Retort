# Retort - An HTTP Request Server.
Conarium Software
## Features:
* [Lua](https://www.lua.org/about.html)-driven - Write your request handler in lua, an easy and lightweight scripting language.
* Dead-Simple -  Not much can go wrong here. Lightweight too.
* Open Source - Check the code out yourself, modify, improve, steal, etc.

## Compiling:
Download the project files firstly.
You'll need the .NET SDK installed, I tested this with 3.1 and .NET5, the others will probably work too.
Grab __NLua__ from the NuGet manager.
Run  ` dotnet build ` or ` dotnet publish ` to build the file.
If any of you would like, send me a message somewhere and i'll upload builds later on.



## Running: 
```sh
retort.exe <address> <script> [args]
```


## Arguments

#### Required Arguments

##### Address: retort.exe **<address>** <script> [args]

Must be a valid URL/IPAddress like so: ` http://127.0.0.1:7777/ ` or ` http://yourweb.site:33333/webapi/ `

Make sure the address ends with a forward slash. It seems to complain if you don't. Perhaps i'll fix that later on.

##### Script: retort.exe <address> **<script>** [args]

A path to a lua script: ` handlers/feedback.lua ` or something along those lines.


##### Optional Arguments

More arguments to be added later. Feel free to make requests for what you need.

```sh
	--verbose	Toggles verbose output
```

## Scripting:
As of this version, the lua environment remains unchanged. No custom APIs (Yet...). If you need some feature, make a pull request or send me a message and i'll see what I can do.

Your lua script must return a function to be valid. This is the function that will run on each HTTPRequest.
The function should take an argument of string request and return a string response.

```lua
return function (requeststr)
    local response = "lol ok";
    return response;
end
```



