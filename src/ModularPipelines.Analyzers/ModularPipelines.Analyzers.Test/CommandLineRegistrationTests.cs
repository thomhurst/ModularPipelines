using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleRegistrationAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

/// <summary>
/// MP0013 counts a registration wherever it appears, so registrations inside
/// System.CommandLine actions need no knowledge of how the action is invoked.
/// </summary>
[TestClass]
public class CommandLineRegistrationTests
{
    [TestMethod]
    [DataRow("Program", false)]
    [DataRow("BuildModule", false)]
    [DataRow("Program", true)]
    [DataRow("BuildModule", true)]
    public async Task Recognizes_Assembly_Registration_In_Action(string assemblyType, bool asynchronous)
    {
        var action = asynchronous
            ? $"async _ => {{ var builder = Pipeline.CreateBuilder(); builder.AddModulesFromAssembly(typeof({assemblyType}).Assembly); await using var pipeline = await builder.BuildAsync(); }}"
            : $"_ => {{ Pipeline.CreateBuilder().AddModulesFromAssembly(typeof({assemblyType}).Assembly); }}";
        await VerifyAsync($"command.SetAction({action}); return "
            + (asynchronous ? "await command.Parse(args).InvokeAsync();" : "command.Parse(args).Invoke();"), true);
    }

    [TestMethod]
    public async Task Recognizes_Registration_In_Subcommand_Action()
    {
        await VerifyAsync(
            """
            var child = new System.CommandLine.Command("build");
            command.Subcommands.Add(child);
            child.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
            return command.Parse(new[] { "build" }).Invoke();
            """,
            true);
    }

    [TestMethod]
    [DataRow("Action run = () => { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); command.Parse(args).Invoke(); }; run();")]
    [DataRow("Run(); void Run() { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); command.Parse(args).Invoke(); }")]
    public async Task Recognizes_Registration_When_Nested_Callable_Captures_Command(string statements)
    {
        await VerifyAsync(statements, true);
    }

    [TestMethod]
    public async Task Recognizes_Registration_When_Replacement_Action_Throws()
    {
        await VerifyAsync(
            """
            command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
            try
            {
                command.SetAction((Action<System.CommandLine.ParseResult>) null!);
            }
            catch (ArgumentNullException)
            {
            }

            return command.Parse(args).Invoke();
            """,
            true);
    }

    [TestMethod]
    [DataRow("command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });")]
    [DataRow("command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); command.SetAction(_ => {}); return command.Parse(args).Invoke();")]
    public async Task Does_Not_Analyse_Whether_Action_Runs(string statements)
    {
        await VerifyAsync(statements, true);
    }

    [TestMethod]
    public async Task Reports_Module_No_Action_Registers()
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
        if (!registered)
        {
            test.ExpectedDiagnostics.Add(VerifyCS.Diagnostic(ModuleRegistrationAnalyzer.UnregisteredModuleId)
                .WithLocation(0).WithArguments("BuildModule"));
        }

        await test.RunAsync(CancellationToken.None);
    }
}
