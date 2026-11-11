using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleTaskRunCancellationAnalyzerTests
{
    [TestMethod]
    [DataRow("execute")]
    [DataRow("lifecycle")]
    [DataRow("configuration")]
    public async Task Accepts_Immutable_Captured_Token(string root)
    {
        await VerifyCS.VerifyAnalyzerAsync(ModuleSource(root, "", "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [DataRow("execute")]
    [DataRow("lifecycle")]
    [DataRow("configuration")]
    public async Task Accepts_Reading_Captured_Token_Into_A_Copy(string root)
    {
        await VerifyCS.VerifyAnalyzerAsync(ModuleSource(root,
            "CancellationToken copy = default; copy = cancellationToken; _ = copy.CanBeCanceled;",
            "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [DataRow("execute")]
    [DataRow("lifecycle")]
    [DataRow("configuration")]
    public async Task Accepts_Reading_Captured_Token_Into_A_Deconstruction(string root)
    {
        await VerifyCS.VerifyAnalyzerAsync(ModuleSource(root,
            "CancellationToken copy = default; (copy, _) = (cancellationToken, 1); _ = copy.CanBeCanceled;",
            "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [DataRow("execute")]
    [DataRow("lifecycle")]
    [DataRow("configuration")]
    public async Task Reports_Captured_Token_Overwritten_In_Enclosing_Callable(string root)
    {
        await VerifyCS.VerifyAnalyzerAsync(ModuleSource(root,
            "cancellationToken = CancellationToken.None;",
            "{|#0:Task.Delay(1, cancellationToken)|}", mutationInCallback: false),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow("execute", "cancellationToken = CancellationToken.None;")]
    [DataRow("lifecycle", "cancellationToken = CancellationToken.None;")]
    [DataRow("configuration", "cancellationToken = CancellationToken.None;")]
    [DataRow("execute", "Reset(ref cancellationToken);")]
    [DataRow("lifecycle", "Reset(ref cancellationToken);")]
    [DataRow("configuration", "Reset(ref cancellationToken);")]
    [DataRow("execute", "ResetOut(out cancellationToken);")]
    [DataRow("execute", "(cancellationToken, _) = (CancellationToken.None, 1);")]
    [DataRow("execute", "ResetAlias();")]
    public async Task Reports_Potentially_Overwritten_Captured_Token(string root, string mutation)
    {
        await VerifyCS.VerifyAnalyzerAsync(
            ModuleSource(root, mutation, "{|#0:Task.Delay(1, cancellationToken)|}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow("Task.Delay(1)")]
    [DataRow("Task.Delay(1, CancellationToken.None)")]
    [DataRow("Task.Delay(1, new CancellationToken())")]
    public async Task Reports_Missing_Or_Unrelated_Token(string call)
    {
        await VerifyCS.VerifyAnalyzerAsync(ModuleSource("execute", "", "{|#0:" + call + "|}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    private static string ModuleSource(string root, string mutation, string call, bool mutationInCallback = true)
    {
        var body = $$"""
            {
                {{(mutationInCallback ? "" : mutation)}}
                await Task.Run(async () =>
                {
                    {{(mutationInCallback ? mutation : "")}}
                    await {{call}};
                }, cancellationToken);
                {{(root == "configuration" ? "return false;" : "")}}
                {{(mutation == "ResetAlias();" ? "void ResetAlias() { ref var alias = ref cancellationToken; alias = CancellationToken.None; }" : "")}}
            }
            """;
        var entry = root switch
        {
            "execute" => "protected override async Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) " + body,
            "lifecycle" => "protected override async Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken) " + body,
            _ => "protected override void Configure(ModuleConfigurationBuilder module) => module.WithSkipWhen(async (context, cancellationToken) => " + body + ");",
        };
        return $$"""
            {{TestSourceConstants.StandardModuleHeader}}
            public sealed class BuildModule : Module
            {
                {{(root == "execute" ? "" : "protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => Task.CompletedTask;")}}
                {{entry}}
                private static void Reset(ref CancellationToken token) => token = CancellationToken.None;
                private static void ResetOut(out CancellationToken token) => token = CancellationToken.None;
            }
            """;
    }
}
