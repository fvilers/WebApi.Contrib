@echo off
erase dist\*.nupkg
msbuild src\WebApi.Contrib.sln /p:Configuration=Release