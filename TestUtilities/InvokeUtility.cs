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

using Fernweh8000.TestUtilities.Private;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Fernweh8000.TestUtilities
{
    /// <summary>
    /// Utility to access private method
    /// </summary>
    /// <typeparam name="TTarget">target type to invoke method. TTarget must be itself or sub class of TDeclaring</typeparam>
    /// <typeparam name="TDeclaring">declearing type of method</typeparam>
    public class InvokeUtility<TTarget, TDeclaring> where TTarget : TDeclaring
    {
        /// <summary>
        /// invoke instance method with return value
        /// </summary>
        /// <param name="name">method name</param>
        /// <param name="args">method arguments</param>
        public static object Invoke(
            string name,
            params object[] args)
        {
            return Invoke(
                constructorArguments: Helper.NoArguments,
                constructorParameterTypes: null,
                name: name,
                args: args,
                parameterTypes: null);
        }

        /// <summary>
        /// invoke instance method with return value
        /// </summary>
        /// <param name="name">method name</param>
        /// <param name="args">method arguments</param>
        /// <param name="parameterTypes">method parameter types. infer types if null</param>
        public static object Invoke(
          string name,
          IEnumerable<object> args,
          IEnumerable<Type> parameterTypes)
        {
            return Invoke(
                constructorArguments: Helper.NoArguments,
                constructorParameterTypes: null,
                name: name,
                args: args,
                parameterTypes: parameterTypes);
        }

        /// <summary>
        /// invoke instance method with return value
        /// </summary>
        /// <param name="constructorArguments">constructor arguments</param>
        /// <param name="name">method name</param>
        /// <param name="args">method arguments</param>
        public static object Invoke(
            IEnumerable<object> constructorArguments,
            string name,
            params object[] args)
        {
            return Invoke(
                constructorArguments,
                null,
                name,
                args,
                null);
        }

        /// <summary>
        /// invoke instance method with return value
        /// </summary>
        /// <param name="constructorArguments">constructor arguments</param>
        /// <param name="constructorParameterTypes">constructor parameter types. infer types if null</param>
        /// <param name="name">method name</param>
        /// <param name="args">method arguments</param>
        public static object Invoke(
            IEnumerable<object> constructorArguments,
            IEnumerable<Type> constructorParameterTypes,
            string name,
            params object[] args)
        {
            return Invoke(
                constructorArguments,
                constructorParameterTypes,
                name,
                args,
                null);
        }

        /// <summary>
        /// invoke instance method with return value
        /// </summary>
        /// <param name="constructorArguments">constructor arguments</param>
        /// <param name="name">method name</param>
        /// <param name="args">method arguments</param>
        /// <param name="parameterTypes">method parameter types. infer types if null</param>
        public static object Invoke(
            IEnumerable<object> constructorArguments,
            string name,
            IEnumerable<object> args,
            IEnumerable<Type> parameterTypes)
        {
            return Invoke(
                constructorArguments,
                null,
                name,
                args,
                parameterTypes);
        }

        /// <summary>
        /// invoke instance method with return value
        /// </summary>
        /// <param name="constructorArguments">constructor arguments</param>
        /// <param name="constructorParameterTypes">constructor parameter types. infer types if null</param>
        /// <param name="name">method name</param>
        /// <param name="args">method arguments</param>
        /// <param name="parameterTypes">method parameter types. infer types if null</param>
        public static object Invoke(
            IEnumerable<object> constructorArguments,
            IEnumerable<Type> constructorParameterTypes,
            string name,
            IEnumerable<object> args,
            IEnumerable<Type> parameterTypes)
        {
            if (constructorArguments == null)
            {
                throw new ArgumentNullException("constructorArguments");
            }

            MethodInfo m = Helper.GetMethod(
                typeof(TDeclaring),
                name,
                Helper.InstanceMethodBindingAttr,
                args,
                parameterTypes);

            if (m == null)
            {
                throw new MissingMethodException(
                  typeof(TDeclaring).ToString(), name);
            }

            var obj = InvokeUtility<TTarget>.New(
                constructorArguments,
                constructorParameterTypes);

            try
            {
                return m.Invoke(
                    obj, args == null ? null : Helper.ArgumentsToArray(args));
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }
    }

    /// <summary>
    /// Utility to access private method
    /// </summary>
    /// <typeparam name="TTarget">target type to invoke method</typeparam>
    public class InvokeUtility<TTarget>
        : InvokeUtility<TTarget, TTarget>
    {
        /// <summary>
        /// invoke constructor to get instance
        /// </summary>
        /// <param name="args">method arguments</param>
        /// <returns>TTarget type new instance</returns>
        public static TTarget New(params object[] args)
        {
            return InvokeUtility<TTarget>.New(args, null);
        }

        /// <summary>
        /// invoke constructor to get instance
        /// </summary>
        /// <param name="args">method arguments</param>
        /// <param name="parameterTypes">constructor parameter types. infer types if null</param>
        /// <returns>TTarget type new instance</returns>
        public static TTarget New(IEnumerable<object> args, IEnumerable<Type> parameterTypes)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            ConstructorInfo ctor = Helper.GetConstructor(
                typeof(TTarget), args, parameterTypes);

            if (ctor == null)
            {
                throw new MissingMethodException(
                    typeof(TTarget).Name, ".ctor");
            }

            try
            {

                return (TTarget)ctor.Invoke(
                    args == null ? null : Helper.ArgumentsToArray(args));
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// invoke static method with return value
        /// </summary>
        /// <param name="name">method name</param>
        /// <param name="args">method arguments</param>
        public static object InvokeStatic(
            string name,
            params object[] args)
        {
            return InvokeStatic(name, args, null);
        }

        /// <summary>
        /// invoke static method with return value
        /// </summary>
        /// <param name="name">method name</param>
        /// <param name="args">method arguments</param>
        /// <param name="parameterTypes">method parameter types. infer types if null</param>
        public static object InvokeStatic(
            string name,
            IEnumerable<object> args,
            IEnumerable<Type> parameterTypes)
        {
            MethodInfo m = Helper.GetMethod(
                typeof(TTarget),
                name,
                Helper.StaticMethodBindingAttr,
                args,
                parameterTypes);

            if (m == null)
            {
                throw new MissingMethodException(
                    typeof(TTarget).Name, name);
            }

            try
            {
                return m.Invoke(
                    null, Helper.ArgumentsToArray(args));
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
