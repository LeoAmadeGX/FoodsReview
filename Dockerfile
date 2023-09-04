#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# 使用官方 ASP.NET Core 映像作為基本映像
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FoodsReview.csproj", "."]
RUN dotnet restore "./FoodsReview.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "FoodsReview.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FoodsReview.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FoodsReview.dll"]