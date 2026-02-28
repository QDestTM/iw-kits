ARG SRC_PATH=/backend/src/IWKits.Api

# STAGE 1 - Builder
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG SRC_PATH
WORKDIR /src

COPY /${SRC_PATH} ./${SRC_PATH}/
COPY *.sln .

RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# STAGE 2 - Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS run
WORKDIR /app

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "IWKits.Api.dll"]