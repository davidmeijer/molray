module MolRay.Tracer

open System
open ShellProgressBar
open MolRay.Types

// ===========================
// Tracer
// ===========================
let SCENE_DEFAULT_COLOR = Color.Black
let MAX_RAYTRACING_DEPTH = 5

let FindNearestIntersection (ray : Ray) (scene : Scene) =
    match scene.Objects with
    | [] -> None 
    | head::tail ->
        let mutable nearestIntersection = head.Intersect ray
        for object in tail do
            let nextIntersection = object.Intersect ray
            if nextIntersection.Distance < nearestIntersection.Distance then
                nearestIntersection <- nextIntersection
        Some nearestIntersection
            
let TestRay (ray : Ray) (scene : Scene) =
    match FindNearestIntersection ray scene with
    | None -> None
    | Some intersection ->
        if intersection.Distance = infinity then None
        else Some intersection.Distance
            
let rec TraceRay (ray : Ray) (scene : Scene) (depth : int) =
    match FindNearestIntersection ray scene with
    | None -> { Color.White with Transparent = true }
    | Some intersection ->
        if intersection.Distance = infinity then { Color.White with Transparent = true }
        else Shade intersection scene depth
            
and Shade (intersection : Intersection) (scene : Scene) (depth : int) =
    let direction = intersection.Ray.Direction
    let position = intersection.Distance * direction + intersection.Ray.Start
    let normal = intersection.Object.Normal position
    let reflectDirection = direction - 2.0 * Vector.Dot (normal, direction) * normal
    let naturalColor = SCENE_DEFAULT_COLOR + (GetNaturalColor (intersection.Object, position, normal, reflectDirection, scene))
    let reflectedColor =
        if depth >= MAX_RAYTRACING_DEPTH then Color.Grey
        else GetReflectionColor (intersection.Object, position + (0.001 * reflectDirection), reflectDirection, scene, depth)
    naturalColor + reflectedColor

and GetReflectionColor(object : Object, position : Vector, rayDirection : Vector, scene : Scene, depth : int) =
    Color.Scale(object.Surface.Reflect position, TraceRay { Start = position; Direction = rayDirection } scene (depth + 1))
        
and GetNaturalColor(object : Object, position : Vector, normal : Vector, rayDirection : Vector, scene : Scene) =
    let addLight
        ( object : Object, position : Vector, normal : Vector, rayDirection : Vector, scene : Scene, color : Color, light : Light) =
        let lightDirection = light.Position - position
        let lightVector = Vector.Normalize lightDirection
        let neatIntersection = TestRay { Start = position; Direction = lightVector } scene
        let isInShadow =
            match neatIntersection with
            | None -> false
            | Some direction -> not (direction > Vector.Magnitude lightDirection)
        
        if isInShadow then color
        else
            let illumination = Vector.Dot (lightVector, normal)
            let lightColor =
                if illumination > 0.0 then Color.Scale (illumination, light.Color)
                else SCENE_DEFAULT_COLOR
                    
            let specular = Vector.Dot (lightVector, Vector.Normalize rayDirection)
            
            let specularColor =
                if specular > 0.0 then Color.Scale (Math.Pow (specular, object.Surface.Roughness), light.Color)
                else SCENE_DEFAULT_COLOR
                    
            color + object.Surface.Diffuse position * lightColor + object.Surface.Specular position * specularColor
        
    match scene.Lights with
    | [] -> SCENE_DEFAULT_COLOR
    | lights ->
        let mutable color = SCENE_DEFAULT_COLOR
        for light in lights do
            color <- addLight (object, position, normal, rayDirection, scene, color, light)
        color
        
let GetPoint (x : int) (y : int) (width : int) (height : int) (camera : Camera) =
    let RecenterX x = (float x - (float width / 2.0)) / (2.0 * float width)
    let RecenterY y = - (float y - (float height / 2.0)) / (2.0 * float height)
    Vector.Normalize (camera.Forward + RecenterX x * camera.Right + RecenterY y * camera.Up)
    
let Render (bitmap : Drawing.Bitmap) (scene : Scene) (x : int, y : int, width : int, height : int) =
    let getProgressBar (maxTicks : int) =
        let options =
            ProgressBarOptions(
                ForegroundColor = ConsoleColor.Yellow,
                BackgroundColor = ConsoleColor.DarkGray,
                ProgressCharacter = '-' )
        let progressBar = new ProgressBar(maxTicks, $"Rendering scene {scene.Name}...", options)
        progressBar
        
    let progressBar = getProgressBar height
    use progressBar = progressBar
    let clamp (value : float) =
        Math.Floor (255.0 * Math.Min(value, 1.0)) |> int
    for y = y to height - 1 do
        for x = x to width - 1 do
            let direction = GetPoint x y width height scene.Camera
            let ray = { Start = scene.Camera.Position; Direction = direction  }
            let color =
                let color = TraceRay ray scene 0
                if color.Transparent then Drawing.Color.Transparent
                else Drawing.Color.FromArgb(clamp color.R, clamp color.G, clamp color.B)
            bitmap.SetPixel(x, y, color)
        progressBar.Tick()  
    bitmap