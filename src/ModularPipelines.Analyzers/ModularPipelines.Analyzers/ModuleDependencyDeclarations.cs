using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using ModularPipelines.Analyzers.Extensions;

namespace ModularPipelines.Analyzers;

internal enum DependencyDeclarationKind
{
    Optional,
    Unknown,
    Required,
}

internal static class ModuleDependencyDeclarations
{
    internal static ImmutableDictionary<ITypeSymbol, DependencyDeclarationKind> GetDeclarations(
        INamedTypeSymbol module,
        Compilation compilation,
        CancellationToken cancellationToken)
    {
        var declarations = ImmutableDictionary.CreateBuilder<ITypeSymbol, DependencyDeclarationKind>(SymbolEqualityComparer.Default);
        AddAttributeDeclarations(module, compilation, declarations);
        AddConfiguredDeclarations(module, compilation, declarations, cancellationToken);
        return declarations.ToImmutable();
    }

    private static void AddAttributeDeclarations(
        INamedTypeSymbol module,
        Compilation compilation,
        ImmutableDictionary<ITypeSymbol, DependencyDeclarationKind>.Builder declarations)
    {
        foreach (var attribute in module.GetAllAttributesIncludingBaseAndInterfaces())
        {
            if (attribute.AttributeClass is not { } attributeClass || !attributeClass.IsDependsOnAttribute(compilation))
            {
                continue;
            }

            var dependency = attributeClass.IsGenericType
                ? attributeClass.TypeArguments[0]
                : attribute.ConstructorArguments.FirstOrDefault().Value as ITypeSymbol;
            if (dependency is not null)
            {
                Add(declarations, dependency, attribute.NamedArguments.Any(argument => argument.Key == "Optional" && argument.Value.Value is true)
                    ? DependencyDeclarationKind.Optional : DependencyDeclarationKind.Required);
            }
        }
    }

    private static void AddConfiguredDeclarations(
        INamedTypeSymbol module,
        Compilation compilation,
        ImmutableDictionary<ITypeSymbol, DependencyDeclarationKind>.Builder declarations,
        CancellationToken cancellationToken)
    {
        var builderType = compilation.GetTypeByMetadataName("ModularPipelines.ModuleConfigurationBuilder");
        if (builderType is null)
        {
            return;
        }

        for (var current = module; current is not null; current = current.BaseType)
        {
            var configure = current.GetMembers("Configure").OfType<IMethodSymbol>()
                .FirstOrDefault(method => IsConfigure(method, compilation, builderType));
            if (configure is null)
            {
                continue;
            }

            new ConfigurationWalker(compilation, builderType, declarations, cancellationToken).Analyze(configure);
            break;
        }
    }

    private static bool IsConfigure(IMethodSymbol method, Compilation compilation, INamedTypeSymbol builderType)
    {
        if (method.Name != "Configure" || method.Parameters.Length != 1
            || !SymbolEqualityComparer.Default.Equals(method.Parameters[0].Type, builderType))
        {
            return false;
        }

        while (method.OverriddenMethod is { } overridden)
        {
            method = overridden;
        }

        return method.ContainingType.IsType(compilation, AnalyzerConstants.FullyQualifiedTypeNames.Module);
    }

    private static void Add(ImmutableDictionary<ITypeSymbol, DependencyDeclarationKind>.Builder declarations,
        ITypeSymbol type, DependencyDeclarationKind kind)
    {
        // Runtime merging keeps a required declaration whenever one exists.
        if (!declarations.TryGetValue(type, out var existing) || kind > existing)
        {
            declarations[type] = kind;
        }
    }

    private sealed class ConfigurationWalker(
        Compilation compilation,
        INamedTypeSymbol builderType,
        ImmutableDictionary<ITypeSymbol, DependencyDeclarationKind>.Builder declarations,
        CancellationToken cancellationToken) : OperationWalker
    {
        private readonly HashSet<IMethodSymbol> _visited = [with(SymbolEqualityComparer.Default)];
        private IMethodSymbol? _method;
        private bool _conditional;

        internal void Analyze(IMethodSymbol method)
        {
            if (!_visited.Add(method))
            {
                return;
            }

            var previous = _method;
            _method = method;
            foreach (var reference in method.DeclaringSyntaxReferences)
            {
                var syntax = reference.GetSyntax(cancellationToken);
                Visit(compilation.GetSemanticModel(syntax.SyntaxTree).GetOperation(syntax, cancellationToken));
            }

            _method = previous;
        }

        public override void VisitAnonymousFunction(IAnonymousFunctionOperation operation)
        {
        }

        public override void VisitLocalFunction(ILocalFunctionOperation operation)
        {
        }

        public override void VisitInvocation(IInvocationOperation operation)
        {
            base.VisitInvocation(operation);
            var method = operation.TargetMethod;
            if (IsDependencyDeclaration(operation))
            {
                AddDependencyDeclaration(operation);
            }
            else if (IsBaseConfigureCall(operation))
            {
                var previous = _conditional;
                _conditional |= IsConditional(operation);
                Analyze(method);
                _conditional = previous;
            }
        }

        private bool IsDependencyDeclaration(IInvocationOperation operation)
        {
            var method = operation.TargetMethod;
            return SymbolEqualityComparer.Default.Equals(method.ContainingType, builderType)
                && method.Name is "DependsOn" or "DependsOnOptional" or "DependsOnIf"
                && IsBuilderParameter(operation.Instance);
        }

        private bool IsBaseConfigureCall(IInvocationOperation operation)
        {
            return operation.Syntax is InvocationExpressionSyntax { Expression: MemberAccessExpressionSyntax { Expression: BaseExpressionSyntax } }
                && IsConfigure(operation.TargetMethod, compilation, builderType)
                && operation.Arguments.Length == 1 && IsBuilderParameter(operation.Arguments[0].Value);
        }

        private void AddDependencyDeclaration(IInvocationOperation operation)
        {
            var method = operation.TargetMethod;
            var dependency = method.TypeArguments.FirstOrDefault()
                ?? operation.Arguments.Select(argument => argument.Value).OfType<ITypeOfOperation>().FirstOrDefault()?.TypeOperand;
            if (dependency is null)
            {
                return;
            }

            var condition = method.Name == "DependsOnIf"
                ? operation.Arguments.FirstOrDefault(argument => argument.Parameter?.Type.SpecialType == SpecialType.System_Boolean)?.Value.ConstantValue
                : null;
            if (condition is { HasValue: true, Value: false })
            {
                return;
            }

            var conditional = _conditional || IsConditional(operation) || HasConditionalExit(operation)
                || (method.Name == "DependsOnIf" && condition is not { HasValue: true, Value: true });
            var kind = GetDeclarationKind(method, conditional);
            Add(declarations, SubstituteType(dependency), kind);
        }

        private static DependencyDeclarationKind GetDeclarationKind(IMethodSymbol method, bool conditional)
        {
            if (conditional)
            {
                return DependencyDeclarationKind.Unknown;
            }

            return method.Name == "DependsOnOptional"
                ? DependencyDeclarationKind.Optional : DependencyDeclarationKind.Required;
        }

        private ITypeSymbol SubstituteType(ITypeSymbol type)
        {
            if (type is INamedTypeSymbol { IsGenericType: true } namedType)
            {
                return namedType.ConstructedFrom.Construct([.. namedType.TypeArguments.Select(SubstituteType)]);
            }

            if (type is ITypeParameterSymbol parameter)
            {
                for (var current = _method?.ContainingType; current is not null; current = current.ContainingType)
                {
                    if (SymbolEqualityComparer.Default.Equals(parameter.ContainingSymbol, current.OriginalDefinition))
                    {
                        return current.TypeArguments[parameter.Ordinal];
                    }
                }
            }

            return type;
        }

        private bool IsBuilderParameter(IOperation? operation)
        {
            for (var depth = 0; depth < 32; depth++)
            {
                switch (operation)
                {
                    case IConversionOperation conversion:
                        operation = conversion.Operand;
                        continue;
                    case IInvocationOperation call when SymbolEqualityComparer.Default.Equals(call.TargetMethod.ContainingType, builderType)
                                                       && SymbolEqualityComparer.Default.Equals(call.Type, builderType):
                        operation = call.Instance;
                        continue;
                    case ILocalReferenceOperation local when !IsWritten(local, local.Local)
                        && local.Local.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax(cancellationToken)
                            is VariableDeclaratorSyntax { Initializer.Value: { } initializer }:
                        operation = compilation.GetSemanticModel(initializer.SyntaxTree).GetOperation(initializer, cancellationToken);
                        continue;
                    default:
                        return operation is IParameterReferenceOperation parameter && parameter.Parameter.Ordinal == 0
                            && SymbolEqualityComparer.Default.Equals(parameter.Parameter.ContainingSymbol.OriginalDefinition, _method?.OriginalDefinition)
                            && !IsWritten(parameter, parameter.Parameter);
                }
            }

            return false;
        }

        private static bool IsWritten(IOperation reference, ISymbol symbol)
        {
            var root = reference;
            while (root.Parent is { } parent)
            {
                root = parent;
            }

            foreach (var operation in root.DescendantsAndSelf())
            {
                ISymbol? referencedSymbol = operation switch
                {
                    ILocalReferenceOperation local => local.Local,
                    IParameterReferenceOperation parameter => parameter.Parameter,
                    _ => null,
                };
                if (!SymbolEqualityComparer.Default.Equals(referencedSymbol, symbol))
                {
                    continue;
                }

                if ((operation.Parent is IAssignmentOperation assignment && assignment.Target == operation)
                    || operation.Parent is IArgumentOperation { Parameter.RefKind: not RefKind.None }
                    || operation.Parent is ITupleOperation)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsConditional(IOperation operation)
        {
            for (var parent = operation.Parent; parent is not null; parent = parent.Parent)
            {
                if (parent is IConditionalOperation or ILoopOperation or ISwitchOperation or ITryOperation)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasConditionalExit(IOperation operation)
        {
            var root = operation;
            while (root.Parent is { } parent)
            {
                root = parent;
            }

            return root.DescendantsAndSelf().OfType<IReturnOperation>()
                .Any(returnOperation => IsConditional(returnOperation));
        }
    }
}
