# astar-aspnet-extensions
Contains some basic extensions to make configuration more consistent

## Version 0.5.0

Version 0.5.0 makes the original ServiceCollectionExtensions > Configure method into 2 separate methods ConfigureUI & ConfigureAPI that perform appropriate configurations.

In addition, added WebApplicationBuilder > DisableServerHeader method that will, as the method name suggests, Disable the "Server" heading usually sent from the Kestrel Server.
