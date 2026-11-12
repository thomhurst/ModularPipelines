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

    private static bool IsFrameworkPayloadAwaitableType(ITypeSymbol? type) =>
        type is INamedTypeSymbol { TypeArguments.Length: 1 } named
        && named.OriginalDefinition.ToDisplayString() is
            "System.Threading.Tasks.Task<TResult>" or "System.Threading.Tasks.ValueTask<TResult>"
            or "System.Runtime.CompilerServices.ConfiguredTaskAwaitable<TResult>"
            or "System.Runtime.CompilerServices.ConfiguredValueTaskAwaitable<TResult>";

    internal static bool IsScalarPayloadType(ITypeSymbol? type, ITypeSymbol scalarType) =>
        SymbolEqualityComparer.Default.Equals(type, scalarType)
        || IsCancellationCarrier(type)
        || (type is INamedTypeSymbol { TypeArguments.Length: 1 } named
            && IsFrameworkPayloadAwaitableType(named)
            && (SymbolEqualityComparer.Default.Equals(named.TypeArguments[0], scalarType)
                || IsCancellationCarrier(named.TypeArguments[0])));

    // Only framework wrappers preserve payload identity. In particular, an arbitrary
    // Result property, GetAwaiter implementation or conversion is not a proof.
    internal static IOperation? GetCancellationPayloadInput(IOperation operation) => operation switch
    {
        // Framework await preserves any T payload, including a helper's type parameter.
        // The recursive proof still has to reach a mapped original token input.
        IAwaitOperation awaited when IsFrameworkPayloadAwaitableType(awaited.Operation.Type) => awaited.Operation,
        IInvocationOperation invocation => GetFrameworkPayloadInvocationInput(invocation),
        IObjectCreationOperation { Arguments.Length: 1 } creation
            when creation.Type is INamedTypeSymbol named
            && named.OriginalDefinition.ToDisplayString() == "System.Threading.Tasks.ValueTask<TResult>"
            => creation.Arguments[0].Value,
        IPropertyReferenceOperation { Property.Name: "Result", Instance: { } instance } property
            when IsFrameworkPayloadContainer(property.Property.ContainingType) => instance,
        _ => null,
    };

    private static bool IsFrameworkPayloadContainer(INamedTypeSymbol type) =>
        type.OriginalDefinition.ToDisplayString() is
            "System.Threading.Tasks.Task<TResult>" or "System.Threading.Tasks.ValueTask<TResult>";

    private static IOperation? GetFrameworkPayloadInvocationInput(IInvocationOperation invocation) => invocation switch
    {
        { Arguments.Length: 1, TargetMethod: { Name: "FromResult", IsStatic: true } } factory
            when factory.TargetMethod.ContainingType.ToDisplayString() is
                "System.Threading.Tasks.Task" or "System.Threading.Tasks.ValueTask" => factory.Arguments[0].Value,
        { Instance: { } instance } wrapper
            when wrapper.TargetMethod.Name is "ConfigureAwait" or "AsTask"
            && IsFrameworkPayloadContainer(wrapper.TargetMethod.ContainingType) => instance,
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
        if (!HasStaticallyBoundPayloadBody(method)
            || invocation.SemanticModel?.Compilation is not { } compilation
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

    private static bool HasStaticallyBoundPayloadBody(IMethodSymbol method) =>
        !method.IsAbstract && !method.IsExtern
        && (!(method.IsVirtual || method.IsOverride) || method.IsSealed)
        && method.ContainingType.TypeKind != TypeKind.Interface;

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
            && IsPotentialScalarWrite(reference));

    private static bool HasUnsupportedLocalPayloadWrite(ILocalReferenceOperation localReference)
    {
        var callable = GetEnclosingCallable(localReference);
        var root = GetRoot(localReference);
        var hasJump = HasPayloadJump(callable ?? root);
        return root.DescendantsAndSelf().OfType<ILocalReferenceOperation>().Any(reference =>
        {
            if (!SymbolEqualityComparer.Default.Equals(reference.Local, localReference.Local)
                || !IsInReachableBranch(reference))
            {
                return false;
            }

            // Captured writes can run at a different point from their declaration.
            if (!ReferenceEquals(GetEnclosingCallable(reference), callable))
            {
                return IsPotentialScalarWrite(reference);
            }

            // A loop backedge or jump can execute a textually later write first.
            // A direct assignment before the use is handled by reaching values;
            // a dominating reset in the loop also kills ordinary backedge writes.
            if (hasJump)
            {
                return IsPotentialScalarWrite(reference);
            }

            if (SharesLoop(reference, localReference))
            {
                return IsUnsupportedLoopPayloadWrite(reference, localReference);
            }

            return reference.Syntax.SpanStart < localReference.Syntax.SpanStart
                && (HasRefStorageAncestor(reference)
                    || reference.Parent is IArgumentOperation { Parameter.RefKind: RefKind.Ref or RefKind.Out }
                    || (reference.Parent is ITupleOperation && IsPayloadAssignmentTarget(reference)));
        });
    }

    private static bool IsUnsupportedLoopPayloadWrite(ILocalReferenceOperation write, ILocalReferenceOperation use)
    {
        if (write.Parent is ISimpleAssignmentOperation { IsRef: false } assignment
            && ReferenceEquals(assignment.Target, write)
            && (write.Syntax.SpanStart < use.Syntax.SpanStart || HasLoopLocalReset(write, use)))
        {
            return false;
        }

        return IsPotentialScalarWrite(write);
    }

    private static bool HasLoopLocalReset(IOperation write, ILocalReferenceOperation use)
    {
        var assignments = FindLocalAssignments(GetRoot(use), use, use.Local, GetEnclosingCallable(use));
        return assignments.Any(assignment =>
        {
            if (assignment.IsRef || !IsLinearPredecessor(assignment, use))
            {
                return false;
            }

            for (var parent = write.Parent; parent is not null; parent = parent.Parent)
            {
                if (parent is ILoopOperation && HasAncestor(use, parent)
                    && !HasAncestor(assignment, parent))
                {
                    return false;
                }
            }

            return true;
        });
    }

    private static bool HasPayloadJump(IOperation operation) =>
        operation.DescendantsAndSelf().OfType<IBranchOperation>()
            .Any(static branch => branch.BranchKind == BranchKind.GoTo);

    private static bool SharesLoop(IOperation candidate, IOperation reference)
    {
        for (var parent = candidate.Parent; parent is not null; parent = parent.Parent)
        {
            if (parent is ILoopOperation && HasAncestor(reference, parent))
            {
                return true;
            }

            if (parent is IAnonymousFunctionOperation or ILocalFunctionOperation)
            {
                break;
            }
        }

        return false;
    }

    // A writable alias can mutate the referenced storage without another direct
    // reference to it. Treat escape as a write rather than trying to track aliases.
    internal static bool IsPotentialScalarWrite(IOperation reference) =>
        IsPayloadAssignmentTarget(reference)
        || reference.Parent is IArgumentOperation { Parameter.RefKind: RefKind.Ref or RefKind.Out }
            or IIncrementOrDecrementOperation
        || HasRefStorageAncestor(reference);

    private static bool HasRefStorageAncestor(IOperation reference)
    {
        for (var parent = reference.Parent; parent is not null; parent = parent.Parent)
        {
            if (parent is ISimpleAssignmentOperation
                {
                    IsRef: true,
                    Target: ILocalReferenceOperation { Local.RefKind: RefKind.Ref }
                        or IFieldReferenceOperation { Field.RefKind: RefKind.Ref },
                }
                or IVariableInitializerOperation { Parent: IVariableDeclaratorOperation { Symbol.RefKind: RefKind.Ref } })
            {
                return true;
            }

            if (parent is IAnonymousFunctionOperation or ILocalFunctionOperation)
            {
                break;
            }
        }

        return false;
    }

    private static bool IsPayloadAssignmentTarget(IOperation reference)
    {
        var target = reference;
        while (target.Parent is ITupleOperation)
        {
            target = target.Parent;
        }

        return target.Parent is IAssignmentOperation assignment && ReferenceEquals(assignment.Target, target);
    }

    private static bool HasUnsupportedPayloadCollections(IOperation operation, IOperation[] returns) =>
        !returns.SelectMany(GetReachableValueLeaves).All(static value => value is IParameterReferenceOperation)
        && operation.DescendantsAndSelf().Any(static child => child.Type is IArrayTypeSymbol
            || (child.Type is INamedTypeSymbol named && named.OriginalDefinition.ToDisplayString() is "System.Span<T>" or "System.ReadOnlySpan<T>"));
}
