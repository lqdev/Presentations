# End-to-End Machine Learning with ML.NET and Azure

## Requirements

- Docker
- Docker Hub Account
- Azure CLI
- Azure Functions Core Tools
- .NET Core

## Building/Running This Project

### 1. Restoring packages

From the `code` directory, enter into the console:

```bash
dotnet restore
```

### 2. Training the model

From the `code` directory enter into the console:

```bash
cd ConsoleTrainingApp
dotnet build
dotnet run
```

### 3 Testing Web API Locally

From the `code` directory enter into the console:

```bash
cd WebApi
dotnet run
```

In POSTMAN or Insomnia make an HTTP POST request to the following endpoint `http://localhost:5000/api/predict` and include the body:

```json
{
    "SepalLength": 3.3,
    "SepalWidth": 1.6,
    "PetalLength": 0.2,
    "PetalWidth": 5.1
}
```

### 4.1 Building WebApi Docker Image

From the `code` directory enter into the console:

```bash
docker image build  -t <YOUR-DOCKERHUB-USERNAME>/<YOUR-IMAGE-NAME>:latest .
```

### 4.2 Test WebApi Docker Image Locally

Enter the following command into the console:

```bash
docker run -p 5000:80 <YOUR-DOCKERHUB-USERNAME>/<YOUR-IMAGE-NAME>:latest
```

In POSTMAN or Insomnia make an HTTP POST request to the following endpoint `http://localhost:5000/api/predict` and include the body:

```json
{
    "SepalLength": 3.3,
    "SepalWidth": 1.6,
    "PetalLength": 0.2,
    "PetalWidth": 5.1
}
```

### 4.3 Push WebApi Docker Image to Docker Hub

Enter the following command into the console:

```bash
docker push <YOUR-DOCKERHUB-USERNAME>/<YOUR-IMAGE-NAME>:latest
```

### 4.4 Deploy WebApi to Docker Conatiners

In the `azure-deploy.json` file inside the `code` directory replace the `containername` and `containerimage` properties

```json
{
    "containername": "<YOUR-IMAGE-NAME>",
    "containerimage": "<YOUR-DOCKERHUB-USERNAME>/<YOUR-IMAGE-NAME>:latest"
}
```

Log into Azure by using the following command and following the prompts.

```bash
az login
```

From the `code` directory enter the following commands. The `name` option can by anything of your choice.

```bash
az group create --name mlnetresourcegroup --location eastus
az group deployment create --resource-group mlnetresourcegroup --template-file azure-deploy.json
```

This should output a JSON object to the screen. Copy the `value` property of the `containerIPv4Address` property 

```json
{
    "outputs": {
        "containerIPv4Address": {
            "type": "String",
            "value": "<IP-ADDRESS>"
        }
    }
}
```

### 4.5 Testing Deployed WebApi Container 

In POSTMAN or Insomnia make an HTTP POST request to the following endpoint `http://<CONTAINER-IPv4-ADDRESS>/api/predict` and include the body:

```json
{
    "SepalLength": 3.3,
    "SepalWidth": 1.6,
    "PetalLength": 0.2,
    "PetalWidth": 5.1
}
```

[//]: <> (TODO - Serverless Function)
### 5.1 