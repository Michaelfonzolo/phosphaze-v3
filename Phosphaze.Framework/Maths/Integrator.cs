using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Maths
{
    public static class Integrator
    {

        // Note to self: Never use Romberg's method in the future, it is utter garbage.

        public static double Simpsons(Func<double, double> f, double a, double b, int n)
        {
            if (n % 2 != 0)
                throw new ArgumentException("The number of steps must be even.");
            if (a == b)
                return 0;
            double s = (b - a) / n, alpha = s / 3.0, interval = s, m = 4.0;
            double sum = f(a) + f(b);

            for (int i = 0; i < n - 1; i++)
            {
                sum += m * f(a + interval);
                m = 6 - m;
                interval += s;
            }

            return alpha * sum;
        }

    }
}