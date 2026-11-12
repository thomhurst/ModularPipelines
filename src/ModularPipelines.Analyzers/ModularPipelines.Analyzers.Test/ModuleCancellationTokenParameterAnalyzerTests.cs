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
    [DataRow("if (context != null) cancellationToken = CancellationToken.None;", false)]
    [DataRow("cancellationToken = context != null ? cancellationToken : CancellationToken.None;", false)]
    [DataRow("cancellationToken = cancellationToken;", false)]
    [DataRow("var saved = cancellationToken; cancellationToken = default; cancellationToken = saved;", false)]
    [DataRow("var saved = cancellationToken; var copy = saved; saved = default; cancellationToken = copy;", false)]
    [DataRow("var saved = cancellationToken; if (context != null) cancellationToken = default; cancellationToken = saved;", false)]
    [DataRow("if (false) cancellationToken = default;", false)]
    [DataRow("if (true) cancellationToken = default;", false)]
    [DataRow("if (context != null) { cancellationToken = default; return Task.CompletedTask; }", false)]
    public async Task Tracks_Reachable_Scalar_Writes(string statements, bool warns)
    {
        await Verify(statements, false, warns);
    }

    [TestMethod]
    [DataRow("cancellationToken = CancellationToken.None;")]
    [DataRow("cancellationToken = new CancellationTokenSource().Token;")]
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
    [DataRow("var saved = cancellationToken; cancellationToken = default; void Restore() => cancellationToken = saved; Restore();")]
    [DataRow("var saved = cancellationToken; cancellationToken = default; Action restore = () => cancellationToken = saved; restore();")]
    [DataRow("var saved = default(CancellationToken); void Restore() => saved = cancellationToken; Restore(); cancellationToken = saved;")]
    public async Task Captured_Writes_Keep_Existing_Behavior(string statements)
    {
        await Verify(statements, false, false);
    }

    [TestMethod]
    [DataRow("var saved = cancellationToken; cancellationToken = default; var slots = new Dictionary<CancellationToken, int>(); slots[cancellationToken = saved] = 0;", false)]
    [DataRow("var saved = cancellationToken; cancellationToken = default; var slots = new int[1]; slots[(cancellationToken = saved).GetHashCode() & 0] = 0;", false)]
    [DataRow("var saved = cancellationToken; cancellationToken = default; var slots = new Dictionary<CancellationToken, int>(); (new[] { slots })[(cancellationToken = saved).GetHashCode() & 0][default] = 0;", false)]
    [DataRow("var saved = cancellationToken; cancellationToken = default; var slots = new Dictionary<CancellationToken, int>(); slots[cancellationToken = default] = (cancellationToken = saved).GetHashCode();", false)]
    [DataRow("var saved = cancellationToken; cancellationToken = default; var slots = new Dictionary<CancellationToken, int>(); slots[cancellationToken = saved] = (cancellationToken = default).GetHashCode();", true)]
    public async Task Tracks_Assignment_Target_Side_Effects_In_Evaluation_Order(string statements, bool warns)
    {
        await Verify(statements, false, warns);
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

    [TestMethod]
    [DataRow("token = default;", false)]
    [DataRow("", false)]
    [DataRow("token = token;", false)]
    [DataRow("var saved = token; token = default; token = saved;", false)]
    [DataRow("if (false) token = default;", false)]
    [DataRow("if (token.CanBeCanceled) token = default;", false)]
    public async Task Tracks_Local_Helper_Own_Parameter(string statements, bool warns)
    {
        await Verify("static CancellationToken Replace(CancellationToken token) { " + statements
            + " return token; } cancellationToken = Replace(cancellationToken);", false, warns);
    }

    [TestMethod]
    public async Task Preserves_Local_Helper_Saved_Return_Value()
    {
        await Verify("static CancellationToken Replace(CancellationToken token) { var saved = token; token = default; return saved; } "
            + "cancellationToken = Replace(cancellationToken);", false, false);
    }

    [TestMethod]
    [DataRow("var saved = cancellationToken; CancellationToken Restore(CancellationToken ignored) => saved;")]
    [DataRow("CancellationToken Restore(CancellationToken ignored) => cancellationToken;")]
    public async Task Captured_Helper_Inputs_Keep_Existing_Proof(string helper)
    {
        await Verify(helper + " cancellationToken = Restore(cancellationToken);", false, false);
    }

    [TestMethod]
    [DataRow("", false)]
    [DataRow("token = default;", false)]
    public async Task Tracks_Generic_Helper_Definition_Parameter(string statements, bool warns)
    {
        await Verify("static CancellationToken Keep<T>(CancellationToken token) { " + statements
            + " return token; } cancellationToken = Keep<int>(cancellationToken);", false, warns);
    }

    [TestMethod]
    [Timeout(30000, CooperativeCancellation = true)]
    public async Task Repeated_Helper_Chains_Have_Bounded_Cost()
    {
        var helpers = string.Concat(Enumerable.Range(0, 20).Select(i =>
            $"CancellationToken Keep{i}(CancellationToken token) {{ token = Keep{i + 1}(Keep{i + 1}(token)); return token; }} "));
        await Verify(helpers + "CancellationToken Keep20(CancellationToken token) => token; "
            + "cancellationToken = Keep0(cancellationToken);", false, false);
    }

    [TestMethod]
    [DataRow("token = default;", true)]
    [DataRow("token = token;", false)]
    [DataRow("var saved = token; token = default; token = saved;", false)]
    public async Task Tracks_Callback_Own_Parameter(string statements, bool warns)
    {
        var call = warns ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)";
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeader}}
            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
                protected override void Configure(ModuleConfigurationBuilder module) =>
                    module.WithSkipWhen(async (context, token) =>
                    {
                        {{statements}}
                        await {{call}};
                        return false;
                    });
            }
            """;
        await VerifyCS.VerifyAnalyzerAsync(source, warns
            ? [VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay")]
            : []);
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
