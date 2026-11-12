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

    [TestMethod]
    [DataRow("execute")]
    [DataRow("lifecycle")]
    [DataRow("configuration")]
    public async Task Accepts_Captured_Token_Read_In_Mutation_Target(string root)
    {
        string[] mutations =
        [
            "tasks[cancellationToken] = Task.CompletedTask;",
            "ref CancellationToken alias = ref (cancellationToken.CanBeCanceled ? ref tokens[0] : ref tokens[1]); alias = CancellationToken.None;",
            "(tasks[cancellationToken], _) = (Task.CompletedTask, 1);",
            "Replace(ref tokens[cancellationToken.GetHashCode()]);",
            "Replace(out tokens[cancellationToken.GetHashCode()]);",
            "ref CancellationToken alias = ref tokens[cancellationToken.GetHashCode()]; alias = CancellationToken.None;",
        ];
        foreach (var mutation in mutations)
        {
            var setup = "var tasks = new System.Collections.Generic.Dictionary<CancellationToken, Task>(); "
                + "var tokens = new CancellationToken[1]; "
                + mutation;
            await VerifyCS.VerifyAnalyzerAsync(Source(root, setup, "Task.Delay(1, cancellationToken)"));
        }
    }

    [TestMethod]
    [DataRow("execute")]
    [DataRow("lifecycle")]
    [DataRow("configuration")]
    public async Task Accepts_Immutable_Captured_Parallel_Callback_Token(string root)
    {
        await VerifyCS.VerifyAnalyzerAsync(
            Source(root, "", "Task.Delay(1, callbackToken)", parallelCallback: true));
    }

    [TestMethod]
    [DataRow("execute", false)]
    [DataRow("lifecycle", false)]
    [DataRow("configuration", false)]
    [DataRow("execute", true)]
    [DataRow("lifecycle", true)]
    [DataRow("configuration", true)]
    public async Task Accepts_Readonly_Ref_Alias_Of_Captured_Token(string root, bool parallelCallback)
    {
        var token = parallelCallback ? "callbackToken" : "cancellationToken";
        string[] aliases =
        [
            $"ref readonly CancellationToken alias = ref {token};",
            $"CancellationToken other = default; ref readonly CancellationToken alias = ref other; alias = ref {token};",
            $"bool flag = true; CancellationToken other = default; ref readonly CancellationToken alias = ref (flag ? ref {token} : ref other);",
        ];
        foreach (var alias in aliases)
        {
            await VerifyCS.VerifyAnalyzerAsync(
                Source(root, alias, $"Task.Delay(1, {token})", parallelCallback));
        }
    }

    private static Microsoft.CodeAnalysis.Testing.DiagnosticResult MissingToken() =>
        VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");

    private static string Source(string root, string mutation, string call, bool parallelCallback = false)
    {
        var body = $$"""
            {{mutation}}
            await Task.Run(async () => await {{call}}, cancellationToken);
            """;
        if (parallelCallback)
        {
            body = $$"""
                await Parallel.ForEachAsync(new[] { 1 }, cancellationToken, async (_, callbackToken) =>
                {
                    {{mutation}}
                    await Task.Run(async () => await {{call}}, callbackToken);
                });
                """;
        }

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
