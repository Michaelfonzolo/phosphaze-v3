using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors.Transitions
{
    public sealed class Mollifier
    {

        public static Mollifier Standard = new Mollifier(
            x => Math.Abs(x) < 1 ? Math.Exp(1.0 / (1.0 - x * x)) : 0, 
            0.443993816168079437823, -1.0, 1.0);

        public static Mollifier QuadraticStandard = new Mollifier(
            x => Math.Abs(x) < 1 ? Math.Exp(1.0 / (1.0 - Math.Pow(x, 4.0))) : 0, 
            0.561541934473828855126, -1.0, 1.0);

        Func<double, double> func;

        double integral;

        private double supportMin, supportMax;

        public double[] Support { get { return new double[] { supportMin, supportMax }; } }

        public Mollifier(Func<double, double> func, double integral, double supportMin, double supportMax)
        {
            this.func = func;
            this.integral = integral;
            this.supportMin = supportMin;
            this.supportMax = supportMax;
        }

        public double Call(double x) { return func(x)/integral; }

        public double Call(double n, double x) { return func(x / n) / n / integral; }

    }
}
