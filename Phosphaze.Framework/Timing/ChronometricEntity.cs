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
 * ChronometricEntity is a base class that tracks the elapsed time and the elapsed
 * number of frames since the object's creation. It also implements a number of time
 * command wrapper methods that return true depending on different time conditions.
 */

#endregion


namespace Phosphaze.Framework.Timing
{
    public class ChronometricEntity
    {
        /// <summary>
        /// The number of frames since the creation of this entity.
        /// </summary>
        public int LocalFrame { get; private set; }

        /// <summary>
        /// The number of milliseconds since the creation of this entity.
        /// </summary>
        public double LocalTime { get; private set; }

        public ChronometricEntity()
        {
            LocalFrame = 0;
            LocalTime = 0;
        }

        /// <summary>
        /// Update the local time of this object.
        /// </summary>
        protected void UpdateTime(ServiceLocator serviceLocator)
        {
            LocalFrame++;
            LocalTime += serviceLocator.Engine.deltaTime;
        }

        /// <summary>
        /// Returns true exactly once at the given time.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool At(double time) 
        { 
            return new At(time).Active(this); 
        }

        /// <summary>
        /// Returns true exactly once at the given frame.
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public bool AtFrame(int frame) 
        { 
            return new AtFrame(frame).Active(this); 
        }

        /// <summary>
        /// Returns true before the given time.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool Before(double time) 
        { 
            return new Before(time).Active(this); 
        }

        /// <summary>
        /// Returns true before the given frame.
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public bool BeforeFrame(int frame) 
        { 
            return new BeforeFrame(frame).Active(this); 
        }

        /// <summary>
        /// Returns true after the given time.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool After(double time) 
        { 
            return new After(time).Active(this); 
        }

        /// <summary>
        /// Returns true after the given frame.
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public bool AfterFrame(int frame) 
        { 
            return new AfterFrame(frame).Active(this); 
        }

        /// <summary>
        /// Returns true during the given time interval (defined by the given start and end times).
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public bool From(double start, double end) 
        { 
            return new From(start, end).Active(this); 
        }

        /// <summary>
        /// Returns true during the given frame interval (defined by the given start and end frames).
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public bool FromFrames(int start, int end) 
        { 
            return new FromFrames(start, end).Active(this); 
        }

        /// <summary>
        /// Returns true outside the given time interval (defined by the given start and end times).
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public bool Outside(double start, double end) 
        { 
            return new Outside(start, end).Active(this); 
        }

        /// <summary>
        /// Returns true outside the given frame interval (defined by the given start and end frames).
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public bool OutsideFrames(int start, int end) 
        { 
            return new OutsideFrames(start, end).Active(this); 
        }

        /// <summary>
        /// Returns true exactly once at given intervals of time.
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public bool AtIntervals(double interval) 
        { 
            return new AtIntervals(interval).Active(this); 
        }

        /// <summary>
        /// Returns true exactly once at given intervals of time starting at the given start time.
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public bool AtIntervals(double interval, double start) 
        { 
            return new AtIntervals(interval, start).Active(this); 
        }

        /// <summary>
        /// Returns true exactly once at given intervals of time starting at the given start time and
        /// ending at the given end time.
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public bool AtIntervals(double interval, double start, double end) 
        { 
            return new AtIntervals(interval, start, end).Active(this); 
        }

        /// <summary>
        /// Returns true exactly once at given frame intervals.
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public bool AtFrameIntervals(int interval) 
        { 
            return new AtFrameIntervals(interval).Active(this); 
        }

        /// <summary>
        /// Returns true exactly once at given frame intervals starting at the given start frame.
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public bool AtFrameIntervals(int interval, int start) 
        { 
            return new AtFrameIntervals(interval, start).Active(this); 
        }

        /// <summary>
        /// Returns true exactly once at given frame intervals starting at the given start frame and
        /// ending at the given end frame.
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public bool AtFrameIntervals(int interval, int start, int end) 
        { 
            return new AtFrameIntervals(interval, start, end).Active(this); 
        }

        /// <summary>
        /// Returns true during equally-spaced intervals of time.
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public bool DuringIntervals(double interval) 
        { 
            return new DuringIntervals(interval).Active(this); 
        }

        /// <summary>
        /// Returns true during equally-spaced intervals of time starting at the given start time.
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public bool DuringIntervals(double interval, double start) 
        { 
            return new DuringIntervals(interval, start).Active(this); 
        }

        /// <summary>
        /// Returns true during equally-spaced intervals of time starting at the given start time
        /// and ending at the given end time.
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public bool DuringIntervals(double interval, double start, double end) 
        { 
            return new DuringIntervals(interval, start, end).Active(this); 
        }

        /// <summary>
        /// Returns true during equally-spaced frame intervals.
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public bool DuringFrameIntervals(int interval) 
        { 
            return new DuringFrameIntervals(interval).Active(this); 
        }

        /// <summary>
        /// Returns true during equally-spaced frame intervals starting at the given start frame.
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public bool DuringFrameIntervals(int interval, int start) 
        { 
            return new DuringFrameIntervals(interval, start).Active(this); 
        }

        /// <summary>
        /// Returns true during equally-spaced frame intervals starting at the given start frame
        /// and ending at the given end frame.
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public bool DuringFrameIntervals(int interval, int start, int end) 
        { 
            return new DuringFrameIntervals(interval, start, end).Active(this); 
        }

    }
}
