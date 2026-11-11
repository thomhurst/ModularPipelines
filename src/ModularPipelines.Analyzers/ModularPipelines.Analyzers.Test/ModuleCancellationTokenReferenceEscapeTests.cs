using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleCancellationTokenReferenceEscapeTests
{
    [TestMethod]
    [DataRow("Task<CancellationToken>", "Task.FromResult(cancellationToken)", "default!")]
    [DataRow("ValueTask<CancellationToken>", "new ValueTask<CancellationToken>(cancellationToken)", "default")]
    [DataRow("Task<CancellationToken>", "Task.FromResult(cancellationToken)", "Task.FromResult(CancellationToken.None)")]
    public async Task Writable_Reference_Invalidates_Awaitable_Payload(string type, string initial, string replacement)
    {
        var source = Source($"var saved = {initial}; ref {type} alias = ref saved; alias = {replacement}; cancellationToken = await saved;", true);
        await VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow("var saved = (Task.FromResult(cancellationToken), 1); ref Task<CancellationToken> alias = ref saved.Item1; alias = default!; cancellationToken = await saved.Item1;")]
    [DataRow("var saved = Task.FromResult(cancellationToken); if (context != null) { ref Task<CancellationToken> alias = ref saved; alias = default!; } cancellationToken = await saved;")]
    [DataRow("var saved = Task.FromResult(cancellationToken); ref Task<CancellationToken> alias = ref saved; saved = default!; cancellationToken = await alias;")]
    [DataRow("var saved = Task.FromResult(cancellationToken); ref readonly Task<CancellationToken> alias = ref saved; saved = default!; cancellationToken = await alias;")]
    public async Task Reference_Escapes_Invalidate_Tuple_And_Branch_Payloads(string statements)
    {
        await VerifyCS.VerifyAnalyzerAsync(Source(statements, true),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    public async Task Reference_Escape_Remains_Unsafe_After_Direct_Reassignment()
    {
        var source = Source("var saved = Task.FromResult(cancellationToken); ref Task<CancellationToken> alias = ref saved; saved = Task.FromResult(cancellationToken); alias = default!; cancellationToken = await saved;", true);
        await VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow("ref CancellationToken alias = ref cancellationToken; alias = CancellationToken.None;")]
    [DataRow("CancellationToken other = default; ref CancellationToken alias = ref other; alias = ref cancellationToken; alias = CancellationToken.None;")]
    [DataRow("var saved = cancellationToken; ref CancellationToken alias = ref saved; alias = CancellationToken.None; cancellationToken = saved;")]
    public async Task Writable_Scalar_References_Invalidate_Token_Provenance(string statements)
    {
        await VerifyCS.VerifyAnalyzerAsync(Source(statements, true),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    public async Task Readonly_Reference_Preserves_Awaitable_Payload()
    {
        await VerifyCS.VerifyAnalyzerAsync(Source("var saved = Task.FromResult(cancellationToken); ref readonly Task<CancellationToken> alias = ref saved; cancellationToken = await saved;", false));
    }

    [TestMethod]
    [DataRow("var saved = (cancellationToken, 1); ref int count = ref saved.Item2; count = 2; cancellationToken = saved.Item1;")]
    [DataRow("var saved = (cancellationToken, cancellationToken); ref CancellationToken alias = ref saved.Item2; alias = default; cancellationToken = saved.Item1;")]
    [DataRow("var saved = ((cancellationToken, cancellationToken), 1); ref CancellationToken alias = ref saved.Item1.Item2; alias = default; cancellationToken = saved.Item1.Item1;")]
    [DataRow("var saved = (Task.FromResult(cancellationToken), 1); if (context != null) { ref int alias = ref saved.Item2; alias = 2; } cancellationToken = await saved.Item1;")]
    public async Task Tuple_Field_References_Preserve_Sibling_Provenance(string statements)
    {
        await VerifyCS.VerifyAnalyzerAsync(Source(statements, false));
    }

    [TestMethod]
    [DataRow("var saved = (cancellationToken, 1); ref CancellationToken alias = ref saved.Item1; saved = (cancellationToken, 2); alias = default; cancellationToken = saved.Item1;")]
    [DataRow("var saved = ((cancellationToken, cancellationToken), 1); ref CancellationToken alias = ref saved.Item1.Item2; saved.Item1 = (cancellationToken, cancellationToken); alias = default; cancellationToken = saved.Item1.Item2;")]
    [DataRow("var saved = (cancellationToken, 1); if (context != null) { ref CancellationToken alias = ref saved.Item1; alias = default; } cancellationToken = saved.Item1;")]
    public async Task Escaped_Tuple_Fields_Remain_Unproven(string statements)
    {
        await VerifyCS.VerifyAnalyzerAsync(Source(statements, true),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay"));
    }

    private static string Source(string statements, bool warns) => $$"""
        {{TestSourceConstants.StandardModuleHeader}}
        #nullable disable
        public class BuildModule : Module<List<string>>
        {
            {{TestSourceConstants.SimpleAsyncExecuteBody}}
            protected override async Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            {
                {{statements}}
                await {{(warns ? "{|#0:Task.Delay(1, cancellationToken)|}" : "Task.Delay(1, cancellationToken)")}};
            }
        }
        """;
}
