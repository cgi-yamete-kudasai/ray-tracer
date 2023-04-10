module PpmBitmapWriter

open System.IO
open RayTracer.Imaging
open RayTracer.Imaging.IO.Writers

type PpmBitmapWriter() =
    interface IBitmapWriter with
        member this.Format = ImageFormat.Ppm
        member this.Write(destination, bitmap) = 
            use writer = new StreamWriter(destination)
            writer.WriteLine "P3"
            writer.WriteLine $"{bitmap.Width} {bitmap.Height}"
            writer.WriteLine "255"

            let b = fun f -> byte (f * 255.0f)

            for y = 0 to bitmap.Height - 1 do
                for x = 0 to bitmap.Width - 1 do
                    let color = bitmap.Get (x, y)
                    writer.WriteLine $"{b color.R} {b color.G} {b color.B}"
