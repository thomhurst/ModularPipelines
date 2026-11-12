using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleCancellationTokenCollectionAnalyzerTests
{
    [TestMethod]
    [DataRow("var tokens = new[] { cancellationToken }; ref readonly CancellationToken alias = ref tokens[0]; cancellationToken = alias;", false)]
    [DataRow("var tokens = new[] { cancellationToken }; ReadOnlySpan<CancellationToken> view = tokens; ref readonly CancellationToken alias = ref view[0]; cancellationToken = alias;", false)]
    [DataRow("var tokens = new[] { cancellationToken }; ref readonly CancellationToken alias = ref tokens[0]; cancellationToken = alias; cancellationToken = default;", true)]
    [DataRow("var saved = cancellationToken; var tokens = new[] { cancellationToken }; ref readonly CancellationToken alias = ref (context != null ? ref saved : ref tokens[0]); cancellationToken = alias;", false)]
    public async Task Unsupported_Readonly_Reference_Target_Remains_Unknown(string statements, bool warns)
    {
        await Verify(statements, warns);
    }

    [TestMethod]
    [DataRow("var tokens = new[] { cancellationToken }; ref readonly CancellationToken[] alias = ref tokens; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(alias).Token;", false)]
    [DataRow("var tokens = new[] { cancellationToken }; ref readonly CancellationToken[] alias = ref tokens; tokens = new CancellationToken[1]; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(alias).Token;", true)]
    [DataRow("var pair = (1, 2); if (cancellationToken is var saved) cancellationToken = saved;", false)]
    [DataRow("var pair = (1, 2); if (cancellationToken is { } saved) cancellationToken = saved;", false)]
    [DataRow("var pair = (1, 2); var saved = cancellationToken; cancellationToken = default; var slots = new Dictionary<CancellationToken, int>(); slots[cancellationToken = saved] = 0;", false)]
    public async Task Review_Storage_And_Target_Effects(string statements, bool warns)
    {
        await Verify(statements, warns);
    }

    [TestMethod]
    [DataRow("Span", false)]
    [DataRow("ReadOnlySpan", false)]
    [DataRow("Span", true)]
    public async Task CSharp13_Array_Span_Conversion_Preserves_Storage(string span, bool mutate)
    {
        var statements = $"var tokens = new[] {{ cancellationToken }}; {span}<CancellationToken> view = tokens; "
            + (mutate ? "view[0] = default; " : "")
            + "cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;";
        var test = new VerifyCS.Test
        {
            TestCode = Source(statements, mutate ? "{|#0:Task.Delay(1, cancellationToken)|}" : "Task.Delay(1, cancellationToken)"),
            ReferenceAssemblies = Net.Net110,
            TestState = { AdditionalReferences = { typeof(IModuleContext).Assembly.Location } },
        };
        test.SolutionTransforms.Add((solution, projectId) => solution.WithProjectParseOptions(projectId,
            ((Microsoft.CodeAnalysis.CSharp.CSharpParseOptions) solution.GetProject(projectId)!.ParseOptions!)
                .WithLanguageVersion(Microsoft.CodeAnalysis.CSharp.LanguageVersion.CSharp13)));
        if (mutate)
        {
            test.ExpectedDiagnostics.Add(VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay"));
        }

        await test.RunAsync(TestContext.CancellationToken);
    }

    [TestMethod]
    [DataRow("var tokens = new[] { cancellationToken }; var view = tokens.AsSpan(); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(view).Token;", false)]
    [DataRow("var tokens = new[] { cancellationToken }; var view = tokens.AsSpan(); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;", false)]
    [DataRow("var tokens = new[] { cancellationToken }; var view = tokens.AsSpan(); view[0] = default; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;", true)]
    [DataRow("var tokens = new[] { cancellationToken }; var view = tokens.AsSpan(); tokens[0] = default; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(view).Token;", true)]
    public async Task Full_Array_Span_Preserves_Aliasing_Facts(string statements, bool warns)
    {
        await Verify(statements, warns);
    }

    [TestMethod]
    [DataRow(false, true)]
    [DataRow(true, true)]
    [DataRow(false, false)]
    public async Task Unknown_Reference_Target_Survives_Join(bool replace, bool mutate)
    {
        var statements = $$"""
            var tokens = new[] { cancellationToken };
            var other = new[] { cancellationToken };
            ref readonly CancellationToken[] alias = ref tokens;
            if (context != null) alias = ref Unknown(ref other);
            var fresh = new[] { cancellationToken };
            other = fresh;
            {{(replace ? "tokens = new[] { cancellationToken };" : "")}}
            {{(mutate ? "alias[0] = default;" : "")}}
            cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(fresh).Token;
            """;
        await Verify(statements, mutate, "private static ref readonly CancellationToken[] Unknown(ref CancellationToken[] value) => ref value;");
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Stable_Readonly_Array_References_Invalidate_Only_Their_Allocation(bool affected)
    {
        var statements = "CancellationToken[] tokens = new[] { cancellationToken }; CancellationToken[] other = new[] { cancellationToken }; ref readonly CancellationToken[] alias = ref other; alias[0] = default; "
            + $"cancellationToken = CancellationTokenSource.CreateLinkedTokenSource({(affected ? "other" : "tokens")}).Token;";
        var call = affected ? "{|#0:Task.Delay(1, cancellationToken)|}" : "Task.Delay(1, cancellationToken)";
        await VerifyCS.VerifyAnalyzerAsync(Source(statements, call), affected
            ? [VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay")]
            : []);
    }

    [TestMethod]
    [DataRow("ref (context != null ? ref first : ref second)", "", "first", true)]
    [DataRow("ref (context != null ? ref first : ref second)", "", "untouched", false)]
    [DataRow("ref first", "alias = ref second;", "first", false)]
    public async Task Array_Reference_Targets_Follow_Conditional_Bindings_And_Reassignment(string binding, string rebind, string source, bool warns)
    {
        var statements = "CancellationToken[] first = new[] { cancellationToken }; CancellationToken[] second = new[] { cancellationToken }; CancellationToken[] untouched = new[] { cancellationToken }; "
            + $"ref readonly CancellationToken[] alias = {binding}; {rebind} alias[0] = default; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource({source}).Token;";
        await VerifyCS.VerifyAnalyzerAsync(Source(statements, warns ? "{|#0:Task.Delay(1, cancellationToken)|}" : "Task.Delay(1, cancellationToken)"), warns
            ? [VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay")]
            : []);
    }

    [TestMethod]
    [DataRow(false, false)]
    [DataRow(true, false)]
    [DataRow(false, true)]
    [DataRow(true, true)]
    public async Task Readonly_Reference_Array_Writes_Track_Current_Target_Storage(bool retargetStorage, bool affected)
    {
        var statements = "CancellationToken[] tokens = new[] { cancellationToken }; CancellationToken[] other = new[] { cancellationToken }; ref readonly CancellationToken[] alias = ref other; "
            + (retargetStorage ? "other = tokens; " : "") + "alias[0] = default; "
            + $"cancellationToken = CancellationTokenSource.CreateLinkedTokenSource({(affected ? "other" : "tokens")}).Token;";
        var warns = affected || retargetStorage;
        await VerifyCS.VerifyAnalyzerAsync(Source(statements, warns ? "{|#0:Task.Delay(1, cancellationToken)|}" : "Task.Delay(1, cancellationToken)"), warns
            ? [VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay")]
            : []);
    }

    [TestMethod]
    [DataRow("ref")]
    [DataRow("ref readonly")]
    public async Task Array_References_Invalidate_Backing_Facts_Before_Storage_Escape(string reference)
    {
        var statements = $"CancellationToken[] tokens = new[] {{ cancellationToken }}; var copy = tokens; {reference} CancellationToken[] alias = ref tokens; alias[0] = default; "
            + "cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(copy).Token;";
        await VerifyCS.VerifyAnalyzerAsync(Source(statements, "{|#0:Task.Delay(1, cancellationToken)|}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Readonly_Span_References_Invalidate_Only_On_Content_Writes(bool writes)
    {
        var mutation = writes ? "alias[0] = default;" : "";
        var statements = "var tokens = new[] { cancellationToken }; var span = new Span<CancellationToken>(tokens); ref readonly Span<CancellationToken> alias = ref span; "
            + $"{mutation} cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;";
        var call = writes ? "{|#0:Task.Delay(1, cancellationToken)|}" : "Task.Delay(1, cancellationToken)";
        await VerifyCS.VerifyAnalyzerAsync(Source(statements, call), writes
            ? [VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay")]
            : []);
    }

    [TestMethod]
    public async Task Readonly_Array_References_Preserve_Unchanged_Contents()
    {
        var statements = "CancellationToken[] tokens = new[] { cancellationToken }; ref readonly CancellationToken[] alias = ref tokens; "
            + "cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;";
        await VerifyCS.VerifyAnalyzerAsync(Source(statements, "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Conditional_Parameter_References_Track_Writable_Escapes(bool writable)
    {
        var reference = writable ? "ref CancellationToken" : "ref readonly CancellationToken";
        var mutation = writable ? "alias = default;" : "";
        var statements = $"CancellationToken other = default; {reference} alias = ref (context != null ? ref cancellationToken : ref other); {mutation}";
        var call = writable ? "{|#0:Task.Delay(1, cancellationToken)|}" : "Task.Delay(1, cancellationToken)";
        await VerifyCS.VerifyAnalyzerAsync(Source(statements, call), writable
            ? [VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay")]
            : []);
    }

    [TestMethod]
    [DataRow("var saved = cancellationToken; (cancellationToken, saved) = (CancellationToken.None, cancellationToken); cancellationToken = CancellationToken.None;")]
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
    [DataRow("var tokens = new[] { cancellationToken }; tokens.SetValue(CancellationToken.None, 0); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; Span<CancellationToken> alias = tokens; alias[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; object saved = (tokens, 1); ((ValueTuple<CancellationToken[], int>)saved).Item1[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var saved = (cancellationToken, 1); saved = (CancellationToken.None, 1); (cancellationToken, _) = saved;")]
    [DataRow("var saved = (cancellationToken, 1); saved.Item1 = CancellationToken.None; cancellationToken = saved.Item1;")]
    [DataRow("var saved = (cancellationToken, 1); if (context != null) saved = (CancellationToken.None, 1); (cancellationToken, _) = saved;")]
    [DataRow("var saved = ((cancellationToken, 1), 2); saved.Item1 = (CancellationToken.None, 1); ((cancellationToken, _), _) = saved;")]
    [DataRow("var saved = ((cancellationToken, 1), 2); (saved.Item1, _) = ((CancellationToken.None, 1), 2); ((cancellationToken, _), _) = saved;")]
    [DataRow("var saved = ((cancellationToken, 1), 2); saved.Item1 = (CancellationToken.None, 1); cancellationToken = saved.Item1.Item1;")]
    [DataRow("var saved = (cancellationToken, 1); (saved.Item1, _) = (CancellationToken.None, 2); cancellationToken = saved.Item1;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = context != null ? tokens : new[] { cancellationToken }; alias[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; (tokens[0], _) = (CancellationToken.None, 1); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var saved = (tokens, 1); tokens[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(saved.Item1).Token;")]
    [DataRow("var tokens = new CancellationToken[1]; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new CancellationToken[1]; tokens[0] = cancellationToken; tokens[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("(CancellationToken First, CancellationToken Second) saved = default; saved.First = cancellationToken; cancellationToken = saved.Second;")]
    [DataRow("(CancellationToken Token, int Count) saved = default; if (context != null) saved.Token = cancellationToken; cancellationToken = saved.Token;")]
    [DataRow("(CancellationToken Token, int Count) saved = default; saved.Token = cancellationToken; saved = default; cancellationToken = saved.Token;")]
    [DataRow("(CancellationToken[] Tokens, int Count) saved = default; saved.Tokens = new[] { cancellationToken }; saved.Tokens[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(saved.Tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = new Span<CancellationToken>(tokens); alias[0] = default; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = new ReadOnlySpan<CancellationToken>(tokens); tokens[0] = default; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(alias).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = new Span<CancellationToken>(tokens, 0, 1); alias[0] = default; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    public async Task Reports_Replaced_Collections_And_Tuples(string statements)
    {
        await Verify(statements, true);
    }

    [TestMethod]
    [DataRow("var tokens = new[] { cancellationToken }; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource([..tokens, CancellationToken.None]).Token;")]
    [DataRow("var saved = (cancellationToken, 1); (cancellationToken, _) = saved;")]
    [DataRow("var saved = (cancellationToken, 1); var copy = saved; (cancellationToken, _) = copy;")]
    [DataRow("var saved = ((cancellationToken, 1), 2); ((cancellationToken, _), _) = saved;")]
    [DataRow("var saved = (1, cancellationToken); (_, cancellationToken) = saved;")]
    [DataRow("var saved = (number: 1, token: cancellationToken); cancellationToken = saved.token;")]
    [DataRow("var saved = (cancellationToken, 1); var copy = saved; copy.Item1 = CancellationToken.None; (cancellationToken, _) = saved;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = tokens; alias = new[] { CancellationToken.None }; alias[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken, cancellationToken }; tokens[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; tokens[0] = cancellationToken; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken, cancellationToken }; if (context != null) tokens[0] = CancellationToken.None; else tokens[1] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
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
    [DataRow("(CancellationToken Token, int Count) saved = default; saved.Token = cancellationToken; cancellationToken = saved.Token;")]
    [DataRow("((CancellationToken Token, int Count) Pair, int Count) saved = default; saved.Pair.Token = cancellationToken; cancellationToken = saved.Pair.Token;")]
    [DataRow("(CancellationToken Token, int Count) saved = default; (saved.Token, _) = (cancellationToken, 1); (cancellationToken, _) = saved;")]
    [DataRow("(CancellationToken First, CancellationToken Second) saved = default; saved.First = cancellationToken; saved.Second = CancellationToken.None; cancellationToken = saved.First;")]
    [DataRow("(CancellationToken Token, int Count) saved = default; if (context != null) saved.Token = cancellationToken; else saved.Token = cancellationToken; cancellationToken = saved.Token;")]
    [DataRow("(CancellationToken[] Tokens, int Count) saved = default; saved.Tokens = new[] { cancellationToken }; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(saved.Tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(new ReadOnlySpan<CancellationToken>(tokens)).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = new Span<CancellationToken>(tokens); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(alias).Token;")]
    [DataRow("var tokens = new CancellationToken[1]; var alias = new Span<CancellationToken>(tokens); alias[0] = cancellationToken; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; _ = new Tuple<CancellationToken[], CancellationToken>(tokens, cancellationToken = tokens[0]);")]
    [DataRow("var tokens = new[] { cancellationToken }; var slice = new Span<CancellationToken>(tokens, 0, 1); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var slice = new ReadOnlySpan<CancellationToken>(tokens, 0, 1); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = new ReadOnlySpan<CancellationToken>(tokens); cancellationToken = alias[0];")]
    public async Task Preserves_Collections_And_Tuples(string statements)
    {
        await Verify(statements, false);
    }

    [TestMethod]
    [DataRow("var saved = cancellationToken; Reset(ref cancellationToken); cancellationToken = saved;", false)]
    [DataRow("var saved = cancellationToken; Reset(ref saved); saved = cancellationToken; cancellationToken = saved;", false)]
    [DataRow("var saved = cancellationToken; ref CancellationToken alias = ref saved; saved = cancellationToken; alias = default; cancellationToken = saved;", true)]
    [DataRow("var saved = (cancellationToken, cancellationToken); ref CancellationToken alias = ref saved.Item2; alias = default; cancellationToken = saved.Item1;", false)]
    [DataRow("var saved = ((cancellationToken, cancellationToken), 1); ref CancellationToken alias = ref saved.Item1.Item2; alias = default; cancellationToken = saved.Item1.Item1;", false)]
    [DataRow("var saved = ((cancellationToken, cancellationToken), 1); ref CancellationToken alias = ref saved.Item1.Item2; saved.Item1 = (cancellationToken, cancellationToken); alias = default; cancellationToken = saved.Item1.Item2;", true)]
    public async Task Reference_Storage_Escapes_Are_Persistent_Only_While_References_Survive(string statements, bool warns)
    {
        await Verify(statements, warns, "private static void Reset(ref CancellationToken token) => token = default;");
    }

    [TestMethod]
    [DataRow("var pair = (1, 2); cancellationToken = Pass(cancellationToken);")]
    [DataRow("var pair = (1, 2); cancellationToken = Task.FromResult(cancellationToken).Result;")]
    public async Task Unsupported_Helper_And_Awaitable_Proofs_Stay_On_Existing_Path(string statements)
    {
        await Verify(statements, false, "private static CancellationToken Pass(CancellationToken token) => token;");
    }

    [TestMethod]
    [DataRow("var saved = cancellationToken; (saved, cancellationToken) = (CancellationToken.None, saved);", false)]
    [DataRow("var tokens = new[] { cancellationToken }; var saved = tokens; tokens[0] = (tokens = new[] { default(CancellationToken) })[0]; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(saved).Token;", true)]
    [DataRow("var tokens = new[] { cancellationToken }; var saved = tokens; var span = tokens.AsSpan(); span[0] = (tokens = new[] { default(CancellationToken) })[0]; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(saved).Token;", true)]
    public async Task Assignments_Snapshot_Values_And_Storage_In_Language_Order(string statements, bool warns)
    {
        await Verify(statements, warns);
    }

    [TestMethod]
    [DataRow("var saved = (cancellationToken, 1); ref readonly (CancellationToken, int) reader = ref saved; ref CancellationToken writer = ref saved.Item1; writer = default; cancellationToken = reader.Item1;", true)]
    [DataRow("var saved = ((cancellationToken, 1), 2); ref readonly ((CancellationToken, int), int) reader = ref saved; ref CancellationToken writer = ref saved.Item1.Item1; writer = default; cancellationToken = reader.Item1.Item1;", true)]
    [DataRow("var saved = ((cancellationToken, 1), 2); ref readonly (CancellationToken, int) reader = ref saved.Item1; ref CancellationToken writer = ref saved.Item1.Item1; writer = default; cancellationToken = reader.Item1;", true)]
    [DataRow("var saved = (cancellationToken, cancellationToken); ref readonly (CancellationToken, CancellationToken) reader = ref saved; ref CancellationToken writer = ref saved.Item2; writer = default; cancellationToken = reader.Item1;", false)]
    [DataRow("var saved = ((cancellationToken, cancellationToken), 1); ref readonly (CancellationToken, CancellationToken) reader = ref saved.Item1; ref CancellationToken writer = ref saved.Item1.Item2; writer = default; cancellationToken = reader.Item1;", false)]
    [DataRow("var saved = (cancellationToken, 1); ref readonly (CancellationToken, int) reader = ref saved; cancellationToken = reader.Item1;", false)]
    [DataRow("var saved = (cancellationToken, 1); ref readonly (CancellationToken, int) reader = ref saved; var copy = reader; ref CancellationToken writer = ref saved.Item1; writer = default; cancellationToken = copy.Item1;", false)]
    public async Task Readonly_Tuple_References_Mask_Escaped_Descendants(string statements, bool warns)
    {
        await Verify(statements, warns);
    }

    [TestMethod]
    [DataRow("var pair = (1, 2); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(Pass(cancellationToken), default).Token;", false)]
    [DataRow("var pair = (1, 2); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(default, Pass(cancellationToken)).Token;", false)]
    [DataRow("var pair = (1, 2); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(Pass(cancellationToken), default).Token; cancellationToken = default;", true)]
    [DataRow("cancellationToken = Pair(cancellationToken).Item1;", false)]
    [DataRow("var saved = Pair(cancellationToken); saved.Item2 = 2; cancellationToken = saved.Item1;", false)]
    [DataRow("var saved = context != null ? Pair(cancellationToken) : (cancellationToken, 1); cancellationToken = saved.Item1;", false)]
    [DataRow("cancellationToken = Nested(cancellationToken).Item1.Item1;", false)]
    [DataRow("cancellationToken = Pair(cancellationToken).Item1; cancellationToken = default;", true)]
    public async Task Unsupported_Helper_Results_Preserve_Unknown_Provenance(string statements, bool warns)
    {
        await Verify(statements, warns, "private static CancellationToken Pass(CancellationToken token) => token; private static (CancellationToken, int) Pair(CancellationToken token) => (token, 1); private static ((CancellationToken, int), int) Nested(CancellationToken token) => ((token, 1), 2);");
    }

    [TestMethod]
    [DataRow("ref CancellationToken slot = ref tokens[0];", "tokens", true)]
    [DataRow("Span<CancellationToken> view = tokens; ref CancellationToken slot = ref view[0];", "tokens", true)]
    [DataRow("ref CancellationToken slot = ref tokens[0];", "other", false)]
    public async Task Writable_Element_References_Prevent_Stale_Array_Proofs(string binding, string source, bool warns)
    {
        await Verify($"var tokens = new[] {{ cancellationToken }}; var other = new[] {{ cancellationToken }}; {binding} tokens[0] = cancellationToken; slot = default; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource({source}).Token;", warns);
    }

    [TestMethod]
    [DataRow("(context != null ? ref other : ref cancellationToken) = default;", true)]
    [DataRow("(context != null ? ref cancellationToken : ref other) = default;", true)]
    [DataRow("(context != null ? ref other : ref cancellationToken) = saved;", false)]
    [DataRow("var storage = (saved, 1); (context != null ? ref other : ref storage.Item1) = default; cancellationToken = storage.Item1;", true)]
    [DataRow("var storage = (saved, 1); (context != null ? ref other : ref storage.Item1) = saved; cancellationToken = storage.Item1;", false)]
    [DataRow("(context != null ? ref other : ref cancellationToken) = default; cancellationToken = saved;", false)]
    public async Task Conditional_Reference_Assignments_Write_Backing_Storage(string assignment, bool warns)
    {
        await Verify($"var pair = (1, 2); var saved = cancellationToken; CancellationToken other = default; cancellationToken = saved; {assignment}", warns);
    }

    [TestMethod]
    [DataRow("(CancellationToken First, CancellationToken Second) storage = default; (context != null ? ref storage.First : ref other) = default; cancellationToken = storage.Second;", true)]
    [DataRow("((CancellationToken First, CancellationToken Second) Nested, int Count) storage = default; (context != null ? ref storage.Nested.First : ref other) = saved; cancellationToken = storage.Nested.Second;", true)]
    [DataRow("(CancellationToken First, CancellationToken Second) storage = default; (context != null ? ref storage.First : ref other) = saved; cancellationToken = storage.First;", true)]
    [DataRow("(CancellationToken First, CancellationToken Second) storage = (saved, saved); (context != null ? ref storage.First : ref other) = default; cancellationToken = storage.Second;", false)]
    public async Task Conditional_Tuple_Field_Writes_Preserve_Default_Siblings(string statements, bool warns)
    {
        await Verify($"var saved = cancellationToken; CancellationToken other = default; {statements}", warns);
    }

    [TestMethod]
    [DataRow("var tokens = new[] { Pass(cancellationToken) };", "tokens", false)]
    [DataRow("var tokens = new[] { Pass(cancellationToken) }; ReadOnlySpan<CancellationToken> view = tokens;", "view", false)]
    [DataRow("var tokens = new CancellationToken[1]; tokens[0] = Pass(cancellationToken);", "tokens", false)]
    [DataRow("var tokens = new[] { Pass(cancellationToken) }; tokens[0] = default;", "tokens", true)]
    [DataRow("var tokens = new[] { Pass(cancellationToken), cancellationToken };", "tokens", false)]
    [DataRow("var tokens = new[] { Pass(cancellationToken) }; var independent = new CancellationToken[1];", "independent", true)]
    public async Task Linked_Collections_Preserve_Unknown_Elements(string statements, string argument, bool warns)
    {
        await Verify($"{statements} cancellationToken = CancellationTokenSource.CreateLinkedTokenSource({argument}).Token;", warns,
            "private static CancellationToken Pass(CancellationToken token) => token;");
    }

    [TestMethod]
    [DataRow("var tokens = new[] { cancellationToken }; ref CancellationToken slot = ref tokens[0]; tokens[0] = Pass(cancellationToken); slot = default;", "tokens", true)]
    [DataRow("var tokens = new[] { Pass(cancellationToken) }; CancellationToken[] copy = [..tokens];", "copy", false)]
    [DataRow("var tokens = new[] { default(CancellationToken) }; CancellationToken[] copy = [..tokens];", "copy", true)]
    [DataRow("var tokens = new[] { cancellationToken }; CancellationToken other = default; ref CancellationToken slot = ref (context != null ? ref tokens[0] : ref other); slot = default;", "tokens", true)]
    [DataRow("var tokens = new[] { cancellationToken }; CancellationToken other = default; ref CancellationToken slot = ref (context != null ? ref other : ref tokens[0]); slot = default;", "tokens", true)]
    [DataRow("var tokens = new[] { cancellationToken }; Span<CancellationToken> view = tokens; CancellationToken other = default; ref CancellationToken slot = ref (context != null ? ref view[0] : ref other); slot = default;", "tokens", true)]
    [DataRow("var tokens = new[] { cancellationToken }; var independent = new[] { cancellationToken }; CancellationToken other = default; ref CancellationToken slot = ref (context != null ? ref tokens[0] : ref other); slot = default;", "independent", false)]
    public async Task Collection_Summaries_Respect_Unknown_And_Escaped_Storage(string statements, string argument, bool warns)
    {
        await Verify($"{statements} cancellationToken = CancellationTokenSource.CreateLinkedTokenSource({argument}).Token;", warns,
            "private static CancellationToken Pass(CancellationToken token) => token;");
    }

    [TestMethod]
    [DataRow("ref CancellationToken slot = ref First(tokens);", "tokens", true)]
    [DataRow("Span<CancellationToken> view = tokens; ref CancellationToken slot = ref FirstSpan(view);", "tokens", true)]
    [DataRow("ref CancellationToken slot = ref FirstRef(ref tokens);", "tokens", true)]
    [DataRow("ref CancellationToken slot = ref First(tokens);", "independent", false)]
    [DataRow("ref CancellationToken slot = ref FirstTuple((tokens, 1));", "tokens", true)]
    [DataRow("ref readonly CancellationToken slot = ref ReadFirst(tokens);", "tokens", false)]
    public async Task Writable_Helper_References_Persist_Collection_Escapes(string binding, string argument, bool warns)
    {
        await Verify($"var tokens = new[] {{ cancellationToken }}; var independent = new[] {{ cancellationToken }}; {binding} tokens[0] = cancellationToken; {(binding.Contains("ref readonly") ? "" : "slot = default;")} cancellationToken = CancellationTokenSource.CreateLinkedTokenSource({argument}).Token;", warns,
            "private static ref CancellationToken First(CancellationToken[] items) => ref items[0]; private static ref CancellationToken FirstSpan(Span<CancellationToken> items) => ref items[0]; private static ref CancellationToken FirstRef(ref CancellationToken[] items) => ref items[0]; private static ref CancellationToken FirstTuple((CancellationToken[] Tokens, int Count) items) => ref items.Tokens[0]; private static ref readonly CancellationToken ReadFirst(CancellationToken[] items) => ref items[0];");
    }

    [TestMethod]
    [DataRow("cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(Pack(cancellationToken)).Token;", false)]
    [DataRow("cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(PackSpan(cancellationToken)).Token;", false)]
    [DataRow("cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(PackReadonly(cancellationToken)).Token;", false)]
    [DataRow("CancellationToken[] tokens = [..Pack(cancellationToken)]; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;", false)]
    [DataRow("cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(Pack(cancellationToken)).Token; cancellationToken = default;", true)]
    [DataRow("var tokens = Pack(cancellationToken); tokens = new CancellationToken[1]; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;", true)]
    public async Task Unsupported_Collection_Helpers_Preserve_Unknown_Provenance(string statements, bool warns)
    {
        await Verify(statements, warns,
            "private static CancellationToken[] Pack(CancellationToken token) => new[] { token }; private static Span<CancellationToken> PackSpan(CancellationToken token) => new CancellationToken[] { token }; private static ReadOnlySpan<CancellationToken> PackReadonly(CancellationToken token) => new CancellationToken[] { token };");
    }

    [TestMethod]
    [DataRow("var copied = First(tokens);", false)]
    [DataRow("First(tokens);", false)]
    [DataRow("var copied = FirstSpan(tokens.AsSpan());", false)]
    [DataRow("ref CancellationToken alias = ref (context != null ? ref (First(tokens)) : ref tokens[0]);", true)]
    [DataRow("ref readonly CancellationToken copied = ref (context != null ? ref First(tokens) : ref tokens[0]);", false)]
    [DataRow("ref readonly CancellationToken copied = ref First(tokens);", false)]
    [DataRow("var receiver = tokens; var copied = FirstRef(ref receiver);", false)]
    [DataRow("ref CancellationToken alias = ref First(tokens);", true)]
    public async Task Copied_Reference_Results_Allow_Facts_To_Be_Restored(string binding, bool warns)
    {
        await Verify($"var tokens = new[] {{ cancellationToken }}; {binding} tokens[0] = cancellationToken; {(warns ? "alias = default;" : "")} cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;", warns,
            "private static ref CancellationToken First(CancellationToken[] items) => ref items[0]; private static ref CancellationToken FirstRef(ref CancellationToken[] items) => ref items[0]; private static ref CancellationToken FirstSpan(Span<CancellationToken> items) => ref items[0];");
    }

    [TestMethod]
    [DataRow("cancellationToken = Pack(cancellationToken)[0];", false)]
    [DataRow("cancellationToken = PackSpan(cancellationToken)[0];", false)]
    [DataRow("cancellationToken = PackReadonly(cancellationToken)[0];", false)]
    [DataRow("cancellationToken = Pack(cancellationToken).AsSpan(0, 1)[0];", false)]
    [DataRow("cancellationToken = new ReadOnlySpan<CancellationToken>(Pack(cancellationToken), 0, 1)[0];", false)]
    [DataRow("CancellationToken[] tokens = [..Pack(cancellationToken)]; cancellationToken = tokens[0];", false)]
    [DataRow("var tokens = Pack(cancellationToken); cancellationToken = tokens[context == null ? 0 : 1];", false)]
    [DataRow("cancellationToken = Pack(cancellationToken)[0]; cancellationToken = default;", true)]
    [DataRow("var tokens = context != null ? Pack(cancellationToken) : new[] { cancellationToken }; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;", false)]
    [DataRow("var tokens = context != null ? new[] { cancellationToken } : Pack(cancellationToken); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;", false)]
    [DataRow("var tokens = context != null ? Pack(cancellationToken) : new[] { cancellationToken }; cancellationToken = tokens[0];", false)]
    [DataRow("var tokens = context != null ? Pack(cancellationToken) : new[] { cancellationToken }; tokens = new CancellationToken[1]; cancellationToken = tokens[0];", true)]
    public async Task Unsupported_Collection_Projections_And_Joins_Remain_Unknown(string statements, bool warns)
    {
        await Verify(statements, warns,
            "private static CancellationToken[] Pack(CancellationToken token) => new[] { token }; private static Span<CancellationToken> PackSpan(CancellationToken token) => new CancellationToken[] { token }; private static ReadOnlySpan<CancellationToken> PackReadonly(CancellationToken token) => new CancellationToken[] { token };");
    }

    [TestMethod]
    [DataRow("var slot = Forward(ref First(tokens));", false)]
    [DataRow("ref CancellationToken slot = ref Forward(ref First(tokens));", true)]
    [DataRow("ref readonly CancellationToken slot = ref Forward(ref First(tokens));", false)]
    [DataRow("ref readonly CancellationToken slot = ref tokens[0]; slot = ref (context != null ? ref First(tokens) : ref First(tokens));", false)]
    [DataRow("ref CancellationToken slot = ref tokens[0]; slot = ref (context != null ? ref First(tokens) : ref First(tokens));", true)]
    [DataRow("var slot = ReadFirst(ref tokens); tokens = new[] { cancellationToken };", false)]
    [DataRow("var slot = context != null ? First(tokens) : First(tokens);", false)]
    [DataRow("var holder = new Holder(ref First(tokens));", true)]
    public async Task Helper_Reference_Consumers_Determine_Persistent_Escapes(string binding, bool warns)
    {
        await Verify($"var tokens = new[] {{ cancellationToken }}; {binding} tokens[0] = cancellationToken; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;", warns,
            "private static ref CancellationToken First(CancellationToken[] items) => ref items[0]; private static ref readonly CancellationToken ReadFirst(ref CancellationToken[] items) => ref items[0]; private static ref CancellationToken Forward(ref CancellationToken token) => ref token; private ref struct Holder { public ref CancellationToken Slot; public Holder(ref CancellationToken token) { Slot = ref token; } }");
    }

    public TestContext TestContext { get; set; } = null!;

    private static Task Verify(string statements, bool warns, string helpers = "")
    {
        var source = Source(statements, warns ? "{|#0:Task.Delay(1, cancellationToken)|}" : "Task.Delay(1, cancellationToken)", helpers);
        return warns ? VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay"))
            : VerifyCS.VerifyAnalyzerAsync(source);
    }

    private static string Source(string statements, string call, string helpers = "") => $$"""
            {{TestSourceConstants.StandardModuleHeader}}
            #nullable disable
            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
                protected override Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                {
                    {{statements}}
                    return {{call}};
                }
                {{helpers}}
            }
            """;
}
