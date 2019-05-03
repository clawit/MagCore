dotnet restore
cd MagCore.Server
dotnet publish -c Release --self-contained -r linux-x64
cd ..
