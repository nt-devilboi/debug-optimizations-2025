using System;
using System.Drawing;
using System.Drawing.Imaging;

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

    public static unsafe explicit operator Matrix(Bitmap bmp)
    {
        var height = bmp.Height - bmp.Height % 8;
        var width = bmp.Width - bmp.Width % 8;
        var matrix = new Matrix(height, width);

        var rec = new Rectangle(0, 0, bmp.Width, bmp.Height);
        var bitmapData = bmp.LockBits(rec, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

        var ptrstart = (byte*)bitmapData.Scan0;

        try
        {
            for (var j = 0; j < height; j++)
            {
                var row = ptrstart + j * bitmapData.Stride;
                for (var i = 0; i < width; i++)
                {
                    var offset = i * 3;
                    matrix.Pixels[j, i] = new Pixel(row[offset], row[offset + 1], row[i + 2], PixelFormat.RGB);
                }
            }
        }
        finally
        {
            bmp.UnlockBits(bitmapData);
        }


        return matrix;
    }

    public static unsafe explicit operator Bitmap(Matrix matrix)
    {
        var bmp = new Bitmap(matrix.Width, matrix.Height);
        var rec = new Rectangle(0, 0, matrix.Width, matrix.Height);
        var bitData = bmp.LockBits(rec, ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

        var ptrstart = (byte*)bitData.Scan0;

        for (var j = 0; j < bmp.Height; j++)
        {
            var row = ptrstart + j * bitData.Stride;
            for (var i = 0; i < bmp.Width; i++)
            {
                var offset = i * 3;
                var pixel = matrix.Pixels[j, i];
                row[offset] = (byte)pixel.B;
                row[offset + 1] = (byte)pixel.G;
                row[offset + 2] = (byte)pixel.R;
            }
        }

        bmp.UnlockBits(bitData);
        return bmp;
    }

    public static byte ToByte(double d)
    {
        double rounded = Math.Round(d);
        if (rounded < byte.MinValue) return byte.MinValue;
        if (rounded > byte.MaxValue) return byte.MaxValue;
        return (byte)rounded;
    }
}