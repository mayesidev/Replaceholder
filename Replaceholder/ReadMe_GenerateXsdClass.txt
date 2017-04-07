To regenerate the ReplacholderSchema.cs file from the ReplaceholderSchema.xsd...

Open a "Developer Command Prompt for VS<year>" where <year> is the year of the
Visual Studio installed (ie- 2015). Then run the following command, where
<codePath> is replaced by the appropriate path to the source code:

xsd /c <codePath>\ReplaceholderSchema.xsd /o:<codePath>

Type the following in to "Developer Command Prompt for VS<year>" for
additional information on the usage of the tool, and possible inputs:

xsd /?
