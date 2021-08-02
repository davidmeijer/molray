module MolRay.Render

open System.Drawing
open MolRay.Chemistry
open MolRay.Types
open MolRay

let renderScene (scene : Scene) (x : int, y : int, width : int, height : int) =
    let bitmap =
        let bm = new Bitmap(width, height)
        Tracer.Render bm scene (x, y, width, height)
    
    bitmap.Save("./molecule.png")
    
let drawAtom (atom : Atom) =
    Objects.Sphere
        (
            atom.Coordinates,
            atom.GetNormalizedRadius(),
            Surfaces.Matt (atom.Type.GetColor())
        ) 

let drawMolecule (molecule : Molecule) =
    
    let sceneObjects =
        molecule.Atoms
        |> Array.map (fun atom -> drawAtom atom)
        |> Array.toList
        
    let viewPoint =
        {
            X = 3.0
            Y = 2.0
            Z = 30.0
        }
        
    let lookAt =
        {
            X = -1.0
            Y = 0.5
            Z = 0.0
        }
        
    let scene =
        {
            Objects = sceneObjects
            Lights = [
                {
                    Position = viewPoint
                    Color = Color.White
                }
            ]
            Camera = Camera(viewPoint, lookAt)
        }
    
    renderScene scene (0, 0, 2048, 2048)