using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleRegistrationAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class CommandLineModuleRegistrationAnalyzerTests
{
    [TestMethod]
    [DataRow("Program", "return await command.Parse(args).InvokeAsync();")]
    [DataRow("BuildModule", "return await command.Parse(args).InvokeAsync();")]
    [DataRow("BuildModule", "var result = command.Parse(args); return await result.InvokeAsync();")]
    [DataRow("BuildModule", "var alias = command; return await alias.Parse(args).InvokeAsync();")]
    public async Task Recognizes_Assembly_Registration_In_Invoked_Action(string assemblyType, string invocation)
    {
        var source = Source($$"""
            command.SetAction(async _ =>
            {
                var builder = Pipeline.CreateBuilder();
                builder.AddModulesFromAssembly(typeof({{assemblyType}}).Assembly);
                await using var pipeline = await builder.BuildAsync();
            });
            {{invocation}}
            """);

        await VerifyAsync(source);
    }

    [TestMethod]
    public async Task Recognizes_Module_Registration_In_Synchronous_Action()
    {
        var source = Source("""
            command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
            return command.Parse(args).Invoke();
            """);

        await VerifyAsync(source);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("command.Parse(args);")]
    [DataRow("await new System.CommandLine.RootCommand().Parse(args).InvokeAsync();")]
    [DataRow("command = new System.CommandLine.RootCommand(); await command.Parse(args).InvokeAsync();")]
    [DataRow("Func<Task<int>> invoke = () => command.Parse(args).InvokeAsync();")]
    public async Task Reports_Module_Registered_Only_In_Uninvoked_Action(string invocation)
    {
        var source = Source($$"""
            command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
            {{invocation}}
            """, registered: false);

        await VerifyAsync(source, UnregisteredModule());
    }

    [TestMethod]
    public async Task Reports_Module_When_Action_Is_Overwritten()
    {
        var source = Source("""
            command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
            command.SetAction(_ => { });
            return command.Parse(args).Invoke();
            """, registered: false);

        await VerifyAsync(source, UnregisteredModule());
    }

    [TestMethod]
    [DataRow("if (args.Length > 0) { command.SetAction(_ => { }); } else { command.SetAction(_ => { }); }", false)]
    [DataRow("if (args.Length > 0) { command.SetAction(_ => { }); }", true)]
    [DataRow("if (args.Length > 0) { if (args.Length > 1) { command.SetAction(_ => { }); } else { command.SetAction(_ => { }); } } else { command.SetAction(_ => { }); }", false)]
    [DataRow("if (args.Length > 0) { command.SetAction(_ => { }); } else { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); }", true)]
    public async Task Tracks_Action_Replacement_Across_Branches(string replacement, bool registered)
    {
        var source = Source($$"""
            command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
            {{replacement}}
            return command.Parse(args).Invoke();
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("if (args.Length > 0) { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); command.SetAction(_ => { }); } else { command.SetAction(_ => { }); }", false)]
    [DataRow("if (args.Length > 0) { command.SetAction(_ => { }); } else { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); command.SetAction(_ => { }); }", false)]
    [DataRow("if (args.Length > 0) { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); if (args.Length > 1) { command.SetAction(_ => { }); } else { command.SetAction(_ => { }); } }", false)]
    [DataRow("if (args.Length > 0) { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); if (args.Length > 1) { command.SetAction(_ => { }); } }", true)]
    [DataRow("if (args.Length > 0) { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); command.SetAction(_ => { }); } else { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); }", true)]
    public async Task Tracks_Action_Replacement_Within_Branches(string statements, bool registered)
    {
        var source = Source($$"""
            {{statements}}
            return command.Parse(args).Invoke();
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("if (args.Length > 0) { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); } else { command.SetAction(_ => { }); command.Parse(args).Invoke(); }", false)]
    [DataRow("if (args.Length > 0) { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); } else { command.Parse(args).Invoke(); }", false)]
    [DataRow("if (args.Length > 1) { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); } else { if (args.Length > 0) { command.Parse(args).Invoke(); } }", false)]
    [DataRow("if (args.Length > 0) { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); command.Parse(args).Invoke(); }", true)]
    [DataRow("if (args.Length > 0) { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); } command.Parse(args).Invoke();", true)]
    [DataRow("foreach (var arg in args) { if (arg.Length > 0) { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); } else { command.Parse(args).Invoke(); } }", true)]
    [DataRow("foreach (var arg in args) { if (arg.Length > 0) { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); } else { command.SetAction(_ => { }); command.Parse(args).Invoke(); } }", false)]
    [DataRow("do { if (args.Length > 0) { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); } else { command.Parse(args).Invoke(); } } while (false);", false)]
    public async Task Tracks_Actions_On_Paths_Compatible_With_Invocation(string statements, bool registered)
    {
        await VerifyAsync(Source(statements, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("return 0;", false)]
    [DataRow("throw new InvalidOperationException();", false)]
    [DataRow("if (args.Length > 1) return 0; else return 1;", false)]
    [DataRow("if (args.Length > 1) return 0;", true)]
    [DataRow("Action exit = () => { return; }; exit();", true)]
    [DataRow("goto Invoke;", true)]
    public async Task Tracks_Actions_In_Branches_That_Can_Reach_Invocation(string exit, bool registered)
    {
        var source = Source($$"""
            if (args.Length > 0)
            {
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                {{exit}}
            }
            Invoke:
            return command.Parse(args).Invoke();
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("return 0;", "finally")]
    [DataRow("throw new InvalidOperationException();", "catch (InvalidOperationException)")]
    public async Task Recognizes_Action_Invoked_By_Exit_Handler(string exit, string handler)
    {
        var source = Source($$"""
            try
            {
                if (args.Length > 0)
                {
                    command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                    {{exit}}
                }
            }
            {{handler}}
            {
                command.Parse(args).Invoke();
            }
            return 0;
            """);

        await VerifyAsync(source);
    }

    [TestMethod]
    [DataRow("", true)]
    [DataRow("return 0;", false)]
    [DataRow("throw;", false)]
    [DataRow("command.SetAction(_ => { });", false)]
    public async Task Tracks_Actions_After_Caught_Exception(string handler, bool registered)
    {
        var source = Source($$"""
            try
            {
                if (args.Length > 0)
                {
                    command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                    throw new InvalidOperationException();
                }
            }
            catch (InvalidOperationException)
            {
                {{handler}}
            }
            return command.Parse(args).Invoke();
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    public async Task Recognizes_Action_After_Unconditional_Caught_Exception()
    {
        await VerifyAsync(Source("""
            try
            {
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                throw new InvalidOperationException();
            }
            catch (InvalidOperationException) { }
            return command.Parse(args).Invoke();
            """));
    }

    [TestMethod]
    [DataRow("InvalidOperationException", "", true)]
    [DataRow("ArgumentException", "", false)]
    [DataRow("InvalidOperationException", "when (true)", true)]
    [DataRow("InvalidOperationException", "when (false)", false)]
    [DataRow("ArgumentException", "when (true)", false)]
    public async Task Matches_Exception_Handler_Before_Later_Invocation(string caught, string filter, bool registered)
    {
        var source = Source($$"""
            try
            {
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                throw new InvalidOperationException();
            }
            catch ({{caught}}) {{filter}} { }
            return command.Parse(args).Invoke();
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("", true)]
    [DataRow("command.SetAction(_ => { });", false)]
    [DataRow("throw new InvalidOperationException();", false)]
    public async Task Tracks_Action_Through_Finally_Before_Later_Invocation(string cleanup, bool registered)
    {
        var source = Source($$"""
            try
            {
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
            }
            finally { {{cleanup}} }
            return command.Parse(args).Invoke();
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    public async Task Recognizes_Action_Invoked_In_Filter_Before_Finally_Replacement()
    {
        await VerifyAsync(Source("""
            try
            {
                try
                {
                    command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                    throw new InvalidOperationException();
                }
                finally { command.SetAction(_ => { }); }
            }
            catch (InvalidOperationException) when (command.Parse(args).Invoke() == 0) { }
            """));
    }

    [TestMethod]
    public async Task Recognizes_Action_When_Thrown_Variable_Has_Base_Type()
    {
        await VerifyAsync(Source("""
            Exception error = new InvalidOperationException();
            try
            {
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                throw error;
            }
            catch (InvalidOperationException) { }
            return command.Parse(args).Invoke();
            """));
    }

    [TestMethod]
    [DataRow("double", "1.0", "/", false)]
    [DataRow("double", "1.0", "%", false)]
    [DataRow("float", "1.0f", "/", false)]
    [DataRow("float", "1.0f", "%", false)]
    [DataRow("double?", "1.0", "/", false)]
    [DataRow("double", "1.0", "+", false)]
    [DataRow("int", "1", "/", true)]
    [DataRow("int", "1", "%", true)]
    [DataRow("int", "1", "+", true)]
    [DataRow("decimal", "1.0m", "/", true)]
    public async Task Tracks_Only_Throwing_Binary_Operations(string type, string value, string operation, bool registered)
    {
        await VerifyAsync(Source($$"""
            {{type}} divisor = 0;
            command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
            try
            {
                _ = checked({{value}} {{operation}} divisor);
                command.SetAction(_ => { });
            }
            catch (Exception) { }
            return command.Parse(args).Invoke();
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("_ = args[0];", "IndexOutOfRangeException")]
    [DataRow("_ = System.Environment.CurrentDirectory;", "Exception")]
    [DataRow("_ = new object();", "Exception")]
    [DataRow("_ = new int[args.Length];", "Exception")]
    [DataRow("_ = 1 / args.Length;", "DivideByZeroException")]
    [DataRow("_ = checked(args.Length + 1);", "OverflowException")]
    [DataRow("_ = (string)(object)args;", "InvalidCastException")]
    public async Task Preserves_Action_When_Operation_Throws_Before_Replacement(string operation, string caught)
    {
        await VerifyAsync(Source($$"""
            try
            {
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                {{operation}}
                command.SetAction(_ => { });
            }
            catch ({{caught}}) { }
            return command.Parse(args).Invoke();
            """));
    }

    [TestMethod]
    [DataRow("_ = args[0];", "IndexOutOfRangeException")]
    [DataRow("_ = System.Environment.CurrentDirectory;", "Exception")]
    [DataRow("_ = new object();", "Exception")]
    public async Task Recognizes_Action_Invoked_In_Operation_Exception_Handler(string operation, string caught)
    {
        await VerifyAsync(Source($$"""
            command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
            try { {{operation}} }
            catch ({{caught}}) { command.Parse(args).Invoke(); }
            """));
    }

    [TestMethod]
    [DataRow("catch", false)]
    [DataRow("catch (Exception)", false)]
    [DataRow("catch (InvalidOperationException)", true)]
    [DataRow("catch (Exception) when (true)", false)]
    [DataRow("catch (Exception) when (false)", true)]
    public async Task Stops_Exception_Search_After_Catch_All(string handler, bool registered)
    {
        await VerifyAsync(Source($$"""
            try
            {
                try
                {
                    command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                    System.GC.KeepAlive(args);
                    command.SetAction(_ => { });
                }
                {{handler}} { return 0; }
            }
            catch { }
            return command.Parse(args).Invoke();
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("true", false)]
    [DataRow("false", true)]
    [DataRow("args.Length > 0", true)]
    public async Task Routes_Rejected_Filter_To_Later_Handler(string filter, bool registered)
    {
        await VerifyAsync(Source($$"""
            try
            {
                try
                {
                    command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                    throw new InvalidOperationException();
                }
                catch (InvalidOperationException) when ({{filter}}) { command.SetAction(_ => { }); }
            }
            catch { }
            return command.Parse(args).Invoke();
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("", true)]
    [DataRow("command.SetAction(_ => { });", false)]
    public async Task Preserves_Finally_Destination_Through_Nested_Catch(string replacement, bool registered)
    {
        await VerifyAsync(Source($$"""
            try { }
            finally
            {
                try { throw new InvalidOperationException(); }
                catch (InvalidOperationException)
                {
                    command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                }
                {{replacement}}
            }
            return command.Parse(args).Invoke();
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("true", false)]
    [DataRow("false", true)]
    public async Task Routes_Rejected_Filter_To_Sibling_Handler(string filter, bool registered)
    {
        await VerifyAsync(Source($$"""
            try
            {
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                throw new InvalidOperationException();
            }
            catch (InvalidOperationException) when ({{filter}}) { command.SetAction(_ => { }); }
            catch (Exception) { }
            return command.Parse(args).Invoke();
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    public async Task Preserves_Outer_Finally_Destination_When_Nested_Finally_Throws()
    {
        await VerifyAsync(Source("""
            try { }
            finally
            {
                try
                {
                    try { } finally { throw new InvalidOperationException(); }
                }
                catch (InvalidOperationException)
                {
                    command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                }
            }
            return command.Parse(args).Invoke();
            """));
    }

    [TestMethod]
    public async Task Discards_Finally_Destination_When_Exception_Escapes()
    {
        await VerifyAsync(Source("""
            try
            {
                try { }
                finally
                {
                    command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                    throw new InvalidOperationException();
                }
                command.Parse(args).Invoke();
            }
            catch (InvalidOperationException) { }
            """, registered: false), UnregisteredModule());
    }

    [TestMethod]
    [DataRow("for (var i = 0; i < 2; i++)", "", "", true)]
    [DataRow("for (var i = 0; i < 2; i++)", "", "break;", false)]
    [DataRow("for (var i = 0; i < 2; i++)", "", "return 0;", false)]
    [DataRow("for (var i = 0; i < 2; i++)", "command.SetAction(_ => { });", "", false)]
    [DataRow("for (var i = 0; i < 2; i++)", "", "command.SetAction(_ => { });", false)]
    [DataRow("do", "", "", false)]
    [DataRow("do", "", "if (args.Length > 0) goto Again;", true)]
    public async Task Tracks_Actions_Installed_After_Invocation(string loop, string beforeInvocation, string afterSetter, bool registered)
    {
        var source = Source($$"""
            command.SetAction(_ => { });
            {{loop}}
            {
                Again:
                {{beforeInvocation}}
                command.Parse(args).Invoke();
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                {{afterSetter}}
            } {{(loop == "do" ? "while (false);" : string.Empty)}}
            return 0;
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("var freshCommand = new System.CommandLine.RootCommand();", "freshCommand", "", false)]
    [DataRow("var freshCommand = new System.CommandLine.RootCommand(); var alias = freshCommand;", "alias", "", false)]
    [DataRow("command = new System.CommandLine.RootCommand();", "command", "", false)]
    [DataRow("var freshCommand = new System.CommandLine.RootCommand();", "freshCommand", "freshCommand.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });", true)]
    [DataRow("var freshCommand = command;", "freshCommand", "", true)]
    public async Task Resets_Actions_When_Loop_Creates_Command(string declaration, string receiver, string beforeInvocation, bool registered)
    {
        await VerifyAsync(Source($$"""
            for (var i = 0; i < 2; i++)
            {
                {{declaration}}
                {{beforeInvocation}}
                {{receiver}}.Parse(args).Invoke();
                {{receiver}}.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
            }
            return 0;
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    public async Task Recognizes_Action_Invoked_Before_Replacement_In_Loop()
    {
        await VerifyAsync(Source("""
            for (var i = 0; i < 2; i++)
            {
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                command.Parse(args).Invoke();
                command.SetAction(_ => { });
            }
            """));
    }

    [TestMethod]
    [DataRow("Action run = () =>", ";")]
    [DataRow("void run()", "")]
    public async Task Recognizes_Action_In_Invoked_Nested_Callable(string declaration, string terminator)
    {
        await VerifyAsync(Source($$"""
            {{declaration}}
            {
                var nestedCommand = new System.CommandLine.RootCommand();
                nestedCommand.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                nestedCommand.Parse(args).Invoke();
            }{{terminator}}
            run();
            """));
    }

    [TestMethod]
    [DataRow("public static Func<int> Run =", true)]
    [DataRow("public static Func<int> Run { get; } =", true)]
    [DataRow("public static Func<int> Run =", false)]
    [DataRow("public static Func<int> Run { get; } =", false)]
    public async Task Tracks_Action_In_Initializer_Lambda(string declaration, bool invoked)
    {
        var source = Source(invoked ? "return Startup.Run();" : "return 0;", registered: invoked) + $$"""

            public static class Startup
            {
                {{declaration}} () =>
                {
                    var command = new System.CommandLine.RootCommand();
                    command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                    return command.Parse(Array.Empty<string>()).Invoke();
                };
            }
            """;

        await VerifyAsync(source, invoked ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("Replace:", false)]
    [DataRow("Replace: Another:", false)]
    [DataRow("", true)]
    public async Task Tracks_Jumps_To_Or_Past_Action_Replacement(string replacementLabel, bool registered)
    {
        var source = Source($$"""
            do
            {
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                if (args.Length > 0) goto Replace;
                {{replacementLabel}}
                command.SetAction(_ => { });
                {{(registered ? "Replace: ;" : string.Empty)}}
            } while (false);
            return command.Parse(args).Invoke();
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("if (args.Length > 0) break;", true)]
    [DataRow("if (args.Length > 0) continue;", true)]
    [DataRow("while (args.Length > 0) { break; }", false)]
    [DataRow("Action skip = () => { return; }; skip();", false)]
    public async Task Tracks_Action_Replacement_Around_Loop_Exits(string interveningStatements, bool registered)
    {
        var source = Source($$"""
            do
            {
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                {{interveningStatements}}
                command.SetAction(_ => { });
            } while (false);
            return command.Parse(args).Invoke();
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    public async Task Recognizes_Action_When_Jump_Skips_Replacement()
    {
        var source = Source("""
            command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
            if (args.Length > 0) goto Invoke;
            command.SetAction(_ => { });
            Invoke:
            return command.Parse(args).Invoke();
            """);

        await VerifyAsync(source);
    }

    [TestMethod]
    public async Task Recognizes_Action_When_Jump_Enters_Invoked_Branch_After_Replacement()
    {
        var source = Source("""
            foreach (var arg in args)
            {
                if (arg.Length > 0)
                {
                    command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                }
                else
                {
                    if (args.Length > 2) goto Invoke;
                    command.SetAction(_ => { });
                    Invoke:
                    command.Parse(args).Invoke();
                }
            }
            """);

        await VerifyAsync(source);
    }

    [TestMethod]
    public async Task Recognizes_Action_When_Branch_Exits_Before_Replacement()
    {
        var source = Source("""
            do
            {
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                if (args.Length > 0)
                {
                    if (args.Length > 1) break;
                    command.SetAction(_ => { });
                }
                else
                {
                    command.SetAction(_ => { });
                }
            } while (false);
            return command.Parse(args).Invoke();
            """);

        await VerifyAsync(source);
    }

    [TestMethod]
    public async Task Recognizes_Action_When_Backward_Jump_Reenters_Opposite_Branch()
    {
        var source = Source("""
            var register = args.Length > 0;
            Again:
            if (register)
            {
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                register = false;
                goto Again;
            }
            else
            {
                command.Parse(args).Invoke();
            }
            """);

        await VerifyAsync(source);
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Tracks_Delegate_Copied_Inside_Action(bool invoked)
    {
        var source = Source($$"""
            Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
            command.SetAction(_ =>
            {
                var copy = register;
                {{(invoked ? "copy();" : string.Empty)}}
            });
            return command.Parse(args).Invoke();
            """, registered: invoked);

        await VerifyAsync(source, invoked ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Tracks_Delegate_Member_Copied_Inside_Action(bool invoked)
    {
        var source = Source($$"""
            command.SetAction(_ =>
            {
                var copy = Callbacks.Register;
                {{(invoked ? "copy();" : string.Empty)}}
            });
            return command.Parse(args).Invoke();
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
            command.SetAction(_ =>
            {
                Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
                {{(invoked ? "register();" : string.Empty)}}
            });
            return command.Parse(args).Invoke();
            """, registered: invoked);

        await VerifyAsync(source, invoked ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    public async Task Recognizes_Action_Stored_In_Delegate_Local()
    {
        var source = Source("""
            Action<System.CommandLine.ParseResult> action = Register;
            command.SetAction(action);
            return command.Parse(args).Invoke();

            void Register(System.CommandLine.ParseResult result) =>
                Pipeline.CreateBuilder().AddModule<BuildModule>();
            """);

        await VerifyAsync(source);
    }

    [TestMethod]
    public async Task Invoked_Action_Does_Not_Hide_Unregistered_Module()
    {
        var source = Source("""
            command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
            return command.Parse(args).Invoke();
            """) + $$"""

            public class {|#0:UnregisteredModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }
            """;

        await VerifyAsync(source, VerifyCS.Diagnostic(ModuleRegistrationAnalyzer.UnregisteredModuleId)
            .WithLocation(0).WithArguments("UnregisteredModule"));
    }

    private static string Source(string statements, bool registered = true) => $$"""
        {{TestSourceConstants.StandardUsingsWithExtensions}}

        var command = new System.CommandLine.RootCommand();
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
        test.TestState.AdditionalReferences.Add(typeof(System.CommandLine.RootCommand).Assembly.Location);
        test.ExpectedDiagnostics.AddRange(expected);
        await test.RunAsync(CancellationToken.None);
    }
}
