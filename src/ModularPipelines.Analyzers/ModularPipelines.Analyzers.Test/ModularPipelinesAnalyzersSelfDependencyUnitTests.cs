using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpCodeFixVerifier<
    ModularPipelines.Analyzers.SelfDependencyAnalyzer,
    ModularPipelines.Analyzers.ConflictingDependsOnAttributeCodeFixProvider>;
using VerifyDuplicateCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpCodeFixVerifier<
    ModularPipelines.Analyzers.Test.DuplicateDependencyDiagnosticAnalyzer,
    ModularPipelines.Analyzers.ConflictingDependsOnAttributeCodeFixProvider>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModularPipelinesAnalyzersSelfDependencyUnitTests
{
    private const string SimpleModuleBody = @"
{
    protected override async Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

    private const string AliasedBadModuleSource = $@"
{TestSourceConstants.StandardUsingsWithLogging}
using Dependency = ModularPipelines.Attributes.DependsOnAttribute;

{TestSourceConstants.ExamplesNamespace}

[{{|#0:Dependency(typeof(Module1))|}}]
public class Module1 : Module<List<string>>
{SimpleModuleBody}
";

    private const string SuffixElidedAliasBadModuleSource = $@"
{TestSourceConstants.StandardUsingsWithLogging}
using DependencyAttribute = ModularPipelines.Attributes.DependsOnAttribute;

{TestSourceConstants.ExamplesNamespace}

[{{|#0:Dependency(typeof(Module1))|}}]
public class Module1 : Module<List<string>>
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

    private const string DuplicateDiagnosticsSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

[Obsolete, {{|#0:DependsOn<Module1>|}}]
public class Module1 : Module<List<string>>
{SimpleModuleBody}
";

    private const string FixedDuplicateDiagnosticsSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

[Obsolete ]
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
    public async Task AnalyzerIsTriggered_When_SelfDependency_UsesAlias()
    {
        var expected = VerifyCS.Diagnostic(SelfDependencyAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("Module1");

        await VerifyCS.VerifyAnalyzerAsync(AliasedBadModuleSource, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_AliasUsesAttributeSuffixElision()
    {
        var expected = VerifyCS.Diagnostic(SelfDependencyAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("Module1");

        await VerifyCS.VerifyAnalyzerAsync(SuffixElidedAliasBadModuleSource, expected);
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

    [TestMethod]
    public async Task CodeFix_Deduplicates_Diagnostics_For_One_Attribute()
    {
        var expected = new[]
        {
            VerifyDuplicateCS.Diagnostic(SelfDependencyAnalyzer.DiagnosticId)
                .WithLocation(0),
            VerifyDuplicateCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
                .WithLocation(0),
        };

        await VerifyDuplicateCS.VerifyCodeFixAsync(
            DuplicateDiagnosticsSource,
            expected,
            FixedDuplicateDiagnosticsSource);
    }
}

[DiagnosticAnalyzer(LanguageNames.CSharp)]
#pragma warning disable RS1036, RS2008
public sealed class DuplicateDependencyDiagnosticAnalyzer : DiagnosticAnalyzer
{
    private static readonly DiagnosticDescriptor SelfDependencyRule = new(
        SelfDependencyAnalyzer.DiagnosticId,
        "Self dependency",
        "Self dependency",
        "Test",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor ConflictingDependencyRule = new(
        ConflictingDependsOnAttributeAnalyzer.DiagnosticId,
        "Conflicting dependency",
        "Conflicting dependency",
        "Test",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        [SelfDependencyRule, ConflictingDependencyRule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(
            static syntaxContext =>
            {
                if (!syntaxContext.Node.ToString().StartsWith(
                        "DependsOn",
                        StringComparison.Ordinal))
                {
                    return;
                }

                var location = syntaxContext.Node.GetLocation();
                syntaxContext.ReportDiagnostic(Diagnostic.Create(
                    SelfDependencyRule,
                    location));
                syntaxContext.ReportDiagnostic(Diagnostic.Create(
                    ConflictingDependencyRule,
                    location));
            },
            SyntaxKind.Attribute);
    }
}
#pragma warning restore RS1036, RS2008
