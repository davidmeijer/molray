module MolRay.Parser

open System.IO
open NCDK.IO
open NCDK.Silent
open MolRay.Chemistry

let ReadSDF (path : string) : Molecule =
    use stream = new StreamReader(path)
    let reader =  new MDLV2000Reader(stream)
    let molecule = reader.Read(AtomContainer())
    let atoms = molecule.Atoms :> seq<_>
    
    {
        Atoms =
            atoms
            |> Seq.map (fun atom ->
                {
                    Type = Atom.FromString atom.Symbol
                    Coordinates =
                        let point = atom.Point3D.Value
                        {
                            X = point.X
                            Y = point.Y
                            Z = point.Z
                        }
                }
            )
            |> Seq.toArray
    }