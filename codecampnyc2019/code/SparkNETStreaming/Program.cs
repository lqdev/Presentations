using System;
using Microsoft.Spark.Sql; 
using Microsoft.Spark.Sql.Streaming;
using static Microsoft.Spark.Sql.Functions;

namespace SparkNETStreaming
{
    class Program
    {
        static void Main(string[] args)
        {
            SparkSession ss = 
                SparkSession
                    .Builder()
                    .AppName(".NET for Spark Streaming")
                    .GetOrCreate();
            
            DataFrame stream =
                ss
                .ReadStream()
                .Format("socket")
                .Option("host","localhost")
                .Option("port",9000)
                .Load();

            DataFrame grade = 
                stream
                    .Select(Col("value"));

            StreamingQuery query = 
                grade
                    .WriteStream()
                    .OutputMode(OutputMode.Append)
                    .Format("console")
                    .Start();

            query.AwaitTermination();
        }
    }
}
