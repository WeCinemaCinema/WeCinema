# Use the official image as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ITDIV_TICKET/ITDIV_TICKET.csproj", "ITDIV_TICKET/"]
RUN dotnet restore "ITDIV_TICKET/ITDIV_TICKET.csproj"
COPY . .
WORKDIR "/src/ITDIV_TICKET"
RUN dotnet build "ITDIV_TICKET.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ITDIV_TICKET.csproj" -c Release -o /app/publish

# Copy the build to the base image and set the entry point
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ITDIV_TICKET.dll"]
