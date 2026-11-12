using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

/// <summary>
/// The MP0016 token-flow contract from docs/docs/how-to/analyzers.md: a passed token is reported
/// only when it is lost on every path, and every shape outside that contract stays silent.
/// </summary>
[TestClass]
public class ModuleCancellationTokenFlowAnalyzerTests
{
    [TestMethod]
    [DataRow("CancellationToken.None")]
    [DataRow("default")]
    [DataRow("default(CancellationToken)")]
    [DataRow("new CancellationToken()")]
    [DataRow("new CancellationTokenSource().Token")]
    [DataRow("new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token")]
    [DataRow("context != null ? CancellationToken.None : default")]
    public async Task Reports_Lost_Token_Argument(string argument)
    {
        await Verify(string.Empty, argument, warns: true);
    }

    [TestMethod]
    [DataRow("var token = CancellationToken.None;")]
    [DataRow("var first = CancellationToken.None; var token = first;")]
    [DataRow("var source = new CancellationTokenSource(); var token = source.Token;")]
    [DataRow("var token = cancellationToken; token = default;")]
    [DataRow("CancellationToken token; if (context != null) token = default; else token = CancellationToken.None;")]
    [DataRow("var token = context != null ? default : CancellationToken.None;")]
    [DataRow("var token = cancellationToken; do { token = default; } while (context == null);")]
    [DataRow("var token = cancellationToken; try { } finally { token = default; }")]
    [DataRow("var token = cancellationToken; try { throw new Exception(); } catch { token = default; }")]
    public async Task Reports_Variable_Lost_On_Every_Path(string statements)
    {
        await Verify(statements, "token", warns: true);
    }

    [TestMethod]
    public async Task Reports_Overwritten_Parameter()
    {
        await Verify("cancellationToken = CancellationToken.None;", "cancellationToken", warns: true);
    }

    [TestMethod]
    [DataRow("var token = cancellationToken;")]
    [DataRow("var token = CancellationToken.None; token = cancellationToken;")]
    [DataRow("var token = CancellationToken.None; if (context != null) token = cancellationToken;")]
    [DataRow("var token = cancellationToken; if (context != null) token = CancellationToken.None;")]
    [DataRow("var token = context != null ? cancellationToken : CancellationToken.None;")]
    [DataRow("var token = cancellationToken; if (context == null) { token = default; return Task.CompletedTask; }")]
    [DataRow("CancellationToken? maybe = null; var token = maybe ?? CancellationToken.None;")]
    [DataRow("var token = CancellationToken.None; for (var i = 0; i < 2; i++) { token = cancellationToken; }")]
    [DataRow("var token = CancellationToken.None; try { token = cancellationToken; } catch { }")]
    [DataRow("var token = CancellationToken.None; try { } finally { token = cancellationToken; }")]
    [DataRow("var token = CancellationToken.None; try { } finally { if ((token = cancellationToken).CanBeCanceled) { _ = context; } }")]
    [DataRow("using var source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken); var token = source.Token;")]
    [DataRow("using var source = CancellationTokenSource.CreateLinkedTokenSource(CancellationToken.None); var token = source.Token;")]
    [DataRow("var source = new CancellationTokenSource(); cancellationToken.Register(() => source.Cancel()); var token = source.Token;")]
    [DataRow("var source = new CancellationTokenSource(); cancellationToken.Register(source.Cancel); var token = source.Token;")]
    [DataRow("var source = new CancellationTokenSource(); Wire(source, cancellationToken); var token = source.Token;")]
    public async Task Does_Not_Report_Token_Kept_On_Some_Path(string statements)
    {
        await Verify(statements, "token", warns: false);
    }

    [TestMethod]
    [DataRow("var token = CancellationToken.None; Set(ref token);")]
    [DataRow("var token = CancellationToken.None; ref var alias = ref token; alias = cancellationToken;")]
    [DataRow("var tokens = new[] { CancellationToken.None }; var token = tokens[0];")]
    [DataRow("var pair = (CancellationToken.None, 1); var token = pair.Item1;")]
    [DataRow("var (token, _) = (CancellationToken.None, 1);")]
    [DataRow("var token = Pass(CancellationToken.None);")]
    [DataRow("var token = _field;")]
    [DataRow("static CancellationToken Pick(CancellationToken value, bool flag) => (flag ? true : true) switch { true => value, false => default }; var token = Pick(cancellationToken, true);")]
    [DataRow("var token = CancellationToken.None; void Restore() => token = cancellationToken; Restore();")]
    [DataRow("static CancellationToken Replace(CancellationToken value) { value = default; return value; } var token = Replace(cancellationToken);")]
    public async Task Does_Not_Report_Shapes_Outside_The_Contract(string statements)
    {
        await Verify(statements, "token", warns: false);
    }

    [TestMethod]
    [DataRow("var token = CancellationToken.None;", true)]
    [DataRow("var token = cancellationToken;", false)]
    [DataRow("var token = CancellationToken.None; token = cancellationToken;", false)]
    [DataRow("var token = cancellationToken; token = CancellationToken.None;", false)]
    public async Task Follows_Captured_Local_Only_When_Never_Reassigned(string statements, bool warns)
    {
        var call = warns ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)";
        await VerifyCS.VerifyAnalyzerAsync(
            Source(statements, $"Task.Run(() => {call}, cancellationToken)"),
            warns ? [Expected()] : []);
    }

    [TestMethod]
    [DataRow("WaitByReference(in cancellationToken)")]
    [DataRow("WaitBoth(cancellationToken, cancellationToken = default)")]
    public async Task Does_Not_Report_Token_Passed_By_Reference_Or_Before_A_Later_Write(string call)
    {
        await VerifyCS.VerifyAnalyzerAsync(Source(string.Empty, call));
    }

    [TestMethod]
    public async Task Reports_Lost_Token_Passed_To_In_Parameter()
    {
        await VerifyCS.VerifyAnalyzerAsync(
            Source(string.Empty, "{|#0:WaitByReference(CancellationToken.None)|}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("WaitByReference"));
    }

    [TestMethod]
    [Timeout(60_000)]
    public async Task Analyzes_Captured_Local_Chains_In_Linear_Time()
    {
        var locals = string.Concat(Enumerable.Range(1, 40).Select(static index =>
            $"var t{index} = context != null ? t{index - 1} : t{index - 1}; "));
        await VerifyCS.VerifyAnalyzerAsync(
            Source(
                "var t0 = CancellationToken.None; " + locals,
                "Task.Run(() => {|#0:Task.Delay(1, t40)|}, cancellationToken)"),
            Expected());
    }

    [TestMethod]
    [Timeout(60_000)]
    public async Task Analyzes_Repeated_Helper_Chains_In_Linear_Time()
    {
        var helpers = string.Concat(Enumerable.Range(0, 20).Select(static index =>
            $"CancellationToken Keep{index}(CancellationToken token) => Keep{index + 1}(Keep{index + 1}(token)); "));
        await Verify(
            helpers + "CancellationToken Keep20(CancellationToken token) => token; cancellationToken = Keep0(cancellationToken);",
            "cancellationToken",
            warns: false);
    }

    private static Task Verify(string statements, string argument, bool warns)
    {
        var call = $"Task.Delay(1, {argument})";
        return VerifyCS.VerifyAnalyzerAsync(
            Source(statements, warns ? "{|#0:" + call + "|}" : call),
            warns ? [Expected()] : []);
    }

    private static Microsoft.CodeAnalysis.Testing.DiagnosticResult Expected() =>
        VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay");

    private static string Source(string statements, string call) => $$"""
        {{TestSourceConstants.StandardModuleHeader}}
        public class BuildModule : Module<List<string>>
        {
            private CancellationToken _field;
            private static CancellationToken Pass(CancellationToken token) => token;
            private static void Set(ref CancellationToken token) { }
            private static Task WaitByReference(in CancellationToken token) => Task.CompletedTask;
            private static Task WaitBoth(CancellationToken first, CancellationToken second) => Task.CompletedTask;
            private static void Wire(CancellationTokenSource source, CancellationToken token) => token.Register(source.Cancel);
            {{TestSourceConstants.SimpleAsyncExecuteBody}}
            protected override Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            {
                {{statements}}
                return {{call}};
            }
        }
        """;
}
