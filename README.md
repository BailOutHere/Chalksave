# Chalksave

Chat-based chalk canvas saver and loader. 
/helpchalk - lists below commands
/listchalk - lists chalk files
/loadchalk <filename> - loads specific file
/savechalk <canvas ID> <filename> - saves specified canvas with filename

- Solution name, project name, and project namespace to your project ID
- Various fields in the manifest.json to your project ID and name

## Building

To build the project, you need to set the `GDWeavePath` environment variable to your game install's GDWeave directory (e.g. `G:\games\steam\steamapps\common\WEBFISHING\GDWeave`). This can also be done in Rider with `File | Settings | Build, Execution, Deployment | Toolset and Build | MSBuild global properties` or with a .user file in Visual Studio.
