module PpmBitmapReader

open System
open System.IO
open RayTracer.Imaging
open RayTracer.Imaging.IO.Readers
open RayTracer.Library.Diagnostics
open RayTracer.Library.Mathematics
open RayTracer.Library.Utils

type PpmBitmapReader() =
    interface IBitmapReader with
        member this.Format = ImageFormat.Ppm
        member this.Read(source: Stream) = 
            use reader = new StreamReader(source)
            Assert.Equal ("P3", reader.ReadLine())
            
            let dims =
                reader.ReadLine().Split()
                |> Array.map int
                
            let maxColor = float32 (reader.ReadLine())

            let pixels =
                reader.ReadToEnd().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                |> Array.map (fun l -> l.Split ' '
                                        |> Array.map (fun c -> float32 c)
                                        |> Array.map (fun c -> c / maxColor))
                |> Array.map (fun arr -> new ColorRGB(arr.[0], arr.[1], arr.[2]))

            let bitmap = new Bitmap(dims.[0], dims.[1])

            pixels
                |> Array.iteri (fun i x -> bitmap.SetColor (i % dims.[0], i / dims.[0], &x))
            
            bitmap
