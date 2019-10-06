using System.Collections.Generic;


namespace INPTPZ1
{
    class Polynomial
    {
        public List<ComplexNumber> Coe { get; set; }

        public Polynomial()
        {
            Coe = new List<ComplexNumber>();
        }

        public Polynomial Derive()
        {
            Polynomial p = new Polynomial();
            for (int i = 1; i < Coe.Count; i++)
            {
                p.Coe.Add(Coe[i].Multiply(new ComplexNumber() { Real = i }));
            }

            return p;
        }

        public ComplexNumber Eval(ComplexNumber x)
        {
            ComplexNumber s = ComplexNumber.Zero;
            for (int i = 0; i < Coe.Count; i++)
            {
                ComplexNumber coef = Coe[i];
                ComplexNumber bx = x;
                int power = i;

                if (i > 0)
                {
                    for (int j = 0; j < power - 1; j++)
                        bx = bx.Multiply(x);

                    coef = coef.Multiply(bx);
                }

                s = s.Add(coef);
            }

            return s;
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < Coe.Count; i++)
            {
                s += Coe[i];
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
