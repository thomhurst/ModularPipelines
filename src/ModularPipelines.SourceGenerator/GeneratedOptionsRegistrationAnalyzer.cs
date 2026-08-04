using System.Collections.Concurrent;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace ModularPipelines.SourceGenerator;

/// <summary>
/// Detects trim-unsafe option registrations emitted by peer source generators.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class GeneratedOptionsRegistrationAnalyzer : DiagnosticAnalyzer
{
    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        [GeneratorDiagnostics.SkippedRuntimeMetadata];

    /// <inheritdoc />
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(
            GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();
        context.RegisterCompilationStartAction(static startContext =>
        {
            if (!IsTrimOrAotEnabled(startContext.Options.AnalyzerConfigOptionsProvider))
            {
                return;
            }

            var coveredTypes = new ConcurrentDictionary<ITypeSymbol, byte>(
                SymbolEqualityComparer.Default);
            var registeredTypes = new ConcurrentDictionary<ITypeSymbol, Location>(
                SymbolEqualityComparer.Default);
            startContext.RegisterSyntaxNodeAction(
                syntaxContext => CollectCoveredType(syntaxContext, coveredTypes),
                SyntaxKind.TypeOfExpression);
            startContext.RegisterSyntaxNodeAction(
                syntaxContext => CollectInvocationRegistration(syntaxContext, registeredTypes),
                SyntaxKind.InvocationExpression);
            startContext.RegisterSyntaxNodeAction(
                syntaxContext => CollectObjectCreationRegistration(syntaxContext, registeredTypes),
                SyntaxKind.ObjectCreationExpression,
                SyntaxKind.ImplicitObjectCreationExpression);
            startContext.RegisterCompilationEndAction(endContext =>
            {
                foreach (var registration in registeredTypes)
                {
                    if (!coveredTypes.ContainsKey(registration.Key)
                        && IsGeneratedSourceType(registration.Key, endContext.CancellationToken))
                    {
                        endContext.ReportDiagnostic(Diagnostic.Create(
                            GeneratorDiagnostics.SkippedRuntimeMetadata,
                            registration.Value,
                            registration.Key.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)));
                    }
                }
            });
        });
    }

    private static void CollectCoveredType(
        SyntaxNodeAnalysisContext context,
        ConcurrentDictionary<ITypeSymbol, byte> coveredTypes)
    {
        var typeOfExpression = (TypeOfExpressionSyntax) context.Node;
        var containingType = context.SemanticModel.GetEnclosingSymbol(typeOfExpression.SpanStart)
            ?.ContainingType;
        if (containingType?.ToDisplayString() != CommandOptionsGenerator.RuntimeMetadataRegistrationFullName
            || context.SemanticModel.GetTypeInfo(typeOfExpression.Type, context.CancellationToken).Type
                is not ITypeSymbol type)
        {
            return;
        }

        coveredTypes.TryAdd(Normalize(type), 0);
    }

    private static void CollectInvocationRegistration(
        SyntaxNodeAnalysisContext context,
        ConcurrentDictionary<ITypeSymbol, Location> registeredTypes)
    {
        var invocation = (InvocationExpressionSyntax) context.Node;
        if (context.SemanticModel.GetSymbolInfo(invocation, context.CancellationToken).Symbol
                is IMethodSymbol method
            && GetRegisteredOptionsType(
                method,
                invocation.ArgumentList,
                context.SemanticModel,
                context.CancellationToken) is { } optionsType)
        {
            registeredTypes.TryAdd(optionsType, invocation.GetLocation());
        }
    }

    private static void CollectObjectCreationRegistration(
        SyntaxNodeAnalysisContext context,
        ConcurrentDictionary<ITypeSymbol, Location> registeredTypes)
    {
        var creation = (BaseObjectCreationExpressionSyntax) context.Node;
        if (context.SemanticModel.GetSymbolInfo(creation, context.CancellationToken).Symbol
                is IMethodSymbol method
            && GetRegisteredOptionsType(
                method,
                creation.ArgumentList,
                context.SemanticModel,
                context.CancellationToken) is { } optionsType)
        {
            registeredTypes.TryAdd(optionsType, creation.GetLocation());
        }
    }

    private static ITypeSymbol? GetRegisteredOptionsType(
        IMethodSymbol method,
        BaseArgumentListSyntax? argumentList,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        var definition = (method.ReducedFrom ?? method).OriginalDefinition;
        if (CommandOptionsGenerator.IsOptionsRegistrationMethod(definition))
        {
            return method.TypeArguments.FirstOrDefault() is { } optionsType
                ? Normalize(optionsType)
                : null;
        }

        if (!CommandOptionsGenerator.IsServiceTypeCarrier(definition))
        {
            return null;
        }

        foreach (var typeArgument in method.TypeArguments)
        {
            if (UnwrapOptionsType(typeArgument) is { } optionsType)
            {
                return Normalize(optionsType);
            }
        }

        return argumentList?.Arguments
            .SelectMany(static argument => argument.Expression
                .DescendantNodesAndSelf()
                .OfType<TypeOfExpressionSyntax>())
            .Select(typeOfExpression => semanticModel.GetTypeInfo(
                typeOfExpression.Type,
                cancellationToken).Type)
            .OfType<ITypeSymbol>()
            .Select(UnwrapOptionsType)
            .OfType<ITypeSymbol>()
            .Select(Normalize)
            .FirstOrDefault();
    }

    private static ITypeSymbol? UnwrapOptionsType(ITypeSymbol type) =>
        type is INamedTypeSymbol namedType
        && CommandOptionsGenerator.IsOptionsTypeUsage(namedType)
            ? namedType.TypeArguments[0]
            : null;

    private static ITypeSymbol Normalize(ITypeSymbol type) =>
        type is INamedTypeSymbol { IsGenericType: true } namedType
            ? namedType.OriginalDefinition
            : type;

    private static bool IsGeneratedSourceType(ITypeSymbol type, CancellationToken cancellationToken) =>
        type.Locations.Any(location =>
            location.SourceTree is { } tree
            && IsGeneratedTree(tree, cancellationToken));

    private static bool IsGeneratedTree(SyntaxTree tree, CancellationToken cancellationToken)
    {
        var path = tree.FilePath;
        if (path.EndsWith(".g.cs", StringComparison.OrdinalIgnoreCase)
            || path.EndsWith(".generated.cs", StringComparison.OrdinalIgnoreCase)
            || path.EndsWith(".designer.cs", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        var text = tree.GetText(cancellationToken);
        var prefixLength = Math.Min(text.Length, 256);
        return text.ToString(TextSpan.FromBounds(0, prefixLength))
            .IndexOf("<auto-generated", StringComparison.OrdinalIgnoreCase) >= 0;
    }

    private static bool IsTrimOrAotEnabled(AnalyzerConfigOptionsProvider optionsProvider) =>
        IsEnabled(optionsProvider, "build_property.PublishTrimmed")
        || IsEnabled(optionsProvider, "build_property.PublishAot");

    private static bool IsEnabled(AnalyzerConfigOptionsProvider optionsProvider, string key) =>
        optionsProvider.GlobalOptions.TryGetValue(key, out var value)
        && string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
}
