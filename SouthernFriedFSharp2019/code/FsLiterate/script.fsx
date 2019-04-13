(*** hide ***)
#r "../packages/Deedle/lib/net45/Deedle.dll"
#r "../packages/XPlot.GoogleCharts/lib/net45/XPlot.GoogleCharts.dll"
#r "../packages/XPlot.GoogleCharts.Deedle/lib/net45/XPlot.GoogleCharts.Deedle.dll"

(**

Welcome to FsLab journal
========================

FsLab journal is a simple Visual Studio template that makes it easy to do
interactive data analysis using F# Interactive and produce HTML or PDF
to document your research.

Next steps
----------

 * To see how things work, run `build run` from the terminal to start the journal
   runner in the background (or hit **F5** in Visual Studio). Executing this
   project will turn this F# script into a report.

 * To generate PDF from your experiments, you need to install `pdflatex` and
   have it accessible in the system `PATH` variable. Then you can run
   `build pdf` in the folder with this script (then check out `output` folder).

Sample experiment
-----------------

We start by referencing `Deedle` and `XPlot.GoogleCharts` libraries and then we
load the contents of *this* file:
*)

(*** define:loading ***)
open Deedle
open System.IO
open XPlot.GoogleCharts

let file = __SOURCE_DIRECTORY__ + "/Experiment1.fsx"
let contents = File.ReadAllText(file)
printfn "Loaded '%s' of length %d" file contents.Length
(*** include:loading ***)

(**
Now, we split the contents of the file into words, count the frequency of
words longer than 3 letters and turn the result into a Deedle series:
*)
let words =
  contents.Split(' ', '"', '\n', '\r', '*')
  |> Array.filter (fun s -> s.Length > 3)
  |> Array.map (fun s -> s.ToLower())
  |> Seq.countBy id
  |> series
(**
We can take the top 5 words occurring in this tutorial and see them in a chart:
*)
(*** define:grid ***)
words
|> Series.sort
|> Series.rev
|> Series.take 7
(*** include:grid ***)
(**
Finally, we can take the same 6 words and call `Chart.Column` to see them in a chart:
*)
(*** define:chart ***)
words
|> Series.sort
|> Series.rev
|> Series.take 7
|> Chart.Column
(*** include:chart ***)

(**
Summary
-------
An image is worth a thousand words:

![](http://imgs.xkcd.com/comics/hofstadter.png)
*)

