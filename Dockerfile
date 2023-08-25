# Stage 1: Build the .NET application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-net
WORKDIR /app
COPY . .
RUN dotnet restore && \
    dotnet publish DERMS.Silo -c Release -o /app/out

# Stage 2: Build the SvelteKit application
FROM node:16 AS build-svelte
WORKDIR /app
COPY miniDERM/package*.json ./
RUN npm install
COPY miniDERM ./
RUN npm run build

# Stage 3: Combine both applications into a single image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build-net /app/out ./
COPY --from=build-svelte /app/build ./wwwroot
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 6001
ENTRYPOINT ["dotnet", "DERMS.Silo.dll"]
