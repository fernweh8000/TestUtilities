TestUtilities
=============

[C#] ユニットテスト用のユーティリティ

License
=======

Copyright (c) 2015 http://fernweh.jp/  
MIT License, see LICENSE.

## InvokeUtility

プライベートメソッドを呼び出すためのユーティリティクラスです。(publicもprotectedも呼び出せます。)

静的プライベートメソッド、プライベートメソッド、プライベートコンストラクターを実行できます。

### 使い方

-	New : プライベートコンストラクターを呼び出してインスタンスを取得します。
	*	InvokeUtility&lt;TTarget&gt;.New(args)
	*	InvokeUtility&lt;TTarget&gt;.New(args, paramTypes)

-	Invoke : プライベートメソッドを呼び出します。
	-	InvokeUtility&lt;TTarget&gt;.Invoke(name, args)
	-	InvokeUtility&lt;TTarget&gt;.Invoke(name, args, paramTypes)
	-	InvokeUtility&lt;TTarget&gt;.Invoke(ctorArgs, name, args)
	-	InvokeUtility&lt;TTarget&gt;.Invoke(ctorArgs, ctorParamTypes, name, args)
	-	InvokeUtility&lt;TTarget&gt;.Invoke(ctorArgs, name, args, paramTypes)
	-	InvokeUtility&lt;TTarget&gt;.Invoke(ctorArgs, ctorParamTypes, name, args, paramTypes)
	-	InvokeUtility&lt;TTarget, TDeclaring&gt;.Invoke(name, args)
	-	InvokeUtility&lt;TTarget, TDeclaring&gt;.Invoke(name, args, paramTypes)
	-	InvokeUtility&lt;TTarget, TDeclaring&gt;.Invoke(ctorArgs, name, args)
	-	InvokeUtility&lt;TTarget, TDeclaring&gt;.Invoke(ctorArgs, ctorParamTypes, name, args)
	-	InvokeUtility&lt;TTarget, TDeclaring&gt;.Invoke(ctorArgs, name, args, paramTypes)
	-	InvokeUtility&lt;TTarget, TDeclaring&gt;.Invoke(ctorArgs, ctorParamTypes, name, args, paramTypes)

-	InvokeStatic : 静的プライベートメソッドを呼び出します。
	-	InvokeUtility&lt;TTarget&gt;.InvokeStatic(name, args)
	-	InvokeUtility&lt;TTarget&gt;.InvokeStatic(name, args, paramTypes)

詳細はテストプロジェクト(TestUtilitiesTest)を見てください。

#### 引数

-	TTarget: テスト対象のクラス
-	TDeclaring: 呼び出すメソッドを実装しているクラス
-	args: メソッドまたはコンストラクタの引数
-	paramTypes: メソッドまたはコンストラクタを特定するための引数の型
-	name: メソッド名
-	ctorArgs: コンストラクタの引数
-	ctorParamTypes: コンストラクタを特定するための引数の型

Todo
====

-	Support Property
-	Support Generics
