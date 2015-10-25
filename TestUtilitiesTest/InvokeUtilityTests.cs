#if !USING_NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClassAttribute = NUnit.Framework.TestFixtureAttribute;
using TestMethodAttribute = NUnit.Framework.TestAttribute;
using TestCategoryAttribute = NUnit.Framework.CategoryAttribute;
#endif

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Xml.Linq;
using Fernweh8000.TestUtilities;
using Fernweh8000.TestUtilitiesTest;



[TestClass]
public class InvokeUtilityUnitTest : TestClassBase
{
    // class for test
    public class FooBase
    {
        protected static DateTime ParseDate(string value)
        {
            Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject hoge;
            return DateTime.ParseExact(value, "yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }

        private DateTime BasePrivateMethod(string value)
        {
            return ParseDate(value);
        }
    }

    // class for test
    public class Foo : FooBase
    {
        // property
        public string Value
        {
            get;
            private set;
        }

        private string PrivateValue
        {
            get { return Value; }
        }

        // constructors
        private Foo()
        {
            Value = "DefaultValue";
        }

        private Foo(string arg)
        {
            Value = arg;
        }

        // methods
        private DateTime PrivateMethod(string value)
        {
            return ParseDate(value);
        }

        private static DateTime PrivateStaticMethod(string value)
        {
            return ParseDate(value);
        }
    }

    private readonly string PrivateGetterName = typeof(Foo).GetProperty(
        "PrivateValue", BindingFlags.Instance | BindingFlags.NonPublic).GetMethod.Name;


    [TestMethod]
    [TestCategory("InvokeUtility Test")]
    public void InvokeUtility_ConsturactorInvocation()
    {
        // New(args)
        Foo obj1 = InvokeUtility<Foo>.New();
        Assert.AreEqual("DefaultValue", obj1.Value);

        // New(args)
        Foo obj2 = InvokeUtility<Foo>.New("TEST");
        Assert.AreEqual("TEST", obj2.Value);

        // New(args)
        Foo obj4 = InvokeUtility<Foo>.New(new NullValue(typeof(string)));
        Assert.AreEqual(null, obj4.Value);

        // New(args, paramTypes)
        Foo obj3 = InvokeUtility<Foo>.New(new object[] { null }, TypeArray<string>());
        Assert.AreEqual(null, obj3.Value);
    }

    [TestMethod]
    [TestCategory("InvokeUtility Test")]
    public void InvokeUtility_PrivateMethodInvocation()
    {
        // Invoke(name, args)
        var actual1 = (DateTime)InvokeUtility<Foo>.Invoke(
            "PrivateMethod", "20150101");
        Assert.AreEqual(new DateTime(2015, 1, 1), actual1);

        // Invoke(name, args, paramTypes)
        var actual2 = (DateTime)InvokeUtility<Foo>.Invoke(
            "PrivateMethod", new object[] { "20150201" }, TypeArray<string>());
        Assert.AreEqual(new DateTime(2015, 2, 1), actual2);

        // Invoke(ctorArgs, name, args)
        var actual3 = (string)InvokeUtility<Foo>.Invoke(
            new object[] { "123" }, PrivateGetterName, new object[] { });
        Assert.AreEqual("123", actual3);

        // Invoke(ctorArgs, ctorParamTypes, name, args)
        var actual4 = (string)InvokeUtility<Foo>.Invoke(
            new object[] { "456" }, TypeArray<string>(), PrivateGetterName, new object[] { });
        Assert.AreEqual("456", actual4);

        // Invoke(ctorArgs, name, args, paramTypes)
        var actual5 = (string)InvokeUtility<Foo>.Invoke(
            new object[] { "789" }, PrivateGetterName, new object[] { }, EmptyTypeArray);
        Assert.AreEqual("789", actual5);

        // Invoke(ctorArgs, ctorParamTypes, name, args)
        var actual6 = (string)InvokeUtility<Foo>.Invoke(
            new object[] { "456" }, TypeArray<string>(), PrivateGetterName, new object[] { }, EmptyTypeArray);
        Assert.AreEqual("456", actual6);
    }

    [TestMethod]
    [TestCategory("InvokeUtility Test")]
    public void InvokeUtility_PrivateMethodInvocationException()
    {
        // Invoke(name, args)
        var ex1 = GetEx<FormatException>(() => InvokeUtility<Foo>.Invoke(
            "PrivateMethod", "ABC"));
        Assert.IsNotNull(ex1);

        // Invoke(name, args, paramTypes)
        var ex2 = GetEx<ArgumentNullException>(() => InvokeUtility<Foo>.Invoke(
            "PrivateMethod", new object[] { null }, TypeArray<string>()));
        Assert.IsNotNull(ex2);

        // Invoke(ctorArgs, name, args)
        var ex3 = GetEx<FormatException>(() => InvokeUtility<Foo>.Invoke(
            new object[] { "_" }, "PrivateMethod", "ABC"));
        Assert.IsNotNull(ex3);

        // Invoke(ctorArgs, ctorParamTypes, name, args)
        var ex4 = GetEx<FormatException>(() => InvokeUtility<Foo>.Invoke(
            new object[] { "_" }, TypeArray<string>(), "PrivateMethod", "ABC"));
        Assert.IsNotNull(ex4);

        // Invoke(ctorArgs, name, args, paramTypes)
        var ex5 = GetEx<FormatException>(() => InvokeUtility<Foo>.Invoke(
            new object[] { "_" }, "PrivateMethod", new object[] { "ABC" }, TypeArray<string>()));
        Assert.IsNotNull(ex5);

        // Invoke(ctorArgs, ctorParamTypes, name, args, paramTypes)
        var ex6 = GetEx<FormatException>(() => InvokeUtility<Foo>.Invoke(
            new object[] { "_" }, TypeArray<string>(), "PrivateMethod", new object[] { "ABC" }, TypeArray<string>()));
        Assert.IsNotNull(ex6);
    }


    [TestMethod]
    [TestCategory("InvokeUtility Test")]
    public void InvokeUtility_PrivateStaticInvocationException()
    {
        // Invoke(name, args)
        var ex1 = GetEx<FormatException>(() => InvokeUtility
            <Foo>.InvokeStatic("PrivateStaticMethod", "ABC"));
        Assert.IsNotNull(ex1);

        // Invoke(name, args, paramTypes)
        var ex2 = GetEx<FormatException>(() => InvokeUtility
            <Foo>.InvokeStatic("PrivateStaticMethod", new object[] { "ABC" }, TypeArray<string>()));
        Assert.IsNotNull(ex2);
    }

    [TestMethod]
    [TestCategory("InvokeUtility Test")]
    public void InvokeUtility_PrivateStaticInvoke()
    {
        var actual = (DateTime)InvokeUtility<Foo>.InvokeStatic(
            "PrivateStaticMethod", "20150102");

        Assert.AreEqual(new DateTime(2015, 1, 2), actual);
    }

    [TestMethod]
    [TestCategory("InvokeUtility Test")]
    public void InvokeUtility_BasePrivateMethodInvocationException()
    {
        var ex1 = GetEx<FormatException>(() => InvokeUtility
            <Foo, FooBase>.Invoke("BasePrivateMethod", "ABC"));

        Assert.IsNotNull(ex1);
    }

    [TestMethod]
    [TestCategory("InvokeUtility Test")]
    public void InvokeUtility_BasePrivateMethodInvocation()
    {
        var actual = (DateTime)InvokeUtility<Foo, FooBase>.Invoke(
            "BasePrivateMethod", "20150104");

        Assert.AreEqual(new DateTime(2015, 1, 4), actual);
    }

    [TestMethod]
    [TestCategory("InvokeUtility Test")]
    public void InvokeUtility_ConstructorArgumentsMustNotBeNull()
    {
        // constroctor arguments must not be null
        var ex1 = GetEx<ArgumentNullException>(() =>
            InvokeUtility<object>.New(null, null));

        Assert.AreEqual("args", ex1.ParamName);


        var ex2 = GetEx<ArgumentNullException>(() =>
            InvokeUtility<object>.Invoke(null, null, "ToString", null));
        Assert.AreEqual("constructorArguments", ex2.ParamName);
    }
}



[TestClass]
public class InvokeUtility_AmbiguousConstructorTests1 : TestClassBase
{
    private class C
    {
        private C(XNode a) { throw new Exception("1"); }
        private C(XText a) { throw new Exception("2"); }
        private C(XCData a) { throw new Exception("3"); }
        private C(int a1, int a2) { throw new Exception("4"); }
    }

    [TestMethod]
    [TestCategory("InvokeUtility Test")]
    public void InvokeUtility_AmbiguousConstructor()
    {
        Assert.AreEqual("1", GetEx<Exception>(() => InvokeUtility<C>.New(new XElement("_"))).Message);
        Assert.AreEqual("2", GetEx<Exception>(() => InvokeUtility<C>.New(new XText(""))).Message);
        Assert.AreEqual("3", GetEx<Exception>(() => InvokeUtility<C>.New(new XCData(""))).Message);
    }

    [TestMethod]
    [TestCategory("InvokeUtility Test")]
    public void InvokeUtility_AmbiguousConstructorError()
    {
        AssertThrows<MissingMethodException>(() =>
            InvokeUtility<C>.New(new object[] { null }));
    }

    [TestMethod]
    [TestCategory("InvokeUtility Test")]
    public void InvokeUtility_AmbiguousInferedConstructor()
    {
        Assert.AreEqual("1", GetEx<Exception>(() => InvokeUtility<C>.New(new object[] { null }, TypeArray<XNode>())).Message);
        Assert.AreEqual("2", GetEx<Exception>(() => InvokeUtility<C>.New(new object[] { null }, TypeArray<XText>())).Message);
        Assert.AreEqual("3", GetEx<Exception>(() => InvokeUtility<C>.New(new object[] { null }, TypeArray<XCData>())).Message);

        Assert.AreEqual("4", GetEx<Exception>(() => InvokeUtility<C>.New(new object[] { null, null })).Message);
    }
}



[TestClass]
public class InvokeUtility_AmbiguousConstructorTests2 : TestClassBase
{
    private class C
    {
        private C(object a1, XText a2) { throw new Exception("1"); }
        private C(XText a1, XText a2) { throw new Exception("2"); }
        private C(XCData a1, XText a2) { throw new Exception("3"); }
    }

    private static readonly object obj = new object();
    private static readonly XText text = new XText("");
    private static readonly XCData cdata = new XCData("");

    [TestMethod]
    [TestCategory("InvokeUtility Test")]
    public void InvokeUtility_AmbiguousConstructor()
    {
        // 1
        Assert.AreEqual("1", GetEx<Exception>(() => InvokeUtility<C>.New(new object[] { obj, text })).Message);
        Assert.AreEqual("1", GetEx<Exception>(() => InvokeUtility<C>.New(new object[] { obj, cdata })).Message);
        // 2
        Assert.AreEqual("2", GetEx<Exception>(() => InvokeUtility<C>.New(new object[] { text, text })).Message);
        Assert.AreEqual("2", GetEx<Exception>(() => InvokeUtility<C>.New(new object[] { text, cdata })).Message);
        // 3
        Assert.AreEqual("3", GetEx<Exception>(() => InvokeUtility<C>.New(new object[] { cdata, text })).Message);
        Assert.AreEqual("3", GetEx<Exception>(() => InvokeUtility<C>.New(new object[] { cdata, cdata })).Message);

        // 1
        Assert.AreEqual("1", GetEx<Exception>(() => InvokeUtility<C>.New(
            new object[] { null, null },
            TypeArray<object, XCData>())).Message);
    }

    [TestMethod]
    [TestCategory("InvokeUtility Test")]
    public void InvokeUtility_AmbiguousInferedConstructor()
    {
        Assert.AreEqual("1", GetEx<Exception>(() => InvokeUtility<C>.New(new object[] { new NullValue(typeof(object)), text })).Message);
        Assert.AreEqual("2", GetEx<Exception>(() => InvokeUtility<C>.New(new object[] { new NullValue(typeof(XText)), text })).Message);
        Assert.AreEqual("3", GetEx<Exception>(() => InvokeUtility<C>.New(new object[] { new NullValue(typeof(XCData)), text })).Message);
    }


    [TestMethod]
    [TestCategory("InvokeUtility Test")]
    public void InvokeUtility_AmbiguousConstructorError()
    {
        AssertThrows<MissingMethodException>(() =>
            InvokeUtility<C>.New(new object[] { null, null }));
    }
}



[TestClass]
public class InvokeUtility_AmbiguousInvokeTests1 : TestClassBase
{
    private class CBase
    {
        private int Test(XNode a) { return 100; }
        private int Test(string a) { return 777; }
    }

    private class C : CBase
    {
        private int Test(XNode a) { return 1; }
        private int Test(XText a) { return 2; }
        private int Test(XCData a) { return 3; }
    }

    [TestMethod]
    [TestCategory("InvokeUtility Test")]
    public void InvokeUtility_AmbiguousInvoke()
    {
        Assert.AreEqual(1, (int)InvokeUtility<C>.Invoke("Test", new XElement("_")));
        Assert.AreEqual(2, (int)InvokeUtility<C>.Invoke("Test", new XText("")));
        Assert.AreEqual(3, (int)InvokeUtility<C>.Invoke("Test", new XCData("")));

        // Base Class
        Assert.AreEqual(100, (int)InvokeUtility<C, CBase>.Invoke("Test", new XElement("_")));
        Assert.AreEqual(777, (int)InvokeUtility<C, CBase>.Invoke("Test", string.Empty));
    }

    [TestMethod]
    [TestCategory("InvokeUtility Test")]
    public void InvokeUtility_AmbiguousInvokeError()
    {
        AssertThrows<MissingMethodException>(() =>
            InvokeUtility<C>.Invoke("Test", new object[] { null }));
    }
}



[TestClass]
public class InvokeUtility_AmbiguousInvokeTests2 : TestClassBase
{
    private class C
    {
        private int Test(object a1, XText a2) { return 1; }
        private int Test(XText a1, XText a2) { return 2; }
        private int Test(XCData a1, XText a2) { return 3; }
    }

    private static readonly object obj = new object();
    private static readonly XText text = new XText("");
    private static readonly XCData cdata = new XCData("");

    [TestMethod]
    [TestCategory("InvokeUtility Test")]
    public void InvokeUtility_AmbiguousInvoke()
    {
        // 1
        Assert.AreEqual(1, (int)InvokeUtility<C>.Invoke("Test", new object[] { obj, text }));
        Assert.AreEqual(1, (int)InvokeUtility<C>.Invoke("Test", new object[] { obj, cdata }));
        // 2
        Assert.AreEqual(2, (int)InvokeUtility<C>.Invoke("Test", new object[] { text, text }));
        Assert.AreEqual(2, (int)InvokeUtility<C>.Invoke("Test", new object[] { text, cdata }));
        // 3
        Assert.AreEqual(3, (int)InvokeUtility<C>.Invoke("Test", new object[] { cdata, text }));
        Assert.AreEqual(3, (int)InvokeUtility<C>.Invoke("Test", new object[] { cdata, cdata }));

        // 1
        Assert.AreEqual(1, (int)InvokeUtility<C>.Invoke("Test",
            new object[] { null, null },
            TypeArray<object, XCData>()));
    }

    [TestMethod]
    [TestCategory("InvokeUtility Test")]
    public void InvokeUtility_AmbiguousInferedInvoke()
    {
        Assert.AreEqual(1, (int)InvokeUtility<C>.Invoke("Test", new object[] { new NullValue(typeof(object)), text }));
        Assert.AreEqual(2, (int)InvokeUtility<C>.Invoke("Test", new object[] { new NullValue(typeof(XText)), text }));
        Assert.AreEqual(3, (int)InvokeUtility<C>.Invoke("Test", new object[] { new NullValue(typeof(XCData)), text }));
    }


    [TestMethod]
    [TestCategory("InvokeUtility Test")]
    public void InvokeUtility_AmbiguousInvokeError()
    {
        AssertThrows<MissingMethodException>(() =>
            InvokeUtility<C>.Invoke("Test", new object[] { null, null }));
    }
}




[TestClass]
public class InvokeUtility_AmbiguousStaticInvokeTests1 : TestClassBase
{
    private class C
    {
        private static int Test(XNode a) { return 1; }
        private static int Test(XText a) { return 2; }
        private static int Test(XCData a) { return 3; }
    }

    [TestMethod]
    [TestCategory("InvokeUtility Test")]
    public void InvokeUtility_AmbiguousStaticInvoke()
    {
        Assert.AreEqual(1, (int)InvokeUtility<C>.InvokeStatic("Test", new XElement("_")));
        Assert.AreEqual(2, (int)InvokeUtility<C>.InvokeStatic("Test", new XText("")));
        Assert.AreEqual(3, (int)InvokeUtility<C>.InvokeStatic("Test", new XCData("")));
    }

    [TestMethod]
    [TestCategory("InvokeUtility Test")]
    public void InvokeUtility_AmbiguousStaticInvokeError()
    {
        AssertThrows<MissingMethodException>(() =>
            InvokeUtility<C>.InvokeStatic("Test", new object[] { null }));

        AssertThrows<MissingMethodException>(() =>
            InvokeUtility<C>.InvokeStatic("Test", new object()));
    }
}




[TestClass]
public class InvokeUtility_AmbiguousStaticInvokeTests2 : TestClassBase
{
    private class C
    {
        private static int Test(object a1, XText a2) { return 1; }
        private static int Test(XText a1, XText a2) { return 2; }
        private static int Test(XCData a1, XText a2) { return 3; }
    }

    private static readonly object obj = new object();
    private static readonly XText text = new XText("");
    private static readonly XCData cdata = new XCData("");

    [TestMethod]
    [TestCategory("InvokeUtility Test")]
    public void InvokeUtility_AmbiguousStaticInvoke()
    {
        //1 aa
        Assert.AreEqual(1, (int)InvokeUtility<C>.InvokeStatic("Test", new object[] { obj, text }));
        Assert.AreEqual(1, (int)InvokeUtility<C>.InvokeStatic("Test", new object[] { obj, cdata }));
        // 2
        Assert.AreEqual(2, (int)InvokeUtility<C>.InvokeStatic("Test", new object[] { text, text }));
        Assert.AreEqual(2, (int)InvokeUtility<C>.InvokeStatic("Test", new object[] { text, cdata }));
        // 3
        Assert.AreEqual(3, (int)InvokeUtility<C>.InvokeStatic("Test", new object[] { cdata, text }));
        Assert.AreEqual(3, (int)InvokeUtility<C>.InvokeStatic("Test", new object[] { cdata, cdata }));

        // 1
        Assert.AreEqual(1, (int)InvokeUtility<C>.InvokeStatic("Test",
            new object[] { null, null },
            TypeArray<object, XCData>()));
    }

    [TestMethod]
    [TestCategory("InvokeUtility Test")]
    public void InvokeUtility_AmbiguousStaticInferedInvoke()
    {
        Assert.AreEqual(1, (int)InvokeUtility<C>.InvokeStatic("Test", new object[] { new NullValue(typeof(object)), text }));
        Assert.AreEqual(2, (int)InvokeUtility<C>.InvokeStatic("Test", new object[] { new NullValue(typeof(XText)), text }));
        Assert.AreEqual(3, (int)InvokeUtility<C>.InvokeStatic("Test", new object[] { new NullValue(typeof(XCData)), text }));
    }


    [TestMethod]
    [TestCategory("InvokeUtility Test")]
    public void InvokeUtility_AmbiguousStaticInvokeError()
    {
        AssertThrows<MissingMethodException>(() =>
            InvokeUtility<C>.InvokeStatic("Test", new object[] { null, null }));

        AssertThrows<MissingMethodException>(() =>
            InvokeUtility<C>.InvokeStatic("Test", new object[] { obj, obj }));
    }
}


