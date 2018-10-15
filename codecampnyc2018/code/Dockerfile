FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY . ./
RUN dotnet restore WebApi/*.csproj
RUN dotnet publish WebApi/*.csproj -c release -o out

FROM microsoft/dotnet:2.1-aspnetcore-runtime AS runtime
WORKDIR /app
COPY WebApi/model.zip .
COPY --from=build /app/WebApi/out ./
ENTRYPOINT ["dotnet", "WebApi.dll"]