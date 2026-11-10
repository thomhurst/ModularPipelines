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
