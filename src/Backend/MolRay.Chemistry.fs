module MolRay.Chemistry

open MolRay.Types

type Molecule =
    {
        Atoms : Atom [] 
    }
    
and Atom =
    {
        Type : AtomType
        Coordinates : Vector
    }
    
    member atom.GetNormalizedRadius () =
        atom.Type.GetRadius() / AtomType.H.GetRadius()
    
    static member FromString (atomType : string) =
        match atomType with
        | "H" -> AtomType.H
        | "C" -> AtomType.C
        | "N" -> AtomType.N
        | "O" -> AtomType.O
        | _ -> failwith $"Unsupported atom type: {atomType}"
    
and AtomType =
    | H
    | C
    | N
    | O
    
    // https://en.wikipedia.org/wiki/Atomic_radius
    member atomType.GetRadius () =
        match atomType with
        | H -> 25.0
        | C -> 70.0
        | N -> 65.0
        | O -> 60.0
        
    member atomType.GetColor () =
        match atomType with
        | H -> Color.White
        | C -> Color.Black
        | N -> Color.Blue
        | O -> Color.Red 