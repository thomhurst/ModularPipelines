using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleLiveRefCancellationAnalyzerTests
{
    [TestMethod]
    [DataRow("alias = default;", true)]
    [DataRow("cancellationToken = default;", true)]
    [DataRow("", false)]
    [DataRow("alias = cancellationToken;", false)]
    public async Task Checks_Live_Ref_In_Module_Loop(string mutation, bool reports)
    {
        var call = "Task.Delay(1, alias)";
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeader}}

            public class BuildModule : Module<List<string>>
            {
                protected override async Task<List<string>> ExecuteAsync(
                    IModuleContext context, CancellationToken cancellationToken)
                {
                    for (var i = 0; i < 2; i++)
                    {
                        ref CancellationToken alias = ref cancellationToken;
                        {{mutation}}
                        await {{(reports ? "{|#0:" + call + "|}" : call)}};
                    }
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
    [DataRow("cancellationToken = default;", true)]
    [DataRow("alias = default;", true)]
    [DataRow("", false)]
    public async Task Checks_Live_Ref_To_Captured_Token(string mutation, bool reports)
    {
        var call = "Task.Delay(1, alias)";
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeader}}

            public class BuildModule : Module<List<string>>
            {
                protected override async Task<List<string>> ExecuteAsync(
                    IModuleContext context, CancellationToken cancellationToken)
                {
                    Task Delay()
                    {
                        ref CancellationToken alias = ref cancellationToken;
                        {{mutation}}
                        return {{(reports ? "{|#0:" + call + "|}" : call)}};
                    }
                    for (var i = 0; i < 2; i++)
                        await Delay();
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
