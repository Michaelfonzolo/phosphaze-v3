using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Maths
{
    public class ODESolver
    {

        public Func<double, double, double> Function { get; private set; }

        public double stepSize { get; private set; }

        public double initialX { get; private set; }

        public double initialY { get; private set; }

        public double currentX { get; private set; }

        public double currentY { get; private set; }

        public ODESolver(Func<double, double, double> f, double stepSize, double initialX, double initialY)
        {
            Function = f;
            this.stepSize = stepSize;
            this.initialX = initialX;
            this.initialY = initialY;
            this.currentX = initialX;
            this.currentY = initialY;
        }

        public double GetNext()
        {
            double k1, k2, k3, k4;
            double h_2 = stepSize / 2;
            currentX += stepSize;
            k1 = Function(currentX, currentY);
            k2 = Function(currentX + h_2, currentY + h_2 * k1);
            k3 = Function(currentX + h_2, currentY + h_2 * k2);
            k4 = Function(currentX + stepSize, currentY + stepSize * k3);
            currentY += stepSize / 6 * (k1 + 2 * (k2 + k3) + k4);
            return currentY;
        }
    }
}
