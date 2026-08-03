using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using ModularPipelines.Analyzers.Extensions;

namespace ModularPipelines.Analyzers;

/// <summary>
/// Analyzer that detects mutable instance state in modules.
/// Modules are registered as Singletons, so any instance state can leak between executions.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public class StatefulModuleAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// Diagnostic ID for mutable instance state in modules.
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

    /// <summary>
    /// Gets the diagnostic rule for mutable auto-properties in modules.
    /// </summary>
    public static DiagnosticDescriptor PropertyRule { get; } = DiagnosticDescriptorFactory.Create(
        DiagnosticId,
        nameof(Resources.StatefulModuleAnalyzerTitle),
        "StatefulModuleAnalyzerPropertyMessageFormat",
        nameof(Resources.StatefulModuleAnalyzerDescription),
        category: "Design",
        severity: DiagnosticSeverity.Warning);

    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule, PropertyRule];

    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSymbolAction(AnalyzeNamedType, SymbolKind.NamedType);
    }

    private static void AnalyzeNamedType(SymbolAnalysisContext context)
    {
        if (context.Symbol is not INamedTypeSymbol { TypeKind: TypeKind.Class } classSymbol)
        {
            return;
        }

        // Check if this class inherits from Module<T>.
        if (!classSymbol.IsModule(context.Compilation))
        {
            return;
        }

        var members = classSymbol.GetMembers();
        var autoProperties = members
            .OfType<IFieldSymbol>()
            .Where(static field => field.IsImplicitlyDeclared)
            .Select(static field => field.AssociatedSymbol)
            .OfType<IPropertySymbol>()
            .ToImmutableHashSet<IPropertySymbol>(SymbolEqualityComparer.Default);

        // Analyze all instance state declared directly in this class.
        foreach (var member in members)
        {
            switch (member)
            {
                case IFieldSymbol { IsStatic: false, IsConst: false } field:
                    AnalyzeField(context, field, classSymbol);
                    break;
                case IPropertySymbol property when IsWritableAutoProperty(
                    property,
                    autoProperties):
                    ReportDiagnostic(context, PropertyRule, property, classSymbol);
                    break;
            }
        }
    }

    private static void AnalyzeField(
        SymbolAnalysisContext context,
        IFieldSymbol fieldSymbol,
        INamedTypeSymbol classSymbol)
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

        ReportDiagnostic(context, Rule, fieldSymbol, classSymbol);
    }

    private static bool IsWritableAutoProperty(
        IPropertySymbol property,
        ImmutableHashSet<IPropertySymbol> autoProperties)
    {
        return !property.IsStatic
               && !property.IsIndexer
               && property.SetMethod is { IsInitOnly: false }
               && autoProperties.Contains(property);
    }

    private static void ReportDiagnostic(
        SymbolAnalysisContext context,
        DiagnosticDescriptor rule,
        ISymbol member,
        INamedTypeSymbol classSymbol)
    {
        var location = member.Locations.FirstOrDefault();
        if (location is null)
        {
            return;
        }

        var diagnostic = Diagnostic.Create(
            rule,
            location,
            member.Name,
            classSymbol.Name);

        context.ReportDiagnostic(diagnostic);
    }
}
