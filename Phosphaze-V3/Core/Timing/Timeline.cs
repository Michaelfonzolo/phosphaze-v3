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
 * A Timeline is an object that sorts a number of chronometrics chronologically and
 * updates them according to a given entity.
 */

#endregion

using System.Collections.Generic;

namespace Phosphaze_V3.Core.Timing
{
    public class Timeline
    {

        /// <summary>
        /// The list of all TimeChronometrics.
        /// </summary>
        private List<TimeChronometric> timeChronometrics = new List<TimeChronometric>();

        /// <summary>
        /// The list of all FrameChronometrics.
        /// </summary>
        private List<FrameChronometric> frameChronometrics = new List<FrameChronometric>();

        /// <summary>
        /// The list of currently active TimeChronometrics.
        /// </summary>
        private List<TimeChronometric> currentlyActiveTimeChrs = new List<TimeChronometric>();

        /// <summary>
        /// The list of currently active FrameChronometrics.
        /// </summary>
        private List<FrameChronometric> currentlyActiveFrameChrs = new List<FrameChronometric>();

        /// <summary>
        /// Whether or not we have begun running or not.
        /// </summary>
        public bool begun { get; private set; }

        public Timeline() 
        {
            begun = false;
        }

        /// <summary>
        /// Add a chronometric to the Timeline.
        /// </summary>
        /// <param name="chronometric"></param>
        public void AddChronometric(TimeChronometric chronometric)
        {
            timeChronometrics.Add(chronometric);
        }

        /// <summary>
        /// Add a chronometric to the Timeline.
        /// </summary>
        /// <param name="chronometric"></param>
        public void AddChronometric(FrameChronometric chronometric)
        {
            frameChronometrics.Add(chronometric);
        }

        /// <summary>
        /// Begin running the Timeline.
        /// </summary>
        public void Begin()
        {
            begun = true;

            // Sort the chronometrics by their start positions.
            timeChronometrics.Sort((t1, t2) => (t1.StartTime.CompareTo(t2.StartTime)));
            frameChronometrics.Sort((f1, f2) => (f1.StartFrame.CompareTo(f2.StartFrame)));
        }

        /// <summary>
        /// Update the Timeline with a given entity.
        /// </summary>
        /// <param name="entity"></param>
        public void Update(ChronometricEntity entity)
        {
            // Append all new chronometrics.
            while (timeChronometrics.Count > 0 && timeChronometrics[0].StartTime <= entity.LocalTime)
            {
                currentlyActiveTimeChrs.Add(timeChronometrics[0]);
                timeChronometrics.RemoveAt(0);
            }

            while (frameChronometrics.Count > 0 && frameChronometrics[0].StartFrame <= entity.LocalFrame)
            {
                currentlyActiveFrameChrs.Add(frameChronometrics[0]);
                frameChronometrics.RemoveAt(0);
            }

            // Activate all currently active chronometrics.
            foreach (var timeChronometric in currentlyActiveTimeChrs)
                if (timeChronometric.action != null && timeChronometric.Active(entity))
                    timeChronometric.action(entity);

            foreach (var frameChronometric in currentlyActiveFrameChrs)
                if (frameChronometric.action != null && frameChronometric.Active(entity))
                    frameChronometric.action(entity);

            // Remove all previously active chronometrics.        
            currentlyActiveTimeChrs.RemoveAll(t => t.EndTime < entity.LocalTime);
            currentlyActiveFrameChrs.RemoveAll(f => f.EndFrame < entity.LocalFrame);

        }

    }
}
