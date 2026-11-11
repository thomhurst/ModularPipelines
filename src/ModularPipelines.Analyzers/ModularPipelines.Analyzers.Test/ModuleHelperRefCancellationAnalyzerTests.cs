using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleHelperRefCancellationAnalyzerTests
{
    [TestMethod]
    [DataRow("ref token, ref token", "", "second = default; return first;", true)]
    [DataRow("ref token, ref token", "", "first = default; return second;", true)]
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
    [DataRow("ref token, ref token", "", "first = second; return second;", false)]
    [DataRow("ref token, ref token", "", "second = ref first; return first;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = saved; return first;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; saved = default; second = saved; return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; second = saved; return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; var copy = saved; second = copy; return first;", false)]
    [DataRow("ref token, ref token", "", "second = Forward(ref first, ref second); return first;", true)]
    public async Task Checks_Helper_Ref_Argument_Storage(
        string arguments, string setup, string body, bool reports)
    {
        var call = "Task.Delay(1, snapshot)";
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeader}}

            public class BuildModule : Module<List<string>>
            {
                private static CancellationToken Forward(ref CancellationToken first, ref CancellationToken second)
                {
                    {{body}}
                }

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
    [TestMethod]
    [DataRow("for (var i = 0; i < 2; i++)", true)]
    [DataRow("while (context != null)", true)]
    [DataRow("if (context != null)", false)]
    public async Task Checks_Ref_Rebinding_After_Helper_Call(string control, bool reports)
    {
        var call = "Task.Delay(1, snapshot)";
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeader}}

            public class BuildModule : Module<List<string>>
            {
                private static CancellationToken Forward(ref CancellationToken first, ref CancellationToken second)
                {
                    second = default;
                    return first;
                }

                protected override async Task<List<string>> ExecuteAsync(
                    IModuleContext context, CancellationToken cancellationToken)
                {
                    var token = cancellationToken;
                    var other = cancellationToken;
                    ref CancellationToken alias = ref other;
                    var snapshot = token;
                    {{control}}
                    {
                        snapshot = Forward(ref token, ref alias);
                        alias = ref token;
                    }

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
