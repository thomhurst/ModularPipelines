using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ModularPipelines.Analyzers;

/// <summary>
/// Analyzer that warns when the deprecated 'Arguments' property is used on CommandLineToolOptions.
/// The Arguments property bypasses type safety - users should use typed positional argument properties instead.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public class ArgumentsPropertyDeprecatedAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// The diagnostic ID for this analyzer.
    /// </summary>
    public const string DiagnosticId = "MP0010";

    /// <summary>
    /// The fully qualified type name for CommandLineToolOptions base class.
    /// </summary>
    private const string CommandLineToolOptionsFullName = "ModularPipelines.Options.CommandLineToolOptions";

    /// <summary>
    /// The property name being deprecated.
    /// </summary>
    private const string ArgumentsPropertyName = "Arguments";

    /// <summary>
    /// The diagnostic rule for this analyzer.
    /// </summary>
    public static DiagnosticDescriptor Rule { get; } = DiagnosticDescriptorFactory.Create(
        DiagnosticId,
        nameof(Resources.ArgumentsPropertyDeprecatedAnalyzerTitle),
        nameof(Resources.ArgumentsPropertyDeprecatedAnalyzerMessageFormat),
        nameof(Resources.ArgumentsPropertyDeprecatedAnalyzerDescription),
        category: "Migration",
        severity: DiagnosticSeverity.Warning);

    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        // Register for object initializer expressions where Arguments might be set
        context.RegisterSyntaxNodeAction(AnalyzeAssignment, SyntaxKind.SimpleAssignmentExpression);
    }

    private void AnalyzeAssignment(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not AssignmentExpressionSyntax assignment)
        {
            return;
        }

        // Check if the left side is an identifier named "Arguments"
        if (assignment.Left is not IdentifierNameSyntax identifierName ||
            identifierName.Identifier.Text != ArgumentsPropertyName)
        {
            return;
        }

        // Get the symbol for the property being assigned
        var symbolInfo = context.SemanticModel.GetSymbolInfo(identifierName);
        if (symbolInfo.Symbol is not IPropertySymbol propertySymbol)
        {
            return;
        }

        // Check if the property is from a type that inherits from CommandLineToolOptions
        var containingType = propertySymbol.ContainingType;
        if (!InheritsFromCommandLineToolOptions(containingType))
        {
            return;
        }

        // Report the diagnostic
        var typeName = containingType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
        context.ReportDiagnostic(Diagnostic.Create(Rule, assignment.GetLocation(), typeName));
    }

    /// <summary>
    /// Checks if the given type inherits from CommandLineToolOptions.
    /// </summary>
    private static bool InheritsFromCommandLineToolOptions(INamedTypeSymbol? type)
    {
        while (type != null)
        {
            if (type.ToDisplayString() == CommandLineToolOptionsFullName)
            {
                return true;
            }

            type = type.BaseType;
        }

        return false;
    }
}
