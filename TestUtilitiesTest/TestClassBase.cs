#if !USING_NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClassAttribute = NUnit.Framework.TestFixtureAttribute;
using TestMethodAttribute = NUnit.Framework.TestAttribute;
using ClassInitializeAttribute = NUnit.Framework.TestFixtureSetUpAttribute;
using ClassCleanupAttribute = NUnit.Framework.TestFixtureTearDownAttribute;
using TestInitializeAttribute = NUnit.Framework.SetUpAttribute;
using TestCleanupAttribute = NUnit.Framework.TearDownAttribute;
using TestCategoryAttribute = NUnit.Framework.CategoryAttribute;
#endif

using System;


namespace Fernweh8000.TestUtilitiesTest
{
    public class TestClassBase
    {
        // shorthands to get Type[]
        public static Type[] EmptyTypeArray { get { return new Type[] { }; } }
        public static Type[] TypeArray<T1>() { return new Type[] { typeof(T1) }; }
        public static Type[] TypeArray<T1, T2>() { return new Type[] { typeof(T1), typeof(T2) }; }
        public static Type[] TypeArray<T1, T2, T3>() { return new Type[] { typeof(T1), typeof(T2), typeof(T3) }; }
        public static Type[] TypeArray<T1, T2, T3, T4>() { return new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }; }
        public static Type[] TypeArray<T1, T2, T3, T4, T5>() { return new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) }; }

        /// <summary>get exception thrown in action</summary>
        /// <typeparam name="E">exception type</typeparam>
        public static E GetException<E>(Action action) where E : Exception
        {
            try
            {
                action();
                return null;
            }
            catch (E ex)
            {
                return ex;
            }
        }

        /// <summary>get exception thrown in action</summary>
        /// <typeparam name="E">exception type</typeparam>
        public static E GetEx<E>(Action action) where E : Exception
        {
            return GetException<E>(action);
        }

        public static void AssertThrows<E>(Action action) where E : Exception
        {
            try
            {
                action();
                Assert.Fail();
            }
            catch (E)
            {
                // OK
            }
        }
    }
}
