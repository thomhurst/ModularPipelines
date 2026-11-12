using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.Text;
using ModularPipelines.Analyzers.Extensions;

namespace ModularPipelines.Analyzers;

internal static class ModuleAuthoringAnalysis
{
    private const string AssemblyMetadataName = "System.Reflection.Assembly";
    private const string CancellationTokenMetadataName = "System.Threading.CancellationToken";

    private static readonly ImmutableHashSet<string> EagerLinqTerminalNames =
    [
        "Aggregate",
        "All",
        "Any",
        "Average",
        "Contains",
        "Count",
        "ElementAt",
        "ElementAtOrDefault",
        "First",
        "FirstOrDefault",
        "Last",
        "LastOrDefault",
        "LongCount",
        "Max",
        "MaxBy",
        "Min",
        "MinBy",
        "SequenceEqual",
        "Single",
        "SingleOrDefault",
        "Sum",
        "ToArray",
        "ToDictionary",
        "ToHashSet",
        "ToList",
        "ToLookup",
    ];

    private static readonly ConditionalWeakTable<
        Compilation,
        ConcurrentDictionary<INamedTypeSymbol, ImmutableHashSet<IMethodSymbol>>>
        ReachableMemberMethods = new();

    private static readonly ConditionalWeakTable<
        Compilation,
        Lazy<ImmutableArray<IMethodSymbol>>>
        ModuleExecuteMethods = new();

    private static readonly ConditionalWeakTable<
        Compilation,
        Lazy<ImmutableDictionary<IMethodSymbol, ImmutableHashSet<IMethodSymbol>>>>
        AdditionalCancellationRoots = new();

    public static void InitializeRegistrationAnalysis(AnalysisContext context)
    {
        context.RegisterCompilationStartAction(StartRegistrationAnalysis);
    }

    public static void InitializeAsyncSafetyAnalysis(AnalysisContext context)
    {
        context.RegisterCompilationStartAction(StartAsyncVoidAnalysis);
        context.RegisterOperationAction(AnalyzeInvocation, OperationKind.Invocation);
        context.RegisterOperationAction(AnalyzePropertyReference, OperationKind.PropertyReference);
        context.RegisterOperationBlockStartAction(blockContext =>
        {
            // Analyze the whole block so findings reached through a returned local
            // function remain local semantic diagnostics in Roslyn. Independent
            // await/return traversals share reporting state for this block.
            var reportedInvocations = new ConcurrentDictionary<(SyntaxTree Tree, TextSpan Span), byte>();
            blockContext.RegisterOperationAction(
                operationContext => AnalyzeAwait(operationContext, reportedInvocations), OperationKind.Await);
            blockContext.RegisterOperationAction(
                operationContext => AnalyzeForEachLoop(operationContext, reportedInvocations), OperationKind.Loop);
            blockContext.RegisterOperationAction(
                operationContext => AnalyzeReturn(operationContext, reportedInvocations), OperationKind.Return);
        });
    }

    public static void InitializeDuplicateDependencyAnalysis(AnalysisContext context)
    {
        context.RegisterSymbolAction(AnalyzeDuplicateDependencies, SymbolKind.NamedType);
    }

    private static void StartRegistrationAnalysis(CompilationStartAnalysisContext context)
    {
        var modules = new ConcurrentBag<INamedTypeSymbol>();
        var registeredModules = new ConcurrentBag<INamedTypeSymbol>();
        var instanceRegisteredModules = new ConcurrentBag<INamedTypeSymbol>();
        var scannedAssemblies = new ConcurrentBag<IAssemblySymbol>();
        var unresolvedModuleRegistrations = new ConcurrentBag<byte>();
        var unresolvedFactoryRegistrations = new ConcurrentBag<byte>();
        var registrationInvocations =
            new ConcurrentBag<(IInvocationOperation Invocation, IMethodSymbol? ContainingMethod)>();
        var indexerAssignments =
            new ConcurrentBag<(ISimpleAssignmentOperation Assignment, IMethodSymbol? ContainingMethod)>();
        var methodCalls =
            new ConcurrentBag<(IMethodSymbol Caller, IMethodSymbol Callee)>();

        context.RegisterSymbolAction(
            symbolContext => CollectModuleType(symbolContext, modules),
            SymbolKind.NamedType);
        context.RegisterOperationAction(
            operationContext => CollectRegistrationInvocation(
                operationContext,
                registrationInvocations,
                methodCalls),
            OperationKind.Invocation);
        context.RegisterOperationAction(
            operationContext => CollectConstructorCall(
                operationContext,
                methodCalls),
            OperationKind.ObjectCreation);
        context.RegisterOperationAction(
            operationContext => CollectPropertyAccessorCall(
                operationContext,
                methodCalls),
            OperationKind.PropertyReference);
        context.RegisterOperationAction(
            operationContext => CollectEventAccessorCall(
                operationContext,
                methodCalls),
            OperationKind.EventAssignment);
        context.RegisterOperationAction(
            operationContext => CollectStaticFieldInitializationCall(
                operationContext,
                methodCalls),
            OperationKind.FieldReference);
        context.RegisterOperationAction(
            operationContext => CollectServiceCollectionIndexerAssignment(
                operationContext,
                indexerAssignments),
            OperationKind.SimpleAssignment);
        context.RegisterCompilationEndAction(endContext =>
        {
            TrackReachableRegistrations(
                endContext,
                registrationInvocations,
                indexerAssignments,
                methodCalls,
                registeredModules,
                instanceRegisteredModules,
                scannedAssemblies,
                unresolvedModuleRegistrations,
                unresolvedFactoryRegistrations);
            ReportModuleDiagnostics(
                endContext,
                modules,
                registeredModules,
                instanceRegisteredModules,
                scannedAssemblies,
                unresolvedModuleRegistrations,
                unresolvedFactoryRegistrations);
        });
    }

    private static void CollectModuleType(
        SymbolAnalysisContext context,
        ConcurrentBag<INamedTypeSymbol> modules)
    {
        var type = (INamedTypeSymbol) context.Symbol;
        if (type.IsAbstract || !type.IsModule(context.Compilation))
        {
            return;
        }

        modules.Add(type);
    }

    private static void StartAsyncVoidAnalysis(
        CompilationStartAnalysisContext context)
    {
        var asyncVoidMethods = new ConcurrentBag<IMethodSymbol>();
        var eventHandlerMethods = new ConcurrentBag<IMethodSymbol>();

        context.RegisterSymbolAction(
            symbolContext => CollectAsyncVoidMethod(symbolContext, asyncVoidMethods),
            SymbolKind.Method);
        context.RegisterOperationAction(
            operationContext => CollectAsyncVoidAnonymousFunction(
                operationContext,
                asyncVoidMethods),
            OperationKind.AnonymousFunction);
        context.RegisterOperationAction(
            operationContext => CollectEventHandlerMethod(
                operationContext,
                eventHandlerMethods),
            OperationKind.EventAssignment);
        context.RegisterCompilationEndAction(endContext =>
            ReportAsyncVoidDiagnostics(
                endContext,
                asyncVoidMethods,
                eventHandlerMethods));
    }

    private static void CollectAsyncVoidMethod(
        SymbolAnalysisContext context,
        ConcurrentBag<IMethodSymbol> asyncVoidMethods)
    {
        var method = (IMethodSymbol) context.Symbol;
        if (method.IsAsync
            && method.ReturnsVoid
            && method.ContainingType.IsModule(context.Compilation))
        {
            asyncVoidMethods.Add(method);
        }
    }

    private static void CollectAsyncVoidAnonymousFunction(
        OperationAnalysisContext context,
        ConcurrentBag<IMethodSymbol> asyncVoidMethods)
    {
        var method = ((IAnonymousFunctionOperation) context.Operation).Symbol;
        if (method.IsAsync
            && method.ReturnsVoid
            && method.ContainingType.IsModule(context.Compilation))
        {
            asyncVoidMethods.Add(method);
        }
    }

    private static void CollectEventHandlerMethod(
        OperationAnalysisContext context,
        ConcurrentBag<IMethodSymbol> eventHandlerMethods)
    {
        var eventAssignment = (IEventAssignmentOperation) context.Operation;
        if (eventAssignment.Adds
            && GetDirectDelegateTarget(eventAssignment.HandlerValue) is { } method)
        {
            eventHandlerMethods.Add(method);
        }
    }

    private static IMethodSymbol? GetDirectDelegateTarget(IOperation operation)
    {
        return operation switch
        {
            IConversionOperation conversion => GetDirectDelegateTarget(conversion.Operand),
            IDelegateCreationOperation { Target: { } target } => GetDirectDelegateTarget(target),
            IMethodReferenceOperation methodReference => methodReference.Method,
            IAnonymousFunctionOperation anonymousFunction => anonymousFunction.Symbol,
            _ => null,
        };
    }

    private static void ReportAsyncVoidDiagnostics(
        CompilationAnalysisContext context,
        ConcurrentBag<IMethodSymbol> asyncVoidMethods,
        ConcurrentBag<IMethodSymbol> eventHandlerMethods)
    {
        foreach (var method in asyncVoidMethods)
        {
            if (eventHandlerMethods.Any(eventHandler =>
                    SymbolEqualityComparer.Default.Equals(eventHandler, method))
                || method.Locations.FirstOrDefault(static item => item.IsInSource)
                    is not { } location)
            {
                continue;
            }

            context.ReportDiagnostic(Diagnostic.Create(
                ModuleAsyncSafetyAnalyzer.AsyncVoidRule,
                location,
                method.MethodKind == MethodKind.AnonymousFunction
                    ? "anonymous function"
                    : method.Name));
        }
    }

    private static void CollectRegistrationInvocation(
        OperationAnalysisContext context,
        ConcurrentBag<(IInvocationOperation Invocation, IMethodSymbol? ContainingMethod)> registrations,
        ConcurrentBag<(IMethodSymbol Caller, IMethodSymbol Callee)> methodCalls)
    {
        var invocation = (IInvocationOperation) context.Operation;
        if (!IsInReachableBranch(invocation))
        {
            return;
        }

        var containingMethods = GetContainingExecutionMethods(invocation, context.ContainingSymbol);
        foreach (var containingMethod in containingMethods.OfType<IMethodSymbol>())
        {
            methodCalls.Add((
                NormalizeMethod(containingMethod),
                NormalizeMethod(invocation.TargetMethod)));
            CollectInvokedCallbackCalls(
                invocation,
                containingMethod,
                context.Compilation,
                methodCalls);
        }

        if (IsPotentialRegistrationInvocation(invocation))
        {
            foreach (var containingMethod in containingMethods)
            {
                registrations.Add((invocation, containingMethod));
            }
        }
    }

    private static void CollectConstructorCall(
        OperationAnalysisContext context,
        ConcurrentBag<(IMethodSymbol Caller, IMethodSymbol Callee)> methodCalls)
    {
        var objectCreation = (IObjectCreationOperation) context.Operation;
        if (objectCreation.Constructor is null
            || !IsInReachableBranch(objectCreation))
        {
            return;
        }

        foreach (var containingMethod in
                 GetContainingExecutionMethods(objectCreation, context.ContainingSymbol)
                     .OfType<IMethodSymbol>())
        {
            methodCalls.Add((
                NormalizeMethod(containingMethod),
                NormalizeMethod(objectCreation.Constructor)));
        }
    }

    private static void CollectPropertyAccessorCall(
        OperationAnalysisContext context,
        ConcurrentBag<(IMethodSymbol Caller, IMethodSymbol Callee)> methodCalls)
    {
        var propertyReference = (IPropertyReferenceOperation) context.Operation;
        if (!IsInReachableBranch(propertyReference))
        {
            return;
        }

        foreach (var containingMethod in
                 GetContainingExecutionMethods(propertyReference, context.ContainingSymbol)
                     .OfType<IMethodSymbol>())
        {
            var caller = NormalizeMethod(containingMethod);
            var isAssignmentTarget = IsPropertyAssignmentTarget(propertyReference);
            if ((!isAssignmentTarget
                 || propertyReference.Parent is not ISimpleAssignmentOperation)
                && propertyReference.Property.GetMethod is { } getter)
            {
                methodCalls.Add((caller, NormalizeMethod(getter)));
            }

            if (isAssignmentTarget
                && propertyReference.Property.SetMethod is { } setter)
            {
                methodCalls.Add((caller, NormalizeMethod(setter)));
            }
        }
    }

    private static void CollectEventAccessorCall(
        OperationAnalysisContext context,
        ConcurrentBag<(IMethodSymbol Caller, IMethodSymbol Callee)> methodCalls)
    {
        var eventAssignment = (IEventAssignmentOperation) context.Operation;
        if (!IsInReachableBranch(eventAssignment))
        {
            return;
        }

        if (GetEventAccessor(eventAssignment) is not { } accessor)
        {
            return;
        }

        foreach (var containingMethod in
                 GetContainingExecutionMethods(eventAssignment, context.ContainingSymbol)
                     .OfType<IMethodSymbol>())
        {
            methodCalls.Add((
                NormalizeMethod(containingMethod),
                NormalizeMethod(accessor)));
        }
    }

    private static void CollectInvokedCallbackCalls(
        IInvocationOperation invocation,
        IMethodSymbol containingMethod,
        Compilation compilation,
        ConcurrentBag<(IMethodSymbol Caller, IMethodSymbol Callee)> methodCalls)
    {
        if (invocation.TargetMethod.MethodKind == MethodKind.DelegateInvoke
            && GetInvocationReceiver(invocation) is { } receiver)
        {
            CollectCallbackCalls(receiver, containingMethod, compilation, methodCalls);
        }

        var targetMethod = NormalizeMethod(invocation.TargetMethod);
        foreach (var argument in invocation.Arguments)
        {
            var targetParameter = GetTargetParameter(targetMethod, argument.Parameter);
            if (targetParameter is null
                || (!IsKnownDelegateInvoker(invocation)
                    && !SourceMethodInvokesDelegateParameter(
                        targetMethod,
                        targetParameter,
                        compilation)))
            {
                continue;
            }

            CollectCallbackCalls(argument.Value, containingMethod, compilation, methodCalls);
        }
    }

    private static void CollectCallbackCalls(
        IOperation value,
        IMethodSymbol containingMethod,
        Compilation compilation,
        ConcurrentBag<(IMethodSymbol Caller, IMethodSymbol Callee)> methodCalls)
    {
        foreach (var callback in GetValueAndReachingCallbackValues(
                         value,
                         compilation,
                         [with(SymbolEqualityComparer.Default)],
                         [with(SymbolEqualityComparer.Default)],
                         [with(SymbolEqualityComparer.Default)])
                     .SelectMany(static candidate => candidate.DescendantsAndSelf())
                     .Select(static operation => operation switch
                     {
                         IMethodReferenceOperation methodReference => methodReference.Method,
                         IAnonymousFunctionOperation anonymousFunction => anonymousFunction.Symbol,
                         _ => null,
                     })
                     .OfType<IMethodSymbol>())
        {
            methodCalls.Add((
                NormalizeMethod(containingMethod),
                NormalizeMethod(callback)));
        }
    }

    private static void AnalyzeInvocation(OperationAnalysisContext context)
    {
        var invocation = (IInvocationOperation) context.Operation;

        if (GetModuleExecutionMethod(context) is null
            || !IsInReachableBranch(invocation))
        {
            return;
        }

        var method = invocation.TargetMethod;
        if (method.Name == "Sleep"
            && method.ContainingType.ToDisplayString() == "System.Threading.Thread")
        {
            context.ReportDiagnostic(Diagnostic.Create(
                ModuleAsyncSafetyAnalyzer.ThreadSleepRule,
                invocation.Syntax.GetLocation()));
            return;
        }

        if ((method.Name is "Wait" or "WaitAll" or "WaitAny"
             && method.ContainingType.InheritsFrom(
                 context.Compilation.GetTypeByMetadataName("System.Threading.Tasks.Task")))
            || (method.Name == "GetResult" && IsAwaiterGetResult(invocation)))
        {
            context.ReportDiagnostic(Diagnostic.Create(
                ModuleAsyncSafetyAnalyzer.BlockingCallRule,
                invocation.Syntax.GetLocation(),
                method.Name));
        }
    }

    private static void AnalyzePropertyReference(OperationAnalysisContext context)
    {
        var propertyReference = (IPropertyReferenceOperation) context.Operation;
        if (GetModuleExecutionMethod(context) is null
            || !IsInReachableBranch(propertyReference)
            || propertyReference.Property.Name != "Result"
            || !IsBlockingResultType(
                propertyReference.Property.ContainingType,
                context.Compilation))
        {
            return;
        }

        if (TaskCompletionAnalysis.IsCompleted(propertyReference, context.CancellationToken))
        {
            return;
        }

        context.ReportDiagnostic(Diagnostic.Create(
            ModuleAsyncSafetyAnalyzer.BlockingCallRule,
            propertyReference.Syntax.GetLocation(),
            propertyReference.Property.Name));
    }

    private static bool IsBlockingResultType(
        INamedTypeSymbol type,
        Compilation compilation)
    {
        return type.InheritsFrom(
                   compilation.GetTypeByMetadataName("System.Threading.Tasks.Task"))
               || type.OriginalDefinition.ToDisplayString()
               == "System.Threading.Tasks.ValueTask<TResult>";
    }

    private static void AnalyzeAwait(
        OperationAnalysisContext context,
        ConcurrentDictionary<(SyntaxTree Tree, TextSpan Span), byte> reportedInvocations)
    {
        if (GetCancellationExecutionMethod(context) is not { } executionMethod
            || !IsInReachableBranch(context.Operation)
            || context.Operation is not IAwaitOperation awaitOperation)
        {
            return;
        }

        AnalyzeCancellationInvocations(
            context,
            executionMethod,
            GetAwaitedInvocations(awaitOperation.Operation),
            reportedInvocations);
    }

    private static void AnalyzeForEachLoop(
        OperationAnalysisContext context,
        ConcurrentDictionary<(SyntaxTree Tree, TextSpan Span), byte> reportedInvocations)
    {
        if (GetCancellationExecutionMethod(context) is not { } executionMethod
            || !IsInReachableBranch(context.Operation)
            || context.Operation is not IForEachLoopOperation { IsAsynchronous: true } loop)
        {
            return;
        }

        AnalyzeCancellationInvocations(
            context,
            executionMethod,
            GetAwaitedInvocations(loop.Collection),
            reportedInvocations);
    }

    private static void AnalyzeReturn(
        OperationAnalysisContext context,
        ConcurrentDictionary<(SyntaxTree Tree, TextSpan Span), byte> reportedInvocations)
    {
        if (context.ContainingSymbol is not IMethodSymbol method
            || !IsInReachableBranch(context.Operation)
            || method.MethodKind is not (MethodKind.Ordinary or MethodKind.PropertyGet
                or MethodKind.AnonymousFunction or MethodKind.LocalFunction)
            || method.IsAsync
            || GetCancellationExecutionMethod(context) is not { } executionMethod
            || context.Operation is not IReturnOperation { ReturnedValue: { } returnedValue })
        {
            return;
        }

        AnalyzeCancellationInvocations(
            context,
            executionMethod,
            GetAwaitedInvocations(returnedValue),
            reportedInvocations);
    }

    private static void AnalyzeCancellationInvocations(
        OperationAnalysisContext context,
        IMethodSymbol executeMethod,
        IEnumerable<IInvocationOperation> invocations,
        ConcurrentDictionary<(SyntaxTree Tree, TextSpan Span), byte> reportedInvocations)
    {
        var cancellationTokens = GetFlowingCancellationTokens(
            context,
            executeMethod,
            context.Operation);
        if (cancellationTokens.IsEmpty)
        {
            return;
        }

        var enclosingCallable = GetEnclosingCallable(context.Operation);
        foreach (var invocation in invocations)
        {
            // Returned tasks can lead into a local function with its own token parameters.
            var invocationTokens = ReferenceEquals(GetEnclosingCallable(invocation), enclosingCallable)
                ? cancellationTokens
                : GetFlowingCancellationTokens(context, executeMethod, invocation);
            if (invocationTokens.Any(
                    cancellationToken =>
                        InvocationUsesCancellation(invocation, cancellationToken))
                || !InvocationAcceptsCancellationToken(
                    invocation.TargetMethod,
                    context.Compilation,
                    executeMethod.ContainingType))
            {
                continue;
            }

            // Only record an invocation after establishing that it is a finding;
            // a traversal with a valid token must not suppress a later warning.
            if (!reportedInvocations.TryAdd((invocation.Syntax.SyntaxTree, invocation.Syntax.Span), 0))
            {
                continue;
            }

            context.ReportDiagnostic(Diagnostic.Create(
                ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenRule,
                invocation.Syntax.GetLocation(),
                invocation.TargetMethod.Name));
        }
    }

    private static IEnumerable<IInvocationOperation> GetAwaitedInvocations(
        IOperation operation)
    {
        return GetAwaitedInvocations(
            operation,
            [with(SymbolEqualityComparer.Default)]);
    }

    private static IEnumerable<IInvocationOperation> GetAwaitedInvocations(
        IOperation operation,
        HashSet<ILocalSymbol> visitedLocals)
    {
        var pending = new Stack<(IOperation Operation, bool RequireTaskLike)>();
        HashSet<IMethodSymbol> visitedCallables = [with(SymbolEqualityComparer.Default)];
        pending.Push((operation, false));

        while (pending.Count > 0)
        {
            var (current, requireTaskLike) = pending.Pop();
            if (ProcessAwaitedOperation(
                    current,
                    requireTaskLike,
                    visitedLocals,
                    visitedCallables,
                    pending) is { } invocation)
            {
                yield return invocation;
            }
        }
    }

    private static IInvocationOperation? ProcessAwaitedOperation(
        IOperation operation,
        bool requireTaskLike,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedCallables,
        Stack<(IOperation Operation, bool RequireTaskLike)> pending)
    {
        if (operation is IAnonymousFunctionOperation or IAwaitOperation)
        {
            return null;
        }

        if (operation is ILocalReferenceOperation localReference)
        {
            QueueLocalValue(localReference, requireTaskLike, visitedLocals, pending);
            return null;
        }

        if (operation is IInvocationOperation invocation)
        {
            return ProcessAwaitedInvocation(
                invocation,
                requireTaskLike,
                visitedCallables,
                pending);
        }

        if (operation is IConditionalOperation conditional)
        {
            QueueConditionalOperations(conditional, requireTaskLike, pending);
            return null;
        }

        if (operation is ISwitchExpressionOperation switchExpression)
        {
            QueueSwitchExpressionOperations(switchExpression, requireTaskLike, pending);
            return null;
        }

        if (operation is IArrayElementReferenceOperation
            or IObjectCreationOperation
            or IPropertyReferenceOperation)
        {
            QueueChildOperations(operation, true, pending);
            return null;
        }

        QueueChildOperations(operation, requireTaskLike, pending);
        return null;
    }

    private static void QueueConditionalOperations(
        IConditionalOperation conditional,
        bool requireTaskLike,
        Stack<(IOperation Operation, bool RequireTaskLike)> pending)
    {
        if (conditional.Condition.ConstantValue is { HasValue: true, Value: bool condition })
        {
            var selectedBranch = condition
                ? conditional.WhenTrue
                : conditional.WhenFalse;
            if (selectedBranch is not null)
            {
                pending.Push((selectedBranch, requireTaskLike));
            }

            pending.Push((conditional.Condition, true));
            return;
        }

        if (conditional.WhenFalse is { } whenFalse)
        {
            pending.Push((whenFalse, requireTaskLike));
        }

        pending.Push((conditional.WhenTrue, requireTaskLike));
        pending.Push((conditional.Condition, true));
    }

    private static void QueueSwitchExpressionOperations(
        ISwitchExpressionOperation switchExpression,
        bool requireTaskLike,
        Stack<(IOperation Operation, bool RequireTaskLike)> pending)
    {
        foreach (var arm in switchExpression.Arms
                     .Where(arm => !IsDeadSwitchExpressionArm(arm, switchExpression))
                     .Reverse())
        {
            pending.Push((arm.Value, requireTaskLike));
            if (arm.Guard is { } guard)
            {
                pending.Push((guard, true));
            }

            pending.Push((arm.Pattern, true));
        }

        pending.Push((switchExpression.Value, true));
    }

    private static void QueueLocalValue(
        ILocalReferenceOperation localReference,
        bool requireTaskLike,
        HashSet<ILocalSymbol> visitedLocals,
        Stack<(IOperation Operation, bool RequireTaskLike)> pending)
    {
        if (!visitedLocals.Add(localReference.Local))
        {
            return;
        }

        foreach (var localValue in FindReachingLocalValues(
                     localReference,
                     localReference.Local))
        {
            pending.Push((localValue, requireTaskLike));
        }
    }

    private static IInvocationOperation? ProcessAwaitedInvocation(
        IInvocationOperation invocation,
        bool requireTaskLike,
        HashSet<IMethodSymbol> visitedCallables,
        Stack<(IOperation Operation, bool RequireTaskLike)> pending)
    {
        if (invocation.TargetMethod.Name == "ConfigureAwait"
            && invocation.Instance is { } configuredOperation)
        {
            pending.Push((configuredOperation, requireTaskLike));
            return null;
        }

        if (IsTaskJoin(invocation))
        {
            QueueChildOperations(invocation, true, pending);
            return null;
        }

        QueueInvokedCallbackReturns(invocation, visitedCallables, pending);
        foreach (var argument in invocation.Arguments.Reverse())
        {
            pending.Push((argument.Value, true));
        }

        if (invocation.Instance is { } instance && IsTaskLike(instance.Type))
        {
            pending.Push((instance, true));
        }

        return !requireTaskLike || IsTaskLike(invocation.Type)
            ? invocation
            : null;
    }

    private static void QueueInvokedCallbackReturns(
        IInvocationOperation invocation,
        HashSet<IMethodSymbol> visitedCallables,
        Stack<(IOperation Operation, bool RequireTaskLike)> pending)
    {
        var root = GetRoot(invocation);
        foreach (var callableOperation in root.DescendantsAndSelf()
                     .Where(static operation =>
                         operation is IAnonymousFunctionOperation
                             or ILocalFunctionOperation))
        {
            var callableSymbol = GetCallableSymbol(callableOperation);
            if (callableSymbol is null
                || !InvocationTargetsCallable(invocation, callableSymbol)
                || !visitedCallables.Add(callableSymbol))
            {
                continue;
            }

            QueueCallableReturns(
                callableOperation,
                callableOperation,
                pending);
        }
    }

    private static bool IsStableCapturedCancellationToken(IParameterReferenceOperation reference)
    {
        var callable = GetCallableSymbol(GetEnclosingCallable(reference));
        if (callable is null
            || SymbolEqualityComparer.Default.Equals(callable, reference.Parameter.ContainingSymbol))
        {
            return true;
        }

        // This deliberately includes unreachable and nested writes. Precise lifetime
        // and alias analysis can relax the guard without accepting mutable captures.
        return !GetRoot(reference).DescendantsAndSelf()
            .OfType<IParameterReferenceOperation>()
            .Where(candidate => SymbolEqualityComparer.Default.Equals(candidate.Parameter, reference.Parameter))
            .Any(IsPotentialParameterWrite);
    }

    private static bool IsPotentialParameterWrite(IParameterReferenceOperation reference)
    {
        IOperation target = reference;
        while (target.Parent is IConversionOperation or IParenthesizedOperation or ITupleOperation
            || (target.Parent is IConditionalOperation { IsRef: true } conditional
                && (ReferenceEquals(conditional.WhenTrue, target) || ReferenceEquals(conditional.WhenFalse, target))))
        {
            target = target.Parent;
        }

        return target.Parent switch
        {
            IAssignmentOperation assignment when ReferenceEquals(assignment.Target, target) => true,
            ISimpleAssignmentOperation { IsRef: true, Target: ILocalReferenceOperation { Local.RefKind: RefKind.Ref } } assignment
                when ReferenceEquals(assignment.Value, target) => true,
            IArgumentOperation { Parameter.RefKind: RefKind.Ref or RefKind.Out } => true,
            IVariableInitializerOperation { Parent: IVariableDeclaratorOperation { Symbol.RefKind: RefKind.Ref } } => true,
            _ => false,
        };
    }

    private static ImmutableArray<IParameterSymbol> GetFlowingCancellationTokens(
        OperationAnalysisContext context,
        IMethodSymbol executeMethod,
        IOperation operation)
    {
        if (executeMethod.Parameters.FirstOrDefault(IsCancellationToken)
            is not { } executeCancellationToken)
        {
            return [];
        }

        var containingMethod = GetContainingMemberMethod(
            GetCallableSymbol(GetEnclosingCallable(operation))
                ?? context.ContainingSymbol);
        if (containingMethod is null
            || SymbolEqualityComparer.Default.Equals(
                containingMethod,
                executeMethod))
        {
            return [executeCancellationToken];
        }

        return [
            executeCancellationToken,
            .. FindMappedCancellationTokens(
                executeMethod,
                containingMethod,
                executeCancellationToken,
                context.Compilation,
                context.CancellationToken),
        ];
    }

    private static IMethodSymbol? GetContainingMemberMethod(ISymbol symbol)
    {
        for (var method = symbol as IMethodSymbol;
             method is not null;
             method = method.ContainingSymbol as IMethodSymbol)
        {
            if (method.MethodKind is
                MethodKind.Ordinary or MethodKind.PropertyGet or MethodKind.LocalFunction)
            {
                return method;
            }
        }

        return null;
    }

    private static ImmutableArray<IParameterSymbol> FindMappedCancellationTokens(
        IMethodSymbol executeMethod,
        IMethodSymbol targetMethod,
        IParameterSymbol executeCancellationToken,
        Compilation compilation,
        CancellationToken cancellationToken)
    {
        var analysisType = executeMethod.ContainingType.InheritsFrom(targetMethod.ContainingType)
            ? executeMethod.ContainingType
            : targetMethod.ContainingType;
        var memberMethods = GetModuleMemberMethods(analysisType, compilation);

        var mappedTokenPaths = FindMappedCancellationTokenPaths(
                executeMethod,
                targetMethod,
                [executeCancellationToken],
                memberMethods,
                compilation,
                [
                    with(SymbolEqualityComparer.Default),
                    executeMethod,
                ],
                cancellationToken)
            .ToArray();
        if (mappedTokenPaths.Length == 0)
        {
            return [];
        }

        var guaranteedTokens = new HashSet<IParameterSymbol>(
            mappedTokenPaths[0],
            SymbolEqualityComparer.Default);
        foreach (var mappedTokens in mappedTokenPaths.Skip(1))
        {
            guaranteedTokens.IntersectWith(mappedTokens);
        }

        return [.. guaranteedTokens];
    }

    private static IEnumerable<ImmutableArray<IParameterSymbol>>
        FindMappedCancellationTokenPaths(
            IMethodSymbol currentMethod,
            IMethodSymbol targetMethod,
            ImmutableArray<IParameterSymbol> sourceTokens,
            ImmutableArray<IMethodSymbol> memberMethods,
            Compilation compilation,
            HashSet<IMethodSymbol> visitedMethods,
            CancellationToken cancellationToken)
    {
        if (GetMethodOperation(currentMethod, compilation, cancellationToken)
            is not { } operation)
        {
            yield break;
        }

        if (targetMethod.MethodKind == MethodKind.LocalFunction)
        {
            memberMethods = [
                .. memberMethods.AddRange(operation.DescendantsAndSelf()
                    .OfType<ILocalFunctionOperation>()
                    .Select(static localFunction => localFunction.Symbol))
                    .Distinct<IMethodSymbol>(SymbolEqualityComparer.Default),
            ];
        }

        foreach (var invocation in GetCancellationMappingInvocations(
                     operation,
                     currentMethod,
                     targetMethod))
        {
            foreach (var (method, tokens) in GetInvocationCancellationTokenMappings(
                         invocation,
                         memberMethods,
                         sourceTokens))
            {
                if (SymbolEqualityComparer.Default.Equals(
                        method,
                        targetMethod))
                {
                    yield return method.MethodKind == MethodKind.LocalFunction
                        ? sourceTokens.AddRange(tokens)
                        : tokens;
                    continue;
                }

                var nextVisitedMethods = new HashSet<IMethodSymbol>(
                    visitedMethods,
                    SymbolEqualityComparer.Default);
                if (!nextVisitedMethods.Add(method))
                {
                    continue;
                }

                // Local functions retain access to tokens captured from their caller.
                var nextTokens = method.MethodKind == MethodKind.LocalFunction
                    ? sourceTokens.AddRange(tokens)
                    : tokens;

                foreach (var mappedTokens in FindMappedCancellationTokenPaths(
                             method,
                             targetMethod,
                             nextTokens,
                             memberMethods,
                             compilation,
                             nextVisitedMethods,
                             cancellationToken))
                {
                    yield return mappedTokens;
                }
            }
        }
    }

    private static IEnumerable<IInvocationOperation> GetCancellationMappingInvocations(
        IOperation operation,
        IMethodSymbol currentMethod,
        IMethodSymbol targetMethod)
    {
        foreach (var invocation in GetReachableInvocations(operation))
        {
            // Local functions map their arguments in their own traversal step.
            if (targetMethod.MethodKind == MethodKind.LocalFunction
                && GetEnclosingCallable(invocation) is ILocalFunctionOperation localFunction
                && !SymbolEqualityComparer.Default.Equals(localFunction.Symbol, currentMethod))
            {
                continue;
            }

            yield return invocation;
        }
    }

    private static IEnumerable<IInvocationOperation> GetReachableInvocations(
        IOperation operation)
    {
        var invocations = operation.DescendantsAndSelf()
            .OfType<IInvocationOperation>()
            .ToImmutableArray();
        var nestedCallables = operation.DescendantsAndSelf()
            .Select(GetCallableSymbol)
            .Where(static callable => callable is not null)
            .Cast<IMethodSymbol>()
            .Distinct<IMethodSymbol>(SymbolEqualityComparer.Default)
            .ToImmutableArray();
        var reachableNestedCallables = GetReachableCallables(
            invocations,
            nestedCallables,
            GetCallableSymbol(operation));

        return invocations.Where(invocation =>
            IsInsideReachableCallable(
                invocation,
                reachableNestedCallables));
    }

    private static IEnumerable<(
        IMethodSymbol Method,
        ImmutableArray<IParameterSymbol> Tokens)>
        GetInvocationCancellationTokenMappings(
            IInvocationOperation invocation,
            ImmutableArray<IMethodSymbol> memberMethods,
            ImmutableArray<IParameterSymbol> sourceTokens)
    {
        foreach (var method in memberMethods.Where(candidate =>
                     InvocationTargetsCallable(invocation, candidate)))
        {
            var mappedTokens = invocation.Arguments
                .Where(argument =>
                    argument.Parameter is not null
                    && IsCancellationToken(argument.Parameter)
                    && sourceTokens.Any(sourceToken =>
                        FlowsFromCancellationToken(
                            argument.Value,
                            sourceToken,
                            [with(SymbolEqualityComparer.Default)])))
                .Select(argument => method.Parameters.ElementAtOrDefault(
                    argument.Parameter!.Ordinal))
                .Where(static token => token is not null)
                .Cast<IParameterSymbol>()
                .Where(IsCancellationToken)
                .Distinct<IParameterSymbol>(SymbolEqualityComparer.Default)
                .ToImmutableArray();
            yield return (method, mappedTokens);
        }
    }

    private static void QueueCallableReturns(
        IOperation body,
        IOperation callable,
        Stack<(IOperation Operation, bool RequireTaskLike)> pending)
    {
        foreach (var returnValue in body.DescendantsAndSelf()
                     .OfType<IReturnOperation>()
                     .Where(returnOperation =>
                         IsInReachableBranch(returnOperation)
                         && ReferenceEquals(
                             GetEnclosingCallable(returnOperation),
                             callable))
                     .Select(static returnOperation => returnOperation.ReturnedValue)
                     .OfType<IOperation>()
                     .Reverse())
        {
            pending.Push((returnValue, true));
        }
    }

    private static void QueueChildOperations(
        IOperation operation,
        bool requireTaskLike,
        Stack<(IOperation Operation, bool RequireTaskLike)> pending)
    {
        foreach (var child in operation.ChildOperations.Reverse())
        {
            pending.Push((child, requireTaskLike));
        }
    }

    private static bool IsTaskJoin(IInvocationOperation invocation)
    {
        return invocation.TargetMethod.Name is "WhenAll" or "WhenAny"
            && invocation.TargetMethod.ContainingType.ToDisplayString()
            == "System.Threading.Tasks.Task";
    }

    private static bool IsTaskLike(ITypeSymbol? type)
    {
        return type is not null
               && TryGetTaskResultType(type, out _);
    }

    private static void TrackRegistrationInvocation(
        IInvocationOperation invocation,
        Compilation compilation,
        IAssemblySymbol currentAssembly,
        bool isApplication,
        ConcurrentBag<INamedTypeSymbol> registeredModules,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        ConcurrentBag<IAssemblySymbol> scannedAssemblies,
        ConcurrentBag<byte> unresolvedModuleRegistrations,
        ConcurrentBag<byte> unresolvedFactoryRegistrations)
    {
        var method = invocation.TargetMethod;
        if (TryTrackDirectModuleServiceRegistration(
                invocation,
                compilation,
                instanceRegisteredModules,
                unresolvedModuleRegistrations,
                unresolvedFactoryRegistrations))
        {
            return;
        }

        if (!IsModuleRegistrationMethod(method))
        {
            return;
        }

        if (method.Name.StartsWith("AddModulesFromAssembly", StringComparison.Ordinal))
        {
            TrackScannedAssemblies(
                invocation,
                compilation,
                currentAssembly,
                isApplication,
                scannedAssemblies);
            return;
        }

        TrackGenericModuleRegistrations(
            invocation,
            compilation,
            registeredModules,
            instanceRegisteredModules);
        if (method.Name == "AddModules")
        {
            TrackDynamicModuleRegistrations(
                invocation,
                compilation,
                registeredModules,
                unresolvedModuleRegistrations);
        }
    }

    private static bool IsModuleRegistrationMethod(IMethodSymbol method)
    {
        var definition = (method.ReducedFrom ?? method).OriginalDefinition;
        if (IsModuleTesterRegistrationMethod(definition))
        {
            return true;
        }

        var containingType = definition.ContainingType;
        var isRegistrationType = containingType.ToDisplayString() is
                                     "ModularPipelines.PipelineBuilderExtensions"
                                     or "ModularPipelines.Extensions.PipelineBuilderExtensions"
                                     or "ModularPipelines.Extensions.ServiceCollectionExtensions";
        return definition.ContainingAssembly.Name == "ModularPipelines"
               && isRegistrationType
               && definition.Name is
                   "AddModule"
                   or "AddModules"
                   or "AddModulesFromAssembly"
                   or "AddModulesFromAssemblyContainingType";
    }

    private static bool IsModuleTesterRegistrationMethod(IMethodSymbol definition)
    {
        return definition.ContainingAssembly.Name == "ModularPipelines.Testing"
               && definition.ContainingType.OriginalDefinition.ToDisplayString() is
                   "ModularPipelines.Testing.ModuleTestBuilder<TModule>"
                   or "ModularPipelines.Testing.ModuleTestBuilder<TModule, TResult>"
               && definition.Name == "ExecuteAsync"
               && !definition.IsStatic
               && definition.Arity == 0
               && definition.Parameters.Length == 1
               && IsCancellationToken(definition.Parameters[0]);
    }

    private static bool TryTrackDirectModuleServiceRegistration(
        IInvocationOperation invocation,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        ConcurrentBag<byte> unresolvedModuleRegistrations,
        ConcurrentBag<byte> unresolvedFactoryRegistrations)
    {
        var method = invocation.TargetMethod;
        var definition = (method.ReducedFrom ?? method).OriginalDefinition;
        if (IsServiceDescriptorRegistrationMethod(invocation))
        {
            return TryTrackServiceDescriptorArguments(
                invocation,
                compilation,
                instanceRegisteredModules,
                unresolvedModuleRegistrations,
                unresolvedFactoryRegistrations);
        }

        if (!IsDirectServiceRegistrationMethod(definition)
            || !RegistersModuleService(invocation, method))
        {
            return false;
        }

        if (!TryTrackDirectImplementationType(
                invocation,
                method,
                compilation,
                instanceRegisteredModules,
                unresolvedModuleRegistrations))
        {
            TrackDirectImplementationValue(
                invocation,
                compilation,
                instanceRegisteredModules,
                unresolvedModuleRegistrations,
                unresolvedFactoryRegistrations);
        }

        return true;
    }

    private static bool IsDirectServiceRegistrationMethod(IMethodSymbol definition)
    {
        var containingType = definition.ContainingType.ToDisplayString();
        return (definition.Name is "AddSingleton" or "AddScoped" or "AddTransient"
                && containingType is
                    "Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions"
                    or "ModularPipelines.PipelineBuilderExtensions"
                    or "ModularPipelines.Extensions.PipelineBuilderExtensions")
               || (definition.Name is "TryAddSingleton" or "TryAddScoped" or "TryAddTransient"
                   && containingType
                   == "Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions");
    }

    private static bool IsServiceDescriptorRegistrationMethod(
        IInvocationOperation invocation)
    {
        var definition = (invocation.TargetMethod.ReducedFrom ?? invocation.TargetMethod)
            .OriginalDefinition;
        if (!InvocationTargetsServiceCollection(invocation)
            || !invocation.TargetMethod.Parameters.Any(static parameter =>
                IsServiceDescriptorType(parameter.Type)))
        {
            return false;
        }

        var containingType = definition.ContainingType.OriginalDefinition.ToDisplayString();
        return definition.Name switch
        {
            "Add" => containingType is
                "System.Collections.Generic.ICollection<T>"
                or "Microsoft.Extensions.DependencyInjection.ServiceCollection",
            "Insert" => containingType is
                "System.Collections.Generic.IList<T>"
                or "Microsoft.Extensions.DependencyInjection.ServiceCollection",
            "Replace" or "TryAdd" or "TryAddEnumerable" => containingType ==
                "Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions",
            _ => false,
        };
    }

    private static bool InvocationTargetsServiceCollection(
        IInvocationOperation invocation)
    {
        if (IsServiceCollectionType(invocation.Instance?.Type))
        {
            return true;
        }

        return invocation.Arguments.Any(argument =>
            argument.Parameter?.Type is { } parameterType
            && IsServiceCollectionType(parameterType));
    }

    private static bool IsServiceCollectionType(ITypeSymbol? type)
    {
        const string serviceCollectionType =
            "Microsoft.Extensions.DependencyInjection.IServiceCollection";
        return type?.ToDisplayString() == serviceCollectionType
               || (type is INamedTypeSymbol namedType
                   && namedType.AllInterfaces.Any(interfaceType =>
                       interfaceType.ToDisplayString() == serviceCollectionType));
    }

    private static bool IsServiceDescriptorType(ITypeSymbol type)
    {
        if (type.ToDisplayString()
            == "Microsoft.Extensions.DependencyInjection.ServiceDescriptor")
        {
            return true;
        }

        return type is INamedTypeSymbol namedType
               && namedType.IsGenericType
               && namedType.TypeArguments.Length == 1
               && namedType.TypeArguments[0].ToDisplayString()
               == "Microsoft.Extensions.DependencyInjection.ServiceDescriptor";
    }

    private static bool TryTrackServiceDescriptorArguments(
        IInvocationOperation invocation,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        ConcurrentBag<byte> unresolvedModuleRegistrations,
        ConcurrentBag<byte> unresolvedFactoryRegistrations)
    {
        var descriptorArguments = invocation.Arguments
            .Where(argument => argument.Parameter is not null
                               && IsServiceDescriptorType(argument.Parameter.Type))
            .ToArray();
        foreach (var argument in descriptorArguments)
        {
            if (TryTrackServiceDescriptor(
                    argument.Value,
                    compilation,
                    instanceRegisteredModules,
                    [with(SymbolEqualityComparer.Default)],
                    [with(SymbolEqualityComparer.Default)]))
            {
                return true;
            }

            if (IsUnresolvedModuleServiceDescriptor(argument.Value))
            {
                unresolvedModuleRegistrations.Add(0);
                if (ContainsUnresolvedModuleServiceDescriptorFactory(argument.Value))
                {
                    unresolvedFactoryRegistrations.Add(0);
                }

                return true;
            }
        }

        return false;
    }

    private static bool IsUnresolvedModuleServiceDescriptor(IOperation operation)
    {
        return IsUnresolvedModuleServiceDescriptor(operation, [], []);
    }

    private static bool IsUnresolvedModuleServiceDescriptor(
        IOperation operation,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods)
    {
        while (operation is IConversionOperation conversion)
        {
            operation = conversion.Operand;
        }

        return operation switch
        {
            IConversionOperation conversion =>
                IsUnresolvedModuleServiceDescriptor(conversion.Operand, visitedLocals, visitedMethods),
            IArrayCreationOperation { Initializer: { } initializer } =>
                initializer.ElementValues.Any(element =>
                    IsUnresolvedModuleServiceDescriptor(
                        element,
                        CloneVisitedLocals(visitedLocals),
                        CloneVisitedMethods(visitedMethods))),
            ICollectionExpressionOperation collection =>
                collection.Elements.Any(element =>
                    IsUnresolvedModuleServiceDescriptor(
                        element,
                        CloneVisitedLocals(visitedLocals),
                        CloneVisitedMethods(visitedMethods))),
            ISpreadOperation spread =>
                IsUnresolvedModuleServiceDescriptor(spread.Operand, visitedLocals, visitedMethods),
            IConditionalOperation conditional =>
                IsUnresolvedModuleServiceDescriptorConditional(
                    conditional,
                    visitedLocals,
                    visitedMethods),
            ICoalesceOperation coalesce =>
                IsUnresolvedModuleServiceDescriptor(
                    coalesce.Value,
                    CloneVisitedLocals(visitedLocals),
                    CloneVisitedMethods(visitedMethods))
                || IsUnresolvedModuleServiceDescriptor(
                    coalesce.WhenNull,
                    CloneVisitedLocals(visitedLocals),
                    CloneVisitedMethods(visitedMethods)),
            ISwitchExpressionOperation switchExpression =>
                switchExpression.Arms
                    .Where(arm => !IsDeadSwitchExpressionArm(arm, switchExpression))
                    .Any(arm =>
                    IsUnresolvedModuleServiceDescriptor(
                        arm.Value,
                        CloneVisitedLocals(visitedLocals),
                        CloneVisitedMethods(visitedMethods))),
            ILocalReferenceOperation localReference =>
                IsUnresolvedModuleServiceDescriptorLocal(
                    localReference,
                    visitedLocals,
                    visitedMethods),
            IFieldReferenceOperation fieldReference =>
                IsUnresolvedModuleServiceDescriptorMember(
                    fieldReference,
                    fieldReference.Field,
                    visitedLocals,
                    visitedMethods,
                    [with(SymbolEqualityComparer.Default)]),
            IPropertyReferenceOperation propertyReference =>
                IsUnresolvedModuleServiceDescriptorMember(
                    propertyReference,
                    propertyReference.Property,
                    visitedLocals,
                    visitedMethods,
                    [with(SymbolEqualityComparer.Default)]),
            IInvocationOperation invocation => IsUnresolvedModuleServiceDescriptorInvocation(
                invocation,
                visitedLocals,
                visitedMethods),
            _ => false,
        };
    }

    private static bool IsUnresolvedModuleServiceDescriptorConditional(
        IConditionalOperation conditional,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods)
    {
        if (conditional.Condition.ConstantValue is { HasValue: true, Value: bool condition })
        {
            var selectedBranch = condition
                ? conditional.WhenTrue
                : conditional.WhenFalse;
            return selectedBranch is not null
                   && IsUnresolvedModuleServiceDescriptor(
                       selectedBranch,
                       visitedLocals,
                       visitedMethods);
        }

        return IsUnresolvedModuleServiceDescriptor(
                   conditional.WhenTrue,
                   CloneVisitedLocals(visitedLocals),
                   CloneVisitedMethods(visitedMethods))
               || (conditional.WhenFalse is not null
                   && IsUnresolvedModuleServiceDescriptor(
                       conditional.WhenFalse,
                       CloneVisitedLocals(visitedLocals),
                       CloneVisitedMethods(visitedMethods)));
    }

    private static bool IsUnresolvedModuleServiceDescriptorLocal(
        ILocalReferenceOperation localReference,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods)
    {
        return visitedLocals.Add(localReference.Local)
               && FindReachingLocalValues(localReference, localReference.Local)
                   .Any(value => IsUnresolvedModuleServiceDescriptor(
                       value,
                       CloneVisitedLocals(visitedLocals),
                       CloneVisitedMethods(visitedMethods)));
    }

    private static bool IsUnresolvedModuleServiceDescriptorMember(
        IOperation memberReference,
        ISymbol member,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods,
        HashSet<ISymbol> visitedMembers)
    {
        if (!visitedMembers.Add(member)
            || memberReference.SemanticModel?.Compilation is not { } compilation)
        {
            return false;
        }

        return GetMemberValues(memberReference, member, compilation).Any(value => value switch
        {
            IFieldReferenceOperation fieldReference =>
                IsUnresolvedModuleServiceDescriptorMember(
                    fieldReference,
                    fieldReference.Field,
                    CloneVisitedLocals(visitedLocals),
                    CloneVisitedMethods(visitedMethods),
                    new HashSet<ISymbol>(visitedMembers, SymbolEqualityComparer.Default)),
            IPropertyReferenceOperation propertyReference =>
                IsUnresolvedModuleServiceDescriptorMember(
                    propertyReference,
                    propertyReference.Property,
                    CloneVisitedLocals(visitedLocals),
                    CloneVisitedMethods(visitedMethods),
                    new HashSet<ISymbol>(visitedMembers, SymbolEqualityComparer.Default)),
            _ => IsUnresolvedModuleServiceDescriptor(
                value,
                CloneVisitedLocals(visitedLocals),
                CloneVisitedMethods(visitedMethods)),
        });
    }

    private static bool IsUnresolvedModuleServiceDescriptorInvocation(
        IInvocationOperation invocation,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods)
    {
        if (IsUnresolvedModuleServiceDescriptorFactory(invocation))
        {
            return true;
        }

        var method = invocation.TargetMethod.OriginalDefinition;
        if (!visitedMethods.Add(method)
            || invocation.SemanticModel?.Compilation is not { } compilation
            || GetMethodOperation(method, compilation, default) is not { } operation)
        {
            return false;
        }

        return operation.DescendantsAndSelf()
            .OfType<IReturnOperation>()
            .Where(static returnOperation =>
                GetEnclosingCallable(returnOperation) is null
                && IsInReachableBranch(returnOperation))
            .Select(static returnOperation => returnOperation.ReturnedValue)
            .OfType<IOperation>()
            .Any(returnValue => IsUnresolvedModuleServiceDescriptorInvocationReturn(
                returnValue,
                invocation,
                CloneVisitedLocals(visitedLocals),
                CloneVisitedMethods(visitedMethods)));
    }

    private static bool IsUnresolvedModuleServiceDescriptorInvocationReturn(
        IOperation returnValue,
        IInvocationOperation invocation,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods)
    {
        foreach (var candidate in GetReachableValueLeaves(returnValue))
        {
            if (candidate is IParameterReferenceOperation parameterReference)
            {
                var targetMethod = NormalizeMethod(invocation.TargetMethod);
                var argument = invocation.Arguments.FirstOrDefault(argument =>
                    SymbolEqualityComparer.Default.Equals(
                        GetTargetParameter(targetMethod, argument.Parameter),
                        parameterReference.Parameter));
                if (argument is not null
                    && IsUnresolvedModuleServiceDescriptor(
                        argument.Value,
                        CloneVisitedLocals(visitedLocals),
                        CloneVisitedMethods(visitedMethods)))
                {
                    return true;
                }

                continue;
            }

            if (IsUnresolvedModuleServiceDescriptor(
                    candidate,
                    CloneVisitedLocals(visitedLocals),
                    CloneVisitedMethods(visitedMethods)))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsUnresolvedModuleServiceDescriptorFactory(IInvocationOperation invocation)
    {
        var implementation = invocation.Arguments.FirstOrDefault(
            static argument => argument.Parameter?.Name is
                "implementationType" or "implementationFactory");
        return IsServiceDescriptorFactory(invocation.TargetMethod)
               && RegistersModuleService(invocation.Arguments, invocation.TargetMethod.TypeArguments)
               && implementation is not null
               && (implementation.Parameter?.Name == "implementationFactory"
                   || !TryGetTypeOfNamedTypes(implementation.Value, out _));
    }

    private static bool ContainsUnresolvedModuleServiceDescriptorFactory(IOperation operation) =>
        operation.DescendantsAndSelf()
            .OfType<IInvocationOperation>()
            .Any(invocation =>
                IsUnresolvedModuleServiceDescriptorFactory(invocation)
                && invocation.Arguments.Any(static argument =>
                    argument.Parameter?.Name == "implementationFactory"));

    private static bool TryTrackServiceDescriptor(
        IOperation operation,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods)
    {
        return operation switch
        {
            IConversionOperation conversion => TryTrackServiceDescriptor(
                conversion.Operand,
                compilation,
                instanceRegisteredModules,
                visitedLocals,
                visitedMethods),
            ILocalReferenceOperation localReference => TryTrackServiceDescriptorLocal(
                localReference,
                compilation,
                instanceRegisteredModules,
                visitedLocals,
                visitedMethods),
            IFieldReferenceOperation fieldReference => TryTrackServiceDescriptorMember(
                fieldReference,
                fieldReference.Field,
                compilation,
                instanceRegisteredModules,
                visitedLocals,
                visitedMethods,
                [with(SymbolEqualityComparer.Default)]),
            IPropertyReferenceOperation propertyReference => TryTrackServiceDescriptorMember(
                propertyReference,
                propertyReference.Property,
                compilation,
                instanceRegisteredModules,
                visitedLocals,
                visitedMethods,
                [with(SymbolEqualityComparer.Default)]),
            IConditionalOperation conditional => TryTrackServiceDescriptorConditional(
                conditional,
                compilation,
                instanceRegisteredModules,
                visitedLocals,
                visitedMethods),
            ICoalesceOperation coalesce => TryTrackServiceDescriptorCoalesce(
                coalesce,
                compilation,
                instanceRegisteredModules,
                visitedLocals,
                visitedMethods),
            ISwitchExpressionOperation switchExpression => TryTrackServiceDescriptorSwitchExpression(
                switchExpression,
                compilation,
                instanceRegisteredModules,
                visitedLocals,
                visitedMethods),
            IInvocationOperation descriptorFactory
                when IsServiceDescriptorFactory(descriptorFactory.TargetMethod) =>
                TrackServiceDescriptor(
                    descriptorFactory.Arguments,
                    descriptorFactory.TargetMethod.TypeArguments,
                    compilation,
                    instanceRegisteredModules),
            IInvocationOperation invocation => TryTrackServiceDescriptorInvocation(
                invocation,
                compilation,
                instanceRegisteredModules,
                visitedLocals,
                visitedMethods),
            IObjectCreationOperation objectCreation
                when objectCreation.Type?.ToDisplayString()
                     == "Microsoft.Extensions.DependencyInjection.ServiceDescriptor" =>
                TrackServiceDescriptor(
                    objectCreation.Arguments,
                    objectCreation.Constructor?.TypeArguments
                    ?? [],
                    compilation,
                    instanceRegisteredModules),
            IArrayCreationOperation { Initializer: { } initializer } => TryTrackServiceDescriptorCollection(
                initializer.ElementValues,
                compilation,
                instanceRegisteredModules,
                visitedLocals,
                visitedMethods),
            ICollectionExpressionOperation collection => TryTrackServiceDescriptorCollection(
                collection.Elements,
                compilation,
                instanceRegisteredModules,
                visitedLocals,
                visitedMethods),
            ISpreadOperation spread => TryTrackServiceDescriptor(
                spread.Operand,
                compilation,
                instanceRegisteredModules,
                visitedLocals,
                visitedMethods),
            _ => false,
        };
    }

    private static bool TryTrackServiceDescriptorCoalesce(
        ICoalesceOperation coalesce,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods)
    {
        if (coalesce.Value.ConstantValue is { HasValue: true, Value: null })
        {
            return TryTrackServiceDescriptor(
                coalesce.WhenNull,
                compilation,
                instanceRegisteredModules,
                visitedLocals,
                visitedMethods);
        }

        return TryTrackServiceDescriptor(
                   coalesce.Value,
                   compilation,
                   instanceRegisteredModules,
                   CloneVisitedLocals(visitedLocals),
                   CloneVisitedMethods(visitedMethods))
               && TryTrackServiceDescriptor(
                   coalesce.WhenNull,
                   compilation,
                   instanceRegisteredModules,
                   CloneVisitedLocals(visitedLocals),
                   CloneVisitedMethods(visitedMethods));
    }

    private static bool TryTrackServiceDescriptorConditional(
        IConditionalOperation conditional,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods)
    {
        if (conditional.Condition.ConstantValue is { HasValue: true, Value: bool condition })
        {
            var selectedBranch = condition
                ? conditional.WhenTrue
                : conditional.WhenFalse;
            return selectedBranch is not null
                   && TryTrackServiceDescriptor(
                       selectedBranch,
                       compilation,
                       instanceRegisteredModules,
                       visitedLocals,
                       visitedMethods);
        }

        return (AlwaysThrows(conditional.WhenTrue)
                || TryTrackServiceDescriptor(
                    conditional.WhenTrue,
                    compilation,
                    instanceRegisteredModules,
                    CloneVisitedLocals(visitedLocals),
                    CloneVisitedMethods(visitedMethods)))
               && conditional.WhenFalse is { } whenFalse
               && (AlwaysThrows(whenFalse)
                   || TryTrackServiceDescriptor(
                       whenFalse,
                       compilation,
                       instanceRegisteredModules,
                       CloneVisitedLocals(visitedLocals),
                       CloneVisitedMethods(visitedMethods)));
    }

    private static bool TryTrackServiceDescriptorSwitchExpression(
        ISwitchExpressionOperation switchExpression,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods)
    {
        var reachableArms = switchExpression.Arms
            .Where(arm => !IsDeadSwitchExpressionArm(arm, switchExpression))
            .Where(static arm => !AlwaysThrows(arm.Value))
            .ToArray();
        return reachableArms.Length > 0
               && reachableArms.All(arm => TryTrackServiceDescriptor(
                   arm.Value,
                   compilation,
                   instanceRegisteredModules,
                   CloneVisitedLocals(visitedLocals),
                   CloneVisitedMethods(visitedMethods)));
    }

    private static bool TryTrackServiceDescriptorLocal(
        ILocalReferenceOperation localReference,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods)
    {
        if (!visitedLocals.Add(localReference.Local))
        {
            return false;
        }

        return TryTrackServiceDescriptorCollection(
            FindReachingLocalValues(localReference, localReference.Local),
            compilation,
            instanceRegisteredModules,
            visitedLocals,
            visitedMethods);
    }

    private static bool TryTrackServiceDescriptorMember(
        IOperation memberReference,
        ISymbol member,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods,
        HashSet<ISymbol> visitedMembers)
    {
        if (!visitedMembers.Add(member))
        {
            return false;
        }

        var memberValues = GetMemberValues(memberReference, member, compilation).ToArray();

        return memberValues.Length > 0
               && memberValues.All(value => value switch
               {
                   IFieldReferenceOperation fieldReference => TryTrackServiceDescriptorMember(
                       fieldReference,
                       fieldReference.Field,
                       compilation,
                       instanceRegisteredModules,
                       CloneVisitedLocals(visitedLocals),
                       CloneVisitedMethods(visitedMethods),
                       new HashSet<ISymbol>(visitedMembers, SymbolEqualityComparer.Default)),
                   IPropertyReferenceOperation propertyReference => TryTrackServiceDescriptorMember(
                       propertyReference,
                       propertyReference.Property,
                       compilation,
                       instanceRegisteredModules,
                       CloneVisitedLocals(visitedLocals),
                       CloneVisitedMethods(visitedMethods),
                       new HashSet<ISymbol>(visitedMembers, SymbolEqualityComparer.Default)),
                   _ => TryTrackServiceDescriptor(
                       value,
                       compilation,
                       instanceRegisteredModules,
                       CloneVisitedLocals(visitedLocals),
                       CloneVisitedMethods(visitedMethods)),
               });
    }

    private static IEnumerable<IOperation> GetDeclaredMemberValues(
        ISymbol member,
        Compilation compilation)
    {
        var operations = GetDeclaredMemberOperations(member, compilation).ToArray();
        var foundValue = false;
        foreach (var operation in operations)
        {
            if (GetDeclaredMemberValue(operation, member) is { } value)
            {
                foundValue = true;
                yield return value;
            }
        }

        if (!foundValue
            && TryGetFallbackDeclaredMemberValue(
                operations,
                GetMemberType(member),
                compilation) is { } fallbackValue)
        {
            yield return fallbackValue;
        }
    }

    private static IOperation? GetDeclaredMemberValue(
        IOperation operation,
        ISymbol member) =>
        operation switch
        {
            IFieldInitializerOperation fieldInitializer
                when fieldInitializer.InitializedFields.Any(field =>
                    SymbolEqualityComparer.Default.Equals(field, member)) =>
                fieldInitializer.Value,
            IPropertyInitializerOperation propertyInitializer
                when propertyInitializer.InitializedProperties.Any(property =>
                    SymbolEqualityComparer.Default.Equals(property, member)) =>
                propertyInitializer.Value,
            IReturnOperation { ReturnedValue: { } returnedValue }
                when GetEnclosingCallable(operation) is null
                     && IsInReachableBranch(operation) => returnedValue,
            _ => null,
        };

    private static ITypeSymbol? GetMemberType(ISymbol member) =>
        member switch
        {
            IFieldSymbol field => field.Type,
            IPropertySymbol property => property.Type,
            _ => null,
        };

    private static IOperation? TryGetFallbackDeclaredMemberValue(
        IEnumerable<IOperation> operations,
        ITypeSymbol? memberType,
        Compilation compilation) =>
        memberType is null
            ? null
            : operations
                .Where(operation => operation.Type is { } operationType
                                    && (SymbolEqualityComparer.Default.Equals(
                                            operationType,
                                            memberType)
                                        || compilation.ClassifyCommonConversion(
                                                operationType,
                                                memberType)
                                            .IsImplicit))
                .OrderByDescending(static operation => operation.Syntax.Span.Length)
                .FirstOrDefault();

    private static IEnumerable<IOperation> GetMemberValues(
        IOperation memberReference,
        ISymbol member,
        Compilation compilation)
    {
        if (member is IEventSymbol @event)
        {
            return GetEventHandlerValues(memberReference, @event, compilation);
        }

        var memberValues = FindReachingMemberValues(memberReference, member).ToArray();
        if (memberValues.Length > 0)
        {
            return memberValues;
        }

        memberValues = [
            .. GetInvokedHelperMemberValues(
                memberReference,
                member,
                compilation),
        ];
        if (memberValues.Length > 0)
        {
            return memberValues;
        }

        memberValues = [.. GetConstructorMemberValues(member, compilation)];
        return memberValues.Length > 0
            ? memberValues
            : GetDeclaredMemberValues(member, compilation);
    }

    private static IEnumerable<IOperation> GetInvokedHelperMemberValues(
        IOperation memberReference,
        ISymbol member,
        Compilation compilation)
    {
        var callable = GetEnclosingCallable(memberReference);
        var precedingInvocations = GetRoot(memberReference)
            .DescendantsAndSelf()
            .OfType<IInvocationOperation>()
            .Where(invocation =>
                invocation.Syntax.Span.End <= memberReference.Syntax.SpanStart)
            .Where(invocation => ReferenceEquals(
                GetEnclosingCallable(invocation),
                callable))
            .Where(IsInReachableBranch)
            .Where(invocation => IsLinearPredecessor(invocation, memberReference))
            .OrderByDescending(static invocation => invocation.Syntax.SpanStart);

        foreach (var invocation in precedingInvocations)
        {
            var method = NormalizeMethod(invocation.TargetMethod);
            if (GetMethodOperation(method, compilation, default) is not { } operation)
            {
                continue;
            }

            var values = GetFinalMemberValues(operation, member).ToArray();
            if (values.Length == 0)
            {
                continue;
            }

            foreach (var value in values)
            {
                yield return value;
            }

            yield break;
        }
    }

    private static IEnumerable<IOperation> GetEventHandlerValues(
        IOperation eventReference,
        IEventSymbol @event,
        Compilation compilation)
    {
        var callable = GetEnclosingCallable(eventReference);
        var reachingHandlers = GetRoot(eventReference)
            .DescendantsAndSelf()
            .OfType<IEventAssignmentOperation>()
            .Where(static assignment => assignment.Adds)
            .Where(assignment => assignment.Syntax.SpanStart < eventReference.Syntax.SpanStart)
            .Where(assignment => ReferenceEquals(GetEnclosingCallable(assignment), callable))
            .Where(IsInReachableBranch)
            .Where(assignment => EventAssignmentTargets(assignment, @event))
            .Select(static assignment => assignment.HandlerValue);
        return reachingHandlers.Concat(GetConstructorEventHandlerValues(@event, compilation));
    }

    private static IEnumerable<IOperation> GetConstructorEventHandlerValues(
        IEventSymbol @event,
        Compilation compilation)
    {
        var constructors = @event.IsStatic
            ? @event.ContainingType.StaticConstructors
            : @event.ContainingType.InstanceConstructors;
        foreach (var constructor in constructors)
        {
            if (GetMethodOperation(constructor, compilation, default) is not { } operation)
            {
                continue;
            }

            foreach (var assignment in operation.DescendantsAndSelf()
                         .OfType<IEventAssignmentOperation>()
                         .Where(static assignment => assignment.Adds)
                         .Where(static assignment => GetEnclosingCallable(assignment) is null)
                         .Where(IsInReachableBranch)
                         .Where(assignment => EventAssignmentTargets(assignment, @event)))
            {
                yield return assignment.HandlerValue;
            }
        }
    }

    private static bool EventAssignmentTargets(
        IEventAssignmentOperation assignment,
        IEventSymbol @event) =>
        assignment.EventReference is IEventReferenceOperation eventReference
        && SymbolEqualityComparer.Default.Equals(eventReference.Event, @event);

    private static bool TryTrackServiceDescriptorInvocation(
        IInvocationOperation invocation,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods)
    {
        var method = invocation.TargetMethod.OriginalDefinition;
        if (!visitedMethods.Add(method)
            || GetMethodOperation(method, compilation, default) is not { } operation)
        {
            return false;
        }

        var returnValues = operation.DescendantsAndSelf()
            .OfType<IReturnOperation>()
            .Where(static returnOperation =>
                GetEnclosingCallable(returnOperation) is null
                && IsInReachableBranch(returnOperation))
            .Select(static returnOperation => returnOperation.ReturnedValue)
            .OfType<IOperation>()
            .ToArray();
        return returnValues.Length > 0
               && returnValues.All(returnValue =>
                   TryTrackServiceDescriptorInvocationReturn(
                       returnValue,
                       invocation,
                       compilation,
                       instanceRegisteredModules,
                       CloneVisitedLocals(visitedLocals),
                       CloneVisitedMethods(visitedMethods)));
    }

    private static bool TryTrackServiceDescriptorInvocationReturn(
        IOperation returnValue,
        IInvocationOperation invocation,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods)
    {
        return returnValue switch
        {
            IConversionOperation conversion => TryTrackServiceDescriptorInvocationReturn(
                conversion.Operand,
                invocation,
                compilation,
                instanceRegisteredModules,
                visitedLocals,
                visitedMethods),
            IParameterReferenceOperation parameterReference => TryTrackServiceDescriptorParameterReturn(
                parameterReference,
                invocation,
                compilation,
                instanceRegisteredModules,
                visitedLocals,
                visitedMethods),
            IConditionalOperation conditional => TryTrackServiceDescriptorConditionalReturn(
                conditional,
                invocation,
                compilation,
                instanceRegisteredModules,
                visitedLocals,
                visitedMethods),
            ISwitchExpressionOperation switchExpression => TryTrackServiceDescriptorSwitchReturn(
                switchExpression,
                invocation,
                compilation,
                instanceRegisteredModules,
                visitedLocals,
                visitedMethods),
            _ => TryTrackServiceDescriptor(
                returnValue,
                compilation,
                instanceRegisteredModules,
                visitedLocals,
                visitedMethods),
        };
    }

    private static bool TryTrackServiceDescriptorParameterReturn(
        IParameterReferenceOperation parameterReference,
        IInvocationOperation invocation,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods)
    {
        var targetMethod = NormalizeMethod(invocation.TargetMethod);
        var argument = invocation.Arguments.FirstOrDefault(candidate =>
            SymbolEqualityComparer.Default.Equals(
                GetTargetParameter(targetMethod, candidate.Parameter),
                parameterReference.Parameter));
        return argument is not null
               && TryTrackServiceDescriptor(
                   argument.Value,
                   compilation,
                   instanceRegisteredModules,
                   visitedLocals,
                   visitedMethods);
    }

    private static bool TryTrackServiceDescriptorConditionalReturn(
        IConditionalOperation conditional,
        IInvocationOperation invocation,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods)
    {
        if (conditional.Condition.ConstantValue is { HasValue: true, Value: bool condition })
        {
            var selectedBranch = condition
                ? conditional.WhenTrue
                : conditional.WhenFalse;
            return selectedBranch is not null
                   && TryTrackServiceDescriptorInvocationReturn(
                       selectedBranch,
                       invocation,
                       compilation,
                       instanceRegisteredModules,
                       visitedLocals,
                       visitedMethods);
        }

        return (AlwaysThrows(conditional.WhenTrue)
                || TryTrackServiceDescriptorInvocationReturn(
                    conditional.WhenTrue,
                    invocation,
                    compilation,
                    instanceRegisteredModules,
                    CloneVisitedLocals(visitedLocals),
                    CloneVisitedMethods(visitedMethods)))
               && conditional.WhenFalse is { } whenFalse
               && (AlwaysThrows(whenFalse)
                   || TryTrackServiceDescriptorInvocationReturn(
                       whenFalse,
                       invocation,
                       compilation,
                       instanceRegisteredModules,
                       CloneVisitedLocals(visitedLocals),
                       CloneVisitedMethods(visitedMethods)));
    }

    private static bool IsPotentialRegistrationInvocation(
        IInvocationOperation invocation)
    {
        var method = invocation.TargetMethod;
        var definition = (method.ReducedFrom ?? method).OriginalDefinition;
        return IsModuleRegistrationMethod(method)
               || IsServiceDescriptorRegistrationMethod(invocation)
               || (IsDirectServiceRegistrationMethod(definition)
                   && RegistersModuleService(invocation, method));
    }

    private static void CollectServiceCollectionIndexerAssignment(
        OperationAnalysisContext context,
        ConcurrentBag<(ISimpleAssignmentOperation Assignment, IMethodSymbol? ContainingMethod)>
            assignments)
    {
        var assignment = (ISimpleAssignmentOperation) context.Operation;
        if (!IsInReachableBranch(assignment)
            || !IsServiceCollectionIndexerAssignment(assignment))
        {
            return;
        }

        foreach (var containingMethod in
                 GetContainingExecutionMethods(assignment, context.ContainingSymbol))
        {
            assignments.Add((assignment, containingMethod));
        }
    }

    private static bool IsServiceCollectionIndexerAssignment(
        ISimpleAssignmentOperation assignment)
    {
        return assignment.Target is IPropertyReferenceOperation indexer
               && indexer.Property.IsIndexer
               && IsServiceCollectionType(indexer.Instance?.Type)
               && IsServiceDescriptorType(indexer.Property.Type);
    }

    private static bool TryTrackServiceDescriptorCollection(
        IEnumerable<IOperation> elements,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods)
    {
        var tracked = true;
        foreach (var element in elements)
        {
            tracked &= TryTrackServiceDescriptor(
                element,
                compilation,
                instanceRegisteredModules,
                CloneVisitedLocals(visitedLocals),
                CloneVisitedMethods(visitedMethods));
        }

        return tracked;
    }

    private static bool IsServiceDescriptorFactory(IMethodSymbol method)
    {
        return method.Name is "Singleton" or "Scoped" or "Transient" or "Describe"
               && method.ContainingType.ToDisplayString()
               == "Microsoft.Extensions.DependencyInjection.ServiceDescriptor";
    }

    private static bool TrackServiceDescriptor(
        ImmutableArray<IArgumentOperation> arguments,
        ImmutableArray<ITypeSymbol> typeArguments,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules)
    {
        if (!RegistersModuleService(arguments, typeArguments))
        {
            return false;
        }

        if (typeArguments.ElementAtOrDefault(1) is INamedTypeSymbol implementationType)
        {
            instanceRegisteredModules.Add(implementationType.OriginalDefinition);
            return true;
        }

        var implementationTypeArgument = arguments.FirstOrDefault(
            static argument => argument.Parameter?.Name == "implementationType");
        if (implementationTypeArgument is not null
            && TryGetTypeOfNamedTypes(
                implementationTypeArgument.Value,
                compilation,
                out var implementationTypes))
        {
            foreach (var trackedType in implementationTypes)
            {
                instanceRegisteredModules.Add(trackedType.OriginalDefinition);
            }

            return true;
        }

        return arguments
            .Where(static argument => argument.Parameter?.Name is
                "implementationInstance" or "implementationFactory")
            .Any(argument => TryTrackInstanceModuleTypes(
                argument.Value,
                compilation,
                instanceRegisteredModules,
                [with(SymbolEqualityComparer.Default)],
                [with(SymbolEqualityComparer.Default)]));
    }

    private static bool RegistersModuleService(
        IInvocationOperation invocation,
        IMethodSymbol method)
    {
        return RegistersModuleService(invocation.Arguments, method.TypeArguments);
    }

    private static bool RegistersModuleService(
        ImmutableArray<IArgumentOperation> arguments,
        ImmutableArray<ITypeSymbol> typeArguments)
    {
        if (typeArguments.FirstOrDefault()?.ToDisplayString()
            == "ModularPipelines.IModule")
        {
            return true;
        }

        return arguments.Any(argument =>
            argument.Parameter?.Name is "serviceType" or "service"
            && TryGetTypeOfNamedTypes(
                argument.Value,
                out var serviceTypes)
            && serviceTypes.All(serviceType =>
                serviceType.ToDisplayString()
                == "ModularPipelines.IModule"));
    }

    private static bool TryTrackDirectImplementationType(
        IInvocationOperation invocation,
        IMethodSymbol method,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        ConcurrentBag<byte> unresolvedModuleRegistrations)
    {
        if (method.TypeArguments.ElementAtOrDefault(1) is INamedTypeSymbol implementationType)
        {
            instanceRegisteredModules.Add(implementationType.OriginalDefinition);
            return true;
        }

        var implementationTypeArgument = invocation.Arguments.FirstOrDefault(
            static argument => argument.Parameter?.Name == "implementationType");
        if (implementationTypeArgument is null)
        {
            return false;
        }

        if (!TryGetTypeOfNamedTypes(
                implementationTypeArgument.Value,
                compilation,
                out var implementationTypes))
        {
            unresolvedModuleRegistrations.Add(0);
            return true;
        }

        foreach (var trackedType in implementationTypes)
        {
            instanceRegisteredModules.Add(trackedType.OriginalDefinition);
        }

        return true;
    }

    private static void TrackDirectImplementationValue(
        IInvocationOperation invocation,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        ConcurrentBag<byte> unresolvedModuleRegistrations,
        ConcurrentBag<byte> unresolvedFactoryRegistrations)
    {
        var tracked = invocation.Arguments
            .Where(static argument => argument.Parameter?.Name is
                "implementationInstance" or "implementationFactory")
            .Any(argument => TryTrackInstanceModuleTypes(
                argument.Value,
                compilation,
                instanceRegisteredModules,
                [with(SymbolEqualityComparer.Default)],
                [with(SymbolEqualityComparer.Default)]));
        if (!tracked)
        {
            unresolvedModuleRegistrations.Add(0);
            if (invocation.Arguments.Any(static argument =>
                    argument.Parameter?.Name == "implementationFactory"))
            {
                unresolvedFactoryRegistrations.Add(0);
            }
        }
    }

    private static bool TryGetTypeOfNamedTypes(
        IOperation operation,
        out ImmutableArray<INamedTypeSymbol> namedTypes) =>
        TryGetTypeOfNamedTypes(operation, compilation: null, out namedTypes);

    private static bool TryGetTypeOfNamedTypes(
        IOperation operation,
        Compilation? compilation,
        out ImmutableArray<INamedTypeSymbol> namedTypes)
    {
        var types = ImmutableArray.CreateBuilder<INamedTypeSymbol>();
        if (!TryCollectTypeOfNamedTypes(
                operation,
                compilation,
                types,
                [with(SymbolEqualityComparer.Default)],
                [with(SymbolEqualityComparer.Default)]))
        {
            namedTypes = [];
            return false;
        }

        namedTypes = types.ToImmutable();
        return namedTypes.Length > 0;
    }

    private static bool TryCollectTypeOfNamedTypes(
        IOperation operation,
        Compilation? compilation,
        ImmutableArray<INamedTypeSymbol>.Builder types,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<ISymbol> visitedMembers)
    {
        switch (operation)
        {
            case IConversionOperation conversion:
                return TryCollectTypeOfNamedTypes(
                    conversion.Operand,
                    compilation,
                    types,
                    visitedLocals,
                    visitedMembers);
            case ITypeOfOperation { TypeOperand: INamedTypeSymbol typeOperand }:
                types.Add(typeOperand);
                return true;
            case ILocalReferenceOperation localReference
                when visitedLocals.Add(localReference.Local):
                var values = FindReachingLocalValues(
                        operation,
                        localReference.Local)
                    .ToArray();
                return values.Length > 0
                       && values.All(value => TryCollectTypeOfNamedTypes(
                           value,
                           compilation,
                           types,
                           CloneVisitedLocals(visitedLocals),
                           new HashSet<ISymbol>(visitedMembers, SymbolEqualityComparer.Default)));
            case IFieldReferenceOperation fieldReference when compilation is not null:
                return TryCollectTypeOfNamedTypesMember(
                    fieldReference,
                    fieldReference.Field,
                    compilation,
                    types,
                    visitedLocals,
                    visitedMembers);
            case IPropertyReferenceOperation propertyReference when compilation is not null:
                return TryCollectTypeOfNamedTypesMember(
                    propertyReference,
                    propertyReference.Property,
                    compilation,
                    types,
                    visitedLocals,
                    visitedMembers);
            case IConditionalOperation conditional:
                ImmutableArray<IOperation> branches;
                if (conditional.Condition.ConstantValue is
                    { HasValue: true, Value: bool condition })
                {
                    branches = condition
                        ? [conditional.WhenTrue]
                        : conditional.WhenFalse is { } falseBranch
                            ? [falseBranch]
                            : [];
                }
                else
                {
                    branches = conditional.WhenFalse is { } alternative
                        ? [conditional.WhenTrue, alternative]
                        : [conditional.WhenTrue];
                }

                return branches.All(branch => TryCollectTypeOfNamedTypes(
                    branch,
                    compilation,
                    types,
                    CloneVisitedLocals(visitedLocals),
                    new HashSet<ISymbol>(visitedMembers, SymbolEqualityComparer.Default)));
            default:
                return false;
        }
    }

    private static bool TryCollectTypeOfNamedTypesMember(
        IOperation memberReference,
        ISymbol member,
        Compilation compilation,
        ImmutableArray<INamedTypeSymbol>.Builder types,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<ISymbol> visitedMembers)
    {
        if (!visitedMembers.Add(member))
        {
            return false;
        }

        var values = GetMemberValues(memberReference, member, compilation).ToArray();
        return values.Length > 0
               && values.All(value => TryCollectTypeOfNamedTypes(
                   value,
                   compilation,
                   types,
                   CloneVisitedLocals(visitedLocals),
                   new HashSet<ISymbol>(visitedMembers, SymbolEqualityComparer.Default)));
    }

    private static void TrackGenericModuleRegistrations(
        IInvocationOperation invocation,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> registeredModules,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules)
    {
        var method = invocation.TargetMethod;
        // Executing a ModuleTester builder registers only TModule, even when TResult is a module type.
        var typeArguments = IsModuleTesterRegistrationMethod(method.OriginalDefinition)
            ? method.ContainingType.TypeArguments.Take(1)
            : method.TypeArguments;
        foreach (var typeArgument in typeArguments.OfType<INamedTypeSymbol>())
        {
            var normalizedType = typeArgument.OriginalDefinition;
            registeredModules.Add(normalizedType);
            if (method.Name == "AddModule"
                && method.Parameters.Any(static parameter =>
                    parameter.Name is "module" or "factory"))
            {
                instanceRegisteredModules.Add(normalizedType);
            }
        }

        if (method.Name != "AddModule")
        {
            return;
        }

        foreach (var argument in invocation.Arguments.Where(static argument =>
                     argument.Parameter?.Name is "module" or "factory"))
        {
            _ = TryTrackInstanceModuleTypes(
                argument.Value,
                compilation,
                instanceRegisteredModules,
                [with(SymbolEqualityComparer.Default)],
                [with(SymbolEqualityComparer.Default)]);
        }
    }

    private static bool TryTrackInstanceModuleTypes(
        IOperation operation,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods)
    {
        switch (operation)
        {
            case IConversionOperation conversion:
                return TryTrackInstanceModuleTypes(
                    conversion.Operand,
                    compilation,
                    instanceRegisteredModules,
                    visitedLocals,
                    visitedMethods);
            case IDelegateCreationOperation delegateCreation:
                return TryTrackInstanceModuleTypes(
                    delegateCreation.Target,
                    compilation,
                    instanceRegisteredModules,
                    visitedLocals,
                    visitedMethods);
            case IObjectCreationOperation { Type: INamedTypeSymbol moduleType }:
                var normalizedType = moduleType.OriginalDefinition;
                instanceRegisteredModules.Add(normalizedType);
                return true;
            case IInvocationOperation invocation:
                return TryTrackInvocationModuleTypes(
                    invocation,
                    compilation,
                    instanceRegisteredModules,
                    visitedLocals,
                    visitedMethods);
            case ILocalReferenceOperation localReference:
                return TryTrackInstanceModuleTypesLocal(
                    localReference,
                    compilation,
                    instanceRegisteredModules,
                    visitedLocals,
                    visitedMethods);
            case IFieldReferenceOperation fieldReference:
                return TryTrackInstanceModuleTypesMember(
                    fieldReference,
                    fieldReference.Field,
                    compilation,
                    instanceRegisteredModules,
                    visitedLocals,
                    visitedMethods,
                    [with(SymbolEqualityComparer.Default)]);
            case IPropertyReferenceOperation propertyReference:
                return TryTrackInstanceModuleTypesMember(
                    propertyReference,
                    propertyReference.Property,
                    compilation,
                    instanceRegisteredModules,
                    visitedLocals,
                    visitedMethods,
                    [with(SymbolEqualityComparer.Default)]);
            case IAnonymousFunctionOperation anonymousFunction:
                return TryTrackAnonymousFunctionModuleTypes(
                    anonymousFunction,
                    compilation,
                    instanceRegisteredModules,
                    visitedLocals,
                    visitedMethods);
            case IConditionalOperation conditional:
                return TryTrackConditionalModuleTypes(
                    conditional,
                    compilation,
                    instanceRegisteredModules,
                    visitedLocals,
                    visitedMethods);
            case ISwitchExpressionOperation switchExpression:
                return TryTrackSwitchExpressionModuleTypes(
                    switchExpression,
                    compilation,
                    instanceRegisteredModules,
                    visitedLocals,
                    visitedMethods);
            default:
                return false;
        }
    }

    private static bool TryTrackInstanceModuleTypesMember(
        IOperation memberReference,
        ISymbol member,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods,
        HashSet<ISymbol> visitedMembers)
    {
        if (!visitedMembers.Add(member))
        {
            return false;
        }

        var memberValues = GetMemberValues(memberReference, member, compilation).ToArray();
        return memberValues.Length > 0
               && memberValues.All(value => value switch
               {
                   IFieldReferenceOperation fieldReference => TryTrackInstanceModuleTypesMember(
                       fieldReference,
                       fieldReference.Field,
                       compilation,
                       instanceRegisteredModules,
                       CloneVisitedLocals(visitedLocals),
                       CloneVisitedMethods(visitedMethods),
                       new HashSet<ISymbol>(visitedMembers, SymbolEqualityComparer.Default)),
                   IPropertyReferenceOperation propertyReference => TryTrackInstanceModuleTypesMember(
                       propertyReference,
                       propertyReference.Property,
                       compilation,
                       instanceRegisteredModules,
                       CloneVisitedLocals(visitedLocals),
                       CloneVisitedMethods(visitedMethods),
                       new HashSet<ISymbol>(visitedMembers, SymbolEqualityComparer.Default)),
                   _ => TryTrackInstanceModuleTypes(
                       value,
                       compilation,
                       instanceRegisteredModules,
                       CloneVisitedLocals(visitedLocals),
                       CloneVisitedMethods(visitedMethods)),
               });
    }

    private static void CollectStaticFieldInitializationCall(
        OperationAnalysisContext context,
        ConcurrentBag<(IMethodSymbol Caller, IMethodSymbol Callee)> methodCalls)
    {
        var fieldReference = (IFieldReferenceOperation) context.Operation;
        if (!fieldReference.Field.IsStatic
            || fieldReference.Field.IsConst
            || !IsInReachableBranch(fieldReference)
            || !IsInsideReachableNestedCallable(fieldReference))
        {
            return;
        }

        foreach (var containingMethod in
                 GetContainingExecutionMethods(context.ContainingSymbol)
                     .OfType<IMethodSymbol>())
        {
            foreach (var staticConstructor in
                     fieldReference.Field.ContainingType.StaticConstructors)
            {
                methodCalls.Add((
                    NormalizeMethod(containingMethod),
                    NormalizeMethod(staticConstructor)));
            }
        }
    }

    private static bool TryTrackAnonymousFunctionModuleTypes(
        IAnonymousFunctionOperation anonymousFunction,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods)
    {
        var returnValues = anonymousFunction.Body
            .DescendantsAndSelf()
            .OfType<IReturnOperation>()
            .Where(returnOperation =>
                ReferenceEquals(
                    GetEnclosingCallable(returnOperation),
                    anonymousFunction)
                && IsInReachableBranch(returnOperation))
            .Select(static returnOperation => returnOperation.ReturnedValue)
            .OfType<IOperation>()
            .ToArray();
        return returnValues.Length > 0
               && returnValues.All(returnValue =>
                   TryTrackInstanceModuleTypes(
                       returnValue,
                       compilation,
                       instanceRegisteredModules,
                       CloneVisitedLocals(visitedLocals),
                       CloneVisitedMethods(visitedMethods)));
    }

    private static bool TryTrackConditionalModuleTypes(
        IConditionalOperation conditional,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods)
    {
        if (conditional.Condition.ConstantValue is
            { HasValue: true, Value: bool condition })
        {
            var selectedBranch = condition
                ? conditional.WhenTrue
                : conditional.WhenFalse;
            return selectedBranch is not null
                   && TryTrackInstanceModuleTypes(
                       selectedBranch,
                       compilation,
                       instanceRegisteredModules,
                       visitedLocals,
                       visitedMethods);
        }

        return TryTrackInstanceModuleTypes(
                   conditional.WhenTrue,
                   compilation,
                   instanceRegisteredModules,
                   CloneVisitedLocals(visitedLocals),
                   CloneVisitedMethods(visitedMethods))
               && conditional.WhenFalse is { } whenFalse
               && TryTrackInstanceModuleTypes(
                   whenFalse,
                   compilation,
                   instanceRegisteredModules,
                   CloneVisitedLocals(visitedLocals),
                   CloneVisitedMethods(visitedMethods));
    }

    private static bool TryTrackSwitchExpressionModuleTypes(
        ISwitchExpressionOperation switchExpression,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods)
    {
        var reachableArms = switchExpression.Arms
            .Where(arm => !IsDeadSwitchExpressionArm(arm, switchExpression))
            .Where(static arm => !AlwaysThrows(arm.Value))
            .ToArray();
        return reachableArms.Length > 0
               && reachableArms.All(arm => TryTrackInstanceModuleTypes(
                   arm.Value,
                   compilation,
                   instanceRegisteredModules,
                   CloneVisitedLocals(visitedLocals),
                   CloneVisitedMethods(visitedMethods)));
    }

    private static bool TryTrackServiceDescriptorSwitchReturn(
        ISwitchExpressionOperation switchExpression,
        IInvocationOperation invocation,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods)
    {
        var reachableArms = switchExpression.Arms
            .Where(arm => !IsDeadSwitchExpressionArm(arm, switchExpression))
            .Where(static arm => !AlwaysThrows(arm.Value))
            .ToArray();
        return reachableArms.Length > 0
               && reachableArms.All(arm => TryTrackServiceDescriptorInvocationReturn(
                   arm.Value,
                   invocation,
                   compilation,
                   instanceRegisteredModules,
                   CloneVisitedLocals(visitedLocals),
                   CloneVisitedMethods(visitedMethods)));
    }

    private static bool TryTrackInstanceModuleTypesLocal(
        ILocalReferenceOperation localReference,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods)
    {
        if (!visitedLocals.Add(localReference.Local))
        {
            return false;
        }

        var localValues = FindReachingLocalValues(
                localReference,
                localReference.Local)
            .ToArray();
        var trackedAll = localValues.Length > 0;
        foreach (var localValue in localValues)
        {
            trackedAll &= TryTrackInstanceModuleTypes(
                localValue,
                compilation,
                instanceRegisteredModules,
                CloneVisitedLocals(visitedLocals),
                CloneVisitedMethods(visitedMethods));
        }

        return trackedAll;
    }

    private static void TrackDynamicModuleRegistrations(
        IInvocationOperation invocation,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> registeredModules,
        ConcurrentBag<byte> unresolvedModuleRegistrations)
    {
        foreach (var argument in invocation.Arguments.Where(static argument =>
                     argument.Parameter?.Name == "moduleTypes"))
        {
            if (!TryTrackModuleTypes(
                    argument.Value,
                    compilation,
                    registeredModules,
                    [with(SymbolEqualityComparer.Default)],
                    [with(SymbolEqualityComparer.Default)]))
            {
                unresolvedModuleRegistrations.Add(0);
            }
        }
    }

    private static bool TryTrackInvocationModuleTypes(
        IInvocationOperation invocation,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods)
    {
        var method = invocation.TargetMethod.OriginalDefinition;
        if (visitedMethods.Add(method)
            && GetMethodOperation(method, compilation, default) is { } operation)
        {
            var returnValues = operation.DescendantsAndSelf()
                .OfType<IReturnOperation>()
                .Where(static returnOperation =>
                    GetEnclosingCallable(returnOperation) is null
                    && IsInReachableBranch(returnOperation))
                .Select(static returnOperation => returnOperation.ReturnedValue)
                .OfType<IOperation>()
                .ToArray();
            if (returnValues.Length > 0)
            {
                return returnValues.All(returnValue =>
                    TryTrackInvocationModuleTypeReturn(
                        returnValue,
                        invocation,
                        compilation,
                        instanceRegisteredModules,
                        CloneVisitedLocals(visitedLocals),
                        CloneVisitedMethods(visitedMethods)));
            }
        }

        if (invocation.Type is not INamedTypeSymbol moduleType
            || moduleType.TypeKind != TypeKind.Class
            || moduleType.IsAbstract
            || !moduleType.IsModule(compilation))
        {
            return false;
        }

        instanceRegisteredModules.Add(moduleType.OriginalDefinition);
        return true;
    }

    private static bool TryTrackInvocationModuleTypeReturn(
        IOperation returnValue,
        IInvocationOperation invocation,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods)
    {
        return returnValue switch
        {
            IConversionOperation conversion => TryTrackInvocationModuleTypeReturn(
                conversion.Operand,
                invocation,
                compilation,
                instanceRegisteredModules,
                visitedLocals,
                visitedMethods),
            IParameterReferenceOperation parameterReference => TryTrackInvocationModuleTypeParameterReturn(
                parameterReference,
                invocation,
                compilation,
                instanceRegisteredModules,
                visitedLocals,
                visitedMethods),
            IConditionalOperation conditional => TryTrackInvocationModuleTypeConditionalReturn(
                conditional,
                invocation,
                compilation,
                instanceRegisteredModules,
                visitedLocals,
                visitedMethods),
            ISwitchExpressionOperation switchExpression => TryTrackInvocationModuleTypeSwitchReturn(
                switchExpression,
                invocation,
                compilation,
                instanceRegisteredModules,
                visitedLocals,
                visitedMethods),
            _ => TryTrackInstanceModuleTypes(
                returnValue,
                compilation,
                instanceRegisteredModules,
                visitedLocals,
                visitedMethods),
        };
    }

    private static bool TryTrackInvocationModuleTypeParameterReturn(
        IParameterReferenceOperation parameterReference,
        IInvocationOperation invocation,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods)
    {
        var targetMethod = NormalizeMethod(invocation.TargetMethod);
        var argument = invocation.Arguments.FirstOrDefault(candidate =>
            SymbolEqualityComparer.Default.Equals(
                GetTargetParameter(targetMethod, candidate.Parameter),
                parameterReference.Parameter));
        return argument is not null
               && TryTrackInstanceModuleTypes(
                   argument.Value,
                   compilation,
                   instanceRegisteredModules,
                   visitedLocals,
                   visitedMethods);
    }

    private static bool TryTrackInvocationModuleTypeConditionalReturn(
        IConditionalOperation conditional,
        IInvocationOperation invocation,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods)
    {
        if (conditional.Condition.ConstantValue is
            { HasValue: true, Value: bool condition })
        {
            var selectedBranch = condition
                ? conditional.WhenTrue
                : conditional.WhenFalse;
            return selectedBranch is not null
                   && TryTrackInvocationModuleTypeReturn(
                       selectedBranch,
                       invocation,
                       compilation,
                       instanceRegisteredModules,
                       visitedLocals,
                       visitedMethods);
        }

        return TryTrackInvocationModuleTypeReturn(
                   conditional.WhenTrue,
                   invocation,
                   compilation,
                   instanceRegisteredModules,
                   CloneVisitedLocals(visitedLocals),
                   CloneVisitedMethods(visitedMethods))
               && conditional.WhenFalse is { } whenFalse
               && TryTrackInvocationModuleTypeReturn(
                   whenFalse,
                   invocation,
                   compilation,
                   instanceRegisteredModules,
                   CloneVisitedLocals(visitedLocals),
                   CloneVisitedMethods(visitedMethods));
    }

    private static bool TryTrackInvocationModuleTypeSwitchReturn(
        ISwitchExpressionOperation switchExpression,
        IInvocationOperation invocation,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods)
    {
        var reachableArms = switchExpression.Arms
            .Where(arm => !IsDeadSwitchExpressionArm(arm, switchExpression))
            .Where(static arm => !AlwaysThrows(arm.Value))
            .ToArray();
        return reachableArms.Length > 0
               && reachableArms.All(arm => TryTrackInvocationModuleTypeReturn(
                   arm.Value,
                   invocation,
                   compilation,
                   instanceRegisteredModules,
                   CloneVisitedLocals(visitedLocals),
                   CloneVisitedMethods(visitedMethods)));
    }

    private static HashSet<IMethodSymbol> CloneVisitedMethods(
        HashSet<IMethodSymbol> visitedMethods) =>
        new(visitedMethods, visitedMethods.Comparer);

    private static bool TryTrackModuleTypes(
        IOperation operation,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> registeredModules,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<ISymbol> visitedMembers)
    {
        switch (operation)
        {
            case IConversionOperation conversion:
                return TryTrackModuleTypes(
                    conversion.Operand,
                    compilation,
                    registeredModules,
                    visitedLocals,
                    visitedMembers);
            case ITypeOfOperation { TypeOperand: INamedTypeSymbol moduleType }:
                registeredModules.Add(moduleType.OriginalDefinition);
                return true;
            case ILocalReferenceOperation localReference:
                return TryTrackModuleTypesLocal(
                    localReference,
                    compilation,
                    registeredModules,
                    visitedLocals,
                    visitedMembers);
            case IFieldReferenceOperation fieldReference:
                return TryTrackModuleTypesMember(
                    fieldReference,
                    fieldReference.Field,
                    compilation,
                    registeredModules,
                    visitedLocals,
                    visitedMembers);
            case IPropertyReferenceOperation propertyReference:
                return TryTrackModuleTypesMember(
                    propertyReference,
                    propertyReference.Property,
                    compilation,
                    registeredModules,
                    visitedLocals,
                    visitedMembers);
            case IArrayCreationOperation { Initializer: { } initializer }:
                return initializer.ElementValues.All(element =>
                    TryTrackModuleTypes(
                        element,
                        compilation,
                        registeredModules,
                        CloneVisitedLocals(visitedLocals),
                        new HashSet<ISymbol>(visitedMembers, SymbolEqualityComparer.Default)));
            case IArrayCreationOperation { Initializer: null } arrayCreation
                when arrayCreation.DimensionSizes.Length == 1
                     && arrayCreation.DimensionSizes[0].ConstantValue
                         is { HasValue: true, Value: 0 }:
                return true;
            case ICollectionExpressionOperation collection:
                return collection.Elements.All(element =>
                    TryTrackModuleTypes(
                        element,
                        compilation,
                        registeredModules,
                        CloneVisitedLocals(visitedLocals),
                        new HashSet<ISymbol>(visitedMembers, SymbolEqualityComparer.Default)));
            case ISpreadOperation spread:
                return TryTrackModuleTypes(
                    spread.Operand,
                    compilation,
                    registeredModules,
                    visitedLocals,
                    visitedMembers);
            case IConditionalOperation or ISwitchExpressionOperation:
                var reachableValues = GetReachableValueLeaves(operation)
                    .Where(static value => !AlwaysThrows(value))
                    .ToArray();
                return reachableValues.Length > 0
                       && reachableValues.All(value => TryTrackModuleTypes(
                           value,
                           compilation,
                           registeredModules,
                           CloneVisitedLocals(visitedLocals),
                           new HashSet<ISymbol>(visitedMembers, SymbolEqualityComparer.Default)));
            case IInvocationOperation invocation
                when invocation.TargetMethod.Name == "Empty"
                     && invocation.TargetMethod.ContainingType.OriginalDefinition
                         .ToDisplayString() == "System.Array":
                return true;
            default:
                return false;
        }
    }

    private static void TrackScannedAssemblies(
        IInvocationOperation invocation,
        Compilation compilation,
        IAssemblySymbol currentAssembly,
        bool isApplication,
        ConcurrentBag<IAssemblySymbol> scannedAssemblies)
    {
        foreach (var typeArgument in invocation.TargetMethod.TypeArguments)
        {
            if (typeArgument is INamedTypeSymbol namedType)
            {
                scannedAssemblies.Add(namedType.ContainingAssembly);
            }
        }

        foreach (var argument in invocation.Arguments.Where(static argument =>
                     argument.Parameter?.Type.ToDisplayString()
                     == AssemblyMetadataName))
        {
            _ = TryTrackScannedAssembly(
                argument.Value,
                compilation,
                currentAssembly,
                isApplication,
                scannedAssemblies,
                [with(SymbolEqualityComparer.Default)],
                [with(SymbolEqualityComparer.Default)]);
        }
    }

    private static bool TryTrackModuleTypesLocal(
        ILocalReferenceOperation localReference,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> registeredModules,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<ISymbol> visitedMembers)
    {
        if (!visitedLocals.Add(localReference.Local))
        {
            return false;
        }

        var localValues = FindReachingLocalValues(
                localReference,
                localReference.Local)
            .ToArray();
        var trackedAll = localValues.Length > 0;
        foreach (var localValue in localValues)
        {
            trackedAll &= TryTrackModuleTypes(
                localValue,
                compilation,
                registeredModules,
                CloneVisitedLocals(visitedLocals),
                new HashSet<ISymbol>(visitedMembers, SymbolEqualityComparer.Default));
        }

        return trackedAll;
    }

    private static bool TryTrackScannedAssembly(
        IOperation operation,
        Compilation compilation,
        IAssemblySymbol currentAssembly,
        bool isApplication,
        ConcurrentBag<IAssemblySymbol> scannedAssemblies,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<ISymbol> visitedMembers)
    {
        switch (operation)
        {
            case IConversionOperation conversion:
                return TryTrackScannedAssembly(
                    conversion.Operand,
                    compilation,
                    currentAssembly,
                    isApplication,
                    scannedAssemblies,
                    visitedLocals,
                    visitedMembers);
            case ILocalReferenceOperation localReference:
                return TryTrackScannedAssemblyLocal(
                    localReference,
                    compilation,
                    currentAssembly,
                    isApplication,
                    scannedAssemblies,
                    visitedLocals,
                    visitedMembers);
            case IFieldReferenceOperation fieldReference:
                return TryTrackScannedAssemblyMember(
                    fieldReference,
                    fieldReference.Field,
                    compilation,
                    currentAssembly,
                    isApplication,
                    scannedAssemblies,
                    visitedLocals,
                    visitedMembers);
            case IPropertyReferenceOperation
            {
                Property.Name: "Assembly",
                Instance: ITypeOfOperation typeOfOperation,
            }

                propertyReference
                when propertyReference.Property.ContainingType.ToDisplayString()
                     == "System.Type"
                     && typeOfOperation.TypeOperand is INamedTypeSymbol namedType:
                scannedAssemblies.Add(namedType.ContainingAssembly);
                return true;
            case IPropertyReferenceOperation propertyReference:
                return TryTrackScannedAssemblyMember(
                    propertyReference,
                    propertyReference.Property,
                    compilation,
                    currentAssembly,
                    isApplication,
                    scannedAssemblies,
                    visitedLocals,
                    visitedMembers);
            case IConditionalOperation or ISwitchExpressionOperation:
                var reachableValues = GetReachableValueLeaves(operation)
                    .Where(static value => !AlwaysThrows(value))
                    .ToArray();
                return reachableValues.Length > 0
                       && reachableValues.All(value => TryTrackScannedAssembly(
                           value,
                           compilation,
                           currentAssembly,
                           isApplication,
                           scannedAssemblies,
                           CloneVisitedLocals(visitedLocals),
                           new HashSet<ISymbol>(visitedMembers, SymbolEqualityComparer.Default)));
            case IInvocationOperation invocation:
                return TryTrackScannedAssemblyInvocation(
                    invocation,
                    currentAssembly,
                    isApplication,
                    scannedAssemblies);
            default:
                return false;
        }
    }

    private static bool TryTrackScannedAssemblyLocal(
        ILocalReferenceOperation localReference,
        Compilation compilation,
        IAssemblySymbol currentAssembly,
        bool isApplication,
        ConcurrentBag<IAssemblySymbol> scannedAssemblies,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<ISymbol> visitedMembers)
    {
        if (!visitedLocals.Add(localReference.Local))
        {
            return false;
        }

        var localValues = FindReachingLocalValues(
                localReference,
                localReference.Local)
            .ToArray();
        var trackedAll = localValues.Length > 0;
        foreach (var localValue in localValues)
        {
            trackedAll &= TryTrackScannedAssembly(
                localValue,
                compilation,
                currentAssembly,
                isApplication,
                scannedAssemblies,
                CloneVisitedLocals(visitedLocals),
                new HashSet<ISymbol>(visitedMembers, SymbolEqualityComparer.Default));
        }

        return trackedAll;
    }

    private static bool TryTrackModuleTypesMember(
        IOperation memberReference,
        ISymbol member,
        Compilation compilation,
        ConcurrentBag<INamedTypeSymbol> registeredModules,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<ISymbol> visitedMembers)
    {
        if (!visitedMembers.Add(member))
        {
            return false;
        }

        var memberValues = GetMemberValues(memberReference, member, compilation).ToArray();
        return memberValues.Length > 0
               && memberValues.All(value => TryTrackModuleTypes(
                   value,
                   compilation,
                   registeredModules,
                   CloneVisitedLocals(visitedLocals),
                   new HashSet<ISymbol>(visitedMembers, SymbolEqualityComparer.Default)));
    }

    private static bool TryTrackScannedAssemblyMember(
        IOperation memberReference,
        ISymbol member,
        Compilation compilation,
        IAssemblySymbol currentAssembly,
        bool isApplication,
        ConcurrentBag<IAssemblySymbol> scannedAssemblies,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<ISymbol> visitedMembers)
    {
        if (!visitedMembers.Add(member))
        {
            return false;
        }

        var memberValues = GetMemberValues(memberReference, member, compilation).ToArray();
        return memberValues.Length > 0
               && memberValues.All(value => TryTrackScannedAssembly(
                   value,
                   compilation,
                   currentAssembly,
                   isApplication,
                   scannedAssemblies,
                   CloneVisitedLocals(visitedLocals),
                   new HashSet<ISymbol>(visitedMembers, SymbolEqualityComparer.Default)));
    }

    private static bool TryTrackScannedAssemblyInvocation(
        IInvocationOperation invocation,
        IAssemblySymbol currentAssembly,
        bool isApplication,
        ConcurrentBag<IAssemblySymbol> scannedAssemblies)
    {
        if (invocation.TargetMethod.ContainingType.ToDisplayString()
            != AssemblyMetadataName)
        {
            return false;
        }

        if (invocation.TargetMethod.Name == "GetExecutingAssembly")
        {
            scannedAssemblies.Add(currentAssembly);
            return true;
        }

        if (invocation.TargetMethod.Name == "GetEntryAssembly")
        {
            if (isApplication)
            {
                scannedAssemblies.Add(currentAssembly);
            }

            return true;
        }

        if (invocation.TargetMethod.Name == "GetAssembly"
            && invocation.Arguments.FirstOrDefault()?.Value is
                ITypeOfOperation
            {
                TypeOperand: INamedTypeSymbol namedType,
            })
        {
            scannedAssemblies.Add(namedType.ContainingAssembly);
            return true;
        }

        return false;
    }

    private static void TrackReachableRegistrations(
        CompilationAnalysisContext context,
        ConcurrentBag<(IInvocationOperation Invocation, IMethodSymbol? ContainingMethod)>
            registrationInvocations,
        ConcurrentBag<(ISimpleAssignmentOperation Assignment, IMethodSymbol? ContainingMethod)>
            indexerAssignments,
        ConcurrentBag<(IMethodSymbol Caller, IMethodSymbol Callee)> methodCalls,
        ConcurrentBag<INamedTypeSymbol> registeredModules,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        ConcurrentBag<IAssemblySymbol> scannedAssemblies,
        ConcurrentBag<byte> unresolvedModuleRegistrations,
        ConcurrentBag<byte> unresolvedFactoryRegistrations)
    {
        var reachableMethods = GetReachableStartupMethods(
            context.Compilation,
            registrationInvocations.Select(static item => item.ContainingMethod)
                .Concat(indexerAssignments.Select(static item => item.ContainingMethod)),
            methodCalls);

        foreach (var (invocation, containingMethod) in registrationInvocations)
        {
            if (!IsReachableStartupMethod(containingMethod, reachableMethods))
            {
                continue;
            }

            TrackRegistrationInvocation(
                invocation,
                context.Compilation,
                context.Compilation.Assembly,
                IsApplication(context.Compilation.Options.OutputKind),
                registeredModules,
                instanceRegisteredModules,
                scannedAssemblies,
                unresolvedModuleRegistrations,
                unresolvedFactoryRegistrations);
        }

        foreach (var (assignment, containingMethod) in indexerAssignments)
        {
            if (!IsReachableStartupMethod(containingMethod, reachableMethods))
            {
                continue;
            }

            if (!TryTrackServiceDescriptor(
                    assignment.Value,
                    context.Compilation,
                    instanceRegisteredModules,
                    [with(SymbolEqualityComparer.Default)],
                    [with(SymbolEqualityComparer.Default)])
                && IsUnresolvedModuleServiceDescriptor(assignment.Value))
            {
                unresolvedModuleRegistrations.Add(0);
            }
        }
    }

    private static ImmutableHashSet<IMethodSymbol> GetReachableStartupMethods(
        Compilation compilation,
        IEnumerable<IMethodSymbol?> registrationMethods,
        IEnumerable<(IMethodSymbol Caller, IMethodSymbol Callee)> methodCalls)
    {
        var comparer = SymbolEqualityComparer.Default;
        var directCalls = methodCalls.ToImmutableArray();
        var directCandidateMethods = registrationMethods
            .OfType<IMethodSymbol>()
            .Select(NormalizeMethod)
            .Concat(directCalls.SelectMany(static call => new[] { call.Caller, call.Callee }))
            .Distinct<IMethodSymbol>(comparer)
            .ToImmutableArray();
        var calls = directCalls
            .Concat(directCandidateMethods.SelectMany(static method =>
                method.MethodKind == MethodKind.StaticConstructor
                    ? []
                    : method.ContainingType.StaticConstructors.Select(staticConstructor =>
                        (Caller: method, Callee: NormalizeMethod(staticConstructor)))))
            .ToImmutableArray();
        var candidateMethods = directCandidateMethods
            .Concat(calls.SelectMany(static call => new[] { call.Caller, call.Callee }))
            .Distinct<IMethodSymbol>(comparer)
            .ToImmutableArray();
        var entryPoint = compilation.GetEntryPoint(default);
        var reachable = new HashSet<IMethodSymbol>(comparer);
        var pending = new Queue<IMethodSymbol>();

        foreach (var method in candidateMethods.Where(method =>
                     IsStartupRoot(method, entryPoint)))
        {
            if (reachable.Add(method))
            {
                pending.Enqueue(method);
            }
        }

        var callsByCaller = calls.ToLookup(
            static call => call.Caller,
            static call => call.Callee,
            comparer);
        while (pending.Count > 0)
        {
            foreach (var callee in callsByCaller[pending.Dequeue()])
            {
                if (reachable.Add(callee))
                {
                    pending.Enqueue(callee);
                }
            }
        }

        return ImmutableHashSet.CreateRange<IMethodSymbol>(comparer, reachable);
    }

    private static bool IsStartupRoot(
        IMethodSymbol method,
        IMethodSymbol? entryPoint)
    {
        return (entryPoint is not null
                && SymbolEqualityComparer.Default.Equals(
                    method,
                    NormalizeMethod(entryPoint)))
               || (method.MethodKind != MethodKind.Constructor
                   && method.DeclaredAccessibility is not
                       (Accessibility.Private or Accessibility.NotApplicable));
    }

    private static bool IsReachableStartupMethod(
        IMethodSymbol? method,
        ImmutableHashSet<IMethodSymbol> reachableMethods)
    {
        return method is null || reachableMethods.Contains(NormalizeMethod(method));
    }

    private static IMethodSymbol NormalizeMethod(IMethodSymbol method)
    {
        return (method.ReducedFrom ?? method).OriginalDefinition;
    }

    private static IMethodSymbol? GetContainingMethod(ISymbol symbol)
    {
        for (var current = symbol; current is not null; current = current.ContainingSymbol)
        {
            if (current is IMethodSymbol method)
            {
                return method;
            }
        }

        return null;
    }

    private static ImmutableArray<IMethodSymbol?> GetContainingExecutionMethods(
        ISymbol symbol)
    {
        if (GetContainingMethod(symbol) is { } method)
        {
            return [method];
        }

        for (var current = symbol; current is not null; current = current.ContainingSymbol)
        {
            var initializer = current switch
            {
                IFieldSymbol field => (field.IsStatic, field.ContainingType),
                IPropertySymbol property => (property.IsStatic, property.ContainingType),
                IEventSymbol @event => (@event.IsStatic, @event.ContainingType),
                _ => ((bool IsStatic, INamedTypeSymbol Type)?) null,
            };
            if (initializer is not { } initializerOwner)
            {
                continue;
            }

            var constructors = initializerOwner.IsStatic
                ? initializerOwner.Type.StaticConstructors
                : initializerOwner.Type.InstanceConstructors;
            return [.. constructors.Cast<IMethodSymbol?>()];
        }

        return [null];
    }

    private static ImmutableArray<IMethodSymbol?> GetContainingExecutionMethods(
        IOperation operation,
        ISymbol containingSymbol)
    {
        return GetCallableSymbol(GetEnclosingCallable(operation)) is { } callable
            ? [callable]
            : GetContainingExecutionMethods(containingSymbol);
    }

    private static bool IsInReachableBranch(IOperation operation)
    {
        for (var current = operation; current.Parent is { } parent; current = parent)
        {
            if (IsDeadBranch(current, parent))
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsDeadBranch(IOperation current, IOperation parent)
    {
        return parent switch
        {
            IConditionalOperation conditional =>
                IsDeadConditionalBranch(current, conditional),
            IWhileLoopOperation whileLoop => IsDeadWhileBody(current, whileLoop),
            IForLoopOperation forLoop => IsDeadForBody(current, forLoop),
            ISwitchOperation switchOperation =>
                IsDeadSwitchCase(current, switchOperation),
            ISwitchExpressionOperation switchExpression =>
                IsDeadSwitchExpressionArm(current, switchExpression),
            _ => false,
        };
    }

    private static bool IsDeadConditionalBranch(
        IOperation current,
        IConditionalOperation conditional)
    {
        var constantValue = conditional.Condition.ConstantValue;
        return constantValue.HasValue
               && constantValue.Value is bool condition
               && ((ReferenceEquals(current, conditional.WhenTrue) && !condition)
                   || (ReferenceEquals(current, conditional.WhenFalse) && condition));
    }

    private static bool IsDeadWhileBody(
        IOperation current,
        IWhileLoopOperation whileLoop)
    {
        var constantValue = whileLoop.Condition?.ConstantValue;
        return whileLoop.ConditionIsTop
               && constantValue is { HasValue: true, Value: false }
               && ReferenceEquals(current, whileLoop.Body);
    }

    private static bool IsDeadForBody(
        IOperation current,
        IForLoopOperation forLoop)
    {
        return forLoop.Condition?.ConstantValue
                   is { HasValue: true, Value: false }
               && ReferenceEquals(current, forLoop.Body);
    }

    private static bool IsDeadSwitchCase(
        IOperation current,
        ISwitchOperation switchOperation)
    {
        if (current is not ISwitchCaseOperation switchCase
            || switchOperation.Value.ConstantValue
                is not { HasValue: true } switchValue)
        {
            return false;
        }

        var caseMatch = SwitchCaseMatchesConstant(switchCase, switchValue.Value);
        if (caseMatch is true)
        {
            return false;
        }

        var matchingCase = switchOperation.Cases.FirstOrDefault(@case =>
            SwitchCaseMatchesConstant(@case, switchValue.Value) is true);
        if (matchingCase is not null
            && switchCase.Clauses.Any(static clause =>
                clause is IDefaultCaseClauseOperation))
        {
            return true;
        }

        return caseMatch is false;
    }

    private static bool IsDeadSwitchExpressionArm(
        IOperation current,
        ISwitchExpressionOperation switchExpression)
    {
        if (current is not ISwitchExpressionArmOperation arm
            || switchExpression.Value.ConstantValue
                is not { HasValue: true } switchValue)
        {
            return false;
        }

        if (PatternAndGuardMatchesConstant(
                arm.Pattern,
                arm.Guard,
                switchValue.Value) is false)
        {
            return true;
        }

        return switchExpression.Arms
            .TakeWhile(candidate => !ReferenceEquals(candidate, arm))
            .Any(candidate => PatternAndGuardMatchesConstant(
                candidate.Pattern,
                candidate.Guard,
                switchValue.Value) is true);
    }

    private static bool? SwitchCaseMatchesConstant(
        ISwitchCaseOperation switchCase,
        object? switchValue)
    {
        var hasUnknownClause = false;
        foreach (var clause in switchCase.Clauses)
        {
            var matches = SwitchClauseMatchesConstant(clause, switchValue);
            if (matches is true)
            {
                return true;
            }

            hasUnknownClause |= matches is null;
        }

        return hasUnknownClause ? null : false;
    }

    private static bool? SwitchClauseMatchesConstant(
        ICaseClauseOperation clause,
        object? switchValue)
    {
        return clause switch
        {
            ISingleValueCaseClauseOperation singleValueClause =>
                ConstantEquals(singleValueClause.Value, switchValue),
            IPatternCaseClauseOperation patternClause =>
                PatternClauseMatchesConstant(patternClause, switchValue),
            IDefaultCaseClauseOperation => null,
            _ => null,
        };
    }

    private static bool? PatternClauseMatchesConstant(
        IPatternCaseClauseOperation clause,
        object? switchValue) =>
        PatternAndGuardMatchesConstant(
            clause.Pattern,
            clause.Guard,
            switchValue);

    private static bool? PatternAndGuardMatchesConstant(
        IPatternOperation pattern,
        IOperation? guard,
        object? switchValue)
    {
        var patternMatches = PatternMatchesConstant(pattern, switchValue);
        if (patternMatches is false
            || guard?.ConstantValue is { HasValue: true, Value: false })
        {
            return false;
        }

        if (patternMatches is true
            && (guard is null
                || guard.ConstantValue is { HasValue: true, Value: true }))
        {
            return true;
        }

        return null;
    }

    private static bool? PatternMatchesConstant(
        IPatternOperation pattern,
        object? switchValue)
    {
        return pattern switch
        {
            IConstantPatternOperation constantPattern =>
                ConstantEquals(constantPattern.Value, switchValue),
            IRelationalPatternOperation relationalPattern =>
                RelationalPatternMatchesConstant(relationalPattern, switchValue),
            IBinaryPatternOperation binaryPattern =>
                BinaryPatternMatchesConstant(binaryPattern, switchValue),
            INegatedPatternOperation negatedPattern =>
                Negate(PatternMatchesConstant(negatedPattern.Pattern, switchValue)),
            _ => null,
        };
    }

    private static bool? BinaryPatternMatchesConstant(
        IBinaryPatternOperation pattern,
        object? switchValue)
    {
        var left = PatternMatchesConstant(pattern.LeftPattern, switchValue);
        var right = PatternMatchesConstant(pattern.RightPattern, switchValue);
        return pattern.OperatorKind switch
        {
            BinaryOperatorKind.And or BinaryOperatorKind.ConditionalAnd =>
                left & right,
            BinaryOperatorKind.Or or BinaryOperatorKind.ConditionalOr =>
                left | right,
            _ => null,
        };
    }

    private static bool? Negate(bool? value) =>
        value.HasValue ? !value.Value : null;

    private static bool? ConstantEquals(
        IOperation operation,
        object? switchValue)
    {
        return operation.ConstantValue is { HasValue: true } constant
            ? Equals(constant.Value, switchValue)
            : null;
    }

    private static bool? RelationalPatternMatchesConstant(
        IRelationalPatternOperation pattern,
        object? switchValue)
    {
        if (switchValue is not IComparable comparable
            || pattern.Value.ConstantValue is not { HasValue: true, Value: { } patternValue }
            || switchValue.GetType() != patternValue.GetType())
        {
            return null;
        }

        var comparison = comparable.CompareTo(patternValue);
        return pattern.OperatorKind switch
        {
            BinaryOperatorKind.GreaterThan => comparison > 0,
            BinaryOperatorKind.GreaterThanOrEqual => comparison >= 0,
            BinaryOperatorKind.LessThan => comparison < 0,
            BinaryOperatorKind.LessThanOrEqual => comparison <= 0,
            _ => null,
        };
    }

    private static void ReportModuleDiagnostics(
        CompilationAnalysisContext context,
        ConcurrentBag<INamedTypeSymbol> modules,
        ConcurrentBag<INamedTypeSymbol> registeredModules,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        ConcurrentBag<IAssemblySymbol> scannedAssemblies,
        ConcurrentBag<byte> unresolvedModuleRegistrations,
        ConcurrentBag<byte> unresolvedFactoryRegistrations)
    {
        var moduleSet = modules
            .Distinct<INamedTypeSymbol>(SymbolEqualityComparer.Default)
            .ToImmutableArray();
        var instanceRegistered = instanceRegisteredModules.ToImmutableHashSet<INamedTypeSymbol>(
            SymbolEqualityComparer.Default);
        var scanned = scannedAssemblies.ToImmutableHashSet<IAssemblySymbol>(
            SymbolEqualityComparer.Default);
        foreach (var module in moduleSet.Where(module =>
                     unresolvedFactoryRegistrations.IsEmpty
                     && !IsPublic(module)
                     && !instanceRegistered.Contains(module)
                     && !IsAssemblyScannedModule(module, scanned)))
        {
            ReportModuleDiagnostic(
                context,
                module,
                ModuleRegistrationAnalyzer.NonPublicModuleRule);
        }

        // A runtime-computed Type[] can register any module in this compilation,
        // but it cannot change whether a module declaration is public.
        if (!unresolvedModuleRegistrations.IsEmpty)
        {
            return;
        }

        if (!IsApplication(context.Compilation.Options.OutputKind))
        {
            return;
        }

        var registered = new HashSet<INamedTypeSymbol>(
            registeredModules,
            SymbolEqualityComparer.Default);
        foreach (var module in moduleSet.Where(module =>
                     IsAssemblyScannedModule(module, scanned)))
        {
            registered.Add(module);
        }

        AddRequiredDependencyClosure(registered, context.Compilation);
        foreach (var module in moduleSet)
        {
            if (registered.Contains(module)
                || instanceRegistered.Contains(module))
            {
                continue;
            }

            ReportModuleDiagnostic(
                context,
                module,
                ModuleRegistrationAnalyzer.UnregisteredModuleRule);
        }
    }

    private static void AddRequiredDependencyClosure(
        HashSet<INamedTypeSymbol> registered,
        Compilation compilation)
    {
        var pending = new Queue<INamedTypeSymbol>(registered);
        while (pending.Count > 0)
        {
            var module = pending.Dequeue();
            foreach (var dependency in module.GetAllAttributesIncludingBaseAndInterfaces()
                         .Where(attribute => !IsOptionalDependency(attribute))
                         .Select(attribute => GetDependencyType(attribute, compilation))
                         .OfType<INamedTypeSymbol>())
            {
                var normalizedDependency = dependency.OriginalDefinition;
                if (registered.Add(normalizedDependency))
                {
                    pending.Enqueue(normalizedDependency);
                }
            }
        }
    }

    private static bool IsOptionalDependency(AttributeData attribute)
    {
        return attribute.NamedArguments.Any(static argument =>
            argument.Key == "Optional" && argument.Value.Value is true);
    }

    private static void ReportModuleDiagnostic(
        CompilationAnalysisContext context,
        INamedTypeSymbol module,
        DiagnosticDescriptor rule)
    {
        var location = module.Locations.FirstOrDefault(static item => item.IsInSource);
        if (location is not null)
        {
            context.ReportDiagnostic(Diagnostic.Create(rule, location, module.Name));
        }
    }

    private static bool IsApplication(OutputKind outputKind)
    {
        return outputKind is OutputKind.ConsoleApplication
            or OutputKind.WindowsApplication
            or OutputKind.WindowsRuntimeApplication;
    }

    private static bool IsOpenGenericType(INamedTypeSymbol type)
    {
        for (var current = type; current is not null; current = current.ContainingType)
        {
            if (current.Arity > 0)
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsAssemblyScannedModule(
        INamedTypeSymbol module,
        ImmutableHashSet<IAssemblySymbol> scannedAssemblies)
    {
        return scannedAssemblies.Contains(module.ContainingAssembly)
               && !IsOpenGenericType(module);
    }

    private static void AnalyzeDuplicateDependencies(SymbolAnalysisContext context)
    {
        var module = (INamedTypeSymbol) context.Symbol;
        if (!module.IsModule(context.Compilation))
        {
            return;
        }

        var attributes = GetInheritedAttributes(module)
            .Select(static attribute => (Attribute: attribute, IsInherited: true))
            .Concat(module.GetAttributes()
                .Select(static attribute => (Attribute: attribute, IsInherited: false)));
        var dependencies = attributes
            .Select(item => new
            {
                item.Attribute,
                item.IsInherited,
                Type = GetDependencyType(item.Attribute, context.Compilation),
            })
            .Where(static item => item.Type is not null)
            .GroupBy(static item => item.Type!, SymbolEqualityComparer.Default);

        foreach (var duplicates in dependencies.Where(static group => group.Skip(1).Any()))
        {
            // Keep a required declaration when one exists so removing reported
            // duplicates cannot make the effective dependency optional.
            foreach (var duplicate in duplicates
                         .OrderBy(static item => IsOptionalDependency(item.Attribute))
                         .Skip(1)
                         .Where(static item => !item.IsInherited))
            {
                var location = duplicate.Attribute.ApplicationSyntaxReference?.GetSyntax(
                    context.CancellationToken).GetLocation();
                if (location is not null)
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        DuplicateDependsOnAnalyzer.Rule,
                        location,
                        module.Name,
                        duplicates.Key!.Name));
                }
            }
        }
    }

    private static IEnumerable<AttributeData> GetInheritedAttributes(INamedTypeSymbol module)
    {
        foreach (var attribute in module.AllInterfaces.SelectMany(
                     static interfaceType => interfaceType.GetAttributes()))
        {
            yield return attribute;
        }

        for (var baseType = module.BaseType; baseType is not null; baseType = baseType.BaseType)
        {
            foreach (var attribute in baseType.GetAttributes())
            {
                yield return attribute;
            }
        }
    }

    private static ITypeSymbol? GetDependencyType(
        AttributeData attribute,
        Compilation compilation)
    {
        if (attribute.AttributeClass is not { } attributeClass
            || !attributeClass.IsDependsOnAttribute(compilation))
        {
            return null;
        }

        return attributeClass.IsGenericType
            ? attributeClass.TypeArguments[0]
            : attribute.ConstructorArguments.FirstOrDefault().Value as ITypeSymbol;
    }

    private static IMethodSymbol? GetCancellationExecutionMethod(OperationAnalysisContext context)
    {
        var roots = AdditionalCancellationRoots.GetValue(
            context.Compilation,
            static compilation => new Lazy<ImmutableDictionary<IMethodSymbol, ImmutableHashSet<IMethodSymbol>>>(
                () => GetAdditionalCancellationRoots(compilation))).Value;
        var nestedCallables = GetEnclosingNestedCallables(context.Operation);

        var methods = nestedCallables.Concat(
            context.ContainingSymbol is IMethodSymbol method ? [method] : []);
        foreach (var candidate in methods)
        {
            foreach (var root in roots)
            {
                if (!root.Value.Contains(candidate))
                {
                    continue;
                }

                var remainingCallables = nestedCallables.Where(callable =>
                        !SymbolEqualityComparer.Default.Equals(callable, root.Key))
                    .ToList();
                if (NestedCallablesAreInvoked(context.Operation, remainingCallables, root.Key))
                {
                    return root.Key;
                }
            }
        }

        return GetModuleExecutionMethod(context);
    }

    private static ImmutableDictionary<IMethodSymbol, ImmutableHashSet<IMethodSymbol>>
        GetAdditionalCancellationRoots(Compilation compilation)
    {
        var roots = new HashSet<IMethodSymbol>(SymbolEqualityComparer.Default);
        foreach (var method in GetNamedTypes(compilation.Assembly.GlobalNamespace)
                     .SelectMany(static type => type.GetMembers().OfType<IMethodSymbol>()))
        {
            if (IsModuleLifecycleHook(method, compilation))
            {
                roots.Add(method);
            }
        }

        roots.UnionWith(GetConfigurationCancellationRoots(compilation));

        var result = ImmutableDictionary.CreateBuilder<IMethodSymbol, ImmutableHashSet<IMethodSymbol>>(
            SymbolEqualityComparer.Default);
        foreach (var root in roots)
        {
            result.Add(root, GetReachableMemberMethods(root, root.ContainingType, compilation, default));
        }

        return result.ToImmutable();
    }

    private static IEnumerable<IMethodSymbol> GetConfigurationCancellationRoots(Compilation compilation)
    {
        var builderType = compilation.GetTypeByMetadataName("ModularPipelines.ModuleConfigurationBuilder");
        if (builderType is null)
        {
            yield break;
        }

        var configurationMethodNames = builderType.GetMembers()
            .OfType<IMethodSymbol>()
            .Where(static method => method.Parameters.Any(parameter =>
                IsCancellationConfigurationDelegate(parameter.Type)))
            .Select(static method => method.Name)
            .ToImmutableHashSet(StringComparer.Ordinal);
        foreach (var syntaxTree in compilation.SyntaxTrees)
        {
            foreach (var invocation in GetConfigurationInvocations(compilation, syntaxTree, builderType, configurationMethodNames))
            {
                foreach (var callback in GetCancellationConfigurationCallbacks(invocation, compilation))
                {
                    yield return callback;
                }
            }
        }
    }

    private static IEnumerable<IInvocationOperation> GetConfigurationInvocations(
        Compilation compilation,
        SyntaxTree syntaxTree,
        INamedTypeSymbol builderType,
        ImmutableHashSet<string> configurationMethodNames)
    {
        var model = compilation.GetSemanticModel(syntaxTree);
        foreach (var syntax in syntaxTree.GetRoot().DescendantNodes().OfType<InvocationExpressionSyntax>())
        {
            var name = syntax.Expression switch
            {
                MemberAccessExpressionSyntax member => member.Name.Identifier.ValueText,
                MemberBindingExpressionSyntax member => member.Name.Identifier.ValueText,
                SimpleNameSyntax simpleName => simpleName.Identifier.ValueText,
                _ => null,
            };
            if (name is not null
                && configurationMethodNames.Contains(name)
                && model.GetOperation(syntax) is IInvocationOperation invocation
                && SymbolEqualityComparer.Default.Equals(invocation.TargetMethod.ContainingType, builderType)
                && IsInReachableBranch(invocation))
            {
                yield return invocation;
            }
        }
    }

    private static IEnumerable<IMethodSymbol> GetCancellationConfigurationCallbacks(
        IInvocationOperation invocation,
        Compilation compilation)
    {
        foreach (var argument in invocation.Arguments)
        {
            if (!IsCancellationConfigurationDelegate(argument.Parameter?.Type))
            {
                continue;
            }

            foreach (var value in GetValueAndReachingCallbackValues(
                         argument.Value,
                         compilation,
                         [with(SymbolEqualityComparer.Default)],
                         [with(SymbolEqualityComparer.Default)],
                         [with(SymbolEqualityComparer.Default)]))
            {
                foreach (var callback in GetConfigurationCallbackTargets(value)
                             .Where(static callback => callback.Parameters.Any(IsCancellationToken)
                                 && IsTaskLike(callback.ReturnType)))
                {
                    yield return callback;
                }
            }
        }
    }

    private static bool IsCancellationConfigurationDelegate(ITypeSymbol? type)
    {
        if (type is IArrayTypeSymbol arrayType)
        {
            type = arrayType.ElementType;
        }

        return type is INamedTypeSymbol { DelegateInvokeMethod: { } invokeMethod }
               && invokeMethod.Parameters.Any(IsCancellationToken)
               && IsTaskLike(invokeMethod.ReturnType);
    }

    private static IEnumerable<IMethodSymbol> GetConfigurationCallbackTargets(IOperation value)
    {
        if (GetDirectDelegateTarget(value) is { } callback)
        {
            yield return callback;
            yield break;
        }

        foreach (var child in value.ChildOperations)
        {
            foreach (var target in GetConfigurationCallbackTargets(child))
            {
                yield return target;
            }
        }
    }

    private static bool IsModuleLifecycleHook(IMethodSymbol method, Compilation compilation)
    {
        if (!method.IsOverride
            || method.Name is not ("OnBeforeExecuteAsync" or "OnAfterExecuteAsync"
                or "OnSkippedAsync" or "OnCachedResultAsync" or "OnFailedAsync"))
        {
            return false;
        }

        var moduleType = compilation.GetTypeByMetadataName(AnalyzerConstants.FullyQualifiedTypeNames.Module);
        for (var overridden = method.OverriddenMethod;
             overridden is not null;
             overridden = overridden.OverriddenMethod)
        {
            if (SymbolEqualityComparer.Default.Equals(overridden.ContainingType.OriginalDefinition, moduleType))
            {
                return true;
            }
        }

        return false;
    }

    private static IMethodSymbol? GetModuleExecutionMethod(OperationAnalysisContext context)
    {
        var nestedCallables = GetEnclosingNestedCallables(context.Operation);

        for (var method = context.ContainingSymbol as IMethodSymbol;
             method is not null;
             method = method.ContainingSymbol as IMethodSymbol)
        {
            if (IsModuleExecuteAsync(method, context.Compilation))
            {
                return NestedCallablesAreInvoked(context.Operation, nestedCallables)
                    ? method
                    : null;
            }

            if (GetReachableMemberExecutionMethod(context, method) is { } executionMethod)
            {
                return NestedCallablesAreInvoked(context.Operation, nestedCallables)
                    ? executionMethod
                    : null;
            }

            if (!IsNestedCallable(method))
            {
                return null;
            }

            if (!nestedCallables.Contains(method, SymbolEqualityComparer.Default))
            {
                nestedCallables.Add(method);
            }
        }

        return null;
    }

    private static bool IsModuleExecuteAsync(IMethodSymbol method, Compilation compilation)
    {
        return method.Name == AnalyzerConstants.MethodNames.ExecuteAsync
               && method.IsOverride
               && method.OverriddenMethod?.ContainingType.IsModule(compilation) == true;
    }

    private static IMethodSymbol? GetReachableMemberExecutionMethod(
        OperationAnalysisContext context,
        IMethodSymbol method)
    {
        if (method.MethodKind is not (
                MethodKind.Ordinary
                or MethodKind.ExplicitInterfaceImplementation
                or MethodKind.PropertyGet
                or MethodKind.PropertySet
                or MethodKind.EventAdd
                or MethodKind.EventRemove)
            || !TryGetReachableExecuteMethod(context, method, out var executeMethod))
        {
            return null;
        }

        return executeMethod;
    }

    private static bool IsNestedCallable(IMethodSymbol method)
    {
        return method.MethodKind is MethodKind.LocalFunction or MethodKind.AnonymousFunction;
    }

    private static List<IMethodSymbol> GetEnclosingNestedCallables(IOperation operation)
    {
        var nestedCallables = new List<IMethodSymbol>();

        for (var current = operation.Parent; current is not null; current = current.Parent)
        {
            var symbol = current switch
            {
                ILocalFunctionOperation localFunction => localFunction.Symbol,
                IAnonymousFunctionOperation anonymousFunction => anonymousFunction.Symbol,
                _ => null,
            };
            if (symbol is not null
                && !nestedCallables.Contains(symbol, SymbolEqualityComparer.Default))
            {
                nestedCallables.Add(symbol);
            }
        }

        return nestedCallables;
    }

    private static bool IsAwaiterGetResult(IInvocationOperation invocation)
    {
        return invocation.Instance is IInvocationOperation
        {
            TargetMethod.Name: "GetAwaiter",
        };
    }

    private static bool NestedCallablesAreInvoked(
        IOperation operation,
        List<IMethodSymbol> nestedCallables,
        IMethodSymbol? executionRoot = null)
    {
        if (nestedCallables.Count == 0)
        {
            return true;
        }

        var root = GetRoot(operation);
        var invocations = root.DescendantsAndSelf()
            .OfType<IInvocationOperation>()
            .ToImmutableArray();
        var callables = root.DescendantsAndSelf()
            .Select(GetCallableSymbol)
            .Where(static callable => callable is not null)
            .Cast<IMethodSymbol>()
            .Distinct<IMethodSymbol>(SymbolEqualityComparer.Default)
            .ToImmutableArray();
        var reachableCallables = GetReachableCallables(invocations, callables, executionRoot);

        return nestedCallables.All(reachableCallables.Contains);
    }

    private static bool IsInsideReachableNestedCallable(IOperation operation)
    {
        return GetCallableSymbol(GetEnclosingCallable(operation)) is not { } callable
               || NestedCallablesAreInvoked(operation, [callable]);
    }

    private static HashSet<IMethodSymbol> GetReachableCallables(
        ImmutableArray<IInvocationOperation> invocations,
        ImmutableArray<IMethodSymbol> callables,
        IMethodSymbol? executionRoot = null)
    {
        var reachable = new HashSet<IMethodSymbol>(SymbolEqualityComparer.Default);
        var pending = new Queue<IMethodSymbol>();
        if (executionRoot is not null)
        {
            reachable.Add(executionRoot);
            pending.Enqueue(executionRoot);
        }

        AddInvocationTargets(null, invocations, callables, reachable, pending);
        while (pending.Count > 0)
        {
            AddInvocationTargets(
                pending.Dequeue(),
                invocations,
                callables,
                reachable,
                pending);
        }

        return reachable;
    }

    private static void AddInvocationTargets(
        IMethodSymbol? caller,
        ImmutableArray<IInvocationOperation> invocations,
        ImmutableArray<IMethodSymbol> callables,
        HashSet<IMethodSymbol> reachable,
        Queue<IMethodSymbol> pending)
    {
        foreach (var invocation in invocations.Where(candidate =>
                     IsInReachableBranch(candidate)
                     && SymbolEqualityComparer.Default.Equals(
                         GetCallableSymbol(GetEnclosingCallable(candidate)),
                         caller)))
        {
            foreach (var target in GetInvocationTargets(invocation, callables))
            {
                if (reachable.Add(target))
                {
                    pending.Enqueue(target);
                }
            }
        }
    }

    private static IEnumerable<IMethodSymbol> GetInvocationTargets(
        IInvocationOperation invocation,
        ImmutableArray<IMethodSymbol> callables)
    {
        var targetMethod = invocation.TargetMethod.OriginalDefinition;
        foreach (var callable in callables)
        {
            if (SymbolEqualityComparer.Default.Equals(
                    callable.OriginalDefinition,
                    targetMethod)
                || InvocationTargetsCallable(invocation, callable))
            {
                yield return callable;
            }
        }
    }

    private static IOperation? GetInvocationReceiver(IInvocationOperation invocation)
    {
        if (invocation.Instance is not IConditionalAccessInstanceOperation)
        {
            return invocation.Instance;
        }

        for (var current = invocation.Parent; current is not null; current = current.Parent)
        {
            if (current is IConditionalAccessOperation conditionalAccess)
            {
                return conditionalAccess.Operation;
            }
        }

        return null;
    }

    private static IMethodSymbol? GetCallableSymbol(IOperation? operation)
    {
        return operation switch
        {
            ILocalFunctionOperation localFunction => localFunction.Symbol,
            IAnonymousFunctionOperation anonymousFunction => anonymousFunction.Symbol,
            _ => null,
        };
    }

    private static bool InvocationTargetsCallable(
        IInvocationOperation invocation,
        IMethodSymbol callable)
    {
        if (SymbolEqualityComparer.Default.Equals(
                invocation.TargetMethod.OriginalDefinition,
                callable.OriginalDefinition))
        {
            return true;
        }

        if (CallableImplementsInterfaceMethod(invocation.TargetMethod, callable))
        {
            return true;
        }

        for (var overridden = invocation.IsVirtual
                 ? callable.OverriddenMethod
                 : null;
             overridden is not null;
             overridden = overridden.OverriddenMethod)
        {
            if (SymbolEqualityComparer.Default.Equals(
                    invocation.TargetMethod.OriginalDefinition,
                    overridden.OriginalDefinition))
            {
                return true;
            }
        }

        if (GetInvocationReceiver(invocation) is { } receiver
            && ValueContainsCallable(
                receiver,
                callable,
                [with(SymbolEqualityComparer.Default)]))
        {
            return true;
        }

        return (IsKnownDelegateInvoker(invocation)
                && invocation.Arguments.Any(argument =>
                    ValueContainsCallable(
                        argument.Value,
                        callable,
                        [with(SymbolEqualityComparer.Default)])))
               || SourceMethodInvokesCallableArgument(invocation, callable);
    }

    private static bool CallableImplementsInterfaceMethod(
        IMethodSymbol targetMethod,
        IMethodSymbol callable)
    {
        return targetMethod.ContainingType.TypeKind == TypeKind.Interface
               && (callable.ExplicitInterfaceImplementations.Any(
                       implementation => SymbolEqualityComparer.Default.Equals(
                           implementation.OriginalDefinition,
                           targetMethod.OriginalDefinition))
                   || (callable.ContainingType.FindImplementationForInterfaceMember(
                           targetMethod.OriginalDefinition) is IMethodSymbol implementation
                       && SymbolEqualityComparer.Default.Equals(
                           implementation.OriginalDefinition,
                           callable.OriginalDefinition)));
    }

    private static bool SourceMethodInvokesCallableArgument(
        IInvocationOperation invocation,
        IMethodSymbol callable)
    {
        if (invocation.SemanticModel?.Compilation is not { } compilation)
        {
            return false;
        }

        var method = NormalizeMethod(invocation.TargetMethod);
        foreach (var argument in invocation.Arguments.Where(argument =>
                     argument.Parameter is not null
                     && ValueContainsCallable(
                         argument.Value,
                         callable,
                         [with(SymbolEqualityComparer.Default)])))
        {
            var parameter = GetTargetParameter(method, argument.Parameter);
            if (parameter is not null
                && SourceMethodInvokesDelegateParameter(
                    method,
                    parameter,
                    compilation))
            {
                return true;
            }
        }

        return false;
    }

    private static bool SourceMethodInvokesDelegateParameter(
        IMethodSymbol method,
        IParameterSymbol parameter,
        Compilation compilation)
    {
        return SourceMethodInvokesDelegateParameter(
            method,
            parameter,
            compilation,
            [with(SymbolEqualityComparer.Default)]);
    }

    private static bool SourceMethodInvokesDelegateParameter(
        IMethodSymbol method,
        IParameterSymbol parameter,
        Compilation compilation,
        HashSet<IParameterSymbol> visitedParameters)
    {
        if (!visitedParameters.Add(parameter)
            || GetMethodOperation(method, compilation, default) is not { } operation)
        {
            return false;
        }

        return GetReachableInvocations(operation)
            .Any(candidate => InvocationInvokesOrForwardsParameter(
                candidate,
                parameter,
                compilation,
                visitedParameters));
    }

    private static bool InvocationInvokesOrForwardsParameter(
        IInvocationOperation invocation,
        IParameterSymbol parameter,
        Compilation compilation,
        HashSet<IParameterSymbol> visitedParameters)
    {
        return (invocation.TargetMethod.MethodKind == MethodKind.DelegateInvoke
                && GetInvocationReceiver(invocation) is { } instance
                && ValueReferencesParameter(instance, parameter))
               || InvocationForwardsParameter(
                   invocation,
                   parameter,
                   compilation,
                   visitedParameters);
    }

    private static bool InvocationForwardsParameter(
        IInvocationOperation invocation,
        IParameterSymbol parameter,
        Compilation compilation,
        HashSet<IParameterSymbol> visitedParameters)
    {
        var targetMethod = NormalizeMethod(invocation.TargetMethod);
        return invocation.Arguments
            .Where(argument =>
                argument.Parameter is not null
                && ValueReferencesParameter(argument.Value, parameter))
            .Select(argument => GetTargetParameter(
                targetMethod,
                argument.Parameter))
            .Any(targetParameter =>
                targetParameter is not null
                && SourceMethodInvokesDelegateParameter(
                    targetMethod,
                    targetParameter,
                    compilation,
                    visitedParameters));
    }

    private static bool ValueReferencesParameter(
        IOperation value,
        IParameterSymbol parameter)
    {
        return ValueReferencesParameter(
            value,
            parameter,
            [with(SymbolEqualityComparer.Default)]);
    }

    private static bool ValueReferencesParameter(
        IOperation value,
        IParameterSymbol parameter,
        HashSet<ISymbol> visitedMembers)
    {
        if (GetValueAndReachingLocalValues(
                value,
                [with(SymbolEqualityComparer.Default)])
            .SelectMany(static candidate => candidate.DescendantsAndSelf())
            .OfType<IParameterReferenceOperation>()
            .Any(reference => SymbolEqualityComparer.Default.Equals(
                reference.Parameter,
                parameter)))
        {
            return true;
        }

        foreach (var memberReference in value.DescendantsAndSelf()
                     .Where(static candidate => candidate is
                         IFieldReferenceOperation or IPropertyReferenceOperation))
        {
            var member = GetReferencedMember(memberReference);
            if (member is null || visitedMembers.Contains(member))
            {
                continue;
            }

            var nextVisitedMembers = new HashSet<ISymbol>(
                visitedMembers,
                SymbolEqualityComparer.Default)
            {
                member,
            };
            if (FindReachingMemberValues(memberReference, member).Any(
                    memberValue => ValueReferencesParameter(
                        memberValue,
                        parameter,
                        new HashSet<ISymbol>(
                            nextVisitedMembers,
                            SymbolEqualityComparer.Default))))
            {
                return true;
            }
        }

        return false;
    }

    private static IParameterSymbol? GetTargetParameter(
        IMethodSymbol targetMethod,
        IParameterSymbol? argumentParameter)
    {
        return argumentParameter is null
            ? null
            : targetMethod.Parameters.FirstOrDefault(candidate =>
                candidate.Name == argumentParameter.Name);
    }

    private static IEnumerable<IOperation> FindReachingMemberValues(
        IOperation operation,
        ISymbol member)
    {
        var callable = GetEnclosingCallable(operation);
        var assignments = GetRoot(operation)
            .DescendantsAndSelf()
            .OfType<ISimpleAssignmentOperation>()
            .Where(candidate => candidate.Syntax.SpanStart < operation.Syntax.SpanStart)
            .Where(candidate =>
                ReferenceEquals(GetEnclosingCallable(candidate), callable))
            .Where(IsInReachableBranch)
            .Where(candidate => SymbolEqualityComparer.Default.Equals(
                GetReferencedMember(candidate.Target),
                member))
            .OrderByDescending(static candidate => candidate.Syntax.SpanStart)
            .ToArray();
        var linearAssignment = assignments.FirstOrDefault(candidate =>
            IsLinearPredecessor(candidate, operation));
        return assignments
            .Where(candidate =>
                linearAssignment is null
                || candidate.Syntax.SpanStart >= linearAssignment.Syntax.SpanStart)
            .Select(static assignment => assignment.Value);
    }

    private static ISymbol? GetReferencedMember(IOperation operation)
    {
        return operation switch
        {
            IFieldReferenceOperation fieldReference => fieldReference.Field,
            IPropertyReferenceOperation propertyReference =>
                propertyReference.Property,
            IEventReferenceOperation eventReference => eventReference.Event,
            _ => null,
        };
    }

    private static bool IsKnownDelegateInvoker(IInvocationOperation invocation)
    {
        var method = invocation.TargetMethod;
        var containingType = method.ContainingType.OriginalDefinition.ToDisplayString();
        return containingType switch
        {
            "System.Threading.Tasks.Task" =>
                method.Name is "Run" or "ContinueWith",
            "System.Threading.Tasks.Task<TResult>" => method.Name == "ContinueWith",
            "System.Threading.Tasks.TaskFactory"
                or "System.Threading.Tasks.TaskFactory<TResult>" =>
                method.Name == "StartNew",
            "System.Array" or "System.Collections.Generic.List<T>" =>
                method.Name == "ForEach",
            "System.Threading.Tasks.Parallel" =>
                method.Name is "For" or "ForEach" or "ForEachAsync" or "Invoke",
            "System.Linq.Enumerable" => IsLinqCallbackInvoked(invocation),
            _ => false,
        };
    }

    private static bool IsLinqCallbackInvoked(
        IInvocationOperation invocation)
    {
        if (EagerLinqTerminalNames.Contains(invocation.TargetMethod.Name))
        {
            return true;
        }

        return IsDeferredLinqSequenceConsumed(
            invocation,
            [with(SymbolEqualityComparer.Default)]);
    }

    private static bool IsDeferredLinqSequenceConsumed(
        IOperation operation,
        HashSet<ILocalSymbol> visitedLocals)
    {
        for (var current = operation.Parent; current is not null;)
        {
            if (current is IArgumentOperation or IConversionOperation)
            {
                current = current.Parent;
                continue;
            }

            if (current is IForEachLoopOperation)
            {
                return true;
            }

            if (GetDeferredLinqLocalConsumption(
                    operation,
                    current,
                    visitedLocals) is { } localConsumption)
            {
                return localConsumption;
            }

            if (current is not IInvocationOperation parentInvocation)
            {
                return false;
            }

            if (GetLinqInvocationConsumption(parentInvocation) is { } invocationConsumption)
            {
                return invocationConsumption;
            }

            current = parentInvocation.Parent;
        }

        return false;
    }

    private static bool? GetDeferredLinqLocalConsumption(
        IOperation operation,
        IOperation current,
        HashSet<ILocalSymbol> visitedLocals)
    {
        if (current is IVariableInitializerOperation
            {
                Parent: IVariableDeclaratorOperation declarator,
            })
        {
            return IsDeferredLinqLocalConsumed(
                current,
                declarator.Symbol,
                visitedLocals);
        }

        if (current is ISimpleAssignmentOperation assignment
            && assignment.Target is ILocalReferenceOperation localReference
            && HasAncestor(operation, assignment.Value))
        {
            return IsDeferredLinqLocalConsumed(
                current,
                localReference.Local,
                visitedLocals);
        }

        return null;
    }

    private static bool TryGetReachableExecuteMethod(
        OperationAnalysisContext context,
        IMethodSymbol targetMethod,
        out IMethodSymbol executeMethod)
    {
        var cache = ReachableMemberMethods.GetValue(
            context.Compilation,
            static _ => new(SymbolEqualityComparer.Default));

        foreach (var candidate in GetModuleExecuteMethods(context.Compilation))
        {
            var analysisType = candidate.ContainingType.InheritsFrom(targetMethod.ContainingType)
                ? candidate.ContainingType
                : targetMethod.ContainingType.InheritsFrom(candidate.ContainingType)
                    ? targetMethod.ContainingType
                    : null;
            if (analysisType is null)
            {
                continue;
            }

            if (!SymbolEqualityComparer.Default.Equals(
                    candidate,
                    GetEffectiveModuleExecuteMethod(analysisType, context.Compilation)))
            {
                continue;
            }

            var reachable = cache.GetOrAdd(
                analysisType,
                _ => GetReachableMemberMethods(
                    candidate,
                    analysisType,
                    context.Compilation,
                    context.CancellationToken));
            if (reachable.Contains(targetMethod))
            {
                executeMethod = candidate;
                return true;
            }
        }

        executeMethod = null!;
        return false;
    }

    private static IMethodSymbol? GetEffectiveModuleExecuteMethod(
        INamedTypeSymbol moduleType,
        Compilation compilation)
    {
        return GetModuleTypeHierarchy(moduleType, compilation)
            .SelectMany(type => type.GetMembers(
                AnalyzerConstants.MethodNames.ExecuteAsync))
            .OfType<IMethodSymbol>()
            .FirstOrDefault(method => IsModuleExecuteAsync(method, compilation));
    }

    private static ImmutableArray<IMethodSymbol> GetModuleExecuteMethods(
        Compilation compilation)
    {
        return ModuleExecuteMethods.GetValue(
            compilation,
            static currentCompilation => new Lazy<ImmutableArray<IMethodSymbol>>(
                () => [
                    .. GetNamedTypes(currentCompilation.Assembly.GlobalNamespace)
                        .SelectMany(type => type.GetMembers(
                            AnalyzerConstants.MethodNames.ExecuteAsync))
                        .OfType<IMethodSymbol>()
                        .Where(method => IsModuleExecuteAsync(method, currentCompilation)),
                ])).Value;
    }

    private static IEnumerable<INamedTypeSymbol> GetNamedTypes(
        INamespaceOrTypeSymbol container)
    {
        foreach (var member in container.GetMembers())
        {
            if (member is INamedTypeSymbol type)
            {
                yield return type;
                foreach (var nestedType in GetNamedTypes(type))
                {
                    yield return nestedType;
                }
            }
            else if (member is INamespaceSymbol namespaceSymbol)
            {
                foreach (var namespaceType in GetNamedTypes(namespaceSymbol))
                {
                    yield return namespaceType;
                }
            }
        }
    }

    private static ImmutableHashSet<IMethodSymbol> GetReachableMemberMethods(
        IMethodSymbol executeMethod,
        INamedTypeSymbol analysisType,
        Compilation compilation,
        CancellationToken cancellationToken)
    {
        var memberMethods = GetModuleMemberMethods(analysisType, compilation);
        var reachable = new HashSet<IMethodSymbol>(SymbolEqualityComparer.Default)
        {
            executeMethod,
        };
        var pending = new Queue<IMethodSymbol>();
        pending.Enqueue(executeMethod);

        while (pending.Count > 0)
        {
            var method = pending.Dequeue();
            if (GetMethodOperation(
                    method,
                    compilation,
                    cancellationToken) is not { } operation)
            {
                continue;
            }

            foreach (var candidate in GetInvokedMemberMethods(operation, memberMethods))
            {
                if (reachable.Add(candidate))
                {
                    pending.Enqueue(candidate);
                }
            }
        }

        return ImmutableHashSet.CreateRange<IMethodSymbol>(
            SymbolEqualityComparer.Default,
            reachable);
    }

    private static ImmutableArray<IMethodSymbol> GetModuleMemberMethods(
        INamedTypeSymbol analysisType,
        Compilation compilation)
    {
        return [
            .. GetModuleTypeHierarchy(analysisType, compilation)
                .SelectMany(static type => type.GetMembers())
                .OfType<IMethodSymbol>()
                .Where(static method =>
                    method.MethodKind is MethodKind.Ordinary
                        or MethodKind.ExplicitInterfaceImplementation
                        or MethodKind.PropertyGet
                        or MethodKind.PropertySet
                        or MethodKind.EventAdd
                        or MethodKind.EventRemove
                    && method.DeclaringSyntaxReferences.Length > 0),
        ];
    }

    private static IEnumerable<INamedTypeSymbol> GetModuleTypeHierarchy(
        INamedTypeSymbol moduleType,
        Compilation compilation)
    {
        for (var type = moduleType;
             type is not null && type.IsModule(compilation);
             type = type.BaseType)
        {
            yield return type;
        }
    }

    private static IOperation? GetMethodOperation(
        IMethodSymbol method,
        Compilation compilation,
        CancellationToken cancellationToken)
    {
        foreach (var syntaxReference in method.DeclaringSyntaxReferences)
        {
            // Workspace references can expose source owned by another compilation.
            // Treat those method bodies like metadata references.
            if (!compilation.ContainsSyntaxTree(syntaxReference.SyntaxTree))
            {
                continue;
            }

            var syntax = syntaxReference.GetSyntax(cancellationToken);
            var operation = compilation.GetSemanticModel(syntax.SyntaxTree)
                .GetOperation(syntax, cancellationToken);
            if (operation is not null)
            {
                return operation;
            }
        }

        return null;
    }

    private static IEnumerable<IMethodSymbol> GetInvokedMemberMethods(
        IOperation operation,
        ImmutableArray<IMethodSymbol> memberMethods)
    {
        var invocations = operation.DescendantsAndSelf()
            .OfType<IInvocationOperation>()
            .ToImmutableArray();
        var propertyReferences = operation.DescendantsAndSelf()
            .OfType<IPropertyReferenceOperation>()
            .ToImmutableArray();
        var eventAssignments = operation.DescendantsAndSelf()
            .OfType<IEventAssignmentOperation>()
            .ToImmutableArray();
        var nestedCallables = operation.DescendantsAndSelf()
            .Select(GetCallableSymbol)
            .Where(static callable => callable is not null)
            .Cast<IMethodSymbol>()
            .Distinct<IMethodSymbol>(SymbolEqualityComparer.Default)
            .ToImmutableArray();
        var reachableNestedCallables = GetReachableCallables(
            invocations,
            nestedCallables,
            GetCallableSymbol(operation));

        return GetInvokedMethods(
                invocations,
                memberMethods,
                reachableNestedCallables)
            .Concat(GetReferencedPropertyAccessors(
                propertyReferences,
                memberMethods,
                reachableNestedCallables))
            .Concat(GetReferencedEventAccessors(
                eventAssignments,
                memberMethods,
                reachableNestedCallables));
    }

    private static IEnumerable<IMethodSymbol> GetInvokedMethods(
        ImmutableArray<IInvocationOperation> invocations,
        ImmutableArray<IMethodSymbol> memberMethods,
        HashSet<IMethodSymbol> reachableNestedCallables)
    {
        foreach (var invocation in invocations)
        {
            if (!IsInReachableBranch(invocation)
                || !IsInsideReachableCallable(invocation, reachableNestedCallables))
            {
                continue;
            }

            foreach (var method in memberMethods)
            {
                if (InvocationTargetsCallable(invocation, method))
                {
                    yield return method;
                }
            }
        }
    }

    private static IEnumerable<IMethodSymbol> GetReferencedPropertyAccessors(
        ImmutableArray<IPropertyReferenceOperation> propertyReferences,
        ImmutableArray<IMethodSymbol> memberMethods,
        HashSet<IMethodSymbol> reachableNestedCallables)
    {
        foreach (var propertyReference in propertyReferences)
        {
            if (!IsInReachableBranch(propertyReference)
                || !IsInsideReachableCallable(
                    propertyReference,
                    reachableNestedCallables))
            {
                continue;
            }

            foreach (var method in memberMethods)
            {
                if (PropertyReferenceTargetsGetter(propertyReference, method)
                    || PropertyReferenceTargetsSetter(propertyReference, method))
                {
                    yield return method;
                }
            }
        }
    }

    private static IEnumerable<IMethodSymbol> GetReferencedEventAccessors(
        ImmutableArray<IEventAssignmentOperation> eventAssignments,
        ImmutableArray<IMethodSymbol> memberMethods,
        HashSet<IMethodSymbol> reachableNestedCallables)
    {
        foreach (var eventAssignment in eventAssignments)
        {
            if (!IsInReachableBranch(eventAssignment)
                || !IsInsideReachableCallable(
                    eventAssignment,
                    reachableNestedCallables))
            {
                continue;
            }

            if (GetEventAccessor(eventAssignment) is not { } accessor)
            {
                continue;
            }

            foreach (var method in memberMethods)
            {
                if (AccessorTargetsCallable(accessor, method))
                {
                    yield return method;
                }
            }
        }
    }

    private static IMethodSymbol? GetEventAccessor(
        IEventAssignmentOperation eventAssignment)
    {
        if (eventAssignment.EventReference is not IEventReferenceOperation eventReference)
        {
            return null;
        }

        return eventAssignment.Adds
            ? eventReference.Event.AddMethod
            : eventReference.Event.RemoveMethod;
    }

    private static bool IsInsideReachableCallable(
        IOperation operation,
        HashSet<IMethodSymbol> reachableNestedCallables)
    {
        var caller = GetCallableSymbol(GetEnclosingCallable(operation));
        return caller is null || reachableNestedCallables.Contains(caller);
    }

    private static bool PropertyReferenceTargetsGetter(
        IPropertyReferenceOperation propertyReference,
        IMethodSymbol method)
    {
        if (propertyReference.Property.GetMethod is not { } getter
            || (propertyReference.Parent is ISimpleAssignmentOperation assignment
            && ReferenceEquals(assignment.Target, propertyReference)))
        {
            return false;
        }

        return AccessorTargetsCallable(getter, method);
    }

    private static bool PropertyReferenceTargetsSetter(
        IPropertyReferenceOperation propertyReference,
        IMethodSymbol method)
    {
        if (propertyReference.Property.SetMethod is not { } setter
            || !IsPropertyAssignmentTarget(propertyReference))
        {
            return false;
        }

        return AccessorTargetsCallable(setter, method);
    }

    private static bool AccessorTargetsCallable(
        IMethodSymbol accessor,
        IMethodSymbol callable)
    {
        if (CallableImplementsInterfaceMethod(accessor, callable))
        {
            return true;
        }

        for (var candidate = callable;
             candidate is not null;
             candidate = candidate.OverriddenMethod)
        {
            if (SymbolEqualityComparer.Default.Equals(
                    accessor.OriginalDefinition,
                    candidate.OriginalDefinition))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsPropertyAssignmentTarget(
        IPropertyReferenceOperation propertyReference)
    {
        return propertyReference.Parent switch
        {
            IAssignmentOperation assignment =>
                ReferenceEquals(assignment.Target, propertyReference),
            IIncrementOrDecrementOperation increment =>
                ReferenceEquals(increment.Target, propertyReference),
            _ => false,
        };
    }

    private static bool? GetLinqInvocationConsumption(
        IInvocationOperation invocation)
    {
        if (IsTaskJoin(invocation))
        {
            return IsAwaitedBeforeNestedCallable(invocation);
        }

        if (invocation.TargetMethod.ContainingType.OriginalDefinition
            .ToDisplayString() != "System.Linq.Enumerable")
        {
            return false;
        }

        return EagerLinqTerminalNames.Contains(invocation.TargetMethod.Name)
            ? true
            : null;
    }

    private static bool IsDeferredLinqLocalConsumed(
        IOperation assignment,
        ILocalSymbol local,
        HashSet<ILocalSymbol> visitedLocals)
    {
        if (!visitedLocals.Add(local))
        {
            return false;
        }

        var callable = GetEnclosingCallable(assignment);
        return GetRoot(assignment)
            .DescendantsAndSelf()
            .OfType<ILocalReferenceOperation>()
            .Where(reference => reference.Syntax.SpanStart > assignment.Syntax.Span.End)
            .Where(reference => SymbolEqualityComparer.Default.Equals(
                reference.Local,
                local))
            .Where(reference => IsDeferredLinqReferenceReachable(
                assignment,
                reference,
                local,
                callable,
                visitedLocals))
            .Where(static reference => !IsLocalAssignmentTarget(reference))
            .Any(reference => IsDeferredLinqSequenceConsumed(
                reference,
                CloneVisitedLocals(visitedLocals)));
    }

    private static bool IsDeferredLinqReferenceReachable(
        IOperation assignment,
        ILocalReferenceOperation reference,
        ILocalSymbol local,
        IOperation? assignmentCallable,
        HashSet<ILocalSymbol> visitedLocals)
    {
        var referenceCallable = GetEnclosingCallable(reference);
        if (ReferenceEquals(referenceCallable, assignmentCallable))
        {
            return FindReachingLocalValue(reference, local) is { } reachingValue
                   && HasAncestor(reachingValue, assignment);
        }

        if (GetCallableSymbol(referenceCallable) is not { } referenceCallableSymbol)
        {
            return false;
        }

        return GetRoot(assignment)
            .DescendantsAndSelf()
            .OfType<IInvocationOperation>()
            .Where(invocation => invocation.Syntax.SpanStart > assignment.Syntax.Span.End)
            .Where(invocation => ReferenceEquals(
                GetEnclosingCallable(invocation),
                assignmentCallable))
            .Where(IsAwaitedBeforeNestedCallable)
            .Where(invocation => ValueContainsCallable(
                invocation,
                referenceCallableSymbol,
                CloneVisitedLocals(visitedLocals)))
            .Any(invocation =>
                FindReachingLocalValue(invocation, local) is { } reachingValue
                && HasAncestor(reachingValue, assignment));
    }

    private static bool IsLocalAssignmentTarget(ILocalReferenceOperation reference)
    {
        return reference.Parent is ISimpleAssignmentOperation assignment
               && ReferenceEquals(assignment.Target, reference);
    }

    private static bool IsAwaitedBeforeNestedCallable(IOperation operation)
    {
        for (var ancestor = operation.Parent;
             ancestor is not null;
             ancestor = ancestor.Parent)
        {
            if (ancestor is IAwaitOperation)
            {
                return true;
            }

            if (ancestor is IAnonymousFunctionOperation or ILocalFunctionOperation)
            {
                return false;
            }
        }

        return false;
    }

    private static bool ValueContainsCallable(
        IOperation value,
        IMethodSymbol callable,
        HashSet<ILocalSymbol> visitedLocals)
    {
        var values = (value.SemanticModel?.Compilation is { } compilation
                ? GetValueAndReachingCallbackValues(
                    value,
                    compilation,
                    visitedLocals,
                    [with(SymbolEqualityComparer.Default)],
                    [with(SymbolEqualityComparer.Default)])
                : GetValueAndReachingLocalValues(value, visitedLocals))
            .ToArray();
        return values.SelectMany(static candidate => candidate.DescendantsAndSelf())
            .OfType<IAnonymousFunctionOperation>()
            .Any(candidate => SymbolEqualityComparer.Default.Equals(
                candidate.Symbol,
                callable))
            || values.SelectMany(static candidate => candidate.DescendantsAndSelf())
                .OfType<IMethodReferenceOperation>()
                .Any(candidate => SymbolEqualityComparer.Default.Equals(
                    candidate.Method.OriginalDefinition,
                    callable.OriginalDefinition));
    }

    private static bool InvocationUsesCancellation(
        IInvocationOperation invocation,
        IParameterSymbol cancellationToken)
    {
        return invocation.Arguments
            .Where(static argument => argument.Parameter is not null)
            .Where(argument => IsCancellationToken(argument.Parameter!))
            .Select(static argument => argument.Value)
            .Any(value => FlowsFromCancellationToken(
                value,
                cancellationToken,
                [with(SymbolEqualityComparer.Default)]));
    }

    private static bool FlowsFromCancellationToken(
        IOperation value,
        IParameterSymbol cancellationToken,
        HashSet<ILocalSymbol> visitedLocals) =>
        FlowsFromCancellationToken(
            value,
            [cancellationToken],
            visitedLocals,
            [with(SymbolEqualityComparer.Default)]);

    private static bool FlowsFromCancellationToken(
        IOperation value,
        IReadOnlyCollection<IParameterSymbol> cancellationTokens,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods) =>
        FlowsFromCancellationToken(value, cancellationTokens, visitedLocals, visitedMethods, []);

    private static bool FlowsFromCancellationToken(
        IOperation value,
        IReadOnlyCollection<IParameterSymbol> cancellationTokens,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods,
        HashSet<IOperation> visitedWrites)
    {
        return value switch
        {
            IConversionOperation conversion => FlowsFromCancellationToken(
                conversion.Operand,
                cancellationTokens,
                visitedLocals,
                visitedMethods,
                visitedWrites),
            IParenthesizedOperation parenthesized => FlowsFromCancellationToken(
                parenthesized.Operand,
                cancellationTokens,
                visitedLocals,
                visitedMethods,
                visitedWrites),
            IParameterReferenceOperation parameterReference =>
                IsStableCapturedCancellationToken(parameterReference)
                && (FlowsFromCancellationTokenStorage(
                    parameterReference,
                    cancellationTokens,
                    visitedLocals,
                    visitedMethods,
                    visitedWrites)
                    || FlowsFromParallelCallbackToken(
                        parameterReference,
                        cancellationTokens,
                        visitedLocals,
                        visitedMethods,
                        visitedWrites)),
            ILocalReferenceOperation { Local.RefKind: not RefKind.None } localReference =>
                FlowsFromCancellationTokenStorage(
                    localReference,
                    cancellationTokens,
                    visitedLocals,
                    visitedMethods,
                    visitedWrites),
            ILocalReferenceOperation localReference =>
                FlowsFromCancellationTokenLocal(
                    localReference,
                    cancellationTokens,
                    visitedLocals,
                    visitedMethods,
                    visitedWrites),
            IPropertyReferenceOperation propertyReference =>
                FlowsFromCancellationTokenProperty(
                    propertyReference,
                    cancellationTokens,
                    visitedLocals,
                    visitedMethods,
                    visitedWrites),
            IInvocationOperation invocation =>
                FlowsFromCancellationTokenInvocation(
                    invocation,
                    cancellationTokens,
                    visitedLocals,
                    visitedMethods,
                    visitedWrites),
            IArrayCreationOperation { Initializer: { } initializer } =>
                FlowsFromCancellationToken(
                    initializer,
                    cancellationTokens,
                    visitedLocals,
                    visitedMethods,
                    visitedWrites),
            IArrayInitializerOperation initializer =>
                initializer.ElementValues.Any(element => FlowsFromCancellationToken(
                    element,
                    cancellationTokens,
                    visitedLocals,
                    visitedMethods,
                    visitedWrites)),
            ICollectionExpressionOperation collection =>
                collection.Elements.Any(element => FlowsFromCancellationToken(
                    element,
                    cancellationTokens,
                    visitedLocals,
                    visitedMethods,
                    visitedWrites)),
            ISpreadOperation spread =>
                FlowsFromCancellationToken(
                    spread.Operand,
                    cancellationTokens,
                    visitedLocals,
                    visitedMethods,
                    visitedWrites),
            IConditionalOperation conditional =>
                FlowsFromCancellationTokenConditional(
                    conditional,
                    cancellationTokens,
                    visitedLocals,
                    visitedMethods,
                    visitedWrites),
            ISwitchExpressionOperation switchExpression =>
                FlowsFromCancellationTokenSwitchExpression(
                    switchExpression,
                    cancellationTokens,
                    visitedLocals,
                    visitedMethods,
                    visitedWrites),
            ICoalesceOperation coalesce =>
                FlowsFromCancellationTokenCoalesce(
                    coalesce,
                    cancellationTokens,
                    visitedLocals,
                    visitedMethods,
                    visitedWrites),
            _ => false,
        };
    }

    private static bool FlowsFromCancellationTokenStorage(
        IOperation reference,
        IReadOnlyCollection<IParameterSymbol> cancellationTokens,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods,
        HashSet<IOperation> visitedWrites)
    {
        // Module entry tokens retain their existing identity-based contract. Storage
        // value tracking is needed for ref parameters and aliases in source helpers.
        if (reference is IParameterReferenceOperation { Parameter.RefKind: RefKind.None } parameter)
        {
            return cancellationTokens.Contains(parameter.Parameter, SymbolEqualityComparer.Default);
        }

        return FlowsFromCancellationTokenStorageValue(
            reference, reference, cancellationTokens, visitedLocals, visitedMethods, visitedWrites);
    }

    private static bool FlowsFromCancellationTokenStorageValue(
        IOperation value,
        IOperation read,
        IReadOnlyCollection<IParameterSymbol> cancellationTokens,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods,
        HashSet<IOperation> visitedWrites)
    {
        if (value is IConversionOperation conversion)
        {
            return FlowsFromCancellationTokenStorageValue(
                conversion.Operand, read, cancellationTokens, visitedLocals, visitedMethods, visitedWrites);
        }

        if (value is IParenthesizedOperation parenthesized)
        {
            return FlowsFromCancellationTokenStorageValue(
                parenthesized.Operand, read, cancellationTokens, visitedLocals, visitedMethods, visitedWrites);
        }

        if (GetCancellationStorageSymbol(value) is null)
        {
            return FlowsFromCancellationToken(value, cancellationTokens, visitedLocals, visitedMethods, visitedWrites);
        }

        var storages = GetCancellationTokenStorages(value, value, []).ToArray();
        var aliasWrite = storages.Length > 1 ? FindDefiniteCancellationAliasWrite(read, cancellationTokens, visitedLocals, visitedMethods, visitedWrites) : null;
        return storages.Length > 0 && storages.All(StorageFlows);

        bool StorageFlows(IOperation storage) => CancellationTokenStorageFlows(
            storage, read, cancellationTokens, visitedLocals, visitedMethods, visitedWrites, aliasWrite);
    }

    private static bool CancellationTokenStorageFlows(
        IOperation storage,
        IOperation read,
        IReadOnlyCollection<IParameterSymbol> cancellationTokens,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods,
        HashSet<IOperation> visitedWrites,
        IOperation? aliasWrite)
    {
        var symbol = GetCancellationStorageSymbol(storage);
        var writes = FindCancellationStorageAssignments(read, symbol, isRef: false);
        var definiteWrite = FindDefiniteCancellationStorageWrite(read, symbol, isRef: false);
        if (aliasWrite is not null && (definiteWrite is null || aliasWrite.Syntax.SpanStart > definiteWrite.Syntax.SpanStart))
        {
            definiteWrite = aliasWrite;
        }

        var reachingWrites = writes
            .Where(write => !IsOverwrittenCancellationStorageWrite(write, read, symbol))
            .Where(write => !IsFormerCancellationAliasReferentWrite(write, read, symbol))
            .Where(write => ReachesAfterCancellationStorageReset(write, definiteWrite, read));
        foreach (var write in reachingWrites)
        {
            // A cyclic write cannot prove token provenance; report conservatively.
            if (!visitedWrites.Add(write))
            {
                return false;
            }

            var flows = FlowsFromCancellationTokenStorageValue(
                write.Value, write.Value, cancellationTokens,
                CloneVisitedLocals(visitedLocals), CloneVisitedMethods(visitedMethods), visitedWrites);
            visitedWrites.Remove(write);
            if (!flows)
            {
                return false;
            }
        }

        if (definiteWrite is not null)
        {
            return true;
        }

        return InitialCancellationTokenStorageFlows(storage, read, cancellationTokens, visitedLocals, visitedMethods, visitedWrites);
    }

    private static bool IsFormerCancellationAliasReferentWrite(
        IOperation write, IOperation read, ISymbol? storageSymbol)
    {
        var aliasSymbol = GetCancellationStorageSymbol(read);
        if (aliasSymbol is not (ILocalSymbol { RefKind: not RefKind.None }
            or IParameterSymbol { RefKind: not RefKind.None })
            || SymbolEqualityComparer.Default.Equals(aliasSymbol, storageSymbol)
            || SharesCancellationStorageLoop(write, read)
            || SharesCancellationStorageBranchTransfer(write, read)
            || !IsOverwrittenCancellationStorageWrite(write, read, aliasSymbol, isRef: true))
        {
            return false;
        }

        // A branch can write the old referent and then select different storage.
        // Keep the write if any subsequent reachable binding could select that
        // old storage again, including after the branch rejoins the read's path.
        var bindings = FindCancellationStorageAssignments(read, aliasSymbol, isRef: true)
            .Where(binding => binding.Syntax.SpanStart > write.Syntax.SpanStart)
            .Where(binding => CanReachCancellationStorageRead(write, binding))
            .ToArray();
        return bindings.Length > 0 && bindings.All(binding =>
            !SharesCancellationStorageLoop(binding, read)
            && !SharesCancellationStorageBranchTransfer(binding, read)
            && GetCancellationTokenStorages(binding.Value, binding, []).All(storage =>
                GetCancellationStorageSymbol(storage) is { } symbol
                && !SymbolEqualityComparer.Default.Equals(symbol, storageSymbol)));
    }

    private static bool InitialCancellationTokenStorageFlows(
        IOperation storage,
        IOperation read,
        IReadOnlyCollection<IParameterSymbol> cancellationTokens,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods,
        HashSet<IOperation> visitedWrites)
    {
        return storage switch
        {
            IParameterReferenceOperation parameter =>
                IsStableCapturedCancellationToken(parameter)
                && cancellationTokens.Contains(parameter.Parameter, SymbolEqualityComparer.Default),
            ILocalReferenceOperation local when !visitedLocals.Contains(local.Local) =>
                FindReachingLocalValues(read, local.Local).ToArray() is { Length: > 0 } values
                && values.All(localValue => FlowsFromCancellationTokenStorageValue(
                    localValue, localValue, cancellationTokens,
                    new HashSet<ILocalSymbol>(visitedLocals, visitedLocals.Comparer) { local.Local },
                    CloneVisitedMethods(visitedMethods), visitedWrites)),
            _ => false,
        };
    }

    private static IEnumerable<IOperation> GetCancellationTokenStorages(
        IOperation reference,
        IOperation bindingPoint,
        HashSet<IOperation> visitedBindings)
    {
        if (reference is IConversionOperation conversion)
        {
            return GetCancellationTokenStorages(conversion.Operand, bindingPoint, visitedBindings);
        }

        if (reference is IParenthesizedOperation parenthesized)
        {
            return GetCancellationTokenStorages(parenthesized.Operand, bindingPoint, visitedBindings);
        }

        if (reference is IConditionalOperation { IsRef: true })
        {
            return GetReachableValueLeaves(reference).SelectMany(value =>
                GetCancellationTokenStorages(value, bindingPoint, [.. visitedBindings]));
        }

        var symbol = GetCancellationStorageSymbol(reference);
        if (symbol is not (IParameterSymbol { RefKind: not RefKind.None }
            or ILocalSymbol { RefKind: not RefKind.None }))
        {
            return [reference];
        }

        return ResolveCancellationTokenReferenceStorage(reference, bindingPoint, symbol, visitedBindings);
    }

    private static IEnumerable<IOperation> ResolveCancellationTokenReferenceStorage(
        IOperation reference, IOperation bindingPoint, ISymbol symbol, HashSet<IOperation> visitedBindings)
    {
        var root = GetRoot(bindingPoint);
        var callable = GetEnclosingCallable(bindingPoint);
        var bindings = FindCancellationStorageAssignments(bindingPoint, symbol, isRef: true);
        var definiteBinding = FindDefiniteCancellationStorageWrite(bindingPoint, symbol, isRef: true);
        var storages = bindings
            .Where(binding => !IsOverwrittenCancellationStorageWrite(binding, bindingPoint, symbol, isRef: true))
            .Where(binding => ReachesAfterCancellationStorageReset(binding, definiteBinding, bindingPoint))
            .SelectMany(binding => visitedBindings.Contains(binding)
                ? [binding]
                : GetCancellationTokenStorages(
                    binding.Value,
                    binding,
                    [with(visitedBindings), binding]));
        if (definiteBinding is not null)
        {
            return storages;
        }

        // Resolve an alias when it is bound, then read its referent at the use.
        // A later rebinding of the source reference does not redirect this alias.
        if (symbol is ILocalSymbol local
            && FindLocalDeclarator(root, bindingPoint, local, callable) is { Initializer.Value: { } initial } declarator
            && visitedBindings.Add(declarator))
        {
            return storages.Concat(GetCancellationTokenStorages(initial, declarator, visitedBindings));
        }

        return storages.Append(reference);
    }

    private static ISimpleAssignmentOperation[] FindCancellationStorageAssignments(
        IOperation read, ISymbol? symbol, bool isRef)
    {
        var callable = GetEnclosingCallable(read);
        return [
            .. GetRoot(read).DescendantsAndSelf()
            .OfType<ISimpleAssignmentOperation>()
            .Where(assignment => assignment.IsRef == isRef)
            .Where(assignment => !HasAncestor(read, assignment))
            .Where(assignment => ReferenceEquals(GetEnclosingCallable(assignment), callable))
            .Where(IsInReachableBranch)
            .Where(assignment => CanReachCancellationStorageRead(assignment, read))
            .Where(assignment => assignment.Syntax.SpanStart < read.Syntax.SpanStart
                || SharesCancellationStorageLoop(assignment, read)
                || SharesCancellationStorageBranchTransfer(assignment, read))
            .Where(assignment => isRef
                ? CancellationStorageAssignmentTargets(assignment, symbol, isRef)
                : GetCancellationTokenStorages(assignment.Target, assignment, [])
                    .Any(target => SymbolEqualityComparer.Default.Equals(GetCancellationStorageSymbol(target), symbol)))
            .OrderByDescending(static assignment => assignment.Syntax.SpanStart)
        ];
    }

    private static IOperation? FindDefiniteCancellationStorageWrite(IOperation read, ISymbol? symbol, bool? isRef)
    {
        var callable = GetEnclosingCallable(read);
        // Exhaustive branches can replace storage collectively even when no
        // individual assignment is a linear predecessor of the read.
        return GetRoot(read).DescendantsAndSelf()
            .Where(operation => operation is ISimpleAssignmentOperation or IConditionalOperation or ISwitchOperation
                or IWhileLoopOperation { ConditionIsTop: false } or ITryOperation)
            .Where(operation => !HasAncestor(read, operation))
            .Where(IsInReachableBranch)
            .Where(operation => CanReachCancellationStorageRead(operation, read))
            .Where(operation => ReferenceEquals(GetEnclosingCallable(operation), callable))
            .Where(operation => IsDefiniteCancellationStorageBinding(operation, read))
            .Where(operation => DefinitelyWritesCancellationStorage(operation, symbol, isRef, read))
            .OrderByDescending(static operation => operation.Syntax.SpanStart)
            .FirstOrDefault();
    }

    private static IOperation? FindDefiniteCancellationAliasWrite(
        IOperation read,
        IReadOnlyCollection<IParameterSymbol> cancellationTokens,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods,
        HashSet<IOperation> visitedWrites)
    {
        var symbol = GetCancellationStorageSymbol(read);
        if (symbol is not (ILocalSymbol { RefKind: not RefKind.None } or IParameterSymbol { RefKind: not RefKind.None }))
        {
            return null;
        }

        var write = FindDefiniteCancellationStorageWrite(read, symbol, isRef: null);
        var bindings = FindCancellationStorageAssignments(read, symbol, isRef: true);
        // Exhaustive branches may establish the alias's value either by writing
        // its referent or by selecting incoming-token storage. This proves only
        // the alias read: neither possible referent is definitely written.
        if (write is null || bindings.Any(binding =>
            (!HasAncestor(binding, write) && binding.Syntax.SpanStart > write.Syntax.SpanStart)
                || SharesCancellationStorageLoop(binding, read)))
        {
            return null;
        }

        // A branch may select non-flowing storage and then replace its value.
        // Only bindings whose contents survive that branch constrain the read.
        if (!bindings.Where(binding => HasAncestor(binding, write))
            .Where(binding => !IsOverwrittenCancellationStorageWrite(binding, read, symbol, isRef: null))
            .All(binding =>
            CancellationAliasBindingFlows(binding, cancellationTokens, visitedLocals, visitedMethods, visitedWrites)))
        {
            return null;
        }

        return write;
    }

    private static bool CancellationAliasBindingFlows(
        ISimpleAssignmentOperation binding,
        IReadOnlyCollection<IParameterSymbol> cancellationTokens,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods,
        HashSet<IOperation> visitedWrites)
    {
        if (!visitedWrites.Add(binding))
        {
            return false;
        }

        var flows = FlowsFromCancellationTokenStorageValue(
            binding.Value, binding.Value, cancellationTokens,
            CloneVisitedLocals(visitedLocals), CloneVisitedMethods(visitedMethods), visitedWrites);
        visitedWrites.Remove(binding);
        return flows;
    }

    private static bool ReachesAfterCancellationStorageReset(IOperation write, IOperation? reset, IOperation read) =>
        reset is null
        || (write.Syntax.SpanStart >= reset.Syntax.SpanStart && write.Syntax.SpanStart < read.Syntax.SpanStart
            && !ResetsCancellationStorageBranchRevisits(reset, write, read))
        || (SharesCancellationStorageLoop(write, read) && !ResetsCancellationStorageBackedges(reset, write, read))
        || (SharesCancellationStorageBranchTransfer(write, read)
            && !ResetsCancellationStorageBranchRevisits(reset, write, read));

    private static bool SharesCancellationStorageBranchTransfer(IOperation write, IOperation read)
    {
        var construct = FindCancellationStorageSiblingBranch(write, read);
        return construct is not null
            && (CanRevisitCancellationStorageOperation(construct)
                || HasCancellationStorageCaseTransfer(construct, write, read));
    }

    private static bool ResetsCancellationStorageBranchRevisits(IOperation reset, IOperation write, IOperation read)
    {
        if (SharesCancellationStorageLoop(write, read))
        {
            return false;
        }

        var construct = FindCancellationStorageSiblingBranch(write, read);
        if (construct is null)
        {
            return false;
        }

        // A reset already proven definite before the read also executes when
        // entering its branch through a case transfer or a construct revisit.
        if (HasAncestor(reset, construct))
        {
            return true;
        }

        if (reset.Syntax.Span.End > construct.Syntax.SpanStart)
        {
            return false;
        }

        // Without direct case transfers, sibling branches communicate only by
        // revisiting the construct. Internal and trailing backward jumps must
        // all re-execute the reset; an unknown target remains conservative.
        var targets = GetCancellationStorageRevisitTargets(construct).ToArray();
        return targets.Length > 0
            && targets.All(target => target.Syntax.SpanStart <= reset.Syntax.SpanStart)
            && construct.DescendantsAndSelf().OfType<IBranchOperation>()
                .Where(static branch => branch.BranchKind == BranchKind.GoTo)
                .All(branch => targets.Any(target =>
                    SymbolEqualityComparer.Default.Equals(target.Label, branch.Target)));
    }

    private static bool HasCancellationStorageCaseTransfer(IOperation construct, IOperation write, IOperation read) =>
        construct is ISwitchOperation switchOperation
        && switchOperation.Cases.FirstOrDefault(candidate => HasAncestor(write, candidate)) is { } source
        && switchOperation.Cases.FirstOrDefault(candidate => HasAncestor(read, candidate)) is { } target
        && CanTransferCancellationStorageBetweenCases(source, target, switchOperation);

    private static IOperation? FindCancellationStorageSiblingBranch(IOperation write, IOperation read)
    {
        for (var current = write; current.Parent is { } parent; current = parent)
        {
            if (current is ISwitchCaseOperation sourceCase && parent is ISwitchOperation switchOperation
                && switchOperation.Cases.FirstOrDefault(candidate => HasAncestor(read, candidate)) is { } targetCase
                && !ReferenceEquals(sourceCase, targetCase))
            {
                return switchOperation;
            }

            if (parent is IConditionalOperation conditional
                && IsReadInSiblingCancellationConditional(current, conditional, read)
                && !conditional.DescendantsAndSelf().OfType<IBranchOperation>().Any())
            {
                return conditional;
            }
        }

        return null;
    }

    private static bool CancellationStorageAssignmentTargets(ISimpleAssignmentOperation assignment, ISymbol? symbol, bool? isRef)
    {
        // null proves the value read through this alias, not a write to each
        // possible referent. Storage-level proofs must resolve the actual target.
        if (isRef is not false)
        {
            return SymbolEqualityComparer.Default.Equals(GetCancellationStorageSymbol(assignment.Target), symbol);
        }

        var targets = GetCancellationTokenStorages(assignment.Target, assignment, []).ToArray();
        return targets.Length > 0 && targets.All(target =>
            SymbolEqualityComparer.Default.Equals(GetCancellationStorageSymbol(target), symbol));
    }

    private static bool DefinitelyWritesCancellationStorage(IOperation operation, ISymbol? symbol, bool? isRef = false, IOperation? read = null)
    {
        if (operation is ISwitchOperation switchOperation)
        {
            return DefinitelyWritesCancellationStorageSwitch(switchOperation, symbol, isRef, read);
        }

        // Jumps inside a branch can bypass an otherwise unconditional write.
        if (operation.DescendantsAndSelf().OfType<IBranchOperation>().Any(branch => !IsNestedCancellationStorageSwitchBreak(branch, operation)))
        {
            return false;
        }

        return operation switch
        {
            ISimpleAssignmentOperation assignment => (isRef is null || assignment.IsRef == isRef)
                && CancellationStorageAssignmentTargets(assignment, symbol, isRef),
            IExpressionStatementOperation expression => DefinitelyWritesCancellationStorage(expression.Operation, symbol, isRef, read),
            IBlockOperation block => block.Operations.Any(child => DefinitelyWritesCancellationStorage(child, symbol, isRef, read)),
            IConditionalOperation conditional => DefinitelyWritesCancellationStorageBranches(conditional, symbol, isRef, read),
            IWhileLoopOperation { ConditionIsTop: false } loop => DefinitelyWritesCancellationStorage(loop.Body, symbol, isRef, read),
            ITryOperation attempt => DefinitelyWritesCancellationStorageTry(attempt, symbol, isRef, read),
            _ => false,
        };
    }

    private static bool IsNestedCancellationStorageSwitchBreak(IBranchOperation branch, IOperation operation)
    {
        if (branch.BranchKind != BranchKind.Break)
        {
            return false;
        }

        for (var parent = branch.Parent; parent is not null; parent = parent.Parent)
        {
            if (parent is ISwitchOperation nestedSwitch
                && SymbolEqualityComparer.Default.Equals(branch.Target, nestedSwitch.ExitLabel))
            {
                return true;
            }

            if (ReferenceEquals(parent, operation))
            {
                break;
            }
        }

        return false;
    }

    private static bool DefinitelyWritesCancellationStorageSwitch(
        ISwitchOperation operation, ISymbol? symbol, bool? isRef, IOperation? read)
    {
        // A contained switch consumes its own breaks; each case must still write.
        return operation.Cases.Any(@case => @case.Clauses.Any(static clause => clause is IDefaultCaseClauseOperation))
            && !operation.DescendantsAndSelf().OfType<IBranchOperation>().Any(branch =>
                !IsNestedCancellationStorageSwitchBreak(branch, operation))
            && operation.Cases.All(@case => CancellationStorageSequenceDefinitelyWrites(@case.Body, symbol, isRef, read)
                || (SwitchCaseCannotReachFollowingOperation(@case)
                    && !CanReachReadThroughCaughtCancellationStorageThrow(@case, read ?? GetRoot(@case))));
    }

    private static bool CancellationStorageSequenceDefinitelyWrites(
        IEnumerable<IOperation> operations, ISymbol? symbol, bool? isRef, IOperation? read)
    {
        foreach (var operation in operations)
        {
            if (DefinitelyWritesCancellationStorage(operation, symbol, isRef, read))
            {
                return true;
            }

            if (operation.DescendantsAndSelf().Any(child => child is IReturnOperation or IThrowOperation
                || (child is IBranchOperation branch && !IsNestedCancellationStorageSwitchBreak(branch, operation))))
            {
                return false;
            }
        }

        return false;
    }

    private static bool DefinitelyWritesCancellationStorageBranches(
        IConditionalOperation conditional, ISymbol? symbol, bool? isRef, IOperation? read)
    {
        if (conditional.Condition.ConstantValue is { HasValue: true, Value: bool condition })
        {
            return (condition ? conditional.WhenTrue : conditional.WhenFalse) is { } selected
                && DefinitelyWritesCancellationStorage(selected, symbol, isRef, read);
        }

        return conditional.WhenFalse is { } whenFalse
            && DefinitelyWritesCancellationStorageOrExits(conditional.WhenTrue, symbol, isRef, read)
            && DefinitelyWritesCancellationStorageOrExits(whenFalse, symbol, isRef, read);
    }

    private static bool DefinitelyWritesCancellationStorageOrExits(
        IOperation operation, ISymbol? symbol, bool? isRef, IOperation? read) =>
        DefinitelyWritesCancellationStorage(operation, symbol, isRef, read)
        || (AlwaysExitsCallable(operation)
            && !CanReachReadThroughCaughtCancellationStorageThrow(operation, read ?? GetRoot(operation)));

    private static bool DefinitelyWritesCancellationStorageTry(
        ITryOperation attempt, ISymbol? symbol, bool? isRef, IOperation? read)
    {
        return (attempt.Finally is { } finallyBlock
                && DefinitelyWritesCancellationStorage(finallyBlock, symbol, isRef, read))
            || (DefinitelyWritesCancellationStorage(attempt.Body, symbol, isRef, read)
                && attempt.Catches.All(handler => DefinitelyWritesCancellationStorageOrExits(handler.Handler, symbol, isRef, read)));
    }

    private static bool IsOverwrittenCancellationStorageWrite(
        IOperation write, IOperation read, ISymbol? symbol, bool? isRef = false)
    {
        // A caught exception can reach a read outside the try without executing
        // a later overwrite. Preserve intermediate values on that exceptional path.
        if (CanEscapeCancellationStorageWriteToRead(write, read, symbol, isRef))
        {
            return false;
        }

        // A later unconditional overwrite in this branch kills intermediate
        // values even when the branch as a whole is the definite predecessor.
        for (var current = write; current.Parent is { } parent; current = parent)
        {
            if (parent is ITryOperation attempt
                && CancellationStorageTryOverwrites(attempt, current, read, symbol, isRef))
            {
                return true;
            }

            var sequence = parent switch
            {
                IBlockOperation block => block.Operations,
                ISwitchCaseOperation @case => @case.Body,
                _ => default,
            };
            if (sequence.IsDefault)
            {
                continue;
            }

            if (ContainsCancellationStorageBranchOrReturn(current))
            {
                break;
            }

            if (CancellationStorageFollowingSequenceOverwrites(sequence, current, read, symbol, isRef))
            {
                return true;
            }

            // A case break reaches the end of its switch, so it cannot bypass
            // an overwrite after that switch. Other jumps still stop traversal.
            if (HasAncestor(read, parent)
                || ContainsCancellationStorageBranchOrReturn(parent, parent is ISwitchCaseOperation ? parent.Parent : parent))
            {
                break;
            }
        }

        return false;
    }

    private static bool ContainsCancellationStorageBranchOrReturn(IOperation operation, IOperation? branchBoundary = null) =>
        operation.DescendantsAndSelf().Any(child => child is IReturnOperation
            || (child is IBranchOperation branch && !IsNestedCancellationStorageSwitchBreak(branch, branchBoundary ?? operation)));

    private static bool CancellationStorageTryOverwrites(
        ITryOperation attempt, IOperation current, IOperation read, ISymbol? symbol, bool? isRef) =>
        (!ReferenceEquals(current, attempt.Finally)
            && FinallyOverwritesCancellationStorage(attempt, read, symbol, isRef))
        || (attempt.Catches.Length > 0 && ReferenceEquals(current, attempt.Body)
            && !HasAncestor(read, attempt.Body) && AlwaysExitsCallable(attempt.Body)
            && attempt.Catches.All(handler => DefinitelyWritesCancellationStorage(handler.Handler, symbol, isRef)));

    private static bool CancellationStorageFollowingSequenceOverwrites(
        IEnumerable<IOperation> sequence, IOperation current, IOperation read, ISymbol? symbol, bool? isRef)
    {
        foreach (var next in sequence.SkipWhile(operation => !ReferenceEquals(operation, current)).Skip(1))
        {
            if (HasAncestor(read, next) || next.Syntax.SpanStart >= read.Syntax.SpanStart
                || ContainsCancellationStorageBranchOrReturn(next))
            {
                break;
            }

            if (DefinitelyWritesCancellationStorage(next, symbol, isRef))
            {
                return true;
            }
        }

        return false;
    }

    private static bool FinallyOverwritesCancellationStorage(
        ITryOperation attempt, IOperation read, ISymbol? symbol, bool? isRef) =>
        attempt.Finally is { } finalizer && !HasAncestor(read, attempt)
        && DefinitelyWritesCancellationStorage(finalizer, symbol, isRef, read)
        && !CancellationStorageOperationCanThrowBeforeOverwrite(
            finalizer, symbol, isRef, new OperationExceptionClassifier(attempt.SemanticModel), out _);

    private static bool CanEscapeCancellationStorageWriteToRead(
        IOperation write, IOperation read, ISymbol? symbol, bool? isRef)
    {
        for (var current = write.Parent; current is not null; current = current.Parent)
        {
            if (current is ITryOperation attempt && HasAncestor(write, attempt.Body)
                && !FinallyOverwritesCancellationStorage(attempt, read, symbol, isRef)
                && ((attempt.Finally is { } finallyBlock && HasAncestor(read, finallyBlock))
                    || (!HasAncestor(read, attempt.Body) && attempt.Catches.Any(handler =>
                        !DefinitelyWritesCancellationStorage(handler.Handler, symbol, isRef))))
                && CanThrowBeforeCancellationStorageOverwrite(write, attempt.Body, symbol, isRef))
            {
                return true;
            }
        }

        return false;
    }

    private static bool CanThrowBeforeCancellationStorageOverwrite(
        IOperation write, IBlockOperation body, ISymbol? symbol, bool? isRef)
    {
        var classifier = new OperationExceptionClassifier(write.SemanticModel);
        for (var current = write; current.Parent is { } parent; current = parent)
        {
            var sequence = parent switch
            {
                IBlockOperation block => block.Operations,
                ISwitchCaseOperation @case => @case.Body,
                _ => default,
            };
            if (!sequence.IsDefault)
            {
                if (CancellationStorageSequenceCanThrowBeforeOverwrite(
                    sequence.SkipWhile(operation => !ReferenceEquals(operation, current)).Skip(1),
                    symbol, isRef, classifier, out var overwritten))
                {
                    return true;
                }

                if (overwritten)
                {
                    return false;
                }
            }

            // Leaving a loop body evaluates its condition and iteration steps
            // before a later statement can overwrite the storage.
            if (parent is ILoopOperation loop && ReferenceEquals(current, loop.Body)
                && CanRepeatCancellationStorageLoop(loop))
            {
                if (CancellationStorageSequenceCanThrowBeforeOverwrite(
                    CancellationStorageLoopExitOperations(loop), symbol, isRef, classifier, out var overwritten))
                {
                    return true;
                }

                if (overwritten)
                {
                    return false;
                }
            }

            if (ReferenceEquals(parent, body))
            {
                break;
            }
        }

        return false;
    }

    private static IEnumerable<IOperation> CancellationStorageLoopExitOperations(ILoopOperation loop)
    {
        return loop switch
        {
            IWhileLoopOperation { Condition: { } condition } => (IEnumerable<IOperation>) [condition],
            IForLoopOperation forLoop => forLoop.Condition is { } condition
                ? forLoop.AtLoopBottom.Concat([condition])
                : forLoop.AtLoopBottom,
            _ => [],
        };
    }

    private static bool CancellationStorageOperationCanThrowBeforeOverwrite(
        IOperation operation, ISymbol? symbol, bool? isRef, OperationExceptionClassifier classifier, out bool overwritten)
    {
        overwritten = false;
        if (operation is IBlockOperation block)
        {
            return CancellationStorageSequenceCanThrowBeforeOverwrite(block.Operations, symbol, isRef, classifier, out overwritten);
        }

        if (operation is IConditionalOperation conditional)
        {
            return CancellationStorageBranchesCanThrowBeforeOverwrite(conditional, symbol, isRef, classifier, out overwritten);
        }

        overwritten = DefinitelyWritesCancellationStorage(operation, symbol, isRef);
        return operation.DescendantsAndSelf().Any(child =>
            ReferenceEquals(GetEnclosingCallable(child), GetEnclosingCallable(operation))
            && (child is IThrowOperation || !classifier.Classify(child).IsEmpty));
    }

    private static bool CancellationStorageSequenceCanThrowBeforeOverwrite(
        IEnumerable<IOperation> operations, ISymbol? symbol, bool? isRef,
        OperationExceptionClassifier classifier, out bool overwritten)
    {
        overwritten = false;
        foreach (var child in operations)
        {
            if (CancellationStorageOperationCanThrowBeforeOverwrite(child, symbol, isRef, classifier, out overwritten))
            {
                return true;
            }

            if (overwritten)
            {
                return false;
            }
        }

        return false;
    }

    private static bool CancellationStorageBranchesCanThrowBeforeOverwrite(
        IConditionalOperation conditional, ISymbol? symbol, bool? isRef,
        OperationExceptionClassifier classifier, out bool overwritten)
    {
        overwritten = false;
        if (conditional.Condition.ConstantValue is { HasValue: true, Value: bool condition })
        {
            return (condition ? conditional.WhenTrue : conditional.WhenFalse) is { } selected
                && CancellationStorageOperationCanThrowBeforeOverwrite(selected, symbol, isRef, classifier, out overwritten);
        }

        if (conditional.Condition.DescendantsAndSelf().Any(child => !classifier.Classify(child).IsEmpty)
            || CancellationStorageOperationCanThrowBeforeOverwrite(conditional.WhenTrue, symbol, isRef, classifier, out var whenTrue))
        {
            return true;
        }

        if (conditional.WhenFalse is not { } whenFalse)
        {
            return false;
        }

        var canThrow = CancellationStorageOperationCanThrowBeforeOverwrite(whenFalse, symbol, isRef, classifier, out var falseOverwrite);
        overwritten = whenTrue && falseOverwrite;
        return canThrow;
    }

    private static bool CanRepeatCancellationStorageLoop(ILoopOperation loop)
    {
        if ((loop is IWhileLoopOperation { Condition.ConstantValue: { HasValue: true, Value: bool condition } } whileLoop
                && condition == whileLoop.ConditionIsUntil)
            || loop is IForLoopOperation { Condition.ConstantValue: { HasValue: true, Value: false } })
        {
            return false;
        }

        var flow = loop.SemanticModel?.AnalyzeControlFlow(loop.Body.Syntax);
        return flow?.Succeeded != true || flow.EndPointIsReachable
            || loop.Body.DescendantsAndSelf().OfType<IBranchOperation>().Any(branch =>
                branch.BranchKind == BranchKind.GoTo
                || SymbolEqualityComparer.Default.Equals(branch.Target, loop.ContinueLabel));
    }

    private static bool ResetsCancellationStorageBackedges(IOperation reset, IOperation write, IOperation read)
    {
        // Every shared loop can carry the write back to the read. A reset in an
        // outer loop cannot kill writes from a subsequent inner iteration.
        for (var current = write.Parent; current is not null; current = current.Parent)
        {
            if (current is ILoopOperation loop && HasAncestor(read, current)
                && !HasAncestor(reset, current) && CanRepeatCancellationStorageLoop(loop))
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsDefiniteCancellationStorageBinding(IOperation binding, IOperation read)
    {
        var predecessor = GetCancellationStorageBindingPredecessor(binding, read);
        var sequence = GetCancellationStorageSequenceContainer(predecessor);

        // Internal jumps in a promoted block are not region entry points.
        // A later label can therefore bypass the assignment being proved.
        if (!ReferenceEquals(predecessor, binding)
            && predecessor.DescendantsAndSelf().OfType<ILabeledOperation>()
                .Any(label => label.Syntax.SpanStart > binding.Syntax.Span.End))
        {
            return false;
        }

        if (binding.Syntax.SpanStart >= read.Syntax.SpanStart || sequence is null
            || !HasAncestor(read, sequence) || HasBranchingAncestor(predecessor, sequence)
            || (!ReferenceEquals(binding, predecessor) && HasBranchingAncestor(binding, predecessor)))
        {
            return false;
        }

        return HasDefiniteCancellationStorageEntry(binding, predecessor, read, sequence);
    }

    private static IOperation GetCancellationStorageBindingPredecessor(IOperation binding, IOperation read)
    {
        var predecessor = binding;
        var sequence = GetCancellationStorageSequenceContainer(predecessor);
        while (sequence is IBlockOperation && !HasAncestor(read, sequence)
            && sequence.Parent is IBlockOperation or ILabeledOperation)
        {
            predecessor = sequence;
            sequence = GetCancellationStorageSequenceContainer(predecessor);
        }

        return predecessor;
    }

    private static bool HasDefiniteCancellationStorageEntry(
        IOperation binding, IOperation predecessor, IOperation read, IOperation sequence)
    {
        var first = predecessor.Syntax.AncestorsAndSelf().OfType<StatementSyntax>()
            .FirstOrDefault(statement => statement.Parent == sequence.Syntax);
        var last = read.Syntax.AncestorsAndSelf().OfType<StatementSyntax>()
            .FirstOrDefault(statement => statement.Parent == sequence.Syntax);
        if (first is null || last is null)
        {
            return false;
        }

        var flow = binding.SemanticModel?.AnalyzeControlFlow(first, last);
        return flow?.Succeeded == true
            && !flow.EntryPoints.Any(entry => entry.SpanStart > binding.Syntax.Span.End);
    }

    private static IOperation? GetCancellationStorageSequenceContainer(IOperation operation)
    {
        for (var current = operation.Parent; current is not null; current = current.Parent)
        {
            if (current is IBlockOperation or ISwitchCaseOperation)
            {
                return current;
            }
        }

        return null;
    }

    private static bool CanReachCancellationStorageRead(IOperation binding, IOperation read)
    {
        for (var current = binding; current.Parent is { } parent; current = parent)
        {
            if (AlwaysExitsCallable(current) && !HasAncestor(read, current)
                && !CanReachReadThroughCaughtCancellationStorageThrow(current, read))
            {
                return false;
            }

            if (IsUnreachableCancellationStorageCase(current, binding, read))
            {
                return false;
            }

            if (parent is IConditionalOperation conditional
                && IsReadInSiblingCancellationConditional(current, conditional, read)
                && !SharesCancellationStorageLoop(binding, read)
                && !CanRevisitCancellationStorageOperation(conditional))
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsReadInSiblingCancellationConditional(
        IOperation branch, IConditionalOperation conditional, IOperation read) =>
        (ReferenceEquals(conditional.WhenTrue, branch)
            && conditional.WhenFalse is { } whenFalse && HasAncestor(read, whenFalse))
        || (ReferenceEquals(conditional.WhenFalse, branch) && HasAncestor(read, conditional.WhenTrue));

    private static bool IsUnreachableCancellationStorageCase(IOperation current, IOperation binding, IOperation read) =>
        current is ISwitchCaseOperation sourceCase && current.Parent is ISwitchOperation switchOperation
        && switchOperation.Cases.FirstOrDefault(candidate => HasAncestor(read, candidate)) is { } targetCase
        && !ReferenceEquals(sourceCase, targetCase)
        && !SharesCancellationStorageLoop(binding, read)
        && !CanRevisitCancellationStorageOperation(switchOperation)
        && !CanTransferCancellationStorageBetweenCases(sourceCase, targetCase, switchOperation);

    private static bool CanRevisitCancellationStorageOperation(IOperation operation) =>
        GetCancellationStorageRevisitTargets(operation).Any();

    private static IEnumerable<ILabeledOperation> GetCancellationStorageRevisitTargets(IOperation operation)
    {
        var callable = GetEnclosingCallable(operation);
        var labels = GetRoot(operation).DescendantsAndSelf().OfType<ILabeledOperation>()
            .Where(label => label.Syntax.SpanStart <= operation.Syntax.SpanStart)
            .Where(label => ReferenceEquals(GetEnclosingCallable(label), callable))
            .ToArray();
        // A backward jump within or after a construct can carry a write into a
        // different branch on its next visit, like a structured loop backedge.
        var targets = GetRoot(operation).DescendantsAndSelf().OfType<IBranchOperation>()
            .Where(branch => branch.BranchKind == BranchKind.GoTo
                && branch.Syntax.SpanStart >= operation.Syntax.SpanStart
                && ReferenceEquals(GetEnclosingCallable(branch), callable))
            .Select(static branch => branch.Target).ToArray();
        return labels.Where(label => targets.Contains(label.Label, SymbolEqualityComparer.Default));
    }

    private static bool CanTransferCancellationStorageBetweenCases(
        ISwitchCaseOperation source,
        ISwitchCaseOperation target,
        ISwitchOperation switchOperation)
    {
        var pending = new Stack<ISwitchCaseOperation>();
        var visited = new HashSet<ISwitchCaseOperation>();
        pending.Push(source);
        while (pending.Count > 0)
        {
            var current = pending.Pop();
            if (ReferenceEquals(current, target))
            {
                return true;
            }

            if (!visited.Add(current))
            {
                continue;
            }

            var labels = current.DescendantsAndSelf().OfType<IBranchOperation>()
                .Where(static branch => branch.BranchKind == BranchKind.GoTo)
                .Select(static branch => branch.Target).ToArray();
            // A jump outside the switch may re-enter it through a later case.
            if (labels.Any(label => !switchOperation.Cases.Any(candidate =>
                    IsCancellationStorageCaseTarget(candidate, label))))
            {
                return true;
            }

            foreach (var candidate in switchOperation.Cases)
            {
                if (labels.Any(label => IsCancellationStorageCaseTarget(candidate, label)))
                {
                    pending.Push(candidate);
                }
            }
        }

        return false;
    }

    private static bool IsCancellationStorageCaseTarget(ISwitchCaseOperation candidate, ILabelSymbol label) =>
        candidate.Clauses.Any(clause => SymbolEqualityComparer.Default.Equals(label, clause.Label))
        || candidate.DescendantsAndSelf().OfType<ILabeledOperation>().Any(operation =>
            SymbolEqualityComparer.Default.Equals(label, operation.Label));

    private static bool CanReachReadThroughCaughtCancellationStorageThrow(IOperation operation, IOperation read)
    {
        if (!operation.DescendantsAndSelf().Any(static child => child is IThrowOperation))
        {
            return false;
        }

        for (var current = operation; current.Parent is { } parent; current = parent)
        {
            if (parent is ITryOperation attempt && ReferenceEquals(current, attempt.Body)
                && !HasAncestor(read, attempt.Body)
                && (attempt.Catches.Any(handler => !AlwaysExitsCallable(handler.Handler))
                    || (attempt.Finally is { } finallyBlock && HasAncestor(read, finallyBlock))))
            {
                return true;
            }
        }

        return false;
    }

    private static bool SharesCancellationStorageLoop(IOperation binding, IOperation read)
    {
        for (var current = binding.Parent; current is not null; current = current.Parent)
        {
            if (current is ILoopOperation loop && HasAncestor(read, current)
                && CanRepeatCancellationStorageLoop(loop))
            {
                return true;
            }
        }

        return false;
    }

    private static ISymbol? GetCancellationStorageSymbol(IOperation reference) => reference switch
    {
        IParameterReferenceOperation parameter => parameter.Parameter,
        ILocalReferenceOperation local => local.Local,
        _ => null,
    };

    private static bool FlowsFromParallelCallbackToken(
        IParameterReferenceOperation parameterReference,
        IReadOnlyCollection<IParameterSymbol> cancellationTokens,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods,
        HashSet<IOperation> visitedWrites)
    {
        if (!IsCancellationToken(parameterReference.Parameter)
            || parameterReference.Parameter.ContainingSymbol
                is not IMethodSymbol callback)
        {
            return false;
        }

        return GetRoot(parameterReference)
            .DescendantsAndSelf()
            .OfType<IInvocationOperation>()
            .Where(static invocation =>
                invocation.TargetMethod.Name == "ForEachAsync"
                && invocation.TargetMethod.ContainingType.ToDisplayString()
                == "System.Threading.Tasks.Parallel")
            .Where(invocation => InvocationTargetsCallable(invocation, callback))
            .SelectMany(static invocation => invocation.Arguments)
            .Where(static argument =>
                argument.Parameter is not null
                && IsCancellationToken(argument.Parameter))
            .Any(argument => FlowsFromCancellationToken(
                argument.Value,
                cancellationTokens,
                CloneVisitedLocals(visitedLocals),
                CloneVisitedMethods(visitedMethods),
                visitedWrites));
    }

    private static bool FlowsFromCancellationTokenProperty(
        IPropertyReferenceOperation propertyReference,
        IReadOnlyCollection<IParameterSymbol> cancellationTokens,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods,
        HashSet<IOperation> visitedWrites)
    {
        return IsCancellationTokenSourceToken(propertyReference)
               && propertyReference.Instance is not null
               && FlowsFromCancellationToken(
                   propertyReference.Instance,
                   cancellationTokens,
                   visitedLocals,
                   visitedMethods,
                   visitedWrites);
    }

    private static bool FlowsFromCancellationTokenInvocation(
        IInvocationOperation invocation,
        IReadOnlyCollection<IParameterSymbol> cancellationTokens,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods,
        HashSet<IOperation> visitedWrites)
    {
        if (IsKnownCancellationCarrierFactory(invocation))
        {
            return invocation.Arguments.Any(argument =>
                argument.Parameter is not null
                && IsCancellationInput(argument.Parameter)
                && FlowsFromCancellationToken(
                    argument.Value,
                    cancellationTokens,
                    visitedLocals,
                    visitedMethods,
                    visitedWrites));
        }

        return FlowsFromCancellationTokenSourceHelper(
            invocation,
            cancellationTokens,
            visitedLocals,
            visitedMethods,
            visitedWrites);
    }

    private static bool FlowsFromCancellationTokenSourceHelper(
        IInvocationOperation invocation,
        IReadOnlyCollection<IParameterSymbol> cancellationTokens,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods,
        HashSet<IOperation> visitedWrites)
    {
        var method = NormalizeMethod(invocation.TargetMethod);
        if (!visitedMethods.Add(method)
            || invocation.SemanticModel?.Compilation is not { } compilation
            || GetMethodOperation(method, compilation, default)
                is not { } operation)
        {
            return false;
        }

        var flowedParameters = GetFlowedCancellationParameters(
            invocation,
            method,
            cancellationTokens,
            visitedLocals,
            visitedMethods,
            visitedWrites);
        if (flowedParameters.Count == 0)
        {
            return false;
        }

        var returnValues = operation.DescendantsAndSelf()
            .OfType<IReturnOperation>()
            .Where(static returnOperation =>
                GetEnclosingCallable(returnOperation) is null
                && IsInReachableBranch(returnOperation))
            .Select(static returnOperation => returnOperation.ReturnedValue)
            .OfType<IOperation>()
            .ToArray();
        return returnValues.Length > 0
               && returnValues
                   .SelectMany(GetReachableValueLeaves)
                   .Where(static returnValue => !AlwaysThrows(returnValue))
                   .All(returnValue =>
                       FlowsFromCancellationToken(
                           returnValue,
                           flowedParameters,
                           CloneVisitedLocals(visitedLocals),
                           CloneVisitedMethods(visitedMethods),
                           visitedWrites));
    }

    private static HashSet<IParameterSymbol> GetFlowedCancellationParameters(
        IInvocationOperation invocation,
        IMethodSymbol method,
        IReadOnlyCollection<IParameterSymbol> cancellationTokens,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods,
        HashSet<IOperation> visitedWrites)
    {
        var flowedParameters = new HashSet<IParameterSymbol>(
            invocation.Arguments
                .Where(static argument =>
                    argument.Parameter is not null
                    && IsCancellationToken(argument.Parameter))
                .Where(argument => FlowsFromCancellationToken(
                    argument.Value,
                    cancellationTokens,
                    CloneVisitedLocals(visitedLocals),
                    CloneVisitedMethods(visitedMethods),
                    visitedWrites))
                .Select(argument => GetTargetParameter(method, argument.Parameter))
                .OfType<IParameterSymbol>(),
            SymbolEqualityComparer.Default);
        if (TryGetFlowedCancellationReceiver(
                invocation,
                method,
                cancellationTokens,
                visitedLocals,
                visitedMethods,
                visitedWrites) is { } receiverParameter)
        {
            flowedParameters.Add(receiverParameter);
        }

        return flowedParameters;
    }

    private static IParameterSymbol? TryGetFlowedCancellationReceiver(
        IInvocationOperation invocation,
        IMethodSymbol method,
        IReadOnlyCollection<IParameterSymbol> cancellationTokens,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods,
        HashSet<IOperation> visitedWrites)
    {
        if (invocation.TargetMethod.ReducedFrom is not null
            && invocation.Instance is { } instance
            && method.Parameters.FirstOrDefault() is { } receiverParameter
            && IsCancellationToken(receiverParameter)
            && FlowsFromCancellationToken(
                instance,
                cancellationTokens,
                CloneVisitedLocals(visitedLocals),
                CloneVisitedMethods(visitedMethods),
                visitedWrites))
        {
            return receiverParameter;
        }

        return null;
    }

    private static bool IsKnownCancellationCarrierFactory(
        IInvocationOperation invocation)
    {
        return IsCancellationCarrier(invocation.Type)
               && invocation.TargetMethod.Name == "CreateLinkedTokenSource"
               && invocation.TargetMethod.ContainingType.ToDisplayString()
               == "System.Threading.CancellationTokenSource";
    }

    private static bool IsCancellationInput(IParameterSymbol parameter)
    {
        return IsCancellationToken(parameter)
               || (parameter.Type is IArrayTypeSymbol arrayType
                   && arrayType.ElementType.ToDisplayString()
                   == CancellationTokenMetadataName)
               || (parameter.Type is INamedTypeSymbol { IsGenericType: true } namedType
                   && namedType.TypeArguments.Length == 1
                   && namedType.OriginalDefinition.ToDisplayString() is
                       "System.Span<T>"
                       or "System.ReadOnlySpan<T>"
                   && namedType.TypeArguments[0].ToDisplayString()
                   == CancellationTokenMetadataName);
    }

    private static bool FlowsFromCancellationTokenConditional(
        IConditionalOperation conditional,
        IReadOnlyCollection<IParameterSymbol> cancellationTokens,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods,
        HashSet<IOperation> visitedWrites)
    {
        if (conditional.Condition.ConstantValue is { HasValue: true, Value: bool condition })
        {
            var selectedBranch = condition
                ? conditional.WhenTrue
                : conditional.WhenFalse;
            return BranchFlowsFromCancellationToken(
                selectedBranch,
                cancellationTokens,
                visitedLocals,
                visitedMethods,
                visitedWrites);
        }

        return BranchFlowsFromCancellationToken(
                   conditional.WhenTrue,
                   cancellationTokens,
                   CloneVisitedLocals(visitedLocals),
                   CloneVisitedMethods(visitedMethods),
                   visitedWrites)
               && BranchFlowsFromCancellationToken(
                   conditional.WhenFalse,
                   cancellationTokens,
                   CloneVisitedLocals(visitedLocals),
                   CloneVisitedMethods(visitedMethods),
                   visitedWrites);
    }

    private static bool BranchFlowsFromCancellationToken(
        IOperation? branch,
        IReadOnlyCollection<IParameterSymbol> cancellationTokens,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods,
        HashSet<IOperation> visitedWrites)
    {
        return branch is not null
               && (AlwaysThrows(branch)
                   || FlowsFromCancellationToken(
                       branch,
                       cancellationTokens,
                       visitedLocals,
                       visitedMethods,
                       visitedWrites));
    }

    private static bool FlowsFromCancellationTokenSwitchExpression(
        ISwitchExpressionOperation switchExpression,
        IReadOnlyCollection<IParameterSymbol> cancellationTokens,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods,
        HashSet<IOperation> visitedWrites)
    {
        return switchExpression.Arms.Length > 0
               && switchExpression.Arms
                   .Where(arm => !IsDeadSwitchExpressionArm(arm, switchExpression))
                   .Where(static arm => !AlwaysThrows(arm.Value))
                   .All(arm =>
                    FlowsFromCancellationToken(
                        arm.Value,
                        cancellationTokens,
                        CloneVisitedLocals(visitedLocals),
                        CloneVisitedMethods(visitedMethods),
                        visitedWrites));
    }

    private static bool AlwaysThrows(IOperation operation)
    {
        return operation switch
        {
            IConversionOperation conversion => AlwaysThrows(conversion.Operand),
            IThrowOperation => true,
            _ => false,
        };
    }

    private static IEnumerable<IOperation> GetReachableValueLeaves(IOperation operation)
    {
        return operation switch
        {
            IConversionOperation conversion => GetReachableValueLeaves(conversion.Operand),
            IConditionalOperation conditional => GetReachableConditionalLeaves(conditional),
            ICoalesceOperation coalesce => GetReachableCoalesceLeaves(coalesce),
            ISwitchExpressionOperation switchExpression => switchExpression.Arms
                .Where(arm => !IsDeadSwitchExpressionArm(arm, switchExpression))
                .SelectMany(arm => GetReachableValueLeaves(arm.Value)),
            _ => [operation],
        };
    }

    private static IEnumerable<IOperation> GetReachableConditionalLeaves(
        IConditionalOperation conditional)
    {
        if (conditional.Condition.ConstantValue is { HasValue: true, Value: bool condition })
        {
            return GetReachableValueLeaves(
                condition ? conditional.WhenTrue : conditional.WhenFalse!);
        }

        return GetReachableValueLeaves(conditional.WhenTrue)
            .Concat(conditional.WhenFalse is null
                ? []
                : GetReachableValueLeaves(conditional.WhenFalse));
    }

    private static IEnumerable<IOperation> GetReachableCoalesceLeaves(
        ICoalesceOperation coalesce)
    {
        if (coalesce.Value.ConstantValue is { HasValue: true, Value: null })
        {
            return GetReachableValueLeaves(coalesce.WhenNull);
        }

        return IsStaticallyNonNullCoalesceValue(coalesce.Value)
            ? GetReachableValueLeaves(coalesce.Value)
            : GetReachableValueLeaves(coalesce.Value)
                .Concat(GetReachableValueLeaves(coalesce.WhenNull));
    }

    private static bool FlowsFromCancellationTokenCoalesce(
        ICoalesceOperation coalesce,
        IReadOnlyCollection<IParameterSymbol> cancellationTokens,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods,
        HashSet<IOperation> visitedWrites)
    {
        if (coalesce.Value.ConstantValue is { HasValue: true, Value: null })
        {
            return FlowsFromCancellationToken(
                coalesce.WhenNull,
                cancellationTokens,
                visitedLocals,
                visitedMethods,
                visitedWrites);
        }

        if (IsStaticallyNonNullCoalesceValue(coalesce.Value))
        {
            return FlowsFromCancellationToken(
                coalesce.Value,
                cancellationTokens,
                visitedLocals,
                visitedMethods,
                visitedWrites);
        }

        return FlowsFromCancellationToken(
                   coalesce.Value,
                   cancellationTokens,
                   CloneVisitedLocals(visitedLocals),
                   CloneVisitedMethods(visitedMethods),
                   visitedWrites)
               && FlowsFromCancellationToken(
                   coalesce.WhenNull,
                   cancellationTokens,
                   CloneVisitedLocals(visitedLocals),
                   CloneVisitedMethods(visitedMethods),
                   visitedWrites);
    }

    private static bool IsStaticallyNonNullCoalesceValue(IOperation value)
    {
        while (value is IConversionOperation conversion)
        {
            if (conversion.Operand.Type is { IsValueType: true } operandType
                && !IsNullableValueType(operandType))
            {
                return true;
            }

            value = conversion.Operand;
        }

        return false;
    }

    private static bool IsNullableValueType(ITypeSymbol type) =>
        type is INamedTypeSymbol
        {
            OriginalDefinition.SpecialType: SpecialType.System_Nullable_T,
        };

    private static HashSet<ILocalSymbol> CloneVisitedLocals(
        HashSet<ILocalSymbol> visitedLocals) =>
        new(visitedLocals, visitedLocals.Comparer);

    private static bool FlowsFromCancellationTokenLocal(
        ILocalReferenceOperation localReference,
        IReadOnlyCollection<IParameterSymbol> cancellationTokens,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<IMethodSymbol> visitedMethods,
        HashSet<IOperation> visitedWrites)
    {
        if (!visitedLocals.Add(localReference.Local))
        {
            return false;
        }

        var localValues = FindReachingLocalValues(
                localReference,
                localReference.Local)
            .ToArray();
        return localValues.Length > 0
               && localValues.All(localValue =>
                   FlowsFromCancellationToken(
                       localValue,
                       cancellationTokens,
                       CloneVisitedLocals(visitedLocals),
                       CloneVisitedMethods(visitedMethods),
                       visitedWrites));
    }

    private static bool IsCancellationTokenSourceToken(
        IPropertyReferenceOperation propertyReference)
    {
        return propertyReference.Property.Name == "Token"
               && propertyReference.Property.ContainingType.ToDisplayString()
               == "System.Threading.CancellationTokenSource";
    }

    private static bool IsCancellationCarrier(ITypeSymbol? type)
    {
        return type?.ToDisplayString() is
            CancellationTokenMetadataName
            or "System.Threading.CancellationTokenSource";
    }

    private static IEnumerable<IOperation> GetValueAndReachingLocalValues(
        IOperation value,
        HashSet<ILocalSymbol> visitedLocals)
    {
        yield return value;

        foreach (var localReference in value.DescendantsAndSelf().OfType<ILocalReferenceOperation>())
        {
            if (!visitedLocals.Add(localReference.Local)
                || FindReachingLocalValue(value, localReference.Local) is not { } localValue)
            {
                continue;
            }

            foreach (var operation in GetValueAndReachingLocalValues(localValue, visitedLocals))
            {
                yield return operation;
            }
        }
    }

    private static IEnumerable<IOperation> GetValueAndReachingCallbackValues(
        IOperation value,
        Compilation compilation,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<ISymbol> visitedMembers,
        HashSet<IMethodSymbol> visitedMethods)
    {
        foreach (var candidate in GetValueAndReachingLocalValues(value, visitedLocals))
        {
            yield return candidate;

            foreach (var operation in GetInvocationCallbackValues(
                         candidate,
                         compilation,
                         visitedLocals,
                         visitedMembers,
                         visitedMethods))
            {
                yield return operation;
            }

            foreach (var operation in GetMemberCallbackValues(
                         candidate,
                         compilation,
                         visitedLocals,
                         visitedMembers,
                         visitedMethods))
            {
                yield return operation;
            }
        }
    }

    private static IEnumerable<IOperation> GetInvocationCallbackValues(
        IOperation candidate,
        Compilation compilation,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<ISymbol> visitedMembers,
        HashSet<IMethodSymbol> visitedMethods)
    {
        if (candidate is not IInvocationOperation invocation)
        {
            yield break;
        }

        var nextVisitedMethods = CloneVisitedMethods(visitedMethods);
        foreach (var returnValue in GetSourceInvocationReturnValues(
                     invocation,
                     compilation,
                     nextVisitedMethods).ToArray())
        {
            foreach (var operation in GetValueAndReachingCallbackValues(
                         returnValue,
                         compilation,
                         CloneVisitedLocals(visitedLocals),
                         new HashSet<ISymbol>(visitedMembers, SymbolEqualityComparer.Default),
                         CloneVisitedMethods(nextVisitedMethods)))
            {
                yield return operation;
            }
        }
    }

    private static IEnumerable<IOperation> GetMemberCallbackValues(
        IOperation candidate,
        Compilation compilation,
        HashSet<ILocalSymbol> visitedLocals,
        HashSet<ISymbol> visitedMembers,
        HashSet<IMethodSymbol> visitedMethods)
    {
        foreach (var memberReference in candidate.DescendantsAndSelf()
                     .Where(static operation => operation is
                         IFieldReferenceOperation
                             or IPropertyReferenceOperation
                             or IEventReferenceOperation))
        {
            var member = GetReferencedMember(memberReference);
            if (member is null || !visitedMembers.Add(member))
            {
                continue;
            }

            foreach (var memberValue in GetMemberValues(
                         memberReference,
                         member,
                         compilation).ToArray())
            {
                foreach (var operation in GetValueAndReachingCallbackValues(
                             memberValue,
                             compilation,
                             visitedLocals,
                             visitedMembers,
                             visitedMethods))
                {
                    yield return operation;
                }
            }
        }
    }

    private static IEnumerable<IOperation> GetSourceInvocationReturnValues(
        IInvocationOperation invocation,
        Compilation compilation,
        HashSet<IMethodSymbol> visitedMethods)
    {
        var method = NormalizeMethod(invocation.TargetMethod);
        if (!visitedMethods.Add(method)
            || GetMethodOperation(method, compilation, default) is not { } operation)
        {
            yield break;
        }

        foreach (var returnValue in operation.DescendantsAndSelf()
                     .OfType<IReturnOperation>()
                     .Where(static returnOperation =>
                         GetEnclosingCallable(returnOperation) is null
                         && IsInReachableBranch(returnOperation))
                     .Select(static returnOperation => returnOperation.ReturnedValue)
                     .OfType<IOperation>()
                     .SelectMany(GetReachableValueLeaves))
        {
            if (returnValue is not IParameterReferenceOperation parameterReference)
            {
                yield return returnValue;
                continue;
            }

            var argument = invocation.Arguments.FirstOrDefault(candidate =>
                SymbolEqualityComparer.Default.Equals(
                    GetTargetParameter(method, candidate.Parameter),
                    parameterReference.Parameter));
            if (argument is not null)
            {
                yield return argument.Value;
            }
        }
    }

    private static IEnumerable<IOperation> GetConstructorMemberValues(
        ISymbol member,
        Compilation compilation)
    {
        var constructors = member.IsStatic
            ? member.ContainingType.StaticConstructors
            : member.ContainingType.InstanceConstructors;
        foreach (var constructor in constructors)
        {
            foreach (var syntaxReference in constructor.DeclaringSyntaxReferences)
            {
                foreach (var value in GetConstructorSyntaxMemberValues(
                             syntaxReference,
                             member,
                             compilation))
                {
                    yield return value;
                }
            }
        }
    }

    private static IEnumerable<IOperation> GetConstructorSyntaxMemberValues(
        SyntaxReference syntaxReference,
        ISymbol member,
        Compilation compilation)
    {
        var syntaxTree = syntaxReference.SyntaxTree;
        if (!compilation.ContainsSyntaxTree(syntaxTree))
        {
            return [];
        }

        var semanticModel = compilation.GetSemanticModel(syntaxTree);
        var operation = syntaxReference.GetSyntax()
            .DescendantNodesAndSelf()
            .Select(syntax => semanticModel.GetOperation(syntax))
            .OfType<IOperation>()
            .FirstOrDefault();
        return operation is null
            ? []
            : GetFinalMemberValues(operation, member);
    }

    private static IEnumerable<IOperation> GetFinalMemberValues(
        IOperation operation,
        ISymbol member)
    {
        var operations = operation.DescendantsAndSelf().ToArray();
        var assignments = operations
            .OfType<ISimpleAssignmentOperation>()
            .Where(IsInReachableBranch)
            .Where(static assignment => GetEnclosingCallable(assignment) is null)
            .Where(assignment => SymbolEqualityComparer.Default.Equals(
                GetReferencedMember(assignment.Target),
                member))
            .OrderByDescending(static assignment => assignment.Syntax.SpanStart)
            .ToArray();
        var linearAssignment = assignments.FirstOrDefault(assignment =>
            ReferenceEquals(
                GetContainingBlock(assignment),
                GetOutermostContainingBlock(assignment)));
        var branchingOperation = GetLatestBranchingMemberAssignment(
            operations,
            member);
        var lowerBound = Math.Max(
            linearAssignment?.Syntax.SpanStart ?? int.MinValue,
            branchingOperation?.Syntax.SpanStart ?? int.MinValue);

        foreach (var assignment in assignments.Where(assignment =>
                     assignment.Syntax.SpanStart >= lowerBound))
        {
            yield return assignment.Value;
        }
    }

    private static IConditionalOperation? GetLatestBranchingMemberAssignment(
        IEnumerable<IOperation> operations,
        ISymbol member) =>
        operations
            .OfType<IConditionalOperation>()
            .Where(IsInReachableBranch)
            .Where(static conditional => GetEnclosingCallable(conditional) is null)
            .Where(conditional => ReferenceEquals(
                GetContainingBlock(conditional),
                GetOutermostContainingBlock(conditional)))
            .Where(conditional =>
                conditional.WhenFalse is not null
                && DefinitelyAssignsMember(conditional.WhenTrue, member)
                && DefinitelyAssignsMember(conditional.WhenFalse, member))
            .OrderByDescending(static conditional => conditional.Syntax.SpanStart)
            .FirstOrDefault();

    private static bool DefinitelyAssignsMember(
        IOperation operation,
        ISymbol member)
    {
        if (operation is ISimpleAssignmentOperation assignment
            && SymbolEqualityComparer.Default.Equals(
                GetReferencedMember(assignment.Target),
                member))
        {
            return true;
        }

        return operation switch
        {
            IBlockOperation block => block.Operations.Any(candidate =>
                DefinitelyAssignsMember(candidate, member)),
            IConditionalOperation { WhenFalse: { } whenFalse } conditional =>
                DefinitelyAssignsMember(conditional.WhenTrue, member)
                && DefinitelyAssignsMember(whenFalse, member),
            IExpressionStatementOperation expressionStatement =>
                DefinitelyAssignsMember(expressionStatement.Operation, member),
            _ => false,
        };
    }

    private static IBlockOperation? GetOutermostContainingBlock(IOperation operation)
    {
        IBlockOperation? result = null;
        for (var current = operation.Parent; current is not null; current = current.Parent)
        {
            if (current is IBlockOperation block)
            {
                result = block;
            }
        }

        return result;
    }

    private static IEnumerable<IOperation> GetDeclaredMemberOperations(
        ISymbol member,
        Compilation compilation)
    {
        foreach (var syntaxReference in member.DeclaringSyntaxReferences)
        {
            var syntaxTree = syntaxReference.SyntaxTree;
            if (!compilation.ContainsSyntaxTree(syntaxTree))
            {
                continue;
            }

            var semanticModel = compilation.GetSemanticModel(syntaxTree);
            foreach (var syntax in syntaxReference.GetSyntax().DescendantNodesAndSelf())
            {
                if (semanticModel.GetOperation(syntax) is { } operation)
                {
                    yield return operation;
                }
            }
        }
    }

    private static IOperation? FindReachingLocalValue(
        IOperation operation,
        ILocalSymbol local)
    {
        return FindReachingLocalValue(operation, local, out _);
    }

    private static IEnumerable<IOperation> FindReachingLocalValues(
        IOperation operation,
        ILocalSymbol local)
    {
        var root = GetRoot(operation);
        var callable = GetEnclosingCallable(operation);
        var assignments = FindLocalAssignments(root, operation, local, callable);
        var linearAssignment = assignments.FirstOrDefault(candidate =>
            IsLinearPredecessor(candidate, operation));
        var branchingOperation = FindLatestDefinitelyAssigningBranch(
            root,
            operation,
            local,
            callable);
        var lowerBound = Math.Max(
            linearAssignment?.Syntax.SpanStart ?? int.MinValue,
            branchingOperation?.Syntax.SpanStart ?? int.MinValue);

        if (branchingOperation is not null
            && branchingOperation.Syntax.SpanStart == lowerBound)
        {
            var branchValues = GetReachingLocalAssignments(
                    branchingOperation,
                    local)
                .Select(static assignment => assignment.Value);
            var subsequentValues = assignments
                .Where(candidate =>
                    candidate.Syntax.SpanStart > branchingOperation.Syntax.Span.End)
                .Where(IsInReachableBranch)
                .Select(static assignment => assignment.Value);
            return subsequentValues.Concat(branchValues);
        }

        var assignedValues = assignments
            .Where(candidate => candidate.Syntax.SpanStart >= lowerBound)
            .Where(IsInReachableBranch)
            .Select(static assignment => assignment.Value);
        if (linearAssignment is not null || branchingOperation is not null)
        {
            return assignedValues;
        }

        var declarator = FindLocalDeclarator(root, operation, local, callable);
        return declarator?.Initializer?.Value is { } initialValue
            ? assignedValues.Append(initialValue)
            : assignedValues;
    }

    private static IOperation? FindLatestDefinitelyAssigningBranch(
        IOperation root,
        IOperation operation,
        ILocalSymbol local,
        IOperation? callable)
    {
        return root.DescendantsAndSelf()
            .Where(static candidate =>
                candidate is IConditionalOperation
                    or ISwitchOperation
                    or IWhileLoopOperation { ConditionIsTop: false }
                    or ITryOperation)
            .Where(candidate => candidate.Syntax.SpanStart < operation.Syntax.SpanStart)
            .Where(candidate => ReferenceEquals(GetEnclosingCallable(candidate), callable))
            .Where(candidate => IsLinearPredecessor(candidate, operation))
            .Where(candidate => DefinitelyAssignsLocal(candidate, local))
            .OrderByDescending(static candidate => candidate.Syntax.SpanStart)
            .FirstOrDefault();
    }

    private static IEnumerable<ISimpleAssignmentOperation>
        GetReachingLocalAssignments(
            IOperation operation,
            ILocalSymbol local)
    {
        if (!IsInReachableBranch(operation))
        {
            return [];
        }

        return operation switch
        {
            ISimpleAssignmentOperation assignment
                when AssignsLocal(assignment, local) =>
                [assignment],
            IConditionalOperation conditional =>
                GetReachingLocalAssignments(conditional.WhenTrue, local)
                    .Concat(conditional.WhenFalse is { } whenFalse
                        ? GetReachingLocalAssignments(whenFalse, local)
                        : []),
            ISwitchOperation switchOperation =>
                switchOperation.Cases.SelectMany(@case =>
                    GetReachingSequenceAssignments(@case.Body, local)),
            IWhileLoopOperation { ConditionIsTop: false } whileLoop =>
                GetReachingLocalAssignments(whileLoop.Body, local),
            ITryOperation tryOperation =>
                GetReachingLocalAssignments(tryOperation, local),
            IBlockOperation block =>
                GetReachingSequenceAssignments(block.Operations, local),
            IExpressionStatementOperation expressionStatement =>
                GetReachingLocalAssignments(
                    expressionStatement.Operation,
                    local),
            _ => [],
        };
    }

    private static IEnumerable<ISimpleAssignmentOperation>
        GetReachingLocalAssignments(
            ITryOperation tryOperation,
            ILocalSymbol local)
    {
        if (tryOperation.Finally is { } finallyBlock
            && DefinitelyAssignsLocal(finallyBlock, local))
        {
            return GetReachingLocalAssignments(finallyBlock, local);
        }

        var tryAndCatchAssignments =
            GetReachingLocalAssignments(tryOperation.Body, local)
                .Concat(tryOperation.Catches.SelectMany(catchClause =>
                    GetReachingLocalAssignments(catchClause.Handler, local)));
        return tryOperation.Finally is { } partialFinally
            ? tryAndCatchAssignments.Concat(
                GetReachingLocalAssignments(partialFinally, local))
            : tryAndCatchAssignments;
    }

    private static List<ISimpleAssignmentOperation>
        GetReachingSequenceAssignments(
            IEnumerable<IOperation> operations,
            ILocalSymbol local)
    {
        var assignments = new List<ISimpleAssignmentOperation>();
        foreach (var operation in operations.Reverse())
        {
            assignments.AddRange(GetReachingLocalAssignments(operation, local));
            if (DefinitelyAssignsLocal(operation, local))
            {
                break;
            }
        }

        return assignments;
    }

    private static bool AssignsLocal(
        ISimpleAssignmentOperation assignment,
        ILocalSymbol local)
    {
        return assignment.Target is ILocalReferenceOperation localReference
               && SymbolEqualityComparer.Default.Equals(localReference.Local, local);
    }

    private static bool DefinitelyAssignsLocal(
        IOperation operation,
        ILocalSymbol local)
    {
        if (operation is ISimpleAssignmentOperation assignment
            && AssignsLocal(assignment, local))
        {
            return true;
        }

        if (operation is IConditionalOperation conditional)
        {
            return conditional.WhenFalse is not null
                   && DefinitelyAssignsLocal(conditional.WhenTrue, local)
                   && DefinitelyAssignsLocal(conditional.WhenFalse, local);
        }

        return operation switch
        {
            IBlockOperation block => block.Operations.Any(candidate =>
                DefinitelyAssignsLocal(candidate, local)),
            ISwitchOperation switchOperation =>
                DefinitelyAssignsLocalInSwitch(switchOperation, local),
            IWhileLoopOperation { ConditionIsTop: false } whileLoop =>
                DefinitelyAssignsLocalInDoLoop(whileLoop, local),
            ITryOperation tryOperation =>
                DefinitelyAssignsLocalInTry(tryOperation, local),
            IExpressionStatementOperation expressionStatement =>
                DefinitelyAssignsLocal(expressionStatement.Operation, local),
            _ => false,
        };
    }

    private static bool DefinitelyAssignsLocalInTry(
        ITryOperation tryOperation,
        ILocalSymbol local)
    {
        if (tryOperation.Finally is { } finallyBlock
            && DefinitelyAssignsLocal(finallyBlock, local))
        {
            return true;
        }

        return DefinitelyAssignsLocal(tryOperation.Body, local)
               && tryOperation.Catches.All(catchClause =>
                   DefinitelyAssignsLocal(catchClause.Handler, local)
                   || AlwaysExitsCallable(catchClause.Handler));
    }

    private static bool DefinitelyAssignsLocalInDoLoop(
        IWhileLoopOperation whileLoop,
        ILocalSymbol local)
    {
        var dataFlow = whileLoop.Body.SemanticModel?.AnalyzeDataFlow(
            whileLoop.Body.Syntax);
        return dataFlow?.Succeeded == true
               && dataFlow.AlwaysAssigned.Contains(
                   local,
                   SymbolEqualityComparer.Default);
    }

    private static bool DefinitelyAssignsLocalInSwitch(
        ISwitchOperation switchOperation,
        ILocalSymbol local)
    {
        return switchOperation.Cases.Any(@case =>
                   @case.Clauses.Any(static clause =>
                       clause is IDefaultCaseClauseOperation))
               && switchOperation.Cases.All(@case =>
                   SwitchCaseCannotReachFollowingOperation(@case)
                   || @case.Body.Any(candidate =>
                       DefinitelyAssignsLocal(candidate, local)));
    }

    private static bool SwitchCaseCannotReachFollowingOperation(
        ISwitchCaseOperation switchCase)
    {
        return switchCase.Body.Length > 0
               && AlwaysExitsCallable(switchCase.Body[switchCase.Body.Length - 1]);
    }

    private static bool AlwaysExitsCallable(IOperation operation)
    {
        return operation switch
        {
            IReturnOperation or IThrowOperation => true,
            IBlockOperation { Operations.Length: > 0 } block =>
                AlwaysExitsCallable(block.Operations[block.Operations.Length - 1]),
            IConditionalOperation { WhenFalse: { } whenFalse } conditional =>
                AlwaysExitsCallable(conditional.WhenTrue)
                && AlwaysExitsCallable(whenFalse),
            _ => false,
        };
    }

    private static IOperation? FindReachingLocalValue(
        IOperation operation,
        ILocalSymbol local,
        out bool isAmbiguous)
    {
        isAmbiguous = false;
        var root = GetRoot(operation);
        var callable = GetEnclosingCallable(operation);
        var assignments = FindLocalAssignments(root, operation, local, callable);
        var assignment = assignments.FirstOrDefault(candidate =>
            IsLinearPredecessor(candidate, operation));
        if (assignments.Any(candidate =>
                !IsLinearPredecessor(candidate, operation)
                && (assignment is null
                    || candidate.Syntax.SpanStart > assignment.Syntax.SpanStart)))
        {
            isAmbiguous = true;
            return null;
        }

        if (assignment is not null)
        {
            return assignment.Value;
        }

        var declarator = FindLocalDeclarator(root, operation, local, callable);
        if (declarator is null)
        {
            return null;
        }

        if (!IsLinearPredecessor(declarator, operation))
        {
            isAmbiguous = true;
            return null;
        }

        return declarator.Initializer?.Value;
    }

    private static ISimpleAssignmentOperation[] FindLocalAssignments(
        IOperation root,
        IOperation operation,
        ILocalSymbol local,
        IOperation? callable)
    {
        return
        [
            .. root.DescendantsAndSelf()
            .OfType<ISimpleAssignmentOperation>()
            .Where(candidate => candidate.Syntax.SpanStart < operation.Syntax.SpanStart)
            .Where(candidate => candidate.Target is ILocalReferenceOperation localReference
                && SymbolEqualityComparer.Default.Equals(localReference.Local, local))
            .Where(candidate => ReferenceEquals(GetEnclosingCallable(candidate), callable))
            .OrderByDescending(static candidate => candidate.Syntax.SpanStart),
        ];
    }

    private static IVariableDeclaratorOperation? FindLocalDeclarator(
        IOperation root,
        IOperation operation,
        ILocalSymbol local,
        IOperation? callable)
    {
        return root.DescendantsAndSelf()
            .OfType<IVariableDeclaratorOperation>()
            .Where(declarator => declarator.Syntax.SpanStart < operation.Syntax.SpanStart)
            .Where(declarator => SymbolEqualityComparer.Default.Equals(declarator.Symbol, local))
            .Where(declarator => ReferenceEquals(GetEnclosingCallable(declarator), callable))
            .OrderByDescending(static declarator => declarator.Syntax.SpanStart)
            .FirstOrDefault();
    }

    private static bool IsLinearPredecessor(
        IOperation candidate,
        IOperation operation)
    {
        var containingBlock = GetContainingBlock(candidate);
        return containingBlock is not null
               && HasAncestor(operation, containingBlock)
               && !HasBranchingAncestor(candidate, containingBlock);
    }

    private static IBlockOperation? GetContainingBlock(IOperation operation)
    {
        for (var ancestor = operation.Parent; ancestor is not null; ancestor = ancestor.Parent)
        {
            if (ancestor is IBlockOperation block)
            {
                return block;
            }
        }

        return null;
    }

    private static bool HasBranchingAncestor(
        IOperation operation,
        IOperation containingBlock)
    {
        for (var ancestor = operation.Parent;
             ancestor is not null && !ReferenceEquals(ancestor, containingBlock);
             ancestor = ancestor.Parent)
        {
            if (ancestor is IConditionalOperation
                or ILoopOperation
                or ISwitchOperation)
            {
                return true;
            }
        }

        return false;
    }

    private static bool HasAncestor(
        IOperation operation,
        IOperation expectedAncestor)
    {
        for (var ancestor = operation; ancestor is not null; ancestor = ancestor.Parent)
        {
            if (ReferenceEquals(ancestor, expectedAncestor))
            {
                return true;
            }
        }

        return false;
    }

    private static IOperation? GetEnclosingCallable(IOperation operation)
    {
        for (var current = operation.Parent; current is not null; current = current.Parent)
        {
            if (current is IAnonymousFunctionOperation or ILocalFunctionOperation)
            {
                return current;
            }
        }

        return null;
    }

    private static IOperation GetRoot(IOperation operation)
    {
        while (operation.Parent is not null)
        {
            operation = operation.Parent;
        }

        return operation;
    }

    private static bool InvocationAcceptsCancellationToken(
        IMethodSymbol method,
        Compilation compilation,
        INamedTypeSymbol within)
    {
        if (method.Parameters.Any(IsCancellationToken))
        {
            return true;
        }

        var selectedMethod = method.ReducedFrom ?? method;
        return selectedMethod.ContainingType.GetMembers(selectedMethod.Name)
            .OfType<IMethodSymbol>()
            .Where(candidate => candidate.IsStatic == selectedMethod.IsStatic)
            .Where(candidate => compilation.IsSymbolAccessibleWithin(candidate, within))
            .Any(candidate => IsCancellationOverload(
                selectedMethod,
                candidate,
                compilation));
    }

    private static bool IsCancellationOverload(
        IMethodSymbol selectedMethod,
        IMethodSymbol candidate,
        Compilation compilation)
    {
        if (candidate.Arity != selectedMethod.Arity)
        {
            return false;
        }

        if (candidate.IsGenericMethod)
        {
            candidate = candidate.Construct([.. selectedMethod.TypeArguments]);
            if (!SatisfiesGenericConstraints(candidate, compilation))
            {
                return false;
            }
        }

        if (!HasCompatibleAwaitedResult(
                selectedMethod.ReturnType,
                candidate.ReturnType))
        {
            return false;
        }

        var candidateParameters = candidate.Parameters
            .Where(parameter => !IsCancellationToken(parameter))
            .ToArray();
        if (candidateParameters.Length < selectedMethod.Parameters.Length
            || candidate.Parameters.Length == candidateParameters.Length)
        {
            return false;
        }

        return candidateParameters
                   .Take(selectedMethod.Parameters.Length)
            .Zip(
                selectedMethod.Parameters,
                static (left, right) => left.RefKind == right.RefKind
                    && SymbolEqualityComparer.Default.Equals(left.Type, right.Type))
            .All(static matches => matches)
               && candidateParameters
                   .Skip(selectedMethod.Parameters.Length)
                   .All(static parameter => parameter.IsOptional);
    }

    private static bool SatisfiesGenericConstraints(
        IMethodSymbol method,
        Compilation compilation)
    {
        for (var index = 0; index < method.TypeParameters.Length; index++)
        {
            var parameter = method.TypeParameters[index];
            var argument = method.TypeArguments[index];
            if (argument is ITypeParameterSymbol
                || (parameter.HasReferenceTypeConstraint && !argument.IsReferenceType)
                || (parameter.HasValueTypeConstraint && !SatisfiesValueTypeConstraint(argument))
                || (parameter.HasUnmanagedTypeConstraint && !argument.IsUnmanagedType)
                || (parameter.HasConstructorConstraint
                    && !HasPublicParameterlessConstructor(argument))
                || parameter.ConstraintTypes.Any(constraint =>
                    !compilation.ClassifyCommonConversion(argument, constraint).IsImplicit))
            {
                return false;
            }
        }

        return true;
    }

    private static bool SatisfiesValueTypeConstraint(ITypeSymbol type)
    {
        return type.IsValueType
               && type.OriginalDefinition.SpecialType != SpecialType.System_Nullable_T;
    }

    private static bool HasPublicParameterlessConstructor(ITypeSymbol type)
    {
        return type.IsValueType
               || (type is INamedTypeSymbol namedType
                   && !namedType.IsAbstract
                   && namedType.InstanceConstructors.Any(static constructor =>
                       constructor.Parameters.Length == 0
                       && constructor.DeclaredAccessibility == Accessibility.Public));
    }

    private static bool HasCompatibleAwaitedResult(
        ITypeSymbol selectedReturnType,
        ITypeSymbol candidateReturnType)
    {
        if (SymbolEqualityComparer.Default.Equals(
                selectedReturnType,
                candidateReturnType))
        {
            return true;
        }

        return TryGetTaskResultType(selectedReturnType, out var selectedResultType)
               && TryGetTaskResultType(candidateReturnType, out var candidateResultType)
               && SymbolEqualityComparer.Default.Equals(
                   selectedResultType,
                   candidateResultType);
    }

    private static bool TryGetTaskResultType(
        ITypeSymbol returnType,
        out ITypeSymbol? resultType)
    {
        for (var current = returnType as INamedTypeSymbol;
             current is not null;
             current = current.BaseType)
        {
            switch (current.OriginalDefinition.ToDisplayString())
            {
                case "System.Threading.Tasks.Task":
                case "System.Threading.Tasks.ValueTask":
                    resultType = null;
                    return true;
                case "System.Threading.Tasks.Task<TResult>":
                case "System.Threading.Tasks.ValueTask<TResult>":
                    resultType = current.TypeArguments[0];
                    return true;
            }
        }

        resultType = null;
        return false;
    }

    private static bool IsCancellationToken(IParameterSymbol parameter)
    {
        return parameter.Type.ToDisplayString() == CancellationTokenMetadataName;
    }

    private static bool IsPublic(INamedTypeSymbol type)
    {
        for (var current = type; current is not null; current = current.ContainingType)
        {
            if (current.DeclaredAccessibility != Accessibility.Public)
            {
                return false;
            }
        }

        return true;
    }
}
