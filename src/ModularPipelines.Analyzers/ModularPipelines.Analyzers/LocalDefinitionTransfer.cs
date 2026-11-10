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
/// </remarks>
internal sealed class LocalDefinitionTransfer
{
    private static readonly TextSpan IncomingValue = new(int.MaxValue, 0);
    private static readonly TextSpan UnknownValue = new(int.MaxValue - 1, 0);
    private readonly IOperation _operationRoot;
    private readonly ILocalSymbol _local;
    private readonly Dictionary<TextSpan, IOperation> _definitions;
    private readonly HashSet<ILocalSymbol> _aliases;

    internal LocalDefinitionTransfer(IOperation operationRoot, ILocalSymbol local)
    {
        _operationRoot = operationRoot;
        _local = local;
        _definitions = operationRoot.DescendantsAndSelf().SelectMany(operation => GetAssignedValues(operation, local))
            .OfType<IOperation>().GroupBy(static value => value.Syntax.Span)
            .ToDictionary(static group => group.Key, static group => group.First());
        _aliases = GetAliases(operationRoot, local);
    }

    // States are caller-owned. Clone before following a branch; UnionWith joins reaching paths.
    internal static HashSet<TextSpan> CreateIncomingState() => [IncomingValue];

    internal bool TryGetValues(HashSet<TextSpan> state, out ImmutableArray<IOperation> values,
        out bool inheritsIncomingValue)
    {
        values = [];
        inheritsIncomingValue = false;
        if (state.Contains(UnknownValue))
        {
            return false;
        }

        values = [.. state.Where(span => span != IncomingValue).Select(span => _definitions[span])];
        inheritsIncomingValue = state.Contains(IncomingValue);
        return true;
    }

    internal HashSet<TextSpan>? CaptureDelegateInputs(IOperation operation, HashSet<TextSpan> state) =>
        operation is ICompoundAssignmentOperation { Target: ILocalReferenceOperation target } assignment
        && SymbolEqualityComparer.Default.Equals(target.Local, _local) && IsDelegateAddition(assignment)
            ? [with(state)] : null;

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
            && _operationRoot.SemanticModel?.GetOperation(operation.Syntax) is ICoalesceAssignmentOperation
            { Target: ILocalReferenceOperation reference }
            && SymbolEqualityComparer.Default.Equals(reference.Local, _local))
        {
            ApplyAssignedValue(assignment.Value, state);
            return;
        }

        var target = operation switch
        {
            ISimpleAssignmentOperation { IsRef: true } => null,
            IAssignmentOperation store => store.Target,
            IIncrementOrDecrementOperation increment => increment.Target,
            _ => null,
        };
        if (target is not null && MayReferenceLocal(target)
            && (target is not ILocalReferenceOperation direct
                || !SymbolEqualityComparer.Default.Equals(direct.Local, _local)))
        {
            Invalidate(state);
            return;
        }

        foreach (var value in GetAssignedValues(operation, _local))
        {
            ApplyAssignedValue(value, state, delegateInputs);
        }
    }

    internal void ApplyTargetStore(IOperation target, IOperation? value, HashSet<TextSpan> state)
    {
        if (target is IDeclarationExpressionOperation declaration)
        {
            target = declaration.Expression;
        }

        if (target is ITupleOperation tuple)
        {
            while (value is IConversionOperation conversion)
            {
                value = conversion.Operand;
            }

            var values = value as ITupleOperation;
            for (var index = 0; index < tuple.Elements.Length; index++)
            {
                ApplyTargetStore(tuple.Elements[index], values is not null && index < values.Elements.Length
                    ? values.Elements[index] : null, state);
            }
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

    private bool MayReferenceLocal(IOperation value) =>
        _aliases.Any(alias => TargetsLocal(value, alias))
        // Conditional ref expressions are lowered to captures; consult the original storage tree.
        || (value is IFlowCaptureReferenceOperation && _operationRoot.DescendantsAndSelf().Any(candidate =>
            candidate.Syntax.Span == value.Syntax.Span && _aliases.Any(alias => TargetsLocal(candidate, alias))));

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

                if (aliases.Contains(alias))
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

    private static IEnumerable<(ILocalSymbol Alias, IOperation Source)> GetAliasSources(IOperation operation)
    {
        if (operation is IVariableDeclaratorOperation { Symbol.RefKind: not RefKind.None, Initializer: { } initializer } declarator)
        {
            yield return (declarator.Symbol, initializer.Value);
        }
        else if (operation is ISimpleAssignmentOperation { IsRef: true, Target: ILocalReferenceOperation reference } assignment)
        {
            yield return (reference.Local, assignment.Value);
        }
    }

    private static bool TargetsLocal(IOperation target, ILocalSymbol local) => target switch
    {
        ILocalReferenceOperation reference => SymbolEqualityComparer.Default.Equals(reference.Local, local),
        IFieldReferenceOperation { Instance: { Type.IsValueType: true } instance } => TargetsLocal(instance, local),
        IPropertyReferenceOperation { Instance: { Type.IsValueType: true } instance } => TargetsLocal(instance, local),
        IDeclarationExpressionOperation declaration => TargetsLocal(declaration.Expression, local),
        ITupleOperation tuple => tuple.Elements.Any(element => TargetsLocal(element, local)),
        IConditionalOperation conditional => TargetsLocal(conditional.WhenTrue, local)
            || (conditional.WhenFalse is { } whenFalse && TargetsLocal(whenFalse, local)),
        IConversionOperation conversion => TargetsLocal(conversion.Operand, local),
        IParenthesizedOperation parenthesized => TargetsLocal(parenthesized.Operand, local),
        _ => false,
    };
}
