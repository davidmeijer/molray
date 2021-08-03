module MolRay.Render

open System.Drawing
open MolRay.Chemistry
open MolRay.Types
open MolRay

let renderScene (scenes : Scene []) (dirPath : string) (x : int, y : int, width : int, height : int) =
    
    for scene in scenes do
    
        let bitmap =
            let bm = new Bitmap(width, height)
            Tracer.Render bm scene (x, y, width, height)
        
        bitmap.Save($"{dirPath}{scene.Name}.png")
    
let drawAtom (atom : Atom) =
    Objects.Sphere
        (
            atom.Coordinates,
            atom.GetNormalizedRadius(),
            Surfaces.Matt (atom.Type.GetColor())
        ) 

let drawMolecule (molecule : Molecule) =
        
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
      
    let scenes =
        [|
            // for degree in [| 0.0 .. 1.0 .. 360.0 |] do
            for degree in [| 0.0 |] do
                let scene =
                    {
                        Name = $"scene_{degree}"
                        
                        Objects =
                            molecule.Transform(Axis.Y, degree).Atoms
                            |> Array.map (fun atom -> drawAtom atom)
                            |> Array.toList
                            
                        Lights = [
                            {
                                Position = viewPoint
                                Color = Color.White
                            }
                        ]
                        
                        Camera = Camera(viewPoint, lookAt)
                    }
                yield scene
        |]
   
    let dirPath = "./"
    renderScene scenes dirPath (0, 0, 4096, 4096)