using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace ModularPipelines.Analyzers;

/// <summary>
/// Warns when type-erased results are tested against non-generic outcome records.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public sealed class NonGenericModuleResultAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "MP0023";

    public static DiagnosticDescriptor Rule { get; } = DiagnosticDescriptorFactory.Create(
        DiagnosticId,
        nameof(Resources.NonGenericModuleResultAnalyzerTitle),
        nameof(Resources.NonGenericModuleResultAnalyzerMessageFormat),
        nameof(Resources.NonGenericModuleResultAnalyzerDescription),
        severity: DiagnosticSeverity.Warning);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [Rule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterCompilationStartAction(startContext =>
        {
            var resultType = startContext.Compilation.GetTypeByMetadataName("ModularPipelines.ModuleResult");
            var resultInterface = startContext.Compilation.GetTypeByMetadataName("ModularPipelines.IModuleResult");
            if (resultType is null || resultInterface is null)
            {
                return;
            }

            var failureType = resultType.GetTypeMembers("Failure").SingleOrDefault();
            var skippedType = resultType.GetTypeMembers("Skipped").SingleOrDefault();
            var enumerableType = startContext.Compilation.GetTypeByMetadataName("System.Linq.Enumerable");
            var queryableType = startContext.Compilation.GetTypeByMetadataName("System.Linq.Queryable");
            var sequenceType = startContext.Compilation.GetTypeByMetadataName("System.Collections.Generic.IEnumerable`1");

            bool IsErasedResult(ITypeSymbol? type) =>
                SymbolEqualityComparer.Default.Equals(type, resultType)
                || SymbolEqualityComparer.Default.Equals(type, resultInterface);

            bool IsNonGenericOutcome(ITypeSymbol? type) => type is not null
                && (SymbolEqualityComparer.Default.Equals(type, failureType)
                    || SymbolEqualityComparer.Default.Equals(type, skippedType));

            startContext.RegisterOperationAction(operationContext =>
            {
                var (inputType, matchedType, syntax) = GetOutcomeTest(operationContext.Operation);

                if (syntax is not null && IsErasedResult(inputType) && IsNonGenericOutcome(matchedType))
                {
                    operationContext.ReportDiagnostic(Diagnostic.Create(Rule, syntax.GetLocation()));
                }
            }, OperationKind.IsType, OperationKind.DeclarationPattern, OperationKind.TypePattern, OperationKind.RecursivePattern);

            startContext.RegisterOperationAction(operationContext =>
            {
                var invocation = (IInvocationOperation) operationContext.Operation;
                var method = invocation.TargetMethod;
                if (method.Name != "OfType" || method.TypeArguments.Length != 1
                    || !IsNonGenericOutcome(method.TypeArguments[0])
                    || !(SymbolEqualityComparer.Default.Equals(method.ContainingType, enumerableType)
                        || SymbolEqualityComparer.Default.Equals(method.ContainingType, queryableType))
                    || invocation.Arguments.Length != 1)
                {
                    return;
                }

                // OfType accepts non-generic IEnumerable. Inspect the source before that implicit conversion.
                var source = invocation.Arguments[0].Value;
                while (source is IConversionOperation { IsImplicit: true } conversion)
                {
                    source = conversion.Operand;
                }

                var sourceType = source.Type;
                if (sourceType is null || !sourceType.AllInterfaces.Concat([sourceType]).OfType<INamedTypeSymbol>()
                    .Any(type => SymbolEqualityComparer.Default.Equals(type.OriginalDefinition, sequenceType)
                        && IsErasedResult(type.TypeArguments[0])))
                {
                    return;
                }

                var name = (invocation.Syntax as InvocationExpressionSyntax)?.Expression switch
                {
                    MemberAccessExpressionSyntax access => access.Name as GenericNameSyntax,
                    MemberBindingExpressionSyntax binding => binding.Name as GenericNameSyntax,
                    GenericNameSyntax generic => generic,
                    _ => null,
                };
                if (name is not null)
                {
                    operationContext.ReportDiagnostic(Diagnostic.Create(Rule, name.TypeArgumentList.Arguments[0].GetLocation()));
                }
            }, OperationKind.Invocation);
        });
    }

    private static (ITypeSymbol? InputType, ITypeSymbol? MatchedType, SyntaxNode? Syntax) GetOutcomeTest(IOperation operation) =>
        operation switch
        {
            IIsTypeOperation test => (test.ValueOperand.Type, test.TypeOperand,
                (test.Syntax as BinaryExpressionSyntax)?.Right),
            IDeclarationPatternOperation pattern => (pattern.InputType, pattern.MatchedType,
                (pattern.Syntax as DeclarationPatternSyntax)?.Type),
            ITypePatternOperation pattern => (pattern.InputType, pattern.MatchedType, pattern.Syntax),
            IRecursivePatternOperation pattern => (pattern.InputType, pattern.MatchedType,
                (pattern.Syntax as RecursivePatternSyntax)?.Type),
            _ => (null, null, null),
        };
}
