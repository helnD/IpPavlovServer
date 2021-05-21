FROM microsoft/dotnet:5.0-aspnetcore-runtime
RUN apt-get update && apt-get install -y libgdiplus libc6-dev && ln -s /usr/lib/libgdiplus.so /usr/lib/gdiplus.dll

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

COPY WebApplication/*.csproj ./WebApplication/
COPY Domain/*.csproj ./Domain/
COPY Infrastructure/*.csproj ./Infrastructure/
COPY Infrastructure.Abstractions/*.csproj ./Infrastructure.Abstractions/
COPY Infrastructure.DataAccess/*.csproj ./Infrastructure.DataAccess/
COPY UseCases/*.csproj ./UseCases/
RUN dotnet restore WebApplication/WebApplication.csproj

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/out/ .
ENTRYPOINT ["dotnet", "WebApplication.dll"]