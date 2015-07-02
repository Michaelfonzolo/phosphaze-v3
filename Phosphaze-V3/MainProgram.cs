#define TESTING_SOMETHING_STUPID
// #undef TESTING_SOMETHING_STUPID
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
 * This is the main file that is run when the program starts up.
 */

#endregion

#region Using Statements

using System;
using System.Collections.Generic;
using System.Linq;
using Phosphaze_V3.Tests.Test001;
using Phosphaze_V3.Tests.Test002;

#endregion

namespace Phosphaze_V3
{
#if TESTING_SOMETHING_STUPID
    using Phosphaze_V3.Framework.Maths.Geometry;
    using Microsoft.Xna.Framework;
#endif
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class MainProgram
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if TESTING_SOMETHING_STUPID
            var v1 = new Vector2(1, 0);
            Console.WriteLine(v1);
            Console.WriteLine(VectorUtils.Rotate(v1, 90, Vector2.Zero));
#else
            using (var game = new Phosphaze(new Test002Engine()))
                game.Run();
#endif
        }
    }
#endif
}
