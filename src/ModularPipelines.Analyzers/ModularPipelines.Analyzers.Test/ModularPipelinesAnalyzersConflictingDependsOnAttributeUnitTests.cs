using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ConflictingDependsOnAttributeAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModularPipelinesAnalyzersConflictingDependsOnAttributeUnitTests
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

[{{|#0:DependsOn<Module2>|}}]
public class Module1 : Module<List<string>>
{SimpleModuleBody}

[{{|#1:DependsOn<Module1>|}}]
public class Module2 : Module<List<string>>
{SimpleModuleBody}
";

    private const string BadModuleSource2 = $@"
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
    public async Task AnalyzerIsTriggered_When_Conflicting_Dependencies()
    {
        var expected1 = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("Module2", "Module1");

        var expected2 = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(1)
            .WithArguments("Module1", "Module2");

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSource, expected1, expected2);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_Dependency_Depends_On_Self()
    {
        var expected = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("Module1", "Module1");

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSource2, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_When_No_Conflicting_Dependencies()
    {
        await VerifyCS.VerifyAnalyzerAsync(GoodModuleSource);
    }
}