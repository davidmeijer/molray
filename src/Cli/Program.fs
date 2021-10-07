module Program

open System
open MolRay

let runProgram () =
    let pathToSDF = Environment.GetCommandLineArgs().[1]
    let molecule = Parser.ReadSDF(pathToSDF)
    Render.drawMolecule(molecule)
    0

[<EntryPoint>]
runProgram() |> printfn "%A"