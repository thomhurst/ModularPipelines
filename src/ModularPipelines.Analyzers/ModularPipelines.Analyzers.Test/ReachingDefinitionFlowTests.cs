using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ReachingDefinitionFlowTests
{
    [TestMethod]
    [DataRow("string value = \"old\"; value = \"new\"; [|Use(value)|];", "\"new\"")]
    [DataRow("string value = \"old\"; if (flag) value = \"new\"; [|Use(value)|];", "\"new\"|\"old\"")]
    [DataRow("string value = \"old\"; while (flag) { value = \"loop\"; } [|Use(value)|];", "\"loop\"|\"old\"")]
    [DataRow("string value = \"old\"; do { value = \"loop\"; } while (flag); [|Use(value)|];", "\"loop\"")]
    [DataRow("string value = \"old\"; goto end; value = \"dead\"; end: [|Use(value)|];", "\"old\"")]
    [DataRow("string value = \"old\"; while (flag) { value = \"loop\"; break; value = \"dead\"; } [|Use(value)|];", "\"loop\"|\"old\"")]
    [DataRow("string value = \"old\"; while (flag) { value = \"loop\"; continue; value = \"dead\"; } [|Use(value)|];", "\"loop\"|\"old\"")]
    [DataRow("string value = \"old\"; if (false) value = \"dead\"; [|Use(value)|];", "\"old\"")]
    [DataRow("string value = \"old\"; string other = \"other\"; (value, other) = (\"tuple\", value); [|Use(value)|];", "\"tuple\"")]
    [DataRow("string value = \"old\"; string other = value; value = other; [|Use(value)|];", "other")]
    [DataRow("string value = \"old\"; Action nested = () => value = \"nested\"; nested(); [|Use(value)|];", "\"old\"")]
    [DataRow("string value = \"old\"; switch (flag) { case true: value = \"switch\"; break; default: value = \"default\"; break; } [|Use(value)|];", "\"default\"|\"switch\"")]
    [DataRow("string value = \"old\"; (value, _) = ((\"tuple\", \"ignored\")); [|Use(value)|];", "\"tuple\"")]
    [DataRow("string value = \"old\"; void Nested() { value = \"nested\"; } Nested(); [|Use(value)|];", "\"old\"")]
    public void Local_Definitions_Follow_Control_Flow(string body, string expected) =>
        AssertFlow(body, expected);

    [TestMethod]
    [DataRow("string value = \"old\"; string other = \"other\"; var pair = (\"new\", \"next\"); (value, other) = pair; [|Use(value)|];")]
    [DataRow("string value = \"old\"; var pair = (\"new\", \"next\"); (value, string other) = pair; [|Use(value)|];")]
    [DataRow("string value = \"old\"; var pair = (\"new\", \"next\"); (_, value) = pair; [|Use(value)|];")]
    [DataRow("string value = \"old\"; var pair = (\"new\", \"next\"); ((value, _), _) = (pair, 0); [|Use(value)|];")]
    [DataRow("string value = \"old\"; var pair = (\"new\", \"next\"); (value, (value, _)) = (\"known\", pair); [|Use(value)|];")]
    [DataRow("string value = \"old\"; var pair = (\"new\", \"next\"); if (flag) (value, _) = pair; [|Use(value)|];")]
    [DataRow("string value = \"old\"; var pair = ((string, string))item; (value, _) = pair; [|Use(value)|];")]
    public void Unresolved_Tuple_Element_Definitions_Are_Unsupported(string body) =>
        AssertFlow(body, "", supported: false);

    [TestMethod]
    [DataRow("string value = \"old\"; var pair = (\"new\", \"next\"); (value, _) = pair; value = \"known\"; [|Use(value)|];", "\"known\"")]
    [DataRow("string value = \"old\"; var pair = (\"new\", \"next\"); ((value, _), value) = (pair, \"known\"); [|Use(value)|];", "\"known\"")]
    [DataRow("string value = \"old\"; var pair = (\"new\", \"next\"); if (false) (value, _) = pair; [|Use(value)|];", "\"old\"")]
    [DataRow("string value = \"old\"; var pair = (\"new\", \"next\"); [|Use(value)|]; (value, _) = pair;", "\"old\"")]
    [DataRow("string value = \"old\"; var pair = (\"new\", \"next\"); Action nested = () => (value, _) = pair; [|Use(value)|];", "\"old\"")]
    [DataRow("string value = \"old\"; string other = \"other\"; var pair = (\"new\", \"next\"); (other, _) = pair; [|Use(value)|];", "\"old\"")]
    public void Unresolved_Tuple_Writes_Only_Affect_Reaching_Paths(string body, string expected) =>
        AssertFlow(body, expected);

    [TestMethod]
    [DataRow("int value = 1; value++; [|Use(value)|];")]
    [DataRow("int value = 1; ++value; [|Use(value)|];")]
    [DataRow("int value = 1; value--; [|Use(value)|];")]
    [DataRow("int value = 1; --value; [|Use(value)|];")]
    [DataRow("int value = 1; value += 2; [|Use(value)|];")]
    [DataRow("int value = 1; value -= 2; [|Use(value)|];")]
    [DataRow("int value = 1; value *= 2; [|Use(value)|];")]
    [DataRow("int value = 1; value /= 2; [|Use(value)|];")]
    [DataRow("int value = 1; value %= 2; [|Use(value)|];")]
    [DataRow("int value = 1; value &= 2; [|Use(value)|];")]
    [DataRow("int value = 1; value |= 2; [|Use(value)|];")]
    [DataRow("int value = 1; value ^= 2; [|Use(value)|];")]
    [DataRow("int value = 1; value <<= 2; [|Use(value)|];")]
    [DataRow("int value = 1; value >>= 2; [|Use(value)|];")]
    [DataRow("int value = 1; value >>>= 2; [|Use(value)|];")]
    [DataRow("string value = \"old\"; value += \"new\"; [|Use(value)|];")]
    [DataRow("Action value = A; value -= A; [|Use(value)|];")]
    [DataRow("dynamic value = 1; value += 2; [|Use(value)|];")]
    [DataRow("int value = 1; if (flag) value++; [|Use(value)|];")]
    [DataRow("int value = 1; while (flag) value--; [|Use(value)|];")]
    [DataRow("Action value = A; value -= A; value += B; [|Use(value)|];")]
    public void Unsupported_Local_Mutations_Invalidate_Definitions(string body) =>
        AssertFlow(body, "", supported: false);

    [TestMethod]
    [DataRow("int value = 1; value++; value = 2; [|Use(value)|];", "2")]
    [DataRow("int value = 1; value *= 2; value = 3; [|Use(value)|];", "3")]
    [DataRow("Action value = A; value -= A; value = B; [|Use(value)|];", "B")]
    [DataRow("int value = 1; if (false) value++; [|Use(value)|];", "1")]
    [DataRow("int value = 1; [|Use(value)|]; value++;", "1")]
    [DataRow("int value = 1; Action nested = () => value++; [|Use(value)|];", "1")]
    [DataRow("int value = 1; void Nested() { value *= 2; } [|Use(value)|];", "1")]
    [DataRow("int value = 1; int other = 2; other++; [|Use(value)|];", "1")]
    [DataRow("int value = 1; int other = 2; other *= 2; [|Use(value)|];", "1")]
    [DataRow("int value = 1; try { value /= 0; } catch (DivideByZeroException) { [|Use(value)|]; }", "1")]
    [DataRow("int value = int.MaxValue; try { checked { value++; } } catch (OverflowException) { [|Use(value)|]; }", "int.MaxValue")]
    [DataRow("int value = 1; try { value *= (value = 2) / 0; } catch (DivideByZeroException) { [|Use(value)|]; }", "2")]
    public void Unsupported_Local_Mutations_Only_Affect_Reaching_Paths(string body, string expected) =>
        AssertFlow(body, expected);

    [TestMethod]
    [DataRow("int value = 1; Change(ref value); [|Use(value)|];")]
    [DataRow("int value = 1; Assign(out value); [|Use(value)|];")]
    [DataRow("int value = 1; try { Change(ref value); } catch { [|Use(value)|]; }")]
    [DataRow("int value = 1; try { Assign(out value); } catch { [|Use(value)|]; }")]
    [DataRow("int value = 1; ref int alias = ref value; alias = 2; [|Use(value)|];")]
    [DataRow("int value = 1; ref int alias = ref value; alias++; [|Use(value)|];")]
    [DataRow("int value = 1; ref int alias = ref value; alias *= 2; [|Use(value)|];")]
    [DataRow("int value = 1; ref int alias = ref value; Change(ref alias); [|Use(value)|];")]
    [DataRow("int value = 1; ref int alias = ref value; ref int second = ref alias; second = 2; [|Use(value)|];")]
    [DataRow("int value = 1; int other = 0; ref int alias = ref (flag ? ref value : ref other); alias = 2; [|Use(value)|];")]
    [DataRow("int value = 1; int other = 0; ref int alias = ref other; alias = ref value; alias = 2; [|Use(value)|];")]
    [DataRow("int value = 1; ref int alias = ref value; value = 2; alias = 3; [|Use(value)|];")]
    [DataRow("int value = 1; ref int alias = ref value; (alias, _) = (2, 0); [|Use(value)|];")]
    [DataRow("int value = 1; dynamic receiver = item; receiver.Change(ref value); [|Use(value)|];")]
    [DataRow("int value = 1; dynamic receiver = item; receiver.Assign(out value); [|Use(value)|];")]
    [DataRow("int value = 1; new RefHolder(0, ref value); [|Use(value)|];")]
    [DataRow("int value = 1; new RefHolder((dynamic)item, ref value); [|Use(value)|];")]
    [DataRow("int value = 1; int other = 0; Change(ref (flag ? ref value : ref other)); [|Use(value)|];")]
    [DataRow("int value = 1; int other = 0; dynamic receiver = item; receiver.Change(ref (flag ? ref value : ref other)); [|Use(value)|];")]
    [DataRow("int value = 1; int other = 0; (flag ? ref value : ref other) = 2; [|Use(value)|];")]
    public void ByReference_Mutations_Invalidate_Definitions(string body) =>
        AssertFlow(body, "", supported: false);

    [TestMethod]
    [DataRow("int value = 1; Change(ref value); value = 2; [|Use(value)|];", "2")]
    [DataRow("int value = 1; ref int alias = ref value; alias = 2; value = 3; [|Use(value)|];", "3")]
    [DataRow("int value = 1; if (false) Change(ref value); [|Use(value)|];", "1")]
    [DataRow("int value = 1; [|Use(value)|]; Change(ref value);", "1")]
    [DataRow("int value = 1; Action nested = () => Change(ref value); [|Use(value)|];", "1")]
    [DataRow("int value = 1; int other = 0; Change(ref other); [|Use(value)|];", "1")]
    [DataRow("int value = 1; ref int alias = ref value; [|Use(value)|];", "1")]
    [DataRow("int value = 1; int other = 0; ref int alias = ref other; alias = 2; [|Use(value)|];", "1")]
    [DataRow("int value = 1; ref int alias = ref value; if (false) alias = 2; [|Use(value)|];", "1")]
    [DataRow("int value = 1; ref int alias = ref value; [|Use(value)|]; alias = 2;", "1")]
    [DataRow("int value = 1; ChangeWithArgument(ref value, [|value|]);", "1")]
    public void ByReference_Mutations_Respect_Paths_And_Evaluation_Order(string body, string expected) =>
        AssertFlow(body, expected);

    [TestMethod]
    [DataRow("int source = 0; ref int value = ref source; value = 1; source = 2; [|Use(value)|];")]
    [DataRow("int source = 0; ref int sibling = ref source; ref int value = ref source; value = 1; sibling = 2; [|Use(value)|];")]
    [DataRow("int source = 0; ref int value = ref source; ref int sibling = ref source; value = 1; sibling++; [|Use(value)|];")]
    [DataRow("int source = 0; ref int middle = ref source; ref int value = ref middle; value = 1; source = 2; [|Use(value)|];")]
    [DataRow("int source = 0; ref int value = ref source; value = 1; Change(ref source); [|Use(value)|];")]
    [DataRow("int source = 0; ref int value = ref source; value = 1; (source, _) = (2, 0); [|Use(value)|];")]
    [DataRow("int source = 0; int other = 0; ref int value = ref (flag ? ref source : ref other); value = 1; other = 2; [|Use(value)|];")]
    [DataRow("int source = 0; int other = 0; ref int value = ref other; value = ref source; value = 1; source = 2; [|Use(value)|];")]
    [DataRow("int source = 0; ref int value = ref source; value = 1; try { Change(ref source); } catch { [|Use(value)|]; }")]
    public void Ref_Local_Source_And_Sibling_Writes_Invalidate_Definitions(string body) =>
        AssertFlow(body, "", supported: false);

    [TestMethod]
    [DataRow("int source = 0; ref int value = ref source; value = 1; source = 2; value = 3; [|Use(value)|];", "3")]
    [DataRow("int source = 0; ref int value = ref source; value = 1; if (false) source = 2; [|Use(value)|];", "1")]
    [DataRow("int source = 0; ref int value = ref source; value = 1; [|Use(value)|]; source = 2;", "1")]
    [DataRow("int source = 0; int other = 0; ref int value = ref source; value = 1; other = 2; [|Use(value)|];", "1")]
    [DataRow("int source = 0; ref int value = ref source; value = 1; ChangeWithArgument(ref source, [|value|]);", "1")]
    public void Ref_Local_Source_Writes_Respect_Paths_And_Evaluation_Order(string body, string expected) =>
        AssertFlow(body, expected);

    [TestMethod]
    [DataRow("string value = null; value ??= \"new\"; [|Use(value)|];", "\"new\"|null")]
    [DataRow("string value = \"old\"; value ??= \"new\"; [|Use(value)|];", "\"new\"|\"old\"")]
    [DataRow("string value = null; if (flag) value ??= \"new\"; [|Use(value)|];", "\"new\"|null")]
    [DataRow("string value = null; value ??= \"new\"; value = \"known\"; [|Use(value)|];", "\"known\"")]
    [DataRow("string value = null; [|Use(value)|]; value ??= \"new\";", "null")]
    [DataRow("Action value = null; value ??= A; [|Use(value)|];", "A|null")]
    public void Coalescing_Assignments_Transfer_Their_Conditional_Stores(string body, string expected) =>
        AssertFlow(body, expected);

    [TestMethod]
    public void Delegate_Addition_Preserves_Previous_Definitions() => AssertFlow(
        "Action value = A; value += B; [|Use(value)|];", "A|B");

    [TestMethod]
    [DataRow("Action nested = () => { [|Use(value)|]; };", "")]
    [DataRow("void Nested() { [|Use(value)|]; }", "")]
    [DataRow("Action nested = () => { if (flag) value = \"inner\"; [|Use(value)|]; };", "\"inner\"")]
    public void Nested_Callable_Inherits_Incoming_Value(string callable, string expected) => AssertFlow(
        "string value = \"outer\"; " + callable, expected, incoming: true);

    [TestMethod]
    public void Unreachable_Query_Has_No_Definitions() => AssertFlow(
        "string value = \"old\"; return; [|Use(value)|];", "");

    [TestMethod]
    [DataRow("try { value = \"try\"; Unknown(); value = \"after\"; } catch { [|Use(value)|]; }", "\"try\"")]
    [DataRow("try { throw null; value = \"dead\"; } catch { [|Use(value)|]; }", "\"old\"")]
    [DataRow("try { Unknown(); } finally { value = \"finally\"; } [|Use(value)|];", "\"finally\"")]
    [DataRow("try { Unknown(); } catch when ((value = \"filter\") != null) { [|Use(value)|]; }", "\"filter\"")]
    [DataRow("try { Unknown(); } catch when ((value = \"filter\") == null) { } catch { [|Use(value)|]; }", "\"filter\"")]
    [DataRow("try { Unknown(); } catch when (ThrowFilter(value = \"filter\")) { } catch { [|Use(value)|]; }", "\"filter\"")]
    [DataRow("try { try { Unknown(); } finally { value = \"cleanup\"; } } catch when (flag) { [|Use(value)|]; }", "\"cleanup\"")]
    [DataRow("try { Unknown(); } catch when (Observe(value = \"filter\")) { } finally { value = \"cleanup\"; } [|Use(value)|];", "\"cleanup\"")]
    [DataRow("try { try { Unknown(); } finally { value = \"cleanup\"; } } catch when (Observe(value = \"filter\")) { } catch { [|Use(value)|]; }", "\"cleanup\"")]
    [DataRow("try { try { Unknown(); } catch { value = \"inner\"; throw; } } catch { [|Use(value)|]; }", "\"inner\"")]
    [DataRow("try { try { Unknown(); } finally { value = \"cleanup\"; } } catch when ([|Observe(value)|]) { }", "\"old\"")]
    public void Exceptions_Filters_And_Finally_Keep_Definitions(string statements, string expected) =>
        AssertFlow("string value = \"old\"; " + statements, expected);

    [TestMethod]
    [DataRow("var text = \"prefix\" + item;", "\"old\"")]
    [DataRow("var text = item + \"suffix\";", "\"old\"")]
    [DataRow("var text = \"prefix\" + source;", "\"old\"")]
    [DataRow("var text = \"prefix\" + structure;", "\"old\"")]
    [DataRow("var text = \"prefix\"; text += item;", "\"old\"")]
    [DataRow("object text = item; text += \"suffix\";", "\"old\"")]
    [DataRow("value += item;", "\"old\"")]
    [DataRow("value = \"prefix\" + item;", "\"old\"")]
    [DataRow("var text = (value = \"rhs\") + item;", "\"rhs\"")]
    [DataRow("var text = item + (value = \"rhs\");", "\"rhs\"")]
    [DataRow("var text = \"prefix\" + value;", "")]
    [DataRow("var text = \"prefix\"; text += value;", "")]
    [DataRow("var text = \"prefix\" + (object)null;", "")]
    public void String_Concatenation_Exceptions_Reach_Handlers_After_Operand_Evaluation(string statement, string expected) => AssertFlow(
        "string value = \"old\"; TextSource source = new TextSource(); TextStruct structure = default; try { "
        + statement + " } catch { [|Use(value)|]; }", expected);

    [TestMethod]
    public void String_Concatenation_Exceptions_Reach_Filters_Before_Finally() => AssertFlow("""
        string value = "old";
        try { try { var text = "prefix" + item; } finally { value = "cleanup"; } }
        catch when ([|Observe(value)|]) { }
        """, "\"old\"");

    [TestMethod]
    [DataRow("(value, _) = holder;", "\"old\"")]
    [DataRow("(value, string other) = holder;", "\"old\"")]
    [DataRow("((value, _), _) = (holder, 0);", "\"old\"")]
    [DataRow("(value, (_, _)) = (value = \"rhs\", holder);", "\"rhs\"")]
    [DataRow("(value, _) = (\"tuple\", 0);", "")]
    public void Deconstruct_Exceptions_Precede_Definition_Writes(string statement, string expected) => AssertFlow(
        "string value = \"old\"; Holder holder = null; try { " + statement + " } catch { [|Use(value)|]; }", expected);

    [TestMethod]
    [DataRow("if (holder is (var first, var second)) { }", "\"old\"")]
    [DataRow("if (holder is Holder(var first, var second)) { }", "\"old\"")]
    [DataRow("var match = holder switch { (var first, var second) => true, _ => false };", "\"old\"")]
    [DataRow("var pair = (\"one\", \"two\"); if (pair is (var first, var second)) { }", "")]
    public void Positional_Pattern_Deconstruct_Exceptions_Reach_Handlers(string statement, string expected) => AssertFlow(
        "string value = \"old\"; Holder holder = new Holder(); try { " + statement + " } catch { [|Use(value)|]; }", expected);

    [TestMethod]
    public void Positional_Pattern_Deconstruct_Exceptions_Reach_Filters_Before_Finally() => AssertFlow("""
        string value = "old";
        Holder holder = new Holder();
        try { try { if (holder is (var first, var second)) { } } finally { value = "cleanup"; } }
        catch when ([|Observe(value)|]) { }
        """, "\"old\"");

    [TestMethod]
    [DataRow("ListHolder items = new ListHolder();", "if (items is []) { }", "\"old\"")]
    [DataRow("ListHolder items = new ListHolder();", "if (items is [var first]) { }", "\"old\"")]
    [DataRow("ListHolder items = new ListHolder();", "var match = items switch { [var first] => true, _ => false };", "\"old\"")]
    [DataRow("ListHolder items = new ListHolder();", "if (items is [.. var rest]) { }", "\"old\"")]
    [DataRow("string[] items = null;", "if (items is [var first]) { }", "")]
    [DataRow("string[] items = null;", "if (items is []) { }", "")]
    [DataRow("string items = null;", "if (items is [var first]) { }", "")]
    [DataRow("string items = null;", "if (items is []) { }", "")]
    public void List_Pattern_Member_Exceptions_Reach_Handlers(string declaration, string statement, string expected) => AssertFlow(
        "string value = \"old\"; " + declaration + " try { " + statement + " } catch { [|Use(value)|]; }", expected);

    [TestMethod]
    public void List_Pattern_Member_Exceptions_Reach_Filters_Before_Finally() => AssertFlow("""
        string value = "old";
        ListHolder items = new ListHolder();
        try { try { if (items is [var first]) { } } finally { value = "cleanup"; } }
        catch when ([|Observe(value)|]) { }
        """, "\"old\"");

    [TestMethod]
    [DataRow("(value, _) = (source, 0);", "\"old\"")]
    [DataRow("(value, string declared) = (source, \"other\");", "\"old\"")]
    [DataRow("((value, _), _) = ((source, 0), 0);", "\"old\"")]
    [DataRow("(value, other) = (\"new\", source);", "\"old\"")]
    [DataRow("(value, other) = (value = \"rhs\", source);", "\"rhs\"")]
    [DataRow("(value, _) = pair;", "\"old\"")]
    [DataRow("((value, _), _) = (pair, 0);", "\"old\"")]
    [DataRow("var second = (\"new\", source); (value, other) = second;", "\"old\"")]
    [DataRow("(value, _) = (\"plain\", 0);", "")]
    public void Deconstruction_Conversion_Exceptions_Precede_All_Stores(string statement, string expected) => AssertFlow(
        "string value = \"old\"; string other = null; ConversionSource source = null; var pair = (source, 0); try { "
        + statement + " } catch { [|Use(value)|]; }", expected);

    [TestMethod]
    public void Deconstruction_Conversion_Exceptions_Reach_Filters_Before_Finally() => AssertFlow("""
        string value = "old";
        ConversionSource source = null;
        var pair = (source, 0);
        try { try { (value, _) = pair; } finally { value = "cleanup"; } }
        catch when ([|Observe(value)|]) { }
        """, "\"old\"");

    [TestMethod]
    public void Deconstruction_Dynamic_Conversion_Can_Throw() => AssertFlow("""
        string value = "old";
        dynamic source = item;
        try { (value, _) = (source, 0); }
        catch { [|Use(value)|]; }
        """, "\"old\"");

    [TestMethod]
    [DataRow("(value, holder.Property) = (\"new\", \"x\");", "\"new\"")]
    [DataRow("(holder.Property, value) = (\"x\", \"new\");", "\"old\"")]
    [DataRow("(value, holder.Property, value) = (\"first\", \"x\", \"last\");", "\"first\"")]
    [DataRow("((value, holder.Property), value) = ((\"first\", \"x\"), \"last\");", "\"first\"")]
    [DataRow("(value, holder.Field) = (\"new\", \"x\");", "")]
    [DataRow("(value, missing.Field) = (\"new\", \"x\");", "\"new\"")]
    [DataRow("(value, array[2]) = (\"new\", \"x\");", "\"new\"")]
    [DataRow("(array[2], value) = (\"x\", \"new\");", "\"old\"")]
    [DataRow("(value, holder.RefProperty) = (\"new\", \"x\");", "\"old\"")]
    [DataRow("(value, holder.Property) = (\"new\", value = \"rhs\");", "\"new\"")]
    [DataRow("(holder.Property, value) = (\"x\", value = \"rhs\");", "\"rhs\"")]
    public void Deconstruction_Stores_Follow_Assignment_Order(string statement, string expected) => AssertFlow(
        "string value = \"old\"; Holder holder = new Holder(); Holder missing = null; string[] array = new string[1]; try { "
        + statement + " } catch { [|Use(value)|]; }", expected);

    [TestMethod]
    [DataRow("item", "\"new\"")]
    [DataRow("null", "")]
    public void Deconstruction_Array_Stores_Check_Covariance(string assigned, string expected) => AssertFlow($$"""
        string value = "old";
        object[] array = new string[1];
        try { (value, array[0]) = ("new", {{assigned}}); }
        catch (ArrayTypeMismatchException) { [|Use(value)|]; }
        """, expected);

    [TestMethod]
    public void Deconstruction_Setter_Exception_Reaches_Filter_Before_Finally() => AssertFlow("""
        string value = "old";
        Holder holder = new Holder();
        try { try { (value, holder.Property) = ("new", "x"); } finally { value = "cleanup"; } }
        catch when ([|Observe(value)|]) { }
        """, "\"new\"");

    [TestMethod]
    public void Deconstruct_Exceptions_Reach_Filters_Before_Finally() => AssertFlow("""
        string value = "old";
        Holder holder = null;
        try { try { (value, _) = holder; } finally { value = "cleanup"; } }
        catch when ([|Observe(value)|]) { }
        """, "\"old\"");

    [TestMethod]
    [DataRow("DivideByZeroException", "\"rethrow\"")]
    [DataRow("InvalidOperationException", "")]
    public void Rethrow_Preserves_Active_Exception_Type(string caught, string expected) => AssertFlow($$"""
        string value = "old";
        try
        {
            value = "before";
            try { int zero = 0; var number = 1 / zero; }
            catch (Exception) { value = "rethrow"; throw; }
        }
        catch ({{caught}}) { [|Use(value)|]; }
        """, expected);

    [TestMethod]
    public void Nested_Handler_Does_Not_Replace_Outer_Rethrow_Type() => AssertFlow("""
        string value = "old";
        try
        {
            value = "before";
            try { int zero = 0; var number = 1 / zero; }
            catch (Exception)
            {
                try { int zero = 0; var number = 1 / zero; } catch (DivideByZeroException) { }
                value = "rethrow";
                throw;
            }
        }
        catch (DivideByZeroException) { [|Use(value)|]; }
        """, "\"rethrow\"");

    [TestMethod]
    [DataRow("try { [|return value;|] } finally { value = \"cleanup\"; }", "\"cleanup\"")]
    [DataRow("try { [|return value;|] } finally { throw null; }", "")]
    [DataRow("try { try { [|return value;|] } finally { throw null; } } catch { return \"replacement\"; }", "")]
    [DataRow("try { [|return value;|] } finally { try { throw null; } catch { value = \"cleanup\"; } }", "\"cleanup\"")]
    [DataRow("try { [|return value;|] } finally { if (flag) throw null; value = \"cleanup\"; }", "\"cleanup\"")]
    [DataRow("try { try { [|return value;|] } finally { value = \"inner\"; } } finally { value = \"outer\"; }", "\"outer\"")]
    public void Selected_Return_Must_Complete(string statements, string expected) =>
        AssertFlow("string value = \"old\"; " + statements, expected, returns: true);

    [TestMethod]
    [DataRow("holder.Property = (value = \"rhs\");", "\"rhs\"")]
    [DataRow("value = holder.Property;", "\"old\"")]
    [DataRow("holder.Field = (value = \"rhs\");", "\"rhs\"")]
    [DataRow("holder.Number += (value = \"rhs\").Length;", "\"old\"|\"rhs\"")]
    [DataRow("((dynamic)holder).Property = (value = \"rhs\");", "\"rhs\"")]
    [DataRow("((dynamic)holder)[0] = (value = \"rhs\");", "\"rhs\"")]
    [DataRow("var converted = (int)(object)(value = \"rhs\");", "\"rhs\"")]
    [DataRow("var number = checked(int.MaxValue + (value = \"rhs\").Length);", "\"rhs\"")]
    [DataRow("var number = 1 / (value = \"rhs\").Length;", "\"rhs\"")]
    [DataRow("var created = new Holder(value = \"rhs\");", "\"rhs\"")]
    [DataRow("Holder.StaticField = (value = \"rhs\");", "\"rhs\"")]
    [DataRow("holder.RefProperty = (value = \"rhs\");", "\"old\"")]
    [DataRow("var number = (decimal)int.MaxValue + (value = \"rhs\").Length;", "\"rhs\"")]
    public void Exception_Timing_Respects_Evaluation_Order(string statement, string expected) => AssertFlow(
        "string value = \"old\"; Holder holder = null; try { " + statement + " } catch { [|Use(value)|]; }", expected);

    [TestMethod]
    [DataRow("decimal number = decimal.MaxValue;", "sbyte", "\"old\"")]
    [DataRow("decimal number = decimal.MaxValue;", "byte", "\"old\"")]
    [DataRow("decimal number = decimal.MaxValue;", "short", "\"old\"")]
    [DataRow("decimal number = decimal.MaxValue;", "ushort", "\"old\"")]
    [DataRow("decimal number = decimal.MaxValue;", "int", "\"old\"")]
    [DataRow("decimal number = decimal.MaxValue;", "uint", "\"old\"")]
    [DataRow("decimal number = decimal.MaxValue;", "long", "\"old\"")]
    [DataRow("decimal number = decimal.MaxValue;", "ulong", "\"old\"")]
    [DataRow("decimal number = decimal.MaxValue;", "char", "\"old\"")]
    [DataRow("decimal number = decimal.MaxValue;", "nint", "\"old\"")]
    [DataRow("decimal number = decimal.MaxValue;", "nuint", "\"old\"")]
    [DataRow("decimal number = decimal.MaxValue;", "DayOfWeek", "\"old\"")]
    [DataRow("double number = double.MaxValue;", "decimal", "\"old\"")]
    [DataRow("float number = float.MaxValue;", "decimal", "\"old\"")]
    [DataRow("decimal? number = decimal.MaxValue;", "int?", "\"old\"")]
    [DataRow("double? number = double.MaxValue;", "decimal?", "\"old\"")]
    [DataRow("decimal number = decimal.MaxValue;", "double", "")]
    [DataRow("decimal number = decimal.MaxValue;", "float", "")]
    [DataRow("long number = long.MaxValue;", "decimal", "")]
    [DataRow("ulong number = ulong.MaxValue;", "decimal", "")]
    [DataRow("double number = double.MaxValue;", "int", "")]
    public void Unchecked_Numeric_Conversion_Exceptions(string declaration, string target, string expected) => AssertFlow($$"""
        string value = "old";
        {{declaration}}
        try { var converted = unchecked(({{target}})number); }
        catch (OverflowException) { [|Use(value)|]; }
        """, expected);

    [TestMethod]
    public void Decimal_Conversion_Exception_Follows_Operand_Evaluation_Before_Finally() => AssertFlow("""
        string value = "old";
        decimal number = decimal.MaxValue;
        try
        {
            try { var converted = unchecked((int)(flag ? number : (value = "rhs").Length)); }
            finally { value = "cleanup"; }
        }
        catch when ([|Observe(value)|]) { }
        """, "\"old\"|\"rhs\"");

    [TestMethod]
    [DataRow("Holder holder = new Holder();", "", "")]
    [DataRow("Holder holder = new Holder();", "ref Holder alias = ref (flag ? ref holder : ref other);", "\"after\"")]
    [DataRow("Holder holder = new Holder();", "ref Holder alias = ref other; alias = ref (flag ? ref holder : ref other);", "\"after\"")]
    [DataRow("Holder holder = new Holder();", "Change(ref (flag ? ref holder : ref other));", "\"after\"")]
    public void Conditional_Ref_Aliases_Invalidate_NonNull_Proof(string declaration, string alias, string expected) => AssertFlow($$"""
        string value = "old";
        {{declaration}}
        Holder other = null;
        {{alias}}
        value = "after";
        try { var field = holder.Field; } catch (NullReferenceException) { [|Use(value)|]; }
        """, expected);

    [TestMethod]
    [DataRow("", "")]
    [DataRow("ref object[] alias = ref (flag ? ref array : ref other);", "\"after\"")]
    [DataRow("ref object[] alias = ref other; alias = ref (flag ? ref array : ref other);", "\"after\"")]
    [DataRow("Change(ref (flag ? ref array : ref other));", "\"after\"")]
    public void Conditional_Ref_Aliases_Invalidate_Exact_Array_Proof(string alias, string expected) => AssertFlow($$"""
        string value = "old";
        object[] array = new object[1];
        object[] other = new string[1];
        {{alias}}
        value = "after";
        try { array[0] = item; } catch (ArrayTypeMismatchException) { [|Use(value)|]; }
        """, expected);

    [TestMethod]
    [DataRow("object source = item;", "var text = $\"{source}\";", "\"old\"")]
    [DataRow("TextSource source = null;", "var text = $\"{source}\";", "\"old\"")]
    [DataRow("TextStruct source = default;", "var text = $\"{source}\";", "\"old\"")]
    [DataRow("FormattedSource source = null;", "var text = $\"{source:X}\";", "\"old\"")]
    [DataRow("object source = item;", "value = $\"{source}\";", "\"old\"")]
    [DataRow("object source = item;", "var text = $\"{(value = \"rhs\")}{source}\";", "\"rhs\"")]
    [DataRow("object source = item;", "var text = $\"{source}{(value = \"later\")}\";", "\"old\"")]
    [DataRow("string source = null;", "var text = $\"{source}\";", "")]
    [DataRow("", "var text = $\"{(string)null}\";", "")]
    [DataRow("", "var text = $\"literal\";", "")]
    [DataRow("object source = item;", "FormattableString text = $\"{source}\";", "")]
    [DataRow("object source = item;", "IFormattable text = $\"{source}\";", "")]
    [DataRow("", "var text = $\"{1:Q}\";", "\"old\"")]
    [DataRow("string source = null;", "var text = $\"{source,-2147483648}\";", "\"old\"")]
    [DataRow("", "var text = $\"{(object)null}\";", "")]
    public void Interpolation_Exceptions_Respect_Formatting_Timing(string declaration, string statement, string expected) => AssertFlow(
        "string value = \"old\"; " + declaration + " try { " + statement + " } catch { [|Use(value)|]; }", expected);

    [TestMethod]
    public void Interpolation_Exceptions_Reach_Filters_Before_Finally() => AssertFlow("""
        string value = "old";
        object source = item;
        try { try { var text = $"{source}"; } finally { value = "cleanup"; } }
        catch when ([|Observe(value)|]) { }
        """, "\"old\"");

    [TestMethod]
    [DataRow("object text = $\"{item}\";", "\"old\"")]
    [DataRow("object text = $\"{item}{(value = \"later\")}\";", "\"old\"")]
    [DataRow("IComparable text = $\"{item}\";", "\"old\"")]
    [DataRow("object text = $\"{(string)null}\";", "")]
    public void Interpolation_Converted_String_Still_Formats(string statement, string expected) => AssertFlow(
        "string value = \"old\"; try { " + statement + " } catch { [|Use(value)|]; }", expected);

    [TestMethod]
    [DataRow("var text = $\"{item}{(value = \"later\")}\";", "\"later\"")]
    [DataRow("object text = $\"{item}{(value = \"later\")}\";", "\"later\"")]
    [DataRow("value = $\"{item}{(value = \"later\")}\";", "\"later\"")]
    [DataRow("var text = $\"{(value = \"first\")}{item}\";", "\"first\"")]
    [DataRow("FormattableString text = $\"{item}{(value = \"later\")}\";", "")]
    [DataRow("IFormattable text = $\"{item}{(value = \"later\")}\";", "")]
    [DataRow("var text = $\"{(string)null}{(value = \"later\")}\";", "")]
    public void Legacy_Interpolation_Formats_After_All_Arguments(string statement, string expected) => AssertFlow(
        "string value = \"old\"; try { " + statement + " } catch { [|Use(value)|]; }", expected,
        languageVersion: LanguageVersion.CSharp9);

    [TestMethod]
    public async Task Interpolation_Without_Runtime_Handler_Formats_After_All_Arguments()
    {
        var references = await ReferenceAssemblies.NetStandard.NetStandard20.ResolveAsync(LanguageNames.CSharp, CancellationToken.None);
        AssertFlow("""
            string value = "old";
            try { var text = $"{item}{(value = "later")}"; }
            catch { [|Use(value)|]; }
            """, "\"later\"", references: references);
    }

    [TestMethod]
    public void Dynamic_Interpolation_Formats_After_All_Arguments() => AssertFlow("""
        string value = "old";
        dynamic source = item;
        try { var text = $"{source}{(value = "later")}"; }
        catch { [|Use(value)|]; }
        """, "\"later\"");

    [TestMethod]
    [DataRow("if (item is string value) { [|Use(value)|]; }", false, "")]
    [DataRow("if (item is var value) { [|Use(value)|]; }", false, "")]
    [DataRow("if (item is Holder { Field: var value }) { [|Use(value)|]; }", false, "")]
    [DataRow("if (item is Holder value) { [|Use(value)|]; }", false, "")]
    [DataRow("if (item is Holder { } value) { [|Use(value)|]; }", false, "")]
    [DataRow("if (item is string[] array && array is [var value]) { [|Use(value)|]; }", false, "")]
    [DataRow("if (item is string[] array && array is [..] value) { [|Use(value)|]; }", false, "")]
    [DataRow("if (item is string value) { value = \"known\"; [|Use(value)|]; }", true, "\"known\"")]
    [DataRow("if (item is string value) { Action nested = () => [|Use(value)|]; }", true, "")]
    public void Pattern_Definitions_Are_Unsupported_Until_Overwritten(string body, bool supported, string expected) =>
        AssertFlow(body, expected, incoming: body.Contains("Action nested", StringComparison.Ordinal), supported: supported);

    [TestMethod]
    [DataRow("foreach (string value in new[] { \"item\" }) { [|Use(value)|]; }")]
    [DataRow("foreach (var value in (System.Collections.Generic.IEnumerable<string>)new[] { \"item\" }) { [|Use(value)|]; }")]
    [DataRow("foreach (char value in \"text\") { [|Use(value)|]; }")]
    [DataRow("foreach (var (value, other) in new[] { (\"item\", 0) }) { [|Use(value)|]; }")]
    [DataRow("foreach (var (other, (value, last)) in new[] { (0, (\"item\", 1)) }) { [|Use(value)|]; }")]
    [DataRow("foreach (ref string value in new Span<string>(new[] { \"item\" })) { [|Use(value)|]; }")]
    [DataRow("foreach (ref readonly string value in new ReadOnlySpan<string>(new[] { \"item\" })) { [|Use(value)|]; }")]
    [DataRow("foreach (string other in new[] { \"outer\" }) { foreach (string value in new[] { \"inner\" }) { [|Use(value)|]; } }")]
    public void Foreach_Definitions_Are_Unsupported(string body) =>
        AssertFlow(body, "", supported: false);

    [TestMethod]
    [DataRow("foreach (ref string value in new Span<string>(new[] { \"item\" })) { value = \"known\"; [|Use(value)|]; }")]
    [DataRow("foreach (ref string value in new Span<string>(new[] { \"item\" })) { if (flag) value = \"first\"; else value = \"second\"; [|Use(value)|]; }")]
    [DataRow("var items = new[] { \"item\" }; foreach (ref string value in new Span<string>(items)) { value = \"known\"; items[0] = \"changed\"; [|Use(value)|]; }")]
    public void Foreach_Ref_Values_With_Heap_Backing_Remain_Unsupported_After_Writes(string body) =>
        AssertFlow(body, "", supported: false);

    [TestMethod]
    [DataRow("string value = \"old\"; foreach (string other in new[] { \"item\" }) { [|Use(value)|]; }", "\"old\"")]
    public void Foreach_Definitions_Only_Affect_Reaching_Paths(string body, string expected) =>
        AssertFlow(body, expected);

    [TestMethod]
    public void Nested_Callable_Inherits_Foreach_Capture() =>
        AssertFlow("foreach (string value in new[] { \"item\" }) { Action nested = () => [|Use(value)|]; }", "", incoming: true);

    [TestMethod]
    [DataRow("ThrowingCollection items = [];", "\"old\"")]
    [DataRow("ThrowingCollection items = [(value = \"element\")];", "\"element\"|\"old\"")]
    [DataRow("ThrowingCollection items = [(value = \"first\"), (value = \"second\")];", "\"first\"|\"old\"|\"second\"")]
    [DataRow("ThrowingCollection items = [flag ? (value = \"first\") : (value = \"second\")];", "\"first\"|\"old\"|\"second\"")]
    [DataRow("ThrowingValueCollection items = [];", "\"old\"")]
    [DataRow("ThrowingValueCollection items = [(value = \"element\")];", "\"element\"|\"old\"")]
    [DataRow("BuiltCollection items = [];", "\"old\"")]
    [DataRow("BuiltCollection items = [(value = \"element\")];", "\"element\"")]
    [DataRow("BuiltCollection items = [(value = \"first\"), (value = \"second\")];", "\"second\"")]
    [DataRow("BuiltCollection items = [flag ? (value = \"first\") : (value = \"second\")];", "\"first\"|\"second\"")]
    [DataRow("ReadOnlySpan<string> items = [];", "")]
    [DataRow("ReadOnlySpan<string> items = [(value = \"element\")];", "")]
    public void Collection_Construction_Exceptions_Respect_Element_Evaluation(string statement, string expected) => AssertFlow(
        "string value = \"old\"; try { " + statement + " } catch { [|Use(value)|]; }", expected,
        additionalSource: CollectionTypes);

    [TestMethod]
    [DataRow("ThrowingCollection items = [];", "\"old\"")]
    [DataRow("BuiltCollection items = [(value = \"element\")];", "\"element\"")]
    public void Collection_Construction_Exceptions_Reach_Filters_Before_Finally(string statement, string expected) => AssertFlow(
        "string value = \"old\"; try { try { " + statement + " } finally { value = \"cleanup\"; } } catch when ([|Observe(value)|]) { }", expected,
        additionalSource: CollectionTypes);

    [TestMethod]
    [DataRow("ThrowingCollection", "[..source, (value = \"later\")]", "\"later\"|\"old\"")]
    [DataRow("BuiltCollection", "[..source, (value = \"later\")]", "\"later\"|\"old\"")]
    [DataRow("string[]", "[..source, (value = \"later\")]", "\"later\"|\"old\"")]
    [DataRow("ReadOnlySpan<string>", "[..source, (value = \"later\")]", "\"later\"|\"old\"")]
    [DataRow("BuiltCollection", "[(value = \"first\"), ..source, (value = \"later\")]", "\"first\"|\"later\"")]
    [DataRow("BuiltCollection", "[..source, flag ? (value = \"first\") : (value = \"second\")]", "\"first\"|\"old\"|\"second\"")]
    [DataRow("BuiltCollection", "[(value = \"before\"), ..source, flag ? (value = \"first\") : (value = \"second\")]", "\"before\"|\"first\"|\"second\"")]
    [DataRow("string[]", "[..source, flag ? (value = \"first\") : (value = \"second\")]", "\"first\"|\"old\"|\"second\"")]
    public void Collection_Spread_Exceptions_Respect_Element_Evaluation(string target, string elements, string expected) => AssertFlow(
        "string value = \"old\"; System.Collections.Generic.IEnumerable<string> source = null; try { " + target + " items = " + elements + "; } catch { [|Use(value)|]; }", expected,
        additionalSource: CollectionTypes);

    [TestMethod]
    [DataRow("if (false) { ThrowingCollection items = []; }", "\"old\"")]
    [DataRow("Action nested = () => { ThrowingCollection items = []; };", "\"old\"")]
    [DataRow("ThrowingCollection items = []; value = \"after\";", "\"after\"")]
    public void Collection_Construction_Only_Affects_Reaching_Paths(string statement, string expected) => AssertFlow(
        "string value = \"old\"; " + statement + " [|Use(value)|];", expected, additionalSource: CollectionTypes);

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
    public void Struct_Member_Stores_Invalidate_Whole_Local(string declarations, string store) =>
        AssertFlow(declarations + store + "; [|Use(value)|];", "", supported: false, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("(flag ? ref value : ref other).Field = 1")]
    [DataRow("(flag ? ref other : ref value).Property++")]
    [DataRow("(flag ? ref value : ref other).Nested.Field += 1")]
    [DataRow("(flag ? ref value : ref other)[0] = 1")]
    [DataRow("Change(ref (flag ? ref value : ref other).Nested.Field)")]
    public void Conditional_Ref_Member_Stores_Invalidate_Whole_Local(string expression) =>
        AssertFlow("Mutable value = new Mutable(); Mutable other = new Mutable(); " + expression + "; [|Use(value)|];",
            "", supported: false, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("value.Field = 1;")]
    [DataRow("Change(ref value.Nested.Field);")]
    [DataRow("Assign(out value.Field);")]
    [DataRow("value.Mutate();")]
    [DataRow("var (first, second) = value;")]
    [DataRow("value.Nested.Mutate();")]
    [DataRow("ref Mutable alias = ref value; alias.Mutate();")]
    [DataRow("ref Inner alias = ref value.Nested; alias.Mutate();")]
    [DataRow("Mutable other = new Mutable(); (flag ? ref value : ref other).Mutate();")]
    public void Mutations_Are_Unknown_On_Normal_And_Exceptional_Completion(string statement)
    {
        AssertFlow("Mutable value = new Mutable(); " + statement + " [|Use(value)|];",
            "", supported: false, additionalSource: MutableTypes);
        AssertFlow("Mutable value = new Mutable(); try { " + statement + " Unknown(); } catch { [|Use(value)|]; }",
            "", supported: false, additionalSource: MutableTypes);
    }

    [TestMethod]
    [DataRow("if (false) value.Field = 1;", "new Mutable()")]
    [DataRow("if (false) value.Mutate();", "new Mutable()")]
    [DataRow("value.Read();", "new Mutable()")]
    [DataRow("value.Nested.Read();", "new Mutable()")]
    [DataRow("Mutable other = new Mutable(); other.Mutate();", "new Mutable()")]
    [DataRow("value.Field = 1; value = default;", "default")]
    [DataRow("value.Mutate(); value = new Mutable();", "new Mutable()")]
    [DataRow("Mutable other = new Mutable(); (flag ? ref value : ref other).Reference.Field = 1;", "new Mutable()")]
    [DataRow("Mutable other = new Mutable(); Mutable third = new Mutable(); (value.Field == 0 ? ref other : ref third).Field = 1;", "new Mutable()")]
    public void Unreachable_Readonly_And_Unrelated_Mutations_Preserve_Definitions(string statement, string expected) =>
        AssertFlow("Mutable value = new Mutable(); " + statement + " [|Use(value)|];", expected, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("var holder = new Retained(ref value); holder.Field = 2;")]
    [DataRow("ref int alias = ref Forward(ref value); alias = 2;")]
    public void Retained_Ref_Escapes_Do_Not_Return_Stale_Initializers(string statement) =>
        AssertFlow("int value = 0; " + statement + " [|Use(value)|];", "", supported: false, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("var holder = new Retained(ref value); value = 3; holder.Field = 2;")]
    [DataRow("ref int alias = ref Forward(ref value); value = 3; alias = 2;")]
    [DataRow("var holder = new Retained(ref value); var copy = holder; value = 3; copy.Field = 2;")]
    [DataRow("scoped Retained holder = default; holder = new Retained(ref value); value = 3; holder.Field = 2;")]
    public void Escaped_Reference_Writes_Invalidate_A_Later_Whole_Local_Overwrite(string statement) =>
        AssertFlow("int value = 0; " + statement + " [|Use(value)|];", "", supported: false, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("RetainedCalls.Mutate(holder);", false)]
    [DataRow("RetainedCalls.MutateReadonly(in holder);", false)]
    [DataRow("RetainedCalls.MutateScoped(holder);", false)]
    [DataRow("var consumer = new RetainedConsumer(holder);", false)]
    [DataRow("try { RetainedCalls.Mutate(holder); } catch { [|Use(value)|]; }", true)]
    [DataRow("try { var consumer = new RetainedConsumer(holder); } catch { [|Use(value)|]; }", true)]
    public void RefLike_Arguments_Invalidate_Retained_Storage(string statement, bool exceptional) =>
        AssertFlow("int value = 0; var holder = new Retained(ref value); value = 3; " + statement
            + (exceptional ? "" : " [|Use(value)|];"), "", supported: false, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("RetainedCalls.Mutate(holder); value = 4;", "4", false)]
    [DataRow("if (false) RetainedCalls.Mutate(holder);", "3", false)]
    [DataRow("int other = 0; var unrelated = new Retained(ref other); RetainedCalls.Mutate(unrelated);", "3", false)]
    [DataRow("RetainedCalls.MutateAfterArgument(holder, [|RetainedCalls.ReadInt(value)|]);", "3", true)]
    public void RefLike_Argument_Invalidation_Respects_Execution_And_Recovery(string statement, string expected, bool marked) =>
        AssertFlow("int value = 0; var holder = new Retained(ref value); value = 3; " + statement
            + (marked ? "" : " [|Use(value)|];"), expected, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("holder.Mutate();", false)]
    [DataRow("_ = holder.MutatingProperty;", false)]
    [DataRow("(int first, int second) = holder;", false)]
    [DataRow("try { holder.Mutate(); } catch { [|Use(value)|]; }", true)]
    [DataRow("try { _ = holder.MutatingProperty; } catch { [|Use(value)|]; }", true)]
    [DataRow("try { (int first, int second) = holder; } catch { [|Use(value)|]; }", true)]
    public void Readonly_RefLike_Receivers_Invalidate_Retained_Storage(string statement, bool exceptional) =>
        AssertFlow("int value = 0; var holder = new Retained(ref value); value = 3; " + statement
            + (exceptional ? "" : " [|Use(value)|];"), "", supported: false, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("holder.Mutate();", false)]
    [DataRow("try { holder.Mutate(); } catch { [|Use(value)|]; }", true)]
    public void Readonly_RefLike_Types_Invalidate_Retained_Storage(string statement, bool exceptional) =>
        AssertFlow("int value = 0; var holder = new ReadonlyRetained(ref value); value = 3; " + statement
            + (exceptional ? "" : " [|Use(value)|];"), "", supported: false, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("holder.Mutate(); value = 4;", "4", false)]
    [DataRow("if (false) holder.Mutate();", "3", false)]
    [DataRow("int other = 0; var unrelated = new Retained(ref other); unrelated.Mutate();", "3", false)]
    [DataRow("holder.MutateAfterArgument([|RetainedCalls.ReadInt(value)|]);", "3", true)]
    public void Readonly_RefLike_Receiver_Invalidation_Preserves_Recovery_And_Evaluation_Order(
        string statement, string expected, bool marked) =>
        AssertFlow("int value = 0; var holder = new Retained(ref value); value = 3; " + statement
            + (marked ? "" : " [|Use(value)|];"), expected, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("var holder = new Retained(ref value); var copy = holder; value = 3; copy.Field++;")]
    [DataRow("var holder = new Retained(ref value); value = 3; (holder.Field, _) = (2, 0);")]
    [DataRow("var holder = new Retained(ref value); value = 3; holder.Mutate();")]
    [DataRow("var holder = new Retained(ref value); value = 3; _ = holder.Property;")]
    [DataRow("var holder = new Retained(ref value); value = 3; _ = holder + 2;")]
    [DataRow("scoped Retained holder = default; holder.Field = ref value; value = 3; holder.Field = 2;")]
    [DataRow("scoped Retained holder = default; holder.Bind(ref value); value = 3; holder.Field = 2;")]
    [DataRow("var holder = new Retained(ref value); (value, holder.Field) = (3, 2);")]
    [DataRow("var holder = new Retained(ref value); value = 3; if (flag) holder.Field = 2;")]
    [DataRow("var holder = new Retained(ref value); value = 3; while (flag) { holder.Field = 2; }")]
    public void Retained_Ref_Writes_After_Overwrite_Do_Not_Return_Stale_Definitions(string statement) =>
        AssertFlow("int value = 0; " + statement + " [|Use(value)|];", "", supported: false, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("var holder = new Retained(ref value); holder.Field = 2;")]
    [DataRow("ref int alias = ref Forward(ref value); alias = 2;")]
    [DataRow("var holder = new Retained(ref value); value = 1; holder.Mutate();")]
    [DataRow("var holder = new Retained(ref value); (holder.Field, value) = (2, 1);")]
    public void Retained_Ref_Escape_Can_Recover_After_Whole_Local_Overwrite(string statement) =>
        AssertFlow("int value = 0; " + statement + " value = 3; [|Use(value)|];",
            "3", additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("RetainedCalls.Bind(ref holder, ref value); value = 3; holder.Field = 2;", "", false)]
    [DataRow("RetainedCalls.Bind(ref holder, ref value); var copy = holder; value = 3; copy.Field = 2;", "", false)]
    [DataRow("RetainedCalls.BindOut(out holder, ref value); value = 3; holder.Mutate();", "", false)]
    [DataRow("RetainedCalls.Bind(ref holder, ref value); value = 3; try { holder.MutateAndThrow(); } catch { [|Use(value)|]; }", "", false)]
    [DataRow("RetainedCalls.Bind(ref holder, ref value); holder.Field = 2; value = 3;", "3", true)]
    [DataRow("int other = 0; RetainedCalls.Bind(ref holder, ref other); value = 3; holder.Field = 2;", "3", true)]
    [DataRow("RetainedCalls.Bind(ref holder, ref value); value = 3; if (false) holder.Field = 2;", "3", true)]
    [DataRow("RetainedCalls.Bind(ref holder, ref value); value = 3; holder.Field = [|value|];", "3", true)]
    public void Writable_RefLike_Arguments_Retain_Storage_Bindings(string statement, string expected, bool supported) =>
        AssertFlow("int value = 0; scoped Retained holder = default; " + statement
            + (statement.Contains("[|", StringComparison.Ordinal) ? "" : " [|Use(value)|];"),
            expected, supported: supported, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("new RetainedBinder(ref holder, ref value); value = 3; holder.Field = 2;", "", false)]
    [DataRow("new RetainedBinder(ref holder, ref value); var copy = holder; value = 3; copy.Field = 2;", "", false)]
    [DataRow("RetainedBinder binder = new(ref holder, ref value); value = 3; holder.Field = 2;", "", false)]
    [DataRow("try { new RetainedBinder(ref holder, ref value, true); } catch { value = 3; holder.Field = 2; [|Use(value)|]; }", "", false)]
    [DataRow("new RetainedOutBinder(out holder, ref value); value = 3; holder.Mutate();", "", false)]
    [DataRow("new RetainedBinder(ref holder, ref value); value = 3; try { holder.MutateAndThrow(); } catch { [|Use(value)|]; }", "", false)]
    [DataRow("new RetainedBinder(ref holder, ref value); holder.Field = 2; value = 3;", "3", true)]
    [DataRow("int other = 0; new RetainedBinder(ref holder, ref other); value = 3; holder.Field = 2;", "3", true)]
    [DataRow("new RetainedBinder(ref holder, ref value); value = 3; if (false) holder.Field = 2;", "3", true)]
    [DataRow("new RetainedBinder(ref holder, ref value); value = 3; holder.Field = [|value|];", "3", true)]
    public void Constructor_Arguments_Retain_Storage_Bindings(string statement, string expected, bool supported) =>
        AssertFlow("int value = 0; scoped Retained holder = default; " + statement
            + (statement.Contains("[|", StringComparison.Ordinal) ? "" : " [|Use(value)|];"),
            expected, supported: supported, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("_ = holder is (0, 0); [|Use(value)|];", "", false)]
    [DataRow("try { _ = holder is (0, 0); } catch { [|Use(value)|]; }", "", false)]
    [DataRow("_ = holder switch { (0, 0) => true, _ => false }; [|Use(value)|];", "", false)]
    [DataRow("try { switch (holder) { case (0, 0): break; } } catch { [|Use(value)|]; }", "", false)]
    [DataRow("_ = holder is (0, 0); value = 4; [|Use(value)|];", "4", true)]
    [DataRow("if (false) { _ = holder is (0, 0); } [|Use(value)|];", "3", true)]
    [DataRow("int other = 0; var unrelated = new Retained(ref other); _ = unrelated is (0, 0); [|Use(value)|];", "3", true)]
    public void Positional_Patterns_Invalidate_Retained_Receiver_Storage(string statement, string expected, bool supported) =>
        AssertFlow("int value = 0; var holder = new Retained(ref value); value = 3; " + statement,
            expected, supported: supported, additionalSource: MutableTypes);

    [TestMethod]
    public void Retained_Ref_Write_Invalidates_Before_Exceptional_Completion() =>
        AssertFlow("int value = 0; var holder = new Retained(ref value); value = 3; try { holder.MutateAndThrow(); } catch { [|Use(value)|]; }",
            "", supported: false, additionalSource: MutableTypes);

    [TestMethod]
    public void Retained_Ref_Write_Observes_Rhs_Before_Store() =>
        AssertFlow("int value = 0; var holder = new Retained(ref value); value = 3; holder.Field = [|value|];",
            "3", additionalSource: MutableTypes);

    [TestMethod]
    public void Unreachable_Ref_Escape_Does_Not_Invalidate_Later_Writes() =>
        AssertFlow("int value = 0; if (false) { var holder = new Retained(ref value); } int other = 2; [|Use(value)|];",
            "0", additionalSource: MutableTypes);

    [TestMethod]
    public void Ref_Returning_Receiver_Can_Escape_Storage_Across_Overwrite() =>
        AssertFlow("Inner value = new Inner(); ref int alias = ref value.GetRef(); value = new Inner(); alias = 2; [|Use(value)|];",
            "", supported: false, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("sink[holder] = [|RetainedCalls.ReadInt(value)|];", "3", true)]
    [DataRow("sink[holder] = 2; [|Use(value)|];", "", false)]
    [DataRow("try { sink[holder] = 2; } catch { [|Use(value)|]; }", "", false)]
    [DataRow("_ = sink[holder]; [|Use(value)|];", "", false)]
    public void RefLike_Indexer_Arguments_Invalidate_At_Accessor_Execution(string statement, string expected, bool supported) =>
        AssertFlow("int value = 0; var holder = new Retained(ref value); var sink = new RetainedSink(); value = 3; " + statement,
            expected, supported: supported, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("foreach (var copy in new RetainedItems(holder)) { value = 3; copy.Field = 2; [|Use(value)|]; }", "", false)]
    [DataRow("var items = new RetainedItems(holder); foreach (var copy in items) { var second = copy; value = 3; second.Field = 2; [|Use(value)|]; }", "", false)]
    [DataRow("foreach (var copy in new RetainedItems(holder)) { value = 3; try { copy.MutateAndThrow(); } catch { [|Use(value)|]; } }", "", false)]
    [DataRow("foreach (var copy in new RetainedItems(holder)) { copy.Field = 2; value = 3; [|Use(value)|]; }", "3", true)]
    [DataRow("foreach (var copy in new RetainedItems(holder)) { value = 3; if (false) copy.Field = 2; [|Use(value)|]; }", "3", true)]
    [DataRow("foreach (var copy in new RetainedItems(holder)) { value = 3; copy.Field = [|value|]; }", "3", true)]
    [DataRow("int other = 0; var unrelated = new Retained(ref other); foreach (var copy in new RetainedItems(unrelated)) { value = 3; copy.Field = 2; [|Use(value)|]; }", "3", true)]
    [DataRow("foreach (ref var copy in new RetainedItems(holder)) { value = 3; copy.Field = 2; [|Use(value)|]; }", "", false)]
    [DataRow("foreach (ref readonly var copy in new RetainedItems(holder)) { value = 3; copy.MutateAfterArgument(0); [|Use(value)|]; }", "", false)]
    [DataRow("foreach (ref int copy in new RefItems(ref value)) { value = 3; copy = 2; [|Use(value)|]; }", "", false)]
    public void Foreach_RefLike_Values_Retain_Collection_Storage(string statement, string expected, bool supported) =>
        AssertFlow("int value = 0; var holder = new Retained(ref value); " + statement,
            expected, supported: supported, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("ref int alias = ref sink[holder]; value = 3; alias = 2;", "", false)]
    [DataRow("var copy = sink[holder, 0]; value = 3; copy.Field = 2;", "", false)]
    [DataRow("var copy = sink[holder, 0]; var second = copy; value = 3; second.Field = 2;", "", false)]
    [DataRow("var copy = sink[holder, 0]; value = 3; try { copy.MutateAndThrow(); } catch { [|Use(value)|]; }", "", false)]
    [DataRow("ref int alias = ref sink[holder]; value = 3; alias = 2; value = 4;", "4", true)]
    [DataRow("ref int alias = ref sink[holder]; value = 3; if (false) alias = 2;", "3", true)]
    [DataRow("ref int alias = ref sink[holder]; value = 3; alias = [|value|];", "3", true)]
    [DataRow("int other = 0; var unrelated = new Retained(ref other); ref int alias = ref sink[unrelated]; value = 3; alias = 2;", "3", true)]
    public void Indexer_Returns_Retain_Argument_Storage(string statement, string expected, bool supported) =>
        AssertFlow("int value = 0; var holder = new Retained(ref value); var sink = new ReturningSink(); " + statement
            + (statement.Contains("[|", StringComparison.Ordinal) ? "" : " [|Use(value)|];"),
            expected, supported: supported, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("if (holder is var copy) { value = 3; copy.Field = 2; [|Use(value)|]; }", "", false)]
    [DataRow("if (holder is { } copy) { value = 3; copy.Field = 2; [|Use(value)|]; }", "", false)]
    [DataRow("if (holder is var copy) { var second = copy; value = 3; second.Field = 2; [|Use(value)|]; }", "", false)]
    [DataRow("switch (holder) { case var copy: value = 3; copy.Field = 2; [|Use(value)|]; break; }", "", false)]
    [DataRow("_ = holder switch { var copy => PatternCalls.MutatePattern(copy) }; [|Use(value)|];", "", false)]
    [DataRow("if (holder is var copy) { value = 3; try { copy.MutateAndThrow(); } catch { [|Use(value)|]; } }", "", false)]
    [DataRow("if (holder is var copy) { copy.Field = 2; value = 3; [|Use(value)|]; }", "3", true)]
    [DataRow("if (holder is var copy) { value = 3; if (false) copy.Field = 2; [|Use(value)|]; }", "3", true)]
    [DataRow("if (holder is var copy) { value = 3; copy.Field = [|value|]; }", "3", true)]
    [DataRow("int other = 0; var unrelated = new Retained(ref other); if (unrelated is var copy) { value = 3; copy.Field = 2; [|Use(value)|]; }", "3", true)]
    public void RefLike_Pattern_Bindings_Retain_Input_Storage(string statement, string expected, bool supported) =>
        AssertFlow("int value = 0; var holder = new Retained(ref value); value = 3; " + statement,
            expected, supported: supported, additionalSource: MutableTypes
                + " static class PatternCalls { public static int MutatePattern(Retained copy) { copy.Field = 2; return 0; } }");

    [TestMethod]
    [DataRow("holder.Copy(out copy); value = 3; copy.Field = 2;", "", false)]
    [DataRow("holder.CopyRef(ref copy); var second = copy; value = 3; second.Field = 2;", "", false)]
    [DataRow("holder.Copy(out copy); value = 3; try { copy.MutateAndThrow(); } catch { [|Use(value)|]; }", "", false)]
    [DataRow("holder.Copy(out copy); copy.Field = 2; value = 3;", "3", true)]
    [DataRow("holder.Copy(out copy); value = 3; if (false) copy.Field = 2;", "3", true)]
    [DataRow("holder.Copy(out copy); value = 3; copy.Field = [|value|];", "3", true)]
    [DataRow("int other = 0; var unrelated = new Retained(ref other); unrelated.Copy(out copy); value = 3; copy.Field = 2;", "3", true)]
    public void Receiver_Storage_Is_Retained_By_Writable_Arguments(string statement, string expected, bool supported) =>
        AssertFlow("int value = 0; var holder = new Retained(ref value); scoped Retained copy = default; " + statement
            + (statement.Contains("[|", StringComparison.Ordinal) ? "" : " [|Use(value)|];"),
            expected, supported: supported, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("(copy, _) = holder; value = 3; copy.Field = 2;", "", false)]
    [DataRow("(copy, _) = holder; var second = copy; value = 3; second.Field = 2;", "", false)]
    [DataRow("(copy, _) = holder; value = 3; try { copy.MutateAndThrow(); } catch { [|Use(value)|]; }", "", false)]
    [DataRow("(copy, _) = holder; copy.Field = 2; value = 3;", "3", true)]
    [DataRow("(copy, _) = holder; value = 3; if (false) copy.Field = 2;", "3", true)]
    [DataRow("(copy, _) = holder; value = 3; copy.Field = [|value|];", "3", true)]
    [DataRow("int other = 0; var unrelated = new Retained(ref other); (copy, _) = unrelated; value = 3; copy.Field = 2;", "3", true)]
    public void Deconstruction_Outputs_Retain_Storage(string statement, string expected, bool supported) =>
        AssertFlow("int value = 0; var holder = new Retained(ref value); scoped Retained copy = default; " + statement
            + (statement.Contains("[|", StringComparison.Ordinal) ? "" : " [|Use(value)|];"),
            expected, supported: supported, additionalSource: MutableTypes.Replace(
                "public readonly void Deconstruct(out int first, out int second)",
                "public readonly void Deconstruct(out Retained copy, out int ignored) { copy = this; ignored = 0; } public readonly void DeconstructUnused(out int first, out int second)",
                StringComparison.Ordinal));

    [TestMethod]
    public void Value_Receiver_Storage_Can_Be_Retained_By_Output() =>
        AssertFlow("Inner value = new Inner(); scoped Retained copy = default; value.Copy(out copy); value = new Inner(); copy.Field = 2; [|Use(value)|];",
            "", supported: false, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("container.Item = holder; value = 3; container.Item.Field = 2;", "", false)]
    [DataRow("container.Item = holder; var copy = container; value = 3; copy.Item.Field = 2;", "", false)]
    [DataRow("container.Item = holder; value = 3; try { container.Item.MutateAndThrow(); } catch { [|Use(value)|]; }", "", false)]
    [DataRow("container.Item = holder; container.Item.Field = 2; value = 3;", "3", true)]
    [DataRow("container.Item = holder; value = 3; if (false) container.Item.Field = 2;", "3", true)]
    [DataRow("container.Item = holder; value = 3; container.Item.Field = [|value|];", "3", true)]
    [DataRow("int other = 0; var unrelated = new Retained(ref other); container.Item = unrelated; value = 3; container.Item.Field = 2;", "3", true)]
    public void RefLike_Field_Values_Retain_Storage(string statement, string expected, bool supported) =>
        AssertFlow("int value = 0; var holder = new Retained(ref value); scoped RetainedContainer container = default; " + statement
            + (statement.Contains("[|", StringComparison.Ordinal) ? "" : " [|Use(value)|];"),
            expected, supported: supported, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("scoped RetainedContainer container = new() { Item = holder }; value = 3; container.Item.Field = 2;", "", false)]
    [DataRow("scoped RetainedContainer container = new() { Item = holder }; var copy = container; value = 3; copy.Item.Field = 2;", "", false)]
    [DataRow("scoped RetainedContainer container = new() { Item = holder }; value = 3; try { container.Item.MutateAndThrow(); } catch { [|Use(value)|]; }", "", false)]
    [DataRow("scoped RetainedContainer container = new() { Item = holder }; container.Item.Field = 2; value = 3;", "3", true)]
    [DataRow("scoped RetainedContainer container = new() { Item = holder }; value = 3; if (false) container.Item.Field = 2;", "3", true)]
    [DataRow("scoped RetainedContainer container = new() { Item = holder }; value = 3; container.Item.Field = [|value|];", "3", true)]
    [DataRow("int other = 0; var unrelated = new Retained(ref other); scoped RetainedContainer container = new() { Item = unrelated }; value = 3; container.Item.Field = 2;", "3", true)]
    public void Object_Initializers_Retain_Storage(string statement, string expected, bool supported) =>
        AssertFlow("int value = 0; var holder = new Retained(ref value); " + statement
            + (statement.Contains("[|", StringComparison.Ordinal) ? "" : " [|Use(value)|];"),
            expected, supported: supported, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("scoped RetainedCollection items = new() { holder }; value = 3; items.Item.Field = 2;", "", false)]
    [DataRow("scoped RetainedCollection items = new() { holder }; var copy = items; value = 3; copy.Item.Field = 2;", "", false)]
    [DataRow("scoped RetainedCollection items = default; items = new() { holder }; value = 3; items.Item.Field = 2;", "", false)]
    [DataRow("scoped RetainedCollection items = new() { { default, holder } }; value = 3; items.Item.Field = 2;", "", false)]
    [DataRow("scoped RetainedCollection items = new() { holder }; value = 3; try { items.Item.MutateAndThrow(); } catch { [|Use(value)|]; }", "", false)]
    [DataRow("scoped RetainedCollection items = new() { holder }; items.Item.Field = 2; value = 3;", "3", true)]
    [DataRow("scoped RetainedCollection items = new() { holder }; value = 3; if (false) items.Item.Field = 2;", "3", true)]
    [DataRow("scoped RetainedCollection items = new() { holder }; value = 3; items.Item.Field = [|value|];", "3", true)]
    [DataRow("int other = 0; var unrelated = new Retained(ref other); scoped RetainedCollection items = new() { unrelated }; value = 3; items.Item.Field = 2;", "3", true)]
    public void Collection_Initializers_Retain_Storage(string statement, string expected, bool supported) =>
        AssertFlow("int value = 0; var holder = new Retained(ref value); " + statement
            + (statement.Contains("[|", StringComparison.Ordinal) ? "" : " [|Use(value)|];"),
            expected, supported: supported, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("var copy = flag switch { true => holder, _ => holder }; value = 3; copy.Field = 2;", "", false)]
    [DataRow("scoped Retained copy = default; copy = flag switch { true => holder, _ => default }; value = 3; copy.Field = 2;", "", false)]
    [DataRow("var copy = flag switch { true => default, _ => holder }; var second = copy; value = 3; second.Field = 2;", "", false)]
    [DataRow("var copy = flag switch { true => holder, _ => holder }; value = 3; try { copy.MutateAndThrow(); } catch { [|Use(value)|]; }", "", false)]
    [DataRow("var copy = flag switch { true => holder, _ => holder }; copy.Field = 2; value = 3;", "3", true)]
    [DataRow("var copy = flag switch { true => holder, _ => holder }; value = 3; if (false) copy.Field = 2;", "3", true)]
    [DataRow("var copy = flag switch { true => holder, _ => holder }; value = 3; copy.Field = [|value|];", "3", true)]
    [DataRow("int other = 0; var unrelated = new Retained(ref other); var copy = flag switch { true => unrelated, _ => unrelated }; value = 3; copy.Field = 2;", "3", true)]
    public void Switch_Expression_Results_Retain_Storage(string statement, string expected, bool supported) =>
        AssertFlow("int value = 0; var holder = new Retained(ref value); " + statement
            + (statement.Contains("[|", StringComparison.Ordinal) ? "" : " [|Use(value)|];"),
            expected, supported: supported, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("var copy = holder - 0; value = 3; copy.Field = 2;", "", false)]
    [DataRow("var copy = 0 - holder; value = 3; copy.Field = 2;", "", false)]
    [DataRow("var copy = ++holder; value = 3; copy.Field = 2;", "", false)]
    [DataRow("var copy = holder++; value = 3; copy.Field = 2;", "", false)]
    [DataRow("var copy = -holder; var second = copy; value = 3; second.Field = 2;", "", false)]
    [DataRow("scoped Retained copy = default; copy = holder - 0; value = 3; try { copy.MutateAndThrow(); } catch { [|Use(value)|]; }", "", false)]
    [DataRow("var copy = holder - 0; copy.Field = 2; value = 3;", "3", true)]
    [DataRow("var copy = -holder; value = 3; if (false) copy.Field = 2;", "3", true)]
    [DataRow("var copy = holder - 0; value = 3; copy.Field = [|value|];", "3", true)]
    [DataRow("int other = 0; var unrelated = new Retained(ref other); var copy = -unrelated; value = 3; copy.Field = 2;", "3", true)]
    public void Operator_Results_Retain_Storage(string statement, string expected, bool supported) =>
        AssertFlow("int value = 0; var holder = new Retained(ref value); " + statement
            + (statement.Contains("[|", StringComparison.Ordinal) ? "" : " [|Use(value)|];"),
            expected, supported: supported, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("var copy = holder with { }; value = 3; copy.Field = 2;", "", false)]
    [DataRow("var copy = holder with { }; var second = copy; value = 3; second.Field = 2;", "", false)]
    [DataRow("scoped Retained copy = default; copy = holder with { }; value = 3; try { copy.MutateAndThrow(); } catch { [|Use(value)|]; }", "", false)]
    [DataRow("scoped RetainedContainer container = default; var copy = container with { Item = holder }; value = 3; copy.Item.Field = 2;", "", false)]
    [DataRow("var copy = holder with { }; copy.Field = 2; value = 3;", "3", true)]
    [DataRow("var copy = holder with { }; value = 3; if (false) copy.Field = 2;", "3", true)]
    [DataRow("var copy = holder with { }; value = 3; copy.Field = [|value|];", "3", true)]
    [DataRow("int other = 0; var unrelated = new Retained(ref other); var copy = unrelated with { }; value = 3; copy.Field = 2;", "3", true)]
    public void With_Expression_Results_Retain_Storage(string statement, string expected, bool supported) =>
        AssertFlow("int value = 0; var holder = new Retained(ref value); " + statement
            + (statement.Contains("[|", StringComparison.Ordinal) ? "" : " [|Use(value)|];"),
            expected, supported: supported, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("scoped Retained copy = default; copy += holder; value = 3; copy.Field = 2;", "", false)]
    [DataRow("scoped Retained copy = default; copy += holder; var second = copy; value = 3; second.Field = 2;", "", false)]
    [DataRow("scoped RetainedContainer container = default; container.Item += holder; value = 3; container.Item.Field = 2;", "", false)]
    [DataRow("scoped Retained copy = default; copy += holder; value = 3; try { copy.MutateAndThrow(); } catch { [|Use(value)|]; }", "", false)]
    [DataRow("scoped Retained copy = default; copy += holder; copy.Field = 2; value = 3;", "3", true)]
    [DataRow("scoped Retained copy = default; copy += holder; value = 3; if (false) copy.Field = 2;", "3", true)]
    [DataRow("scoped Retained copy = default; copy += holder; value = 3; copy.Field = [|value|];", "3", true)]
    [DataRow("int other = 0; var unrelated = new Retained(ref other); scoped Retained copy = default; copy += unrelated; value = 3; copy.Field = 2;", "3", true)]
    public void Compound_Assignment_Results_Retain_Storage(string statement, string expected, bool supported) =>
        AssertFlow("int value = 0; var holder = new Retained(ref value); " + statement
            + (statement.Contains("[|", StringComparison.Ordinal) ? "" : " [|Use(value)|];"),
            expected, supported: supported, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("_ = container[holder]; value = 3; container.Item.Field = 2;", "", false)]
    [DataRow("_ = container[holder]; var copy = container; value = 3; copy.Item.Field = 2;", "", false)]
    [DataRow("container[holder] = 0; value = 3; container.Item.Field = 2;", "", false)]
    [DataRow("try { _ = container[holder, true]; } catch { value = 3; container.Item.Field = 2; [|Use(value)|]; }", "", false)]
    [DataRow("_ = container[holder]; container.Item.Field = 2; value = 3;", "3", true)]
    [DataRow("_ = container[holder]; value = 3; if (false) container.Item.Field = 2;", "3", true)]
    [DataRow("value = 3; _ = container[holder, [|value|] == 0];", "3", true)]
    [DataRow("int other = 0; var unrelated = new Retained(ref other); _ = container[unrelated]; value = 3; container.Item.Field = 2;", "3", true)]
    public void Indexer_Receivers_Retain_Argument_Storage(string statement, string expected, bool supported) =>
        AssertFlow("int value = 0; var holder = new Retained(ref value); scoped RetainedContainer container = default; " + statement
            + (statement.Contains("[|", StringComparison.Ordinal) ? "" : " [|Use(value)|];"),
            expected, supported: supported, additionalSource: MutableTypes);

    [TestMethod]
    public void Mutating_Setter_Invalidates_Before_Throwing() =>
        AssertFlow("Mutable value = new Mutable(); try { value.Property = 1; } catch { [|Use(value)|]; }",
            "", supported: false, additionalSource: MutableTypes);

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
    public void Event_Stores_Invalidate_Only_Value_Type_Storage(string declarations, string store, bool invalidates) =>
        AssertFlow(declarations + store + "; [|Use(value)|];",
            invalidates ? "" : declarations.StartsWith("Holder", StringComparison.Ordinal) ? "new Holder()" : "new Mutable()",
            supported: !invalidates, additionalSource: MutableTypes);

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
    public void Generic_Member_Stores_Invalidate_Only_Potential_Value_Types(string constraints, string store, bool invalidates) =>
        AssertFlow("T value = new T(); " + store + "; [|Use(value)|];", invalidates ? "" : "new T()",
            supported: !invalidates, additionalSource: MutableTypes, constraints: constraints);

    [TestMethod]
    [DataRow("value[0] = 1")]
    [DataRow("value[0]++")]
    [DataRow("value[0] += 1")]
    [DataRow("alias = 1")]
    [DataRow("bufferAlias[0] = 1")]
    [DataRow("(flag ? ref value : ref other)[0] = 1")]
    public void Inline_Array_Stores_Invalidate_Receiver_And_Recover(string store)
    {
        const string declarations = "Buffer value = default; Buffer other = default; ref int alias = ref value[0]; ref Buffer bufferAlias = ref value;";
        AssertFlow(declarations + store + "; [|Use(value)|];", "", supported: false, additionalSource: MutableTypes);
        AssertFlow(declarations + store + "; value = new Buffer(); [|Use(value)|];", "new Buffer()", additionalSource: MutableTypes);
    }

    [TestMethod]
    [DataRow("Change(ref value[0])")]
    [DataRow("Assign(out value[0])")]
    [DataRow("Change(ref alias)")]
    public void Inline_Array_Ref_Out_Arguments_Invalidate_Receiver(string call) =>
        AssertFlow("Buffer value = default; ref int alias = ref value[0]; " + call + "; [|Use(value)|];",
            "", supported: false, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("value.Field = ref other", true)]
    [DataRow("alias.Field = ref other", true)]
    [DataRow("unrelated.Field = ref other", false)]
    [DataRow("alias = ref unrelated", false)]
    public void Ref_Field_Rebinding_Invalidates_Only_Receiver_Storage(string store, bool invalidates) =>
        AssertFlow("int source = 0; int other = 1; RefValue value = new RefValue(ref source); RefValue unrelated = new RefValue(ref source); ref RefValue alias = ref value; "
            + store + "; _ = [|value|];", invalidates ? "" : "new RefValue(ref source)", supported: !invalidates, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("holder.Field", "holder.Field = 2")]
    [DataRow("holder.Nested.Field", "holder.Nested.Field = 2")]
    [DataRow("items[0]", "items[0] = 2")]
    [DataRow("flag ? ref source : ref holder.Field", "holder.Field = 2")]
    [DataRow("alias", "holder.Field = 2")]
    public void Ref_Locals_With_Unrepresented_Backing_Storage_Remain_Unknown(string source, string store) =>
        AssertFlow("var holder = new Reference(); int[] items = new int[1]; int source = 0; ref int alias = ref holder.Field; ref int value = ref ("
            + source + "); value = 1; " + store + "; value = 3; [|Use(value)|];", "", supported: false, additionalSource: MutableTypes);

    [TestMethod]
    [DataRow("holder = new Retained(ref value); value = 3; holder.Field = 2; [|Use(value)|];", "", false)]
    [DataRow("holder = new Retained(ref value); var copy = holder; value = 3; copy.Field = 2; [|Use(value)|];", "", false)]
    [DataRow("holder.Field = ref value; value = 3; holder.Field = 2; [|Use(value)|];", "", false)]
    [DataRow("holder.Bind(ref value); value = 3; holder.Field = 2; [|Use(value)|];", "", false)]
    [DataRow("holder = new Retained(ref value); value = 3; try { holder.Mutate(); } catch { [|Use(value)|]; }", "", false)]
    [DataRow("holder = new Retained(ref value); value = 3; holder.Field = 2; value = 4; [|Use(value)|];", "4", true)]
    [DataRow("holder = new Retained(ref value); value = 3; if (false) holder.Field = 2; [|Use(value)|];", "3", true)]
    [DataRow("holder = new Retained(ref value); value = 3; holder.Field = [|value|];", "3", true)]
    [DataRow("int other = 0; holder = new Retained(ref other); value = 3; holder.Field = 2; [|Use(value)|];", "3", true)]
    public void Scoped_Parameter_Holders_Retain_Local_Storage(string statement, string expected, bool supported) =>
        AssertFlow("int value = 0; " + statement,
            expected, supported: supported, additionalSource: MutableTypes,
            parameters: ", scoped Retained holder");

    private const string MutableTypes = """
        [System.Runtime.CompilerServices.InlineArray(1)]
        struct Buffer { private int _element0; }
        interface IMutable { event Action Changed; int Property { get; set; } int this[int index] { get; set; } }
        class MutableBase { public int Property { get; set; } }
        ref struct RefValue { public ref int Field; public RefValue(ref int field) { Field = ref field; } }
        struct Inner
        {
            public event Action Changed { add { } remove { } }
            public int Field;
            public int Property { get => Field; set { Field = value; throw new Exception(); } }
            public void Deconstruct(out int first, out int second) { Field++; first = Field; second = 0; }
            public void Mutate() { Field++; }
            [System.Diagnostics.CodeAnalysis.UnscopedRef]
            public ref int GetRef() => ref Field;
            [System.Diagnostics.CodeAnalysis.UnscopedRef]
            public void Copy(out Retained copy) { copy = new Retained(ref Field); }
            public readonly int Read() => Field;
        }
        struct Mutable
        {
            public event Action Changed { add { } remove { } }
            public int Field;
            public Inner Nested;
            public Reference Reference;
            public int Property { get => Field; set { Field = value; throw new Exception(); } }
            public int this[int index] { get => Field; set { Field = value; } }
            public void Deconstruct(out int first, out int second) { Field++; first = Field; second = 0; }
            public void Mutate() { Field++; }
            public readonly int Read() => Field;
        }
        class Reference { public int Field; public Inner Nested; public event Action Changed { add { } remove { } } }
        static class RetainedCalls
        {
            public static void Bind(ref Retained holder, [System.Diagnostics.CodeAnalysis.UnscopedRef] ref int field) { holder.Field = ref field; }
            public static void BindOut(out Retained holder, [System.Diagnostics.CodeAnalysis.UnscopedRef] ref int field) { holder = new Retained(ref field); }
            public static void Mutate(Retained holder) { holder.Field = 2; throw new Exception(); }
            public static void MutateReadonly(in Retained holder) { holder.Field = 2; }
            public static void MutateScoped(scoped Retained holder) { holder.Field = 2; }
            public static void MutateAfterArgument(Retained holder, int argument) { holder.Field = 2; }
            public static int ReadInt(int item) => item;
        }
        class RetainedBinder
        {
            public RetainedBinder(ref Retained holder, [System.Diagnostics.CodeAnalysis.UnscopedRef] ref int field, bool fail = false)
            {
                holder.Field = ref field;
                if (fail) throw new Exception();
            }
        }
        class RetainedOutBinder
        {
            public RetainedOutBinder(out Retained holder, [System.Diagnostics.CodeAnalysis.UnscopedRef] ref int field) { holder = new Retained(ref field); }
        }
        class RetainedConsumer { public RetainedConsumer(Retained holder) { holder.Field = 2; throw new Exception(); } }
        class RetainedSink
        {
            public int this[Retained holder]
            {
                get { holder.Field = 2; return 0; }
                set { holder.Field = value; throw new Exception(); }
            }
        }
        class ReturningSink
        {
            public ref int this[Retained holder] => ref holder.Field;
            public Retained this[Retained holder, int index] => holder;
        }
        readonly ref struct ReadonlyRetained
        {
            public readonly ref int Field;
            public ReadonlyRetained(ref int field) { Field = ref field; }
            public void Mutate() { Field = 2; throw new Exception(); }
        }
        ref struct RetainedContainer
        {
            public Retained Item;
            [System.Runtime.CompilerServices.IndexerName("RetainedAt")]
            public int this[Retained item, bool fail = false]
            {
                get { Item = item; if (fail) throw new Exception(); return 0; }
                set { Item = item; }
            }
        }
        ref struct RetainedCollection : System.Collections.IEnumerable
        {
            public Retained Item;
            public void Add(Retained item) { Item = item; }
            public void Add(Retained first, Retained second) { Item = second; }
            public System.Collections.IEnumerator GetEnumerator() => throw new Exception();
        }
        ref struct RefItems
        {
            private ref int _item;
            public RefItems(ref int item) { _item = ref item; }
            public RefItems GetEnumerator() => this;
            public bool MoveNext() => true;
            public ref int Current => ref _item;
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
        ref struct Retained
        {
            public ref int Field;
            public Retained(ref int field) { Field = ref field; }
            public readonly void Copy(out Retained copy) { copy = this; }
            public readonly void CopyRef(ref Retained copy) { copy = this; }
            public void Bind([System.Diagnostics.CodeAnalysis.UnscopedRef] ref int field) { Field = ref field; }
            public readonly void Mutate() { Field = 2; throw new Exception(); }
            public readonly int MutatingProperty { get { Field = 2; throw new Exception(); } }
            public readonly void Deconstruct(out int first, out int second) { Field = 2; first = 0; second = 0; throw new Exception(); }
            public readonly void MutateAfterArgument(int argument) { Field = 2; }
            public readonly void MutateAndThrow() { Field = 2; throw new Exception(); }
            public readonly int Property { get { Field = 2; return 0; } }
            public static Retained operator +(Retained left, Retained right) => right;
            public static Retained operator ++(Retained holder) => holder;
            public static Retained operator -(Retained holder, int unused) => holder;
            public static Retained operator -(int unused, Retained holder) => holder;
            public static Retained operator -(Retained holder) => holder;
            public static int operator +(Retained holder, int value) { holder.Field = value; return value; }
        }
        """;

    private const string CollectionTypes = """
        class ThrowingCollection : System.Collections.Generic.IEnumerable<string>
        {
            public ThrowingCollection() => throw new Exception();
            public void Add(string item) => throw new Exception();
            public System.Collections.Generic.IEnumerator<string> GetEnumerator() => throw new Exception();
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
        }
        struct ThrowingValueCollection : System.Collections.Generic.IEnumerable<string>
        {
            public ThrowingValueCollection() => throw new Exception();
            public void Add(string item) => throw new Exception();
            public System.Collections.Generic.IEnumerator<string> GetEnumerator() => throw new Exception();
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
        }
        [System.Runtime.CompilerServices.CollectionBuilder(typeof(CollectionFactory), nameof(CollectionFactory.Create))]
        class BuiltCollection : System.Collections.Generic.IEnumerable<string>
        {
            public System.Collections.Generic.IEnumerator<string> GetEnumerator() => throw new Exception();
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
        }
        static class CollectionFactory
        {
            public static BuiltCollection Create(ReadOnlySpan<string> items) => throw new Exception();
        }
        """;

    private static void AssertFlow(string body, string expected, bool incoming = false, bool returns = false,
        bool supported = true, LanguageVersion languageVersion = LanguageVersion.Preview,
        IEnumerable<MetadataReference>? references = null, string additionalSource = "", string? constraints = null, string parameters = "")
    {
        var source = $$"""
            using System;
            {{additionalSource}}
            class ConversionSource
            {
                public static implicit operator string(ConversionSource source) => throw new Exception();
            }
            class FormattedSource : IFormattable
            {
                public string ToString(string format, IFormatProvider provider) => throw new Exception();
            }
            class TextSource { public override string ToString() => throw new Exception(); }
            struct TextStruct { public override string ToString() => throw new Exception(); }
            class RefHolder { public RefHolder(int unused, ref int item) { } }
            class ListHolder
            {
                public int Length => throw new Exception();
                public string this[int index] => throw new Exception();
                public ListHolder Slice(int start, int length) => throw new Exception();
            }
            class Holder
            {
                public event Action Changed { add { } remove { } }
                public Holder(string text = null) { }
                public string Field;
                public static string StaticField;
                public string Property { get; set; }
                public int Number { get; set; }
                public ref string RefProperty => ref Field;
                public void Deconstruct(out string first, out string second) => throw new Exception();
            }
            class Example
            {
                static ref int Forward(ref int item) => ref item;
                static void A() { }
                static void B() { }
                static void Use<T>(T item) { }
                static void Unknown() { }
                static bool ThrowFilter(string item) => true;
                static bool Observe(string item) => true;
                static void Change<T>(ref T item) { }
                static void Assign<T>(out T item) => item = default;
                static void ChangeWithArgument<T>(ref T item, int argument) { }
                static {{(returns ? "string" : "void")}} Run(bool flag, object item{{parameters}})
                {
                    {{body}}
                }
            }
            """;
        if (constraints is not null)
        {
            source = source.Replace("Run(bool flag, object item)", "Run<T>(bool flag, object item) where T : " + constraints);
        }

        var start = source.IndexOf("[|", StringComparison.Ordinal);
        var end = source.IndexOf("|]", StringComparison.Ordinal);
        Assert.IsGreaterThanOrEqualTo(0, start);
        Assert.IsGreaterThanOrEqualTo(start + 2, end);
        var span = new TextSpan(start, end - start - 2);
        source = source.Remove(end, 2).Remove(start, 2);
        var tree = CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(languageVersion));
        references ??= ((string) AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator)
            .Select(static path => MetadataReference.CreateFromFile(path));
        var compilation = CSharpCompilation.Create("FlowTest", [tree], references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        var errors = compilation.GetDiagnostics().Where(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error).ToArray();
        Assert.IsEmpty(errors, string.Join(Environment.NewLine, errors.Select(static error => error.ToString())));
        var model = compilation.GetSemanticModel(tree);
        var method = tree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().Single(static method => method.Identifier.ValueText == "Run");
        var root = model.GetOperation(method)!;
        var operation = root.DescendantsAndSelf().First(candidate => candidate.Syntax.Span == span);
        var local = method.DescendantNodes().Select(node => model.GetDeclaredSymbol(node)).OfType<ILocalSymbol>()
            .First(static symbol => symbol.Name == "value");
        Assert.AreEqual(supported, ReachingDefinitionFlow.TryGetReachingLocalValues(root, operation, local, out var values, out var inherits));
        Assert.AreEqual(incoming, inherits);
        Assert.AreSequenceEqual([.. expected.Split('|', StringSplitOptions.RemoveEmptyEntries).Order(StringComparer.Ordinal)], [.. values.Select(static value => value.Syntax.ToString()).Order(StringComparer.Ordinal)], "Actual definitions: " + string.Join(" | ", values.Select(static value => value.Syntax.ToString())));
    }
}
