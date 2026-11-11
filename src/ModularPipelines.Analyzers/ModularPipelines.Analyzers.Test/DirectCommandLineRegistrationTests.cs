using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleRegistrationAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class DirectCommandLineRegistrationTests
{
    [TestMethod]
    [DataRow("Program", false)]
    [DataRow("BuildModule", false)]
    [DataRow("Program", true)]
    [DataRow("BuildModule", true)]
    public async Task Recognizes_Inline_Assembly_Action(string assemblyType, bool asynchronous)
    {
        var action = asynchronous
            ? $"async _ => {{ var builder = Pipeline.CreateBuilder(); builder.AddModulesFromAssembly(typeof({assemblyType}).Assembly); await using var pipeline = await builder.BuildAsync(); }}"
            : $"_ => {{ Pipeline.CreateBuilder().AddModulesFromAssembly(typeof({assemblyType}).Assembly); }}";
        await VerifyAsync($"command.SetAction({action}); return "
            + (asynchronous ? "await command.Parse(args).InvokeAsync();" : "command.Parse(args).Invoke();"), true);
    }

    [TestMethod]
    [DataRow("Func<int, int> identity = x => x;")]
    [DataRow("int Identity(int x) => x;")]
    public async Task Recognizes_Action_With_Nested_Parameters(string callback)
    {
        await VerifyAsync("command.SetAction(_ => { " + callback
            + " Pipeline.CreateBuilder().AddModulesFromAssembly(typeof(Program).Assembly); }); return command.Parse(args).Invoke();", true);
    }

    [TestMethod]
    public async Task Keeps_Diagnostic_For_Captured_Outer_Parameter()
    {
        await VerifyAsync("command.SetAction(_ => { Func<string[]> read = () => args; Pipeline.CreateBuilder().AddModulesFromAssembly(typeof(Program).Assembly); }); return command.Parse(args).Invoke();", false);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("command.Parse(args);")]
    [DataRow("if (args.Length > 0) { command.SetAction(_ => {}); } return command.Parse(args).Invoke();")]
    [DataRow("command.Action = null; return command.Parse(args).Invoke();")]
    [DataRow("return new System.CommandLine.RootCommand().Parse(args).Invoke();")]
    [DataRow("command.SetAction(_ => {}); return command.Parse(args).Invoke();")]
    [DataRow("command = new System.CommandLine.RootCommand(); return command.Parse(args).Invoke();")]
    [DataRow("var alias = command; alias.SetAction(_ => {}); return command.Parse(args).Invoke();")]
    [DataRow("Action mutate = () => command.SetAction(_ => {}); mutate(); return command.Parse(args).Invoke();")]
    [DataRow("Mutate(command); return command.Parse(args).Invoke(); static void Mutate(System.CommandLine.Command c) => c.SetAction(_ => {});")]
    [DataRow("Func<int> invoke = () => command.Parse(args).Invoke();")]
    [DataRow("goto invoke; command.SetAction(_ => {}); invoke: return command.Parse(args).Invoke();")]
    public async Task Keeps_Diagnostic_Without_Proven_Direct_Action(string invocation)
    {
        await VerifyAsync("command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); " + invocation, false);
    }

    [TestMethod]
    [DataRow("if (args.Length > 0) return command.Parse(args).Invoke(); return 0;")]
    [DataRow("while (args.Length > 0) return command.Parse(args).Invoke(); return 0;")]
    [DataRow("return args.Length > 0 ? command.Parse(args).Invoke() : 0;")]
    [DataRow("int? exitCode = 0; exitCode ??= command.Parse(args).Invoke(); return exitCode.Value;")]
    public async Task Keeps_Diagnostic_For_Conditional_Invocation(string invocation)
    {
        await VerifyAsync("command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); " + invocation, false);
    }

    [TestMethod]
    public async Task Keeps_Diagnostic_For_Unreachable_Setter()
    {
        await VerifyAsync("if (false) command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); return command.Parse(args).Invoke();", false);
    }

    [TestMethod]
    public async Task Keeps_Diagnostic_For_Captured_Delegate()
    {
        await VerifyAsync("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); command.SetAction(_ => register()); register = () => {}; return command.Parse(args).Invoke();", false);
    }

    [TestMethod]
    public async Task Keeps_Diagnostic_For_Uninvoked_Nested_Callback()
    {
        await VerifyAsync("command.SetAction(_ => { Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }); return command.Parse(args).Invoke();", false);
    }

    [TestMethod]
    public async Task Keeps_Diagnostic_For_Genuinely_Unregistered_Module()
    {
        await VerifyAsync("command.SetAction(_ => {}); return command.Parse(args).Invoke();", false);
    }

    private static async Task VerifyAsync(string statements, bool registered)
    {
        var test = new VerifyCS.Test
        {
            TestCode = $$"""
                {{TestSourceConstants.StandardUsingsWithExtensions}}
                var command = new System.CommandLine.RootCommand();
                {{statements}}
                public class {{(registered ? "BuildModule" : "{|#0:BuildModule|}")}} : Module<List<string>>
                {
                    {{TestSourceConstants.SimpleAsyncExecuteBody}}
                }
                """,
            ReferenceAssemblies = Net.Net110,
        };
        test.TestState.OutputKind = OutputKind.ConsoleApplication;
        test.TestState.AdditionalReferences.Add(typeof(IModuleContext).Assembly.Location);
        test.TestState.AdditionalReferences.Add(typeof(System.CommandLine.RootCommand).Assembly.Location);
        // Some negative controls intentionally contain unreachable statements or unused locals.
        test.CompilerDiagnostics = CompilerDiagnostics.Errors;
        if (!registered)
        {
            test.ExpectedDiagnostics.Add(VerifyCS.Diagnostic(ModuleRegistrationAnalyzer.UnregisteredModuleId)
                .WithLocation(0).WithArguments("BuildModule"));
        }

        await test.RunAsync(CancellationToken.None);
    }
}
