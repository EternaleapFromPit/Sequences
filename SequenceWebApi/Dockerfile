#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SequenceWebApi/SequenceWebApi.csproj", "SequenceWebApi/"]
COPY ["SequenceLibrary/SequenceLibrary.csproj", "SequenceLibrary/"]
RUN dotnet restore "SequenceWebApi/SequenceWebApi.csproj"
COPY . .
WORKDIR "/src/SequenceWebApi"
RUN dotnet build "SequenceWebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SequenceWebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SequenceWebApi.dll"]