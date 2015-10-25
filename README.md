TestUtilities
=============

[C#] Test Utility For Unit Testing.

Lisence
=======

Copyright (c) 2015 http://fernweh.jp/  
MIT License, see LICENSE.

InvokeUtility
-------------

Utility Class for Invoking Private (?:Static Method | Method | Constructor)

### Usage

-	New : Invoke private constructor to get instance
	*	InvokeUtility&lt;TTarget&gt;.New(args)
	*	InvokeUtility&lt;TTarget&gt;.New(args, paramTypes)

-	Invoke : Invoke private method
	*	InvokeUtility&lt;TTarget&gt;.Invoke(name, args)
	*	InvokeUtility&lt;TTarget&gt;.Invoke(name, args, paramTypes)
	*	InvokeUtility&lt;TTarget&gt;.Invoke(ctorArgs, name, args)
	*	InvokeUtility&lt;TTarget&gt;.Invoke(ctorArgs, ctorParamTypes, name, args)
	*	InvokeUtility&lt;TTarget&gt;.Invoke(ctorArgs, name, args, paramTypes)
	*	InvokeUtility&lt;TTarget&gt;.Invoke(ctorArgs, ctorParamTypes, name, args, paramTypes)
	*	InvokeUtility&lt;TTarget, TDeclaring&gt;.Invoke(name, args)
	*	InvokeUtility&lt;TTarget, TDeclaring&gt;.Invoke(name, args, paramTypes)
	*	InvokeUtility&lt;TTarget, TDeclaring&gt;.Invoke(ctorArgs, name, args)
	*	InvokeUtility&lt;TTarget, TDeclaring&gt;.Invoke(ctorArgs, ctorParamTypes, name, args)
	*	InvokeUtility&lt;TTarget, TDeclaring&gt;.Invoke(ctorArgs, name, args, paramTypes)
	*	InvokeUtility&lt;TTarget, TDeclaring&gt;.Invoke(ctorArgs, ctorParamTypes, name, args, paramTypes)

-	InvokeStatic : Invoke private static method
	*	InvokeUtility&lt;TTarget&gt;.InvokeStatic(name, args)
	*	InvokeUtility&lt;TTarget&gt;.InvokeStatic(name, args, paramTypes)

Show Test Project(InvokeUtilityTest) to know detail.

#### Parameters

-	TTarget: test target class
-	TDeclaring: method declaring type
-	args: method or constructor arguments
-	paramTypes: parameter types of arguments to specify method
-	name: method name
-	ctorArgs: constructor arguments
-	ctorParamTypes: parameter types of constructor arguments to specify method

Todo
====

-	Support Property
-	Support Generics
