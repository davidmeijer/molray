module MolRay.Render

open Browser.Types
open Browser
open MolRay.Types
open MolRay.Tracer

let renderScene (scene : Scene) (x : int, y : int, width : int, height : int) =
    let canvas = document.getElementsByTagName("canvas").[0] :?> HTMLCanvasElement
    let ctx = canvas.getContext_2d()
    let img = ctx.createImageData(float width, float height)
    
    Render scene img.data (x, y, width, height)
    ctx.putImageData(img, float -x, float -y)