using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using ModularPipelines.Analyzers.Extensions;

namespace ModularPipelines.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public class AwaitThisAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "AwaitThis";

    public static DiagnosticDescriptor Rule => PrivateRule;

    private const string Category = "Usage";
    private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AwaitThisAnalyzerTitle), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AwaitThisAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AwaitThisAnalyzerDescription), Resources.ResourceManager, typeof(Resources));
    private static readonly DiagnosticDescriptor PrivateRule = new(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeAwaitExpression, SyntaxKind.AwaitExpression);
    }

    private void AnalyzeAwaitExpression(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not AwaitExpressionSyntax awaitExpression)
        {
            return;
        }

        // Check if the awaited expression is 'this'
        if (awaitExpression.Expression is not ThisExpressionSyntax)
        {
            return;
        }

        // Check if we're inside a module class (inherits from ModuleBase)
        var containingClass = context.GetClassThatNodeIsIn();
        if (containingClass == null)
        {
            return;
        }

        if (containingClass.GetSelfAndAllBaseTypes().All(x => x.Name != "ModuleBase"))
        {
            return;
        }

        // Report the diagnostic
        context.ReportDiagnostic(Diagnostic.Create(Rule, awaitExpression.GetLocation()));
    }
}