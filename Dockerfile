FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-net
WORKDIR /app
COPY . .
RUN dotnet restore && \
    dotnet publish DERMS.Silo -c Release -o /app/out --no-restore


FROM node:16 AS build-svelte
WORKDIR /app
COPY miniDERM/package*.json ./
RUN npm install
COPY miniDERM ./
RUN npm run build

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS runtime
WORKDIR /app
COPY --from=build-net /app/out ./
COPY --from=build-svelte /app/build ./wwwroot

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS="http://+:5000"

EXPOSE 5000

ENTRYPOINT ["dotnet", "DERMS.Silo.dll"]
