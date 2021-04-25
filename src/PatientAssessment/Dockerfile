#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/PatientAssessment/PatientAssessment.csproj", "src/PatientAssessment/"]
RUN dotnet restore "src/PatientAssessment/PatientAssessment.csproj"
COPY . .
WORKDIR "/src/src/PatientAssessment"
RUN dotnet build "PatientAssessment.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PatientAssessment.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PatientAssessment.dll"]