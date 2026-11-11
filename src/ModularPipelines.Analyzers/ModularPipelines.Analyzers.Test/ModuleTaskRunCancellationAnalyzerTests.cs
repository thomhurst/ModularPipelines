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
    public async Task Reports_Potentially_Modified_Captured_Token(string root)
    {
        string[] mutations =
        [
            "cancellationToken = CancellationToken.None;",
            "Replace(ref cancellationToken);",
            "Replace(out cancellationToken);",
            "ref CancellationToken alias = ref cancellationToken; alias = CancellationToken.None;",
            "(cancellationToken, _) = (CancellationToken.None, 1);",
        ];
        foreach (var mutation in mutations)
        {
            await VerifyCS.VerifyAnalyzerAsync(
                Source(root, mutation, "{|#0:Task.Delay(1, cancellationToken)|}"),
                MissingToken());
        }
    }

    [TestMethod]
    [DataRow("execute")]
    [DataRow("lifecycle")]
    [DataRow("configuration")]
    public async Task Accepts_Immutable_Captured_Token(string root)
    {
        await VerifyCS.VerifyAnalyzerAsync(Source(root, "", "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [DataRow("execute")]
    [DataRow("lifecycle")]
    [DataRow("configuration")]
    public async Task Reports_Missing_Or_Unrelated_Token(string root)
    {
        string[] calls = ["Task.Delay(1)", "Task.Delay(1, CancellationToken.None)", "Task.Delay(1, unrelated)"];
        foreach (var call in calls)
        {
            await VerifyCS.VerifyAnalyzerAsync(
                Source(root, "CancellationToken unrelated = default;", "{|#0:" + call + "|}"),
                MissingToken());
        }
    }

    private static Microsoft.CodeAnalysis.Testing.DiagnosticResult MissingToken() =>
        VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");

    private static string Source(string root, string mutation, string call)
    {
        var body = $$"""
            {{mutation}}
            await Task.Run(async () => await {{call}}, cancellationToken);
            """;
        var entry = root switch
        {
            "execute" => $$"""
                protected override async Task<List<string>> ExecuteAsync(
                    IModuleContext context, CancellationToken cancellationToken)
                {
                    {{body}}
                    return null!;
                }
                """,
            "lifecycle" => $$"""
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
                protected override async Task OnBeforeExecuteAsync(
                    IModuleContext context, CancellationToken cancellationToken)
                {
                    {{body}}
                }
                """,
            "configuration" => $$"""
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
                protected override void Configure(ModuleConfigurationBuilder module) =>
                    module.WithSkipWhen(async (context, cancellationToken) =>
                    {
                        {{body}}
                        return false;
                    });
                """,
            _ => throw new ArgumentOutOfRangeException(nameof(root)),
        };
        return $$"""
            {{TestSourceConstants.StandardModuleHeaderWithExtensions}}

            public class BuildModule : Module<List<string>>
            {
                {{entry}}
                private static void Replace(ref CancellationToken token) => token = CancellationToken.None;
                private static void Replace(out CancellationToken token, int unused = 0) => token = CancellationToken.None;
            }
            """;
    }
}
