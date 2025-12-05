using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ModularPipelines.Development.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class VirtualSwitchPropertyAnalyzer : DiagnosticAnalyzer
{
    private const string Category = "Usage";

    public const string DiagnosticId = "MPD0001";

    private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.MPD0001Title),
        Resources.ResourceManager, typeof(Resources));

    private static readonly LocalizableString MessageFormat =
        new LocalizableResourceString(nameof(Resources.MPD0001MessageFormat), Resources.ResourceManager,
            typeof(Resources));

    private static readonly LocalizableString Description =
        new LocalizableResourceString(nameof(Resources.MPD0001Description), Resources.ResourceManager,
            typeof(Resources));

    private static readonly DiagnosticDescriptor Rule = new(DiagnosticId, Title, MessageFormat, Category,
        DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

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