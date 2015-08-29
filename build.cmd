
@echo off
SET BUILDCMD_RELEASE=beta6
SET DNX_BUILD_VERSION=%BUILDCMD_RELEASE%-%APPVEYOR_BUILD_NUMBER%
cd %~dp0
SETLOCAL
SET CACHED_NUGET=%LocalAppData%\NuGet\NuGet.exe

IF EXIST %CACHED_NUGET% goto copynuget
echo Downloading latest version of NuGet.exe...
IF NOT EXIST %LocalAppData%\NuGet md %LocalAppData%\NuGet
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://www.nuget.org/nuget.exe' -OutFile '%CACHED_NUGET%'"

:copynuget
IF EXIST .nuget\nuget.exe goto restore
md .nuget
copy %CACHED_NUGET% .nuget\nuget.exe > nul

.nuget\NuGet.exe sources add -Name aspnetrelease -Source https://www.myget.org/F/aspnetrelease/api/v2
rem .nuget\NuGet.exe sources 
rem .nuget\NuGet.exe

:restore
IF EXIST packages\KoreBuild goto sake
IF DEFINED BUILDCMD_RELEASE (
	.nuget\NuGet.exe install KoreBuild -version 0.2.1-%BUILDCMD_RELEASE% -ExcludeVersion -o packages -nocache -pre
) ELSE (
	.nuget\NuGet.exe install KoreBuild -ExcludeVersion -o packages -nocache -pre
)
:sake
IF EXIST packages\Sake goto dnvm
.nuget\NuGet.exe install Sake -version 0.2 -o packages -ExcludeVersion

:dnvm
IF EXIST %USERPROFILE%\.dnx\runtimes\dnx-clr-win-x86.1.0.0-%BUILDCMD_RELEASE% goto coreInstall
IF DEFINED BUILDCMD_RELEASE (
	CALL packages\KoreBuild\build\dnvm install 1.0.0-%BUILDCMD_RELEASE% -runtime CLR -arch x86 -a default
) ELSE (
	CALL packages\KoreBuild\build\dnvm upgrade -runtime CLR -arch x86 
)

:coreInstall
IF EXIST %USERPROFILE%\.dnx\runtimes\dnx-CoreCLR-win-x86.1.0.0-%BUILDCMD_RELEASE% goto use
CALL packages\KoreBuild\build\dnvm install default -runtime CoreCLR -arch x86

:use
echo ;%PATH%; | find /C /I "dnx-clr-win-x86.1.0.0-%BUILDCMD_RELEASE%" >nul
if %ERRORLEVEL% EQU 0 goto build
CALL packages\KoreBuild\build\dnvm use default -runtime CLR -arch x86	

:build
packages\Sake\tools\Sake.exe -I packages\KoreBuild\build -v -f makefile.shade %*
