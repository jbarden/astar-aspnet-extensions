@echo off
IF EXIST "c:\local-nuget" (
	echo Copying output files
	FORFILES /P . /M *.*nupkg /S /C "cmd /c copy @file c:\local-nuget"
) ELSE ( echo Not copying output files - target does not exist )