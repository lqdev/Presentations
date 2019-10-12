using System;
using Microsoft.Spark.Sql;
using Microsoft.Spark.Sql.Types;
using static Microsoft.Spark.Sql.Functions;

namespace SparkNETDataFrames
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize Session
            SparkSession ss = 
                SparkSession
                    .Builder()
                    .AppName("Working with DataFrames")
                    .GetOrCreate();

            // Read Data
            DataFrame businesses = 
                ss
                .Read()
                .Option("header","true")
                .Option("inferSchema","true")
                .Csv("Data/NYC-Restaurant-Inspections.csv");
            
            businesses = businesses.Select("CAMIS","DBA","BORO","CUISINE DESCRIPTION");
                
            DataFrame inspections = 
                ss
                .Read()
                .Option("header","true")
                .Option("inferSchema","true")
                .Csv("Data/NYC-Restaurant-Inspections.csv");

            inspections = inspections.Select("CAMIS","INSPECTION DATE","VIOLATION CODE","CRITICAL FLAG","SCORE","GRADE","INSPECTION TYPE");

            // Select columns
            businesses.Select(Col("CAMIS"),Col("DBA")).Show(1);

            inspections.Select(inspections["VIOLATION CODE"]).Show(1);

            // Filter
            businesses
                .Filter(Col("BORO") == "Manhattan")
                .Select("DBA","BORO")
                .Show(3);

            // Group / Aggregate
            businesses
                .GroupBy("CUISINE DESCRIPTION")
                .Agg(Count("CUISINE DESCRIPTION").Alias("CUISINE COUNT"))
                .Show(10);

            // Order
            businesses
                .GroupBy("CUISINE DESCRIPTION")
                .Agg(Count("CUISINE DESCRIPTION").Alias("CUISINE COUNT"))
                .OrderBy(Col("CUISINE COUNT").Desc())
                .Show(3);

            // Join
            DataFrame joinedDf = 
                businesses
                    .Join(inspections,"CAMIS")
                    .Select(Col("DBA"),Col("CUISINE DESCRIPTION"),Col("GRADE"));
            
            joinedDf.Show(5);

            // SQL            
            businesses.CreateOrReplaceTempView("businesses");

            inspections.CreateOrReplaceTempView("inspections");

            ss.Sql(@"SELECT b.DBA,b.`CUISINE DESCRIPTION`,i.GRADE FROM businesses b JOIN inspections i ON b.CAMIS = i.CAMIS").Show(5);
            
            // UDF
            ss.Udf().Register<string,string>("Tupper",Tupper);

            inspections
                .Select(CallUDF("Tupper",Col("INSPECTION TYPE")).Alias("CAPITALIZED"))
                .Show(3);

            // Save
            joinedDf
                .Write()
                .Mode(SaveMode.Overwrite)
                .Csv("output");
        }

        public static string Tupper(string inspectionType)
        {
            return inspectionType.ToUpper();
        }
    }
}
