msbuild /t:restore
msbuild /p:Configuration=Release
msbuild /t:pack /p:Configuration=Release /p:PackageOutputPath=c:\_Code\Nuget