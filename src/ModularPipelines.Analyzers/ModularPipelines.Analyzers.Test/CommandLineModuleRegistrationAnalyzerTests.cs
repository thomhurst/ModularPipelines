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
