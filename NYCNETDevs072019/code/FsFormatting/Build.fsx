#load "../packages/FSharp.Formatting/FSharp.Formatting.fsx"
open FSharp.Literate
open System.IO

let source = __SOURCE_DIRECTORY__
let template = Path.Combine(source, "template.html")

let script = Path.Combine(source, "FsLiterateExperiment.fsx")
let output = Literate.ProcessScriptFile(script, template)

// Write output
output.Parameters
|> List.filter(fun param -> (fst param) = "document")
|> List.map(snd)
|> List.head
|> (fun content -> File.WriteAllText(Path.Combine(source,"output.html"),content))
