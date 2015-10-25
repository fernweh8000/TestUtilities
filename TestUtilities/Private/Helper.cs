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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fernweh8000.TestUtilities.Private
{
    internal static class Helper
    {
        internal static readonly object[] NoArguments = { };

        internal const BindingFlags InstanceMethodBindingAttr
            = BindingFlags.Instance
            | BindingFlags.Public
            | BindingFlags.NonPublic;

        internal const BindingFlags StaticMethodBindingAttr
            = BindingFlags.Static
            | BindingFlags.Public
            | BindingFlags.NonPublic;

        internal static object[] ArgumentsToArray(IEnumerable<object> args)
        {
            int argsCount = args.Count();
            object[] ret = new object[argsCount];
            for (int i = 0; i < argsCount; i++)
            {
                var arg = args.ElementAt(i);
                ret[i] = arg is NullValue ? null : arg;
            }
            return ret;
        }

        internal static ConstructorInfo GetConstructor(
            Type targetType,
            IEnumerable<object> args,
            IEnumerable<Type> parameterTypes)
        {
            ConstructorInfo ret;
            if (parameterTypes == null)
            {
                ret = InferConstructor(targetType, args);
            }
            else
            {
                ret = targetType.GetConstructor(
                    InstanceMethodBindingAttr,
                    null,
                    parameterTypes.ToArray(),
                    null);
            }
            return ret;
        }

        internal static ConstructorInfo InferConstructor(
            Type targetType,
            IEnumerable<object> args)
        {
            Type[] types = Helper.GetTypesFromArguments(args);
            ConstructorInfo ctor = null;
            if (types != null)
            {
                ctor = targetType.GetConstructor(
                    Helper.InstanceMethodBindingAttr,
                    null,
                    types,
                    null);
            }

            if (ctor == null)
            {
                int argsCount = args.Count();
                var ctors = targetType.GetConstructors(
                    Helper.InstanceMethodBindingAttr)
                    .Where(c => c.GetParameters().Count() == argsCount);
                if (ctors.Count() == 1)
                {
                    ctor = ctors.First();
                }
            }

            return ctor;
        }

        internal static MethodInfo GetMethod(
            Type targetType,
            string methodName,
            BindingFlags bindingAttr,
            IEnumerable<object> args,
            IEnumerable<Type> parameterTypes)
        {
            MethodInfo ret;
            if (parameterTypes == null)
            {
                ret = InferMethod(
                    targetType,
                    methodName,
                    bindingAttr,
                    args);
            }
            else
            {
                ret = targetType.GetMethod(
                    methodName,
                    bindingAttr,
                    null,
                    parameterTypes.ToArray(),
                    null);
            }
            return ret;
        }

        internal static MethodInfo InferMethod(
            Type targetType,
            string methodName,
            BindingFlags bindingAttr,
            IEnumerable<object> args)
        {
            MethodInfo m = null;
            try
            {
                m = targetType.GetMethod(
                    methodName, bindingAttr);
            }
            catch (AmbiguousMatchException)
            {
                // ignore
            }

            if (m == null)
            {
                Type[] types = Helper.GetTypesFromArguments(args);
                if (types != null)
                {
                    m = targetType.GetMethod(
                      methodName,
                      bindingAttr,
                      null,
                      types,
                      null);
                }
            }

            return m;
        }

        internal static Type[] GetTypesFromArguments(IEnumerable<object> args)
        {
            if (args.Any(arg => arg == null))
            {
                return null;
            }

            Type[] types = new Type[args != null ? args.Count() : 0];
            for (int i = 0; i < types.Length; i++)
            {
                var arg = args.ElementAt(i);
                var temp = arg as NullValue;
                if (temp != null)
                {
                    types[i] = temp.Type;
                }
                else
                {
                    types[i] = arg.GetType();
                }
            }
            return types;
        }
    }
}
