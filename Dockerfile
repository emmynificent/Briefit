FROM mcr.microsoft.com/dotnet/sdk:9.0 as build
workdir /src


COPY ["Briefit.sln", "./"]
COPY ["Briefit/Briefit.csproj", "Briefit/"]
RUN dotnet restore "Briefit/Briefit.csproj"


COPY . .
WORKDIR "/src/Briefit"
RUN dotnet publish "Briefit.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .


ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "briefit.csproj"]
