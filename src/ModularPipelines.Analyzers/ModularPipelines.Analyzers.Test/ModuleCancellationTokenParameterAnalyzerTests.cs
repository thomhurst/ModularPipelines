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
    [DataRow("var tokens = new[] { cancellationToken }; tokens[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = tokens; alias[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = context != null ? tokens : new CancellationToken[1]; alias[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var saved = context != null ? (tokens, 1) : default; saved.Item1[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = tokens; tokens = new[] { cancellationToken }; alias[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(alias).Token;")]
    [DataRow("var saved = (cancellationToken, 1); saved.Item1 = CancellationToken.None; (cancellationToken, _) = saved;")]
    [DataRow("var saved = (cancellationToken, 1); var copy = saved; copy.Item1 = CancellationToken.None; (cancellationToken, _) = copy;")]
    [DataRow("var saved = (cancellationToken, 1); if (context != null) saved.Item1 = CancellationToken.None; (cancellationToken, _) = saved;")]
    [DataRow("var tokens = new[] { cancellationToken }; tokens[context != null ? 0 : 0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var saved = (tokens, 1); saved.Item1[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; Array.Clear(tokens, 0, 1); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; tokens.SetValue(CancellationToken.None, 0); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; Span<CancellationToken> alias = tokens; alias[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; object saved = (tokens, 1); ((ValueTuple<CancellationToken[], int>)saved).Item1[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var saved = new object[] { tokens }; ((CancellationToken[])saved[0])[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var saved = (cancellationToken, 1); saved = (CancellationToken.None, 1); (cancellationToken, _) = saved;")]
    [DataRow("var saved = (cancellationToken, 1); saved.Item1 = CancellationToken.None; cancellationToken = saved.Item1;")]
    [DataRow("var saved = (cancellationToken, 1); if (context != null) saved = (CancellationToken.None, 1); (cancellationToken, _) = saved;")]
    [DataRow("var saved = ((cancellationToken, 1), 2); saved.Item1 = (CancellationToken.None, 1); ((cancellationToken, _), _) = saved;")]
    [DataRow("var saved = ((cancellationToken, 1), 2); static void Clear(ref CancellationToken token) => token = CancellationToken.None; Clear(ref saved.Item1.Item1); ((cancellationToken, _), _) = saved;")]
    [DataRow("var saved = ((cancellationToken, 1), 2); static void Clear(out CancellationToken token) => token = CancellationToken.None; Clear(out saved.Item1.Item1); ((cancellationToken, _), _) = saved;")]
    [DataRow("var saved = ((cancellationToken, 1), 2); (saved.Item1, _) = ((CancellationToken.None, 1), 2); ((cancellationToken, _), _) = saved;")]
    [DataRow("var saved = ((cancellationToken, 1), 2); saved.Item1 = (CancellationToken.None, 1); cancellationToken = saved.Item1.Item1;")]
    [DataRow("var saved = ((cancellationToken, 1), 2); static void Clear(ref (CancellationToken, int) pair) => pair = (CancellationToken.None, 1); Clear(ref saved.Item1); ((cancellationToken, _), _) = saved;")]
    [DataRow("var saved = ((cancellationToken, 1), 2); static void Clear(out (CancellationToken, int) pair) => pair = (CancellationToken.None, 1); Clear(out saved.Item1); ((cancellationToken, _), _) = saved;")]
    [DataRow("var saved = (cancellationToken, 1); (saved.Item1, _) = (CancellationToken.None, 2); cancellationToken = saved.Item1;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = context != null ? tokens : new[] { cancellationToken }; alias[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; static void Clear(ref CancellationToken token) => token = CancellationToken.None; Clear(ref tokens[0]); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; (tokens[0], _) = (CancellationToken.None, 1); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = tokens; static void Clear(ref CancellationToken[] values) => values[0] = CancellationToken.None; Clear(ref alias); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var saved = (tokens, 1); tokens[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(saved.Item1).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; static CancellationToken[] PassArray(CancellationToken[] value) => value; var alias = PassArray(tokens); alias[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(alias).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; static CancellationToken[] PassArray(CancellationToken[] value) => value; var alias = PassArray(tokens); alias[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new CancellationToken[1]; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new CancellationToken[1]; tokens[0] = cancellationToken; tokens[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = tokens.PassArray(); alias[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; static object PassObject(object value) => value; var alias = (CancellationToken[])PassObject(tokens); alias[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(alias).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; static CancellationToken[] Clear(CancellationToken[] value) { value[0] = CancellationToken.None; return value; } var alias = Clear(tokens); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(alias).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; static object Box((CancellationToken[], int) pair) => pair; var saved = Box((tokens, 1)); ((ValueTuple<CancellationToken[], int>)saved).Item1[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
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
    [DataRow("var saved = (cancellationToken, 1); (cancellationToken, _) = saved;")]
    [DataRow("static (CancellationToken, int) PassPair((CancellationToken, int) pair) => pair; var saved = PassPair((cancellationToken, 1)); (cancellationToken, _) = saved;")]
    [DataRow("var saved = (cancellationToken, 1); var copy = saved; (cancellationToken, _) = copy;")]
    [DataRow("var saved = ((cancellationToken, 1), 2); ((cancellationToken, _), _) = saved;")]
    [DataRow("var saved = (1, cancellationToken); (_, cancellationToken) = saved;")]
    [DataRow("var saved = (number: 1, token: cancellationToken); cancellationToken = saved.token;")]
    [DataRow("var saved = (cancellationToken, 1); var copy = saved; copy.Item1 = CancellationToken.None; (cancellationToken, _) = saved;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = tokens; alias = new[] { CancellationToken.None }; alias[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken, cancellationToken }; tokens[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; tokens[0] = cancellationToken; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken, cancellationToken }; if (context != null) tokens[0] = CancellationToken.None; else tokens[1] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("cancellationToken = cancellationToken; var values = new object[] { cancellationToken }; values[0] = values; GC.KeepAlive(values);")]
    [DataRow("var tokens = new[] { cancellationToken }; while (DateTime.Now.Ticks < 0) tokens = [..tokens, cancellationToken]; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("cancellationToken = cancellationToken; object saved = cancellationToken; while (DateTime.Now.Ticks < 0) saved = (saved, 1);")]
    [DataRow("var saved = ((1, cancellationToken), 2); ((_, cancellationToken), _) = saved;")]
    [DataRow("var saved = (cancellationToken, 1); var copy = saved; cancellationToken = CancellationToken.None; (cancellationToken, _) = copy;")]
    [DataRow("var saved = context != null ? (cancellationToken, 1) : (cancellationToken, 2); (cancellationToken, _) = saved;")]
    [DataRow("var tokens = new[] { cancellationToken }; var other = new[] { cancellationToken }; other[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var linked = CancellationTokenSource.CreateLinkedTokenSource(tokens); tokens[0] = CancellationToken.None; cancellationToken = linked.Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = tokens; var linked = CancellationTokenSource.CreateLinkedTokenSource(tokens); alias[0] = CancellationToken.None; cancellationToken = linked.Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = tokens; alias = new[] { cancellationToken }; alias[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; tokens[0] = CancellationToken.None; tokens = new[] { cancellationToken }; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var saved = ((CancellationToken.None, 1), 2); saved.Item1 = (cancellationToken, 1); ((cancellationToken, _), _) = saved;")]
    [DataRow("var saved = ((CancellationToken.None, 1), 2); (saved.Item1, _) = ((cancellationToken, 1), 2); ((cancellationToken, _), _) = saved;")]
    [DataRow("var saved = ((cancellationToken, 1), (cancellationToken, 2)); saved.Item1 = (CancellationToken.None, 1); cancellationToken = saved.Item2.Item1;")]
    [DataRow("var saved = ((cancellationToken, 1), 2); var pair = saved.Item1; cancellationToken = CancellationToken.None; (cancellationToken, _) = pair;")]
    [DataRow("var saved = ((cancellationToken, 1), 2); saved.Item1 = saved.Item1; ((cancellationToken, _), _) = saved;")]
    [DataRow("var tokens = new CancellationToken[1]; tokens[0] = cancellationToken; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new CancellationToken[int.MaxValue]; tokens[1000000] = cancellationToken; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new CancellationToken[context != null ? 1 : 2]; tokens[0] = cancellationToken; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; GC.KeepAlive(tokens); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; static CancellationToken[] PassArray(CancellationToken[] value) => value; var alias = PassArray(tokens); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(alias).Token;")]
    [DataRow("var tokens = new CancellationToken[1]; static CancellationToken[] PassArray(CancellationToken[] value) { return value; } var alias = PassArray(tokens); alias[0] = cancellationToken; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = tokens.PassArray(); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(alias).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; static object PassObject(object value) => value; var alias = (CancellationToken[])PassObject(tokens); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(alias).Token;")]
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
    [Timeout(30000, CooperativeCancellation = true)]
    [DataRow("static object Box((object, int) pair) => pair;", "Box((saved, 1))")]
    [DataRow("static object Box((object, int) pair) { return pair; }", "Box((saved, 1))")]
    [DataRow("static object Box((object, int) pair) => (object)pair;", "Box((saved, 1))")]
    [DataRow("static (object, int) PassPair((object, int) pair) => pair;", "PassPair((saved, 1))")]
    public async Task Tuple_Helper_Boxing_Has_Bounded_Cost(string helper, string expression)
    {
        var statements = $"{helper} cancellationToken = cancellationToken; object saved = cancellationToken; "
            + $"while (DateTime.Now.Ticks < 0) saved = {expression};";
        await VerifyCS.VerifyAnalyzerAsync(Source(statements, "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [Timeout(30000, CooperativeCancellation = true)]
    public async Task Conditional_Copies_Have_Bounded_Cost()
    {
        var statements = "var saved = cancellationToken; "
            + string.Concat(Enumerable.Range(0, 40).Select(i =>
                $"var copy{i} = context != null ? saved : saved; saved = copy{i}; "))
            + "cancellationToken = CancellationToken.None; cancellationToken = saved;";
        await VerifyCS.VerifyAnalyzerAsync(Source(statements, "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [Timeout(30000, CooperativeCancellation = true)]
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
            public static CancellationToken[] PassArray(this CancellationToken[] tokens) => tokens;
        }
        """;
}
