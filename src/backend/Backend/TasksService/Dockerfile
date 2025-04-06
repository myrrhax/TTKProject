FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ./src/backend/Backend/TasksService/TasksService.csproj TasksService/
RUN dotnet restore TasksService/TasksService.csproj
COPY ./src/backend/Backend/TasksService/ TasksService/
WORKDIR /src/TasksService
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "TasksService.dll"]
