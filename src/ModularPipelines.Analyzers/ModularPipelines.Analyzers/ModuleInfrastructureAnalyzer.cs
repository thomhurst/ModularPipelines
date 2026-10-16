using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using ModularPipelines.Analyzers.Extensions;

namespace ModularPipelines.Analyzers;

/// <summary>
/// Warns when modules bypass pipeline services and dependency accessors.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public sealed class ModuleInfrastructureAnalyzer : DiagnosticAnalyzer
{
    public const string DirectProcessId = "MP0024";
    public const string DirectHttpClientId = "MP0025";
    public const string InjectedModuleId = "MP0026";

    public static DiagnosticDescriptor DirectProcessRule { get; } = DiagnosticDescriptorFactory.Create(
        DirectProcessId,
        nameof(Resources.DirectProcessTitle),
        nameof(Resources.DirectProcessMessageFormat),
        nameof(Resources.DirectProcessDescription),
        severity: DiagnosticSeverity.Warning);

    public static DiagnosticDescriptor DirectHttpClientRule { get; } = DiagnosticDescriptorFactory.Create(
        DirectHttpClientId,
        nameof(Resources.DirectHttpClientTitle),
        nameof(Resources.DirectHttpClientMessageFormat),
        nameof(Resources.DirectHttpClientDescription),
        severity: DiagnosticSeverity.Warning);

    public static DiagnosticDescriptor InjectedModuleRule { get; } = DiagnosticDescriptorFactory.Create(
        InjectedModuleId,
        nameof(Resources.InjectedModuleTitle),
        nameof(Resources.InjectedModuleMessageFormat),
        nameof(Resources.InjectedModuleDescription),
        severity: DiagnosticSeverity.Warning);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        [DirectProcessRule, DirectHttpClientRule, InjectedModuleRule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterCompilationStartAction(startContext =>
        {
            var moduleType = startContext.Compilation.GetTypeByMetadataName(AnalyzerConstants.FullyQualifiedTypeNames.Module);
            if (moduleType is null)
            {
                return;
            }

            var processType = startContext.Compilation.GetTypeByMetadataName("System.Diagnostics.Process");
            var startInfoType = startContext.Compilation.GetTypeByMetadataName("System.Diagnostics.ProcessStartInfo");
            var httpClientType = startContext.Compilation.GetTypeByMetadataName("System.Net.Http.HttpClient");
            var moduleInterface = startContext.Compilation.GetTypeByMetadataName("ModularPipelines.Modules.IModule");

            startContext.RegisterOperationAction(operationContext =>
            {
                if (operationContext.ContainingSymbol.ContainingType.InheritsFrom(moduleType)
                    && operationContext.Operation is IInvocationOperation { TargetMethod.Name: "Start" } invocation
                    && SymbolEqualityComparer.Default.Equals(invocation.TargetMethod.ContainingType, processType))
                {
                    operationContext.ReportDiagnostic(Diagnostic.Create(DirectProcessRule, invocation.Syntax.GetLocation()));
                }
            }, OperationKind.Invocation);

            startContext.RegisterOperationAction(operationContext =>
            {
                if (!operationContext.ContainingSymbol.ContainingType.InheritsFrom(moduleType)
                    || operationContext.Operation is not IObjectCreationOperation creation)
                {
                    return;
                }

                if (SymbolEqualityComparer.Default.Equals(creation.Type, startInfoType))
                {
                    operationContext.ReportDiagnostic(Diagnostic.Create(DirectProcessRule, creation.Syntax.GetLocation()));
                }
                else if (SymbolEqualityComparer.Default.Equals(creation.Type, httpClientType))
                {
                    operationContext.ReportDiagnostic(Diagnostic.Create(DirectHttpClientRule, creation.Syntax.GetLocation()));
                }
            }, OperationKind.ObjectCreation);

            startContext.RegisterSymbolAction(symbolContext =>
                AnalyzeConstructors(symbolContext, moduleType, moduleInterface), SymbolKind.NamedType);
        });
    }

    private static void AnalyzeConstructors(SymbolAnalysisContext context, INamedTypeSymbol moduleType,
        INamedTypeSymbol? moduleInterface)
    {
        if (context.Symbol is not INamedTypeSymbol type || !type.InheritsFrom(moduleType))
        {
            return;
        }

        foreach (var parameter in type.InstanceConstructors.SelectMany(static constructor => constructor.Parameters))
        {
            if (!IsModuleParameter(parameter.Type, moduleType, moduleInterface))
            {
                continue;
            }

            foreach (var reference in parameter.DeclaringSyntaxReferences)
            {
                if (reference.GetSyntax(context.CancellationToken) is ParameterSyntax syntax)
                {
                    context.ReportDiagnostic(Diagnostic.Create(InjectedModuleRule, syntax.GetLocation()));
                }
            }
        }
    }

    private static bool IsModuleParameter(ITypeSymbol type, INamedTypeSymbol moduleType,
        INamedTypeSymbol? moduleInterface, HashSet<ITypeSymbol>? visited = null)
    {
        if (type.InheritsFrom(moduleType)
            || (moduleInterface is not null && type.ImplementsInterface(moduleInterface)))
        {
            return true;
        }

        return type is ITypeParameterSymbol parameter
            && (visited ??= [with(SymbolEqualityComparer.Default)]).Add(type)
            && parameter.ConstraintTypes.Any(constraint => IsModuleParameter(constraint, moduleType, moduleInterface, visited));
    }
}
