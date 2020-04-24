# ML.NET Sentiment Analysis Model Deployed as a Saturn Web API and Azure Function Custom Handler

## Prerequisites

This project was built on a Windows 10 PC, but it should work cross-platform on Mac and Linux.

- [Node.js](https://nodejs.org/en/)
- [Racket](https://download.racket-lang.org/)
- [Azure Functions Core Tools](https://docs.microsoft.com/azure/azure-functions/functions-run-local). This sample uses v2.x of the tool.

## Package the application

Enter the following command into the command prompt:

```bash
dotnet build -r win-x64
```

## Run the server (not as an Azure Function)

Enter the following command into the command prompt.

```bash
dotnet run -r win-x64
```

Using an application like Postman or Insomnia, make a `POST` request to `localhost:7071/analyze-sentiment` with the following body:

```json
{
  "Comment": "The was a bad steak"
}
```

The response should look the one below:

```text
Negative
```

## Run Azure Function

Enter the following command into the command prompt from the root application directory.

```bash
func start
```

Using an application like Postman or Insomnia, make a `POST` request to `localhost:7071/api/analyze-sentiment`  with the following body:

```json
{
  "Comment": "The was a bad steak"
}
```

The response should look the one below:

```text
Negative
```