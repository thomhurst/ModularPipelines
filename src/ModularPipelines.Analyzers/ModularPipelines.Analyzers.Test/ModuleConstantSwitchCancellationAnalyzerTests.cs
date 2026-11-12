using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleConstantSwitchCancellationAnalyzerTests
{
    [TestMethod]
    [DataRow("true switch { true => first, false => default }", false)]
    [DataRow("false switch { true => default, false => first }", false)]
    [DataRow("true switch { true => default, false => first }", true)]
    [DataRow("false switch { true => first, false => default }", true)]
    [DataRow("(Environment.TickCount > 0) switch { true => first, false => default }", true)]
    [DataRow("(Environment.TickCount > 0) switch { true => first, false => first }", false)]
    [DataRow("true switch { true => first, false => throw new Exception() }", false)]
    [DataRow("false switch { true => throw new Exception(), false => first }", false)]
    [DataRow("(Environment.TickCount > 0) switch { true => first, false => throw new Exception() }", false)]
    [DataRow("true switch { true => CancellationTokenSource.CreateLinkedTokenSource(first).Token, false => default }", false)]
    [DataRow("false switch { true => default, false => CancellationTokenSource.CreateLinkedTokenSource(first).Token }", false)]
    [DataRow("1 switch { 1 => first, _ => default }", false)]
    [DataRow("2 switch { 1 => first, _ => default }", true)]
    [DataRow("(Environment.TickCount > 0 ? true : false) switch { true => first, false => default }", true)]
    [DataRow("(Environment.TickCount > 0 ? true : true) switch { true => first, false => default }", false)]
    public async Task Tracks_Selected_Arm_In_Helper_Parameter_Assignment(string expression, bool warns)
    {
        var call = "Task.Delay(1, Forward(cancellationToken))";
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeader}}
            public class BuildModule : Module<List<string>>
            {
                protected override async Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                {
                    while (Environment.TickCount > 0)
                    {
                        await {{(warns ? "{|#0:" + call + "|}" : call)}};
                    }

                    return null!;
                }

                private static CancellationToken Forward(CancellationToken first)
                {
                    first = {{expression}};
                    return first;
                }
            }
            """;
        await VerifyCS.VerifyAnalyzerAsync(source, warns
            ? [VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay")]
            : []);
    }
}
