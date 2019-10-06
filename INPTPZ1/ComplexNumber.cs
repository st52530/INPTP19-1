using System;

namespace INPTPZ1
{
    class ComplexNumber
    {
        public double Real { get; set; }
        public double Imaginary { get; set; }

        public readonly static ComplexNumber Zero = new ComplexNumber()
        {
            Real = 0,
            Imaginary = 0
        };

        public ComplexNumber Add(ComplexNumber b)
        {
            ComplexNumber a = this;
            return new ComplexNumber()
            {
                Real = a.Real + b.Real,
                Imaginary = a.Imaginary + b.Imaginary
            };
        }
        public ComplexNumber Subtract(ComplexNumber b)
        {
            ComplexNumber a = this;
            return new ComplexNumber()
            {
                Real = a.Real - b.Real,
                Imaginary = a.Imaginary - b.Imaginary
            };
        }

        public ComplexNumber Multiply(ComplexNumber b)
        {
            ComplexNumber a = this;
            double newReal = a.Real * b.Real - a.Imaginary * b.Imaginary;
            double newImaginary = a.Real * b.Imaginary + a.Imaginary * b.Real;
            
            return new ComplexNumber()
            {
                Real = newReal,
                Imaginary = newImaginary
            };
        }

        public ComplexNumber Divide(ComplexNumber b)
        {
            if (b.Equals(Zero))
            {
                throw new DivideByZeroException();
            }

            // Complex number B with negative imaginary part.
            ComplexNumber bNegative = new ComplexNumber()
            {
                Real = b.Real,
                Imaginary = -b.Imaginary
            };
            ComplexNumber calculationPart1 = this.Multiply(bNegative);
            double calculationPart2 = b.Real * b.Real + b.Imaginary * b.Imaginary;
            
            return new ComplexNumber()
            {
                Real = calculationPart1.Real / calculationPart2,
                Imaginary = calculationPart1.Imaginary / calculationPart2
            };
        }

        public override string ToString()
        {
            return $"({Real} + {Imaginary}i)";
        }

        public override bool Equals(object obj)
        {
            return obj is ComplexNumber number &&
                   Real == number.Real &&
                   Imaginary == number.Imaginary;
        }
    }
}
