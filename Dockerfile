FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY *.sln .
COPY ./CommunityService.API/*.csproj ./CommunityService.API/
COPY ./CommunityService.Application/*.csproj ./CommunityService.Application/
COPY ./CommunityService.Core/*csproj ./CommunityService.Core/
COPY ./CommunityService.Infrastructure/*.csproj ./CommunityService.Infrastructure/
COPY ./CommunityService.Persistence/ ./CommunityService.Persistence/
RUN dotnet restore

COPY ./CommunityService.API/ ./CommunityService.API/
COPY ./CommunityService.Application/ ./CommunityService.Application/
COPY ./CommunityService.Core/ ./CommunityService.Core/
COPY ./CommunityService.Infrastructure/ ./CommunityService.Infrastructure/
COPY ./CommunityService.Persistence/ ./CommunityService.Persistence/
WORKDIR /source/CommunityService.API/
RUN dotnet publish -c release -o /app 

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT [ "dotnet", "CommunityService.API.dll" ]
