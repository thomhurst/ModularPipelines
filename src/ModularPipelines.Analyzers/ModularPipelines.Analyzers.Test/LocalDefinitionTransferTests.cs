using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class LocalDefinitionTransferTests
{
    [TestMethod]
    public void Inline_Array_Element_Ref_Local_Tracks_Backing_Storage()
    {
        var scenario = new Scenario("Buffer buffer = default; ref int value = ref buffer[0];"
            + "value = 1; buffer[0] = 2; value = 3;");
        scenario.Store("value = 1");
        scenario.AssertValues("1");
        scenario.Store("buffer[0] = 2");
        scenario.AssertUnknown();
        scenario.Store("value = 3");
        scenario.AssertValues("3");
    }

    [TestMethod]
    [DataRow("value[0] = 1")]
    [DataRow("value[0]++")]
    [DataRow("value[0] += 1")]
    [DataRow("alias = 1")]
    [DataRow("bufferAlias[0] = 1")]
    [DataRow("(flag ? ref value : ref other)[0] = 1")]
    public void Inline_Array_Stores_Invalidate_Receiver_And_Recover(string store)
    {
        var scenario = new Scenario("Buffer value = default; Buffer other = default;"
            + "ref int alias = ref value[0]; ref Buffer bufferAlias = ref value;"
            + store + ";value = new Buffer();");
        scenario.AssertValues("default");
        scenario.Store(store);
        scenario.AssertUnknown();
        scenario.Store("value = new Buffer()");
        scenario.AssertValues("new Buffer()");
    }

    [TestMethod]
    [DataRow("Change(ref value[0])")]
    [DataRow("Assign(out value[0])")]
    [DataRow("Change(ref alias)")]
    public void Inline_Array_Ref_Out_Arguments_Invalidate_Receiver(string call)
    {
        var scenario = new Scenario("Buffer value = default; ref int alias = ref value[0];" + call + ";");
        scenario.Transfer.InvalidateByReferenceArguments(scenario.Operation(call), scenario.State);
        scenario.AssertUnknown();
    }

    [TestMethod]
    [DataRow("int value = 0; Buffer buffer = default;", "buffer[value] = 1", "0")]
    [DataRow("int value = 0; Buffer buffer = default; ref int alias = ref buffer[value];", "alias = 1", "0")]
    [DataRow("Buffer value = default; Buffer other = default;", "other[0] = 1", "default")]
    public void Inline_Array_Indices_And_Unrelated_Storage_Preserve_Definitions(string declarations, string store, string expected)
    {
        var scenario = new Scenario(declarations + store + ";");
        scenario.Store(store);
        scenario.AssertValues(expected);
    }

    [TestMethod]
    [DataRow("value.Field = ref other", true)]
    [DataRow("alias.Field = ref other", true)]
    [DataRow("unrelated.Field = ref other", false)]
    [DataRow("alias = ref unrelated", false)]
    public void Ref_Field_Rebinding_Invalidates_Only_Receiver_Storage(string store, bool invalidates)
    {
        var scenario = new Scenario("int source = 0; int other = 1; RefValue value = new RefValue(ref source);"
            + "RefValue unrelated = new RefValue(ref source); ref RefValue alias = ref value;"
            + store + "; value = default;");
        scenario.Store(store);
        if (invalidates)
        {
            scenario.AssertUnknown();
            scenario.Store("value = default");
            scenario.AssertValues("default");
        }
        else
        {
            scenario.AssertValues("new RefValue(ref source)");
        }
    }

    [TestMethod]
    [DataRow("holder.Field", "holder.Field = 2")]
    [DataRow("holder.Nested.Field", "holder.Nested.Field = 2")]
    [DataRow("items[0]", "items[0] = 2")]
    [DataRow("flag ? ref source : ref holder.Field", "holder.Field = 2")]
    [DataRow("alias", "holder.Field = 2")]
    public void Ref_Locals_With_Unrepresented_Backing_Storage_Remain_Unknown(string source, string store)
    {
        var scenario = new Scenario("var holder = new Holder(); int[] items = new int[1]; int source = 0;"
            + "ref int alias = ref holder.Field; ref int value = ref (" + source + "); value = 1;"
            + store + "; value = 3;");
        scenario.Store("value = 1");
        scenario.Store(store);
        scenario.AssertUnknown();
        scenario.Store("value = 3");
        scenario.AssertUnknown();
    }

    [TestMethod]
    [DataRow("Mutable value = new Mutable();", "value.Changed += A", true)]
    [DataRow("Mutable value = new Mutable();", "value.Changed -= A", true)]
    [DataRow("Mutable value = new Mutable(); ref Mutable alias = ref value;", "alias.Changed += A", true)]
    [DataRow("Mutable value = new Mutable(); ref Mutable alias = ref value;", "alias.Changed -= A", true)]
    [DataRow("Mutable value = new Mutable();", "value.Nested.Changed += A", true)]
    [DataRow("Mutable value = new Mutable(); Mutable other = new Mutable();", "(flag ? ref value : ref other).Changed += A", true)]
    [DataRow("Holder value = new Holder();", "value.Changed += A", false)]
    [DataRow("Mutable value = new Mutable();", "value.Reference.Changed -= A", false)]
    [DataRow("Mutable value = new Mutable(); Mutable other = new Mutable();", "other.Changed += A", false)]
    public void Event_Stores_Invalidate_Only_Value_Type_Storage(string declarations, string store, bool invalidates)
    {
        var scenario = new Scenario(declarations + store + ";value = default;");
        scenario.Store(store);
        if (invalidates)
        {
            scenario.AssertUnknown();
            scenario.Store("value = default");
            scenario.AssertValues("default");
        }
        else
        {
            scenario.AssertValues(declarations.StartsWith("Holder", StringComparison.Ordinal) ? "new Holder()" : "new Mutable()");
        }
    }

    [TestMethod]
    [DataRow("IMutable, new()", "value.Changed += A", true)]
    [DataRow("struct, IMutable", "value.Changed -= A", true)]
    [DataRow("class, IMutable, new()", "value.Changed += A", false)]
    [DataRow("IMutable, new()", "value.Property = 1", true)]
    [DataRow("IMutable, new()", "value.Property++", true)]
    [DataRow("IMutable, new()", "value[0] = 1", true)]
    [DataRow("struct, IMutable", "value.Property = 1", true)]
    [DataRow("class, IMutable, new()", "value.Property = 1", false)]
    [DataRow("MutableBase, new()", "value.Property = 1", false)]
    public void Generic_Member_Stores_Invalidate_Only_Potential_Value_Types(string constraints, string store, bool invalidates)
    {
        var scenario = new Scenario("T value = new T();" + store + ";value = new T();", constraints: constraints);
        scenario.AssertValues("new T()");
        scenario.Store(store);
        if (invalidates)
        {
            scenario.AssertUnknown();
            scenario.Store("value = new T()");
            scenario.AssertValues("new T()");
        }
        else
        {
            scenario.AssertValues("new T()");
        }
    }

    [TestMethod]
    [DataRow("(int X, int Y) value = (0, 0);", "value.X = 1")]
    [DataRow("Mutable value = new Mutable();", "value.Field = 1")]
    [DataRow("Mutable value = new Mutable();", "value.Property = 1")]
    [DataRow("Mutable value = new Mutable();", "value.Nested.Field = 1")]
    [DataRow("Mutable value = new Mutable();", "value.Field += 1")]
    [DataRow("Mutable value = new Mutable();", "value.Property++")]
    [DataRow("Mutable value = new Mutable();", "value.Nested.Field--")]
    [DataRow("Mutable value = new Mutable();", "value[0] = 1")]
    [DataRow("Mutable value = new Mutable();", "(value.Field, _) = (1, 0)")]
    [DataRow("Mutable value = new Mutable(); ref int alias = ref value.Field;", "alias = 1")]
    [DataRow("Mutable value = new Mutable(); ref Mutable alias = ref value;", "alias.Field = 1")]
    [DataRow("Mutable value = new Mutable(); ref Inner alias = ref value.Nested;", "alias.Property++")]
    [DataRow("Mutable source = new Mutable(); ref Mutable value = ref source;", "source.Field = 1")]
    [DataRow("Mutable source = new Mutable(); ref Mutable sibling = ref source; ref Mutable value = ref source;", "sibling.Property = 1")]
    public void Struct_Member_Stores_Invalidate_Whole_Local(string declarations, string store)
    {
        var scenario = new Scenario(declarations + store + ";");
        scenario.Store(store);
        scenario.AssertUnknown();
    }

    [TestMethod]
    [DataRow("int value = 1;", "value++")]
    [DataRow("int value = 1;", "++value")]
    [DataRow("int value = 1;", "value--")]
    [DataRow("int value = 1;", "value += 2")]
    [DataRow("int value = 1;", "value -= 2")]
    [DataRow("int value = 1;", "value *= 2")]
    [DataRow("int value = 1;", "value /= 2")]
    [DataRow("int value = 1;", "value %= 2")]
    [DataRow("int value = 1;", "value &= 2")]
    [DataRow("int value = 1;", "value |= 2")]
    [DataRow("int value = 1;", "value ^= 2")]
    [DataRow("int value = 1;", "value <<= 2")]
    [DataRow("int value = 1;", "value >>= 2")]
    [DataRow("int value = 1;", "value >>>= 2")]
    [DataRow("string value = \"old\";", "value += \"new\"")]
    [DataRow("Action value = A;", "value -= A")]
    [DataRow("dynamic value = 1;", "value += 2")]
    public void Unsupported_Direct_Stores_Invalidate(string declaration, string store)
    {
        var scenario = new Scenario(declaration + store + ";");
        scenario.Store(store);
        scenario.AssertUnknown();
    }

    [TestMethod]
    [DataRow("ref int alias = ref value;", "alias = 2")]
    [DataRow("ref int alias = ref value;", "alias++")]
    [DataRow("ref int alias = ref value;", "alias *= 2")]
    [DataRow("ref int alias = ref value; ref int second = ref alias;", "second = 2")]
    [DataRow("int other = 0; ref int alias = ref (flag ? ref value : ref other);", "alias = 2")]
    [DataRow("int other = 0; ref int alias = ref other; alias = ref value;", "alias = 2")]
    [DataRow("ref int alias = ref value;", "(alias, _) = (2, 0)")]
    public void Aliases_Invalidate_Only_When_Store_Is_Applied(string declarations, string store)
    {
        var scenario = new Scenario("int value = 1;" + declarations + store + ";");
        scenario.AssertValues("1");
        scenario.Store(store);
        scenario.AssertUnknown();
    }

    [TestMethod]
    [DataRow("int source = 0; ref int value = ref source;", "source = 2")]
    [DataRow("int source = 0; ref int sibling = ref source; ref int value = ref source;", "sibling = 2")]
    [DataRow("int source = 0; ref int value = ref source; ref int sibling = ref source;", "sibling++")]
    [DataRow("int source = 0; ref int middle = ref source; ref int value = ref middle;", "source = 2")]
    [DataRow("int source = 0; int other = 0; ref int value = ref (flag ? ref source : ref other);", "other = 2")]
    [DataRow("int source = 0; int other = 0; ref int value = ref other; value = ref source;", "source = 2")]
    [DataRow("int source = 0; ref int value = ref source;", "(source, _) = (2, 0)")]
    public void Reverse_And_Sibling_Aliases_Invalidate(string declarations, string store)
    {
        var scenario = new Scenario(declarations + "value = 1;" + store + ";value = 3;");
        scenario.Store("value = 1");
        scenario.Store(store);
        scenario.AssertUnknown();
        scenario.Store("value = 3");
        scenario.AssertValues("3");
    }

    [TestMethod]
    [DataRow("int value = 1;", "Change(ref value)")]
    [DataRow("int value = 1;", "Assign(out value)")]
    [DataRow("int value = 1; ref int alias = ref value;", "Change(ref alias)")]
    [DataRow("Mutable value = new Mutable();", "Change(ref value.Field)")]
    [DataRow("Mutable value = new Mutable();", "Assign(out value.Nested.Field)")]
    [DataRow("Mutable value = new Mutable(); ref Mutable alias = ref value;", "Change(ref alias.Field)")]
    [DataRow("int value = 1; int other = 0;", "Change(ref (flag ? ref value : ref other))")]
    [DataRow("int value = 1; dynamic receiver = item;", "receiver.Change(ref value)")]
    [DataRow("int value = 1; dynamic receiver = item;", "receiver.Assign(out value)")]
    [DataRow("int value = 1;", "new RefHolder(ref value)")]
    [DataRow("int value = 1; dynamic receiver = item;", "new RefHolder(receiver, ref value)")]
    public void Ref_Out_Calls_Invalidate_Before_Completion_Or_Throw(string declarations, string call)
    {
        var scenario = new Scenario(declarations + call + ";");
        scenario.Transfer.InvalidateByReferenceArguments(scenario.Operation(call), scenario.State);
        scenario.AssertUnknown();
    }

    [TestMethod]
    [DataRow("Mutable value = new Mutable();", "value.Reference.Field = 1", "new Mutable()")]
    [DataRow("Holder value = new Holder();", "value.Field = 1", "new Holder()")]
    [DataRow("Holder value = new Holder();", "value.Nested.Field = 1", "new Holder()")]
    [DataRow("int value = 1; var holder = new Holder();", "holder[value] = 2", "1")]
    [DataRow("int value = 1; int[] array = new int[3];", "array[value] = 2", "1")]
    [DataRow("int value = 1; var holder = new Holder(); ref int alias = ref holder[value];", "alias = 2", "1")]
    [DataRow("Holder value = new Holder(); ref int alias = ref value.Field;", "alias = 2", "new Holder()")]
    [DataRow("int value = 1; int other = 0; ref int alias = ref other;", "alias++", "1")]
    [DataRow("int value = 1; int other = 0; ref int alias = ref (flag ? ref value : ref other);", "other = 2", "1")]
    [DataRow("int value = 1; int other = 0; ref int alias = ref (flag ? ref other : ref value); ref int sibling = ref other;", "sibling++", "1")]
    [DataRow("int value = 1; int other = 0;", "other = value", "1")]
    public void Unrelated_And_Reference_Member_Stores_Preserve_Definitions(string declarations, string store, string expected)
    {
        var scenario = new Scenario(declarations + store + ";");
        scenario.Store(store);
        scenario.AssertValues(expected);
    }

    [TestMethod]
    [DataRow("int value = 1; int other = 0;", "Change(ref other)", "1")]
    [DataRow("int value = 1; int other = 0; ref int alias = ref (flag ? ref value : ref other);", "Change(ref other)", "1")]
    [DataRow("Holder value = new Holder();", "Change(ref value.Field)", "new Holder()")]
    [DataRow("int value = 1; int[] array = new int[3];", "Change(ref array[value])", "1")]
    [DataRow("int value = 1;", "Read(in value)", "1")]
    public void Unrelated_And_Readonly_Arguments_Preserve_Definitions(string declarations, string call, string expected)
    {
        var scenario = new Scenario(declarations + call + ";");
        scenario.Transfer.InvalidateByReferenceArguments(scenario.Operation(call), scenario.State);
        scenario.AssertValues(expected);
    }

    [TestMethod]
    public void Whole_Local_Overwrite_Recovers_After_Member_Write()
    {
        var scenario = new Scenario("Mutable value = new Mutable(); value.Field = 1; value = default;");
        scenario.Store("value.Field = 1");
        scenario.AssertUnknown();
        scenario.Store("value = default");
        scenario.AssertValues("default");
    }

    [TestMethod]
    public void Skipped_Member_Write_Does_Not_Transfer_And_Join_Preserves_Unknown()
    {
        var scenario = new Scenario("Mutable value = new Mutable(); if (flag) value.Field = 1;");
        var skipped = new HashSet<TextSpan>(scenario.State);
        scenario.AssertValues("new Mutable()");
        scenario.Store("value.Field = 1");
        scenario.State.UnionWith(skipped);
        scenario.AssertUnknown();
    }

    [TestMethod]
    public void Incoming_Unknown_And_Overwritten_States_Are_Distinct()
    {
        var scenario = new Scenario("int value = 1; value++; value = 2;", initialize: false);
        scenario.AssertValues("", incoming: true);
        var incoming = new HashSet<TextSpan>(scenario.State);
        scenario.Store("value++");
        scenario.AssertUnknown();
        scenario.State.UnionWith(incoming);
        scenario.AssertUnknown();
        scenario.Store("value = 2");
        scenario.AssertValues("2");
        scenario.State.UnionWith(incoming);
        scenario.AssertValues("2", incoming: true);
    }

    [TestMethod]
    [DataRow("(value, _) = (\"tuple\", 0)", "\"tuple\"")]
    [DataRow("(value, _) = ((\"tuple\", 0))", "\"tuple\"")]
    [DataRow("((value, _), _) = ((\"nested\", 0), 0)", "\"nested\"")]
    [DataRow("(value, value) = (\"first\", \"last\")", "\"last\"")]
    [DataRow("(value, (value, _)) = (\"known\", pair)", null)]
    [DataRow("((value, _), value) = (pair, \"known\")", "\"known\"")]
    [DataRow("(value, _) = pair", null)]
    [DataRow("(value, string other) = pair", null)]
    [DataRow("(_, value) = pair", null)]
    [DataRow("((value, _), _) = (pair, 0)", null)]
    [DataRow("(value, _) = holder", null)]
    public void Tuple_Stores_Use_Element_Expressions_Or_Unknown(string store, string? expected)
    {
        var scenario = new Scenario("string value = \"old\"; var pair = (\"new\", \"next\"); var holder = new Holder();" + store + ";");
        scenario.Store(store);
        if (expected is null)
        {
            scenario.AssertUnknown();
        }
        else
        {
            scenario.AssertValues(expected);
        }
    }

    [TestMethod]
    [DataRow("(flag ? ref value : ref other).Field = 1")]
    [DataRow("(flag ? ref other : ref value).Property++")]
    [DataRow("(flag ? ref value : ref other).Nested.Field += 1")]
    [DataRow("(flag ? ref value : ref other)[0] = 1")]
    [DataRow("Change(ref (flag ? ref value : ref other).Nested.Field)")]
    public void Lowered_Conditional_Ref_Member_Storage_Is_Recognized(string expression)
    {
        var scenario = new Scenario("Mutable value = new Mutable(); Mutable other = new Mutable();" + expression + ";");
        var graph = ControlFlowGraph.Create((IMethodBodyOperation) scenario.Root, TestContext.CancellationToken);
        var operation = graph.Blocks.SelectMany(static block => block.Operations)
            .SelectMany(static operation => operation.DescendantsAndSelf())
            .Single(operation => operation.Syntax.ToString() == expression
                && operation is IAssignmentOperation or IIncrementOrDecrementOperation or IInvocationOperation);
        scenario.Transfer.ApplyStore(operation, scenario.State);
        scenario.Transfer.InvalidateByReferenceArguments(operation, scenario.State);
        scenario.AssertUnknown();
    }

    [TestMethod]
    [DataRow("(flag ? ref value : ref other).Reference.Field = 1")]
    [DataRow("(value.Field == 0 ? ref other : ref third).Field = 1")]
    public void Lowered_Conditional_Ref_Unrelated_Member_Storage_Preserves_Local(string expression)
    {
        var scenario = new Scenario("Mutable value = new Mutable(); Mutable other = new Mutable(); Mutable third = new Mutable();" + expression + ";");
        var graph = ControlFlowGraph.Create((IMethodBodyOperation) scenario.Root, TestContext.CancellationToken);
        var operation = graph.Blocks.SelectMany(static block => block.Operations)
            .SelectMany(static operation => operation.DescendantsAndSelf())
            .OfType<ISimpleAssignmentOperation>()
            .Single(operation => operation.Syntax.ToString() == expression);
        scenario.Transfer.ApplyStore(operation, scenario.State);
        scenario.AssertValues("new Mutable()");
    }

    [TestMethod]
    public void Caller_Can_Apply_Deconstruction_Stores_Individually()
    {
        var scenario = new Scenario("string value = \"old\"; (value, value) = (\"first\", \"second\");");
        var assignment = (IDeconstructionAssignmentOperation) scenario.Operation("(value, value) = (\"first\", \"second\")");
        var targets = (ITupleOperation) assignment.Target;
        var values = (ITupleOperation) assignment.Value;
        scenario.Transfer.ApplyTargetStore(targets.Elements[0], values.Elements[0], scenario.State);
        scenario.AssertValues("\"first\"");
        scenario.Transfer.ApplyTargetStore(targets.Elements[1], values.Elements[1], scenario.State);
        scenario.AssertValues("\"second\"");
    }

    [TestMethod]
    public void Direct_Overwrites_And_Branch_Joins_Retain_Exact_Source_Expressions()
    {
        var scenario = new Scenario("string value = null; string other = value; value = other; value = string.Empty;");
        var skipped = new HashSet<TextSpan>(scenario.State);
        scenario.Store("value = other");
        scenario.AssertValues("other");
        scenario.State.UnionWith(skipped);
        scenario.AssertValues("null|other");
        scenario.Store("value = string.Empty");
        scenario.AssertValues("string.Empty");
    }

    [TestMethod]
    [DataRow("Change(ref (flag ? ref value : ref other))", true)]
    [DataRow("(flag ? ref value : ref other) = 2", false)]
    public void Lowered_Conditional_Ref_Storage_Is_Recognized(string expression, bool call)
    {
        var scenario = new Scenario("int value = 1; int other = 0;" + expression + ";");
        var graph = ControlFlowGraph.Create((IMethodBodyOperation) scenario.Root, TestContext.CancellationToken);
        var operation = graph.Blocks.SelectMany(static block => block.Operations)
            .SelectMany(static operation => operation.DescendantsAndSelf())
            .Single(operation => operation.Syntax.ToString() == expression
                && (call ? operation is IInvocationOperation : operation is ISimpleAssignmentOperation));
        if (call)
        {
            scenario.Transfer.InvalidateByReferenceArguments(operation, scenario.State);
        }
        else
        {
            scenario.Transfer.ApplyStore(operation, scenario.State);
        }

        scenario.AssertUnknown();
    }

    [TestMethod]
    public void Delegate_Addition_Uses_Snapshot_Before_Right_Hand_Evaluation()
    {
        var scenario = new Scenario("Action value = A; value += (value = B);");
        var addition = scenario.Operation("value += (value = B)");
        var before = scenario.Transfer.CaptureDelegateInputs(addition, scenario.State);
        Assert.IsNotNull(before);
        scenario.Store("value = B");
        scenario.Transfer.ApplyStore(addition, scenario.State, before);
        scenario.AssertValues("A|value = B");
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void Delegate_Addition_Without_Snapshot_Invalidates(bool initialize)
    {
        var scenario = new Scenario("Action value = A; value += B; value = A;", initialize);
        scenario.Transfer.ApplyStore(scenario.Operation("value += B"), scenario.State);
        scenario.AssertUnknown();
        scenario.Store("value = A");
        scenario.AssertValues("A");
    }

    [TestMethod]
    public void Delegate_Addition_Preserves_Incoming_And_Unknown()
    {
        var scenario = new Scenario("Action value = A; value -= A; value += B;", initialize: false);
        var addition = scenario.Operation("value += B");
        scenario.Transfer.ApplyStore(addition, scenario.State, scenario.Transfer.CaptureDelegateInputs(addition, scenario.State));
        scenario.AssertValues("B", incoming: true);
        scenario.Store("value -= A");
        scenario.Transfer.ApplyStore(addition, scenario.State, scenario.Transfer.CaptureDelegateInputs(addition, scenario.State));
        scenario.AssertUnknown();
    }

    [TestMethod]
    public void Coalescing_Store_Transfers_Only_When_Caller_Selects_Store_Path()
    {
        var scenario = new Scenario("string value = null; value ??= \"new\";");
        var skipped = new HashSet<TextSpan>(scenario.State);
        scenario.Store("value ??= \"new\"");
        scenario.AssertValues("\"new\"");
        scenario.State.UnionWith(skipped);
        scenario.AssertValues("null|\"new\"");
    }

    [TestMethod]
    public void Lowered_Coalescing_Store_Uses_Registered_Value()
    {
        var scenario = new Scenario("string value = null; value ??= \"new\";");
        var graph = ControlFlowGraph.Create((IMethodBodyOperation) scenario.Root, TestContext.CancellationToken);
        var store = graph.Blocks.SelectMany(static block => block.Operations)
            .SelectMany(static operation => operation.DescendantsAndSelf()).OfType<ISimpleAssignmentOperation>()
            .Single(static operation => operation.Syntax.ToString() == "value ??= \"new\"");
        scenario.Transfer.ApplyStore(store, scenario.State);
        scenario.AssertValues("\"new\"");
    }

    [TestMethod]
    [DataRow("if (item is string value) { }")]
    [DataRow("if (item is var value) { }")]
    [DataRow("if (item is Holder { Field: var value }) { }")]
    [DataRow("if (item is Holder value) { }")]
    [DataRow("if (item is Holder { } value) { }")]
    [DataRow("if (item is int[] array && array is [var value]) { }")]
    [DataRow("if (item is int[] array && array is [..] value) { }")]
    public void Pattern_Stores_Clear_Incoming_Until_Overwritten(string body)
    {
        var scenario = new Scenario(body + "value = default;", initialize: false);
        foreach (var operation in scenario.Root.DescendantsAndSelf().OfType<IPatternOperation>())
        {
            scenario.Transfer.ApplyStore(operation, scenario.State);
        }

        scenario.AssertUnknown();
        scenario.Store("value = default");
        scenario.AssertValues("default");
    }

    [TestMethod]
    [DataRow("foreach (string value in new[] { \"item\" }) { Use(value); }")]
    [DataRow("foreach (var value in (System.Collections.Generic.IEnumerable<string>)new[] { \"item\" }) { Use(value); }")]
    [DataRow("foreach (char value in \"text\") { Use(value); }")]
    [DataRow("foreach (var (value, other) in new[] { (\"item\", 0) }) { Use(value); }")]
    [DataRow("foreach (var (other, (value, last)) in new[] { (0, (\"item\", 1)) }) { Use(value); }")]
    [DataRow("foreach (ref string value in new Span<string>(new[] { \"item\" })) { Use(value); }")]
    public void Lowered_Foreach_Stores_Clear_Incoming(string body)
    {
        var scenario = new Scenario(body, initialize: false);
        var graph = ControlFlowGraph.Create((IMethodBodyOperation) scenario.Root, TestContext.CancellationToken);
        var stores = graph.Blocks.SelectMany(static block => block.Operations)
            .SelectMany(static operation => operation.DescendantsAndSelf())
            .Where(static operation => operation is IAssignmentOperation or IVariableDeclaratorOperation).ToArray();
        foreach (var store in stores)
        {
            scenario.Transfer.ApplyStore(store, scenario.State);
        }

        scenario.AssertUnknown();
    }

    [TestMethod]
    public void Ref_Arguments_Invalidate_Only_After_All_Arguments_Evaluate()
    {
        var scenario = new Scenario("int value = 1; ChangeWithArgument(ref value, value = 2);");
        scenario.AssertValues("1");
        scenario.Store("value = 2");
        scenario.AssertValues("2");
        scenario.Transfer.InvalidateByReferenceArguments(scenario.Operation("ChangeWithArgument(ref value, value = 2)"), scenario.State);
        scenario.AssertUnknown();
    }

    [TestMethod]
    public void Unregistered_Store_Clears_Known_And_Incoming_Values()
    {
        var scenario = new Scenario("int value = 1; Use(value); Use(2);", initialize: false);
        var unregistered = scenario.Operation("2");
        scenario.Transfer.ApplyTargetStore(scenario.Operation("value"), unregistered, scenario.State);
        scenario.AssertUnknown();
        scenario.Transfer.ApplyStore(scenario.Root.DescendantsAndSelf().OfType<IVariableDeclaratorOperation>().Single(), scenario.State);
        scenario.AssertValues("1");
        scenario.Transfer.ApplyTargetStore(scenario.Operation("value"), unregistered, scenario.State);
        scenario.AssertUnknown();
    }

    private sealed class Scenario
    {
        private static readonly MetadataReference[] References = [.. ((string) AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!)
            .Split(Path.PathSeparator).Select(static path => MetadataReference.CreateFromFile(path))];

        internal Scenario(string body, bool initialize = true, string? constraints = null)
        {
            var source = """
                using System;
                [System.Runtime.CompilerServices.InlineArray(1)]
                struct Buffer
                {
                    private int _element0;
                }
                interface IMutable
                {
                    event Action Changed;
                    int Property { get; set; }
                    int this[int index] { get; set; }
                }
                class MutableBase
                {
                    public int Property { get; set; }
                }
                class Holder
                {
                    public event Action Changed { add { } remove { } }
                    public int Field;
                    public Mutable Nested;
                    public ref int this[int index] => ref Field;
                    public void Deconstruct(out string first, out int second) { first = null; second = 0; }
                }
                struct Inner
                {
                    public event Action Changed { add { } remove { } }
                    public int Field;
                    public int Property { get; set; }
                }
                struct Mutable
                {
                    public event Action Changed { add { } remove { } }
                    public int Field;
                    public int Property { get; set; }
                    public Inner Nested;
                    public Holder Reference;
                    public int this[int index] { get => 0; set { } }
                }
                ref struct RefValue
                {
                    public ref int Field;
                    public RefValue(ref int field) { Field = ref field; }
                }
                class RefHolder
                {
                    public RefHolder(ref int value) { }
                    public RefHolder(object argument, ref int value) { }
                }
                class Example
                {
                    static void A() { }
                    static void B() { }
                    static void Use<T>(T item) { }
                    static void Change<T>(ref T item) { }
                    static void Assign<T>(out T item) => item = default;
                    static void Read<T>(in T item) { }
                    static void ChangeWithArgument(ref int value, int argument) { }
                    static void Run(bool flag, object item)
                    {
                """ + body + "}}";
            if (constraints is not null)
            {
                source = source.Replace("Run(bool flag, object item)", "Run<T>(bool flag, object item) where T : " + constraints);
            }
            var tree = CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(LanguageVersion.Preview));
            var compilation = CSharpCompilation.Create("TransferTest", [tree], References,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            var errors = compilation.GetDiagnostics().Where(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error).ToArray();
            Assert.IsEmpty(errors, string.Join(Environment.NewLine, errors.Select(static error => error.ToString())));
            var model = compilation.GetSemanticModel(tree);
            var method = tree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().Single(static method => method.Identifier.ValueText == "Run");
            Root = model.GetOperation(method)!;
            var local = method.DescendantNodes().Select(node => model.GetDeclaredSymbol(node)).OfType<ILocalSymbol>()
                .First(static symbol => symbol.Name == "value");
            Transfer = new LocalDefinitionTransfer(Root, local);
            if (initialize && Root.DescendantsAndSelf().OfType<IVariableDeclaratorOperation>()
                    .FirstOrDefault(declarator => SymbolEqualityComparer.Default.Equals(declarator.Symbol, local)) is { } declaration)
            {
                Transfer.ApplyStore(declaration, State);
            }
        }

        internal IOperation Root { get; }
        internal LocalDefinitionTransfer Transfer { get; }
        internal HashSet<TextSpan> State { get; } = LocalDefinitionTransfer.CreateIncomingState();

        internal IOperation Operation(string syntax) => Root.DescendantsAndSelf()
            .First(operation => operation.Syntax.ToString() == syntax && operation is not (IExpressionStatementOperation or IArgumentOperation));

        internal void Store(string syntax)
        {
            var operation = Operation(syntax);
            Transfer.ApplyStore(operation, State, Transfer.CaptureDelegateInputs(operation, State));
        }

        internal void AssertUnknown()
        {
            Assert.IsFalse(Transfer.TryGetValues(State, out var values, out var incoming));
            Assert.IsEmpty(values);
            Assert.IsFalse(incoming);
        }

        internal void AssertValues(string expected, bool incoming = false)
        {
            Assert.IsTrue(Transfer.TryGetValues(State, out var values, out var inherits));
            Assert.AreEqual(incoming, inherits);
            Assert.AreSequenceEqual([.. expected.Split('|', StringSplitOptions.RemoveEmptyEntries).Order(StringComparer.Ordinal)],
                [.. values.Select(static operation => operation.Syntax.ToString()).Order(StringComparer.Ordinal)]);
        }
    }

    public TestContext TestContext { get; set; } = null!;
}
