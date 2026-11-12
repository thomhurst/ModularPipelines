using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class OperationExceptionClassifierTests
{
    private static readonly string[] NullReceiverExceptions = ["System.NullReferenceException"];
    private static readonly string[] StaticInitializationExceptions = ["System.TypeInitializationException"];

    [TestMethod]
    [DataRow("static unsafe void M(delegate* managed<void> action) => action();")]
    [DataRow("static unsafe void M() { delegate* managed<void> action = &Throw; action(); }")]
    public void FunctionPointerInvocationsRetainUnknownExceptions(string method)
    {
        var (root, classifier) = Compile($"class C {{ static void Throw() => throw new System.Exception(); {method} }}", allowUnsafe: true);
        var invocation = root.DescendantsAndSelf().OfType<IFunctionPointerInvocationOperation>().Single();
        Assert.AreSequenceEqual([null], classifier.Classify(invocation).ToArray());
    }

    [TestMethod]
    [DataRow("class V { public static implicit operator int(V value) => throw new System.Exception(); }", "V", "int", "0", true)]
    [DataRow("struct V { public static implicit operator int(V value) => throw new System.Exception(); }", "V?", "int", "0", true)]
    [DataRow("", "int?", "long", "0L", false)]
    [DataRow("", "string", "object", "new object()", false)]
    [DataRow("struct V { public static implicit operator int(V value) => throw new System.Exception(); }", "(V, V)?", "(int, int)", "(0, 0)", true)]
    [DataRow("struct V { public static implicit operator int(V value) => throw new System.Exception(); }", "((V, V), V)?", "((int, int), int)", "((0, 0), 0)", true)]
    [DataRow("", "(int, int)?", "(long, long)", "(0L, 0L)", false)]
    [DataRow("", "(int, int)?", "(int, int)", "(0, 0)", false)]
    [DataRow("struct V { public static implicit operator int(V value) => throw new System.Exception(); }", "(V?, int)?", "(int?, long)", "((int?)null, 0L)", true)]
    [DataRow("", "(string, int)?", "(object, long)", "(new object(), 0L)", false)]
    [DataRow("struct V { public static implicit operator int(V value) => throw new System.Exception(); }", "(V, V)?", "(V, V)", "(new V(), new V())", false)]
    public void CoalesceInspectsHiddenValueConversion(string declaration, string from, string to, string fallback, bool throws)
    {
        var (root, classifier) = Compile($"{declaration} class C {{ {to} M({from} x) => x ?? {fallback}; }}");
        var coalesce = root.DescendantsAndSelf().OfType<ICoalesceOperation>().Single();
        Assert.AreSequenceEqual(throws ? [null] : [], classifier.Classify(coalesce).ToArray());
    }

    [TestMethod]
    [DataRow("decimal", "int", true)]
    [DataRow("decimal", "byte", true)]
    [DataRow("decimal", "sbyte", true)]
    [DataRow("decimal", "short", true)]
    [DataRow("decimal", "ushort", true)]
    [DataRow("decimal", "uint", true)]
    [DataRow("decimal", "char", true)]
    [DataRow("decimal", "long", true)]
    [DataRow("decimal", "ulong", true)]
    [DataRow("decimal", "nint", true)]
    [DataRow("decimal", "nuint", true)]
    [DataRow("decimal", "E", true)]
    [DataRow("decimal?", "int?", true)]
    [DataRow("double", "decimal", true)]
    [DataRow("float", "decimal?", true)]
    [DataRow("double?", "decimal?", true)]
    [DataRow("ulong", "decimal", false)]
    [DataRow("int", "decimal", false)]
    [DataRow("long", "decimal", false)]
    [DataRow("decimal", "double", false)]
    [DataRow("decimal", "float", false)]
    [DataRow("decimal?", "double?", false)]
    [DataRow("decimal?", "float?", false)]
    [DataRow("int?", "long?", false)]
    [DataRow("E?", "long?", false)]
    [DataRow("E", "long", false)]
    [DataRow("int", "E", false)]
    [DataRow("E", "Other", false)]
    [DataRow("decimal", "E?", true)]
    [DataRow("E?", "long", true)]
    [DataRow("decimal?", "double", true)]
    [DataRow("int?", "long", true)]
    [DataRow("object", "int?", true)]
    [DataRow("double", "int", false)]
    public void UncheckedNumericConversions(string from, string to, bool throws)
    {
        var (root, classifier) = Compile($"enum E {{ A }} enum Other {{ A }} class C {{ {to} M({from} x) => unchecked(({to})x); }}");
        var conversion = root.DescendantsAndSelf().OfType<IConversionOperation>()
            .Single(operation => operation.Syntax is CastExpressionSyntax);
        Assert.AreEqual(throws, !classifier.Classify(conversion).IsEmpty);
    }

    [TestMethod]
    [DataRow("int", "x / y", "System.DivideByZeroException,System.OverflowException")]
    [DataRow("uint", "x / y", "System.DivideByZeroException")]
    [DataRow("int", "x % y", "System.DivideByZeroException,System.OverflowException")]
    [DataRow("double", "x / y", "")]
    [DataRow("decimal", "x * y", "System.OverflowException")]
    [DataRow("int", "checked(x + y)", "System.OverflowException")]
    [DataRow("int", "unchecked(x + y)", "")]
    [DataRow("dynamic", "x + y", "?")]
    public void ArithmeticExceptionTypes(string type, string expression, string expected)
    {
        var (root, classifier) = Compile($"class C {{ object M({type} x, {type} y) => {expression}; }}");
        var operation = root.DescendantsAndSelf().OfType<IBinaryOperation>().Single();
        Assert.AreEqual(expected, string.Join(",", classifier.Classify(operation).Select(name => name ?? "?")));
    }

    [TestMethod]
    [DataRow("decimal", "int", "+=", false, "System.OverflowException")]
    [DataRow("decimal?", "int?", "+=", false, "System.OverflowException")]
    [DataRow("decimal", "int", "%=", false, "System.DivideByZeroException")]
    [DataRow("byte", "byte", "+=", false, "")]
    [DataRow("byte", "byte", "+=", true, "System.OverflowException")]
    [DataRow("sbyte", "sbyte", "/=", false, "System.DivideByZeroException")]
    [DataRow("sbyte", "sbyte", "/=", true, "System.DivideByZeroException,System.OverflowException")]
    [DataRow("int", "int", "/=", false, "System.DivideByZeroException,System.OverflowException")]
    [DataRow("double", "int", "/=", false, "")]
    [DataRow("float", "float", "*=", true, "")]
    public void ValidCompoundNumericPromotionsPreserveExceptions(string targetType, string valueType,
        string operatorToken, bool checkOverflow, string expected)
    {
        var (root, classifier) = Compile(
            $"class C {{ void M(ref {targetType} x, {valueType} y) {{ x {operatorToken} y; }} }}",
            checkOverflow: checkOverflow);
        var assignment = root.DescendantsAndSelf().OfType<ICompoundAssignmentOperation>().Single();
        Assert.AreEqual(expected, string.Join(",", classifier.Classify(assignment).Select(name => name ?? "?")));
    }

    [TestMethod]
    [DataRow("string", LanguageVersion.CSharp10, false, true, false)]
    [DataRow("object", LanguageVersion.CSharp10, false, true, false)]
    [DataRow("System.IComparable", LanguageVersion.CSharp10, false, true, false)]
    [DataRow("string", LanguageVersion.CSharp9, false, false, true)]
    [DataRow("object", LanguageVersion.CSharp9, false, false, true)]
    [DataRow("string", LanguageVersion.CSharp10, true, false, true)]
    [DataRow("System.FormattableString", LanguageVersion.CSharp10, false, false, false)]
    [DataRow("System.IFormattable", LanguageVersion.CSharp10, false, false, false)]
    public void InterpolationFormattingPoints(string destination, LanguageVersion version, bool dynamicHole,
        bool perHole, bool completed)
    {
        var (root, classifier) = Compile($$"""
            class C { {{destination}} M({{(dynamicHole ? "dynamic" : "object")}} x) => $"{x}{x}"; }
            """, version);
        var text = root.DescendantsAndSelf().OfType<IInterpolatedStringOperation>().Single();
        Assert.AreEqual(completed, !classifier.Classify(text).IsEmpty);
        foreach (var hole in text.Parts.OfType<IInterpolationOperation>())
        {
            Assert.AreEqual(perHole, !classifier.Classify(hole).IsEmpty);
        }
    }

    [TestMethod]
    [DataRow("$\"{x}\"", false)]
    [DataRow("$\"{x,10}\"", true)]
    [DataRow("$\"{(object)x}\"", true)]
    [DataRow("$\"{(object)null}\"", false)]
    public void FormattingCanInvokeUserCode(string expression, bool throws)
    {
        var (root, classifier) = Compile($"class C {{ string M(string x) => {expression}; }}");
        var hole = root.DescendantsAndSelf().OfType<IInterpolationOperation>().Single();
        Assert.AreEqual(throws, !classifier.Classify(hole).IsEmpty);
    }

    [TestMethod]
    [DataRow("() => d.P")]
    [DataRow("async () => await task")]
    public void NestedLambdaDoesNotDeferOuterInterpolationFormatting(string lambda)
    {
        var (root, classifier) = Compile($$"""
            class C {
                object Make(System.Func<object> action) => null;
                object Make(System.Func<System.Threading.Tasks.Task<object>> action) => null;
                object Later() => null;
                string M(dynamic d, System.Threading.Tasks.Task<object> task) => $"{Make({{lambda}})}{Later()}";
            }
            """);
        var text = root.DescendantsAndSelf().OfType<IInterpolatedStringOperation>().Single();
        Assert.IsTrue(classifier.Classify(text).IsEmpty);
        Assert.IsTrue(text.Parts.OfType<IInterpolationOperation>().All(hole => !classifier.Classify(hole).IsEmpty));
    }

    [TestMethod]
    [DataRow("??= value", true)]
    [DataRow("??= null", false)]
    [DataRow("+= (dynamic)value", true)]
    public void ConditionalAndCompoundArrayStoresClassifyMismatchAfterValue(string store, bool mismatch)
    {
        var (root, classifier) = Compile($"class C {{ void M(object[] a, object value) {{ a[0] {store}; }} }}");
        var assignment = root.DescendantsAndSelf().OfType<IAssignmentOperation>().Single();
        Assert.DoesNotContain("System.ArrayTypeMismatchException", classifier.Classify(assignment.Target));
        Assert.Contains("System.NullReferenceException", classifier.Classify(assignment.Target));
        Assert.Contains("System.IndexOutOfRangeException", classifier.Classify(assignment.Target));
        Assert.AreEqual(mismatch, classifier.Classify(assignment).Contains("System.ArrayTypeMismatchException"));
        Assert.DoesNotContain("System.ArrayTypeMismatchException",
            new OperationExceptionClassifier(hasExactArrayType: _ => true).Classify(assignment));
    }

    [TestMethod]
    [DataRow("object", "a[0] += (dynamic)value;", "?")]
    [DataRow("int", "checked { a[0] += value; }", "System.OverflowException")]
    [DataRow("int", "a[0] /= value;", "System.DivideByZeroException")]
    public void CompoundArrayStoresPreserveOperatorExceptions(string type, string statement, string expected)
    {
        var (root, classifier) = Compile($"class C {{ void M({type}[] a, {type} value) {{ {statement} }} }}");
        var assignment = root.DescendantsAndSelf().OfType<ICompoundAssignmentOperation>().Single();
        Assert.Contains(expected, classifier.Classify(assignment).Select(name => name ?? "?"));
    }

    [TestMethod]
    [DataRow("value")]
    [DataRow("null")]
    public void RefConditionalAssignmentChecksCovarianceBeforeStoredValue(string value)
    {
        var (root, classifier) = Compile($"class C {{ void M(bool flag, object[] a, object[] b, object value) {{ (flag ? ref a[0] : ref b[0]) = {value}; }} }}");
        foreach (var array in root.DescendantsAndSelf().OfType<IArrayElementReferenceOperation>())
        {
            Assert.Contains("System.ArrayTypeMismatchException", classifier.Classify(array));
            Assert.DoesNotContain("System.ArrayTypeMismatchException",
                new OperationExceptionClassifier(hasExactArrayType: _ => true).Classify(array));
        }
    }

    [TestMethod]
    [DataRow("x + y", true)]
    [DataRow("x + (string)y", false)]
    [DataRow("x + 1", true)]
    public void ConcatenationFormatting(string expression, bool throws)
    {
        var (root, classifier) = Compile($"class C {{ string M(string x, object y) => {expression}; }}");
        var binary = root.DescendantsAndSelf().OfType<IBinaryOperation>().Single();
        Assert.AreEqual(throws, !classifier.Classify(binary).IsEmpty);
    }

    [TestMethod]
    public void ReceiverAndExactArrayFactsAreExplicit()
    {
        var (root, classifier) = Compile("class C { public int F; void M(C c, object[] a, object value) { c.F = 1; a[0] = value; } }");
        var assignments = root.DescendantsAndSelf().OfType<ISimpleAssignmentOperation>().ToArray();
        var field = assignments[0];
        Assert.IsTrue(classifier.Classify(field.Target).IsEmpty);
        Assert.AreSequenceEqual(NullReceiverExceptions, classifier.Classify(field).ToArray());
        Assert.IsTrue(new OperationExceptionClassifier(isKnownNonNull: _ => true).Classify(field).IsEmpty);
        var array = assignments[1];
        Assert.IsTrue(classifier.Classify(array.Target).IsEmpty);
        Assert.Contains("System.ArrayTypeMismatchException", classifier.Classify(array));
        Assert.DoesNotContain("System.ArrayTypeMismatchException", new OperationExceptionClassifier(hasExactArrayType: _ => true).Classify(array));
    }

    [TestMethod]
    [DataRow("c.E", false, false)]
    [DataRow("c.E = value", true, false)]
    [DataRow("E", false, true)]
    [DataRow("E = value", true, true)]
    public void FieldLikeEventAccessPreservesReceiverTiming(string expression, bool store, bool isStatic)
    {
        var (root, classifier) = Compile($$"""
            class C {
                {{(isStatic ? "static " : "")}}event System.Action E;
                System.Action M(C c, System.Action value) => {{expression}};
            }
            """);
        var access = root.DescendantsAndSelf().OfType<IEventReferenceOperation>().Single();
        var operation = store ? access.Parent! : access;
        if (store)
        {
            Assert.IsTrue(classifier.Classify(access).IsEmpty);
        }

        Assert.AreSequenceEqual(isStatic ? StaticInitializationExceptions : NullReceiverExceptions,
            classifier.Classify(operation).ToArray());
        if (!isStatic)
        {
            Assert.IsTrue(new OperationExceptionClassifier(isKnownNonNull: _ => true).Classify(operation).IsEmpty);
        }
    }

    [TestMethod]
    [DataRow("checked((E)x)", true)]
    [DataRow("checked(unchecked((E)x))", false)]
    public void EnumerationConversionsRespectCheckedContext(string expression, bool throws)
    {
        var (root, classifier) = Compile($"enum E : byte {{ A }} class C {{ E M(int x) => {expression}; }}");
        var conversion = root.DescendantsAndSelf().OfType<IConversionOperation>().Single();
        Assert.AreEqual(throws, !classifier.Classify(conversion).IsEmpty);
    }

    [TestMethod]
    public void SetterAndStaticInitializationFollowAssignedValue()
    {
        var (root, classifier) = Compile("class C { static int F; int P { get; set; } void M() { P = 1; F = 2; } }");
        var assignments = root.DescendantsAndSelf().OfType<ISimpleAssignmentOperation>().ToArray();
        Assert.IsTrue(classifier.Classify(assignments[0].Target).IsEmpty);
        Assert.AreSequenceEqual([null], classifier.Classify(assignments[0]).ToArray());
        Assert.IsTrue(classifier.Classify(assignments[1].Target).IsEmpty);
        Assert.AreSequenceEqual(StaticInitializationExceptions, classifier.Classify(assignments[1]).ToArray());
    }

    [TestMethod]
    [DataRow("(int a, int b) = x;", true)]
    [DataRow("(int a, int b) = (1, 2);", false)]
    [DataRow("(int a, int b) = (new V(), new V());", true)]
    public void ImplicitDeconstructionAndElementConversions(string statement, bool throws)
    {
        var (root, classifier) = Compile($$"""
            class V { public static implicit operator int(V v) => 0; }
            class C { public void Deconstruct(out int a, out int b) { a = b = 0; }
                void M(C x) { {{statement}} } }
            """);
        var operation = root.DescendantsAndSelf().OfType<IDeconstructionAssignmentOperation>().Single();
        Assert.AreEqual(throws, operation.DescendantsAndSelf().Any(part => !classifier.Classify(part).IsEmpty));
    }

    [TestMethod]
    [DataRow("x is { P: 1 }", true)]
    [DataRow("x is (1, 2)", true)]
    [DataRow("a is [1, 2]", false)]
    [DataRow("x is [1, 2]", true)]
    public void ImplicitPatternCalls(string expression, bool throws)
    {
        var (root, classifier) = Compile($$"""
            class C {
                public int P => 0; public int Length => 2; public int this[int i] => 0;
                public void Deconstruct(out int a, out int b) { a = b = 0; }
                bool M(C x, int[] a) => {{expression}};
            }
            """);
        var pattern = root.DescendantsAndSelf().OfType<IIsPatternOperation>().Single();
        Assert.AreEqual(throws, pattern.DescendantsAndSelf().Any(operation => !classifier.Classify(operation).IsEmpty));
    }

    [TestMethod]
    [DataRow("System.Collections.Generic.List<int>", "[1, flag ? 2 : 3]", true, false)]
    [DataRow("int[]", "[1, flag ? 2 : 3]", false, false)]
    [DataRow("int[]", "[1, .. values, flag ? 2 : 3]", true, true)]
    [DataRow("Built", "[1, flag ? 2 : 3]", false, false)]
    public void CollectionCallsDescribeEarlyRangesAndCompletion(string destination, string expression,
        bool early, bool followsSpread)
    {
        var (root, classifier) = Compile($$"""
            using System;
            using System.Collections;
            using System.Collections.Generic;
            using System.Runtime.CompilerServices;
            [CollectionBuilder(typeof(Builder), nameof(Builder.Create))]
            class Built : IEnumerable<int> {
                public IEnumerator<int> GetEnumerator() => throw new Exception();
                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            }
            static class Builder { public static Built Create(ReadOnlySpan<int> items) => new Built(); }
            class C { {{destination}} M(bool flag, int[] values) => {{expression}}; }
            """);
        var collection = root.DescendantsAndSelf().OfType<ICollectionExpressionOperation>().Single();
        var spans = OperationExceptionClassifier.GetEarlyCollectionCallSpans(collection);
        Assert.AreEqual(early, !spans.IsEmpty);
        Assert.AreEqual(collection.ConstructMethod is not null || collection.Elements.Any(e => e is ISpreadOperation),
            !classifier.Classify(collection).IsEmpty);
        if (early)
        {
            var expectedElements = followsSpread
                ? collection.Elements.SkipWhile(static element => element is not ISpreadOperation).Skip(1)
                : collection.Elements;
            Assert.AreSequenceEqual(expectedElements.Select(static element => element.Syntax.Span), spans);
            var conditional = collection.DescendantsAndSelf().OfType<IConditionalOperation>().Single();
            Assert.IsTrue(spans.Any(span => span.Contains(conditional.Syntax.Span)));
        }
    }

    [TestMethod]
    public async Task MissingRuntimeHandlerUsesCompletionFormatting()
    {
        var references = await ReferenceAssemblies.NetStandard.NetStandard20.ResolveAsync(LanguageNames.CSharp, CancellationToken.None);
        var (root, classifier) = Compile("class C { string M(object x) => $\"{x}{x}\"; }", references: references);
        var text = root.DescendantsAndSelf().OfType<IInterpolatedStringOperation>().Single();
        Assert.IsFalse(classifier.Classify(text).IsEmpty);
        Assert.IsTrue(text.Parts.OfType<IInterpolationOperation>().All(hole => classifier.Classify(hole).IsEmpty));
    }

    [TestMethod]
    [DataRow("System.Linq.Expressions.Expression<System.Func<object, string>>", "x => $\"{x}{x}\"")]
    [DataRow("System.Func<System.Threading.Tasks.Task<object>, System.Threading.Tasks.Task<string>>", "async x => $\"{await x}{await x}\"")]
    public void ExpressionTreesAndAwaitUseCompletionFormatting(string type, string expression)
    {
        var (root, classifier) = Compile($"class C {{ {type} M() => {expression}; }}");
        var text = root.DescendantsAndSelf().OfType<IInterpolatedStringOperation>().Single();
        Assert.IsFalse(classifier.Classify(text).IsEmpty);
        Assert.IsTrue(text.Parts.OfType<IInterpolationOperation>().All(hole => classifier.Classify(hole).IsEmpty));
    }

    [TestMethod]
    [DataRow("a[0] = null;", false)]
    [DataRow("a[0] = value;", true)]
    [DataRow("Change(ref a[0]);", true)]
    [DataRow("ref object alias = ref a[0];", true)]
    [DataRow("ref object alias = ref (a.Length > 0 ? ref a[0] : ref a[1]);", true)]
    [DataRow("ref readonly object alias = ref (a.Length > 0 ? ref a[0] : ref a[1]);", false)]
    [DataRow("var item = a[0];", false)]
    public void ArrayMismatchRequiresWritableAccess(string statement, bool mismatch)
    {
        var (root, classifier) = Compile($"class C {{ void Change(ref object x) {{ }} void M(object[] a, object value) {{ {statement} }} }}");
        var exceptions = root.DescendantsAndSelf().SelectMany(operation => classifier.Classify(operation)).ToArray();
        Assert.IsTrue(exceptions.Contains("System.NullReferenceException"));
        Assert.IsTrue(exceptions.Contains("System.IndexOutOfRangeException"));
        Assert.AreEqual(mismatch, exceptions.Contains("System.ArrayTypeMismatchException"));
    }

    [TestMethod]
    [DataRow("ref object", "=> ref a[0];", true)]
    [DataRow("ref object", "{ return ref a[0]; }", true)]
    [DataRow("ref readonly object", "=> ref a[0];", false)]
    [DataRow("ref readonly object", "{ return ref a[0]; }", false)]
    [DataRow("object", "=> a[0];", false)]
    public void ArrayRefReturnsRequireWritableAccess(string returnType, string body, bool mismatch)
    {
        var (root, classifier) = Compile($"class C {{ {returnType} M(object[] a) {body} }}");
        var array = root.DescendantsAndSelf().OfType<IArrayElementReferenceOperation>().Single();
        Assert.AreEqual(mismatch, classifier.Classify(array).Contains("System.ArrayTypeMismatchException"));
        Assert.AreEqual(mismatch, new OperationExceptionClassifier().Classify(array).Contains("System.ArrayTypeMismatchException"));
        Assert.DoesNotContain("System.ArrayTypeMismatchException",
            new OperationExceptionClassifier(hasExactArrayType: _ => true).Classify(array));
    }

    [TestMethod]
    [DataRow("ref", true)]
    [DataRow("ref readonly", false)]
    public void RefConditionalArrayReturnsPreserveWritableContext(string modifier, bool mismatch)
    {
        var (root, classifier) = Compile($$"""
            class C {
                {{modifier}} object M(bool flag, object[] a, object[] b)
                    => ref (flag ? ref a[0] : ref b[0]);
            }
            """);
        foreach (var array in root.DescendantsAndSelf().OfType<IArrayElementReferenceOperation>())
        {
            Assert.AreEqual(mismatch, classifier.Classify(array).Contains("System.ArrayTypeMismatchException"));
            Assert.DoesNotContain("System.ArrayTypeMismatchException",
                new OperationExceptionClassifier(hasExactArrayType: _ => true).Classify(array));
        }
    }

    [TestMethod]
    [DataRow("??= value")]
    [DataRow("+= (dynamic)value")]
    public void RefConditionalArrayAssignmentTargetsRequireWritableReferences(string store)
    {
        var (root, classifier) = Compile($$"""
            class C {
                void M(bool flag, object[] a, object[] b, object value) {
                    (flag ? ref a[0] : ref b[0]) {{store}};
                }
            }
            """);
        foreach (var array in root.DescendantsAndSelf().OfType<IArrayElementReferenceOperation>())
        {
            Assert.Contains("System.ArrayTypeMismatchException", classifier.Classify(array));
            Assert.DoesNotContain("System.ArrayTypeMismatchException",
                new OperationExceptionClassifier(hasExactArrayType: _ => true).Classify(array));
        }
    }

    [TestMethod]
    [DataRow("++")]
    [DataRow("--")]
    public void RefConditionalArrayIncrementTargetsRequireWritableReferences(string operatorToken)
    {
        var (root, classifier) = Compile($$"""
            class V {
                public static V operator ++(V value) => value;
                public static V operator --(V value) => value;
            }
            class C { void M(bool flag, V[] a, V[] b) { (flag ? ref a[0] : ref b[0]){{operatorToken}}; } }
            """);
        foreach (var array in root.DescendantsAndSelf().OfType<IArrayElementReferenceOperation>())
        {
            Assert.Contains("System.ArrayTypeMismatchException", classifier.Classify(array));
        }
    }

    [TestMethod]
    [DataRow("ref", "a[0]", true)]
    [DataRow("ref", "flag ? ref a[0] : ref a[1]", true)]
    [DataRow("out", "a[0]", true)]
    [DataRow("in", "a[0]", false)]
    [DataRow("ref readonly", "a[0]", false)]
    public void RefParameterReassignmentPreservesWritableContext(string modifier, string value, bool mismatch)
    {
        var (root, classifier) = Compile($$"""
            class C {
                void M({{modifier}} object alias, bool flag, object[] a) {
                    {{(modifier == "out" ? "alias = null;" : "")}}
                    alias = ref ({{value}});
                }
            }
            """);
        foreach (var array in root.DescendantsAndSelf().OfType<IArrayElementReferenceOperation>())
        {
            Assert.AreEqual(mismatch, classifier.Classify(array).Contains("System.ArrayTypeMismatchException"));
            Assert.DoesNotContain("System.ArrayTypeMismatchException",
                new OperationExceptionClassifier(hasExactArrayType: _ => true).Classify(array));
        }
    }

    [TestMethod]
    [DataRow("ref", "a[0]", true)]
    [DataRow("ref", "flag ? ref a[0] : ref a[1]", true)]
    [DataRow("ref readonly", "a[0]", false)]
    [DataRow("ref readonly", "flag ? ref a[0] : ref a[1]", false)]
    public void RefFieldReassignmentPreservesWritableContext(string modifier, string value, bool mismatch)
    {
        var (root, classifier) = Compile($$"""
            ref struct C {
                public {{modifier}} object Alias;
                void M(bool flag, object[] a) { Alias = ref ({{value}}); }
            }
            """);
        foreach (var array in root.DescendantsAndSelf().OfType<IArrayElementReferenceOperation>())
        {
            Assert.AreEqual(mismatch, classifier.Classify(array).Contains("System.ArrayTypeMismatchException"));
            Assert.DoesNotContain("System.ArrayTypeMismatchException",
                new OperationExceptionClassifier(hasExactArrayType: _ => true).Classify(array));
        }
    }

    [TestMethod]
    [DataRow("a[0] += value;")]
    [DataRow("a[0]++;")]
    public void OverloadedArrayStoresPreserveMismatchAndOperatorExceptions(string statement)
    {
        var (root, classifier) = Compile($$"""
            class V {
                public static V operator +(V left, V right) => left;
                public static V operator ++(V value) => value;
            }
            class C { void M(V[] a, V value) { {{statement}} } }
            """);
        var store = root.DescendantsAndSelf().Single(operation => operation is
            ICompoundAssignmentOperation or IIncrementOrDecrementOperation);
        Assert.Contains("System.ArrayTypeMismatchException", classifier.Classify(store));
        Assert.Contains((string?) null, classifier.Classify(store));
    }

    [TestMethod]
    [DataRow("return (int)x;", true, true)]
    [DataRow("checked { return (int)x; }", false, true)]
    [DataRow("unchecked { return (int)x; }", true, false)]
    [DataRow("checked { return unchecked((int)x); }", false, false)]
    [DataRow("checked { System.Func<E, int> convert = value => (int)value; return convert(x); }", false, false)]
    [DataRow("checked { int Convert(E value) => (int)value; return Convert(x); }", false, false)]
    public void EnumConversionsUseCompilationAndLocalOverflowContext(string body, bool checkOverflow, bool throws)
    {
        var (root, classifier) = Compile($"enum E : long {{ A }} class C {{ int M(E x) {{ {body} }} }}",
            checkOverflow: checkOverflow);
        var conversion = root.DescendantsAndSelf().OfType<IConversionOperation>().Single();
        Assert.AreEqual(throws, !classifier.Classify(conversion).IsEmpty);
    }

    [TestMethod]
    public void RefGetterRunsAtTargetEvaluation()
    {
        var (root, classifier) = Compile("class C { int storage; ref int P => ref storage; void M() { P = 1; } }");
        var assignment = root.DescendantsAndSelf().OfType<ISimpleAssignmentOperation>().Single();
        Assert.IsFalse(classifier.Classify(assignment.Target).IsEmpty);
        Assert.IsTrue(classifier.Classify(assignment).IsEmpty);
    }

    [TestMethod]
    [DataRow("new C()")]
    [DataRow("new int[1]")]
    [DataRow("Call()")]
    public void CallsAndAllocationsHaveUnknownExceptions(string expression)
    {
        var (root, classifier) = Compile($"class C {{ object Call() => null; object M() => {expression}; }}");
        var operation = root.DescendantsAndSelf().Single(operation => operation is IObjectCreationOperation or IArrayCreationOperation or IInvocationOperation);
        Assert.AreSequenceEqual([null], classifier.Classify(operation).ToArray());
    }

    [TestMethod]
    [DataRow("record R;", "R", "x with { }", true)]
    [DataRow("record R { protected R(R original) { throw new System.Exception(); } }", "R", "x with { }", true)]
    [DataRow("record struct R;", "R", "x with { }", false)]
    [DataRow("", "object", "new { Value = x } with { }", false)]
    public void RecordCloneCallsHaveUnknownExceptions(string declaration, string type, string expression, bool throws)
    {
        var (root, classifier) = Compile($"{declaration} class C {{ object M({type} x) => {expression}; }}");
        var operation = root.DescendantsAndSelf().OfType<IWithOperation>().Single();
        Assert.AreEqual(throws, operation.CloneMethod is not null);
        Assert.AreSequenceEqual(throws ? [null] : [], [.. classifier.Classify(operation)]);
    }

    [TestMethod]
    [DataRow("int M(C x, System.Index i) => x[i];")]
    [DataRow("C M(C x, System.Range r) => x[r];")]
    public void ImplicitIndexerCallsHaveUnknownExceptions(string method)
    {
        var (root, classifier) = Compile($"class C {{ public int Length => throw new System.Exception(); public int this[int i] => throw new System.Exception(); public C Slice(int start, int length) => throw new System.Exception(); {method} }}");
        var access = root.DescendantsAndSelf().OfType<IImplicitIndexerReferenceOperation>().Single();
        Assert.AreSequenceEqual([null], classifier.Classify(access).ToArray());
    }

    [TestMethod]
    [DataRow("x[i] = 1;", false)]
    [DataRow("x[i] += 1;", false)]
    [DataRow("x[i]++;", false)]
    [DataRow("x[i] = 1;", true)]
    public void ImplicitIndexerSeparatesLengthAndSetterCalls(string statement, bool returnsByRef)
    {
        var indexer = returnsByRef ? "public ref int this[int i] => throw new System.Exception();"
            : "public int this[int i] { get => throw new System.Exception(); set => throw new System.Exception(); }";
        var (root, classifier) = Compile($"class C {{ public int Length => throw new System.Exception(); {indexer} void M(C x, System.Index i) {{ {statement} }} }}");
        var access = root.DescendantsAndSelf().OfType<IImplicitIndexerReferenceOperation>().Single();
        Assert.AreSequenceEqual([null], classifier.Classify(access).ToArray());
        var store = root.DescendantsAndSelf().Single(operation => operation is IAssignmentOperation or IIncrementOrDecrementOperation);
        Assert.AreSequenceEqual(returnsByRef ? [] : [null], classifier.Classify(store).ToArray());
    }

    [TestMethod]
    public void MissingSemanticModelConservativelyClassifiesDeconstruction()
    {
        var (root, _) = Compile("class C { void M() { (int a, int b) = (1, 2); } }");
        var operation = root.DescendantsAndSelf().OfType<IDeconstructionAssignmentOperation>().Single();
        Assert.AreSequenceEqual([null], new OperationExceptionClassifier().Classify(operation).ToArray());
    }

    [TestMethod]
    [DataRow("x.P = 1;", false)]
    [DataRow("x[0] = 1;", false)]
    [DataRow("x.P += 1;", true)]
    [DataRow("x.P++;", true)]
    public void DynamicSetterRunsAfterValueAndCompoundAccessAlsoReads(string statement, bool reads)
    {
        var (root, classifier) = Compile($"class C {{ void M(dynamic x) {{ {statement} }} }}");
        var access = root.DescendantsAndSelf().Single(operation => operation is IDynamicMemberReferenceOperation or IDynamicIndexerAccessOperation);
        Assert.AreEqual(reads, !classifier.Classify(access).IsEmpty);
        var store = root.DescendantsAndSelf().Single(operation => operation is IAssignmentOperation or IIncrementOrDecrementOperation);
        Assert.IsFalse(classifier.Classify(store).IsEmpty);
    }

    [TestMethod]
    [DataRow("(V, V)", "==", true)]
    [DataRow("(V, V)", "!=", true)]
    [DataRow("((int, V), int)", "==", true)]
    [DataRow("(V?, int)?", "!=", true)]
    [DataRow("(dynamic, int)", "==", true)]
    [DataRow("(int, int)", "==", false)]
    [DataRow("((int?, decimal), string)", "!=", false)]
    [DataRow("(System.DayOfWeek, object)?", "==", false)]
    public void TupleParameterComparisonsInspectHiddenElementOperators(string type, string comparison, bool throws)
    {
        var (root, classifier) = Compile($$"""
            struct V {
                public static bool operator ==(V x, V y) => throw new System.Exception();
                public static bool operator !=(V x, V y) => throw new System.Exception();
            }
            class C { bool M({{type}} x, {{type}} y) => x {{comparison}} y; }
            """);
        var operation = root.DescendantsAndSelf().OfType<ITupleBinaryOperation>().Single();
        Assert.AreSequenceEqual(throws ? [null] : [], classifier.Classify(operation).ToArray());
    }

    [TestMethod]
    [DataRow("(V, V)", "(int, int)", "x", true)]
    [DataRow("(V, V)?", "(int, int)?", "x", true)]
    [DataRow("(int, int)?", "(long, long)?", "x", false)]
    [DataRow("((V, V), int)", "((int, int), long)", "x", true)]
    [DataRow("(int, int)", "(long, long)", "x", false)]
    [DataRow("(int, int)", "(int, int)", "x", false)]
    [DataRow("(decimal, int)", "(int, long)", "((int, long))x", true)]
    public void TupleParameterConversionsInspectElementConversions(string from, string to, string expression, bool throws)
    {
        var (root, classifier) = Compile($"class V {{ public static implicit operator int(V value) => throw new System.Exception(); }} class C {{ {to} M({from} x) => {expression}; }}");
        var result = root.DescendantsAndSelf().SelectMany(operation => classifier.Classify(operation));
        Assert.AreEqual(throws, result.Any());
    }

    [TestMethod]
    [DataRow("object M(object[] a, object value) => a[0] = value;", true)]
    [DataRow("object M(object[] a, object value) { return a[0] = value; }", true)]
    [DataRow("System.Func<object, object> M(object[] a) => value => a[0] = value;", true)]
    [DataRow("object M(object[] a) => a[0] = null;", false)]
    [DataRow("V M(V[] a, V value) => a[0] += value;", true)]
    [DataRow("V M(V[] a) => a[0]++;", true)]
    public void ReturnedArrayStoresPreserveCovarianceChecks(string method, bool mismatch)
    {
        var (root, classifier) = Compile($"class V {{ public static V operator +(V x, V y) => x; public static V operator ++(V x) => x; }} class C {{ {method} }}");
        var store = root.DescendantsAndSelf().Single(operation => operation is IAssignmentOperation or IIncrementOrDecrementOperation);
        Assert.AreEqual(mismatch, classifier.Classify(store).Contains("System.ArrayTypeMismatchException"));
        Assert.DoesNotContain("System.ArrayTypeMismatchException", new OperationExceptionClassifier(hasExactArrayType: _ => true).Classify(store));
    }

    [TestMethod]
    public void ImplicitDeconstructElementConversionIsNotAnOperationChild()
    {
        var (root, classifier) = Compile("""
            class V { public static implicit operator int(V value) => 0; }
            class C {
                public void Deconstruct(out V a, out V b) { a = b = null; }
                void M(C x) { (int a, int b) = x; }
            }
            """);
        var deconstruction = root.DescendantsAndSelf().OfType<IDeconstructionAssignmentOperation>().Single();
        Assert.IsFalse(classifier.Classify(deconstruction).IsEmpty);
    }

    [TestMethod]
    public void DeconstructionArrayStoreUsesAssignedValue()
    {
        var (root, classifier) = Compile("class C { void M(object[] a, object value) { (a[0], a[1]) = (value, null); } }");
        var assignment = root.DescendantsAndSelf().OfType<IDeconstructionAssignmentOperation>().Single();
        var targets = ((ITupleOperation) assignment.Target).Elements;
        var value = assignment.Value;
        while (value is IConversionOperation conversion)
        {
            value = conversion.Operand;
        }

        var values = ((ITupleOperation) value).Elements;
        Assert.Contains("System.ArrayTypeMismatchException", classifier.Classify(targets[0], deconstructionStore: true, storedValue: values[0]));
        Assert.DoesNotContain("System.ArrayTypeMismatchException", classifier.Classify(targets[1], deconstructionStore: true, storedValue: values[1]));
    }

    [TestMethod]
    public void MethodReferenceRequiresReceiverButStaticAndExtensionReferencesDoNot()
    {
        var (root, classifier) = Compile("""
            static class Extensions { public static void Extension(this C x) { } }
            class C {
                void Instance() { } static void Static() { }
                void M(C x) { System.Action a = x.Instance; System.Action b = Static; System.Action c = x.Extension; }
            }
            """);
        var methods = root.DescendantsAndSelf().OfType<IMethodReferenceOperation>().ToArray();
        Assert.AreSequenceEqual(NullReceiverExceptions, classifier.Classify(methods[0]).ToArray());
        Assert.IsTrue(classifier.Classify(methods[1]).IsEmpty);
        Assert.IsTrue(classifier.Classify(methods[2]).IsEmpty);
    }

    [TestMethod]
    [DataRow("System.IDisposable", true)]
    [DataRow("class, System.IDisposable", true)]
    [DataRow("notnull, System.IDisposable", true)]
    [DataRow("struct, System.IDisposable", false)]
    [DataRow("unmanaged, System.IDisposable", false)]
    public void GenericMethodReferencePreservesNullableReceiver(string constraint, bool canBeNull)
    {
        var (root, classifier) = Compile($$"""
            class C {
                static System.Action M<T>(T x) where T : {{constraint}} => x.Dispose;
            }
            """);
        var method = root.DescendantsAndSelf().OfType<IMethodReferenceOperation>().Single();
        Assert.AreSequenceEqual(canBeNull ? NullReceiverExceptions : [], classifier.Classify(method).ToArray());
        Assert.IsTrue(new OperationExceptionClassifier(isKnownNonNull: _ => true).Classify(method).IsEmpty);
    }

    [TestMethod]
    [DataRow("range", true)]
    [DataRow("1..3", true)]
    [DataRow("..^1", true)]
    [DataRow("index", false)]
    [DataRow("^1", false)]
    public void ArrayRangeSlicesPreserveUnknownExceptions(string index, bool isSlice)
    {
        var (root, classifier) = Compile($"class C {{ object M(int[] a, System.Range range, int index) => a[{index}]; }}");
        var array = root.DescendantsAndSelf().OfType<IArrayElementReferenceOperation>().Single();
        string?[] expected = isSlice ? [null] : ["System.NullReferenceException", "System.IndexOutOfRangeException"];
        Assert.AreSequenceEqual(expected, [.. classifier.Classify(array)]);
        Assert.AreSequenceEqual([.. classifier.Classify(array)],
            [.. new OperationExceptionClassifier(isKnownNonNull: _ => true, hasExactArrayType: _ => true).Classify(array)]);
    }

    [TestMethod]
    [DataRow("System.Index M(int value) => ^value;")]
    [DataRow("object M(int[] array, int value) => array[^value];")]
    [DataRow("System.Range M(int value) => ..^value;")]
    public void FromEndIndexConstructionRetainsUnknownExceptions(string method)
    {
        var (root, classifier) = Compile($"class C {{ {method} }}");
        var index = root.DescendantsAndSelf().OfType<IUnaryOperation>().Single(operation => operation.OperatorKind == UnaryOperatorKind.Hat);
        Assert.AreSequenceEqual([null], [.. classifier.Classify(index)]);
    }

    private static (IOperation Root, OperationExceptionClassifier Classifier) Compile(string source,
        LanguageVersion version = LanguageVersion.Preview, ImmutableArray<MetadataReference> references = default,
        bool checkOverflow = false, bool allowUnsafe = false)
    {
        var tree = CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(version));
        var paths = ((string) AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator);
        var compilation = CSharpCompilation.Create("ClassifierTests", [tree],
            references.IsDefault ? paths.Select(path => MetadataReference.CreateFromFile(path)) : references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, checkOverflow: checkOverflow, allowUnsafe: allowUnsafe));
        var errors = compilation.GetDiagnostics().Where(d => d.Severity == DiagnosticSeverity.Error).ToArray();
        Assert.IsEmpty(errors, string.Join(Environment.NewLine, errors.Select(e => e.ToString())));
        var model = compilation.GetSemanticModel(tree);
        var method = tree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().Single(m => m.Identifier.Text == "M");
        return (model.GetOperation(method)!, new OperationExceptionClassifier(model));
    }
}
