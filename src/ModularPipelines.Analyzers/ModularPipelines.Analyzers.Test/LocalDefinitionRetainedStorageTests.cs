using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.Text;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class LocalDefinitionRetainedStorageTests
{
    [TestMethod]
    [DataRow("var copy = holder[..];", "copy.Field = 2")]
    [DataRow("var copy = holder[0..1]; var second = copy;", "second.Field = 2")]
    [DataRow("ref int copy = ref holder[^1];", "copy = 2")]
    public void Implicit_Index_And_Range_Results_Retain_Receiver_Storage(string binding, string store)
    {
        var scenario = new Scenario("int value = 0; var holder = new Retained(ref value); " + binding
            + " value = 3; " + store + "; value = 4;");
        var indexer = scenario.Root.DescendantsAndSelf().OfType<IImplicitIndexerReferenceOperation>().Single();
        Assert.IsTrue(binding.Contains("..", StringComparison.Ordinal)
            ? indexer.IndexerSymbol is IMethodSymbol { Name: "Slice" }
            : indexer.IndexerSymbol is IPropertySymbol { ReturnsByRef: true });
        scenario.Store("value = 3");
        scenario.AssertValues("3");
        scenario.Store(store);
        scenario.AssertUnknown();
        scenario.Store("value = 4");
        scenario.AssertValues("4");

        var unrelated = new Scenario("int value = 0; int other = 0; var holder = new Retained(ref other); "
            + binding + " value = 3; " + store + ";");
        unrelated.Store("value = 3");
        unrelated.Store(store);
        unrelated.AssertValues("3");
    }

    [TestMethod]
    [DataRow("var copy = holder;", "copy.Field = 2")]
    [DataRow("scoped Retained middle = default; scoped Retained copy = (middle = holder);", "copy.Field = 2")]
    [DataRow("scoped InstanceRetainedOperators copy = default; copy += holder;", "copy.Field = 2")]
    [DataRow("scoped InstanceRetainedContainer container = default; container.Item += holder;", "container.Item.Field = 2")]
    [DataRow("var copy = Return(holder); var second = copy;", "second.Field = 2")]
    [DataRow("ref int copy = ref ReturnRef(holder);", "copy = 2")]
    [DataRow("scoped Retained copy = default; copy = holder;", "copy.Field = 2")]
    [DataRow("scoped Retained copy = default; copy.Field = ref value;", "copy.Field = 2")]
    [DataRow("scoped Retained copy = default; copy.Bind(ref value);", "copy.Field = 2")]
    [DataRow("scoped Retained copy = default; Bind(ref copy, ref value);", "copy.Field = 2")]
    [DataRow("scoped Retained copy = default; BindScoped(scopedHolder: ref copy, field: ref value);", "copy.Field = 2")]
    [DataRow("scoped Retained copy = default; BindOut(out copy, ref value);", "copy.Field = 2")]
    [DataRow("scoped Retained copy = default; new Binder(ref copy, ref value);", "copy.Field = 2")]
    [DataRow("scoped Retained copy = default; new OutBinder(out copy, ref value);", "copy.Field = 2")]
    [DataRow("scoped Retained copy = default; holder.Copy(out copy);", "copy.Field = 2")]
    [DataRow("scoped Retained copy = default; holder.CopyRef(ref copy);", "copy.Field = 2")]
    [DataRow("scoped Retained copy = default; (copy, _) = holder;", "copy.Field = 2")]
    [DataRow("scoped RetainedContainer container = default; container.Item = holder;", "container.Item.Field = 2")]
    [DataRow("scoped RetainedContainer container = new() { Item = holder };", "container.Item.Field = 2")]
    [DataRow("scoped RetainedCollection items = new() { holder };", "items.Item.Field = 2")]
    [DataRow("var copy = flag switch { true => holder, _ => default };", "copy.Field = 2")]
    [DataRow("var copy = flag ? holder : default;", "copy.Field = 2")]
    [DataRow("var copy = -holder;", "copy.Field = 2")]
    [DataRow("var copy = holder - 0;", "copy.Field = 2")]
    [DataRow("var copy = 0 - holder;", "copy.Field = 2")]
    [DataRow("var copy = ++holder;", "copy.Field = 2")]
    [DataRow("var copy = holder++;", "copy.Field = 2")]
    [DataRow("var copy = holder with { };", "copy.Field = 2")]
    [DataRow("scoped RetainedContainer container = default; var copy = container with { Item = holder };", "copy.Item.Field = 2")]
    [DataRow("scoped Retained copy = default; copy += holder;", "copy.Field = 2")]
    [DataRow("scoped RetainedContainer container = default; container.Item += holder;", "container.Item.Field = 2")]
    [DataRow("scoped RetainedContainer container = default; _ = container[holder];", "container.Item.Field = 2")]
    [DataRow("scoped RetainedContainer container = default; container[holder] = 0;", "container.Item.Field = 2")]
    [DataRow("var sink = new ReturningSink(); var copy = sink[holder, 0];", "copy.Field = 2")]
    [DataRow("var sink = new ReturningSink(); ref int copy = ref sink[holder];", "copy = 2")]
    [DataRow("parameter = holder;", "parameter.Field = 2")]
    [DataRow("parameter.Field = ref value;", "parameter.Field = 2")]
    [DataRow("BindScoped(ref parameter, ref value);", "parameter.Field = 2")]
    public void Retained_Storage_Closure_Invalidates_Only_When_Caller_Applies_Store(string binding, string store)
    {
        var scenario = new Scenario("int value = 0; var holder = new Retained(ref value); " + binding
            + " value = 3; " + store + "; value = 4;");
        // Computing the callable-wide closure does not execute bindings or stores.
        scenario.AssertValues("0");
        scenario.Store("value = 3");
        scenario.AssertValues("3");
        var target = ((ISimpleAssignmentOperation) scenario.Operation(store)).Target;
        Assert.IsTrue(scenario.Transfer.ReferencesStorage(target));
        scenario.Store(store);
        scenario.AssertUnknown();
        scenario.Store("value = 4");
        scenario.AssertValues("4");

        // Replacing every source of the tracked local with unrelated storage keeps precision.
        var unrelated = new Scenario("int value = 0; int other = 0; var holder = new Retained(ref other); "
            + binding.Replace("ref value", "ref other", StringComparison.Ordinal)
            + " value = 3; " + store + ";");
        unrelated.Store("value = 3");
        unrelated.Store(store);
        unrelated.AssertValues("3");
    }

    [TestMethod]
    [DataRow("if (holder is var copy) { value = 3; copy.Field = 2; }")]
    [DataRow("switch (holder) { case var copy: value = 3; copy.Field = 2; break; }")]
    [DataRow("foreach (var copy in new RetainedItems(holder)) { value = 3; copy.Field = 2; }")]
    [DataRow("foreach (ref var copy in new RetainedItems(holder)) { value = 3; copy.Field = 2; }")]
    public void Implicit_Bindings_Retain_Input_Storage(string body)
    {
        var scenario = new Scenario("int value = 0; var holder = new Retained(ref value); " + body);
        scenario.Store("value = 3");
        scenario.AssertValues("3");
        scenario.Store("copy.Field = 2");
        scenario.AssertUnknown();
    }

    [TestMethod]
    [DataRow("Mutate(holder, value = 3)")]
    [DataRow("MutateReadonly(in holder)")]
    [DataRow("MutateScoped(holder)")]
    [DataRow("new Consumer(holder)")]
    public void Retained_Arguments_Invalidate_At_Caller_Selected_Call_Time(string call)
    {
        var scenario = new Scenario("int value = 0; var holder = new Retained(ref value); value = 3; " + call + "; value = 4;");
        scenario.Store("value = 3");
        scenario.AssertValues("3");
        var operation = scenario.Operation(call);
        // Call evaluation and mutation of ref-like values remain caller-owned, including in/scoped values.
        foreach (var argument in operation.ChildOperations.OfType<IArgumentOperation>())
        {
            if (argument.Value.Type?.IsRefLikeType == true)
            {
                scenario.Transfer.InvalidateStorage(argument.Value, scenario.State);
            }
        }

        scenario.Transfer.InvalidateByReferenceArguments(operation, scenario.State);
        scenario.AssertUnknown();
        scenario.Store("value = 4");
        scenario.AssertValues("4");
    }

    [TestMethod]
    public void Receiver_Invalidation_Uses_The_Same_Closure_Without_Executing_Children()
    {
        var scenario = new Scenario("int value = 0; var holder = new Retained(ref value); var copy = holder; value = 3; copy.Mutate();");
        scenario.Store("value = 3");
        var invocation = (IInvocationOperation) scenario.Operation("copy.Mutate()");
        scenario.AssertValues("3");
        scenario.Transfer.InvalidateStorage(invocation.Instance!, scenario.State);
        scenario.AssertUnknown();
    }

    [TestMethod]
    public void Scoped_Ref_Sources_Do_Not_Create_Retained_Bindings()
    {
        var scenario = new Scenario("int value = 0; scoped Retained copy = default; IgnoreScoped(ref copy, ref value); value = 3; copy.Field = 2;");
        scenario.Store("value = 3");
        scenario.Store("copy.Field = 2");
        scenario.AssertValues("3");
    }

    [TestMethod]
    [DataRow("holder += 1", 1)]
    [DataRow("++holder", 0)]
    public void Instance_Operators_Have_Void_Results_And_Cannot_Enter_Result_Retention(string expression, int parameterCount)
    {
        var scenario = new Scenario("int value = 0; var holder = new InstanceOperators(); " + expression + ";");
        var operation = scenario.Operation(expression);
        var method = operation switch
        {
            ICompoundAssignmentOperation compound => compound.OperatorMethod,
            IIncrementOrDecrementOperation increment => increment.OperatorMethod,
            _ => null,
        };
        Assert.IsNotNull(method);
        Assert.IsFalse(method.IsStatic);
        Assert.AreEqual(parameterCount, method.Parameters.Length);
        Assert.AreEqual(SpecialType.System_Void, operation.Type!.SpecialType);
        Assert.IsFalse(scenario.Transfer.ReferencesStorage(operation));
    }

    [TestMethod]
    public void Incomplete_Deconstruction_Does_Not_Throw_While_Building_Storage_Closure()
    {
        var scenario = new Scenario("int value = 0; int a = 0, b = 0, c = 0; (a, b, c) = (1, 2);", "CS8132");
        var assignment = scenario.Root.DescendantsAndSelf().OfType<IDeconstructionAssignmentOperation>().Single();
        Assert.AreEqual(3, ((ITupleOperation) assignment.Target).Elements.Length);
        Assert.AreEqual(2, ((ITupleOperation) assignment.Value).Elements.Length);
        scenario.AssertValues("0");
    }

    private sealed class Scenario
    {
        private static readonly MetadataReference[] References = [.. ((string) AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!)
            .Split(Path.PathSeparator).Select(static path => MetadataReference.CreateFromFile(path))];

        internal Scenario(string body, string? expectedError = null)
        {
            var source = Types + "class Example { " + Methods
                + "static void Run(bool flag, scoped Retained parameter) { " + body + " } }";
            var tree = CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(LanguageVersion.Preview));
            var compilation = CSharpCompilation.Create("RetainedStorageTest", [tree], References,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            var errors = compilation.GetDiagnostics().Where(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error).ToArray();
            if (expectedError is null)
            {
                Assert.IsEmpty(errors, string.Join(Environment.NewLine, errors.Select(static error => error.ToString())));
            }
            else
            {
                Assert.IsTrue(errors.Any(error => error.Id == expectedError));
            }
            var model = compilation.GetSemanticModel(tree);
            var method = tree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().Single(static method => method.Identifier.ValueText == "Run");
            Root = model.GetOperation(method)!;
            var local = method.DescendantNodes().Select(node => model.GetDeclaredSymbol(node)).OfType<ILocalSymbol>()
                .Single(static symbol => symbol.Name == "value");
            Transfer = new LocalDefinitionTransfer(Root, local);
            Transfer.ApplyStore(Root.DescendantsAndSelf().OfType<IVariableDeclaratorOperation>()
                .Single(declarator => SymbolEqualityComparer.Default.Equals(declarator.Symbol, local)), State);
        }

        internal IOperation Root { get; }
        internal LocalDefinitionTransfer Transfer { get; }
        internal HashSet<TextSpan> State { get; } = LocalDefinitionTransfer.CreateIncomingState();
        internal IOperation Operation(string syntax) => Root.DescendantsAndSelf()
            .First(operation => operation.Syntax.ToString() == syntax && operation is not (IExpressionStatementOperation or IArgumentOperation));
        internal void Store(string syntax) => Transfer.ApplyStore(Operation(syntax), State);
        internal void AssertUnknown() => Assert.IsFalse(Transfer.TryGetValues(State, out _, out _));
        internal void AssertValues(string expected)
        {
            Assert.IsTrue(Transfer.TryGetValues(State, out var values, out var incoming));
            Assert.IsFalse(incoming);
            Assert.AreSequenceEqual(new[] { expected }, values.Select(static value => value.Syntax.ToString()).ToArray());
        }
    }

    private const string Methods = """
        static Retained Return(Retained holder) => holder;
        static ref int ReturnRef(Retained holder) => ref holder.Field;
        static void Bind(ref Retained holder, [System.Diagnostics.CodeAnalysis.UnscopedRef] ref int field) { holder.Field = ref field; }
        static void BindScoped(scoped ref Retained scopedHolder, [System.Diagnostics.CodeAnalysis.UnscopedRef] ref int field) { scopedHolder.Field = ref field; }
        static void BindOut(out Retained holder, [System.Diagnostics.CodeAnalysis.UnscopedRef] ref int field) { holder = new Retained(ref field); }
        static void IgnoreScoped(scoped ref Retained holder, scoped ref int field) { }
        static void Mutate(Retained holder, int argument) { holder.Field = 2; }
        static void MutateReadonly(in Retained holder) { holder.Field = 2; }
        static void MutateScoped(scoped Retained holder) { holder.Field = 2; }
        """;

    private const string Types = """
        using System;
        ref struct Retained
        {
            public ref int Field;
            public Retained(ref int field) { Field = ref field; }
            public int Length => 1;
            public ref int this[int index] => ref Field;
            public Retained Slice(int start, int length) => this;
            public void Bind([System.Diagnostics.CodeAnalysis.UnscopedRef] ref int field) { Field = ref field; }
            public readonly void Copy(out Retained copy) { copy = this; }
            public readonly void CopyRef(ref Retained copy) { copy = this; }
            public readonly void Deconstruct(out Retained copy, out int ignored) { copy = this; ignored = 0; }
            public readonly void Mutate() { Field = 2; }
            public static Retained operator +(Retained left, Retained right) => right;
            public static Retained operator ++(Retained holder) => holder;
            public static Retained operator -(Retained holder) => holder;
            public static Retained operator -(Retained holder, int unused) => holder;
            public static Retained operator -(int unused, Retained holder) => holder;
        }
        ref struct RetainedContainer
        {
            public Retained Item;
            [System.Runtime.CompilerServices.IndexerName("RetainedAt")]
            public int this[Retained item] { get { Item = item; return 0; } set { Item = item; } }
        }
        ref struct RetainedCollection : System.Collections.IEnumerable
        {
            public Retained Item;
            public void Add(Retained item) { Item = item; }
            public System.Collections.IEnumerator GetEnumerator() => throw new Exception();
        }
        ref struct RetainedItems
        {
            private Retained _item;
            public RetainedItems(Retained item) { _item = item; }
            public RetainedItems GetEnumerator() => this;
            public bool MoveNext() => true;
            [System.Diagnostics.CodeAnalysis.UnscopedRef]
            public ref Retained Current => ref _item;
        }
        class ReturningSink
        {
            public ref int this[Retained holder] => ref holder.Field;
            public Retained this[Retained holder, int index] => holder;
        }
        class Binder
        {
            public Binder(ref Retained holder, [System.Diagnostics.CodeAnalysis.UnscopedRef] ref int field) { holder.Field = ref field; }
        }
        class OutBinder
        {
            public OutBinder(out Retained holder, [System.Diagnostics.CodeAnalysis.UnscopedRef] ref int field) { holder = new Retained(ref field); }
        }
        class Consumer { public Consumer(Retained holder) { holder.Field = 2; } }
        ref struct InstanceRetainedOperators
        {
            public ref int Field;
            public void operator +=(Retained other) { Field = ref other.Field; }
        }
        ref struct InstanceRetainedContainer
        {
            public InstanceRetainedOperators Item;
        }
        struct InstanceOperators
        {
            public void operator +=(int value) { }
            public void operator ++() { }
        }
        """;
}
