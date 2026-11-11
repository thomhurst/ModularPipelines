using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleCancellationTokenParameterAnalyzerTests
{
    [TestMethod]
    [DataRow("cancellationToken = CancellationToken.None;")]
    [DataRow("cancellationToken = new CancellationToken();")]
    [DataRow("var saved = cancellationToken; cancellationToken = CancellationToken.None; if (context != null) { cancellationToken = saved; }")]
    [DataRow("var saved = cancellationToken; cancellationToken = CancellationToken.None; if (context != null) { cancellationToken = saved; } else { cancellationToken = CancellationToken.None; }")]
    [DataRow("if (DateTime.Now.Ticks > 0) cancellationToken = CancellationToken.None;")]
    [DataRow("var saved = cancellationToken; cancellationToken = CancellationToken.None; saved = cancellationToken; cancellationToken = saved;")]
    [DataRow("if (context != null) cancellationToken = CancellationToken.None; else cancellationToken = new CancellationToken();")]
    [DataRow("(cancellationToken, _) = (CancellationToken.None, 1);")]
    [DataRow("var saved = cancellationToken; (saved, _) = (CancellationToken.None, 1); cancellationToken = saved;")]
    [DataRow("var saved = cancellationToken; (cancellationToken, saved) = (CancellationToken.None, cancellationToken); cancellationToken = CancellationToken.None;")]
    [DataRow("cancellationToken = Replace(cancellationToken);")]
    [DataRow("cancellationToken = Recurse(cancellationToken);")]
    [DataRow("cancellationToken = MutatingRecurse(cancellationToken);")]
    [DataRow("cancellationToken = Pass(CancellationToken.None);")]
    public async Task Reports_Replaced_Parameter(string statements)
    {
        await VerifyCS.VerifyAnalyzerAsync(Source(statements, "{|#0:Task.Delay(1, cancellationToken)|}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("cancellationToken = cancellationToken;")]
    [DataRow("var saved = cancellationToken; cancellationToken = CancellationToken.None; if (context != null) { cancellationToken = saved; } else { cancellationToken = saved; }")]
    [DataRow("var saved = cancellationToken; cancellationToken = CancellationToken.None; if (context != null) { if (DateTime.Now.Ticks > 0) { cancellationToken = saved; } else { cancellationToken = saved; } } else { cancellationToken = saved; }")]
    [DataRow("var saved = cancellationToken; cancellationToken = saved;")]
    [DataRow("var saved = cancellationToken; cancellationToken = CancellationToken.None; cancellationToken = saved;")]
    [DataRow("if (false) cancellationToken = CancellationToken.None;")]
    [DataRow("var saved = cancellationToken; if (context != null) { cancellationToken = CancellationToken.None; cancellationToken = saved; }")]
    [DataRow("var saved = cancellationToken; cancellationToken = saved; saved = cancellationToken; cancellationToken = saved;")]
    [DataRow("var saved = cancellationToken; if (context != null) { saved = CancellationToken.None; saved = cancellationToken; } cancellationToken = saved;")]
    [DataRow("var saved = CancellationToken.None; if (context != null) { saved = cancellationToken; } else { saved = cancellationToken; } cancellationToken = saved;")]
    [DataRow("var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken); cancellationToken = linked.Token;")]
    [DataRow("var saved = cancellationToken; try { cancellationToken = CancellationToken.None; } finally { cancellationToken = saved; }")]
    [DataRow("var saved = cancellationToken; cancellationToken = CancellationToken.None; if (true) cancellationToken = saved;")]
    [DataRow("var saved = cancellationToken; cancellationToken = CancellationToken.None; if (false) {} else cancellationToken = saved;")]
    [DataRow("cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(new[] { cancellationToken, CancellationToken.None }).Token;")]
    [DataRow("cancellationToken = CancellationTokenSource.CreateLinkedTokenSource([cancellationToken, CancellationToken.None]).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource([..tokens, CancellationToken.None]).Token;")]
    [DataRow("static CancellationToken Pass(CancellationToken token) => token; cancellationToken = Pass(cancellationToken);")]
    [DataRow("var saved = cancellationToken; (cancellationToken, saved) = (CancellationToken.None, cancellationToken); cancellationToken = saved;")]
    [DataRow("cancellationToken = Pass(cancellationToken);")]
    [DataRow("cancellationToken = cancellationToken.PassToken();")]
    public async Task Recognizes_Original_Parameter_Value(string statements)
    {
        await VerifyCS.VerifyAnalyzerAsync(Source(statements, "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Reports_Awaited_And_Returned_Replacement(bool awaited)
    {
        var source = Source("cancellationToken = CancellationToken.None;", "{|#0:Task.Delay(1, cancellationToken)|}");
        if (awaited)
        {
            source = source.Replace("override Task OnBeforeExecuteAsync", "override async Task OnBeforeExecuteAsync")
                .Replace("return {|#0:", "await {|#0:");
        }

        await VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [Timeout(30000)]
    public async Task Conditional_Copies_Have_Bounded_Cost()
    {
        var statements = "var saved = cancellationToken; "
            + string.Concat(Enumerable.Range(0, 40).Select(i =>
                $"var copy{i} = context != null ? saved : saved; saved = copy{i}; "))
            + "cancellationToken = CancellationToken.None; cancellationToken = saved;";
        await VerifyCS.VerifyAnalyzerAsync(Source(statements, "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [Timeout(30000)]
    public async Task Many_Task_Calls_Share_Parameter_Analysis()
    {
        var calls = string.Concat(Enumerable.Range(0, 100).Select(_ => "await Task.Delay(1, cancellationToken); "));
        var source = Source("cancellationToken = cancellationToken; " + calls, "Task.Delay(1, cancellationToken)")
            .Replace("override Task OnBeforeExecuteAsync", "override async Task OnBeforeExecuteAsync")
            .Replace("return Task.Delay", "await Task.Delay");
        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    private static string Source(string statements, string call) => $$"""
        {{TestSourceConstants.StandardModuleHeader}}
        public class BuildModule : Module<List<string>>
        {
            private static CancellationToken Pass(CancellationToken token) => token;
            private static CancellationToken Replace(CancellationToken token) => CancellationToken.None;
            private static CancellationToken Recurse(CancellationToken token) => Recurse(token);
            private static CancellationToken MutatingRecurse(CancellationToken token)
            {
                token = Pass(token);
                return MutatingRecurse(token);
            }
            {{TestSourceConstants.SimpleAsyncExecuteBody}}
            protected override Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            {
                {{statements}}
                return {{call}};
            }
        }
        public static class TokenExtensions
        {
            public static CancellationToken PassToken(this CancellationToken token) => token;
        }
        """;
}
