using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.Text;

namespace ModularPipelines.Analyzers;

/// <summary>Tracks storage aliases over Roslyn's lowered control flow, not source statement order.</summary>
internal static class CapturedCancellationTokenAnalysis
{
    private static readonly ConditionalWeakTable<IOperation, ConcurrentDictionary<IParameterSymbol, Lazy<bool>>> Results = new();

    internal static bool IsStable(IOperation root, IParameterSymbol parameter) =>
        Results.GetValue(root, static _ => new ConcurrentDictionary<IParameterSymbol, Lazy<bool>>(SymbolEqualityComparer.Default))
            .GetOrAdd(parameter, token => new Lazy<bool>(() => Analyze(root, token))).Value;

    private static bool Analyze(IOperation root, IParameterSymbol parameter)
    {
        var graph = root switch
        {
            IMethodBodyOperation body => ControlFlowGraph.Create(body),
            IConstructorBodyOperation body => ControlFlowGraph.Create(body),
            IBlockOperation body => ControlFlowGraph.Create(body),
            IFieldInitializerOperation initializer => ControlFlowGraph.Create(initializer),
            IPropertyInitializerOperation initializer => ControlFlowGraph.Create(initializer),
            _ => null,
        };
        // Invalid/incomplete editor code must not establish a mutable capture as safe.
        return graph is not null && !IsWritten(graph, parameter, [with(SymbolEqualityComparer.Default)]);
    }

    private static bool IsWritten(ControlFlowGraph graph, ISymbol trackedSymbol, HashSet<IMethodSymbol> visited)
    {
        var analysis = new WriteAnalysis(graph, trackedSymbol);
        if (analysis.Run())
        {
            return true;
        }

        foreach (var callable in analysis.ReferencedCallables)
        {
            if (!visited.Add(callable))
            {
                continue;
            }

            for (var owner = graph; owner is not null; owner = owner.Parent)
            {
                var local = owner.LocalFunctions.FirstOrDefault(symbol => SymbolEqualityComparer.Default.Equals(symbol, callable));
                if (local is not null && IsWritten(owner.GetLocalFunctionControlFlowGraph(local), trackedSymbol, visited))
                {
                    return true;
                }
            }

            foreach (var anonymous in analysis.AnonymousFunctions.Where(candidate => SymbolEqualityComparer.Default.Equals(candidate.Symbol, callable)))
            {
                if (IsWritten(graph.GetAnonymousFunctionControlFlowGraph(anonymous), trackedSymbol, visited))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private sealed class Storage
    {
        internal HashSet<ISymbol> Aliases { get; } = [with(SymbolEqualityComparer.Default)];
        internal HashSet<CaptureId> Captures { get; } = [];

        internal Storage Clone()
        {
            var copy = new Storage();
            copy.Aliases.UnionWith(Aliases);
            copy.Captures.UnionWith(Captures);
            return copy;
        }

        internal bool Merge(Storage incoming)
        {
            var count = Aliases.Count + Captures.Count;
            Aliases.UnionWith(incoming.Aliases);
            Captures.UnionWith(incoming.Captures);
            return count != Aliases.Count + Captures.Count;
        }
    }

    // Continuations keep finally exits separate: a returning branch cannot flow into
    // the next normal statement, even when it executes the same finally as that path.
    private sealed class Continuation
    {
        internal int Block { get; set; } = -1;
        internal ControlFlowRegion? Search { get; set; }
        internal ITypeSymbol? ExceptionType { get; set; }
        internal ControlFlowRegion? Handlers { get; set; }
        internal int HandlerIndex { get; set; }
        internal Continuation? Next { get; set; }
        internal Continuation? Unwind { get; set; }
    }

    private sealed class WriteAnalysis(ControlFlowGraph graph, ISymbol trackedSymbol)
    {
        private readonly Queue<(int Block, Continuation? Resume)> _pending = new();
        private readonly Dictionary<(int Block, Continuation? Resume), Storage> _inputs = [];
        private readonly Dictionary<(ControlFlowBranch Branch, Continuation? Resume), Continuation?> _branches = [];
        private readonly Dictionary<(ControlFlowRegion Region, int Index, ITypeSymbol? Type, Continuation? Ambient, Continuation? Unwind), Continuation> _handlers = [];
        private readonly Dictionary<(ControlFlowRegion Region, ITypeSymbol? Type, Continuation? Ambient, Continuation? Unwind), Continuation> _exceptions = [];
        private BasicBlock _block = graph.Blocks[0];
        private bool _written;
        private Continuation? _resume;
        private readonly SemanticModel? _model = graph.OriginalOperation.SemanticModel;
        private readonly INamedTypeSymbol? _exceptionType = graph.OriginalOperation.SemanticModel?.Compilation.GetTypeByMetadataName("System.Exception");
        private readonly OperationExceptionClassifier _exceptionClassifier = new(graph.OriginalOperation.SemanticModel);
        private readonly ImmutableArray<TextSpan> _earlyCollectionCalls =
            OperationExceptionClassifier.GetEarlyCollectionCallSpans(graph.OriginalOperation);

        internal HashSet<IMethodSymbol> ReferencedCallables { get; } = [with(SymbolEqualityComparer.Default)];
        internal HashSet<IFlowAnonymousFunctionOperation> AnonymousFunctions { get; } = [];

        internal bool Run()
        {
            var initial = new Storage();
            initial.Aliases.Add(trackedSymbol);
            Enqueue(0, null, initial);
            while (!_written && _pending.Count != 0)
            {
                var point = _pending.Dequeue();
                _block = graph.Blocks[point.Block];
                _resume = point.Resume;
                var storage = _inputs[point].Clone();
                foreach (var operation in _block.Operations)
                {
                    Visit(operation, storage);
                }

                if (_block.BranchValue is { } value)
                {
                    Visit(value, storage);
                }

                Follow(_block.FallThroughSuccessor, point.Resume, storage);
                Follow(_block.ConditionalSuccessor, point.Resume, storage);
            }

            return _written;
        }

        private void Enqueue(int block, Continuation? resume, Storage storage)
        {
            var point = (block, resume);
            if (!_inputs.TryGetValue(point, out var existing))
            {
                _inputs.Add(point, storage.Clone());
                _pending.Enqueue(point);
            }
            else if (existing.Merge(storage))
            {
                _pending.Enqueue(point);
            }
        }

        private void Follow(ControlFlowBranch? branch, Continuation? resume, Storage storage)
        {
            if (branch is null)
            {
                return;
            }

            if (!IsFeasibleBranch(branch))
            {
                return;
            }

            if (branch.Semantics is ControlFlowBranchSemantics.Throw or ControlFlowBranchSemantics.Rethrow)
            {
                if (!IsExhaustiveSwitchFailure(_block.BranchValue))
                {
                    // A throw expression may hold any runtime subtype of its static type.
                    SearchException(_block.EnclosingRegion, storage, null, _resume);
                }
                return;
            }

            if (branch.Semantics == ControlFlowBranchSemantics.StructuredExceptionHandling)
            {
                Resume(resume, storage);
                return;
            }

            if (branch.Semantics is ControlFlowBranchSemantics.ProgramTermination or ControlFlowBranchSemantics.Error)
            {
                return;
            }

            if (!_branches.TryGetValue((branch, resume), out var continuation))
            {
                continuation = CreateBranchContinuation(branch, resume);
                _branches.Add((branch, resume), continuation);
            }

            Resume(continuation, storage);
        }

        private bool IsFeasibleBranch(ControlFlowBranch branch) =>
            ConstantCondition(_block.BranchValue) is not bool condition
            || _block.ConditionKind == ControlFlowConditionKind.None
            || branch.IsConditionalSuccessor == (condition == (_block.ConditionKind == ControlFlowConditionKind.WhenTrue));

        private Continuation? CreateBranchContinuation(ControlFlowBranch branch, Continuation? resume)
        {
            // A filter's accepted edge enters the handler with ordinary flow.
            var acceptedFilter = FindRegion(_block.EnclosingRegion, ControlFlowRegionKind.Filter) is { } filter
                && branch.Destination is { } target && target.Ordinal > filter.LastBlockOrdinal;
            var continuation = branch.Destination is { } destination
                ? new Continuation { Block = destination.Ordinal, Next = acceptedFilter ? resume?.Next : resume }
                : null;
            if (acceptedFilter)
            {
                continuation = ApplyUnwind(resume?.Unwind, continuation);
            }

            foreach (var cleanup in branch.FinallyRegions.Reverse())
            {
                continuation = new Continuation { Block = cleanup.FirstBlockOrdinal, Next = continuation };
            }

            return continuation;
        }

        private void Resume(Continuation? continuation, Storage storage)
        {
            if (continuation is null)
            {
                return;
            }

            if (continuation.Block >= 0)
            {
                Enqueue(continuation.Block, continuation.Next, storage);
            }
            else if (continuation.Handlers is { } handlers)
            {
                DispatchHandler(handlers, continuation.HandlerIndex, storage, continuation.ExceptionType, continuation.Next, continuation.Unwind);
            }
            else if (continuation.Search is { } search)
            {
                SearchException(search, storage, continuation.ExceptionType, continuation.Next, continuation.Unwind);
            }
        }

        private void SearchException(ControlFlowRegion? region, Storage storage, ITypeSymbol? type, Continuation? ambient, Continuation? unwind = null)
        {
            for (; region is not null; region = region.EnclosingRegion)
            {
                if (region.Kind == ControlFlowRegionKind.Filter
                    && region.EnclosingRegion is { } filtered
                    && filtered.EnclosingRegion is { } handlers)
                {
                    // A filter exception behaves like false, retaining side effects.
                    DispatchHandler(handlers, handlers.NestedRegions.IndexOf(filtered) + 1, storage, null, ambient?.Next, ambient?.Unwind);
                    return;
                }

                if (region.Kind != ControlFlowRegionKind.Try || region.EnclosingRegion is not { } enclosing)
                {
                    continue;
                }

                if (enclosing.Kind == ControlFlowRegionKind.TryAndCatch)
                {
                    DispatchHandler(enclosing, 1, storage, type, ambient, unwind);
                    return;
                }

                if (enclosing.Kind == ControlFlowRegionKind.TryAndFinally)
                {
                    // Exception filters run during the search pass, before stack unwinding.
                    // Retain cleanups until a handler is selected, including filter side effects.
                    if (!_exceptions.TryGetValue((enclosing, type, ambient, unwind), out var continuation))
                    {
                        continuation = new Continuation { Block = enclosing.NestedRegions[1].FirstBlockOrdinal, Next = unwind };
                        _exceptions.Add((enclosing, type, ambient, unwind), continuation);
                    }

                    unwind = continuation;
                }
            }

            Resume(ApplyUnwind(unwind, null), storage);
        }

        private void DispatchHandler(ControlFlowRegion handlers, int index, Storage storage, ITypeSymbol? type, Continuation? ambient, Continuation? unwind = null)
        {
            for (; index < handlers.NestedRegions.Length; index++)
            {
                var handler = handlers.NestedRegions[index];
                var caughtType = handler.ExceptionType;
                var catchesAll = CatchesAll(caughtType);
                if (type is not null && !catchesAll && !DerivesFrom(type, caughtType))
                {
                    continue;
                }

                EnterHandler(handlers, index, storage, type, ambient, unwind);

                if (type is not null || catchesAll)
                {
                    return;
                }
            }

            SearchException(handlers.EnclosingRegion, storage, type, ambient, unwind);
        }

        private void EnterHandler(ControlFlowRegion handlers, int index, Storage storage, ITypeSymbol? type, Continuation? ambient, Continuation? unwind)
        {
            var handler = handlers.NestedRegions[index];
            var cleanup = FindRegion(handler, ControlFlowRegionKind.Finally);
            Continuation? handlerResume = null;
            if (cleanup is not null && _block.Ordinal >= cleanup.FirstBlockOrdinal && _block.Ordinal <= cleanup.LastBlockOrdinal)
            {
                handlerResume = ambient;
            }

            if (handler.Kind == ControlFlowRegionKind.FilterAndHandler)
            {
                if (!_handlers.TryGetValue((handlers, index, type, handlerResume, unwind), out var fallback))
                {
                    fallback = new Continuation { Handlers = handlers, HandlerIndex = index + 1, ExceptionType = type, Next = handlerResume, Unwind = unwind };
                    _handlers.Add((handlers, index, type, handlerResume, unwind), fallback);
                }

                Enqueue(handler.FirstBlockOrdinal, fallback, storage);
            }
            else
            {
                Resume(ApplyUnwind(unwind, GetContinuation(handler.FirstBlockOrdinal, handlerResume)), storage);
            }
        }

        private readonly Dictionary<(int Block, Continuation? Next), Continuation> _continuations = [];

        private Continuation GetContinuation(int block, Continuation? next)
        {
            if (!_continuations.TryGetValue((block, next), out var continuation))
            {
                continuation = new Continuation { Block = block, Next = next };
                _continuations.Add((block, next), continuation);
            }

            return continuation;
        }

        private Continuation? ApplyUnwind(Continuation? unwind, Continuation? destination)
        {
            // Search encounters the inner finally first, so the accumulated list is reversed.
            for (var current = unwind; current is not null; current = current.Next)
            {
                destination = GetContinuation(current.Block, destination);
            }

            return destination;
        }

        private static bool DerivesFrom(ITypeSymbol type, ITypeSymbol? expected)
        {
            for (var current = type; current is not null; current = current.BaseType)
            {
                if (SymbolEqualityComparer.Default.Equals(current, expected))
                {
                    return true;
                }
            }

            return false;
        }

        private static ControlFlowRegion? FindRegion(ControlFlowRegion region, ControlFlowRegionKind kind)
        {
            for (var current = region; current is not null; current = current.EnclosingRegion)
            {
                if (current.Kind == kind)
                {
                    return current;
                }
            }

            return null;
        }

        private static bool ReferencesStorage(IOperation? operation, Storage storage) => operation switch
        {
            IParameterReferenceOperation reference => storage.Aliases.Contains(reference.Parameter),
            ILocalReferenceOperation reference => storage.Aliases.Contains(reference.Local),
            IFlowCaptureReferenceOperation reference => storage.Captures.Contains(reference.Id),
            IConversionOperation conversion => ReferencesStorage(conversion.Operand, storage),
            IParenthesizedOperation parentheses => ReferencesStorage(parentheses.Operand, storage),
            ITupleOperation tuple => tuple.Elements.Any(element => ReferencesStorage(element, storage)),
            ISimpleAssignmentOperation { IsRef: true } assignment => ReferencesStorage(assignment.Value, storage),
            _ => false,
        };

        private void Visit(IOperation operation, Storage storage)
        {
            if (operation is IFlowAnonymousFunctionOperation anonymous)
            {
                AnonymousFunctions.Add(anonymous);
                ReferencedCallables.Add(anonymous.Symbol);
                return;
            }

            VisitEarlyCollectionCalls(operation, storage);

            if (operation is IInvocationOperation invocation)
            {
                ReferencedCallables.Add(invocation.TargetMethod.OriginalDefinition);
            }
            else if (operation is IMethodReferenceOperation reference)
            {
                ReferencedCallables.Add(reference.Method.OriginalDefinition);
            }

            VisitStorageEffects(operation, storage);

            if (operation.Syntax.Parent is ArgumentSyntax argumentSyntax
                && argumentSyntax.RefKindKeyword.Kind() is SyntaxKind.RefKeyword or SyntaxKind.OutKeyword)
            {
                _written |= ReferencesStorage(operation, storage);
            }

            if (RequiresExceptionSearch(operation))
            {
                SearchException(_block.EnclosingRegion, storage, null, _resume);
            }
        }

        private bool RequiresExceptionSearch(IOperation operation) =>
            ((CanThrow(operation) && !IsDeferredArrayStore(operation)) || HiddenOperationCanThrow(operation) || ClassifiedOperationCanThrow(operation))
            && !IsExhaustiveSwitchFailure(operation);

        private bool IsDeferredArrayStore(IOperation operation)
        {
            if (operation is not IArrayElementReferenceOperation)
            {
                return false;
            }

            // Source parents preserve the store target when CFG hoists branching values.
            // Its children still contribute early exceptions. Index/Range operands may
            // additionally read the array length before the value is evaluated.
            return (_model?.GetOperation(operation.Syntax) ?? operation) is IArrayElementReferenceOperation { Parent: ISimpleAssignmentOperation assignment } array
                && ReferenceEquals(assignment.Target, array)
                && array.Indices.All(index => index.Type?.SpecialType == SpecialType.System_Int32);
        }

        private void VisitEarlyCollectionCalls(IOperation operation, Storage storage)
        {
            // Constructors, Add and spread iteration may throw before later elements
            // rebind aliases. Source spans also cover elements hoisted into CFG blocks.
            foreach (var span in _earlyCollectionCalls)
            {
                if (span.Contains(operation.Syntax.Span))
                {
                    SearchException(_block.EnclosingRegion, storage, null, _resume);
                    break;
                }
            }
        }

        private void VisitStorageEffects(IOperation operation, Storage storage)
        {
            if (operation is ISimpleAssignmentOperation assignment)
            {
                VisitAssignmentStorageEffects(assignment, storage);
            }
            else if (operation is IDeconstructionAssignmentOperation deconstruction)
            {
                var writes = VisitDeconstructionTarget(deconstruction.Target, storage);
                Visit(deconstruction.Value, storage);
                _written |= writes;
            }
            else
            {
                foreach (var child in operation.ChildOperations)
                {
                    Visit(child, storage);
                }

                RecordStorageWrite(operation, storage);
            }
        }

        private void VisitAssignmentStorageEffects(ISimpleAssignmentOperation assignment, Storage storage)
        {
            Visit(assignment.Target, storage);
            var writes = ReferencesStorage(assignment.Target, storage);
            Visit(assignment.Value, storage);
            if (assignment.IsRef && GetLocal(assignment.Target) is { } alias)
            {
                SetAlias(storage.Aliases, alias, ReferencesStorage(assignment.Value, storage));
            }
            else
            {
                // Transfers into ref fields and other untracked destinations escape
                // the local alias model; subsequent writes can overwrite the token.
                var initializesSavedLocal = trackedSymbol is ILocalSymbol
                    && assignment.Target is ILocalReferenceOperation { IsDeclaration: true } declaration
                    && SymbolEqualityComparer.Default.Equals(declaration.Local, trackedSymbol);
                _written |= (writes && !initializesSavedLocal && !PreservesIncomingToken(assignment.Value))
                    || (assignment.IsRef && ReferencesStorage(assignment.Value, storage));
            }
        }

        private readonly Dictionary<ILocalSymbol, bool> _savedCopies = [with(SymbolEqualityComparer.Default)];

        private bool PreservesIncomingToken(IOperation value)
        {
            // Saved-copy checks must reject every subsequent write, even another copy
            // of the same value. They prove immutability, not parameter provenance.
            if (trackedSymbol is not IParameterSymbol)
            {
                return false;
            }

            return value switch
            {
                IConversionOperation { Conversion.IsIdentity: true } conversion => PreservesIncomingToken(conversion.Operand),
                IParenthesizedOperation parentheses => PreservesIncomingToken(parentheses.Operand),
                IParameterReferenceOperation reference => SymbolEqualityComparer.Default.Equals(reference.Parameter, trackedSymbol),
                ILocalReferenceOperation reference => IsUnchangedSavedCopy(reference.Local),
                _ => false,
            };
        }

        private bool IsUnchangedSavedCopy(ILocalSymbol local)
        {
            if (_savedCopies.TryGetValue(local, out var saved))
            {
                return saved;
            }

            // Seed false to handle incomplete editor code with cyclic initializers.
            _savedCopies.Add(local, false);
            if (local.RefKind != RefKind.None
                || local.DeclaringSyntaxReferences.SingleOrDefault()?.GetSyntax() is not VariableDeclaratorSyntax { Initializer.Value: { } initializer }
                || _model?.GetOperation(initializer) is not { } value
                || !PreservesIncomingToken(value))
            {
                return false;
            }

            // A restoration may be inside a callback while the saved copy is
            // overwritten in its enclosing method. Check the complete owner graph.
            var owner = graph;
            while (owner.Parent is { } parent)
            {
                owner = parent;
            }

            var unchanged = !IsWritten(owner, local, [with(SymbolEqualityComparer.Default)]);
            _savedCopies[local] = unchanged;
            return unchanged;
        }

        private bool VisitDeconstructionTarget(IOperation target, Storage storage)
        {
            if (target is ITupleOperation tuple)
            {
                var writes = false;
                foreach (var element in tuple.Elements)
                {
                    // Capture each destination before later destinations can rebind its alias.
                    writes |= VisitDeconstructionTarget(element, storage);
                }

                return writes;
            }

            Visit(target, storage);
            return ReferencesStorage(target, storage);
        }

        private void RecordStorageWrite(IOperation operation, Storage storage)
        {
            switch (operation)
            {
                case IFlowCaptureOperation capture:
                    SetAlias(storage.Captures, capture.Id, ReferencesStorage(capture.Value, storage));
                    break;
                case IArgumentOperation { Parameter.RefKind: RefKind.Ref or RefKind.Out } argument:
                    _written |= ReferencesStorage(argument.Value, storage);
                    break;
                case ICompoundAssignmentOperation compound:
                    _written |= ReferencesStorage(compound.Target, storage);
                    break;
                case IIncrementOrDecrementOperation increment:
                    _written |= ReferencesStorage(increment.Target, storage);
                    break;
            }
        }

        // Only the synthesized failure is unreachable; conversions around the switch can still throw.
        private bool IsExhaustiveSwitchFailure(IOperation? operation) =>
            operation is IObjectCreationOperation { IsImplicit: true, Syntax: SwitchExpressionSyntax expression }
            && _model?.GetOperation(expression) is ISwitchExpressionOperation { IsExhaustive: true };

        private bool CatchesAll(ITypeSymbol? caughtType) => caughtType is null
            || caughtType.SpecialType == SpecialType.System_Object
            || SymbolEqualityComparer.Default.Equals(caughtType, _exceptionType);

        private ILocalSymbol? GetLocal(IOperation target) => target switch
        {
            ILocalReferenceOperation local => local.Local,
            _ => _model?.GetSymbolInfo(target.Syntax).Symbol as ILocalSymbol,
        };

        private readonly Dictionary<IOperation, bool> _hiddenThrows = [];

        private bool ClassifiedOperationCanThrow(IOperation operation)
        {
            if (operation is IDeconstructionAssignmentOperation deconstruction)
            {
                return DeconstructionStorageCanThrow(deconstruction.Target);
            }

            if (!RequiresExceptionClassification(operation))
            {
                return false;
            }

            // CFG hoists store targets before branching values. Source parents retain
            // the distinction between a member read and a store after value evaluation.
            var classified = operation is IFieldReferenceOperation or IEventReferenceOperation or IPropertyReferenceOperation or IDynamicMemberReferenceOperation or IDynamicIndexerAccessOperation or ISimpleAssignmentOperation ? _model?.GetOperation(operation.Syntax) ?? operation : operation;
            return !(classified is IFieldReferenceOperation or IEventReferenceOperation && IsDeconstructionTarget(classified))
                && !_exceptionClassifier.Classify(classified).IsEmpty;
        }

        private static bool RequiresExceptionClassification(IOperation operation) => operation is
            ICollectionExpressionOperation or ISpreadOperation
            or IEventAssignmentOperation or IEventReferenceOperation or IFieldReferenceOperation or IMethodReferenceOperation
            or ITypeParameterObjectCreationOperation or IFunctionPointerInvocationOperation or IImplicitIndexerReferenceOperation
            or IPropertyReferenceOperation or IDynamicMemberReferenceOperation or IDynamicIndexerAccessOperation
            or ISimpleAssignmentOperation
            {
                Target: IFieldReferenceOperation or IEventReferenceOperation or IFlowCaptureReferenceOperation
                or IPropertyReferenceOperation or IImplicitIndexerReferenceOperation or IDynamicMemberReferenceOperation or IDynamicIndexerAccessOperation
            }
            or IBinaryOperation or IUnaryOperation or ICompoundAssignmentOperation or IIncrementOrDecrementOperation;

        private static bool IsDeconstructionTarget(IOperation operation)
        {
            while (operation.Parent is ITupleOperation or IConversionOperation or IParenthesizedOperation)
            {
                operation = operation.Parent;
            }

            return operation.Parent is IDeconstructionAssignmentOperation assignment
                && ReferenceEquals(assignment.Target, operation);
        }

        private bool DeconstructionStorageCanThrow(IOperation target) => target switch
        {
            ITupleOperation tuple => tuple.Elements.Any(DeconstructionStorageCanThrow),
            IConversionOperation conversion => DeconstructionStorageCanThrow(conversion.Operand),
            IParenthesizedOperation parentheses => DeconstructionStorageCanThrow(parentheses.Operand),
            // Classify the destination only after the assigned value has run. Its source
            // parent is a tuple, so the classifier recognizes the backing-field access.
            IFieldReferenceOperation or IEventReferenceOperation => !_exceptionClassifier.Classify(target).IsEmpty,
            _ => false,
        };

        private bool HiddenOperationCanThrow(IOperation operation)
        {
            if (_model is null || operation is not (ITupleBinaryOperation or IConversionOperation or IDeconstructionAssignmentOperation or IIsPatternOperation))
            {
                return false;
            }

            if (!_hiddenThrows.TryGetValue(operation, out var result))
            {
                result = ClassifyHiddenOperation(operation);
                _hiddenThrows.Add(operation, result);
            }

            return result;
        }

        private bool ClassifyHiddenOperation(IOperation operation) => operation switch
        {
            ITupleBinaryOperation tuple => TupleOperatorCanThrow(tuple.LeftOperand.Type, tuple.RightOperand.Type, tuple),
            IConversionOperation conversion => ScalarConversionCanThrow(conversion)
                || TupleConversionCanThrow(conversion.Operand.Type, conversion.Type,
                _exceptionClassifier.IsInCheckedContext(conversion)),
            IDeconstructionAssignmentOperation { Syntax: AssignmentExpressionSyntax assignment } =>
                DeconstructionCanThrow(_model!.GetDeconstructionInfo(assignment)),
            IIsPatternOperation pattern => pattern.Pattern.DescendantsAndSelf().Any(PatternCanThrow),
            _ => false,
        };

        private bool ScalarConversionCanThrow(IConversionOperation conversion) =>
            OperationExceptionClassifier.CanNumericConversionOverflow(conversion.Operand.Type, conversion.Type)
            || (conversion.Operand.Type is { } source && conversion.Type is { } destination
                && (UnwrapNullable(source)?.TypeKind == TypeKind.Enum || UnwrapNullable(destination)?.TypeKind == TypeKind.Enum)
                && CheckedConversionCanThrow(source, destination, _exceptionClassifier.IsInCheckedContext(conversion)));

        private static bool PatternCanThrow(IOperation operation) => operation is
            IRecursivePatternOperation { DeconstructSymbol: not null }
            or IListPatternOperation { InputType: not IArrayTypeSymbol and not { SpecialType: SpecialType.System_String } };

        private bool TupleOperatorCanThrow(ITypeSymbol? left, ITypeSymbol? right, ITupleBinaryOperation operation)
        {
            left = UnwrapNullable(left);
            right = UnwrapNullable(right);
            if (left is INamedTypeSymbol { IsTupleType: true } leftTuple
                && right is INamedTypeSymbol { IsTupleType: true } rightTuple)
            {
                return leftTuple.TupleElements.Zip(rightTuple.TupleElements,
                    (first, second) => TupleOperatorCanThrow(first.Type, second.Type, operation)).Any(static result => result);
            }

            if (left is null || right is null || _model is null)
            {
                return false;
            }

            if (left.TypeKind == TypeKind.Dynamic || right.TypeKind == TypeKind.Dynamic)
            {
                return true;
            }

            return SelectedTupleOperatorCanThrow(left, right, operation);
        }

        private bool SelectedTupleOperatorCanThrow(ITypeSymbol left, ITypeSymbol right, ITupleBinaryOperation operation)
        {
            // Bind precisely the element operator selected for these operand types.
            // Declared, inapplicable equality overloads do not make reference equality throw.
            var expression = SyntaxFactory.BinaryExpression(
                operation.OperatorKind == BinaryOperatorKind.Equals ? SyntaxKind.EqualsExpression : SyntaxKind.NotEqualsExpression,
                SyntaxFactory.DefaultExpression(SyntaxFactory.ParseTypeName(left.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat))),
                SyntaxFactory.DefaultExpression(SyntaxFactory.ParseTypeName(right.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat))));
            var symbol = _model!.GetSpeculativeSymbolInfo(operation.Syntax.SpanStart, expression, SpeculativeBindingOption.BindAsExpression).Symbol as IMethodSymbol;
            return symbol?.MethodKind == MethodKind.UserDefinedOperator
                && symbol.ContainingType.SpecialType is not (SpecialType.System_String or SpecialType.System_Decimal);
        }

        private bool TupleConversionCanThrow(ITypeSymbol? source, ITypeSymbol? destination, bool checkedContext)
        {
            if (source is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T }
                && destination is { IsValueType: true, OriginalDefinition.SpecialType: not SpecialType.System_Nullable_T })
            {
                return true;
            }

            source = UnwrapNullable(source);
            destination = UnwrapNullable(destination);
            if (source is not INamedTypeSymbol { IsTupleType: true } sourceTuple
                || destination is not INamedTypeSymbol { IsTupleType: true } targetTuple || _model is null)
            {
                return false;
            }

            return sourceTuple.TupleElements.Zip(targetTuple.TupleElements,
                    (from, to) => TupleElementConversionCanThrow(from, to, checkedContext))
                .Any(static result => result);
        }

        private bool TupleElementConversionCanThrow(IFieldSymbol source, IFieldSymbol destination, bool checkedContext)
        {
            var conversion = _model!.Compilation.ClassifyConversion(source.Type, destination.Type);
            return conversion.IsDynamic || conversion.IsUserDefined || conversion.IsUnboxing
                || (conversion.IsReference && !conversion.IsImplicit)
                || OperationExceptionClassifier.CanNumericConversionOverflow(source.Type, destination.Type)
                || CheckedConversionCanThrow(source.Type, destination.Type, checkedContext)
                || TupleConversionCanThrow(source.Type, destination.Type, checkedContext);
        }

        private bool CheckedConversionCanThrow(ITypeSymbol source, ITypeSymbol destination, bool checkedContext)
        {
            if (!checkedContext)
            {
                return false;
            }

            source = UnwrapNullable(source)!;
            destination = UnwrapNullable(destination)!;
            if (source is INamedTypeSymbol { EnumUnderlyingType: { } sourceUnderlying })
            {
                source = sourceUnderlying;
            }

            if (destination is INamedTypeSymbol { EnumUnderlyingType: { } targetUnderlying })
            {
                destination = targetUnderlying;
            }

            var conversion = _model!.Compilation.ClassifyConversion(source, destination);
            return conversion.IsNumeric && !conversion.IsImplicit
                && destination.SpecialType is not (SpecialType.System_Single or SpecialType.System_Double)
                && !(destination.SpecialType == SpecialType.System_Char
                    && source.SpecialType is SpecialType.System_Byte or SpecialType.System_UInt16);
        }

        private static ITypeSymbol? UnwrapNullable(ITypeSymbol? type) =>
            type is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } nullable
                ? nullable.TypeArguments[0]
                : type;

        private static bool DeconstructionCanThrow(DeconstructionInfo info) =>
            info.Method is not null || info.Conversion is { IsUserDefined: true }
                || (!info.Nested.IsDefaultOrEmpty && info.Nested.Any(DeconstructionCanThrow));

        private static bool? ConstantCondition(IOperation? operation)
        {
            if (operation?.ConstantValue is { HasValue: true, Value: bool value })
            {
                return value;
            }

            return operation is IIsPatternOperation pattern ? PatternMatchesAll(pattern.Pattern) : null;
        }

        private static bool? PatternMatchesAll(IPatternOperation pattern) => pattern switch
        {
            IDiscardPatternOperation => true,
            IDeclarationPatternOperation { MatchesNull: true } => true,
            IDeclarationPatternOperation declaration when declaration.InputType.IsValueType
                && SymbolEqualityComparer.Default.Equals(declaration.InputType, declaration.MatchedType) => true,
            _ => null,
        };

        private static void SetAlias<T>(HashSet<T> set, T key, bool contains)
        {
            if (contains)
            {
                set.Add(key);
            }
            else
            {
                set.Remove(key);
            }
        }
    }

    private static bool CanThrow(IOperation operation) => operation is
        IAwaitOperation or IInvocationOperation or IObjectCreationOperation or IArrayCreationOperation
        or IArrayElementReferenceOperation
        or IDynamicInvocationOperation or IDynamicObjectCreationOperation
        or IConversionOperation { OperatorMethod: not null }
        or IConversionOperation { IsChecked: true, Conversion.IsImplicit: false }
        or IConversionOperation { Conversion.IsReference: true, Conversion.IsImplicit: false }
        or IConversionOperation { Operand.Type.TypeKind: TypeKind.Dynamic }
        or IConversionOperation { Operand.Type.IsReferenceType: true, Type.IsValueType: true }
        or IConversionOperation { Operand.Type.OriginalDefinition.SpecialType: SpecialType.System_Nullable_T, Type.OriginalDefinition.SpecialType: not SpecialType.System_Nullable_T }
        or IIncrementOrDecrementOperation { IsChecked: true }
        or IIncrementOrDecrementOperation { OperatorMethod: not null }
        or IIncrementOrDecrementOperation { Type.TypeKind: TypeKind.Dynamic }
        or IInterpolationOperation or IInterpolatedStringOperation
        or ICompoundAssignmentOperation { OperatorMethod: not null }
        or ICompoundAssignmentOperation { Target.Type.TypeKind: TypeKind.Dynamic }
        or ICompoundAssignmentOperation { Value.Type.TypeKind: TypeKind.Dynamic }
        or IBinaryOperation { OperatorMethod: not null }
        or IBinaryOperation { Type.TypeKind: TypeKind.Dynamic }
        or IUnaryOperation { OperatorMethod: not null }
        or IUnaryOperation { IsChecked: true }
        or IUnaryOperation { Type.TypeKind: TypeKind.Dynamic }
        or IIsPatternOperation { Pattern: IRecursivePatternOperation { DeconstructSymbol: not null } }
        || (operation is IConversionOperation conversion && conversion.GetConversion().IsUnboxing)
        || (operation is IAssignmentOperation assignment && TargetCanThrow(assignment.Target))
        || ArithmeticCanThrow(operation);

    private static bool TargetCanThrow(IOperation target) => target switch
    {
        ITupleOperation tuple => tuple.Elements.Any(TargetCanThrow),
        IConversionOperation conversion => TargetCanThrow(conversion.Operand),
        IPropertyReferenceOperation property => !property.Property.ReturnsByRef && !property.Property.ReturnsByRefReadonly,
        IImplicitIndexerReferenceOperation { IndexerSymbol: IPropertySymbol property } => !property.ReturnsByRef && !property.ReturnsByRefReadonly,
        IImplicitIndexerReferenceOperation or IDynamicMemberReferenceOperation or IDynamicIndexerAccessOperation or IArrayElementReferenceOperation => true,
        _ => false,
    };

    private static bool ArithmeticCanThrow(IOperation operation)
    {
        var type = operation.Type;
        if (type is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } nullable)
        {
            type = nullable.TypeArguments[0];
        }

        return operation is ICompoundAssignmentOperation { IsChecked: true }
                or ICompoundAssignmentOperation { OperatorKind: BinaryOperatorKind.Divide or BinaryOperatorKind.Remainder }
                or IBinaryOperation { IsChecked: true }
                or IBinaryOperation { OperatorKind: BinaryOperatorKind.Divide or BinaryOperatorKind.Remainder }
            && type?.SpecialType is not (SpecialType.System_Single or SpecialType.System_Double);
    }
}
