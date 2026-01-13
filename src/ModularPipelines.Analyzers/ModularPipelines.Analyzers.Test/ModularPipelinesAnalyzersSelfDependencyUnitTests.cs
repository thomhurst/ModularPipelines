using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.SelfDependencyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModularPipelinesAnalyzersSelfDependencyUnitTests
{
    private const string SimpleModuleBody = @"
{
    protected override async Task<List<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return new List<string>();
    }
}";

    private const string BadModuleSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

[{{|#0:DependsOn<Module1>|}}]
public class Module1 : Module<List<string>>
{SimpleModuleBody}
";

    private const string GoodModuleSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{SimpleModuleBody}

[DependsOn<Module1>]
public class Module2 : Module<List<string>>
{SimpleModuleBody}
";

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_Module_Depends_On_Self()
    {
        var expected = VerifyCS.Diagnostic(SelfDependencyAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("Module1");

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSource, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_When_Module_Does_Not_Depend_On_Self()
    {
        await VerifyCS.VerifyAnalyzerAsync(GoodModuleSource);
    }
}
