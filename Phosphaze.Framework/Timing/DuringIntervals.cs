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
 * Date of Creation: 6/12/2015
 * 
 * Description
 * ===========
 * DuringIntervals is a TimeChronometric that returns true at equally spaced intervals in time
 * starting at the given start time and ending at the given end time.
 */

#endregion

#region Using Statements

using System;

#endregion

namespace Phosphaze.Framework.Timing
{
    public class DuringIntervals : TimeChronometric
    {

        public override double StartTime { get { return start; } }

        public override double EndTime { get { return end; } }

        double interval, start, end;

        public DuringIntervals(double interval)
            : this(interval, 0, Double.PositiveInfinity) { }

        public DuringIntervals(double interval, double start)
            : this(interval, start, Double.PositiveInfinity) { }

        public DuringIntervals(double interval, double start, double end)
        {
            this.interval = interval;
            this.start = start;
            this.end = end;
        }

        public override bool Active(ChronometricEntity entity)
        {
            return ((entity.LocalTime - start) % (2 * interval)) < interval && 
                   start <= entity.LocalTime && entity.LocalTime <= end;
        }

    }
}
