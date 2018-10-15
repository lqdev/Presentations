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

In the previous step when the model was trained and persisted, a file called `model.zip` should have been created in the `ConsoleTrainingApp` directory. Copy it into the `WebApi` directory.

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

Click on `Create Resource` and search for `container instances`.

![](screenshots/aci1.png)

From the search results, select the `Container Instances` product.

![](screenshots/aci2.png)

Click `Create`

![](screenshots/aci3.png)

Fill in the container name (can be anything), container image (docker image uploaded to Docker Hub) and create a new resource group or use an existing one. Then, click `Ok`.

![](screenshots/aci4.png)

For this section, it's okay to leave the defaults as is. Click `Ok`.

![](screenshots/aci5.png)

Once you confirmed that all the deployment information is correct, click `Ok`.

![](screenshots/aci6.png)

### 4.5 Testing Deployed WebApi Container 

Once the resource has been deployed, you can find it by clicking on `Resource Groups`.

![](screenshots/aci7.png)

Select the newly created resource group from the list of resource groups.

![](screenshots/aci8.png)

This will list your newly created containers. Click on it.

![](screenshots/aci9.png)

In the container's `Overview` page you'll find the IP address through which you can access the Web API.

![](screenshots/aci10.png)

In POSTMAN or Insomnia make an HTTP POST request to the following endpoint `http://<CONTAINER-IP-ADDRESS>/api/predict` and include the body:

```json
{
    "SepalLength": 3.3,
    "SepalWidth": 1.6,
    "PetalLength": 0.2,
    "PetalWidth": 5.1
}
```

### 5.1 Spin Up Azure Functions Application in Azure

In the Azure portal, click on `Create Resource` and click on `Serverless Function App`.

![](screenshots/azfn1.png)


Enter a name for the function app and either create a new or select an existing resource group to deploy the application to. When ready, click `Create`.

![](screenshots/azfn2.png)


### 5.2 Upload Model to Azure Storage

Click on `Resource Groups` and select the resource group containing the newly created Azure Functions Application.

![](screenshots/azfn3.png)

Select Storage Account resource associated with your Azure Functions application. 

![](screenshots/azfn4.png)

Click on `Blobs`

![](screenshots/azfn5.png)

Create a new storage container by clicking on `Container`.

![](screenshots/azfn6.png)

In the form, enter the name `models` and click `OK`. This will create a new blob storage container with the name `models`.

![](screenshots/azfn7.png)

Once the `models` container has been created, click on it.

![](screenshots/azfn8.png)

Then, click on `Upload`

![](screenshots/azfn9.png)

Select `model.zip` which contains your persisted model as the file from your local PC and click `Upload`. When complete it should show up in the `models` container. 

![](screenshots/azfn10.png)

### 5.3 Test Azure Functions Application Locally

Navigate up one level to the storage account resource.

![](screenshots/azfn11.png)

Click on `Access Keys` int he `Settings` section abd copy the `Connection String` (either one is okay) to your clipboard.

![](screenshots/azfn12.png)

In the `ServerlessFunctionApp` directory create a file called `local.settings.json` with the following content.

```json
{
    "IsEncrypted": false,
    "Values": {
        "FUNCTIONS_WORKER_RUNTIME": "dotnet",
        "AzureWebJobsStorage": "<YOUR-CONNECTION-STRING>"
    }
}
```

Paste the `Connection String` as the value of the `AzureWebJobsStorage` property.


Inside the `ServerlessFunctionApp` directory enter the following commands.

```bash
dotnet build
```

Navigate to the `bin\Debug\netstandard2.0` directory and enter into the command line `func host start`. This should run your application locally.

In POSTMAN or Insomnia make an HTTP POST request to the following endpoint `http://localhost:7071/api/predict` and include the body:

```json
{
    "SepalLength": 3.3,
    "SepalWidth": 1.6,
    "PetalLength": 0.2,
    "PetalWidth": 5.1
}
```

### 5.4 Prepare Azure Functions Application for Deployment

Nagivate up one level in the Azure Portal to the Resource Group page which contains your Azure Functions application

![](screenshots/azfn13.png)

Select the Azure Functions `App Service` resource.  

![](screenshots/azfn14.png)

Select the `Platform features` tab and click on `Application Settings`.

![](screenshots/azfn15.png)

Select 64-bit for the `Platform` setting and click `Save`.

![](screenshots/azfn16.png)

### 5.5 Deploy Azure Functions Application

In the command line enter the following command to log into your Azure account to which you'll be deploying the application and follow the prompts.

```bash
func azure login
```

From the `ServerlessFunctionApp` directory, enter the following command into the command line and replace `<YOUR-FUNCTION-APP-NAME>` with the name of your Azure Functions Application.

```bash
func azure functionapp publish <YOUR-FUNCTION-APP-NAME>
```

### 5.6 Test Deployed Azure Functions Application

In the Azure portal open your Azure Functions application resource and click on the function you deployed. In this case, the name of our function is `Predict`.

![](screenshots/azfn17.png)

Inside the function page, click on `Get function URL` and copy it.

![](screenshots/azfn18.png)

In POSTMAN or Insomnia make an HTTP POST request to the function URL you just copied and include the body:

```json
{
    "SepalLength": 3.3,
    "SepalWidth": 1.6,
    "PetalLength": 0.2,
    "PetalWidth": 5.1
}
```