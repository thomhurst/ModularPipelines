using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleCancellationTokenParameterAnalyzerTests
{
    [TestMethod]
    [DataRow("Task", "{ await Task.Yield(); return token; }", false)]
    [DataRow("ValueTask", "{ await Task.Yield(); return token; }", false)]
    [DataRow("Task", "=> token;", false)]
    [DataRow("ValueTask", "=> token;", false)]
    [DataRow("Task", "{ await Task.Yield(); return token; }", true)]
    [DataRow("ValueTask", "{ await Task.Yield(); return token; }", true)]
    public async Task Async_Source_Helpers_Preserve_Original_Payload(string taskType, string body, bool configured)
    {
        var expression = configured ? "PreserveAsync(cancellationToken).ConfigureAwait(false)" : "PreserveAsync(cancellationToken)";
        var statements = $"static async {taskType}<CancellationToken> PreserveAsync(CancellationToken token) {body} "
            + $"var saved = {expression}; cancellationToken = default; cancellationToken = await saved;";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [Timeout(30000, CooperativeCancellation = true)]
    [DataRow("Task", "await Task.Yield(); return CancellationToken.None;")]
    [DataRow("ValueTask", "await Task.Yield(); return CancellationToken.None;")]
    [DataRow("Task", "await Task.Yield(); token = default; return token;")]
    [DataRow("ValueTask", "await Task.Yield(); token = default; return token;")]
    [DataRow("Task", "await Task.Yield(); if (DateTime.Now.Ticks > 0) return token; return CancellationToken.None;")]
    [DataRow("ValueTask", "await Task.Yield(); if (DateTime.Now.Ticks > 0) return token; return CancellationToken.None;")]
    [DataRow("Task", "await Task.Yield(); return await ReplaceAsync(token);")]
    [DataRow("ValueTask", "await Task.Yield(); return await ReplaceAsync(token);")]
    public async Task Async_Source_Helpers_Report_Replaced_Or_Unproven_Payload(string taskType, string body)
    {
        var statements = $"static async {taskType}<CancellationToken> ReplaceAsync(CancellationToken token) {{ {body} }} "
            + "cancellationToken = await ReplaceAsync(cancellationToken);";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, "{|#0:Task.Delay(1, cancellationToken)|}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Awaitable_Payload_Joins_Require_Original_On_Every_Path(bool replaced)
    {
        var replacement = replaced ? "Task.FromResult(CancellationToken.None)" : "Task.FromResult(cancellationToken)";
        var statements = "var saved = Task.FromResult(cancellationToken); "
            + $"if (context != null) saved = {replacement}; "
            + "cancellationToken = await saved;";
        var call = replaced ? "{|#0:Task.Delay(1, cancellationToken)|}" : "Task.Delay(1, cancellationToken)";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, call), replaced
            ? [VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay")]
            : []);
    }

    [TestMethod]
    public async Task Direct_Await_Preserves_Token_Payload()
    {
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(
            "cancellationToken = await Task.FromResult(cancellationToken);", "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [DataRow("Task.FromResult(cancellationToken)")]
    [DataRow("Task.FromResult(cancellationToken).ConfigureAwait(false)")]
    [DataRow("ValueTask.FromResult(cancellationToken)")]
    [DataRow("new ValueTask<CancellationToken>(cancellationToken)")]
    [DataRow("new ValueTask<CancellationToken>(Task.FromResult(cancellationToken)).ConfigureAwait(false)")]
    [DataRow("new ValueTask<CancellationToken>(cancellationToken).AsTask()")]
    public async Task Awaitable_Wrappers_Preserve_Token_Payload(string expression)
    {
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(
            $"var saved = {expression}; cancellationToken = default; cancellationToken = await saved;", "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [DataRow("Task.FromResult(CancellationToken.None)")]
    [DataRow("Task.FromResult(CancellationToken.None).ConfigureAwait(false)")]
    [DataRow("ValueTask.FromResult(CancellationToken.None)")]
    [DataRow("new ValueTask<CancellationToken>(CancellationToken.None)")]
    [DataRow("new ValueTask<CancellationToken>(Task.FromResult(CancellationToken.None)).ConfigureAwait(false)")]
    [DataRow("new ValueTask<CancellationToken>(CancellationToken.None).AsTask()")]
    public async Task Awaitable_Wrappers_Report_Replaced_Payload(string expression)
    {
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(
            $"var saved = {expression}; cancellationToken = await saved;", "{|#0:Task.Delay(1, cancellationToken)|}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow("cancellationToken = await Task.FromResult(cancellationToken);")]
    [DataRow("cancellationToken = await Task.FromResult(cancellationToken).ConfigureAwait(false);")]
    [DataRow("cancellationToken = await ValueTask.FromResult(cancellationToken);")]
    [DataRow("cancellationToken = await ValueTask.FromResult(cancellationToken).ConfigureAwait(false);")]
    [DataRow("cancellationToken = await new ValueTask<CancellationToken>(cancellationToken);")]
    [DataRow("cancellationToken = await new ValueTask<CancellationToken>(Task.FromResult(cancellationToken));")]
    [DataRow("cancellationToken = await new ValueTask<CancellationToken>(cancellationToken).ConfigureAwait(false);")]
    [DataRow("var saved = Task.FromResult(cancellationToken); cancellationToken = CancellationToken.None; cancellationToken = await saved;")]
    [DataRow("var saved = ValueTask.FromResult(cancellationToken); cancellationToken = CancellationToken.None; cancellationToken = await saved;")]
    [DataRow("var saved = context != null ? Task.FromResult(cancellationToken) : Task.FromResult(cancellationToken); cancellationToken = await saved;")]
    [DataRow("static bool Reset(out CancellationToken token) { token = default; return false; } var saved = Task.FromResult(cancellationToken); cancellationToken = await saved.ConfigureAwait(Reset(out cancellationToken));")]
    public async Task Awaitable_Wrappers_Preserve_Original_Parameter(string statements)
    {
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [DataRow("cancellationToken = await Task.FromResult(CancellationToken.None);")]
    [DataRow("cancellationToken = await ValueTask.FromResult(CancellationToken.None);")]
    [DataRow("cancellationToken = await new ValueTask<CancellationToken>(CancellationToken.None);")]
    [DataRow("cancellationToken = await new ValueTask<CancellationToken>(Task.FromResult(CancellationToken.None));")]
    [DataRow("var saved = context != null ? Task.FromResult(cancellationToken) : Task.FromResult(CancellationToken.None); cancellationToken = await saved;")]
    [DataRow("static Task<CancellationToken> ReplaceAsync(CancellationToken token) => Task.FromResult(CancellationToken.None); cancellationToken = await ReplaceAsync(cancellationToken);")]
    [DataRow("cancellationToken = await default(ValueTask<CancellationToken>);")]
    public async Task Reports_Replacement_Through_Awaitable_Wrappers(string statements)
    {
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, "{|#0:Task.Delay(1, cancellationToken)|}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow("cancellationToken = await (ReplacingAwaitable)Task.FromResult(cancellationToken);")]
    [DataRow("cancellationToken = await (Task<CancellationToken>)(ReplacingAwaitable)Task.FromResult(cancellationToken);")]
    public async Task Reports_Replacement_Through_Custom_Awaitable_Conversions(string statements)
    {
        var source = AsyncSource(statements, "{|#0:Task.Delay(1, cancellationToken)|}") + """

            public class ReplacingAwaitable
            {
                public static implicit operator ReplacingAwaitable(Task<CancellationToken> value) => new();
                public static implicit operator Task<CancellationToken>(ReplacingAwaitable value) => Task.FromResult(CancellationToken.None);
                public System.Runtime.CompilerServices.TaskAwaiter<CancellationToken> GetAwaiter() => Task.FromResult(CancellationToken.None).GetAwaiter();
            }
            """;
        await VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [Timeout(30000, CooperativeCancellation = true)]
    public async Task Awaitable_Boxing_Has_Bounded_Cost()
    {
        await VerifyCS.VerifyAnalyzerAsync(Source(
            "cancellationToken = cancellationToken; object saved = cancellationToken; while (DateTime.Now.Ticks < 0) saved = Task.FromResult(saved);",
            "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [DataRow("Span")]
    [DataRow("ReadOnlySpan")]
    public async Task Reports_Replacement_Read_From_Nonzero_Slice(string spanType)
    {
        var statements = $"var tokens = new[] {{ cancellationToken, CancellationToken.None }}; var slice = new {spanType}<CancellationToken>(tokens, 1, 1); cancellationToken = slice[0];";
        await VerifyCS.VerifyAnalyzerAsync(Source(statements, "{|#0:Task.Delay(1, cancellationToken)|}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow("ref readonly CancellationToken first = ref tokens[0];")]
    [DataRow("CancellationToken other = default; ref readonly CancellationToken first = ref other; first = ref tokens[0];")]
    [DataRow("var span = new Span<CancellationToken>(tokens); ref readonly CancellationToken first = ref span[0];")]
    [DataRow("var span = new Span<CancellationToken>(tokens); CancellationToken other = default; ref readonly CancellationToken first = ref other; first = ref span[0];")]
    public async Task Readonly_References_Preserve_Collection_Provenance(string reference)
    {
        var statements = $"var tokens = new[] {{ cancellationToken }}; {reference} cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;";
        await VerifyCS.VerifyAnalyzerAsync(Source(statements, "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [DataRow("var tokens = new[] { cancellationToken }; var span = new Span<CancellationToken>(tokens); ref CancellationToken first = ref span[0]; first = default; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(span).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; ref CancellationToken first = ref tokens[0]; first = default; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
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
    [DataRow("(CancellationToken First, CancellationToken Second) saved = default; saved.First = cancellationToken; cancellationToken = saved.Second;")]
    [DataRow("(CancellationToken Token, int Count) saved = default; if (context != null) saved.Token = cancellationToken; cancellationToken = saved.Token;")]
    [DataRow("(CancellationToken Token, int Count) saved = default; saved.Token = cancellationToken; saved = default; cancellationToken = saved.Token;")]
    [DataRow("(CancellationToken[] Tokens, int Count) saved = default; saved.Tokens = new[] { cancellationToken }; saved.Tokens[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(saved.Tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; static CancellationToken Link(ref CancellationToken token, CancellationToken[] values) { token = default; return CancellationTokenSource.CreateLinkedTokenSource(values).Token; } cancellationToken = Link(ref tokens[0], tokens);")]
    [DataRow("var tokens = new[] { cancellationToken }; static CancellationToken Link(out CancellationToken token, CancellationToken[] values) { token = default; return CancellationTokenSource.CreateLinkedTokenSource(values).Token; } cancellationToken = Link(out tokens[0], tokens);")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = new Span<CancellationToken>(tokens); alias[0] = default; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = new ReadOnlySpan<CancellationToken>(tokens); tokens[0] = default; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(alias).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; static CancellationToken Clear(ref CancellationToken token) { token = default; return default; } static CancellationToken Link(CancellationToken[] values, CancellationToken ignored) => CancellationTokenSource.CreateLinkedTokenSource(values).Token; cancellationToken = Link(tokens, Clear(ref tokens[0]));")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = new Span<CancellationToken>(tokens); static void Clear(ref CancellationToken token) => token = default; Clear(ref alias[0]); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = new Span<CancellationToken>(tokens, 0, 1); alias[0] = default; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
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
    [DataRow("(CancellationToken Token, int Count) saved = default; saved.Token = cancellationToken; cancellationToken = saved.Token;")]
    [DataRow("((CancellationToken Token, int Count) Pair, int Count) saved = default; saved.Pair.Token = cancellationToken; cancellationToken = saved.Pair.Token;")]
    [DataRow("(CancellationToken Token, int Count) saved = default; (saved.Token, _) = (cancellationToken, 1); (cancellationToken, _) = saved;")]
    [DataRow("(CancellationToken First, CancellationToken Second) saved = default; saved.First = cancellationToken; saved.Second = CancellationToken.None; cancellationToken = saved.First;")]
    [DataRow("(CancellationToken Token, int Count) saved = default; if (context != null) saved.Token = cancellationToken; else saved.Token = cancellationToken; cancellationToken = saved.Token;")]
    [DataRow("(CancellationToken[] Tokens, int Count) saved = default; saved.Tokens = new[] { cancellationToken }; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(saved.Tokens).Token;")]
    [DataRow("static CancellationToken Reset(ref CancellationToken token, CancellationToken saved) { token = default; return saved; } cancellationToken = Reset(ref cancellationToken, cancellationToken);")]
    [DataRow("static CancellationToken Reset(out CancellationToken token, CancellationToken saved) { token = default; return saved; } cancellationToken = Reset(out cancellationToken, cancellationToken);")]
    [DataRow("static CancellationToken Reset(ref CancellationToken token, CancellationToken saved) { token = default; return saved; } var saved = (cancellationToken, 1); cancellationToken = Reset(ref saved.Item1, saved.Item1);")]
    [DataRow("var tokens = new[] { cancellationToken }; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(new ReadOnlySpan<CancellationToken>(tokens)).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = new Span<CancellationToken>(tokens); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(alias).Token;")]
    [DataRow("var tokens = new CancellationToken[1]; var alias = new Span<CancellationToken>(tokens); alias[0] = cancellationToken; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; _ = new Tuple<CancellationToken[], CancellationToken>(tokens, cancellationToken = tokens[0]);")]
    [DataRow("var tokens = new[] { cancellationToken }; var slice = new Span<CancellationToken>(tokens, 0, 1); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var slice = new ReadOnlySpan<CancellationToken>(tokens, 0, 1); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = new ReadOnlySpan<CancellationToken>(tokens); cancellationToken = alias[0];")]
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

    private static string AsyncSource(string statements, string call) => Source(statements, call)
        .Replace("override Task OnBeforeExecuteAsync", "override async Task OnBeforeExecuteAsync")
        .Replace("return " + call, "await " + call);

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
