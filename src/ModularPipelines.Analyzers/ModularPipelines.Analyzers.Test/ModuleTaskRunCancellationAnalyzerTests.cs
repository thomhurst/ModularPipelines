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
    public async Task Accepts_ReadOnly_Ref_Aliases(string root)
    {
        await VerifyCS.VerifyAnalyzerAsync(ModuleSource(root,
            "ReadAlias(); void ReadAlias() { ref readonly var alias = ref cancellationToken; _ = alias.CanBeCanceled; }",
            "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [DataRow("execute")]
    [DataRow("lifecycle")]
    [DataRow("configuration")]
    public async Task Accepts_ReadOnly_Ref_Reassignment(string root)
    {
        await VerifyCS.VerifyAnalyzerAsync(ModuleSource(root,
            "ReadAlias(); void ReadAlias() { CancellationToken other = default; ref readonly var alias = ref other; alias = ref cancellationToken; _ = alias.CanBeCanceled; }",
            "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [DataRow("var values = new int[1]; values[cancellationToken.GetHashCode() & 0] = 1;")]
    [DataRow("var values = new int[1]; (values[cancellationToken.GetHashCode() & 0], _) = (1, 2);")]
    [DataRow("var values = new CancellationToken[1]; Reset(ref values[cancellationToken.GetHashCode() & 0]);")]
    [DataRow("var seen = new System.Collections.Generic.Dictionary<CancellationToken, bool>(); seen[cancellationToken] = true;")]
    public async Task Accepts_Reading_Captured_Token_In_Assignment_Or_Ref_Index(string statement)
    {
        await VerifyCS.VerifyAnalyzerAsync(ModuleSource("execute", statement,
            "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [DataRow("execute")]
    [DataRow("lifecycle")]
    [DataRow("configuration")]
    public async Task Reports_Captured_Token_Written_After_Callback_Completes(string root)
    {
        await VerifyCS.VerifyAnalyzerAsync(ModuleSource(root, "",
            "{|#0:Task.Delay(1, cancellationToken)|}",
            afterCallback: "cancellationToken = CancellationToken.None;"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
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
    [DataRow("execute", "((_, cancellationToken), _) = ((1, CancellationToken.None), 2);")]
    [DataRow("execute", "ResetAlias();")]
    [DataRow("execute", "ResetConditionalAlias();")]
    [DataRow("execute", "ResetReassignedAlias();")]
    [DataRow("execute", "ResetReassignedOutAlias(out var copy);")]
    [DataRow("lifecycle", "ResetReassignedOutAlias(out var copy);")]
    [DataRow("configuration", "ResetReassignedOutAlias(out var copy);")]
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

    private static string ModuleSource(string root, string mutation, string call, bool mutationInCallback = true, string afterCallback = "")
    {
        var body = $$"""
            {
                {{(mutationInCallback ? "" : mutation)}}
                await Task.Run(async () =>
                {
                    {{(mutationInCallback ? mutation : "")}}
                    await {{call}};
                }, cancellationToken);
                {{afterCallback}}
                {{(root == "configuration" ? "return false;" : "")}}
                {{(mutation == "ResetReassignedOutAlias(out var copy);" ? "void ResetReassignedOutAlias(out CancellationToken alias) { alias = default; alias = ref cancellationToken; alias = CancellationToken.None; }" : "")}}
                {{(mutation == "ResetReassignedAlias();" ? "void ResetReassignedAlias() { CancellationToken other = default; ref var alias = ref other; alias = ref cancellationToken; alias = CancellationToken.None; }" : "")}}
                {{(mutation == "ResetAlias();" ? "void ResetAlias() { ref var alias = ref cancellationToken; alias = CancellationToken.None; }" : "")}}
                {{(mutation == "ResetConditionalAlias();" ? "void ResetConditionalAlias() { CancellationToken other = default; ref var alias = ref (cancellationToken.CanBeCanceled ? ref cancellationToken : ref other); alias = CancellationToken.None; }" : "")}}
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
