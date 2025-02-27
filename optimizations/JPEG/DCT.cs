using System;
using System.Collections.Generic;
using JPEG.Utilities;

namespace JPEG;

public class DCT
{
    private static readonly double alpha = 1 / Math.Sqrt(2);
    private static Dictionary<int, double> MemoX = new Dictionary<int, double>();
    public static double[,] DCT2D(double[,] input)
    {
        var height = input.GetLength(0);
        var width = input.GetLength(1);
        var coeffs = new double[width, height];

        var alphaU = alpha;
        for (var u = 0; u < width; u++)
        {
            var alphaV = alpha;
            for (var v = 0; v < height; v++)
            {
                var sum = 0d;
                for (var x = 0; x < width; x++)
                for (var y = 0; y < height; y++)
                {
                    var b = Math.Cos(((2d * x + 1d) * u * Math.PI) / (2 * width));
                    var c = Math.Cos(((2d * y + 1d) * v * Math.PI) / (2 * height));
                    var answer = b * c * input[u, v];
                    sum += answer;
                }

                coeffs[u, v] = sum * Beta(height, width) * alphaU * alphaV;
                alphaV = 1;
            }

            alphaU = 1;
        }

        return coeffs;
    }

    public static void IDCT2D(double[,] coeffs, double[,] output)
    {
        var height = coeffs.GetLength(0);
        var width = coeffs.GetLength(1);
    

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                var sum = 0d;
                var alphaU = alpha;
                for (var u = 0; u < width; u++)
                {
                    var alphaV = alpha;
                    for (var v = 0; v < height; v++)
                    {
                        var b = Math.Cos(((2d * x + 1d) * u * Math.PI) / (2 * width));
                        var c = Math.Cos(((2d * y + 1d) * v * Math.PI) / (2 * height));
                        var answer = b * c * coeffs[u, v] * alphaU * alphaV;

                        sum += answer;
                        alphaV = 1;
                    }

                    alphaU = 1;
                }


                output[x, y] = sum * Beta(coeffs.GetLength(0), coeffs.GetLength(1));
            }
        }
    }

    public static double BasisFunction(double a, double u, double v, double x, double y, int height, int width)
    {
        var b = Math.Cos(((2d * x + 1d) * u * Math.PI) / (2 * width));
        var c = Math.Cos(((2d * y + 1d) * v * Math.PI) / (2 * height));

        return a * b * c;
    }

    private static double Alpha(int u)
    {
        if (u == 0)
            return 1 / alpha;
        return 1;
    }

    private static double Beta(int height, int width)
    {
        return 1d / width + 1d / height;
    }
}