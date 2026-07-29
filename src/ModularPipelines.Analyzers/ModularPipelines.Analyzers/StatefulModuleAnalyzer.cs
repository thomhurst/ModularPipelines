using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using ModularPipelines.Analyzers.Extensions;

namespace ModularPipelines.Analyzers;

/// <summary>
/// Analyzer that detects mutable instance fields in modules.
/// Modules are registered as Singletons, so any instance state can leak between executions.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public class StatefulModuleAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// Diagnostic ID for mutable instance fields in modules.
    /// </summary>
    public const string DiagnosticId = "MP0008";

    /// <summary>
    /// Gets the diagnostic rule for stateful modules.
    /// </summary>
    public static DiagnosticDescriptor Rule { get; } = DiagnosticDescriptorFactory.Create(
        DiagnosticId,
        nameof(Resources.StatefulModuleAnalyzerTitle),
        nameof(Resources.StatefulModuleAnalyzerMessageFormat),
        nameof(Resources.StatefulModuleAnalyzerDescription),
        category: "Design",
        severity: DiagnosticSeverity.Warning);

    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeClassDeclaration, SyntaxKind.ClassDeclaration);
    }

    private static void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not ClassDeclarationSyntax classDeclaration)
        {
            return;
        }

        var classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration);
        if (classSymbol is null)
        {
            return;
        }

        // Check if this class inherits from Module<T>.
        if (!classSymbol.IsModule(context.Compilation))
        {
            return;
        }

        // Analyze all instance fields declared directly in this class
        foreach (var member in classSymbol.GetMembers())
        {
            if (member is IFieldSymbol fieldSymbol && !fieldSymbol.IsStatic && !fieldSymbol.IsConst)
            {
                AnalyzeField(context, fieldSymbol, classSymbol);
            }
        }
    }

    private static void AnalyzeField(SyntaxNodeAnalysisContext context, IFieldSymbol fieldSymbol, INamedTypeSymbol classSymbol)
    {
        // Skip readonly fields - they're safe if initialized in constructor
        if (fieldSymbol.IsReadOnly)
        {
            return;
        }

        // Skip fields that are backing fields for auto-properties (they have special names)
        if (fieldSymbol.IsImplicitlyDeclared)
        {
            return;
        }

        // Get field location for reporting
        var location = fieldSymbol.Locations.FirstOrDefault();
        if (location is null)
        {
            return;
        }

        // Report diagnostic for non-readonly instance fields
        var diagnostic = Diagnostic.Create(
            Rule,
            location,
            fieldSymbol.Name,
            classSymbol.Name);

        context.ReportDiagnostic(diagnostic);
    }
}
