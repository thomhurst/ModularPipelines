using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.Text;

namespace ModularPipelines.Analyzers;

/// <summary>Transfers definitions for one local without traversing control flow or evaluating children.</summary>
/// <remarks>
/// The caller supplies only executed stores, after successful evaluation of their operands and conversions.
/// Apply ref/out invalidation after all arguments evaluate, before following either call completion or exceptions.
/// Snapshot delegate inputs before evaluating the RHS. Apply deconstruction targets separately when intervening
/// setters or exceptions need observation. Invalidate member stores before a setter can mutate and throw.
/// Branch selection, state unions and exception routing belong to the caller.
/// Ref locals with backing storage outside the local-symbol alias closure always query as unknown,
/// including after whole-local stores. Ordinary reference-valued locals retain precise definitions.
/// Ref-return aliases and ref-like holders conservatively join the storage closure, including copies.
/// Unknown callees invalidate at call time; their bodies are never analyzed.
/// </remarks>
internal sealed class LocalDefinitionTransfer
{
    private static readonly TextSpan IncomingValue = new(int.MaxValue, 0);
    private static readonly TextSpan UnknownValue = new(int.MaxValue - 1, 0);
    private readonly IOperation _operationRoot;
    private readonly ILocalSymbol _local;
    private readonly Dictionary<TextSpan, IOperation> _definitions;
    private readonly HashSet<ILocalSymbol> _aliases;
    private readonly bool _hasUnrepresentedBackingStorage;

    internal LocalDefinitionTransfer(IOperation operationRoot, ILocalSymbol local)
    {
        _operationRoot = operationRoot;
        _local = local;
        _definitions = operationRoot.DescendantsAndSelf().SelectMany(operation => GetAssignedValues(operation, local))
            .OfType<IOperation>().GroupBy(static value => value.Syntax.Span)
            .ToDictionary(static group => group.Key, static group => group.First());
        _aliases = GetAliases(operationRoot, local);
        _hasUnrepresentedBackingStorage = local.RefKind != RefKind.None
            && operationRoot.DescendantsAndSelf().SelectMany(GetAliasSources)
                .Any(binding => _aliases.Contains(binding.Alias) && !HasRepresentableBackingStorage(binding.Source));
    }

    // States are caller-owned. Clone before following a branch; UnionWith joins reaching paths.
    internal static HashSet<TextSpan> CreateIncomingState() => [IncomingValue];

    internal bool TryGetValues(HashSet<TextSpan> state, out ImmutableArray<IOperation> values,
        out bool inheritsIncomingValue)
    {
        values = [];
        inheritsIncomingValue = false;
        if (_hasUnrepresentedBackingStorage || state.Contains(UnknownValue))
        {
            return false;
        }

        values = [.. state.Where(span => span != IncomingValue).Select(span => _definitions[span])];
        inheritsIncomingValue = state.Contains(IncomingValue);
        return true;
    }

    internal HashSet<TextSpan>? CaptureDelegateInputs(IOperation operation, HashSet<TextSpan> state) =>
        IsLocalDelegateAddition(operation) ? [with(state)] : null;

    private bool IsLocalDelegateAddition(IOperation operation) =>
        operation is ICompoundAssignmentOperation { Target: ILocalReferenceOperation target } assignment
        && SymbolEqualityComparer.Default.Equals(target.Local, _local) && IsDelegateAddition(assignment);

    internal void InvalidateByReferenceArguments(IOperation operation, HashSet<TextSpan> state)
    {
        if (GetByReferenceArguments(operation).Any(MayReferenceLocal))
        {
            Invalidate(state);
        }
    }

    internal void ApplyStore(IOperation operation, HashSet<TextSpan> state, HashSet<TextSpan>? delegateInputs = null)
    {
        if (operation is IDeconstructionAssignmentOperation deconstruction)
        {
            ApplyTargetStore(deconstruction.Target, deconstruction.Value, state);
            return;
        }

        // A captured ??= location lowers to a simple assignment on the store path only.
        if (operation is ISimpleAssignmentOperation assignment
            && IsLoweredCoalescingStore(operation))
        {
            ApplyAssignedValue(assignment.Value, state);
            return;
        }

        if (WritesThroughAliasOrMember(operation))
        {
            Invalidate(state);
            return;
        }

        if (delegateInputs is null && IsLocalDelegateAddition(operation))
        {
            // The post-RHS state cannot reconstruct the delegate's pre-RHS value.
            Invalidate(state);
            return;
        }

        foreach (var value in GetAssignedValues(operation, _local))
        {
            ApplyAssignedValue(value, state, delegateInputs);
        }
    }

    private bool IsLoweredCoalescingStore(IOperation operation) =>
        _operationRoot.SemanticModel?.GetOperation(operation.Syntax) is ICoalesceAssignmentOperation
        { Target: ILocalReferenceOperation reference }
        && SymbolEqualityComparer.Default.Equals(reference.Local, _local);

    private bool WritesThroughAliasOrMember(IOperation operation)
    {
        var target = operation switch
        {
            ISimpleAssignmentOperation { IsRef: true, Target: ILocalReferenceOperation } => null,
            IAssignmentOperation store => store.Target,
            IEventAssignmentOperation subscription => subscription.EventReference,
            IIncrementOrDecrementOperation increment => increment.Target,
            _ => null,
        };
        return target is not null && MayReferenceLocal(target)
            && (target is not ILocalReferenceOperation direct
                || !SymbolEqualityComparer.Default.Equals(direct.Local, _local));
    }

    internal void ApplyTargetStore(IOperation target, IOperation? value, HashSet<TextSpan> state)
    {
        if (target is IDeclarationExpressionOperation declaration)
        {
            target = declaration.Expression;
        }

        if (target is ITupleOperation tuple)
        {
            ApplyTupleStore(tuple, value, state);
        }
        else if (target is ILocalReferenceOperation reference
            && SymbolEqualityComparer.Default.Equals(reference.Local, _local))
        {
            ApplyAssignedValue(value, state);
        }
        else if (MayReferenceLocal(target))
        {
            Invalidate(state);
        }
    }

    private void ApplyTupleStore(ITupleOperation tuple, IOperation? value, HashSet<TextSpan> state)
    {
        while (value is IConversionOperation conversion)
        {
            value = conversion.Operand;
        }

        var values = value as ITupleOperation;
        for (var index = 0; index < tuple.Elements.Length; index++)
        {
            ApplyTargetStore(tuple.Elements[index], values is not null && index < values.Elements.Length
                ? values.Elements[index]
                : null, state);
        }
    }

    private void ApplyAssignedValue(IOperation? value, HashSet<TextSpan> state, HashSet<TextSpan>? delegateInputs = null)
    {
        if (value is not null && !_definitions.ContainsKey(value.Syntax.Span))
        {
            // Lowered stores such as foreach iteration variables have no registered source expression.
            value = null;
        }

        state.Clear();
        if (value is null)
        {
            state.Add(UnknownValue);
            return;
        }

        if (delegateInputs is not null)
        {
            state.UnionWith(delegateInputs);
        }

        state.Add(value.Syntax.Span);
    }

    private static void Invalidate(HashSet<TextSpan> state)
    {
        state.Clear();
        state.Add(UnknownValue);
    }

    // The engine owns call timing; reuse the same storage closure for implicit receivers.
    internal void InvalidateStorage(IOperation value, HashSet<TextSpan> state)
    {
        if (MayReferenceLocal(value))
        {
            Invalidate(state);
        }
    }

    internal bool ReferencesStorage(IOperation value) => MayReferenceLocal(value);

    private bool MayReferenceLocal(IOperation value) =>
        _aliases.Any(alias => TargetsLocal(value, alias, _operationRoot));

    private static bool IsDelegateAddition(ICompoundAssignmentOperation assignment) =>
        assignment.OperatorKind == BinaryOperatorKind.Add && assignment.Target.Type?.TypeKind == TypeKind.Delegate;

    private static bool HasKnownAssignedValue(IAssignmentOperation assignment) =>
        assignment is ISimpleAssignmentOperation { IsRef: false } or ICoalesceAssignmentOperation
        || (assignment is ICompoundAssignmentOperation compound && IsDelegateAddition(compound));

    private static IEnumerable<IOperation?> GetAssignedValues(IOperation operation, ILocalSymbol localSymbol)
    {
        if (operation is IVariableDeclaratorOperation { Initializer: { } initializer } declarator
            && SymbolEqualityComparer.Default.Equals(declarator.Symbol, localSymbol))
        {
            yield return initializer.Value;
        }
        else if (operation is IAssignmentOperation { Target: ILocalReferenceOperation target } assignment
            && SymbolEqualityComparer.Default.Equals(target.Local, localSymbol))
        {
            yield return HasKnownAssignedValue(assignment) ? assignment.Value : null;
        }
        else if (operation is IIncrementOrDecrementOperation { Target: ILocalReferenceOperation reference }
            && SymbolEqualityComparer.Default.Equals(reference.Local, localSymbol))
        {
            yield return null;
        }
        else if (operation is IDeconstructionAssignmentOperation deconstruction)
        {
            foreach (var value in GetDeconstructedValues(deconstruction.Target, deconstruction.Value, localSymbol))
            {
                yield return value;
            }
        }
        else if (SymbolEqualityComparer.Default.Equals(GetPatternLocal(operation), localSymbol))
        {
            // Pattern matching defines the local, but its extracted value has no independent expression.
            yield return null;
        }
    }

    private static ISymbol? GetPatternLocal(IOperation operation) => operation switch
    {
        IDeclarationPatternOperation pattern => pattern.DeclaredSymbol,
        IRecursivePatternOperation pattern => pattern.DeclaredSymbol,
        IListPatternOperation pattern => pattern.DeclaredSymbol,
        _ => null,
    };

    private static IEnumerable<IOperation?> GetDeconstructedValues(IOperation target, IOperation value,
        ILocalSymbol localSymbol)
    {
        if (target is IDeclarationExpressionOperation declaration)
        {
            target = declaration.Expression;
        }

        if (target is ILocalReferenceOperation reference
            && SymbolEqualityComparer.Default.Equals(reference.Local, localSymbol))
        {
            yield return value;
            yield break;
        }

        while (value is IConversionOperation conversion)
        {
            value = conversion.Operand;
        }

        if (target is not ITupleOperation targets)
        {
            yield break;
        }

        if (value is not ITupleOperation values)
        {
            // The tuple source has no element expressions to return for this write.
            if (TargetsLocal(target, localSymbol))
            {
                yield return null;
            }

            yield break;
        }

        foreach (var element in GetTupleElementValues(targets, values, localSymbol))
        {
            yield return element;
        }
    }

    private static IEnumerable<IOperation?> GetTupleElementValues(ITupleOperation targets, ITupleOperation values,
        ILocalSymbol localSymbol)
    {
        for (var index = 0; index < targets.Elements.Length && index < values.Elements.Length; index++)
        {
            foreach (var element in GetDeconstructedValues(targets.Elements[index], values.Elements[index], localSymbol))
            {
                yield return element;
            }
        }
    }

    private static IEnumerable<IOperation> GetByReferenceArguments(IOperation operation)
    {
        foreach (var argument in operation.ChildOperations.OfType<IArgumentOperation>())
        {
            if (argument.Parameter?.RefKind is RefKind.Ref or RefKind.Out)
            {
                yield return argument.Value;
            }
        }

        var dynamicArguments = operation switch
        {
            IDynamicInvocationOperation invocation => invocation.Arguments,
            IDynamicObjectCreationOperation creation => creation.Arguments,
            _ => [],
        };
        for (var index = 0; index < dynamicArguments.Length; index++)
        {
            var refKind = operation switch
            {
                IDynamicInvocationOperation invocation => invocation.GetArgumentRefKind(index),
                IDynamicObjectCreationOperation creation => creation.GetArgumentRefKind(index),
                _ => null,
            };
            if (refKind is RefKind.Ref or RefKind.Out)
            {
                yield return dynamicArguments[index];
            }
        }
    }

    // Bindings are a conservative callable-wide union; only traversed writes invalidate flow.
    private static HashSet<ILocalSymbol> GetAliases(IOperation root, ILocalSymbol local)
    {
        HashSet<ILocalSymbol> aliases = [with(SymbolEqualityComparer.Default), local];
        var sources = root.DescendantsAndSelf().SelectMany(GetAliasSources).ToArray();
        bool changed;
        do
        {
            changed = false;
            foreach (var (alias, source) in sources)
            {
                if (!aliases.Contains(alias) && aliases.Any(candidate => TargetsLocal(source, candidate)))
                {
                    changed |= aliases.Add(alias);
                }

                if (local.RefKind != RefKind.None && aliases.Contains(alias))
                {
                    foreach (var reference in source.DescendantsAndSelf().OfType<ILocalReferenceOperation>())
                    {
                        if (TargetsLocal(source, reference.Local))
                        {
                            changed |= aliases.Add(reference.Local);
                        }
                    }
                }
            }
        }
        while (changed);

        return aliases;
    }

    // Ref locals may alias heap storage that the local-symbol closure cannot represent. Keep those
    // queries unknown even after direct stores: later writes through another heap alias can change them.
    private static bool HasRepresentableBackingStorage(IOperation source) => source switch
    {
        ILocalReferenceOperation => true,
        IInlineArrayAccessOperation access => HasRepresentableBackingStorage(access.Instance),
        IFieldReferenceOperation { Instance: { Type.IsReferenceType: false } instance } => HasRepresentableBackingStorage(instance),
        IConditionalOperation { WhenFalse: { } whenFalse } conditional =>
            HasRepresentableBackingStorage(conditional.WhenTrue) && HasRepresentableBackingStorage(whenFalse),
        IConversionOperation conversion => HasRepresentableBackingStorage(conversion.Operand),
        IParenthesizedOperation parenthesized => HasRepresentableBackingStorage(parenthesized.Operand),
        _ => false,
    };

    private static IEnumerable<(ILocalSymbol Alias, IOperation Source)> GetAliasSources(IOperation operation)
    {
        if (GetDirectAliasSource(operation) is { } direct)
        {
            yield return direct;
        }
        else if (operation is ISimpleAssignmentOperation memberBinding
            && (memberBinding.IsRef || memberBinding.Value.Type?.IsRefLikeType == true))
        {
            foreach (var receiver in GetRefLikeReceivers(memberBinding.Target))
            {
                yield return (receiver.Local, memberBinding.Value);
            }
        }
        else if (operation is IDeconstructionAssignmentOperation deconstruction)
        {
            foreach (var binding in GetDeconstructionAliasSources(deconstruction.Target, deconstruction.Value))
            {
                yield return binding;
            }
        }
        else if (operation is IInvocationOperation or IObjectCreationOperation)
        {
            foreach (var binding in GetCallAliasSources(operation))
            {
                yield return binding;
            }
        }
    }

    private static IEnumerable<(ILocalSymbol Alias, IOperation Source)> GetDeconstructionAliasSources(
        IOperation target, IOperation value)
    {
        if (target is IDeclarationExpressionOperation declaration)
        {
            target = declaration.Expression;
        }

        while (value is IConversionOperation conversion)
        {
            value = conversion.Operand;
        }

        if (target is ITupleOperation targets)
        {
            for (var index = 0; index < targets.Elements.Length; index++)
            {
                // Tuple expressions have independent sources. An implicit Deconstruct can copy
                // receiver storage into any writable ref-like output, including nested outputs.
                var source = value is ITupleOperation values ? values.Elements[index] : value;
                foreach (var binding in GetDeconstructionAliasSources(targets.Elements[index], source))
                {
                    yield return binding;
                }
            }
        }
        else
        {
            foreach (var receiver in GetRefLikeReceivers(target))
            {
                yield return (receiver.Local, value);
            }
        }
    }

    private static (ILocalSymbol Alias, IOperation Source)? GetDirectAliasSource(IOperation operation) => operation switch
    {
        IVariableDeclaratorOperation { Initializer: { } initializer } declarator
            when declarator.Symbol.RefKind != RefKind.None || declarator.Symbol.Type.IsRefLikeType =>
            (declarator.Symbol, initializer.Value),
        ISimpleAssignmentOperation { Target: ILocalReferenceOperation reference } assignment
            when assignment.IsRef || reference.Local.Type.IsRefLikeType => (reference.Local, assignment.Value),
        IOperation pattern when GetPatternLocal(pattern) is ILocalSymbol { Type.IsRefLikeType: true } patternLocal
            && GetMatchedInput(pattern) is { } input => (patternLocal, input),
        _ => null,
    };

    private static IOperation? GetMatchedInput(IOperation pattern)
    {
        // Source-tree pattern bindings do not have variable initializers. Keep their copies
        // connected to the matched storage, including bindings in switch arms and cases.
        for (var parent = pattern.Parent; parent is not null; parent = parent.Parent)
        {
            switch (parent)
            {
                case IIsPatternOperation isPattern:
                    return isPattern.Value;
                case ISwitchExpressionOperation switchExpression:
                    return switchExpression.Value;
                case ISwitchOperation switchStatement:
                    return switchStatement.Value;
            }
        }

        return null;
    }

    private static IEnumerable<(ILocalSymbol Alias, IOperation Source)> GetCallAliasSources(IOperation operation)
    {
        foreach (var source in GetRetentionSources(operation))
        {
            foreach (var receiver in GetRetentionDestinations(operation))
            {
                yield return (receiver.Local, source);
            }
        }
    }

    private static IEnumerable<IOperation> GetRetentionSources(IOperation operation)
    {
        if (operation is IInvocationOperation { Instance: { Type.IsReferenceType: false } instance })
        {
            yield return instance;
        }

        foreach (var argument in operation.ChildOperations.OfType<IArgumentOperation>().Where(static argument =>
            argument.Parameter?.ScopedKind != ScopedKind.ScopedRef
            && (argument.Parameter?.RefKind != RefKind.None || argument.Value.Type?.IsRefLikeType == true)))
        {
            yield return argument.Value;
        }
    }

    private static IEnumerable<ILocalReferenceOperation> GetRetentionDestinations(IOperation operation)
    {
        if (operation is IInvocationOperation { Instance: { } instance })
        {
            foreach (var receiver in GetRefLikeReceivers(instance))
            {
                yield return receiver;
            }
        }

        // A callee can establish a retained binding through writable arguments as well as its
        // receiver. Scoped destinations still permit ref-field writes; scoping limits the source.
        foreach (var argument in operation.ChildOperations.OfType<IArgumentOperation>().Where(static argument =>
            argument.Parameter?.RefKind is RefKind.Ref or RefKind.Out && argument.Value.Type?.IsRefLikeType == true))
        {
            foreach (var receiver in GetRefLikeReceivers(argument.Value))
            {
                yield return receiver;
            }
        }
    }

    private static IEnumerable<ILocalReferenceOperation> GetRefLikeReceivers(IOperation receiver) =>
        receiver.DescendantsAndSelf().OfType<ILocalReferenceOperation>()
            .Where(static reference => reference.Local.Type.IsRefLikeType);

    private static bool TargetsLocal(IOperation target, ILocalSymbol local, IOperation? operationRoot = null) => target switch
    {
        ILocalReferenceOperation reference => SymbolEqualityComparer.Default.Equals(reference.Local, local),
        // A returned ref or ref-like value may retain any non-scoped by-reference input. Keep
        // the holder in the callable-wide closure so a later whole-local store cannot hide its writes.
        IInvocationOperation call => InvocationRetainsStorage(call, local, operationRoot),
        IObjectCreationOperation { Type.IsRefLikeType: true } creation =>
            CreationRetainsStorage(creation, local, operationRoot),
        // Interface-constrained type parameters may hold structs; only definite reference receivers preserve storage.
        IFieldReferenceOperation { Instance: { Type.IsReferenceType: false } instance } => TargetsLocal(instance, local, operationRoot),
        IEventReferenceOperation { Instance: { Type.IsReferenceType: false } instance } => TargetsLocal(instance, local, operationRoot),
        IPropertyReferenceOperation property => PropertyRetainsStorage(property, local, operationRoot),
        IInlineArrayAccessOperation access => TargetsLocal(access.Instance, local, operationRoot),
        IDeclarationExpressionOperation declaration => TargetsLocal(declaration.Expression, local, operationRoot),
        ITupleOperation tuple => tuple.Elements.Any(element => TargetsLocal(element, local, operationRoot)),
        IConditionalOperation conditional => ConditionalTargetsLocal(conditional, local, operationRoot),
        IConversionOperation conversion => TargetsLocal(conversion.Operand, local, operationRoot),
        IParenthesizedOperation parenthesized => TargetsLocal(parenthesized.Operand, local, operationRoot),
        // Conditional ref expressions can lower to captures anywhere along the storage receiver path.
        // Resolve against the original tree without recursing back into lowered captures.
        IFlowCaptureReferenceOperation when operationRoot is not null => CaptureTargetsLocal(target, local, operationRoot),
        _ => false,
    };

    private static bool CreationRetainsStorage(IObjectCreationOperation creation, ILocalSymbol local, IOperation? operationRoot) =>
        creation.Arguments.Any(argument => CanRetainStorage(argument, local, operationRoot))
        || (creation.Initializer is { } initializer
            && initializer.Descendants().OfType<ISimpleAssignmentOperation>().Any(assignment =>
                (assignment.IsRef || assignment.Value.Type?.IsRefLikeType == true)
                && TargetsLocal(assignment.Value, local, operationRoot)));

    private static bool InvocationRetainsStorage(IInvocationOperation call, ILocalSymbol local, IOperation? operationRoot)
    {
        if (!call.TargetMethod.ReturnsByRef && !call.TargetMethod.ReturnsByRefReadonly && call.Type?.IsRefLikeType != true)
        {
            return false;
        }

        return (call.Instance is { Type.IsReferenceType: false } receiver && TargetsLocal(receiver, local, operationRoot))
            || call.Arguments.Any(argument => CanRetainStorage(argument, local, operationRoot));
    }

    private static bool PropertyRetainsStorage(IPropertyReferenceOperation property, ILocalSymbol local, IOperation? operationRoot) =>
        (property.Instance is { Type.IsReferenceType: false } receiver && TargetsLocal(receiver, local, operationRoot))
        || ((property.Property.ReturnsByRef || property.Property.ReturnsByRefReadonly || property.Type?.IsRefLikeType == true)
            && property.Arguments.Any(argument => CanRetainStorage(argument, local, operationRoot)));

    private static bool CanRetainStorage(IArgumentOperation argument, ILocalSymbol local, IOperation? operationRoot) =>
        argument.Parameter?.ScopedKind != ScopedKind.ScopedRef
        && (argument.Parameter?.RefKind != RefKind.None || argument.Value.Type?.IsRefLikeType == true)
        && TargetsLocal(argument.Value, local, operationRoot);

    private static bool ConditionalTargetsLocal(IConditionalOperation conditional, ILocalSymbol local, IOperation? operationRoot) =>
        TargetsLocal(conditional.WhenTrue, local, operationRoot)
        || (conditional.WhenFalse is { } whenFalse && TargetsLocal(whenFalse, local, operationRoot));

    private static bool CaptureTargetsLocal(IOperation target, ILocalSymbol local, IOperation operationRoot) =>
        operationRoot.DescendantsAndSelf().Any(candidate =>
            candidate.Syntax.Span == target.Syntax.Span && TargetsLocal(candidate, local));
}
