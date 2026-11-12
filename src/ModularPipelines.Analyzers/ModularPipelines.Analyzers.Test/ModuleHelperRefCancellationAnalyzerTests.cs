using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleHelperRefCancellationAnalyzerTests
{
    [TestMethod]
    [DataRow("ref token, ref token", "", "second = default; return first;", true)]
    [DataRow("second: ref token, first: ref token", "", "second = default; return first;", true)]
    [DataRow("ref token, ref alias", "ref CancellationToken alias = ref token;", "second = default; return first;", true)]
    [DataRow("ref alias, ref token", "ref CancellationToken alias = ref token;", "second = default; return first;", true)]
    [DataRow("ref token, ref alias", "ref CancellationToken alias = ref token; ref CancellationToken chained = ref alias; alias = ref chained;", "second = default; return first;", true)]
    [DataRow("ref token, ref token", "", "return first;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "second = default; return first;", false)]
    [DataRow("second: ref other, first: ref token", "CancellationToken other = default;", "second = default; return first;", false)]
    [DataRow("ref token, ref alias", "CancellationToken other = default; ref CancellationToken alias = ref other;", "second = default; return first;", false)]
    [DataRow("ref token, ref token", "", "var original = first; second = default; return original;", false)]
    [DataRow("ref token, ref token", "", "if (false) second = default; return first;", false)]
    [DataRow("ref token, ref alias", "CancellationToken other = default; ref CancellationToken alias = ref token; alias = ref other;", "second = default; return first;", false)]
    [DataRow("ref token, ref alias", "CancellationToken other = default; ref CancellationToken alias = ref other; alias = ref token;", "second = default; return first;", true)]
    [DataRow("ref token, ref token", "", "second = first; return first;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = saved; return first;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; saved = default; second = saved; return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; second = saved; return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; var copy = saved; second = copy; return first;", false)]
    [DataRow("ref token, ref token", "", "second = Forward(ref first, ref second); return first;", true)]
    [DataRow("ref token, ref token", "", "ref CancellationToken alias = ref second; return first;", false)]
    [DataRow("ref token, ref token", "", "var stored = first; ref CancellationToken alias = ref stored; alias = default; second = stored; return first;", true)]
    [DataRow("ref token, ref token", "", "var stored = first; ref CancellationToken alias = ref stored; second = stored; return first;", false)]
    [DataRow("ref token, ref token", "", "var stored = first; ref CancellationToken alias = ref stored; alias = default; stored = first; second = stored; return first;", false)]
    [DataRow("ref token, ref token", "", "ref CancellationToken alias = ref second; alias = default; return first;", true)]
    [DataRow("ref token, ref token", "", "ref CancellationToken alias = ref first; alias = ref second; alias = default; return first;", true)]
    [DataRow("ref token, ref token", "", "ref CancellationToken alias = ref second; var saved = first; alias = default; return saved;", false)]
    [DataRow("ref token, ref token", "", "ref CancellationToken alias = ref second; alias = default; var saved = first; return saved;", true)]
    [DataRow("ref token, ref token", "", "int count = 0; ref int alias = ref count; alias = 0; return first;", false)]
    [DataRow("ref token, ref token", "", "ref int count = ref Count; count = 0; return first;", false)]
    [DataRow("ref token, ref token", "", "if (Flag) { var saved = first; second = default; return saved; } return first;", false)]
    [DataRow("ref token, ref token", "", "if (Flag) { second = default; return first; } return first;", true)]
    [DataRow("ref token, ref token", "", "if (Flag) { second = default; } return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; if (Flag) return first; second = default; return saved;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "int count = 0; (first, count) = (first, 1); return first;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "(first, second) = (second, first); return first;", true)]
    [DataRow("ref token, ref Field", "", "second = default; return first;", false)]
    [DataRow("ref token, ref token", "", "int count = 0; (second, count) = (first, 1); return first;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; int count = 0; (count, (second, count)) = (1, (saved, 2)); return first;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; saved = default; int count = 0; (second, count) = (saved, 1); return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; ref CancellationToken alias = ref saved; alias = default; return saved;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; switch (Flag) { case true: second = default; return saved; default: return first; }", false)]
    [DataRow("ref token, ref token", "", "CancellationToken extra = default; ref CancellationToken alias = ref extra; alias = default; return CancellationTokenSource.CreateLinkedTokenSource(first, extra).Token;", false)]
    [DataRow("ref token, ref token", "", "return Identity(ref second, ref first);", false)]
    [DataRow("ref token, ref token", "", "var saved = first; ref CancellationToken alias = ref saved; alias = first; return saved;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; ref CancellationToken alias = ref saved; second = default; alias = first; return saved;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; ref CancellationToken alias = ref saved; alias = default; return CancellationTokenSource.CreateLinkedTokenSource(saved, first).Token;", false)]
    [DataRow("ref token, ref token", "", "switch (Flag) { case true: second = default; goto default; default: return first; }", true)]
    [DataRow("ref token, ref token", "", "switch (Flag) { case true: second = default; break; default: break; } return first;", true)]
    [DataRow("ref token, ref token", "", "return IdentityValue(second = default, first);", true)]
    [DataRow("ref token, ref token", "", "second = CancellationTokenSource.CreateLinkedTokenSource(first, CancellationToken.None).Token; return first;", false)]
    [DataRow("ref token, ref token", "", "second = IdentityValue(default, first); return first;", false)]
    [DataRow("ref token, ref token", "", "second = default; second = IdentityValue(default, first); return first;", true)]
    public async Task Checks_Helper_Ref_Argument_Storage(
        string arguments, string setup, string body, bool reports)
        => await VerifyStorage(arguments, setup, body, reports, firstByRef: true);

    [TestMethod]
    [DataRow("token, ref token", "", "second = default; return first;", false)]
    [DataRow("token, ref Field", "", "second = default; return first;", false)]
    [DataRow("token, ref Field", "", "first = default; return first;", true)]
    [DataRow("token, ref Field", "", "ref CancellationToken alias = ref first; alias = default; return first;", true)]
    [DataRow("token, ref Field", "", "var saved = first; ref CancellationToken alias = ref first; alias = default; return saved;", false)]
    [DataRow("token, ref Field", "", "var saved = first; second = saved; return first;", false)]
    [DataRow("token, ref Field", "", "int count = 0; (second, count) = (default(CancellationToken), 1); return first;", false)]
    [DataRow("token, ref Field", "", "int count = 0; (first, count) = (default(CancellationToken), 1); return first;", true)]
    [DataRow("token, ref Field", "", "var saved = first; int count = 0; (first, count) = (saved, 1); return first;", false)]
    [DataRow("token, ref Field", "", "first = CancellationTokenSource.CreateLinkedTokenSource(first, CancellationToken.None).Token; return first;", false)]
    [DataRow("token, ref Field", "", "first = IdentityValue(default, first); return first;", false)]
    [DataRow("token, ref Field", "", "first = default; first = CancellationTokenSource.CreateLinkedTokenSource(first, CancellationToken.None).Token; return first;", true)]
    [DataRow("token, ref Field", "", "return Flag ? first : CancellationTokenSource.CreateLinkedTokenSource(Forward(first, ref second), first).Token;", false)]
    [DataRow("token, ref Field", "", "return Link(first, 2);", false)]
    [DataRow("token, ref Field", "", "return Mutual(first, 2);", false)]
    [DataRow("token, ref Field", "", "first = IdentityValue(first, default); return first;", true)]
    public async Task Checks_Helper_Private_Value_Parameter_Storage(
        string arguments, string setup, string body, bool reports)
        => await VerifyStorage(arguments, setup, body, reports, firstByRef: false);

    private static async Task VerifyStorage(string arguments, string setup, string body, bool reports, bool firstByRef)
    {
        var call = "Task.Delay(1, snapshot)";
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeader}}

            public class BuildModule : Module<List<string>>
            {
                private static int Count;
                private static bool Flag;
                private static CancellationToken Field;

                private static CancellationToken Forward({{(firstByRef ? "scoped ref " : "")}}CancellationToken first, scoped ref CancellationToken second)
                {
                    {{body}}
                }

                private static CancellationToken Identity(scoped ref CancellationToken ignored, scoped ref CancellationToken value) => value;

                private static CancellationToken IdentityValue(CancellationToken ignored, CancellationToken value) => value;

                private static CancellationToken Link(CancellationToken token, int remaining)
                    => remaining == 0 ? token : CancellationTokenSource.CreateLinkedTokenSource(Link(token, remaining - 1), token).Token;

                private static CancellationToken Mutual(CancellationToken token, int remaining)
                    => remaining == 0 ? token : CancellationTokenSource.CreateLinkedTokenSource(MutualOther(token, remaining - 1), token).Token;

                private static CancellationToken MutualOther(CancellationToken token, int remaining) => Mutual(token, remaining);


                protected override async Task<List<string>> ExecuteAsync(
                    IModuleContext context, CancellationToken cancellationToken)
                {
                    var token = cancellationToken;
                    {{setup}}
                    var snapshot = Forward({{arguments}});
                    for (var i = 0; i < 2; i++)
                        await {{(reports ? "{|#0:" + call + "|}" : call)}};
                    return null!;
                }
            }
            """;

        if (reports)
        {
            await VerifyCS.VerifyAnalyzerAsync(source,
                VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                    .WithLocation(0).WithArguments("Delay"));
        }
        else
        {
            await VerifyCS.VerifyAnalyzerAsync(source);
        }
    }

}
