module MolRay.Objects

open MolRay.Types

// ===========================
// Sphere
// ===========================

let Sphere
    (
        center : Vector,
        radius : float,
        surface : Surface
    ) =
    {
        new Object with
            member this.Surface = surface
            member this.Normal position = Vector.Normalize (position - center)
            member this.Intersect ray =
                let eo = center - ray.Start
                let v = Vector.Dot (eo, ray.Direction)
                
                let distance =
                    if v < 0.0 then
                        infinity
                    else
                        let disc = radius * radius - (Vector.Dot (eo, eo) - (v * v))
                        
                        if disc < 0.0 then
                            infinity
                        else
                            v - (sqrt disc)
                
                {
                    Object = this
                    Ray = ray
                    Distance = distance
                }
    }
    
// ===========================
// Plane
// ===========================

let Plane
    (
        normal : Vector,
        offset : float,
        surface : Surface
    ) =
    {
        new Object with
            member this.Surface = surface
            member this.Normal position = normal
            member this.Intersect ray =
                let denom = Vector.Dot (normal, ray.Direction)
                
                let distance =
                    if denom > 0.0 then
                        infinity
                    else
                        (Vector.Dot (normal, ray.Start) + offset) / (- denom)
                
                {
                    Object = this
                    Ray = ray
                    Distance = distance
                }
    }