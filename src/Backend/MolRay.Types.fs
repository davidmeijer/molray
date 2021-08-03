module MolRay.Types

open System

// ===========================
// Vector
// ===========================

[<Struct>]
type Vector =
    {
        X : float
        Y : float
        Z : float
    }
    
    // Multiply vector by number
    static member (*) (number : float, vector : Vector) =
        {
            X = number * vector.X
            Y = number * vector.Y
            Z = number * vector.Z
        }
    
    // Subtract vectors
    static member (-) (vector_a : Vector, vector_b : Vector) =
        {
            X = vector_a.X - vector_b.X
            Y = vector_a.Y - vector_b.Y
            Z = vector_a.Z - vector_b.Z
        }
    
    // Add vectors
    static member (+) (vector_a : Vector, vector_b : Vector) =
        {
            X = vector_a.X + vector_b.X
            Y = vector_a.Y + vector_b.Y
            Z = vector_a.Z + vector_b.Z
        }
    
    // Vector exponentiation
    static member Power (vector : Vector, power : float) =
        {
            X = vector.X ** power
            Y = vector.Y ** power
            Z = vector.Z ** power 
        }
    
    // Vector summation
    static member Sum (vector : Vector) =
        vector.X + vector.Y + vector.Z
    
    // Dot product between vectors
    static member Dot (vector_a : Vector, vector_b : Vector) =
        vector_a.X * vector_b.X + vector_a.Y * vector_b.Y + vector_a.Z * vector_b.Z
    
    // Magnitude of vector   
    static member Magnitude (vector : Vector) =
        sqrt (Vector.Sum (Vector.Power (vector, 2.0)))
    
    // Normalize vector
    static member Normalize (vector : Vector) =
        let magnitude = Vector.Magnitude vector
        
        let norm =
            if magnitude = 0.0 then
                infinity
            else
                1.0 / magnitude
                
        norm * vector 
    
    // Cross product between vectors
    static member Cross (vector_a : Vector, vector_b : Vector) =
        {
            X = vector_a.Y * vector_a.Z - vector_a.Z * vector_b.Y
            Y = vector_a.Z * vector_b.X - vector_a.X * vector_b.Z
            Z = vector_a.X * vector_b.Y - vector_a.Y * vector_b.X
        }
        
    // Rotate vector
    static member Transform (vector : Vector, axis : Axis, degree : float) =
        let rotationalMatrix = axis.RotationalMatrix(degree)
        
        let X = Vector.Dot(rotationalMatrix.RowX, vector)
        let Y = Vector.Dot(rotationalMatrix.RowY, vector)
        let Z = Vector.Dot(rotationalMatrix.RowZ, vector)
        
        {
            X = X
            Y = Y
            Z = Z
        }
        

and Axis =
    | X
    | Y
    | Z
    
    with
    
    member axis.RotationalMatrix (degree : float) =
        match axis with
        | X ->
            {
                RowX = {
                    X = 1.0
                    Y = 0.0
                    Z = 0.0
                }
                RowY = {
                    X = 0.0
                    Y = Math.Cos(degree)
                    Z = -Math.Sin(degree)
                }
                RowZ = {
                    X = 0.0
                    Y = Math.Sin(degree)
                    Z = Math.Cos(degree)
                }
            }
            
        | Y ->
            {
                RowX = {
                    X = Math.Cos(degree)
                    Y = 0.0
                    Z = Math.Sin(degree)
                }
                RowY = {
                    X = 0.0
                    Y = 1.0
                    Z = 0.0
                }
                RowZ = {
                    X = -Math.Sin(degree)
                    Y = 0.0
                    Z = Math.Cos(degree)
                }
            }
            
        | Z ->
            {
                RowX = {
                    X = Math.Cos(degree)
                    Y = -Math.Sin(degree)
                    Z = 0.0
                }
                RowY = {
                    X = Math.Sin(degree)
                    Y = Math.Cos(degree)
                    Z = 0.0
                }
                RowZ = {
                    X = 0.0
                    Y = 0.0
                    Z = 1.0
                }
            }
        
and Matrix =
    {
        RowX : Vector
        RowY : Vector
        RowZ : Vector
    }
    
    
        
// ===========================
// Color
// ===========================

[<Struct>]
type Color =
    {
        R : float
        B : float
        G : float
    }
    
    // Scale color
    static member Scale (number : float, color : Color) =
        {
            R = number * color.R
            G = number * color.G
            B = number * color.B
        }
    
    // Add colors
    static member (+) (color_a : Color, color_b : Color) =
        {
            R = color_a.R + color_b.R
            G = color_a.G + color_b.G
            B = color_a.B + color_b.B
        }
    
    // Multiply colors
    static member (*) (color_a : Color, color_b : Color) =
        {
            R = color_a.R * color_b.R
            G = color_a.G * color_b.G
            B = color_a.B * color_b.B
        }
        
    // ===========================
    // Color definitions
    // ===========================
    
    static member White =
        {
            R = 1.0
            G = 1.0
            B = 1.0
        }
        
    static member Grey =
        {
            R = 0.5
            G = 0.5
            B = 0.5
        }
    
    static member Black =
        {
            R = 0.0
            G = 0.0
            B = 0.0
        }
        
    static member Red =
        {
            R = 1.0
            G = 0.0
            B = 0.0
        }
        
    static member Green =
        {
            R = 0.0
            G = 1.0
            B = 0.0
        }
        
    static member Blue =
        {
            R = 0.0
            G = 0.0
            B = 1.0
        }
        
    static member Yellow =
        {
            R = 1.0
            G = 1.0
            B = 0.0
        }
        
// ===========================
// Camera
// ===========================

type Camera (position : Vector, lookAt : Vector) =
    
    // Move camera forward
    let forward =
        Vector.Normalize (lookAt - position)
    
    // Move camera down
    let down =
        {
            X = 0.0
            Y = -1.0
            Z = 0.0
        }
        
    // Move camera right
    let right =
        1.5 * Vector.Normalize (Vector.Cross (forward, down))
        
    // Move camera up
    let up =
        1.5 * Vector.Normalize (Vector.Cross (forward, right))
        
    member camera.Position = position
    member camera.Forward = forward
    member camera.Up = up
    member cemera.Right = right
        
// ===========================
// Ray
// ===========================

type Ray =
    {
        Start : Vector
        Direction : Vector
    }
    
type Surface =
    abstract Diffuse : Vector -> Color
    abstract Specular : Vector -> Color
    abstract Reflect : Vector -> float
    abstract Roughness : float
    
[<Struct>]
type Intersection =
    {
        Object : Object
        Ray : Ray
        Distance : float
    }

and Object =
    abstract Surface : Surface 
    abstract Intersect : Ray -> Intersection
    abstract Normal : Vector -> Vector

// ===========================
// Scene
// ===========================

type Light =
    {
        Position : Vector
        Color : Color
    }

type Scene =
    {
        Name : string
        Objects : Object list
        Lights : Light list
        Camera : Camera
    }

    
    