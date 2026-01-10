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
    public const string DiagnosticId = "StatefulModule";

    /// <summary>
    /// Diagnostic rule for stateful modules.
    /// </summary>
    public static DiagnosticDescriptor Rule { get; } = DiagnosticDescriptorFactory.Create(
        DiagnosticId,
        nameof(Resources.StatefulModuleAnalyzerTitle),
        nameof(Resources.StatefulModuleAnalyzerMessageFormat),
        nameof(Resources.StatefulModuleAnalyzerDescription),
        category: "Design",
        severity: DiagnosticSeverity.Warning);

    /// <summary>
    /// Known mutable collection type names that should always trigger warnings.
    /// </summary>
    private static readonly ImmutableArray<string> MutableCollectionTypes = ImmutableArray.Create(
        "System.Collections.Generic.List`1",
        "System.Collections.Generic.Dictionary`2",
        "System.Collections.Generic.HashSet`1",
        "System.Collections.Generic.Queue`1",
        "System.Collections.Generic.Stack`1",
        "System.Collections.Generic.LinkedList`1",
        "System.Collections.Generic.SortedList`2",
        "System.Collections.Generic.SortedDictionary`2",
        "System.Collections.Generic.SortedSet`1",
        "System.Collections.ArrayList",
        "System.Collections.Hashtable",
        "System.Collections.Queue",
        "System.Collections.Stack",
        "System.Text.StringBuilder");

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

        // Check if this class inherits from Module<T> or ModuleBase
        if (!IsModuleClass(classSymbol, context.Compilation))
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

    private static bool IsModuleClass(INamedTypeSymbol classSymbol, Compilation compilation)
    {
        // Check for Module<T> (generic)
        var moduleGenericType = compilation.GetTypeByMetadataName("ModularPipelines.Modules.Module`1");
        if (moduleGenericType is not null && classSymbol.InheritsFrom(moduleGenericType))
        {
            return true;
        }

        // Check for ModuleBase (non-generic base)
        var moduleBaseType = compilation.GetTypeByMetadataName("ModularPipelines.Modules.ModuleBase");
        if (moduleBaseType is not null && classSymbol.InheritsFrom(moduleBaseType))
        {
            return true;
        }

        return false;
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

        // Check if it's a known mutable collection type
        var fieldTypeName = fieldSymbol.Type.OriginalDefinition.ToDisplayString();
        var isMutableCollection = MutableCollectionTypes.Any(t =>
            fieldSymbol.Type.OriginalDefinition.ToDisplayString().StartsWith(t.Split('`')[0], StringComparison.Ordinal) ||
            fieldSymbol.Type.OriginalDefinition.MetadataName == t.Split('.').Last());

        // Report diagnostic for non-readonly instance fields
        var diagnostic = Diagnostic.Create(
            Rule,
            location,
            fieldSymbol.Name,
            classSymbol.Name,
            isMutableCollection ? "mutable collection" : "non-readonly field");

        context.ReportDiagnostic(diagnostic);
    }
}
