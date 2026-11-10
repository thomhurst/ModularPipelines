using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleRegistrationAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class CallbackCaptureAnalyzerTests
{
    [TestMethod]
    [DataRow("new Holder()", "NullReferenceException", false)]
    [DataRow("null!", "InvalidOperationException", false)]
    [DataRow("null!", "NullReferenceException", true)]
    public async Task Tracks_Field_Exception_Paths_For_Callback_Definitions(string receiver, string caught, bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = parseResult =>
            {
                Holder holder = {{receiver}};
                Action register = () => {};
                try { _ = holder.Value; }
                catch ({{caught}}) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }
                register();
            };
            callback(null!); return 0;

            public class Holder { public int Value; }
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("InvalidOperationException", false)]
    [DataRow("IndexOutOfRangeException", true)]
    [DataRow("NullReferenceException", true)]
    [DataRow("Exception", true)]
    public async Task Matches_Array_Exception_Type_For_Callback_Definitions(string caught, bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = parseResult =>
            {
                Action register = () => {};
                try { _ = args[0]; }
                catch ({{caught}}) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }
                register();
            };
            callback(null!); return 0;
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("ref", "object", "args", true)]
    [DataRow("out", "object", "args", true)]
    [DataRow("in", "object", "args", false)]
    [DataRow("", "object", "args", false)]
    [DataRow("ref", "string", "args", false)]
    [DataRow("out", "string", "args", false)]
    [DataRow("ref", "int", "new int[1]", false)]
    [DataRow("out", "int", "new int[1]", false)]
    [DataRow("ref", "object", "new object[1]", false)]
    [DataRow("out", "object", "new object[1]", false)]
    public async Task Array_Argument_Checks_Precede_Later_Argument_Evaluation(
        string modifier, string elementType, string values, bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = parseResult =>
            {
                {{elementType}}[] values = {{values}};
                Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
                try
                {
                    Touch({{modifier}} values[0], register = () => {});
                    register = () => {};
                }
                catch (ArrayTypeMismatchException) { }
                register();
            };
            callback(null!); return 0;

            void Touch({{modifier}} {{elementType}} value, Action replacement)
            {
                {{(modifier == "out" ? "value = default!;" : string.Empty)}}
            }
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("object[] original = new object[1]; object[] values = original;", false)]
    [DataRow("object[] values = args; values = new object[1];", false)]
    [DataRow("object[] values = new object[1]; values = args;", true)]
    [DataRow("object[] values; if (args.Length > 0) values = new object[1]; else values = new object[2];", false)]
    [DataRow("object[] values = new object[1]; if (args.Length > 0) values = args;", true)]
    [DataRow("object[] original = args; object[] values = original;", true)]
    [DataRow("object[] values = new object[1]; Replace(ref values);", true)]
    [DataRow("object[] values = new object[1]; ref object[] alias = ref values; alias = args;", true)]
    [DataRow("object[] values = new object[1]; object[] other = new object[1]; ref object[] alias = ref other; alias = ref values; alias = args;", true)]
    [DataRow("object[] values = new object[1]; Action replace = () => values = args; replace();", true)]
    [DataRow("object[] values = new object[1]; (values, _) = (args, 0);", true)]
    public async Task Tracks_Runtime_Array_Types_At_Writable_Arguments(string declarations, bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = parseResult =>
            {
                {{declarations}}
                Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
                try
                {
                    Touch(ref values[0], register = () => {});
                    register = () => {};
                }
                catch (ArrayTypeMismatchException) { }
                register();
            };
            callback(null!); return 0;

            void Touch(ref object value, Action replacement) { }
            void Replace(ref object[] array) { array = args; }
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Tracks_Delegate_Copied_Inside_Action(bool invoked)
    {
        var source = Source($$"""
            Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
            callback = _ =>
            {
                var copy = register;
                {{(invoked ? "copy();" : string.Empty)}}
            };
            callback(null!); return 0;
            """, registered: invoked);

        await VerifyAsync(source, invoked ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Tracks_Delegate_Member_Copied_Inside_Action(bool invoked)
    {
        var source = Source($$"""
            callback = _ =>
            {
                var copy = Callbacks.Register;
                {{(invoked ? "copy();" : string.Empty)}}
            };
            callback(null!); return 0;
            """, registered: invoked) + """

            public static class Callbacks
            {
                public static Action Register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
            }
            """;

        await VerifyAsync(source, invoked ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Tracks_Nested_Callback_Invocation_Inside_Action(bool invoked)
    {
        var source = Source($$"""
            callback = _ =>
            {
                Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
                {{(invoked ? "register();" : string.Empty)}}
            };
            callback(null!); return 0;
            """, registered: invoked);

        await VerifyAsync(source, invoked ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow(true, false)]
    [DataRow(false, true)]
    public async Task Tracks_Captured_Delegate_Reassigned_After_Action_Installation(bool initiallyRegistered, bool registered)
    {
        var source = Source($$"""
            Action register = {{(initiallyRegistered ? "() => Pipeline.CreateBuilder().AddModule<BuildModule>()" : "() => {}")}};
            callback = _ => register();
            register = {{(registered ? "() => Pipeline.CreateBuilder().AddModule<BuildModule>()" : "() => {}")}};
            callback(null!); return 0;
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("callback = _ => { var copy = register; copy(); };")]
    [DataRow("callback = _ => { Action inner = () => register(); inner(); };")]
    [DataRow("callback = _ => { void Inner() => register(); Inner(); };")]
    public async Task Tracks_Reassigned_Delegate_Through_Nested_Callbacks(string install)
    {
        var source = Source($$"""
            Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
            {{install}}
            register = () => {};
            callback(null!); return 0;
            """, registered: false);

        await VerifyAsync(source, UnregisteredModule());
    }

    [TestMethod]
    [DataRow("Action action = () => register();", "action();")]
    [DataRow("void Action() => register();", "Action();")]
    public async Task Tracks_Reassigned_Delegate_Through_Direct_Callable(string declaration, string invocation)
    {
        var source = Source($$"""
            Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
            {{declaration}}
            register = () => {};
            {{invocation}}
            """, registered: false);

        await VerifyAsync(source, UnregisteredModule());
    }

    [TestMethod]
    public async Task Tracks_Captured_Delegate_At_Each_Execution()
    {
        await VerifyAsync(Source("""
            Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
            callback = _ => register();
            callback(null!);
            register = () => {};
            callback(null!); return 0;
            """));
    }

    [TestMethod]
    public async Task Preserves_Delegate_Value_Copied_Before_Reassignment()
    {
        await VerifyAsync(Source("""
            Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
            var copy = register;
            callback = _ => copy();
            register = () => {};
            callback(null!); return 0;
            """));
    }

    [TestMethod]
    [DataRow(false, false, false)]
    [DataRow(false, true, true)]
    [DataRow(true, false, true)]
    [DataRow(true, true, true)]
    public async Task Tracks_Captured_Delegate_Replaced_In_Every_Branch(bool firstRegisters, bool secondRegisters, bool registered)
    {
        var source = Source($$"""
            Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
            callback = _ =>
            {
                if (args.Length > 0)
                    register = {{(firstRegisters ? "() => Pipeline.CreateBuilder().AddModule<BuildModule>()" : "() => {}")}};
                else
                    register = {{(secondRegisters ? "() => Pipeline.CreateBuilder().AddModule<BuildModule>()" : "() => {}")}};
                register();
            };
            callback(null!); return 0;
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow(false, false, false)]
    [DataRow(false, true, true)]
    [DataRow(true, false, true)]
    [DataRow(true, true, true)]
    public async Task Tracks_Captured_Delegate_Replaced_In_One_Branch(bool initiallyRegisters, bool replacementRegisters, bool registered)
    {
        var source = Source($$"""
            Action register = {{(initiallyRegisters ? "() => Pipeline.CreateBuilder().AddModule<BuildModule>()" : "() => {}")}};
            callback = _ =>
            {
                if (args.Length > 0)
                    register = {{(replacementRegisters ? "() => Pipeline.CreateBuilder().AddModule<BuildModule>()" : "() => {}")}};
                register();
            };
            callback(null!); return 0;
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("return;", false)]
    [DataRow("throw new InvalidOperationException();", false)]
    [DataRow("if (args.Length > 1) return; else return;", false)]
    [DataRow("if (args.Length > 1) return;", true)]
    [DataRow("goto End;", false)]
    [DataRow("goto Invoke;", true)]
    public async Task Excludes_Captured_Delegate_Writes_That_Cannot_Reach_Invocation(string exit, bool registered)
    {
        var source = Source($$"""
            Action register = () => {};
            callback = _ =>
            {
                if (args.Length > 0)
                {
                    register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
                    {{exit}}
                }
                Invoke:
                register();
                End:;
            };
            callback(null!); return 0;
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    public async Task Excludes_Delegate_Writes_In_An_Exiting_Top_Level_Branch()
    {
        var source = Source("""
            Action register = () => {};
            if (args.Length > 0)
            {
                register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
                return 0;
            }
            register();
            return 0;
            """, registered: false);

        await VerifyAsync(source, UnregisteredModule());
    }

    [TestMethod]
    [DataRow("try { if (args.Length > 0) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); throw new InvalidOperationException(); } } catch (InvalidOperationException) { } register();", false, true)]
    [DataRow("try { if (args.Length > 0) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); return; } } finally { register(); }", false, true)]
    [DataRow("try { if (args.Length > 0) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); return; } } finally { register = () => {}; } register();", false, false)]
    [DataRow("if (args.Length > 0) { register = () => {}; return; } register();", true, true)]
    [DataRow("do { if (args.Length > 0) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); continue; } register(); } while (false);", false, false)]
    [DataRow("do { if (args.Length > 0) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); break; } register(); } while (false);", false, false)]
    [DataRow("while (args.Length > 0) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); break; } register();", false, true)]
    [DataRow("if (args.Length > 0) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); } else { register(); }", false, false)]
    [DataRow("for (var i = 0; i < 2; i++) { register(); register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }", false, true)]
    public async Task Tracks_Captured_Delegate_Definitions_Through_Control_Flow(string statements, bool initiallyRegisters, bool registered)
    {
        var source = Source($$"""
            Action register = {{(initiallyRegisters ? "() => Pipeline.CreateBuilder().AddModule<BuildModule>()" : "() => {}")}};
            callback = _ =>
            {
                {{statements}}
            };
            callback(null!); return 0;
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow(false, false)]
    [DataRow(false, true)]
    [DataRow(true, false)]
    [DataRow(true, true)]
    [Timeout(10000, CooperativeCancellation = true)]
    public async Task Terminates_Captured_Delegate_Lookup_In_Recursive_Callables(bool mutualRecursion, bool registered)
    {
        var source = Source($$"""
            Action register = {{(registered ? "() => Pipeline.CreateBuilder().AddModule<BuildModule>()" : "() => {}")}};
            void Run(int depth)
            {
                if (depth > 0) {{(mutualRecursion ? "Other(depth - 1);" : "Run(depth - 1);")}}
                register();
            }
            {{(mutualRecursion ? "void Other(int depth) { if (depth > 0) Run(depth - 1); register(); }" : string.Empty)}}
            Run(2);
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("register = () => {}; if (args.Length > 0) register = () => {};")]
    [DataRow("switch (args.Length) { case 0: register = () => {}; break; default: register = () => {}; break; }")]
    [DataRow("if (args.Length > 0) { if (args.Length > 1) register = () => {}; else register = () => {}; } else register = () => {};")]
    public async Task Does_Not_Resurrect_Captured_Delegate_After_Guaranteed_Replacement(string replacement)
    {
        var source = Source($$"""
            Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
            callback = _ =>
            {
                {{replacement}}
                register();
            };
            callback(null!); return 0;
            """, registered: false);

        await VerifyAsync(source, UnregisteredModule());
    }

    [TestMethod]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); return _ => register();", true, true)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); return _ => register();", true, false)]
    [DataRow("Action register = () => {}; return _ => register();", false, true)]
    [DataRow("Action register = () => {}; return _ => register();", false, false)]
    [DataRow("Action register = () => {}; Action<object> action = _ => register(); register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); return action;", true, true)]
    [DataRow("Action register = () => {}; Action<object> action = _ => register(); register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); return action;", true, false)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); Action<object> action = _ => register(); register = () => {}; return action;", false, true)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); Action<object> action = _ => register(); register = () => {}; return action;", false, false)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); try { return _ => register(); } finally { register = () => {}; }", false, true)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); try { return _ => register(); } finally { register = () => {}; }", false, false)]
    [DataRow("Action register = () => {}; try { return _ => register(); } finally { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }", true, true)]
    [DataRow("Action register = () => {}; try { return _ => register(); } finally { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }", true, false)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); Action<object> action = _ => register(); try { return action; } finally { register = () => {}; }", false, true)]
    [DataRow("Action register = () => {}; Action<object> action = _ => register(); try { return action; } finally { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }", true, false)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); try { try { return _ => register(); } finally { register = () => {}; } } finally { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }", true, true)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); try { try { return _ => register(); } finally { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); } } finally { register = () => {}; }", false, false)]
    [DataRow("Action register = () => {}; try { return _ => register(); } finally { if (Environment.TickCount > 0) register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }", true, true)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); try { return _ => register(); } finally { if (Environment.TickCount > 0) register = () => {}; }", true, false)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); try { try { return _ => register(); } finally { throw new InvalidOperationException(); } } catch (InvalidOperationException) { return _ => {}; }", false, true)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); try { return _ => register(); } finally { try { throw new InvalidOperationException(); } catch (InvalidOperationException) { register = () => {}; } }", false, false)]
    public async Task Preserves_Captures_In_Returned_Actions(string factoryBody, bool registered, bool localFactory)
    {
        var factory = $$"""
            public static class Factory
            {
                public static Action<object> CreateAction()
                {
                    {{factoryBody}}
                }
            }
            """;
        if (localFactory)
        {
            factory = $$"""
                static Action<object> CreateAction()
                {
                    {{factoryBody}}
                }
                """;
        }

        await VerifyAsync(Source($$"""
            callback = {{(localFactory ? "CreateAction()" : "Factory.CreateAction()")}};
            callback(null!); return 0;

            {{factory}}
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("ArgumentException", "", false)]
    [DataRow("InvalidOperationException", "", true)]
    [DataRow("Exception", "", true)]
    [DataRow("ArgumentException", "try { throw new ArgumentException(); } catch (ArgumentException) { }", false)]
    [DataRow("InvalidOperationException", "try { throw new ArgumentException(); } catch (ArgumentException) { }", true)]
    [DataRow("ArgumentException", "try { throw; } catch (InvalidOperationException) { throw; }", false)]
    [DataRow("InvalidOperationException", "try { throw; } catch (InvalidOperationException) { throw; }", true)]
    public async Task Preserves_Active_Exception_Type_Across_Rethrows(string caught, string nestedHandler, bool registered)
    {
        await VerifyAsync(Source($$"""
            Action register = () => {};
            try
            {
                try
                {
                    register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
                    throw new InvalidOperationException();
                }
                catch (InvalidOperationException)
                {
                    {{nestedHandler}}
                    throw;
                }
            }
            catch ({{caught}}) { register(); }
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("ref Holder alias = ref (args.Length > 0 ? ref holder : ref other); alias = null!;")]
    [DataRow("ref Holder alias = ref (args.Length > 0 ? ref other : ref holder); alias = null!;")]
    [DataRow("ref Holder alias = ref other; alias = ref (args.Length > 0 ? ref holder : ref other); alias = null!;")]
    [DataRow("ref Holder alias = ref other; alias = ref (args.Length > 0 ? ref other : ref holder); alias = null!;")]
    [DataRow("Clear(ref (args.Length > 0 ? ref holder : ref other));")]
    [DataRow("Clear(ref (args.Length > 0 ? ref other : ref holder));")]
    public async Task Conditional_Ref_Aliases_Invalidate_NonNull_Receiver_Proof(string mutation)
    {
        await VerifyAsync(Source($$"""
            Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
            #nullable disable
            var holder = new Holder();
            var other = new Holder();
            {{mutation}}
            try
            {
                _ = holder.Value;
                register = () => {};
            }
            catch (NullReferenceException) { }
            register();
            static void Clear(ref Holder value) => value = null!;

            public class Holder { public int Value; }
            """));
    }

    [TestMethod]
    [DataRow("ref object[] alias = ref (args.Length > 0 ? ref values : ref other); alias = new string[1];")]
    [DataRow("ref object[] alias = ref (args.Length > 0 ? ref other : ref values); alias = new string[1];")]
    [DataRow("ref object[] alias = ref other; alias = ref (args.Length > 0 ? ref values : ref other); alias = new string[1];")]
    [DataRow("ref object[] alias = ref other; alias = ref (args.Length > 0 ? ref other : ref values); alias = new string[1];")]
    [DataRow("Replace(ref (args.Length > 0 ? ref values : ref other));")]
    [DataRow("Replace(ref (args.Length > 0 ? ref other : ref values));")]
    public async Task Conditional_Ref_Aliases_Invalidate_Exact_Array_Type_Proof(string mutation)
    {
        await VerifyAsync(Source($$"""
            Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
            object[] values = new object[1];
            object[] other = new object[1];
            var item = new object();
            {{mutation}}
            try
            {
                values[0] = item;
                register = () => {};
            }
            catch (ArrayTypeMismatchException) { }
            register();
            static void Replace(ref object[] value) => value = new string[1];
            """));
    }

    private static string Source(string statements, bool registered = true) => $$"""
        {{TestSourceConstants.StandardUsingsWithExtensions}}

        Action<object> callback = _ => {};
        {{statements}}

        public class {{(registered ? "BuildModule" : "{|#0:BuildModule|}")}} : Module<List<string>>
        {
            {{TestSourceConstants.SimpleAsyncExecuteBody}}
        }
        """;

    private static DiagnosticResult UnregisteredModule() =>
        VerifyCS.Diagnostic(ModuleRegistrationAnalyzer.UnregisteredModuleId)
            .WithLocation(0).WithArguments("BuildModule");

    private static async Task VerifyAsync(string source, params DiagnosticResult[] expected)
    {
        var test = new VerifyCS.Test
        {
            TestCode = source,
            ReferenceAssemblies = Net.Net110,
        };
        test.TestState.OutputKind = OutputKind.ConsoleApplication;
        test.TestState.AdditionalReferences.Add(typeof(IModuleContext).Assembly.Location);
        test.ExpectedDiagnostics.AddRange(expected);
        await test.RunAsync(CancellationToken.None);
    }
}
