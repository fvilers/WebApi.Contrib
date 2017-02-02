@echo off
for %%f in (dist\*.nupkg) do nuget push %%f -Source https://www.nuget.org