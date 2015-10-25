// GitHub URL: https://github.com/fernweh8000/TestUtilities

/* The MIT License (MIT)

Copyright (c) 2015 http://fernweh.jp

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. */

using System;

namespace Fernweh8000.TestUtilities
{
    /// <summary>
    /// <see cref="InvokeUtility">InvokeUtility</see> can infer type
    /// if argument is NullValue instead of null. (Immutable)
    /// </summary>
    public class NullValue
    {
        /// <summary>get Type assigned in constructor</summary>
        internal Type Type { get; private set; }

        public NullValue(Type type)
        {
            this.Type = type;
        }
    }
}