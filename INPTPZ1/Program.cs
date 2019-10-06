using System;
using System.Collections.Generic;
using System.Drawing;

namespace INPTPZ1
{
    /// <summary>
    /// This program produces Newton fractals.
    /// See more at: https://en.wikipedia.org/wiki/Newton_fractal
    /// </summary>
    class Program
    {

        private static readonly int DefaultSize = 300;

        private static readonly Color[] Colors = new Color[]
            {
                Color.Red,
                Color.Blue,
                Color.Green, 
                Color.Yellow, 
                Color.Orange, 
                Color.Fuchsia, 
                Color.Gold, 
                Color.Cyan, 
                Color.Magenta
            };

        private static Bitmap bitmap;

        private static double xMin, yMin, xStep, yStep;

        private static List<ComplexNumber> roots;
        private static Polynomial polynomial, derivedPolynomial;

        static void Main(string[] args)
        {
            InitialSetup(args);
            SetupPolynomials();
            
            ProduceFractals();

            SaveImage();
        }

        private static void InitialSetup(string[] args)
        {
            int imageSize = DefaultSize;
            if (args.Length != 0 && !int.TryParse(args[0], out imageSize))
            {
                imageSize = DefaultSize;
            }
            bitmap = new Bitmap(imageSize, imageSize);

            xMin = -1.5;
            double xMax = 1.5;
            yMin = -1.5;
            double yMax = 1.5;

            xStep = (xMax - xMin) / bitmap.Width;
            yStep = (yMax - yMin) / bitmap.Height;
        }

        private static void SetupPolynomials()
        {
            roots = new List<ComplexNumber>();
            polynomial = new Polynomial();
            polynomial.Coefficients.Add(new ComplexNumber() { Real = 1 });
            polynomial.Coefficients.Add(ComplexNumber.Zero);
            polynomial.Coefficients.Add(ComplexNumber.Zero);
            polynomial.Coefficients.Add(new ComplexNumber() { Real = 1 });
            derivedPolynomial = polynomial.Derive();

            Console.WriteLine(polynomial);
            Console.WriteLine(derivedPolynomial);
        }

        private static void ProduceFractals()
        {
            var maxRootNumber = 0;

            // for every pixel in image...
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    ComplexNumber coordinates = CalculateCoordinates(i, j);
                    int iterationCount = SolveUsingNewtonsIteration(ref coordinates);
                    int rootNumber = SolveRootNumber(ref maxRootNumber, coordinates);
                    ColorizePixel(j, i, iterationCount, rootNumber);
                }
            }
        }

        private static ComplexNumber CalculateCoordinates(int i, int j)
        {
            // find "world" coordinates of pixel
            double x = xMin + j * xStep;
            double y = yMin + i * yStep;

            ComplexNumber coordinates = new ComplexNumber()
            {
                Real = x,
                Imaginary = y
            };

            if (coordinates.Real == 0)
            {
                coordinates.Real = 0.0001;
            }
            if (coordinates.Imaginary == 0)
            {
                coordinates.Imaginary = 0.0001;
            }

            return coordinates;
        }

        /// <summary>
        /// find a solution of equation using newton's iteration
        /// </summary>
        private static int SolveUsingNewtonsIteration(ref ComplexNumber coordinates)
        {
            int iterationCount = 0;
            for (int i = 0; i < 30; i++)
            {
                var difference = polynomial.Evaluate(coordinates).Divide(derivedPolynomial.Evaluate(coordinates));
                coordinates = coordinates.Subtract(difference);

                var realPowered = Math.Pow(difference.Real, 2);
                var imaginaryPowered = Math.Pow(difference.Imaginary, 2);
                if (realPowered + imaginaryPowered >= 0.5)
                {
                    i--;
                }
                iterationCount++;
            }

            return iterationCount;
        }

        private static int SolveRootNumber(ref int maxRootNumber, ComplexNumber coordinates)
        {
            // find solution root number
            var known = false;
            var rootNumber = 0;
            for (int i = 0; i < roots.Count; i++)
            {
                var realPowered = Math.Pow(coordinates.Real - roots[i].Real, 2);
                var imaginaryPowered = Math.Pow(coordinates.Imaginary - roots[i].Imaginary, 2);
                if (realPowered + imaginaryPowered <= 0.01)
                {
                    known = true;
                    rootNumber = i;
                }
            }
            if (!known)
            {
                roots.Add(coordinates);
                rootNumber = roots.Count;
                maxRootNumber = rootNumber + 1;
            }

            return rootNumber;
        }

        /// <summary>
        /// colorize pixel according to root number
        /// </summary>
        private static void ColorizePixel(int x, int y, int iterationCount, int rootNumber)
        {
            Color color = Colors[rootNumber % Colors.Length];
            int r = Math.Min(Math.Max(0, color.R - iterationCount * 2), 255);
            int g = Math.Min(Math.Max(0, color.G - iterationCount * 2), 255);
            int b = Math.Min(Math.Max(0, color.B - iterationCount * 2), 255);
            color = Color.FromArgb(r, g, b);
            bitmap.SetPixel(x, y, color);
        }

        private static void SaveImage()
        {
            bitmap.Save("../../../out.png");
        }
    }
}
