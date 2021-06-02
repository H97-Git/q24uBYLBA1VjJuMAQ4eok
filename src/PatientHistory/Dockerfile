FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["PatientHistory/PatientHistory.csproj", "PatientHistory/"]
RUN dotnet restore "PatientHistory/PatientHistory.csproj"
COPY . .
WORKDIR "/src/PatientHistory"
RUN dotnet build "PatientHistory.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PatientHistory.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PatientHistory.dll"]