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