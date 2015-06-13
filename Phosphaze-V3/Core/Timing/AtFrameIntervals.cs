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
 * AtFrameIntervals is a FrameChronometric that returns true exactly once at given frame
 * intervals starting at a given start frame and ending at a given end frame.
 */

#endregion

#region Using Statements

using System;

#endregion

namespace Phosphaze_V3.Core.Timing
{
    public class AtFrameIntervals : FrameChronometric
    {

        public override int StartFrame { get { return start; } }

        public override int EndFrame { get { return end; } }

        int interval, start, end;

        public AtFrameIntervals(int interval)
            : this(interval, 0, int.MaxValue) { }

        public AtFrameIntervals(int interval, int start)
            : this(interval, start, int.MaxValue) { }

        public AtFrameIntervals(int interval, int start, int end)
        {
            this.interval = interval;
            this.start = start;
            this.end = end;
        }

        public override bool Active(ChronometricEntity entity)
        {
            int modded = entity.LocalFrame % interval;
            return 0 <= modded && modded <= interval && start <= entity.LocalFrame && entity.LocalFrame <= end;
        }

    }
}
