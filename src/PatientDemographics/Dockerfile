FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["PatientDemographics/PatientDemographics.csproj", "PatientDemographics/"]
RUN dotnet restore "PatientDemographics/PatientDemographics.csproj"
COPY . .
WORKDIR "/src/PatientDemographics"
RUN dotnet build "PatientDemographics.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PatientDemographics.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PatientDemographics.dll"]