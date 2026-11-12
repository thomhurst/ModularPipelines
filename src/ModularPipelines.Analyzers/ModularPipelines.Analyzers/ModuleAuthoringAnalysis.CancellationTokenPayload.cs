using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;

namespace ModularPipelines.Analyzers;

internal static partial class ModuleAuthoringAnalysis
{
    [ThreadStatic]
    private static HashSet<IMethodSymbol>? _payloadHelpers;

    internal static bool IsCancellationPayloadType(ITypeSymbol? type) =>
        type?.ToDisplayString() == CancellationTokenMetadataName
        || (type is INamedTypeSymbol { TypeArguments.Length: 1 } named
        && named.TypeArguments[0].ToDisplayString() == CancellationTokenMetadataName
        && named.OriginalDefinition.ToDisplayString() is
            "System.Threading.Tasks.Task<TResult>" or "System.Threading.Tasks.ValueTask<TResult>");

    // Only framework wrappers preserve payload identity. In particular, an arbitrary
    // Result property, GetAwaiter implementation or conversion is not a proof.
    internal static IOperation? GetCancellationPayloadInput(IOperation operation) => operation switch
    {
        IAwaitOperation awaited => awaited.Operation,
        IInvocationOperation { Arguments.Length: 1 } factory
            when factory.TargetMethod is { Name: "FromResult", IsStatic: true }
            && factory.TargetMethod.ContainingType.ToDisplayString() is
                "System.Threading.Tasks.Task" or "System.Threading.Tasks.ValueTask" => factory.Arguments[0].Value,
        IInvocationOperation { Instance: { } instance } wrapper
            when wrapper.TargetMethod.Name is "ConfigureAwait" or "AsTask"
            && wrapper.TargetMethod.ContainingType.OriginalDefinition.ToDisplayString() is
                "System.Threading.Tasks.Task<TResult>" or "System.Threading.Tasks.ValueTask<TResult>" => instance,
        IObjectCreationOperation { Arguments.Length: 1 } creation
            when creation.Type is INamedTypeSymbol named
            && named.OriginalDefinition.ToDisplayString() == "System.Threading.Tasks.ValueTask<TResult>"
            => creation.Arguments[0].Value,
        IPropertyReferenceOperation { Property.Name: "Result", Instance: { } instance } property
            when property.Property.ContainingType.OriginalDefinition.ToDisplayString() is
                "System.Threading.Tasks.Task<TResult>" or "System.Threading.Tasks.ValueTask<TResult>" => instance,
        _ => null,
    };

    internal static bool FlowsFromAssignedCancellationTokenHelper(
        IInvocationOperation invocation, Func<IOperation, bool> flows)
    {
        var method = NormalizeMethod(invocation.TargetMethod);
        var visited = _payloadHelpers ??= [with(SymbolEqualityComparer.Default)];
        if (!visited.Add(method))
        {
            return false;
        }

        try
        {
            return ProveCancellationPayloadHelper(invocation, method, flows);
        }
        finally
        {
            visited.Remove(method);
        }
    }

    private static bool ProveCancellationPayloadHelper(
        IInvocationOperation invocation, IMethodSymbol method, Func<IOperation, bool> flows)
    {
        if (invocation.SemanticModel?.Compilation is not { } compilation
            || GetMethodOperation(method, compilation, default) is not { } operation)
        {
            return false;
        }

        var returns = operation.DescendantsAndSelf().OfType<IReturnOperation>()
            .Where(returned => (GetEnclosingCallable(returned) is null
                || ReferenceEquals(GetEnclosingCallable(returned), operation)) && IsInReachableBranch(returned))
            .Select(static returned => returned.ReturnedValue).OfType<IOperation>().ToArray();
        var parameters = GetScalarPayloadParameters(invocation, method, flows)
            .Where(parameter => !HasPayloadParameterWrite(operation, parameter)).ToArray();
        if (parameters.Length == 0 || returns.Length == 0 || HasUnsupportedPayloadCollections(operation, returns))
        {
            return false;
        }

        // Do not flatten return descendants: a conversion in a branch condition
        // cannot change the selected payload, whereas a conversion of a value can.
        return returns.All(value => ProveReturnedPayload(value, parameters, method));
    }

    private static bool ProveReturnedPayload(IOperation value, IParameterSymbol[] parameters, IMethodSymbol method)
    {
        if (GetCancellationPayloadInput(value) is { } payload)
        {
            return ProveReturnedPayload(payload, parameters, method);
        }

        var leaves = GetReachableValueLeaves(value).Where(static leaf => !AlwaysThrows(leaf)).ToArray();
        if (leaves.Length != 1 || !ReferenceEquals(leaves[0], value))
        {
            return leaves.All(leaf => ProveReturnedPayload(leaf, parameters, method));
        }

        return parameters.Any(parameter => FlowsFromCancellationToken(
            value, parameter, [with(SymbolEqualityComparer.Default)], [with(SymbolEqualityComparer.Default), method]));
    }

    private static IEnumerable<IParameterSymbol> GetScalarPayloadParameters(
        IInvocationOperation invocation, IMethodSymbol method, Func<IOperation, bool> flows)
    {
        foreach (var argument in invocation.Arguments)
        {
            // Inspect the constructed parameter type, then map to its definition.
            if (argument.Parameter is { } parameter && IsCancellationToken(parameter)
                && flows(argument.Value) && GetTargetParameter(method, parameter) is { } target)
            {
                yield return target;
            }
        }

        if (invocation.TargetMethod.ReducedFrom is not null && invocation.Instance is { } receiver
            && receiver.Type?.ToDisplayString() == CancellationTokenMetadataName && flows(receiver))
        {
            yield return method.Parameters[0];
        }
    }

    private static bool HasPayloadParameterWrite(IOperation operation, IParameterSymbol parameter) =>
        operation.DescendantsAndSelf().OfType<IParameterReferenceOperation>().Any(reference =>
            SymbolEqualityComparer.Default.Equals(reference.Parameter, parameter)
            && ((reference.Parent is IAssignmentOperation assignment && ReferenceEquals(assignment.Target, reference))
                || reference.Parent is IArgumentOperation { Parameter.RefKind: RefKind.Ref or RefKind.Out }
                || reference.Parent is IIncrementOrDecrementOperation));

    private static bool HasUnsupportedPayloadCollections(IOperation operation, IOperation[] returns) =>
        !returns.SelectMany(GetReachableValueLeaves).All(static value => value is IParameterReferenceOperation)
        && operation.DescendantsAndSelf().Any(static child => child.Type is IArrayTypeSymbol
            || (child.Type is INamedTypeSymbol named && named.OriginalDefinition.ToDisplayString() is "System.Span<T>" or "System.ReadOnlySpan<T>"));
}
