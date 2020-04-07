using System;
using System.IO;
using Microsoft.Spark.Sql;
using Microsoft.Spark.Sql.Types;
using static Microsoft.Spark.Sql.Functions;

namespace SparkMLNET
{
    class Program
    {

        static void Main(string[] args)
        {
            // Initialize Spark Session
            var ss =
                SparkSession
                .Builder()
                .AppName("TestApp")
                .GetOrCreate();

            // Load counters Df
            var countersDf =
                ss
                .Read()
                .Option("header", true)
                .Option("inferSchema", true)
                .Csv("Data/Bicycle_Counters.csv");

            // Load counts Df
            var countsDf =
                ss
                .Read()
                .Option("header", true)
                .Option("inferSchema", true)
                .Csv("Data/Bicycle_Counts.csv");

            // Select subset of columns 
            var subsetCountersDf =
                countersDf
                .WithColumn("latitude", Col("latitude").Cast("float"))
                .WithColumn("longitude", Col("longitude").Cast("float"))
                .Select(Col("id"), Col("latitude"), Col("longitude"));

            // Select subset of columns 
            var subsetCountsDf =
                countsDf
                .Select(Col("id"), Col("date"), Col("counts"))
                .WithColumn("date", ToDate(Col("date"), "MM/dd/yyyy"));

            // Aggregate daily counts
            var groupedDf =
                subsetCountsDf
                .GroupBy(Col("id"), Col("date"))
                .Agg(Sum("counts").Alias("dailycount"))
                .OrderBy(Col("id"),Col("date"));

            // Get number of entries
            var totalCounterDf = 
                groupedDf
                .GroupBy(Col("id"))
                .Agg(Count("dailycount").Alias("countertotal"))
                .OrderBy(Col("countertotal").DescNullsFirst());

            // Add coordinates to daily counts
            var joinedGroupedDf =
                groupedDf
                .Join(subsetCountersDf, "id")
                .Select(Col("id"), Col("date"), Col("dailycount"), Col("latitude"), Col("longitude"));

            string saveDirectory = "output";

            if(!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }

            totalCounterDf.Write().Mode(SaveMode.Overwrite).Csv(Path.Join(saveDirectory,"totalcounts"));
            joinedGroupedDf.Write().Mode(SaveMode.Overwrite).Csv(Path.Join(saveDirectory,"dailycounts"));
        }
    }
}
