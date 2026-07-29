using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ModularPipelines.Development.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public class VirtualCommandAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "MP0012";

    private static readonly DiagnosticDescriptor Rule = DiagnosticDescriptorFactory.Create(
        DiagnosticId,
        nameof(Resources.MP0012Title),
        nameof(Resources.MP0012MessageFormat),
        nameof(Resources.MP0012Description));

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
            || methodSymbol.IsAbstract
            || methodSymbol.IsStatic
            || methodSymbol.IsOverride
            || methodSymbol.ContainingType.IsSealed
            || methodSymbol.IsSealed)
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
