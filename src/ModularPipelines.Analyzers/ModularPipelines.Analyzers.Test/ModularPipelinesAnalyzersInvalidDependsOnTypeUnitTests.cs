using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpCodeFixVerifier<
    ModularPipelines.Analyzers.InvalidDependsOnTypeAnalyzer,
    ModularPipelines.Analyzers.ConflictingDependsOnAttributeCodeFixProvider>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModularPipelinesAnalyzersInvalidDependsOnTypeUnitTests
{
    private const string SimpleModuleBody = @"
{
    protected override async Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return new List<string>();
    }
}";

    private const string BadModuleSourceGeneric = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class NotAModule {{ }}

[{{|#0:DependsOn(typeof(NotAModule))|}}]
public class Module1 : Module<List<string>>
{SimpleModuleBody}
";

    private const string FixedModuleSourceGeneric = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class NotAModule {{ }}

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

    private const string AliasedBadModuleSource = $@"
{TestSourceConstants.StandardUsingsWithLogging}
using Dependency = ModularPipelines.DependsOnAttribute;

{TestSourceConstants.ExamplesNamespace}

public class NotAModule {{ }}

[{{|#0:Dependency(typeof(NotAModule))|}}]
public class Module1 : Module<List<string>>
{SimpleModuleBody}
";

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_DependsOn_NonModule_Type_Generic()
    {
        var expected = VerifyCS.Diagnostic(InvalidDependsOnTypeAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("NotAModule");

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSourceGeneric, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_When_DependsOn_Valid_Module()
    {
        await VerifyCS.VerifyAnalyzerAsync(GoodModuleSource);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_DependsOn_UsesAlias()
    {
        var expected = VerifyCS.Diagnostic(InvalidDependsOnTypeAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("NotAModule");

        await VerifyCS.VerifyAnalyzerAsync(AliasedBadModuleSource, expected);
    }

    [TestMethod]
    public async Task CodeFix_Removes_Invalid_Dependency()
    {
        var expected = VerifyCS.Diagnostic(InvalidDependsOnTypeAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("NotAModule");

        await VerifyCS.VerifyCodeFixAsync(BadModuleSourceGeneric, expected, FixedModuleSourceGeneric);
    }
}
