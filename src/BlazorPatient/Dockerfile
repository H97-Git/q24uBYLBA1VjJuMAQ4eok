FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_ENVIRONMENT=Docker

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["BlazorPatient/BlazorPatient.csproj", "BlazorPatient/"]
RUN dotnet restore "BlazorPatient/BlazorPatient.csproj"
COPY . .
WORKDIR "/src/BlazorPatient"
RUN dotnet build "BlazorPatient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazorPatient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlazorPatient.dll"]