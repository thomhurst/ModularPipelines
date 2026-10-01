using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace ModularPipelines.Analyzers;

/// <summary>
/// Reports diagnostic MP0020 when one member body calls <c>WithSkipWhen</c> more than once on the same
/// <c>ModuleConfigurationBuilder</c>. Repeated skip conditions are OR-ed, which differs from V3, where a
/// later call replaced an earlier one.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public sealed class RepeatedSkipWhenAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "MP0020";

    private const string BuilderMetadataName = "ModularPipelines.ModuleConfigurationBuilder";
    private const string WithSkipWhenMethodName = "WithSkipWhen";

    public static DiagnosticDescriptor Rule { get; } =
        DiagnosticDescriptorFactory.Create(
            DiagnosticId,
            nameof(Resources.RepeatedSkipWhenAnalyzerTitle),
            nameof(Resources.RepeatedSkipWhenAnalyzerMessageFormat),
            nameof(Resources.RepeatedSkipWhenAnalyzerDescription),
            severity: DiagnosticSeverity.Info);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [Rule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterCompilationStartAction(compilationContext =>
        {
            var builderType = compilationContext.Compilation.GetTypeByMetadataName(BuilderMetadataName);
            if (builderType is null)
            {
                return;
            }

            compilationContext.RegisterOperationBlockStartAction(blockContext =>
            {
                var callsByBuilder = new ConcurrentDictionary<object, ConcurrentBag<IInvocationOperation>>();

                blockContext.RegisterOperationAction(
                    operationContext =>
                    {
                        var invocation = (IInvocationOperation) operationContext.Operation;
                        if (invocation.TargetMethod.Name != WithSkipWhenMethodName
                            || !SymbolEqualityComparer.Default.Equals(invocation.TargetMethod.ContainingType, builderType)
                            || invocation.Instance is null)
                        {
                            return;
                        }

                        var builderKey = GetBuilderKey(invocation.Instance, builderType);
                        callsByBuilder.GetOrAdd(builderKey, _ => []).Add(invocation);
                    },
                    OperationKind.Invocation);

                blockContext.RegisterOperationBlockEndAction(endContext =>
                {
                    foreach (var calls in callsByBuilder.Values)
                    {
                        if (calls.Count < 2)
                        {
                            continue;
                        }

                        foreach (var repeatedCall in calls
                            .OrderBy(call => call.Syntax.SpanStart)
                            .Skip(1))
                        {
                            endContext.ReportDiagnostic(Diagnostic.Create(Rule, GetMethodNameLocation(repeatedCall)));
                        }
                    }
                });
            });
        });
    }

    /// <summary>
    /// Follows a fluent chain of builder calls back to the builder it started from, so calls on the same
    /// parameter, local, field, or chain share one key.
    /// </summary>
    private static object GetBuilderKey(IOperation instance, INamedTypeSymbol builderType)
    {
        var current = instance;
        while (true)
        {
            current = Unwrap(current);
            if (current is IInvocationOperation { Instance: not null } chainedCall
                && SymbolEqualityComparer.Default.Equals(chainedCall.TargetMethod.ContainingType, builderType)
                && SymbolEqualityComparer.Default.Equals(chainedCall.Type, builderType))
            {
                current = chainedCall.Instance;
                continue;
            }

            break;
        }

        return current switch
        {
            IParameterReferenceOperation parameter => parameter.Parameter,
            ILocalReferenceOperation local => local.Local,
            IFieldReferenceOperation { Instance: null or IInstanceReferenceOperation } field => field.Field,
            IPropertyReferenceOperation { Instance: null or IInstanceReferenceOperation } property => property.Property,
            _ => current,
        };
    }

    private static IOperation Unwrap(IOperation operation)
    {
        while (operation is IConversionOperation { IsImplicit: true } or IParenthesizedOperation)
        {
            operation = operation switch
            {
                IConversionOperation conversion => conversion.Operand,
                IParenthesizedOperation parenthesized => parenthesized.Operand,
                _ => operation,
            };
        }

        return operation;
    }

    private static Location GetMethodNameLocation(IInvocationOperation invocation)
    {
        var syntax = invocation.Syntax;
        if (syntax is Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax
            {
                Expression: Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax memberAccess,
            })
        {
            return memberAccess.Name.GetLocation();
        }

        return syntax.GetLocation();
    }
}
