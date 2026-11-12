using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

namespace ModularPipelines.Analyzers;

internal static partial class ModuleAuthoringAnalysis
{
    /// <summary>
    /// Proves that forwarded cancellation containers retain their input storage.
    /// </summary>
    /// <remarks>
    /// This bounded syntax-based proof follows reachable ordinary and ref aliases,
    /// container writes, and escapes. It accepts independent storage only when all
    /// reaching values are fresh allocations and a reset executes on every path.
    /// Unknown calls, writable escapes, captured writes, and unproven control flow
    /// retain uncertainty. This is not general heap tracking or a replacement for
    /// the separate control-flow analysis of cancellation-token local values.
    /// </remarks>
    internal static class ContainerStorageAnalysis
    {
        private static IEnumerable<IOperation> GetReachableContainerOperations(IOperation root)
        {
            var children = root.DescendantsAndSelf().ToArray();
            var invocations = children.OfType<IInvocationOperation>().ToImmutableArray();
            var callables = children.Select(GetCallableSymbol)
                .Where(static callable => callable is not null)
                .Cast<IMethodSymbol>()
                .ToImmutableArray();
            var caller = GetCallableSymbol(root) is null ? null : root;
            var reachable = GetReachableCallables(invocations, callables, GetCallableSymbol(caller));
            AddEscapedContainerCallables(children, caller, static _ => true, invocations, callables, reachable);
            return children.Where(IsInReachableBranch)
                .Where(child => IsInsideReachableCallable(child, reachable));
        }

        internal static bool HasContainerElementMutation(
            IOperation operation, ITypeSymbol? returnType, HashSet<IParameterSymbol> flowedParameters)
        {
            if (!IsCancellationContainer(returnType)
                && !flowedParameters.Any(static parameter => IsCancellationContainer(parameter.Type)))
            {
                return false;
            }

            var relevant = GetForwardedContainerSymbols(operation, flowedParameters);
            return GetReachableContainerOperations(operation).Any(child =>
                MutatesOrExposesContainerStorage(child, relevant)
                || ReplacesForwardedContainerStorage(child, relevant));
        }

        private static bool MutatesOrExposesContainerStorage(IOperation operation, ContainerStorageSymbols relevant) =>
            (ExposesCancellationContainer(operation)
                || (IsCancellationContainerElement(operation) && MayWriteContainerElement(operation)))
            && ReferencesContainer(operation, relevant);

        private static bool ReplacesForwardedContainerStorage(IOperation operation, ContainerStorageSymbols relevant) =>
            operation is IAssignmentOperation assignment
            && !IsRefRebinding(assignment)
            && (GetContainerStorageReferences(assignment.Target).Any(target =>
                    target is IParameterReferenceOperation parameter && relevant.Contains(parameter.Parameter))
                || WritesThroughContainerRefLocal(assignment.Target, relevant));

        private static ContainerStorageSymbols GetForwardedContainerSymbols(
            IOperation operation, HashSet<IParameterSymbol> flowedParameters,
            ILocalReferenceOperation? callerReference = null,
            int storageStart = int.MinValue)
        {
            var relevant = new ContainerStorageSymbols(flowedParameters.Where(static parameter => IsCancellationContainer(parameter.Type)));
            if (callerReference is not null)
            {
                relevant.Add(callerReference.Local);
                relevant.Allocations.UnionWith(GetContainerAllocationValues(callerReference, [with(SymbolEqualityComparer.Default)]).Where(IsFreshArrayStorage));
            }

            var children = (callerReference is null
                    ? GetReachableContainerOperations(operation)
                    : GetCallerContainerOperations(operation, callerReference, storageStart))
                .ToArray();
            AddCallerAllocationAliases(children, callerReference, storageStart, relevant);

            AddCallableContainerSymbols(children, GetEnclosingCallable(callerReference ?? operation), relevant);

            // Follow possible aliases in either direction. Fresh unrelated containers
            // remain independent; scalar reads such as Length do not create aliases.
            bool changed;
            do
            {
                var count = relevant.Count;
                foreach (var child in children)
                {
                    AddContainerAliases(child, relevant);
                }

                changed = relevant.Count != count;
            }
            while (changed);

            return relevant;
        }

        private static void AddCallableContainerSymbols(
            IOperation[] children, IOperation? callable, ContainerStorageSymbols relevant)
        {
            // Parameters owned by this callable may alias even when only one has proven
            // provenance. Nested callable parameters can receive independent scratch storage.
            foreach (var parameter in children.OfType<IParameterReferenceOperation>())
            {
                if (IsCancellationContainer(parameter.Type)
                    && ReferenceEquals(GetEnclosingCallable(parameter), callable))
                {
                    relevant.Add(parameter.Parameter);
                }
            }

            foreach (var returned in children.OfType<IReturnOperation>())
            {
                if (returned.ReturnedValue is { } value && IsCancellationContainer(value.Type)
                    && ReferenceEquals(GetEnclosingCallable(returned), callable))
                {
                    AddContainerReferences(value, relevant);
                    relevant.Allocations.UnionWith(GetContainerAllocationValues(value, [with(SymbolEqualityComparer.Default)]).Where(IsFreshArrayStorage));
                }
            }
        }

        private static void AddCallerAllocationAliases(
            IOperation[] children, ILocalReferenceOperation? callerReference,
            int storageStart, ContainerStorageSymbols relevant)
        {
            if (callerReference is null)
            {
                return;
            }

            // An allocation assignment expression returns the new storage now
            // held by the caller local, rather than its earlier storage.
            foreach (var assignment in children.OfType<ISimpleAssignmentOperation>())
            {
                if (assignment.Target is ILocalReferenceOperation target
                    && SymbolEqualityComparer.Default.Equals(target.Local, callerReference.Local)
                    && assignment.Syntax.SpanStart < callerReference.Syntax.SpanStart
                    && assignment.Syntax.Span.End > storageStart)
                {
                    if (assignment.Parent is IVariableInitializerOperation { Parent: IVariableDeclaratorOperation declaration })
                    {
                        relevant.Add(declaration.Symbol);
                    }
                    else if (assignment.Parent is ISimpleAssignmentOperation outerAssignment)
                    {
                        AddContainerReferences(outerAssignment.Target, relevant);
                    }
                }
            }
        }

        private static IEnumerable<IOperation> GetCallerContainerOperations(
            IOperation root, ILocalReferenceOperation reference, int storageStart)
        {
            var children = root.DescendantsAndSelf().ToArray();
            var caller = GetEnclosingCallable(reference);
            var consumptionEnd = GetContainerConsumptionEnd(reference);
            bool IsWithinStorageLifetime(IOperation child) =>
                child.Syntax.Span.End > storageStart
                && child.Syntax.SpanStart < consumptionEnd;

            // Nested bodies execute at their call sites, irrespective of where the
            // declaration appears. Bound outer calls to the current storage lifetime
            // and follow nested calls transitively, including delegate invocations.
            var invocations = children.OfType<IInvocationOperation>()
                .Where(invocation => !ReferenceEquals(GetEnclosingCallable(invocation), caller)
                    || IsWithinStorageLifetime(invocation))
                .ToImmutableArray();
            var callables = children.Select(GetCallableSymbol)
                .Where(static callable => callable is not null)
                .Cast<IMethodSymbol>()
                .ToImmutableArray();
            var reachable = GetReachableCallables(invocations, callables, GetCallableSymbol(caller));
            AddEscapedContainerCallables(children, caller, IsWithinStorageLifetime, invocations, callables, reachable);
            return children.Where(IsInReachableBranch).Where(child =>
                ReferenceEquals(GetEnclosingCallable(child), caller)
                    ? IsWithinStorageLifetime(child)
                    : GetCallableSymbol(GetEnclosingCallable(child)) is { } callable
                      && reachable.Contains(callable));
        }

        private static int GetContainerConsumptionEnd(ILocalReferenceOperation reference)
        {
            var end = reference.Syntax.SpanStart;
            for (var parent = reference.Parent; parent is not null; parent = parent.Parent)
            {
                if (parent is IVariableInitializerOperation or IExpressionStatementOperation
                    or IReturnOperation or IAnonymousFunctionOperation or ILocalFunctionOperation)
                {
                    break;
                }

                if (parent is IInvocationOperation or IObjectCreationOperation)
                {
                    // Every argument is evaluated before the callee consumes storage.
                    end = parent.Syntax.Span.End;
                    if (parent is IInvocationOperation invocation && IsKnownCancellationCarrierFactory(invocation))
                    {
                        // Registration captures cancellation independently of later storage writes.
                        break;
                    }
                }
            }

            return end;
        }

        private static void AddEscapedContainerCallables(
            IOperation[] children, IOperation? caller, Func<IOperation, bool> isWithinStorageLifetime,
            ImmutableArray<IInvocationOperation> invocations, ImmutableArray<IMethodSymbol> callables,
            HashSet<IMethodSymbol> reachable)
        {
            var arguments = children.OfType<IArgumentOperation>().Where(IsInReachableBranch).ToArray();
            bool changed;
            do
            {
                var count = reachable.Count;
                foreach (var argument in arguments)
                {
                    var enclosing = GetEnclosingCallable(argument);
                    if (ReferenceEquals(enclosing, caller)
                        ? !isWithinStorageLifetime(argument)
                        : GetCallableSymbol(enclosing) is not { } owner || !reachable.Contains(owner))
                    {
                        continue;
                    }

                    foreach (var callable in callables)
                    {
                        if (!reachable.Contains(callable)
                            && ValueContainsCallable(argument.Value, callable, [with(SymbolEqualityComparer.Default)]))
                        {
                            reachable.UnionWith(GetReachableCallables(invocations, callables, callable));
                        }
                    }
                }

                changed = count != reachable.Count;
            }
            while (changed);
        }

        private static void AddContainerAliases(IOperation child, ContainerStorageSymbols relevant)
        {
            if (child is IVariableDeclaratorOperation { Initializer.Value: { } initializer } declaration
                && IsCancellationContainer(initializer.Type)
                && (relevant.Contains(declaration.Symbol) || ReferencesContainer(initializer, relevant)))
            {
                relevant.Add(declaration.Symbol);
                AddContainerReferences(initializer, relevant);
            }
            else if (child is IAssignmentOperation assignment
                && assignment is ISimpleAssignmentOperation { IsRef: false } or ICoalesceAssignmentOperation
                && IsCancellationContainer(assignment.Value.Type)
                && (ReferencesContainer(assignment.Target, relevant) || ReferencesContainer(assignment.Value, relevant)))
            {
                AddContainerReferences(assignment.Target, relevant);
                AddContainerReferences(assignment.Value, relevant);
            }
            else if (child is IDeconstructionAssignmentOperation deconstruction)
            {
                AddDeconstructionContainerAliases(deconstruction.Target, deconstruction.Value, relevant);
            }
        }

        private static void AddDeconstructionContainerAliases(
            IOperation target, IOperation value, ContainerStorageSymbols relevant,
            HashSet<ILocalSymbol>? visitedLocals = null)
        {
            visitedLocals ??= [with(SymbolEqualityComparer.Default)];
            if (value is IConversionOperation or IConditionalOperation or ISwitchExpressionOperation or ICoalesceOperation)
            {
                foreach (var leaf in GetReachableValueLeaves(value))
                {
                    AddDeconstructionContainerAliases(target, leaf, relevant, CloneVisitedLocals(visitedLocals));
                }

                return;
            }

            if (value is ILocalReferenceOperation local && visitedLocals.Add(local.Local))
            {
                foreach (var reachingValue in FindReachingLocalValues(local, local.Local))
                {
                    AddDeconstructionContainerAliases(target, reachingValue, relevant, CloneVisitedLocals(visitedLocals));
                }

                return;
            }

            if (target is IDeclarationExpressionOperation declaration)
            {
                target = declaration.Expression;
            }

            if (value is IConversionOperation conversion)
            {
                value = conversion.Operand;
            }

            AddMatchingContainerAliases(target, value, relevant, visitedLocals);
        }

        private static void AddMatchingContainerAliases(
            IOperation target, IOperation value, ContainerStorageSymbols relevant,
            HashSet<ILocalSymbol> visitedLocals)
        {
            if (target is ITupleOperation targets && value is ITupleOperation values
                && targets.Elements.Length == values.Elements.Length)
            {
                for (var index = 0; index < targets.Elements.Length; index++)
                {
                    AddDeconstructionContainerAliases(targets.Elements[index], values.Elements[index], relevant, CloneVisitedLocals(visitedLocals));
                }
            }
            else if (IsCancellationContainer(value.Type)
                     && (ReferencesContainer(target, relevant) || ReferencesContainer(value, relevant)))
            {
                AddContainerReferences(target, relevant);
                AddContainerReferences(value, relevant);
            }
        }

        private static void AddContainerReferences(IOperation operation, HashSet<ISymbol> symbols)
        {
            foreach (var child in GetContainerStorageReferences(operation))
            {
                if (IsCancellationContainer(child.Type) && GetContainerSymbol(child) is { } symbol)
                {
                    symbols.Add(symbol);
                }
            }
        }

        private static bool ReferencesContainer(IOperation operation, ContainerStorageSymbols symbols) =>
            GetContainerStorageReferences(operation).Any(child =>
                (GetContainerSymbol(child) is { } symbol && symbols.Contains(symbol)
                    && !HasIndependentContainerAllocation(child, symbols))
                || (child is IFieldReferenceOperation or IPropertyReferenceOperation && IsCancellationContainer(child.Type)));

        private static bool HasIndependentContainerAllocation(IOperation operation, ContainerStorageSymbols symbols)
        {
            if (operation is not ILocalReferenceOperation { Local.RefKind: RefKind.None })
            {
                return false;
            }

            if (!symbols.IndependentReferences.TryGetValue(operation, out var independent))
            {
                var values = GetContainerAllocationValues(operation, [with(SymbolEqualityComparer.Default)]).ToArray();
                independent = values.Length > 0 && values.All(value => IsFreshArrayStorage(value) && !symbols.Allocations.Contains(value));
                symbols.IndependentReferences.Add(operation, independent);
            }

            return independent;
        }

        private static IEnumerable<IOperation> GetContainerAllocationValues(IOperation operation, HashSet<ILocalSymbol> visited)
        {
            if (IsFreshArrayStorage(operation))
            {
                return [operation];
            }

            if (operation is ILocalReferenceOperation { Local.RefKind: RefKind.None } local)
            {
                return GetLocalContainerAllocationValues(local, visited);
            }

            if (operation is ISimpleAssignmentOperation assignment)
            {
                return GetContainerAllocationValues(assignment.Value, visited);
            }

            if (operation is ICoalesceAssignmentOperation coalesce)
            {
                return GetContainerAllocationValues(coalesce.Target, CloneVisitedLocals(visited))
                    .Concat(GetContainerAllocationValues(coalesce.Value, CloneVisitedLocals(visited)));
            }

            if (operation is IInvocationOperation invocation)
            {
                // Unknown returns stay conservative, but their input allocations may
                // be the storage ultimately returned or consumed by this proof.
                return GetInvocationContainerAllocationValues(invocation, visited);
            }

            var leaves = GetReachableValueLeaves(operation).ToArray();
            return leaves.Length == 1 && ReferenceEquals(leaves[0], operation)
                ? [operation]
                : leaves.SelectMany(value => GetContainerAllocationValues(value, CloneVisitedLocals(visited)));
        }

        private static IEnumerable<IOperation> GetLocalContainerAllocationValues(
            ILocalReferenceOperation reference, HashSet<ILocalSymbol> visited)
        {
            if (!visited.Add(reference.Local))
            {
                return [reference];
            }

            var values = GetReachingContainerLocalValues(reference).ToArray();
            var allocations = values.SelectMany(value => GetContainerAllocationValues(value, CloneVisitedLocals(visited)));
            // Preserve known inputs when an escape makes the full origin uncertain.
            // They can still be aliases of the storage consumed by the proof.
            return values.Length == 0 || HasLoopCarriedContainerAssignment(reference) || HasWritableContainerStorageAlias(reference) || HasUncertainContainerAssignment(reference)
                ? allocations.Prepend(reference)
                : allocations;
        }

        private static bool HasUncertainContainerAssignment(ILocalReferenceOperation reference)
        {
            var callable = GetEnclosingCallable(reference);
            var root = GetRoot(reference);
            var operations = GetReachableContainerOperations(root).ToArray();
            var reset = FindLocalAssignments(root, reference, reference.Local, callable)
                .FirstOrDefault(assignment => IsDefiniteContainerPredecessor(assignment, reference));
            var lifetime = GetContainerAssignmentLifetime(reference, reset, operations);
            return operations.Any(operation => operation switch
            {
                // Captured writes execute at call sites, rather than declaration offsets.
                IAssignmentOperation { Target: ILocalReferenceOperation target }
                    when SymbolEqualityComparer.Default.Equals(target.Local, reference.Local)
                        && !ReferenceEquals(GetEnclosingCallable(operation), callable) =>
                    lifetime is null || lifetime.Contains(operation)
                    || HasEscapedContainerWriter(operations),
                IDeconstructionAssignmentOperation assignment =>
                    CanDeconstructionReachContainerReference(assignment, reference, operations)
                    && !IsSupersededContainerDeconstruction(assignment, reference, reset, operations)
                    && assignment.Target.DescendantsAndSelf().OfType<ILocalReferenceOperation>()
                        .Any(target => SymbolEqualityComparer.Default.Equals(target.Local, reference.Local)),
                IBranchOperation { BranchKind: BranchKind.GoTo } branch =>
                    CanGotoRevisitContainerReference(branch, reference, operations),
                _ => false,
            });
        }

        private static HashSet<IOperation>? GetContainerAssignmentLifetime(
            ILocalReferenceOperation reference, ISimpleAssignmentOperation? reset, IOperation[] operations)
        {
            // Structured and unstructured back edges can execute earlier calls again.
            if (reset is null || operations.Any(operation => operation is IBranchOperation { BranchKind: BranchKind.GoTo }
                    || (operation is ILoopOperation loop && HasAncestor(reference, loop))))
            {
                return null;
            }

            return [.. GetCallerContainerOperations(GetRoot(reference), reference, reset.Syntax.Span.End)];
        }

        private static bool IsSupersededContainerDeconstruction(
            IDeconstructionAssignmentOperation assignment, ILocalReferenceOperation reference,
            ISimpleAssignmentOperation? reset, IOperation[] operations) =>
            reset is not null && assignment.Syntax.SpanStart < reset.Syntax.SpanStart
            && ReferenceEquals(GetEnclosingCallable(assignment), GetEnclosingCallable(reference))
            && !operations.OfType<ILoopOperation>().Any(loop => HasAncestor(reference, loop) && HasAncestor(assignment, loop));

        private static bool HasEscapedContainerWriter(IOperation[] operations)
        {
            // An escaped wrapper may invoke the writer transitively after the reset.
            var callables = operations.Select(GetCallableSymbol).OfType<IMethodSymbol>().ToArray();
            return operations.OfType<IArgumentOperation>().Any(argument => callables.Any(callable =>
                ValueContainsCallable(argument.Value, callable, [with(SymbolEqualityComparer.Default)])));
        }

        private static bool CanDeconstructionReachContainerReference(
            IDeconstructionAssignmentOperation assignment, ILocalReferenceOperation reference, IOperation[] operations) =>
            !ReferenceEquals(GetEnclosingCallable(assignment), GetEnclosingCallable(reference))
            || assignment.Syntax.SpanStart < reference.Syntax.SpanStart
            || operations.OfType<ILoopOperation>().Any(loop => HasAncestor(reference, loop) && HasAncestor(assignment, loop));

        private static bool CanGotoRevisitContainerReference(
            IBranchOperation branch, ILocalReferenceOperation reference, IOperation[] operations) =>
            ReferenceEquals(GetEnclosingCallable(branch), GetEnclosingCallable(reference))
            && branch.Syntax.SpanStart > reference.Syntax.SpanStart
            && operations.OfType<ILabeledOperation>().Any(label =>
                SymbolEqualityComparer.Default.Equals(label.Label, branch.Target)
                && label.Syntax.SpanStart <= reference.Syntax.SpanStart
                && operations.Any(operation => operation.Syntax.SpanStart > reference.Syntax.SpanStart
                    && operation.Syntax.SpanStart < branch.Syntax.SpanStart
                    && operation is IAssignmentOperation assignment
                    && GetAssignedContainerLocals(assignment.Target).Any(local =>
                        SymbolEqualityComparer.Default.Equals(local, reference.Local)))
                && !FindLocalAssignments(GetRoot(reference), reference, reference.Local, GetEnclosingCallable(reference))
                    .Any(assignment => assignment.Syntax.SpanStart > label.Syntax.SpanStart
                        && IsDefiniteContainerPredecessor(assignment, reference)));

        private static IEnumerable<ILocalSymbol> GetAssignedContainerLocals(IOperation target) => target switch
        {
            ILocalReferenceOperation local => [local.Local],
            ITupleOperation tuple => tuple.Elements.SelectMany(GetAssignedContainerLocals),
            _ => [],
        };

        private static bool HasWritableContainerStorageAlias(ILocalReferenceOperation reference) =>
            GetRoot(reference).DescendantsAndSelf().Any(operation =>
                operation.Syntax.SpanStart < reference.Syntax.SpanStart
                && GetWritableContainerStorage(operation) is { } storage
                && GetContainerStorageReferences(storage).OfType<ILocalReferenceOperation>()
                    .Any(local => SymbolEqualityComparer.Default.Equals(local.Local, reference.Local)));

        private static IOperation? GetWritableContainerStorage(IOperation operation) => operation switch
        {
            IVariableDeclaratorOperation { Symbol.RefKind: not RefKind.None, Initializer.Value: { } value } => value,
            ISimpleAssignmentOperation { IsRef: true } assignment => assignment.Value,
            IArgumentOperation { Parameter.RefKind: RefKind.Ref or RefKind.Out } argument => argument.Value,
            _ => null,
        };

        private static IEnumerable<IOperation> GetInvocationContainerAllocationValues(
            IInvocationOperation invocation, HashSet<ILocalSymbol> visited)
        {
            var inputs = invocation.Arguments.Select(static argument => argument.Value);
            if (invocation.Instance is { } receiver)
            {
                inputs = inputs.Append(receiver);
            }

            return inputs.SelectMany(value => GetContainerAllocationValues(value, CloneVisitedLocals(visited)))
                .Prepend(invocation);
        }

        private static IEnumerable<IOperation> GetReachingContainerLocalValues(ILocalReferenceOperation reference)
        {
            var root = GetRoot(reference);
            var callable = GetEnclosingCallable(reference);
            var assignments = FindLocalAssignments(root, reference, reference.Local, callable)
                .Cast<IAssignmentOperation>()
                .Concat(GetReachableContainerOperations(root).OfType<ICoalesceAssignmentOperation>()
                    .Where(assignment => assignment.Target is ILocalReferenceOperation target
                        && SymbolEqualityComparer.Default.Equals(target.Local, reference.Local)
                        && ReferenceEquals(GetEnclosingCallable(assignment), callable)
                        && assignment.Syntax.SpanStart < reference.Syntax.SpanStart))
                .Where(IsInReachableBranch)
                .OrderByDescending(static assignment => assignment.Syntax.SpanStart).ToArray();
            var definite = assignments.FirstOrDefault(assignment => assignment is ISimpleAssignmentOperation
                && IsDefiniteContainerPredecessor(assignment, reference));
            if (definite is not null)
            {
                return assignments.Where(assignment => assignment.Syntax.SpanStart >= definite.Syntax.SpanStart)
                    .Select(static assignment => assignment.Value);
            }

            var branch = FindLatestDefinitelyAssigningBranch(
                GetRoot(reference), reference, reference.Local, GetEnclosingCallable(reference));
            if (branch is IConditionalOperation conditional
                && IsDirectContainerAssignment(conditional.WhenTrue, reference.Local)
                && conditional.WhenFalse is { } whenFalse
                && IsDirectContainerAssignment(whenFalse, reference.Local)
                && IsDefiniteContainerPredecessor(branch, reference))
            {
                return assignments.Where(assignment => assignment.Syntax.SpanStart >= branch.Syntax.SpanStart)
                    .Select(static assignment => assignment.Value);
            }

            // Without a definite write, the general value lookup can discard the
            // original storage at a skipped assignment (for example a short-circuit
            // operand or a bypassed goto). Keep every possible write and initializer.
            if (assignments.Length > 0)
            {
                var values = assignments.Select(static assignment => assignment.Value);
                var initializer = FindLocalDeclarator(GetRoot(reference), reference, reference.Local, GetEnclosingCallable(reference))
                    ?.Initializer?.Value;
                return initializer is not null ? values.Append(initializer) : values;
            }

            return FindReachingLocalValues(reference, reference.Local);
        }

        // Keep this branch-reset proof bounded: each arm must execute one direct
        // assignment, with no intervening control flow that could skip the write.
        private static bool IsDirectContainerAssignment(IOperation operation, ILocalSymbol local) => operation switch
        {
            ISimpleAssignmentOperation assignment => AssignsLocal(assignment, local),
            IExpressionStatementOperation statement => IsDirectContainerAssignment(statement.Operation, local),
            IBlockOperation { Operations.Length: 1 } block => IsDirectContainerAssignment(block.Operations[0], local),
            _ => false,
        };

        private static bool IsDefiniteContainerPredecessor(IOperation candidate, IOperation reference)
        {
            for (var ancestor = candidate.Parent; ancestor is not null; ancestor = ancestor.Parent)
            {
                if (ancestor is IBlockOperation && HasAncestor(reference, ancestor))
                {
                    return !HasBypassingContainerGoto(candidate, reference);
                }

                if (ancestor is ILoopOperation or ISwitchOperation or ITryOperation
                    || (ancestor is ISwitchExpressionArmOperation arm && !IsGuaranteedContainerSwitchArm(arm))
                    || ancestor is IConditionalOperation { Condition.ConstantValue: not { HasValue: true, Value: bool } }
                    || CanSkipContainerOperand(ancestor, candidate))
                {
                    return false;
                }
            }

            return false;
        }

        private static bool IsGuaranteedContainerSwitchArm(ISwitchExpressionArmOperation arm) =>
            arm is { Guard: null, Pattern: IDiscardPatternOperation, Parent: ISwitchExpressionOperation { Arms.Length: 1 } };

        private static bool CanSkipContainerOperand(IOperation ancestor, IOperation candidate) => ancestor switch
        {
            IBinaryOperation { OperatorKind: BinaryOperatorKind.ConditionalAnd } binary
                when HasAncestor(candidate, binary.RightOperand) =>
                binary.LeftOperand.ConstantValue is not { HasValue: true, Value: true },
            IBinaryOperation { OperatorKind: BinaryOperatorKind.ConditionalOr } binary
                when HasAncestor(candidate, binary.RightOperand) =>
                binary.LeftOperand.ConstantValue is not { HasValue: true, Value: false },
            ICoalesceOperation coalesce when HasAncestor(candidate, coalesce.WhenNull) =>
                coalesce.Value.ConstantValue is not { HasValue: true, Value: null },
            ICoalesceAssignmentOperation assignment when HasAncestor(candidate, assignment.Value) =>
                assignment.Target.ConstantValue is not { HasValue: true, Value: null },
            IConditionalAccessOperation access when HasAncestor(candidate, access.WhenNotNull) => true,
            _ => false,
        };

        private static bool HasBypassingContainerGoto(IOperation candidate, IOperation reference)
        {
            var operations = GetReachableContainerOperations(GetRoot(reference)).ToArray();
            var callable = GetEnclosingCallable(reference);
            return operations.OfType<IBranchOperation>().Any(branch =>
                branch.BranchKind == BranchKind.GoTo
                && branch.Syntax.SpanStart < candidate.Syntax.SpanStart
                && ReferenceEquals(GetEnclosingCallable(branch), callable)
                && operations.OfType<ILabeledOperation>().Any(label =>
                    SymbolEqualityComparer.Default.Equals(label.Label, branch.Target)
                    && label.Syntax.SpanStart > candidate.Syntax.SpanStart
                    && label.Syntax.SpanStart <= reference.Syntax.SpanStart));
        }

        private static bool HasLoopCarriedContainerAssignment(ILocalReferenceOperation reference)
        {
            var callable = GetEnclosingCallable(reference);
            var root = GetRoot(reference);
            var predecessor = (IOperation?) FindLocalAssignments(root, reference, reference.Local, callable)
                .FirstOrDefault(binding => IsLinearPredecessor(binding, reference))
                ?? FindLocalDeclarator(root, reference, reference.Local, callable);
            for (var ancestor = reference.Parent; ancestor is not null && !ReferenceEquals(ancestor, callable); ancestor = ancestor.Parent)
            {
                if (ancestor is ILoopOperation loop
                    && !IsContainerResetBeforeReference(predecessor, reference, loop)
                    && HasLaterContainerAssignment(loop, reference, callable))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsContainerResetBeforeReference(IOperation? predecessor, IOperation reference, ILoopOperation loop) =>
            predecessor is not null && HasAncestor(predecessor, loop.Body) && IsDefiniteContainerPredecessor(predecessor, reference);

        private static bool HasLaterContainerAssignment(ILoopOperation loop, ILocalReferenceOperation reference, IOperation? callable) =>
            loop.DescendantsAndSelf().OfType<ISimpleAssignmentOperation>().Any(assignment =>
                assignment.Syntax.SpanStart > reference.Syntax.SpanStart
                && assignment.Target is ILocalReferenceOperation target
                && SymbolEqualityComparer.Default.Equals(target.Local, reference.Local)
                && ReferenceEquals(GetEnclosingCallable(assignment), callable)
                && IsInReachableBranch(assignment));

        private sealed class ContainerStorageSymbols(IEnumerable<ISymbol> symbols) : HashSet<ISymbol>(symbols, SymbolEqualityComparer.Default)
        {
            public HashSet<IOperation> Allocations { get; } = [];

            public Dictionary<IOperation, bool> IndependentReferences { get; } = [];
        }

        private static IEnumerable<IOperation> GetContainerStorageReferences(IOperation operation, HashSet<IOperation>? visited = null) => operation switch
        {
            ILocalReferenceOperation { Local.RefKind: not RefKind.None } local
                when IsCancellationContainer(local.Type) => GetRefLocalContainerStorageReferences(local, visited ?? []),
            ILocalReferenceOperation or IParameterReferenceOperation or IFieldReferenceOperation or IPropertyReferenceOperation
                when IsCancellationContainer(operation.Type) => [operation],
            IPropertyReferenceOperation { Property.IsIndexer: true, Instance: { } instance } => GetContainerStorageReferences(instance, visited),
            IConversionOperation conversion => GetContainerStorageReferences(conversion.Operand, visited),
            ISimpleAssignmentOperation assignment => GetContainerStorageReferences(assignment.Value, visited),
            ICoalesceAssignmentOperation assignment => GetContainerStorageReferences(assignment.Target, visited)
                .Concat(GetContainerStorageReferences(assignment.Value, visited)),
            _ => GetCompositeContainerStorageReferences(operation, visited),
        };

        private static IEnumerable<IOperation> GetRefLocalContainerStorageReferences(
            ILocalReferenceOperation reference, HashSet<IOperation> visited)
        {
            if (!visited.Add(reference))
            {
                return [];
            }

            // The RHS of a rebinding reads the old location. Reference occurrences,
            // rather than local symbols, distinguish self-rebinding from recursion.
            var root = GetRoot(reference);
            var callable = GetEnclosingCallable(reference);
            var bindings = FindLocalAssignments(root, reference, reference.Local, callable)
                .Where(static assignment => assignment.IsRef)
                .Where(assignment => !assignment.Syntax.Span.Contains(reference.Syntax.Span))
                .Where(IsInReachableBranch)
                .ToArray();
            var latest = bindings.FirstOrDefault(binding => IsDefiniteContainerPredecessor(binding, reference));
            var values = bindings
                .Where(binding => latest is null || binding.Syntax.SpanStart >= latest.Syntax.SpanStart)
                .Concat(GetLoopCarriedRefBindings(reference, (IOperation?) latest ?? FindLocalDeclarator(root, reference, reference.Local, callable)))
                .Concat(GetGotoCarriedRefBindings(reference))
                .Select(static binding => binding.Value);
            if (latest is null
                && FindLocalDeclarator(root, reference, reference.Local, callable)?.Initializer?.Value is { } initializer)
            {
                values = values.Append(initializer);
            }

            return values.SelectMany(value => GetContainerStorageReferences(value, [.. visited]));
        }

        private static IEnumerable<ISimpleAssignmentOperation> GetGotoCarriedRefBindings(ILocalReferenceOperation reference)
        {
            var operations = GetReachableContainerOperations(GetRoot(reference)).ToArray();
            var callable = GetEnclosingCallable(reference);
            foreach (var branch in operations.OfType<IBranchOperation>().Where(branch =>
                         branch.BranchKind == BranchKind.GoTo
                         && branch.Syntax.SpanStart > reference.Syntax.SpanStart
                         && ReferenceEquals(GetEnclosingCallable(branch), callable)))
            {
                var label = operations.OfType<ILabeledOperation>().FirstOrDefault(label =>
                    SymbolEqualityComparer.Default.Equals(label.Label, branch.Target));
                if (label is null || label.Syntax.SpanStart > reference.Syntax.SpanStart)
                {
                    continue;
                }

                var bindings = operations.OfType<ISimpleAssignmentOperation>()
                    .Where(binding => IsReachableRefBinding(binding, reference.Local, callable)).ToArray();
                if (bindings.Any(binding => binding.Syntax.SpanStart > label.Syntax.SpanStart
                        && binding.Syntax.SpanStart < reference.Syntax.SpanStart
                        && IsDefiniteContainerPredecessor(binding, reference)))
                {
                    continue;
                }

                foreach (var binding in bindings.Where(binding => binding.Syntax.SpanStart > reference.Syntax.SpanStart
                             && binding.Syntax.SpanStart < branch.Syntax.SpanStart
                             && !bindings.Any(later => later.Syntax.SpanStart > binding.Syntax.SpanStart
                                 && later.Syntax.SpanStart < branch.Syntax.SpanStart
                                 && IsDefiniteContainerPredecessor(later, branch))))
                {
                    yield return binding;
                }
            }
        }

        private static IEnumerable<ISimpleAssignmentOperation> GetLoopCarriedRefBindings(
            ILocalReferenceOperation reference, IOperation? predecessor)
        {
            var callable = GetEnclosingCallable(reference);
            for (var ancestor = reference.Parent; ancestor is not null && !ReferenceEquals(ancestor, callable); ancestor = ancestor.Parent)
            {
                if (ancestor is not ILoopOperation loop)
                {
                    continue;
                }

                // A guaranteed rebinding in the body replaces the previous iteration's target.
                if (predecessor is not null && HasAncestor(predecessor, loop.Body) && IsDefiniteContainerPredecessor(predecessor, reference))
                {
                    continue;
                }

                foreach (var binding in GetSurvivingLoopContainerBindings(loop, reference, callable))
                {
                    yield return binding;
                }
            }
        }

        private static IEnumerable<ISimpleAssignmentOperation> GetSurvivingLoopContainerBindings(
            ILoopOperation loop, ILocalReferenceOperation reference, IOperation? callable)
        {
            var bindings = loop.DescendantsAndSelf().OfType<ISimpleAssignmentOperation>()
                .Where(binding => IsReachableRefBinding(binding, reference.Local, callable))
                .ToArray();
            return bindings.Where(binding => binding.Syntax.SpanStart > reference.Syntax.SpanStart
                && !HasSupersedingContainerBinding(bindings, loop, binding));
        }

        private static bool HasSupersedingContainerBinding(
            ISimpleAssignmentOperation[] bindings, ILoopOperation loop, ISimpleAssignmentOperation binding) =>
            bindings.Any(later => later.Syntax.SpanStart > binding.Syntax.SpanStart
                && IsDefiniteContainerPredecessor(later, binding)
                && !HasInterveningContainerBranch(loop, binding, later));

        private static bool HasInterveningContainerBranch(
            ILoopOperation loop, IOperation binding, IOperation later) =>
            // Only a branch that can bypass replacement and reach another iteration
            // preserves the earlier target. Exiting this loop cannot carry a binding.
            loop.DescendantsAndSelf().OfType<IBranchOperation>().Any(branch =>
                branch.Syntax.SpanStart > binding.Syntax.SpanStart
                && branch.Syntax.SpanStart < later.Syntax.SpanStart
                && ReferenceEquals(GetEnclosingCallable(branch), GetEnclosingCallable(binding))
                && IsInReachableBranch(branch)
                && CanCarryContainerBinding(branch, loop, binding, later));

        private static bool CanCarryContainerBinding(
            IBranchOperation branch, ILoopOperation loop, IOperation binding, IOperation later)
        {
            if (branch.BranchKind == BranchKind.GoTo)
            {
                return CanGotoCarryContainerBinding(branch, loop, binding, later);
            }

            for (var ancestor = branch.Parent; ancestor is not null; ancestor = ancestor.Parent)
            {
                if (ancestor is ILoopOperation targetLoop)
                {
                    if (SymbolEqualityComparer.Default.Equals(targetLoop.ContinueLabel, branch.Target))
                    {
                        return ReferenceEquals(targetLoop, loop) || HasAncestor(later, targetLoop.Body);
                    }

                    if (SymbolEqualityComparer.Default.Equals(targetLoop.ExitLabel, branch.Target))
                    {
                        return !ReferenceEquals(targetLoop, loop) && HasAncestor(later, targetLoop.Body);
                    }
                }
                else if (ancestor is ISwitchOperation targetSwitch
                         && SymbolEqualityComparer.Default.Equals(targetSwitch.ExitLabel, branch.Target))
                {
                    return HasAncestor(later, targetSwitch);
                }
            }

            return false;
        }

        private static bool CanGotoCarryContainerBinding(
            IBranchOperation branch, ILoopOperation loop, IOperation binding, IOperation later)
        {
            var target = GetRoot(loop).DescendantsAndSelf().OfType<ILabeledOperation>()
                .FirstOrDefault(label => SymbolEqualityComparer.Default.Equals(label.Label, branch.Target));
            // Switch-case targets have no labeled operation; keep those conservative.
            return target is null
                || target.Syntax.SpanStart <= binding.Syntax.SpanStart
                || (HasAncestor(target, loop.Body) && target.Syntax.SpanStart > later.Syntax.SpanStart);
        }

        private static bool IsReachableRefBinding(
            ISimpleAssignmentOperation binding, ILocalSymbol local, IOperation? callable) =>
            binding.IsRef
            && binding.Target is ILocalReferenceOperation target
            && SymbolEqualityComparer.Default.Equals(target.Local, local)
            && ReferenceEquals(GetEnclosingCallable(binding), callable)
            && IsInReachableBranch(binding);

        private static bool IsRefRebinding(IAssignmentOperation assignment) =>
            assignment is ISimpleAssignmentOperation { IsRef: true };

        private static bool WritesThroughContainerRefLocal(IOperation target, ContainerStorageSymbols relevant) => target switch
        {
            ILocalReferenceOperation { Local.RefKind: not RefKind.None } local => ReferencesContainer(local, relevant),
            ITupleOperation tuple => tuple.Elements.Any(element => WritesThroughContainerRefLocal(element, relevant)),
            IConversionOperation conversion => WritesThroughContainerRefLocal(conversion.Operand, relevant),
            IDeclarationExpressionOperation declaration => WritesThroughContainerRefLocal(declaration.Expression, relevant),
            _ => false,
        };

        private static IEnumerable<IOperation> GetCompositeContainerStorageReferences(IOperation operation, HashSet<IOperation>? visited) => operation switch
        {
            ISwitchExpressionOperation expression => expression.Arms.SelectMany(arm => GetContainerStorageReferences(arm.Value, visited)),
            IArgumentOperation argument => GetContainerStorageReferences(argument.Value, visited),
            IConditionalOperation conditional => GetContainerStorageReferences(conditional.WhenTrue, visited)
                .Concat(conditional.WhenFalse is { } alternative ? GetContainerStorageReferences(alternative, visited) : []),
            ICoalesceOperation coalesce => GetContainerStorageReferences(coalesce.Value, visited).Concat(GetContainerStorageReferences(coalesce.WhenNull, visited)),
            ITupleOperation tuple => tuple.Elements.SelectMany(child => GetContainerStorageReferences(child, visited)),
            IDeclarationExpressionOperation declaration => GetContainerStorageReferences(declaration.Expression, visited),
            IDeconstructionAssignmentOperation assignment => GetContainerStorageReferences(assignment.Target, visited).Concat(GetContainerStorageReferences(assignment.Value, visited)),
            IArrayElementReferenceOperation array => GetContainerStorageReferences(array.ArrayReference, visited),
            _ => GetInvocationContainerStorageReferences(operation, visited),
        };

        private static IEnumerable<IOperation> GetInvocationContainerStorageReferences(IOperation operation, HashSet<IOperation>? visited) => operation switch
        {
            IInvocationOperation invocation => invocation.Arguments.SelectMany(child => GetContainerStorageReferences(child, visited))
                .Concat(invocation.Instance is { } receiver ? GetContainerStorageReferences(receiver, visited) : []),
            IDynamicMemberReferenceOperation { Instance: { } instance } => GetContainerStorageReferences(instance, visited),
            IDynamicInvocationOperation or IDynamicObjectCreationOperation => operation.ChildOperations.SelectMany(child => GetContainerStorageReferences(child, visited)),
            _ => [],
        };

        private static ISymbol? GetContainerSymbol(IOperation operation) => operation switch
        {
            ILocalReferenceOperation local => local.Local,
            IParameterReferenceOperation parameter => parameter.Parameter,
            _ => null,
        };

        private static bool IsCancellationContainerElement(IOperation operation)
        {
            return operation switch
            {
                IArrayElementReferenceOperation array => IsCancellationContainer(array.ArrayReference.Type),
                IPropertyReferenceOperation { Property.IsIndexer: true, Instance: { } instance } => IsCancellationContainer(instance.Type),
                _ => false,
            };
        }

        private static bool ExposesCancellationContainer(IOperation operation)
        {
            return operation switch
            {
                IArgumentOperation argument => !(argument.Parent is IInvocationOperation invocation && IsKnownCancellationCarrierFactory(invocation))
                    && IsCancellationContainer(argument.Value.Type),
                IInvocationOperation invocation => IsCancellationContainer(invocation.Instance?.Type),
                IConversionOperation conversion => IsCancellationContainer(conversion.Operand.Type) && !IsCancellationContainer(conversion.Type),
                ISimpleAssignmentOperation assignment => assignment.Target is IFieldReferenceOperation or IPropertyReferenceOperation
                    && IsCancellationContainer(assignment.Value.Type),
                IDynamicInvocationOperation or IDynamicObjectCreationOperation => true,
                _ => false,
            };
        }

        internal static bool IsCancellationContainer(ITypeSymbol? type)
        {
            return !IsCancellationCarrier(type) && IsCancellationInput(type);
        }

        private static bool MayWriteContainerElement(IOperation element)
        {
            // Only the storage expression itself can be written. An element read
            // inside another target's index or receiver is not a write to that element.
            var target = element;
            while (target.Parent is IConversionOperation or ITupleOperation or IDeclarationExpressionOperation)
            {
                target = target.Parent;
            }

            return element.Syntax.Parent is RefExpressionSyntax || target.Parent switch
            {
                IAssignmentOperation assignment => ReferenceEquals(assignment.Target, target),
                IIncrementOrDecrementOperation => true,
                IArgumentOperation argument => argument.Parameter?.RefKind is RefKind.Ref or RefKind.Out,
                IVariableInitializerOperation { Parent: IVariableDeclaratorOperation declaration } => declaration.Symbol.RefKind != RefKind.None,
                _ => false,
            };
        }

        internal static bool HasCallerContainerMutation(ILocalReferenceOperation localReference, int storageStart)
        {
            var root = GetRoot(localReference);
            var relevant = GetForwardedContainerSymbols(root, [], localReference, storageStart);
            return GetCallerContainerOperations(root, localReference, storageStart).Any(child =>
                        !child.Syntax.Span.Contains(localReference.Syntax.Span)
                        && (MutatesOrExposesContainerStorage(child, relevant)
                            || (child is IAssignmentOperation assignment
                                && !IsRefRebinding(assignment)
                                && (WritesThroughContainerRefLocal(assignment.Target, relevant)
                                    || (!ReferenceEquals(GetEnclosingCallable(child), GetEnclosingCallable(localReference))
                                        && GetContainerStorageReferences(assignment.Target).OfType<ILocalReferenceOperation>()
                                            .Any(target => SymbolEqualityComparer.Default.Equals(target.Local, localReference.Local)))))));
        }
    }
}
