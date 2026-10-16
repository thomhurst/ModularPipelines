using System.Collections.Concurrent;
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
public class MissingDependsOnAttributeAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "MP0001";

    public const string OptionalDependencyAccessorId = "MP0021";

    public const string RequiredDependencyAccessorId = "MP0022";

    public static DiagnosticDescriptor OptionalDependencyAccessorRule { get; } = DiagnosticDescriptorFactory.Create(
        OptionalDependencyAccessorId,
        "OptionalDependencyAccessorTitle",
        "OptionalDependencyAccessorMessageFormat",
        "OptionalDependencyAccessorDescription",
        severity: DiagnosticSeverity.Warning);

    public static DiagnosticDescriptor RequiredDependencyAccessorRule { get; } = DiagnosticDescriptorFactory.Create(
        RequiredDependencyAccessorId,
        "RequiredDependencyAccessorTitle",
        "RequiredDependencyAccessorMessageFormat",
        "RequiredDependencyAccessorDescription",
        severity: DiagnosticSeverity.Info);

    public static DiagnosticDescriptor Rule { get; } = DiagnosticDescriptorFactory.Create(
        DiagnosticId,
        nameof(Resources.MissingDependsOnAttributeAnalyzerTitle),
        nameof(Resources.MissingDependsOnAttributeAnalyzerMessageFormat),
        nameof(Resources.MissingDependsOnAttributeAnalyzerDescription));

    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule, OptionalDependencyAccessorRule, RequiredDependencyAccessorRule];

    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterCompilationStartAction(startContext =>
        {
            var declarations = new ConcurrentDictionary<INamedTypeSymbol, ImmutableDictionary<ITypeSymbol, DependencyDeclarationKind>>(SymbolEqualityComparer.Default);
            startContext.RegisterSyntaxNodeAction(
                nodeContext => AnalyzeMissingDependsOnAttributes(nodeContext, declarations),
                SyntaxKind.InvocationExpression);
        });
    }

    private static void AnalyzeMissingDependsOnAttributes(
        SyntaxNodeAnalysisContext context,
        ConcurrentDictionary<INamedTypeSymbol, ImmutableDictionary<ITypeSymbol, DependencyDeclarationKind>> declarations)
    {
        if (!TryGetInvokedModule(context, out var invocationExpressionSyntax, out var methodSymbol, out var namedTypeSymbol))
        {
            return;
        }

        var moduleDeclaration = GetModuleDeclarationSyntax(
            context,
            invocationExpressionSyntax);

        if (moduleDeclaration is null)
        {
            return;
        }

        var classSymbol = context.SemanticModel.GetDeclaredSymbol(moduleDeclaration);

        if (classSymbol is null)
        {
            return;
        }

        var dependencies = declarations.GetOrAdd(classSymbol, module =>
            ModuleDependencyDeclarations.GetDeclarations(module, context.Compilation, context.CancellationToken));

        ReportDependencyDiagnostic(context, moduleDeclaration, methodSymbol, namedTypeSymbol, dependencies);
    }

    private static bool TryGetInvokedModule(
        SyntaxNodeAnalysisContext context,
        out InvocationExpressionSyntax invocationExpressionSyntax,
        out IMethodSymbol methodSymbol,
        out INamedTypeSymbol moduleType)
    {
        invocationExpressionSyntax = null!;
        methodSymbol = null!;
        moduleType = null!;
        if (context.Node is not InvocationExpressionSyntax invocation)
        {
            return false;
        }

        var invokedName = invocation.Expression switch
        {
            SimpleNameSyntax simpleName => simpleName,
            MemberAccessExpressionSyntax memberAccess => memberAccess.Name,
            _ => null,
        };

        if (invokedName is not GenericNameSyntax
            || (invokedName.Identifier.ValueText != AnalyzerConstants.MethodNames.GetModule
                && invokedName.Identifier.ValueText != AnalyzerConstants.MethodNames.GetModuleIfRegistered)
            || context.SemanticModel.GetSymbolInfo(invocation, context.CancellationToken).Symbol is not IMethodSymbol method
            || !TryGetModuleType(context.Compilation, method, out moduleType))
        {
            return false;
        }

        invocationExpressionSyntax = invocation;
        methodSymbol = method;
        return true;
    }

    private static void ReportDependencyDiagnostic(
        SyntaxNodeAnalysisContext context,
        TypeDeclarationSyntax moduleDeclaration,
        IMethodSymbol methodSymbol,
        INamedTypeSymbol namedTypeSymbol,
        ImmutableDictionary<ITypeSymbol, DependencyDeclarationKind> dependencies)
    {
        if (!dependencies.TryGetValue(namedTypeSymbol, out var declarationKind))
        {
            var properties = new Dictionary<string, string?>
            {
                ["Name"] = namedTypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
                ["Optional"] = IsOptionalAccessor(methodSymbol).ToString(),
                ["ModuleDeclarationStart"] = moduleDeclaration.SpanStart.ToString(),
            }.ToImmutableDictionary();

            context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation(), properties, namedTypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
        }
        else if (declarationKind == DependencyDeclarationKind.Optional && !IsOptionalAccessor(methodSymbol))
        {
            context.ReportDiagnostic(Diagnostic.Create(OptionalDependencyAccessorRule, context.Node.GetLocation(), namedTypeSymbol.Name));
        }
        else if (declarationKind == DependencyDeclarationKind.Required && IsOptionalAccessor(methodSymbol))
        {
            context.ReportDiagnostic(Diagnostic.Create(RequiredDependencyAccessorRule, context.Node.GetLocation(), namedTypeSymbol.Name));
        }
    }

    private static bool TryGetModuleType(
        Compilation compilation,
        IMethodSymbol methodSymbol,
        out INamedTypeSymbol moduleType)
    {
        moduleType = null!;
        var moduleContextType = compilation.GetTypeByMetadataName("ModularPipelines.IModuleContext");

        if (moduleContextType is null
            || (methodSymbol.Name != AnalyzerConstants.MethodNames.GetModule
                && methodSymbol.Name != AnalyzerConstants.MethodNames.GetModuleIfRegistered)
            || methodSymbol.TypeArguments.Length != 1
            || !SymbolEqualityComparer.Default.Equals(
                methodSymbol.OriginalDefinition.ContainingType,
                moduleContextType)
            || methodSymbol.TypeArguments[0] is not INamedTypeSymbol directModuleType)
        {
            return false;
        }

        moduleType = directModuleType;
        return true;
    }

    private static bool IsOptionalAccessor(IMethodSymbol methodSymbol)
        => methodSymbol.Name == AnalyzerConstants.MethodNames.GetModuleIfRegistered;

    private static TypeDeclarationSyntax? GetModuleDeclarationSyntax(
        SyntaxNodeAnalysisContext context,
        InvocationExpressionSyntax invocationExpressionSyntax)
    {
        return invocationExpressionSyntax
            .Ancestors()
            .OfType<TypeDeclarationSyntax>()
            .FirstOrDefault(declaration => context.SemanticModel
                .GetDeclaredSymbol(declaration, context.CancellationToken)
                .IsModule(context.Compilation));
    }
}
