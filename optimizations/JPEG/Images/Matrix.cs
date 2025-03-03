using System;
using System.Drawing;
using System.Threading.Tasks;

namespace JPEG.Images;

class Matrix
{
    public readonly Pixel[,] Pixels;
    public readonly int Height;
    public readonly int Width;

    public Matrix(int height, int width)
    {
        Height = height;
        Width = width;

        Pixels = new Pixel[height, width];
        for (var i = 0; i < height; ++i)
        for (var j = 0; j < width; ++j)
            Pixels[i, j] = new Pixel(0, 0, 0, PixelFormat.RGB);
    }

    public static explicit operator Matrix(Bitmap bmp)
    {
        var height = bmp.Height - bmp.Height % 8;
        var width = bmp.Width - bmp.Width % 8;
        var matrix = new Matrix(height, width);

        for (var j = 0; j < height; j++)
        {
            for (var i = 0; i < width; i++)
            {
                var pixel = bmp.GetPixel(i, j);

                matrix.Pixels[j, i].R = pixel.R;
                matrix.Pixels[j, i].G = pixel.G;
                matrix.Pixels[j, i].B = pixel.B;
            }
        }

        return matrix;
    }

    public static unsafe explicit operator Bitmap(Matrix matrix)
    {
        var bmp = new Bitmap(matrix.Width, matrix.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

        var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
        var bmpData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.WriteOnly, bmp.PixelFormat);

        var bytesPerPixel = 3;
        var stride = bmpData.Stride;
        var ptr = bmpData.Scan0;

        var scan0 = (byte*)ptr.ToPointer();
        for (int j = 0; j < bmp.Height; j++)
        {
            var row = scan0 + j * stride;
            for (int i = 0; i < bmp.Width; i++)
            {
                var pixel = matrix.Pixels[j, i];
                var r = (byte)pixel.R;
                var g = (byte)pixel.G;
                var b = (byte)pixel.B;

                var index = i * bytesPerPixel;
                row[index] = b;
                row[index + 1] = g;
                row[index + 2] = r;
            }
        }

        bmp.UnlockBits(bmpData);
        return bmp;
    }
}