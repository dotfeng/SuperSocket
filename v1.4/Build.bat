@echo off

set fdir=%WINDIR%\Microsoft.NET\Framework64

if not exist %fdir% (
	set fdir=%WINDIR%\Microsoft.NET\Framework
)

set msbuild=%fdir%\v4.0.30319\msbuild.exe

%msbuild% SuperSocket.2010.sln /p:Configuration=Release /t:Rebuild /p:SignAssembly=true /p:AssemblyOriginatorKeyFile=..\supersocket.snk

%msbuild% SuperSocket.2010.NET35.sln /p:Configuration=Release /t:Rebuild /p:SignAssembly=true /p:AssemblyOriginatorKeyFile=..\supersocket.snk

pause