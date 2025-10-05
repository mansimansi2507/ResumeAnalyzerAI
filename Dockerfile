# -----------------------
# Stage 1: Build
# -----------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything from repo to container
COPY . .

# Navigate to the project folder containing the .csproj
WORKDIR /app/ResumeAnalyzerAI

# Restore NuGet packages
RUN dotnet restore

# Publish the app in Release mode to /app/out
RUN dotnet publish -c Release -o /app/out

# -----------------------
# Stage 2: Runtime
# -----------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the published files from the build stage
COPY --from=build /app/out .

# Expose Railway default port
EXPOSE 8080

# Run the app
ENTRYPOINT ["dotnet", "ResumeAnalyzerAI.dll"]
