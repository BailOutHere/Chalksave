# Chalksave

Chat-based chalk canvas saver and loader, based on the wonderful GDWeave by NotNet.

# Basic usage
Usage:
/helpchalk - lists below commands
/listchalk - lists chalk files
/loadchalk <filename> - loads specific file
/savechalk <canvas ID> <filename> - saves specified canvas with filename


<p align="left">
  <img src="./MAP.jpg" alt="Canvas ID Map"/>
</p>




## Building

To build the project, you need to set the `GDWeavePath` environment variable to your game install's GDWeave directory (e.g. `G:\games\steam\steamapps\common\WEBFISHING\GDWeave`). This can also be done in Rider with `File | Settings | Build, Execution, Deployment | Toolset and Build | MSBuild global properties` or with a .user file in Visual Studio.

# Technical info - files

Files are stored as json dicts at user://chalksaves/, usually found at AppData\Roaming\Godot\app_userdata\webfishing_2_newver\chalksaves depending on your layout.

Dict keys:
canvasID - the canvas ID, used for loading the file to the correct canvas
canvasArr - Array of 3 dimensional arrays. Each array is split into Canvas x coord, y coord and colour.
