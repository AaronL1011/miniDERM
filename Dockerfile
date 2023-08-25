# Stage 1: Build the .NET application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-net

WORKDIR /app
COPY DERMS.sln .
COPY DERMS.Dto/ DERMS.Dto/
COPY DERMS.Grains/ DERMS.Grains/
COPY DERMS.Silo/ DERMS.Silo/
COPY DERMS.Interfaces/ DERMS.Interfaces/
RUN dotnet restore
WORKDIR /app/DERMS.Silo
RUN dotnet publish -c Release -o /app/out

WORKDIR /app
COPY . .

# Stage 2: Build the SvelteKit application
FROM node:16 AS build-svelte

WORKDIR /app
COPY miniDERM/package.json miniDERM/package-lock.json ./
RUN npm install
COPY miniDERM ./
RUN npm run build

# Stage 3: Combine both applications into a single image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime

WORKDIR /app
COPY --from=build-net /app/out ./
COPY --from=build-svelte /app/build ./wwwroot

# Set the ASP.NET Core environment to Production
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose the ports
EXPOSE 6001

# Start the .NET application
ENTRYPOINT ["dotnet", "DERMS.Silo.dll"]
