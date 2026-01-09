FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution file from root
COPY briefit.sln ./

# Copy project file
COPY briefit/briefit.csproj briefit/
RUN dotnet restore "briefit/briefit.csproj"

# Copy everything
COPY . .

# Build
WORKDIR /src/briefit
RUN dotnet publish "briefit.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Use Render's PORT environment variable
ENV ASPNETCORE_URLS=http://+:${PORT}

# Run the app - note: lowercase 'briefit.dll' matches the csproj name
ENTRYPOINT ["dotnet", "briefit.dll"]
```