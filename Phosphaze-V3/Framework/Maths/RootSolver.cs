#region License

// Copyright (c) 2015 FCDM
// Permission is hereby granted, free of charge, to any person obtaining 
// a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the 
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is furnished 
// to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion

#region Header

/* Author: Michael Ala
 * Date of Creation: 6/30/2015
 * 
 * Description
 * ===========
 * A set of utility for finding the roots of various types of polynomials.
 */

#endregion

#region Using Statements

using System;

#endregion

namespace Phosphaze_V3.Framework.Maths
{
    public static class RootSolver
    {

        /// <summary>
        /// Solve for the real roots of a quadratic. (Ax^2 + Bx + C == 0)
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public static double[] Quadratic(double A, double B, double C)
        {
            double r = B * B - 4 * A * C;
            if (r < 0)
                return new double[] { };
            else if (r == 0)
                return new double[] { -B / (2 * A) };
            double A2 = 2 * A;
            return new double[] {
                ((r - B)/A2),
                ((-r - B)/A2)
            };
        }

        /// <summary>
        /// Solve for the real roots of a cubic. (Ax^3 + Bx^2 + Cx + D == 0)
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="D"></param>
        /// <returns></returns>
        public static double[] Cubic(double A, double B, double C, double D)
        {
            double B_over_A = B / A;
            double F, G, H;
            
            F = ((3*C / A) - (Math.Pow(B_over_A, 2.0))) / 3;
            G = ((2 * Math.Pow(B_over_A, 3.0)) - (9 * C * B_over_A / A) + (27 * D / A)) / 27;
            double G_over_2 = -G / 2.0;
            double G_over_2_sqrd = G_over_2 * G_over_2;
            H = G_over_2_sqrd + (Math.Pow(F / 3.0, 3.0));

            // Only one root is real.
            if (H > 0)
            {
                double sqrt_H = Math.Sqrt(H);
                double R, S, T, U;
                
                R = G_over_2 + sqrt_H;
                if (R < 0)
                    S = -Math.Pow(-R, 1.0 / 3.0);
                else
                    S = Math.Pow(R, 1.0 / 3.0);

                T = G_over_2 - sqrt_H;
                if (T < 0)
                    U = -Math.Pow(-T, 1.0 / 3.0);
                else
                    U = Math.Pow(T, 1.0 / 3.0);

                double X = S + U - B_over_A / 3.0;
                return new double[] { X };
            }
            // All 3 roots are real and equal.
            else if (F == G && G == H && H == 0)
            {
                double D_A = D / A;
                double X;
                if (D_A < 0)
                    X = Math.Pow(-D_A, 1.0 / 3.0);
                else
                    X = -Math.Pow(D_A, 1.0 / 3.0);
                return new double[] { X };
            }

            // All 3 roots are real.
            double I, J, K, L, M, N, P;
            I = Math.Sqrt(G_over_2_sqrd - H);
            J = Math.Pow(I, 1.0 / 3.0);
            K = Math.Acos(-(G / (2 * I))) / 3.0;
            L = -J;
            M = Math.Cos(K);
            N = Math.Sqrt(3) * Math.Sin(K);
            P = -B_over_A / 3.0;

            double X1, X2, X3;
            X1 = 2*J*Math.Cos(K) + P;
            X2 = L*(M + N) + P;
            X3 = L*(M - N) + P;

            return new double[] { X1, X2, X3 };
        }
    }
}
