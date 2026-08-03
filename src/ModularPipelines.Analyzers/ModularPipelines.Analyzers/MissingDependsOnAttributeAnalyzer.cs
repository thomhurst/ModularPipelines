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
    public const string DiagnosticId = "MissingDependsOnAttribute";

    public static DiagnosticDescriptor Rule { get; } = DiagnosticDescriptorFactory.Create(
        DiagnosticId,
        nameof(Resources.MissingDependsOnAttributeAnalyzerTitle),
        nameof(Resources.MissingDependsOnAttributeAnalyzerMessageFormat),
        nameof(Resources.MissingDependsOnAttributeAnalyzerDescription));

    private const string OptionalModuleAccessorSuffix = "ModuleIfRegistered";

    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeMissingDependsOnAttributes, SyntaxKind.InvocationExpression);
    }

    private void AnalyzeMissingDependsOnAttributes(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not InvocationExpressionSyntax invocationExpressionSyntax)
        {
            return;
        }

        var invokedName = invocationExpressionSyntax.Expression switch
        {
            SimpleNameSyntax simpleName => simpleName,
            MemberAccessExpressionSyntax memberAccess => memberAccess.Name,
            _ => null,
        };

        if (invokedName is null || !CouldBeModuleAccessor(invokedName))
        {
            return;
        }

        var methodSymbol = context.SemanticModel.GetSymbolInfo(invocationExpressionSyntax, context.CancellationToken).Symbol as IMethodSymbol;

        if (methodSymbol is null ||
            !TryGetModuleType(context.Compilation, methodSymbol, invokedName, out var namedTypeSymbol))
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

        var attributes = classSymbol.GetAllAttributesIncludingBaseAndInterfaces();

        if (!attributes.Any(x => x.IsDependsOnAttributeFor(context.Compilation, namedTypeSymbol)))
        {
            var properties = new Dictionary<string, string?>
            {
                ["Name"] = namedTypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
                ["Optional"] = IsOptionalAccessor(methodSymbol).ToString(),
                ["ModuleDeclarationStart"] = moduleDeclaration.SpanStart.ToString(),
            }.ToImmutableDictionary();

            context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation(), properties, namedTypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
        }
    }

    private static bool CouldBeModuleAccessor(SimpleNameSyntax invokedName)
    {
        var methodName = invokedName.Identifier.ValueText;

        return methodName.StartsWith("Get", StringComparison.Ordinal)
               && (methodName.EndsWith("Module", StringComparison.Ordinal)
                   || methodName.EndsWith(OptionalModuleAccessorSuffix, StringComparison.Ordinal));
    }

    private static bool TryGetModuleType(
        Compilation compilation,
        IMethodSymbol methodSymbol,
        SimpleNameSyntax invokedName,
        out INamedTypeSymbol moduleType)
    {
        moduleType = null!;
        var moduleContextType = compilation.GetTypeByMetadataName("ModularPipelines.Context.IModuleContext");

        if (moduleContextType is null)
        {
            return false;
        }

        if (TryGetDirectModuleType(
                methodSymbol,
                invokedName,
                moduleContextType,
                out moduleType))
        {
            return true;
        }

        return TryGetGeneratedModuleType(methodSymbol, moduleContextType, out moduleType);
    }

    private static bool TryGetDirectModuleType(
        IMethodSymbol methodSymbol,
        SimpleNameSyntax invokedName,
        INamedTypeSymbol moduleContextType,
        out INamedTypeSymbol moduleType)
    {
        moduleType = null!;

        if (invokedName is not GenericNameSyntax
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

    private static bool TryGetGeneratedModuleType(
        IMethodSymbol methodSymbol,
        INamedTypeSymbol moduleContextType,
        out INamedTypeSymbol moduleType)
    {
        moduleType = null!;
        var extensionMethod = methodSymbol.ReducedFrom ?? methodSymbol;

        if (!extensionMethod.IsExtensionMethod ||
            !extensionMethod.Name.StartsWith("Get", StringComparison.Ordinal) ||
            (!extensionMethod.Name.EndsWith("Module", StringComparison.Ordinal) &&
             !extensionMethod.Name.EndsWith(OptionalModuleAccessorSuffix, StringComparison.Ordinal)) ||
            extensionMethod.Parameters.Length == 0 ||
            !SymbolEqualityComparer.Default.Equals(extensionMethod.Parameters[0].Type, moduleContextType) ||
            !IsGeneratedModuleAccessor(extensionMethod.ContainingType))
        {
            return false;
        }

        if (methodSymbol.ReturnType is not INamedTypeSymbol returnType)
        {
            return false;
        }

        moduleType = (INamedTypeSymbol) returnType
            .WithNullableAnnotation(NullableAnnotation.NotAnnotated);
        return true;
    }

    private static bool IsGeneratedModuleAccessor(INamedTypeSymbol containingType)
    {
        return containingType.Name.EndsWith("ModuleContextExtensions", StringComparison.Ordinal)
            && containingType.ContainingNamespace.ToDisplayString() == "ModularPipelines.Generated"
            && containingType.GetAttributes().Any(attribute =>
            attribute.AttributeClass?.ToDisplayString() == "System.CodeDom.Compiler.GeneratedCodeAttribute" &&
            attribute.ConstructorArguments.Length > 0 &&
            attribute.ConstructorArguments[0].Value is "ModularPipelines.SourceGenerator");
    }

    private static bool IsOptionalAccessor(IMethodSymbol methodSymbol)
        => (methodSymbol.ReducedFrom ?? methodSymbol).Name
            .EndsWith(OptionalModuleAccessorSuffix, StringComparison.Ordinal);

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
