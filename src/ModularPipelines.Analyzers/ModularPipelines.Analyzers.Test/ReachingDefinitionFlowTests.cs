using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
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

    private static void AssertFlow(string body, string expected, bool incoming = false, bool returns = false,
        bool supported = true)
    {
        var source = $$"""
            using System;
            class ConversionSource
            {
                public static implicit operator string(ConversionSource source) => throw new Exception();
            }
            class RefHolder { public RefHolder(int unused, ref int item) { } }
            class ListHolder
            {
                public int Length => throw new Exception();
                public string this[int index] => throw new Exception();
                public ListHolder Slice(int start, int length) => throw new Exception();
            }
            class Holder
            {
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
                static void A() { }
                static void B() { }
                static void Use<T>(T item) { }
                static void Unknown() { }
                static bool ThrowFilter(string item) => true;
                static bool Observe(string item) => true;
                static void Change<T>(ref T item) { }
                static void Assign<T>(out T item) => item = default;
                static void ChangeWithArgument<T>(ref T item, int argument) { }
                static {{(returns ? "string" : "void")}} Run(bool flag, object item)
                {
                    {{body}}
                }
            }
            """;
        var start = source.IndexOf("[|", StringComparison.Ordinal);
        var end = source.IndexOf("|]", StringComparison.Ordinal);
        Assert.IsGreaterThanOrEqualTo(0, start);
        var span = new TextSpan(start, end - start - 2);
        source = source.Remove(end, 2).Remove(start, 2);
        var tree = CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(LanguageVersion.Preview));
        var references = ((string) AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator)
            .Select(static path => MetadataReference.CreateFromFile(path));
        var compilation = CSharpCompilation.Create("FlowTest", [tree], references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        var errors = compilation.GetDiagnostics().Where(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error).ToArray();
        Assert.IsEmpty(errors, string.Join(Environment.NewLine, errors.Select(static error => error.ToString())));
        var model = compilation.GetSemanticModel(tree);
        var method = tree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().Single(static method => method.Identifier.ValueText == "Run");
        var root = model.GetOperation(method)!;
        var operation = root.DescendantsAndSelf().First(candidate => candidate.Syntax.Span == span);
        var local = root.DescendantsAndSelf().OfType<IVariableDeclaratorOperation>().First(static declarator => declarator.Symbol.Name == "value").Symbol;
        Assert.AreEqual(supported, ReachingDefinitionFlow.TryGetReachingLocalValues(root, operation, local, out var values, out var inherits));
        Assert.AreEqual(incoming, inherits);
        Assert.AreSequenceEqual([.. expected.Split('|', StringSplitOptions.RemoveEmptyEntries).Order(StringComparer.Ordinal)], [.. values.Select(static value => value.Syntax.ToString()).Order(StringComparer.Ordinal)], "Actual definitions: " + string.Join(" | ", values.Select(static value => value.Syntax.ToString())));
    }
}
