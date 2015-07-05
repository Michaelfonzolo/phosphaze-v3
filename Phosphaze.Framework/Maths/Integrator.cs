using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Maths
{
    public static class Integrator
    {

        public static double Romberg(Func<double, double> f, double a, double b, int m, int n)
        {
            if (m == 0)
            {
                if (n == 0)
                    return 0.5 * (b - a) * (f(a) + f(b));
                else
                {
                    var result = 0.5 * Romberg(f, a, b, 0, n - 1);
                    var upper = 1 << (n - 1);

                    var h = (b - a)/(1 << n);
                    double sum = 0;
                    for (int k = 1; k <= upper; k++)
                    {
                        sum += f(a + (2 * k - 1) * h);
                    }
                    result += h * sum;
                    return result;
                }
            }
            else
            {
                var k = 1 << 2*m;
                return 1 / (k - 1) * (k * Romberg(f, a, b, m - 1, n) - Romberg(f, a, b, m - 1, n - 1));
            }
        }

        public static double Trapezoidal(Func<double, double> f, double a, double b, int n)
        {
            return Romberg(f, a, b, 0, n);
        }

        public static double Simpsons(Func<double, double> f, double a, double b, int n)
        {
            return Romberg(f, a, b, 1, n);
        }

        public static double Booles(Func<double, double> f, double a, double b, int n)
        {
            return Romberg(f, a, b, 2, n);
        }

    }
}