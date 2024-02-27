#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

ENV JWT__ISSUER="Docker JWT Builder"
ENV JWT__KEY="wx7K_foP9uZUWIgNVGqY2qQ2P5295fAo7vlqwJoBRThz4upUlMdmGq_L4bcxeOMR9q1Bj2W8P3wy0XyYTg3tXhagEo-cFOUfSRgrDSMMhcXOnuDrxKarJdEJe2VRSZ1064qEXyc8GOq_D-4zSqQrRYz-LWQpVfQ67cAXKf1Ggsc"
ENV ENCRYPTION__DEFAULT__KEY="0123456789ABCDEF"
ENV ENCRYPTION__DEFAULT__IV="0123456789ABCDEF"

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["UsersAuthenticationJWT/UsersAuthenticationJWT.csproj", "UsersAuthenticationJWT/"]
RUN dotnet restore "UsersAuthenticationJWT/UsersAuthenticationJWT.csproj"
COPY . .
WORKDIR "/src/UsersAuthenticationJWT"
RUN dotnet build "UsersAuthenticationJWT.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UsersAuthenticationJWT.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["UsersAuthenticationJWT/appsettings.json", "."]

ENTRYPOINT ["dotnet", "UsersAuthenticationJWT.dll"]