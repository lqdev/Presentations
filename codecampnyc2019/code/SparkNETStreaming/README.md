# .NET for Spark Streaming Sample

This sample showcases streaming in .NET for Apache Spark. This app was built for Ubuntu, but should work cross-platform. For convenience, scripts have been created to build and run the application. You can run the commands inside the scripts by entering into the terminal respective to your OS.

## Build the application

```bash
./publish.sh
```

## Run the application

Open a terminal and start netcat

```bash
nc -lp 9000
```

Then, run `run.sh`

```bash
./run.sh
```

In the netcat window, try entering these values, each followed by `Enter`.

```bash
A
B
C
```