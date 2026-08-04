using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ModularPipelines.Development.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public class VirtualSwitchPropertyAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "MP0011";

    private static readonly DiagnosticDescriptor Rule = DiagnosticDescriptorFactory.Create(
        DiagnosticId,
        nameof(Resources.MP0011Title),
        nameof(Resources.MP0011MessageFormat),
        nameof(Resources.MP0011Description));

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [Rule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.PropertyDeclaration);
    }

    private void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not PropertyDeclarationSyntax property)
        {
            return;
        }

        var propertySymbol = context.SemanticModel.GetDeclaredSymbol(property);

        if (propertySymbol is null)
        {
            return;
        }

        // Check for new CLI attributes (CliOption, CliFlag, CliArgument)
        var cliAttributeNames = new HashSet<string> { "CliOptionAttribute", "CliFlagAttribute", "CliArgumentAttribute" };
        var attributes = propertySymbol.GetAttributes()
            .Where(x => x.AttributeClass?.Name is { } name && cliAttributeNames.Contains(name))
            .ToList();

        if (attributes.Count == 0 || propertySymbol.IsVirtual)
        {
            return;
        }

        var diagnostic = Diagnostic.Create(Rule, property.GetLocation());

        context.ReportDiagnostic(diagnostic);
    }
}
