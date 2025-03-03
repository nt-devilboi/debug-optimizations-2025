using System;
using System.Collections.Concurrent;

namespace JPEG;

public class DCT
{
    private static readonly double Alphas = 1 / Math.Sqrt(2);
    private static readonly ConcurrentDictionary<(double, double), double> bCahse = new();

    public static double[,] DCT2D(double[,] input)
    {
        int height = input.GetLength(0);
        int width = input.GetLength(1);
        var beta = 1d / width + 1d / height;
        var coeffs = new double[width, height];
        var alphau = Alphas;
        for (int u = 0; u < width; u++)
        {
            var alphav = Alphas;
            for (int v = 0; v < height; v++)
            {
                double sum = 0;
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        sum += BasisFunction(input[x, y], u, v, x, y, height, width);
                    }
                }

                coeffs[u, v] = sum * beta * alphau * alphav;

                alphav = 1;
            }

            alphau = 1;
        }

        return coeffs;
    }

    public static void IDCT2D(double[,] coeffs, double[,] output)
    {
        int height = coeffs.GetLength(0);
        int width = coeffs.GetLength(1);
        var beta = 1d / width + 1d / height;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                double sum = 0;
                var alphau = Alphas;
                for (int u = 0; u < width; u++)
                {
                    var alphav = Alphas;
                    for (int v = 0; v < height; v++)
                    {
                        sum += BasisFunction(coeffs[u, v], u, v, x, y, height, width) * alphau * alphav;

                        alphav = 1;
                    }

                    alphau = 1;
                }

                output[x, y] = sum * beta;
            }
        }
    }

    public static double BasisFunction(double a, double u, double v, double x, double y, int height, int width)
    {
        var b = Math.Cos(((2d * x + 1d) * u * Math.PI) / (2 * width));
        var c = Math.Cos(((2d * y + 1d) * v * Math.PI) / (2 * height));

        return a * b * c;
    }
}