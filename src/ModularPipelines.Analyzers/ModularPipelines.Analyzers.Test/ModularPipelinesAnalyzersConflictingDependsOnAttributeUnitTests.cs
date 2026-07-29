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

    private const string IndirectCycleSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

[{{|#0:DependsOn<Module2>|}}]
public class Module1 : Module<List<string>>
{SimpleModuleBody}

[{{|#1:DependsOn<Module3>|}}]
public class Module2 : Module<List<string>>
{SimpleModuleBody}

[{{|#2:DependsOn<Module1>|}}]
public class Module3 : Module<List<string>>
{SimpleModuleBody}
";

    private const string NonGenericIndirectCycleSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

[{{|#0:DependsOn(typeof(Module2))|}}]
public class Module1 : Module<List<string>>
{SimpleModuleBody}

[{{|#1:DependsOn(typeof(Module3))|}}]
public class Module2 : Module<List<string>>
{SimpleModuleBody}

[{{|#2:DependsOn(typeof(Module1))|}}]
public class Module3 : Module<List<string>>
{SimpleModuleBody}
";

    private const string AcyclicChainSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

[DependsOn<Module2>]
public class Module1 : Module<List<string>>
{SimpleModuleBody}

[DependsOn<Module3>]
public class Module2 : Module<List<string>>
{SimpleModuleBody}

public class Module3 : Module<List<string>>
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
    public async Task AnalyzerIsTriggered_When_Indirect_Cycle_Exists()
    {
        var expected1 = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("Module2", "Module1");

        var expected2 = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(1)
            .WithArguments("Module3", "Module2");

        var expected3 = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(2)
            .WithArguments("Module1", "Module3");

        await VerifyCS.VerifyAnalyzerAsync(IndirectCycleSource, expected1, expected2, expected3);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_NonGeneric_Indirect_Cycle_Exists()
    {
        var expected1 = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("Module2", "Module1");

        var expected2 = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(1)
            .WithArguments("Module3", "Module2");

        var expected3 = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(2)
            .WithArguments("Module1", "Module3");

        await VerifyCS.VerifyAnalyzerAsync(
            NonGenericIndirectCycleSource,
            expected1,
            expected2,
            expected3);
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_When_Indirect_Dependency_Chain_Is_Acyclic()
    {
        await VerifyCS.VerifyAnalyzerAsync(AcyclicChainSource);
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_When_No_Conflicting_Dependencies()
    {
        await VerifyCS.VerifyAnalyzerAsync(GoodModuleSource);
    }
}
