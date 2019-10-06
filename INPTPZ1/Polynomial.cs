using System.Collections.Generic;

namespace INPTPZ1
{
    class Polynomial
    {
        public List<ComplexNumber> Coefficients { get; set; }

        public Polynomial()
        {
            Coefficients = new List<ComplexNumber>();
        }

        public Polynomial Derive()
        {
            Polynomial derivedPolynomial = new Polynomial();
            for (int i = 1; i < Coefficients.Count; i++)
            {
                derivedPolynomial.Coefficients.Add(Coefficients[i].Multiply(new ComplexNumber() { Real = i }));
            }

            return derivedPolynomial;
        }

        public ComplexNumber Evaluate(ComplexNumber x)
        {
            ComplexNumber result = ComplexNumber.Zero;

            for (int i = 0; i < Coefficients.Count; i++)
            {
                ComplexNumber coefficient = Coefficients[i];
                int power = i;

                if (power > 0)
                {
                    ComplexNumber multipliedX = x;
                    for (int j = 0; j < power - 1; j++)
                    {
                        multipliedX = multipliedX.Multiply(x);
                    }

                    coefficient = coefficient.Multiply(multipliedX);
                }

                result = result.Add(coefficient);
            }

            return result;
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < Coefficients.Count; i++)
            {
                s += Coefficients[i];
                if (i > 0)
                {
                    for (int j = 0; j < i; j++)
                    {
                        s += "x";
                    }
                }
                s += " + ";
            }
            return s;
        }
    }
}
