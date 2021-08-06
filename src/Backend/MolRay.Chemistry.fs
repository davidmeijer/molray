module MolRay.Chemistry

open MolRay.Types

let getCentroid (coordinates : Vector []) =
    coordinates
    |> Array.fold (fun x y -> x + y) { X = 0.0; Y = 0.0; Z = 0.0 }
    |> (/) (float coordinates.Length)
     
let centerOnTarget (target : Vector) (coordinates : Vector []) =
    coordinates
    |> Array.map (fun coords -> coords - target)

type Molecule =
    {
        Atoms : Atom [] 
    }
    
    static member Transform (molecule : Molecule, axis : Axis, degree : float) =
        {
            Atoms = molecule.Atoms |> Array.map (fun atom -> atom.Transform (axis, degree))
        }
    
    static member Center (molecule : Molecule) =
        
        let getCoordinates molecule = molecule.Atoms |> Array.map (fun atom -> atom.Coordinates)
        let centroid = getCentroid (getCoordinates molecule)
        
        let centeredAtoms =
            getCoordinates molecule
            |> centerOnTarget centroid 
            |> Array.zip molecule.Atoms
            |> Array.map (fun (atom, newCoordinates) -> { atom with Coordinates = newCoordinates })
        
        {
            Atoms = centeredAtoms
        }
    
and Atom =
    {
        Type : AtomType
        Coordinates : Vector
    }
    
    member atom.Transform (axis : Axis, degree : float) =
        {
            Type = atom.Type
            Coordinates = Vector.Transform (atom.Coordinates, axis, degree)
        }
    
    member atom.GetNormalizedRadius () =
        atom.Type.GetRadius() / AtomType.H.GetRadius()
    
    static member FromString (atomType : string) =
        match atomType with
        | "H" -> AtomType.H
        | "C" -> AtomType.C
        | "N" -> AtomType.N
        | "O" -> AtomType.O
        | "S" -> AtomType.S
        | _ -> failwith $"Unsupported atom type: {atomType}"
    
and AtomType =
    | H
    | C
    | N
    | O
    | S

    member atomType.GetRadius () =
        match atomType with
        | H -> 25.0
        | C -> 30.0
        | N -> 30.0
        | O -> 30.0
        | S -> 35.0
        
    member atomType.GetColor () =
        match atomType with
        | H -> Color.White
        | C -> Color.Grey
        | N -> Color.Blue
        | O -> Color.Red
        | S -> Color.Yellow
        
     