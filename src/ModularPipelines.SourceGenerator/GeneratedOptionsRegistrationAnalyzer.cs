using System.Collections.Concurrent;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace ModularPipelines.SourceGenerator;

/// <summary>
/// Detects trim-unsafe option registrations emitted by peer source generators.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class GeneratedOptionsRegistrationAnalyzer : DiagnosticAnalyzer
{
    private const string RuntimeMetadataRegistryFullName =
        "ModularPipelines.Metadata.RuntimeMetadataRegistry";

    [Flags]
    private enum MetadataCoverage
    {
        None = 0,
        CommandOptions = 1,
        Secrets = 2,
        All = CommandOptions | Secrets,
    }

    private readonly record struct Registration(Location Location, MetadataCoverage RequiredCoverage);

    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        [GeneratorDiagnostics.PeerGeneratedRuntimeMetadata];

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

            var coveredTypes = new ConcurrentDictionary<ITypeSymbol, MetadataCoverage>(
                SymbolEqualityComparer.Default);
            var registeredTypes = new ConcurrentDictionary<ITypeSymbol, Registration>(
                SymbolEqualityComparer.Default);
            var generatedTypes = new ConcurrentDictionary<ITypeSymbol, byte>(
                SymbolEqualityComparer.Default);
            var generatedTreeRuntimeType = startContext.Compilation
                .GetTypeByMetadataName(CommandOptionsGenerator.RuntimeMetadataRegistrationFullName)
                ?.DeclaringSyntaxReferences
                .FirstOrDefault()
                ?.SyntaxTree
                .GetType();
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
            startContext.RegisterSyntaxNodeAction(
                syntaxContext => CollectGeneratedType(
                    syntaxContext,
                    generatedTreeRuntimeType,
                    generatedTypes,
                    registeredTypes),
                SyntaxKind.ClassDeclaration,
                SyntaxKind.RecordDeclaration,
                SyntaxKind.StructDeclaration,
                SyntaxKind.RecordStructDeclaration);
            startContext.RegisterCompilationEndAction(endContext =>
            {
                foreach (var registration in registeredTypes)
                {
                    coveredTypes.TryGetValue(registration.Key, out var coverage);
                    if (generatedTypes.ContainsKey(registration.Key)
                        && (coverage & registration.Value.RequiredCoverage)
                            != registration.Value.RequiredCoverage)
                    {
                        endContext.ReportDiagnostic(Diagnostic.Create(
                            GeneratorDiagnostics.PeerGeneratedRuntimeMetadata,
                            registration.Value.Location,
                            registration.Key.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)));
                    }
                }
            });
        });
    }

    private static void CollectCoveredType(
        SyntaxNodeAnalysisContext context,
        ConcurrentDictionary<ITypeSymbol, MetadataCoverage> coveredTypes)
    {
        var typeOfExpression = (TypeOfExpressionSyntax) context.Node;
        var coverage = GetMetadataCoverage(context, typeOfExpression);
        if (coverage == MetadataCoverage.None
            || context.SemanticModel.GetTypeInfo(typeOfExpression.Type, context.CancellationToken).Type
            is not ITypeSymbol type)
        {
            return;
        }

        coveredTypes.AddOrUpdate(
            Normalize(type),
            coverage,
            (_, existing) => existing | coverage);
    }

    private static void CollectInvocationRegistration(
        SyntaxNodeAnalysisContext context,
        ConcurrentDictionary<ITypeSymbol, Registration> registeredTypes)
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
            AddRegistration(registeredTypes, optionsType, invocation.GetLocation());
        }
    }

    private static void CollectObjectCreationRegistration(
        SyntaxNodeAnalysisContext context,
        ConcurrentDictionary<ITypeSymbol, Registration> registeredTypes)
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
            AddRegistration(registeredTypes, optionsType, creation.GetLocation());
        }
    }

    private static void CollectGeneratedType(
        SyntaxNodeAnalysisContext context,
        Type? generatedTreeRuntimeType,
        ConcurrentDictionary<ITypeSymbol, byte> generatedTypes,
        ConcurrentDictionary<ITypeSymbol, Registration> registeredTypes)
    {
        // Roslyn parses every AddSource output through the same source-generator-specific
        // syntax-tree implementation. Anchor to this generator's known output so peer
        // output does not depend on hint-name or generated-comment conventions.
        if (!(context.IsGeneratedCode
              || context.Node.SyntaxTree.GetType() == generatedTreeRuntimeType)
            || context.Node is not BaseTypeDeclarationSyntax declaration
            || context.SemanticModel.GetDeclaredSymbol(declaration, context.CancellationToken)
                is not INamedTypeSymbol type)
        {
            return;
        }

        var normalizedType = Normalize(type);
        generatedTypes.TryAdd(normalizedType, 0);
        if (RequiresCommandMetadata(normalizedType))
        {
            AddRegistration(registeredTypes, normalizedType, declaration.Identifier.GetLocation());
        }
    }

    private static void AddRegistration(
        ConcurrentDictionary<ITypeSymbol, Registration> registeredTypes,
        ITypeSymbol optionsType,
        Location location)
    {
        var normalizedType = Normalize(optionsType);
        var registration = new Registration(location, GetRequiredCoverage(normalizedType));
        registeredTypes.AddOrUpdate(
            normalizedType,
            registration,
            (_, existing) => existing with
            {
                RequiredCoverage = existing.RequiredCoverage | registration.RequiredCoverage,
            });
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

    private static MetadataCoverage GetMetadataCoverage(
        SyntaxNodeAnalysisContext context,
        TypeOfExpressionSyntax typeOfExpression)
    {
        var containingType = context.SemanticModel.GetEnclosingSymbol(typeOfExpression.SpanStart)
            ?.ContainingType;
        if (containingType?.ToDisplayString()
            == CommandOptionsGenerator.RuntimeMetadataRegistrationFullName)
        {
            return MetadataCoverage.All;
        }

        if (typeOfExpression.FirstAncestorOrSelf<ArgumentSyntax>() is not { } argument
            || argument.Parent is not ArgumentListSyntax { Parent: InvocationExpressionSyntax invocation }
                argumentList
            || argumentList.Arguments.IndexOf(argument) != 0
            || context.SemanticModel.GetSymbolInfo(invocation, context.CancellationToken).Symbol
                is not IMethodSymbol method
            || method.ContainingType.ToDisplayString() != RuntimeMetadataRegistryFullName)
        {
            return MetadataCoverage.None;
        }

        return method.Name switch
        {
            "RegisterCommandOptions" when method.Parameters.Length == 3
                                          && method.Parameters[2].Type.SpecialType
                                          == SpecialType.System_Int32
                                          && HasCurrentCommandMetadataSchemaVersion(
                                              context,
                                              invocation,
                                              method) => MetadataCoverage.CommandOptions,
            "RegisterSecrets" => MetadataCoverage.Secrets,
            _ => MetadataCoverage.None,
        };
    }

    private static bool HasCurrentCommandMetadataSchemaVersion(
        SyntaxNodeAnalysisContext context,
        InvocationExpressionSyntax invocation,
        IMethodSymbol method)
    {
        if (context.SemanticModel.GetOperation(invocation, context.CancellationToken)
                is not IInvocationOperation invocationOperation
            || invocationOperation.Arguments.FirstOrDefault(
                static argument => argument.Parameter?.Ordinal == 2) is not { } schemaArgument
            || !schemaArgument.Value.ConstantValue.HasValue
            || schemaArgument.Value.ConstantValue.Value is not int registeredSchemaVersion)
        {
            return false;
        }

        return method.ContainingType
                   .GetMembers("CurrentCommandMetadataSchemaVersion")
                   .OfType<IFieldSymbol>()
                   .FirstOrDefault(static field => field.HasConstantValue)
                   ?.ConstantValue is int currentSchemaVersion
               && registeredSchemaVersion == currentSchemaVersion;
    }

    private static MetadataCoverage GetRequiredCoverage(ITypeSymbol type) =>
        RequiresCommandMetadata(type)
            ? MetadataCoverage.All
            : MetadataCoverage.Secrets;

    private static bool RequiresCommandMetadata(ITypeSymbol type)
    {
        for (var current = type as INamedTypeSymbol; current is not null; current = current.BaseType)
        {
            if (current.ToDisplayString() == CommandOptionsGenerator.CommandLineToolOptionsFullName)
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsTrimOrAotEnabled(AnalyzerConfigOptionsProvider optionsProvider) =>
        IsEnabled(optionsProvider, "build_property.PublishTrimmed")
        || IsEnabled(optionsProvider, "build_property.PublishAot");

    private static bool IsEnabled(AnalyzerConfigOptionsProvider optionsProvider, string key) =>
        optionsProvider.GlobalOptions.TryGetValue(key, out var value)
        && string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
}
