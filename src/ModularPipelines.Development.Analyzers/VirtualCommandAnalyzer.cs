using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ModularPipelines.Development.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class VirtualCommandAnalyzer : DiagnosticAnalyzer
{
    private const string Category = "Usage";

    public const string DiagnosticId = "MPD0002";

    private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.MPD0002Title),
        Resources.ResourceManager, typeof(Resources));

    private static readonly LocalizableString MessageFormat =
        new LocalizableResourceString(nameof(Resources.MPD0002MessageFormat), Resources.ResourceManager,
            typeof(Resources));

    private static readonly LocalizableString Description =
        new LocalizableResourceString(nameof(Resources.MPD0002Description), Resources.ResourceManager,
            typeof(Resources));

    private static readonly DiagnosticDescriptor Rule = new(DiagnosticId, Title, MessageFormat, Category,
        DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [Rule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.MethodDeclaration);
    }

    private void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not MethodDeclarationSyntax method)
        {
            return;
        }

        var methodSymbol = context.SemanticModel.GetDeclaredSymbol(method);

        if (methodSymbol is null
            || methodSymbol.IsVirtual
            || methodSymbol.DeclaredAccessibility == Accessibility.Private
            || methodSymbol.ContainingType.TypeKind != TypeKind.Class
            || methodSymbol.ContainingType.IsAbstract
            || methodSymbol.ContainingType.IsStatic
            || methodSymbol.IsOverride
            || methodSymbol.ContainingType.IsSealed)
        {
            return;
        }



        var task = context.Compilation.GetTypeByMetadataName("System.Threading.Tasks.Task`1");

        var commandResult = context.Compilation.GetTypeByMetadataName("ModularPipelines.Models.CommandResult");

        if (task is null || commandResult is null)
        {
            return;
        }

        var commandResultTask = task.Construct(commandResult);

        if (!SymbolEqualityComparer.Default.Equals(methodSymbol.ReturnType, commandResultTask))
        {
            return;
        }

        var diagnostic = Diagnostic.Create(Rule, method.GetLocation());

        context.ReportDiagnostic(diagnostic);
    }
}