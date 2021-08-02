module MolRay.Render

open Browser.Types
open Browser
open MolRay.Chemistry
open MolRay.Types
open MolRay.Tracer
open MolRay

let renderScene (scene : Scene) (x : int, y : int, width : int, height : int) =
    let canvas = document.getElementsByTagName("canvas").[0] :?> HTMLCanvasElement
    let ctx = canvas.getContext_2d()
    let img = ctx.createImageData(float width, float height)
    
    Render scene img.data (x, y, width, height)
    ctx.putImageData(img, float -x, float -y)
    
    ctx
    
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
            Z = 4.0
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
        
    renderScene scene (0, 0, 512, 512) |> ignore // No output is currently shown
    
    ()