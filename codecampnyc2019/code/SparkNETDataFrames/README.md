# Working with DataFrames in .NET for Apache Spark

This application showcases operations on DataFrames using .NET for Apache Spark. This app was built for Ubuntu, but should work cross-platform. For convenience, scripts have been created to build and run the application. You can run the commands inside the scripts by entering into the terminal respective to your OS.

- Read
- Select
- Filter
- Group / Aggregate
- Order
- Join
- UDF
- Save

The data in this sample comes from [DOHMH New York City Restaurant Inspection Results](https://data.cityofnewyork.us/Health/DOHMH-New-York-City-Restaurant-Inspection-Results/43nn-pn8j). 

## Prepare data

1. Create a directory called `Data`.
1. Download the dataset and save it to the newly created `Data` directory.

## Build the application

Run the `publish.sh` script.

```bash
./publish.sh
```

## Run the application

Run the `.run.sh` script.

```bash
./run.sh
```