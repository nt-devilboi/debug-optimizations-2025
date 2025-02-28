using System;
using System.Threading.Tasks;

namespace JPEG;

public class DCT
{
    private static readonly double alpha = 1 / Math.Sqrt(2);

    public static double[,] DCT2D(double[,] input)
    {
        var height = input.GetLength(0);
        var width = input.GetLength(1);
        var coeffs = new double[width, height];
        var threads = 3;
        Parallel.For(0, threads, (t) =>
        {
            var alphaU = alpha;
            for (var u = width / threads * t; u < width / threads * (t + 1); u++)
            {
                var alphaV = alpha;
                for (var v = height / threads * t; v < height / threads * (t + 1); v++)
                {
                    var sum = 0d;
                    for (var x = width / threads * t; x < width / threads * (t + 1); x++)
                    for (var y = height / threads * t; y < height / threads * (t + 1); y++)
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
        });


        return coeffs;
    }

    public static void IDCT2D(double[,] coeffs, double[,] output)
    {
        var height = coeffs.GetLength(0);
        var width = coeffs.GetLength(1);
        var threads = 4;

        Parallel.For(0, threads, (t) =>
        {
            for (var x = width / threads * t; x < width / threads * (t + 1); x++)
            {
                for (var y = height / threads * t; y < height / threads * (t + 1); y++)
                {
                    var sum = 0d;
                    var alphaU = alpha;
                    for (var u = width / threads * t; u < width / threads * (t + 1); u++)
                    {
                        var alphaV = alpha;
                        for (var v = height / threads * t; v < height / threads * (t + 1); v++)
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
        });
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