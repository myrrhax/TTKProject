FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ./src/backend/Backend/ImageService/ImageService.csproj ImageService/
RUN dotnet restore ImageService/ImageService.csproj
COPY ./src/backend/Backend/ImageService/ ImageService/
WORKDIR /src/ImageService
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "ImageService.dll"]