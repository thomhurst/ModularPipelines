using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

namespace ModularPipelines.Analyzers;

internal static class ConfiguredDependencies
{
    internal static bool TryGetDependency(
        InvocationExpressionSyntax invocation,
        SemanticModel semanticModel,
        CancellationToken cancellationToken,
        out IMethodSymbol configureMethod,
        out INamedTypeSymbol dependencyType)
    {
        configureMethod = null!;
        dependencyType = null!;

        var name = (invocation.Expression as MemberAccessExpressionSyntax)?.Name.Identifier.ValueText;
        if (name is not ("DependsOn" or "DependsOnOptional" or "DependsOnIf")
            || invocation.FirstAncestorOrSelf<MethodDeclarationSyntax>() is not { } declaration
            || !IsInMethodBody(invocation, declaration)
            || semanticModel.GetDeclaredSymbol(declaration, cancellationToken) is not IMethodSymbol method
            || !IsConfigure(method, semanticModel.Compilation)
            || semanticModel.GetOperation(invocation, cancellationToken) is not IInvocationOperation operation
            || !IsBuilderMethod(operation.TargetMethod, semanticModel.Compilation)
            || !UsesBuilderParameter(operation.Instance, method.Parameters[0], semanticModel.Compilation))
        {
            return false;
        }

        ITypeSymbol? dependency = null;
        if (operation.TargetMethod.IsGenericMethod)
        {
            dependency = operation.TargetMethod.TypeArguments[0];
        }
        else if (operation.Arguments.FirstOrDefault(argument => argument.Parameter?.Name == "moduleType")?.Value is ITypeOfOperation typeOf)
        {
            dependency = typeOf.TypeOperand;
        }

        if (dependency is not INamedTypeSymbol namedType)
        {
            return false;
        }

        configureMethod = method;
        dependencyType = namedType;
        return true;
    }

    internal static IEnumerable<INamedTypeSymbol> GetDependencies(
        INamedTypeSymbol moduleType,
        Compilation compilation,
        CancellationToken cancellationToken)
    {
        foreach (var method in GetEffectiveConfigureMethods(moduleType, compilation, cancellationToken))
        {
            foreach (var reference in method.DeclaringSyntaxReferences)
            {
                var declaration = reference.GetSyntax(cancellationToken);
                var semanticModel = compilation.GetSemanticModel(declaration.SyntaxTree);
                foreach (var invocation in declaration.DescendantNodes().OfType<InvocationExpressionSyntax>())
                {
                    if (TryGetDependency(invocation, semanticModel, cancellationToken, out _, out var dependency))
                    {
                        yield return dependency;
                    }
                }
            }
        }
    }

    internal static HashSet<IMethodSymbol> GetEffectiveConfigureMethods(
        INamedTypeSymbol moduleType,
        Compilation compilation,
        CancellationToken cancellationToken)
    {
        var result = new HashSet<IMethodSymbol>(SymbolEqualityComparer.Default);
        for (var type = moduleType; type is not null; type = type.BaseType)
        {
            var method = type.GetMembers("Configure").OfType<IMethodSymbol>()
                .FirstOrDefault(candidate => IsConfigure(candidate, compilation));
            if (method is null)
            {
                continue;
            }

            var pending = new Stack<IMethodSymbol>();
            pending.Push(method);
            while (pending.Count > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var current = pending.Pop().OriginalDefinition;
                if (!result.Add(current))
                {
                    continue;
                }

                // Base configuration only runs when the override explicitly invokes it.
                foreach (var reference in current.DeclaringSyntaxReferences)
                {
                    var declaration = reference.GetSyntax(cancellationToken);
                    var semanticModel = compilation.GetSemanticModel(declaration.SyntaxTree);
                    foreach (var invocation in declaration.DescendantNodes().OfType<InvocationExpressionSyntax>())
                    {
                        if (IsInMethodBody(invocation, declaration)
                            && invocation.Expression is MemberAccessExpressionSyntax { Expression: BaseExpressionSyntax, Name.Identifier.ValueText: "Configure" }
                            && semanticModel.GetOperation(invocation, cancellationToken) is IInvocationOperation baseCall
                            && IsConfigure(baseCall.TargetMethod, compilation)
                            && baseCall.Arguments.Length == 1
                            && UsesBuilderParameter(baseCall.Arguments[0].Value, current.Parameters[0], compilation))
                        {
                            pending.Push(baseCall.TargetMethod);
                        }
                    }
                }
            }

            break;
        }

        return result;
    }

    private static bool IsInMethodBody(InvocationExpressionSyntax invocation, SyntaxNode declaration)
    {
        return !invocation.Ancestors().TakeWhile(node => node != declaration)
            .Any(node => node is AnonymousFunctionExpressionSyntax or LocalFunctionStatementSyntax);
    }

    private static bool IsConfigure(IMethodSymbol method, Compilation compilation)
    {
        if (method.Name != "Configure" || method.Parameters.Length != 1)
        {
            return false;
        }

        while (method.OverriddenMethod is { } overridden)
        {
            method = overridden;
        }

        return SymbolEqualityComparer.Default.Equals(method.ContainingType.OriginalDefinition,
            compilation.GetTypeByMetadataName("ModularPipelines.Module`1"));
    }

    private static bool IsBuilderMethod(IMethodSymbol method, Compilation compilation)
    {
        return SymbolEqualityComparer.Default.Equals(method.ContainingType,
            compilation.GetTypeByMetadataName("ModularPipelines.ModuleConfigurationBuilder"));
    }

    private static bool UsesBuilderParameter(IOperation? operation, IParameterSymbol parameter, Compilation compilation)
    {
        while (true)
        {
            switch (operation)
            {
                case IConversionOperation conversion:
                    operation = conversion.Operand;
                    break;
                case IParenthesizedOperation parenthesized:
                    operation = parenthesized.Operand;
                    break;
                case IInvocationOperation invocation when IsBuilderMethod(invocation.TargetMethod, compilation):
                    operation = invocation.Instance;
                    break;
                default:
                    return operation is IParameterReferenceOperation reference
                        && SymbolEqualityComparer.Default.Equals(reference.Parameter, parameter);
            }
        }
    }
}
