rem @if "%SCM_TRACE_LEVEL%" NEQ "4" @echo off
:: this is the custom deploy script for kudu 
:: ----------------------
:: KUDU Deployment Script
:: Version: 0.1.13
:: ----------------------

:: Prerequisites
:: -------------

::Verify if appvoyer did a succesfull build
:: for /f "delims=" %A in ('git rev-parse HEAD') do curl https://api.github.com/repos/Anderman/Mvc.JQuery.Datatables/statuses/%A | findstr \"state\" | findstr succes
:: IF ERRORLEVEL NEQ 0 goto nodesetup
:: echo Appvoyer failed
:: goto error


:nodesetup
:: Verify node.js installed
where node 2>nul >nul
IF %ERRORLEVEL% NEQ 0 (
  echo Missing node.js executable, please install node.js, if already installed make sure it can be reached from current environment.
  goto error
)
NuGet.exe sources add -Name aspnetrelease -Source https://www.myget.org/F/aspnetrelease/api/v2
SET PATH=D:\Program Files (x86)\npm\3.1.0;%PATH%
:: Setup
:: -----

setlocal enabledelayedexpansion

SET ARTIFACTS=%~dp0%..\artifacts

IF NOT DEFINED DEPLOYMENT_SOURCE (
  SET DEPLOYMENT_SOURCE=%~dp0%.
)

IF NOT DEFINED DEPLOYMENT_TARGET (
  SET DEPLOYMENT_TARGET=%ARTIFACTS%\wwwroot
)

IF NOT DEFINED NEXT_MANIFEST_PATH (
  SET NEXT_MANIFEST_PATH=%ARTIFACTS%\manifest

  IF NOT DEFINED PREVIOUS_MANIFEST_PATH (
    SET PREVIOUS_MANIFEST_PATH=%ARTIFACTS%\manifest
  )
)

IF NOT DEFINED KUDU_SYNC_CMD (
  :: Install kudu sync
  echo Installing Kudu Sync
  call npm install kudusync -g --silent
  IF !ERRORLEVEL! NEQ 0 goto error

  :: Locally just running "kuduSync" would also work
  SET KUDU_SYNC_CMD=%appdata%\npm\kuduSync.cmd
)
IF NOT DEFINED DEPLOYMENT_TEMP (
  SET DEPLOYMENT_TEMP=%temp%\___deployTemp%random%
  SET CLEAN_LOCAL_DEPLOYMENT_TEMP=true
)

IF DEFINED CLEAN_LOCAL_DEPLOYMENT_TEMP (
  IF EXIST "%DEPLOYMENT_TEMP%" rd /s /q "%DEPLOYMENT_TEMP%"
  mkdir "%DEPLOYMENT_TEMP%"
)

IF DEFINED MSBUILD_PATH goto MsbuildPathDefined
SET MSBUILD_PATH=%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe
:MsbuildPathDefined
::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: Deployment
:: ----------

echo Handling ASP.NET 5 Web Application deployment.

:: Remove wwwroot if deploying to default location
IF "%DEPLOYMENT_TARGET%" == "%WEBROOT_PATH%" (
    FOR /F %%i IN ("%DEPLOYMENT_TARGET%") DO IF "%%~nxi"=="wwwroot" (
    SET DEPLOYMENT_TARGET=%%~dpi
    )
)

:: Remove trailing slash if present
IF "%DEPLOYMENT_TARGET:~-1%"=="\" (
    SET DEPLOYMENT_TARGET=%DEPLOYMENT_TARGET:~0,-1%
)


:: 1. Set DNX Path
set DNVM_CMD_PATH_FILE="%USERPROFILE%\.dnx\temp-set-envvars.cmd"
set DNX_RUNTIME="%USERPROFILE%\.dnx\runtimes\dnx-CLR-win-x86.1.0.0-beta8"

:: 2. Install DNX
call :ExecuteCmd PowerShell -NoProfile -NoLogo -ExecutionPolicy unrestricted -Command "[System.Threading.Thread]::CurrentThread.CurrentCulture = ''; [System.Threading.Thread]::CurrentThread.CurrentUICulture = '';$CmdPathFile='%DNVM_CMD_PATH_FILE%';& '%SCM_DNVM_PS_PATH%' " install 1.0.0-beta8 -arch x86 -r CLR %SCM_DNVM_INSTALL_OPTIONS%
IF !ERRORLEVEL! NEQ 0 goto error


:: 3. Run DNU Restore
call %DNX_RUNTIME%\bin\dnu restore "%DEPLOYMENT_SOURCE%" %SCM_DNU_RESTORE_OPTIONS%
IF !ERRORLEVEL! NEQ 0 goto error

:: 4. Run DNU Bundle
echo call %DNX_RUNTIME%\bin\dnu publish "D:\home\site\repository\Sample\UserManagement\project.json" --runtime %DNX_RUNTIME% --out "%DEPLOYMENT_TEMP%" %SCM_DNU_PUBLISH_OPTIONS%
call %DNX_RUNTIME%\bin\dnu publish "D:\home\site\repository\Sample\UserManagement\project.json" --runtime %DNX_RUNTIME% --out "%DEPLOYMENT_TEMP%" %SCM_DNU_PUBLISH_OPTIONS%
IF !ERRORLEVEL! NEQ 0 goto error

:: 5. KuduSync
echo call %KUDU_SYNC_CMD% -v 50 -f "%DEPLOYMENT_TEMP%" -t "%DEPLOYMENT_TARGET%" -n "%NEXT_MANIFEST_PATH%" -p "%PREVIOUS_MANIFEST_PATH%" -i ".git;.hg;.deployment;deploy.cmd"
call %KUDU_SYNC_CMD% -v 50 -f "%DEPLOYMENT_TEMP%" -t "%DEPLOYMENT_TARGET%" -n "%NEXT_MANIFEST_PATH%" -p "%PREVIOUS_MANIFEST_PATH%" -i ".git;.hg;.deployment;deploy.cmd"
IF !ERRORLEVEL! NEQ 0 goto error
)

::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

:: Post deployment stub
IF DEFINED POST_DEPLOYMENT_ACTION call "%POST_DEPLOYMENT_ACTION%"
IF !ERRORLEVEL! NEQ 0 goto error

goto end

:: Execute command routine that will echo out when error
:ExecuteCmd
setlocal
set _CMD_=%*
call %_CMD_%
if "%ERRORLEVEL%" NEQ "0" echo Failed exitCode=%ERRORLEVEL%, command=%_CMD_%
exit /b %ERRORLEVEL%

:error
endlocal
echo An error has occurred during web site deployment.
call :exitSetErrorLevel
call :exitFromFunction 2>nul

:exitSetErrorLevel
exit /b 1

:exitFromFunction
()

:end
endlocal
echo Finished successfully.
