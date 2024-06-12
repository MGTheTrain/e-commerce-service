FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

MAINTAINER Marvin Gajek

# ARG NUGET_SOURCE_NAME="PLACEHOLDER_NUGET_SOURCE_NAME"
# ARG NUGET_USERNAME="PLACEHOLDER_NUGET_USERNAME"
# ARG NUGET_PASSWORD="PLACEHOLDER_NUGET_PASSWORD"
# ARG NUGET_REPOSITORY_SRC="PLACEHOLDER_ARG NUGET_REPOSITORY_SRC"

WORKDIR /app
COPY . .
WORKDIR /app/Mgtt.ECom.Web

# RUN dotnet nuget add source --name ${NUGET_SOURCE_NAME} --username ${NUGET_USERNAME} --password ${NUGET_PASSWORD} --store-password-in-clear-text ${NUGET_REPOSITORY_SRC}
RUN dotnet restore ./Mgtt.ECom.Web.csproj
RUN dotnet publish ./Mgtt.ECom.Web.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /app
COPY --from=build /app/Mgtt.ECom.Web/out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Mgtt.ECom.Web.dll"]