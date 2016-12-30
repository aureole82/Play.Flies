@ECHO OFF

ECHO Remove NuGet packages repository...
RMDIR /S /Q packages
ECHO.

ECHO Remove all bin directories...
CALL :DeleteFolder bin
ECHO.

ECHO Remove all obj directories...
CALL :DeleteFolder obj
ECHO.

ECHO Remove Visual Studios local user and environment settings...
RMDIR /S /Q .vs
ECHO.

ECHO Done!
PAUSE
GOTO :EOF 

:DeleteFolder
REM NOTE: Setting the delimiter to a zero string ignores spaces within pathes.
FOR /F "delims=" %%D IN ('DIR /AD /S /B "%~1"') DO RMDIR /S /Q "%%D"
GOTO :EOF 