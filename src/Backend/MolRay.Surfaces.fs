module MolRay.Surfaces

open System
open MolRay.Types

// ===========================
// Shiny surface
// ===========================
let Shiny =
    { new Surface with
        member surface.Diffuse position = Color.White
        member surface.Specular position = Color.Grey
        member surface.Reflect position = 0.7
        member surface.Roughness = 250.0 }
    
// ===========================
// Matt surface
// ===========================
let Matt (surfaceColor : Color) =
    { new Surface with
        member surface.Diffuse position = surfaceColor
        member surface.Specular position = Color.Grey
        member surface.Reflect position = 0.0
        member surface.Roughness = 500.0 }

// ===========================
// Checkerboard surface
// ===========================
let CheckerBoard =
    { new Surface with
        member surface.Diffuse position =
            if (int (Math.Floor position.Z + Math.Floor position.X)) % 2 <> 0 then
                Color.White
            else
                Color.Black
            
        member surface.Specular position = Color.White
        
        member surface.Reflect position =
            if (int (Math.Floor position.Z + Math.Floor position.X)) % 2 <> 0 then
                0.1
            else
                0.7
        
        member surface.Roughness = 150.0 }