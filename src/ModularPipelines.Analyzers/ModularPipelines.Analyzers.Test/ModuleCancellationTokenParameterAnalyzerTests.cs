using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleCancellationTokenParameterAnalyzerTests
{
    [TestMethod]
    [DataRow("cancellationToken = CancellationToken.None;", true)]
    [DataRow("cancellationToken = new CancellationToken();", true)]
    [DataRow("cancellationToken = new CancellationTokenSource().Token;", true)]
    [DataRow("if (context != null) cancellationToken = CancellationToken.None;", true)]
    [DataRow("cancellationToken = context != null ? cancellationToken : CancellationToken.None;", true)]
    [DataRow("cancellationToken = cancellationToken;", false)]
    [DataRow("var saved = cancellationToken; cancellationToken = default; cancellationToken = saved;", false)]
    [DataRow("var saved = cancellationToken; var copy = saved; saved = default; cancellationToken = copy;", false)]
    [DataRow("var saved = cancellationToken; if (context != null) cancellationToken = default; cancellationToken = saved;", false)]
    [DataRow("if (false) cancellationToken = default;", false)]
    [DataRow("if (true) cancellationToken = default;", true)]
    [DataRow("if (context != null) { cancellationToken = default; return Task.CompletedTask; }", false)]
    public async Task Tracks_Reachable_Scalar_Writes(string statements, bool warns)
    {
        await Verify(statements, false, warns);
    }

    [TestMethod]
    [DataRow("cancellationToken = CancellationToken.None;")]
    [DataRow("cancellationToken = new CancellationTokenSource().Token;")]
    [DataRow("if (context != null) cancellationToken = CancellationToken.None;")]
    public async Task Reports_Awaited_Replacements(string statements)
    {
        await Verify(statements, true, true);
    }

    [TestMethod]
    public async Task Preserves_Copy_Saved_Before_Replacement()
    {
        await VerifyCS.VerifyAnalyzerAsync(Source("var saved = cancellationToken; cancellationToken = default;", "Task.Delay(1, saved)"));
    }

    [TestMethod]
    [DataRow("cancellationToken = Pass(cancellationToken);")]
    [DataRow("var tokens = new[] { cancellationToken }; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var saved = (cancellationToken, 1); cancellationToken = saved.Item1;")]
    [DataRow("try { cancellationToken = cancellationToken; } finally { }")]
    [DataRow("if (cancellationToken is var saved) cancellationToken = saved;")]
    [DataRow("if (cancellationToken is { } saved) cancellationToken = saved;")]
    [DataRow("if ((object)cancellationToken is CancellationToken saved) cancellationToken = saved;")]
    [DataRow("if (cancellationToken is var saved) { var copy = saved; cancellationToken = copy; }")]
    public async Task Unsupported_Shapes_Keep_Existing_Behavior(string statements)
    {
        await Verify(statements, false, false);
    }

    [TestMethod]
    [Timeout(30000, CooperativeCancellation = true)]
    public async Task Conditional_Copies_Have_Bounded_Cost()
    {
        var statements = "var saved = cancellationToken; "
            + string.Concat(Enumerable.Range(0, 40).Select(i =>
                $"var copy{i} = context != null ? saved : saved; saved = copy{i}; "))
            + "cancellationToken = default; cancellationToken = saved;";
        await Verify(statements, false, false);
    }

    [TestMethod]
    [Timeout(30000, CooperativeCancellation = true)]
    public async Task Many_Task_Calls_Share_Analysis()
    {
        var statements = "cancellationToken = cancellationToken; "
            + string.Concat(Enumerable.Range(0, 100).Select(_ => "await Task.Delay(1, cancellationToken); "));
        await Verify(statements, true, false);
    }

    private static Task Verify(string statements, bool awaited, bool warns)
    {
        var call = warns ? "{|#0:Task.Delay(1, cancellationToken)|}" : "Task.Delay(1, cancellationToken)";
        var source = Source(statements, call);
        if (awaited)
        {
            source = source.Replace("override Task OnBeforeExecuteAsync", "override async Task OnBeforeExecuteAsync")
                .Replace("return " + call, "await " + call);
        }

        return VerifyCS.VerifyAnalyzerAsync(source, warns
            ? [VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay")]
            : []);
    }

    private static string Source(string statements, string call) => $$"""
        {{TestSourceConstants.StandardModuleHeader}}
        public class BuildModule : Module<List<string>>
        {
            private static CancellationToken Pass(CancellationToken token) => token;
            {{TestSourceConstants.SimpleAsyncExecuteBody}}
            protected override Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            {
                {{statements}}
                return {{call}};
            }
        }
        """;
}
