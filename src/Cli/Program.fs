module Program

open System
open MolRay

let runProgram () =
    let pathToSDF = Environment.GetCommandLineArgs().[2]
    printfn $"Parsing... {pathToSDF}"
    let molecule = Parser.ReadSDF(pathToSDF)
    if molecule.Atoms.Length = 0 then printfn "No atoms to render!"
    else Render.drawMolecule(molecule)
    0

[<EntryPoint>]
runProgram() |> printfn "%A"
