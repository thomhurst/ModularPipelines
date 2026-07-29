using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpCodeFixVerifier<
    ModularPipelines.Analyzers.SelfDependencyAnalyzer,
    ModularPipelines.Analyzers.ConflictingDependsOnAttributeCodeFixProvider>;

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

    private const string FixedModuleSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

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

    private const string DocumentedBadModuleSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

/// <summary>
/// Runs the module.
/// </summary>
[{{|#0:DependsOn<Module1>|}}]
public class Module1 : Module<List<string>>
{SimpleModuleBody}
";

    private const string FixedDocumentedModuleSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

/// <summary>
/// Runs the module.
/// </summary>
public class Module1 : Module<List<string>>
{SimpleModuleBody}
";

    private const string TrailingCommentBadModuleSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

[{{|#0:DependsOn<Module1>|}}] // dependency explanation
public class Module1 : Module<List<string>>
{SimpleModuleBody}
";

    private const string FixedTrailingCommentModuleSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

 // dependency explanation
public class Module1 : Module<List<string>>
{SimpleModuleBody}
";

    private const string SharedAttributeListBadModuleSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

[Obsolete, {{|#0:DependsOn<Module1>|}} /* dependency explanation */]
public class Module1 : Module<List<string>>
{SimpleModuleBody}
";

    private const string FixedSharedAttributeListModuleSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

[Obsolete  /* dependency explanation */]
public class Module1 : Module<List<string>>
{SimpleModuleBody}
";

    private const string DuplicateSelfDependenciesSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

[{{|#0:DependsOn<Module1>|}}, {{|#1:DependsOn<Module1>|}}]
public class Module1 : Module<List<string>>
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

    [TestMethod]
    public async Task CodeFix_Removes_Self_Dependency()
    {
        var expected = VerifyCS.Diagnostic(SelfDependencyAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("Module1");

        await VerifyCS.VerifyCodeFixAsync(BadModuleSource, expected, FixedModuleSource);
    }

    [TestMethod]
    public async Task CodeFix_Preserves_Class_Documentation()
    {
        var expected = VerifyCS.Diagnostic(SelfDependencyAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("Module1");

        await VerifyCS.VerifyCodeFixAsync(
            DocumentedBadModuleSource,
            expected,
            FixedDocumentedModuleSource);
    }

    [TestMethod]
    public async Task CodeFix_Preserves_Trailing_Comment()
    {
        var expected = VerifyCS.Diagnostic(SelfDependencyAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("Module1");

        await VerifyCS.VerifyCodeFixAsync(
            TrailingCommentBadModuleSource,
            expected,
            FixedTrailingCommentModuleSource);
    }

    [TestMethod]
    public async Task CodeFix_Preserves_Comment_In_Shared_Attribute_List()
    {
        var expected = VerifyCS.Diagnostic(SelfDependencyAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("Module1");

        await VerifyCS.VerifyCodeFixAsync(
            SharedAttributeListBadModuleSource,
            expected,
            FixedSharedAttributeListModuleSource);
    }

    [TestMethod]
    public async Task CodeFix_Removes_CoLocated_Self_Dependencies()
    {
        var expected = new[]
        {
            VerifyCS.Diagnostic(SelfDependencyAnalyzer.DiagnosticId)
                .WithLocation(0)
                .WithArguments("Module1"),
            VerifyCS.Diagnostic(SelfDependencyAnalyzer.DiagnosticId)
                .WithLocation(1)
                .WithArguments("Module1"),
        };

        await VerifyCS.VerifyCodeFixAsync(
            DuplicateSelfDependenciesSource,
            expected,
            FixedModuleSource);
    }
}
