FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore

RUN dotnet build --configuration Debug 

RUN mkdir /app
RUN dotnet publish -o /app --configuration Debug

WORKDIR /app

ENTRYPOINT ["dotnet", "/app/LobbyReader.API.dll"]