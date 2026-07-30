using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using ModularPipelines.Analyzers.Extensions;

namespace ModularPipelines.Analyzers;

internal static class ModuleAuthoringAnalysis
{
    private const string AssemblyMetadataName = "System.Reflection.Assembly";
    private const string CancellationTokenMetadataName = "System.Threading.CancellationToken";
    private const string EventArgsMetadataName = "System.EventArgs";

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

    public static void InitializeRegistrationAnalysis(AnalysisContext context)
    {
        context.RegisterCompilationStartAction(StartRegistrationAnalysis);
    }

    public static void InitializeAsyncSafetyAnalysis(AnalysisContext context)
    {
        context.RegisterCompilationStartAction(StartAsyncVoidAnalysis);
        context.RegisterOperationAction(AnalyzeInvocation, OperationKind.Invocation);
        context.RegisterOperationAction(AnalyzePropertyReference, OperationKind.PropertyReference);
        context.RegisterOperationAction(AnalyzeAwait, OperationKind.Await);
        context.RegisterOperationAction(AnalyzeForEachLoop, OperationKind.Loop);
        context.RegisterOperationAction(AnalyzeReturn, OperationKind.Return);
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

        context.RegisterSymbolAction(
            symbolContext => CollectModuleType(symbolContext, modules),
            SymbolKind.NamedType);
        context.RegisterOperationAction(
            operationContext => TrackRegistration(
                operationContext,
                registeredModules,
                instanceRegisteredModules,
                scannedAssemblies),
            OperationKind.Invocation);
        context.RegisterOperationAction(
            operationContext => TrackServiceCollectionIndexerAssignment(
                operationContext,
                instanceRegisteredModules),
            OperationKind.SimpleAssignment);
        context.RegisterCompilationEndAction(endContext =>
            ReportModuleDiagnostics(
                endContext,
                modules,
                registeredModules,
                instanceRegisteredModules,
                scannedAssemblies));
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

    private static void CollectEventHandlerMethod(
        OperationAnalysisContext context,
        ConcurrentBag<IMethodSymbol> eventHandlerMethods)
    {
        var eventAssignment = (IEventAssignmentOperation) context.Operation;
        foreach (var methodReference in eventAssignment.HandlerValue
                     .DescendantsAndSelf()
                     .OfType<IMethodReferenceOperation>())
        {
            eventHandlerMethods.Add(methodReference.Method);
        }
    }

    private static void ReportAsyncVoidDiagnostics(
        CompilationAnalysisContext context,
        ConcurrentBag<IMethodSymbol> asyncVoidMethods,
        ConcurrentBag<IMethodSymbol> eventHandlerMethods)
    {
        foreach (var method in asyncVoidMethods)
        {
            if (HasEventHandlerSignature(method, context.Compilation)
                || eventHandlerMethods.Any(eventHandler =>
                    SymbolEqualityComparer.Default.Equals(eventHandler, method))
                || method.Locations.FirstOrDefault(static item => item.IsInSource)
                    is not { } location)
            {
                continue;
            }

            context.ReportDiagnostic(Diagnostic.Create(
                ModuleAsyncSafetyAnalyzer.AsyncVoidRule,
                location,
                method.Name));
        }
    }

    private static void TrackRegistration(
        OperationAnalysisContext context,
        ConcurrentBag<INamedTypeSymbol> registeredModules,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        ConcurrentBag<IAssemblySymbol> scannedAssemblies)
    {
        var invocation = (IInvocationOperation) context.Operation;
        TrackRegistrationInvocation(
            invocation,
            context.Compilation.Assembly,
            IsApplication(context.Compilation.Options.OutputKind),
            registeredModules,
            instanceRegisteredModules,
            scannedAssemblies);
    }

    private static void AnalyzeInvocation(OperationAnalysisContext context)
    {
        var invocation = (IInvocationOperation) context.Operation;

        if (GetModuleExecutionMethod(context) is null)
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
            || propertyReference.Property.Name != "Result"
            || !IsBlockingResultType(
                propertyReference.Property.ContainingType,
                context.Compilation))
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

    private static void AnalyzeAwait(OperationAnalysisContext context)
    {
        if (GetModuleExecutionMethod(context) is not { } executionMethod
            || context.Operation is not IAwaitOperation awaitOperation)
        {
            return;
        }

        AnalyzeCancellationInvocations(
            context,
            executionMethod,
            GetAwaitedInvocations(awaitOperation.Operation));
    }

    private static void AnalyzeForEachLoop(OperationAnalysisContext context)
    {
        if (GetModuleExecutionMethod(context) is not { } executionMethod
            || context.Operation is not IForEachLoopOperation { IsAsynchronous: true } loop)
        {
            return;
        }

        AnalyzeCancellationInvocations(
            context,
            executionMethod,
            GetAwaitedInvocations(loop.Collection));
    }

    private static void AnalyzeReturn(OperationAnalysisContext context)
    {
        if (context.ContainingSymbol is not IMethodSymbol method
            || method.MethodKind is not (MethodKind.Ordinary or MethodKind.PropertyGet)
            || method.IsAsync
            || GetModuleExecutionMethod(context) is not { } executionMethod
            || context.Operation is not IReturnOperation { ReturnedValue: { } returnedValue })
        {
            return;
        }

        AnalyzeCancellationInvocations(
            context,
            executionMethod,
            GetAwaitedInvocations(returnedValue));
    }

    private static void AnalyzeCancellationInvocations(
        OperationAnalysisContext context,
        IMethodSymbol executeMethod,
        IEnumerable<IInvocationOperation> invocations)
    {
        var cancellationTokens = GetFlowingCancellationTokens(
            context,
            executeMethod);
        if (cancellationTokens.IsEmpty)
        {
            return;
        }

        foreach (var invocation in invocations)
        {
            if (cancellationTokens.Any(
                    cancellationToken =>
                        InvocationUsesCancellation(invocation, cancellationToken))
                || !InvocationAcceptsCancellationToken(
                    invocation.TargetMethod,
                    context.Compilation,
                    executeMethod.ContainingType))
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
        foreach (var arm in switchExpression.Arms.Reverse())
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

    private static ImmutableArray<IParameterSymbol> GetFlowingCancellationTokens(
        OperationAnalysisContext context,
        IMethodSymbol executeMethod)
    {
        if (executeMethod.Parameters.FirstOrDefault(IsCancellationToken)
            is not { } executeCancellationToken)
        {
            return [];
        }

        var containingMethod = GetContainingMemberMethod(context.ContainingSymbol);
        if (containingMethod is null
            || SymbolEqualityComparer.Default.Equals(
                containingMethod,
                executeMethod))
        {
            return [executeCancellationToken];
        }

        return [executeCancellationToken, .. FindMappedCancellationTokens(
            executeMethod,
            containingMethod,
            executeCancellationToken,
            context.Compilation,
            context.CancellationToken)];
    }

    private static IMethodSymbol? GetContainingMemberMethod(ISymbol symbol)
    {
        for (var method = symbol as IMethodSymbol;
             method is not null;
             method = method.ContainingSymbol as IMethodSymbol)
        {
            if (method.MethodKind is MethodKind.Ordinary or MethodKind.PropertyGet)
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
        var analysisType = executeMethod.ContainingType.InheritsFrom(
            targetMethod.ContainingType)
            ? executeMethod.ContainingType
            : targetMethod.ContainingType;
        var memberMethods = GetModuleMemberMethods(analysisType, compilation);
        var mappedTokens = ImmutableArray.CreateBuilder<IParameterSymbol>();
        var visitedTokens = new HashSet<IParameterSymbol>(
            SymbolEqualityComparer.Default)
        {
            executeCancellationToken,
        };
        var pending = new Queue<(IMethodSymbol Method, IParameterSymbol Token)>();
        pending.Enqueue((executeMethod, executeCancellationToken));

        while (pending.Count > 0)
        {
            var (method, token) = pending.Dequeue();
            if (SymbolEqualityComparer.Default.Equals(method, targetMethod))
            {
                mappedTokens.Add(token);
                continue;
            }

            if (GetMethodOperation(method, compilation, cancellationToken)
                is not { } operation)
            {
                continue;
            }

            foreach (var invocation in GetReachableInvocations(operation))
            {
                foreach (var mapping in GetCancellationTokenMappings(
                             invocation,
                             memberMethods,
                             token))
                {
                    if (visitedTokens.Add(mapping.Token))
                    {
                        pending.Enqueue(mapping);
                    }
                }
            }
        }

        return mappedTokens.ToImmutable();
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
            nestedCallables);

        return invocations.Where(invocation =>
            IsInsideReachableCallable(
                invocation,
                reachableNestedCallables));
    }

    private static IEnumerable<(IMethodSymbol Method, IParameterSymbol Token)>
        GetCancellationTokenMappings(
            IInvocationOperation invocation,
            ImmutableArray<IMethodSymbol> memberMethods,
            IParameterSymbol sourceToken)
    {
        foreach (var method in memberMethods.Where(candidate =>
                     InvocationTargetsCallable(invocation, candidate)))
        {
            foreach (var argument in invocation.Arguments.Where(argument =>
                         argument.Parameter is not null
                         && IsCancellationToken(argument.Parameter)
                         && FlowsFromCancellationToken(
                             argument.Value,
                             sourceToken,
                             [with(SymbolEqualityComparer.Default)])))
            {
                var targetToken = method.Parameters.ElementAtOrDefault(
                    argument.Parameter!.Ordinal);
                if (targetToken is not null && IsCancellationToken(targetToken))
                {
                    yield return (method, targetToken);
                }
            }
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
                         ReferenceEquals(
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
        IAssemblySymbol currentAssembly,
        bool isApplication,
        ConcurrentBag<INamedTypeSymbol> registeredModules,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        ConcurrentBag<IAssemblySymbol> scannedAssemblies)
    {
        var method = invocation.TargetMethod;
        if (TryTrackDirectModuleServiceRegistration(
                invocation,
                instanceRegisteredModules))
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
                currentAssembly,
                isApplication,
                scannedAssemblies);
            return;
        }

        TrackGenericModuleRegistrations(
            invocation,
            registeredModules,
            instanceRegisteredModules);
        if (method.Name == "AddModules")
        {
            TrackDynamicModuleRegistrations(
                invocation,
                registeredModules);
        }
    }

    private static bool IsModuleRegistrationMethod(IMethodSymbol method)
    {
        var definition = (method.ReducedFrom ?? method).OriginalDefinition;
        return definition.ContainingAssembly.Name == "ModularPipelines"
               && definition.ContainingType.ToDisplayString() is
                   "ModularPipelines.Extensions.PipelineBuilderExtensions"
                   or "ModularPipelines.Extensions.ServiceCollectionExtensions"
               && definition.Name is
                   "AddModule"
                   or "AddModules"
                   or "AddModulesFromAssembly"
                   or "AddModulesFromAssemblyContainingType";
    }

    private static bool TryTrackDirectModuleServiceRegistration(
        IInvocationOperation invocation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules)
    {
        var method = invocation.TargetMethod;
        var definition = (method.ReducedFrom ?? method).OriginalDefinition;
        if (IsServiceDescriptorRegistrationMethod(invocation))
        {
            return TryTrackServiceDescriptorArguments(
                invocation,
                instanceRegisteredModules);
        }

        if (!IsDirectServiceRegistrationMethod(definition)
            || !RegistersModuleService(invocation, method))
        {
            return false;
        }

        if (!TryTrackDirectImplementationType(
                invocation,
                method,
                instanceRegisteredModules))
        {
            TrackDirectImplementationValue(
                invocation,
                instanceRegisteredModules);
        }

        return true;
    }

    private static bool IsDirectServiceRegistrationMethod(IMethodSymbol definition)
    {
        var containingType = definition.ContainingType.ToDisplayString();
        return (definition.Name is "AddSingleton" or "AddScoped" or "AddTransient"
                && containingType is
                    "Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions"
                    or "ModularPipelines.Extensions.PipelineBuilderExtensions")
               || (definition.Name is "TryAddSingleton" or "TryAddScoped" or "TryAddTransient"
                   && containingType
                   == "Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions");
    }

    private static bool IsServiceDescriptorRegistrationMethod(
        IInvocationOperation invocation)
    {
        if (invocation.TargetMethod.Name is not ("Add" or "Insert" or "TryAdd" or "TryAddEnumerable")
            || !InvocationTargetsServiceCollection(invocation))
        {
            return false;
        }

        return invocation.TargetMethod.Parameters.Any(static parameter =>
            IsServiceDescriptorType(parameter.Type));
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
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules)
    {
        var descriptorArguments = invocation.Arguments
            .Where(argument => argument.Parameter is not null
                               && IsServiceDescriptorType(argument.Parameter.Type))
            .ToArray();
        return descriptorArguments.Any(argument =>
            TryTrackServiceDescriptor(
                argument.Value,
                instanceRegisteredModules,
                [with(SymbolEqualityComparer.Default)]));
    }

    private static bool TryTrackServiceDescriptor(
        IOperation operation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        HashSet<ILocalSymbol> visitedLocals)
    {
        switch (operation)
        {
            case IConversionOperation conversion:
                return TryTrackServiceDescriptor(
                    conversion.Operand,
                    instanceRegisteredModules,
                    visitedLocals);
            case ILocalReferenceOperation localReference
                when visitedLocals.Add(localReference.Local)
                     && FindReachingLocalValue(operation, localReference.Local) is { } localValue:
                return TryTrackServiceDescriptor(
                    localValue,
                    instanceRegisteredModules,
                    visitedLocals);
            case IInvocationOperation descriptorFactory
                when IsServiceDescriptorFactory(descriptorFactory.TargetMethod):
                return TrackServiceDescriptor(
                    descriptorFactory.Arguments,
                    descriptorFactory.TargetMethod.TypeArguments,
                    instanceRegisteredModules);
            case IObjectCreationOperation objectCreation
                when objectCreation.Type?.ToDisplayString()
                     == "Microsoft.Extensions.DependencyInjection.ServiceDescriptor":
                return TrackServiceDescriptor(
                    objectCreation.Arguments,
                    objectCreation.Constructor?.TypeArguments
                    ?? [],
                    instanceRegisteredModules);
            case IArrayCreationOperation { Initializer: { } initializer }:
                return TryTrackServiceDescriptorCollection(
                    initializer.ElementValues,
                    instanceRegisteredModules,
                    visitedLocals);
            case ICollectionExpressionOperation collection:
                return TryTrackServiceDescriptorCollection(
                    collection.Elements,
                    instanceRegisteredModules,
                    visitedLocals);
            case ISpreadOperation spread:
                return TryTrackServiceDescriptor(
                    spread.Operand,
                    instanceRegisteredModules,
                    visitedLocals);
            default:
                return false;
        }
    }

    private static void TrackServiceCollectionIndexerAssignment(
        OperationAnalysisContext context,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules)
    {
        var assignment = (ISimpleAssignmentOperation) context.Operation;
        if (assignment.Target is not IPropertyReferenceOperation indexer
            || !indexer.Property.IsIndexer
            || !IsServiceCollectionType(indexer.Instance?.Type)
            || !IsServiceDescriptorType(indexer.Property.Type))
        {
            return;
        }

        TryTrackServiceDescriptor(
            assignment.Value,
            instanceRegisteredModules,
            [with(SymbolEqualityComparer.Default)]);
    }

    private static bool TryTrackServiceDescriptorCollection(
        IEnumerable<IOperation> elements,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        HashSet<ILocalSymbol> visitedLocals)
    {
        var tracked = false;
        foreach (var element in elements)
        {
            tracked |= TryTrackServiceDescriptor(
                element,
                instanceRegisteredModules,
                CloneVisitedLocals(visitedLocals));
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
            && TryGetTypeOfNamedType(
                implementationTypeArgument.Value,
                [with(SymbolEqualityComparer.Default)],
                out implementationType))
        {
            instanceRegisteredModules.Add(implementationType.OriginalDefinition);
            return true;
        }

        return arguments
            .Where(static argument => argument.Parameter?.Name is
                "implementationInstance" or "implementationFactory")
            .Any(argument => TryTrackInstanceModuleTypes(
                argument.Value,
                instanceRegisteredModules,
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
            == "ModularPipelines.Modules.IModule")
        {
            return true;
        }

        return arguments.Any(argument =>
            argument.Parameter?.Name == "serviceType"
            && TryGetTypeOfNamedType(
                argument.Value,
                [with(SymbolEqualityComparer.Default)],
                out var serviceType)
            && serviceType.ToDisplayString()
            == "ModularPipelines.Modules.IModule");
    }

    private static bool TryTrackDirectImplementationType(
        IInvocationOperation invocation,
        IMethodSymbol method,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules)
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

        if (!TryGetTypeOfNamedType(
                implementationTypeArgument.Value,
                [with(SymbolEqualityComparer.Default)],
                out implementationType))
        {
            return false;
        }

        instanceRegisteredModules.Add(implementationType.OriginalDefinition);
        return true;
    }

    private static void TrackDirectImplementationValue(
        IInvocationOperation invocation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules)
    {
        _ = invocation.Arguments
            .Where(static argument => argument.Parameter?.Name is
                "implementationInstance" or "implementationFactory")
            .Any(argument => TryTrackInstanceModuleTypes(
                argument.Value,
                instanceRegisteredModules,
                [with(SymbolEqualityComparer.Default)]));
    }

    private static bool TryGetTypeOfNamedType(
        IOperation operation,
        HashSet<ILocalSymbol> visitedLocals,
        out INamedTypeSymbol namedType)
    {
        switch (operation)
        {
            case IConversionOperation conversion:
                return TryGetTypeOfNamedType(
                    conversion.Operand,
                    visitedLocals,
                    out namedType);
            case ITypeOfOperation { TypeOperand: INamedTypeSymbol typeOperand }:
                namedType = typeOperand;
                return true;
            case ILocalReferenceOperation localReference
                when visitedLocals.Add(localReference.Local)
                     && FindReachingLocalValue(operation, localReference.Local) is { } localValue:
                return TryGetTypeOfNamedType(
                    localValue,
                    visitedLocals,
                    out namedType);
            default:
                namedType = null!;
                return false;
        }
    }

    private static void TrackGenericModuleRegistrations(
        IInvocationOperation invocation,
        ConcurrentBag<INamedTypeSymbol> registeredModules,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules)
    {
        var method = invocation.TargetMethod;
        foreach (var typeArgument in method.TypeArguments.OfType<INamedTypeSymbol>())
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
                instanceRegisteredModules,
                [with(SymbolEqualityComparer.Default)]);
        }
    }

    private static bool HasEventHandlerSignature(
        IMethodSymbol method,
        Compilation compilation)
    {
        return method.Parameters.Length == 2
               && method.Parameters[0].RefKind == RefKind.None
               && method.Parameters[0].Type.SpecialType == SpecialType.System_Object
               && method.Parameters[1].RefKind == RefKind.None
               && method.Parameters[1].Type.InheritsFrom(
                   compilation,
                   EventArgsMetadataName);
    }

    private static bool TryTrackInstanceModuleTypes(
        IOperation operation,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        HashSet<ILocalSymbol> visitedLocals)
    {
        switch (operation)
        {
            case IConversionOperation conversion:
                return TryTrackInstanceModuleTypes(
                    conversion.Operand,
                    instanceRegisteredModules,
                    visitedLocals);
            case IDelegateCreationOperation delegateCreation:
                return TryTrackInstanceModuleTypes(
                    delegateCreation.Target,
                    instanceRegisteredModules,
                    visitedLocals);
            case IObjectCreationOperation { Type: INamedTypeSymbol moduleType }:
                var normalizedType = moduleType.OriginalDefinition;
                instanceRegisteredModules.Add(normalizedType);
                return true;
            case IInvocationOperation { Type: INamedTypeSymbol moduleType }:
                instanceRegisteredModules.Add(moduleType.OriginalDefinition);
                return true;
            case ILocalReferenceOperation localReference
                when visitedLocals.Add(localReference.Local)
                     && FindReachingLocalValue(operation, localReference.Local) is { } localValue:
                return TryTrackInstanceModuleTypes(
                    localValue,
                    instanceRegisteredModules,
                    visitedLocals);
            case IAnonymousFunctionOperation anonymousFunction:
                var returnValues = anonymousFunction.Body
                    .DescendantsAndSelf()
                    .OfType<IReturnOperation>()
                    .Where(returnOperation =>
                        ReferenceEquals(
                            GetEnclosingCallable(returnOperation),
                            anonymousFunction))
                    .Select(static returnOperation => returnOperation.ReturnedValue)
                    .OfType<IOperation>()
                    .ToArray();
                return returnValues.Length > 0
                       && returnValues.All(returnValue =>
                           TryTrackInstanceModuleTypes(
                               returnValue,
                               instanceRegisteredModules,
                               CloneVisitedLocals(visitedLocals)));
            case IConditionalOperation conditional:
                return TryTrackInstanceModuleTypes(
                           conditional.WhenTrue,
                           instanceRegisteredModules,
                           CloneVisitedLocals(visitedLocals))
                       && conditional.WhenFalse is { } whenFalse
                       && TryTrackInstanceModuleTypes(
                           whenFalse,
                           instanceRegisteredModules,
                           CloneVisitedLocals(visitedLocals));
            default:
                return false;
        }
    }

    private static void TrackDynamicModuleRegistrations(
        IInvocationOperation invocation,
        ConcurrentBag<INamedTypeSymbol> registeredModules)
    {
        foreach (var argument in invocation.Arguments.Where(static argument =>
                     argument.Parameter?.Name == "moduleTypes"))
        {
            _ = TryTrackModuleTypes(
                argument.Value,
                registeredModules,
                [with(SymbolEqualityComparer.Default)]);
        }
    }

    private static bool TryTrackModuleTypes(
        IOperation operation,
        ConcurrentBag<INamedTypeSymbol> registeredModules,
        HashSet<ILocalSymbol> visitedLocals)
    {
        switch (operation)
        {
            case IConversionOperation conversion:
                return TryTrackModuleTypes(
                    conversion.Operand,
                    registeredModules,
                    visitedLocals);
            case ITypeOfOperation { TypeOperand: INamedTypeSymbol moduleType }:
                registeredModules.Add(moduleType.OriginalDefinition);
                return true;
            case ILocalReferenceOperation localReference
                when visitedLocals.Add(localReference.Local)
                     && FindReachingLocalValue(operation, localReference.Local) is { } localValue:
                return TryTrackModuleTypes(
                    localValue,
                    registeredModules,
                    visitedLocals);
            case IArrayCreationOperation { Initializer: { } initializer }:
                return initializer.ElementValues.All(element =>
                    TryTrackModuleTypes(
                        element,
                        registeredModules,
                        CloneVisitedLocals(visitedLocals)));
            case ICollectionExpressionOperation collection:
                return collection.Elements.All(element =>
                    TryTrackModuleTypes(
                        element,
                        registeredModules,
                        CloneVisitedLocals(visitedLocals)));
            case ISpreadOperation spread:
                return TryTrackModuleTypes(
                    spread.Operand,
                    registeredModules,
                    visitedLocals);
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
                currentAssembly,
                isApplication,
                scannedAssemblies,
                [with(SymbolEqualityComparer.Default)]);
        }
    }

    private static bool TryTrackScannedAssembly(
        IOperation operation,
        IAssemblySymbol currentAssembly,
        bool isApplication,
        ConcurrentBag<IAssemblySymbol> scannedAssemblies,
        HashSet<ILocalSymbol> visitedLocals)
    {
        switch (operation)
        {
            case IConversionOperation conversion:
                return TryTrackScannedAssembly(
                    conversion.Operand,
                    currentAssembly,
                    isApplication,
                    scannedAssemblies,
                    visitedLocals);
            case ILocalReferenceOperation localReference
                when visitedLocals.Add(localReference.Local)
                     && FindReachingLocalValue(operation, localReference.Local) is { } localValue:
                return TryTrackScannedAssembly(
                    localValue,
                    currentAssembly,
                    isApplication,
                    scannedAssemblies,
                    visitedLocals);
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
            case IInvocationOperation invocation
                when invocation.TargetMethod.Name == "GetExecutingAssembly"
                     && invocation.TargetMethod.ContainingType.ToDisplayString()
                     == AssemblyMetadataName:
                scannedAssemblies.Add(currentAssembly);
                return true;
            case IInvocationOperation invocation
                when invocation.TargetMethod.Name == "GetEntryAssembly"
                     && invocation.TargetMethod.ContainingType.ToDisplayString()
                     == AssemblyMetadataName:
                if (isApplication)
                {
                    scannedAssemblies.Add(currentAssembly);
                }

                return true;
            default:
                return false;
        }
    }

    private static void ReportModuleDiagnostics(
        CompilationAnalysisContext context,
        ConcurrentBag<INamedTypeSymbol> modules,
        ConcurrentBag<INamedTypeSymbol> registeredModules,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        ConcurrentBag<IAssemblySymbol> scannedAssemblies)
    {
        var moduleSet = modules
            .Distinct<INamedTypeSymbol>(SymbolEqualityComparer.Default)
            .ToImmutableArray();
        var instanceRegistered = instanceRegisteredModules.ToImmutableHashSet<INamedTypeSymbol>(
            SymbolEqualityComparer.Default);
        var scanned = scannedAssemblies.ToImmutableHashSet<IAssemblySymbol>(
            SymbolEqualityComparer.Default);
        foreach (var module in moduleSet.Where(module =>
                     !IsPublic(module)
                     && !instanceRegistered.Contains(module)
                     && !IsAssemblyScannedModule(module, scanned)))
        {
            ReportModuleDiagnostic(
                context,
                module,
                ModuleRegistrationAnalyzer.NonPublicModuleRule);
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
        if (module.IsAbstract || !module.IsModule(context.Compilation))
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
        if (method.MethodKind is not (MethodKind.Ordinary or MethodKind.PropertyGet)
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
        List<IMethodSymbol> nestedCallables)
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
        var reachableCallables = GetReachableCallables(invocations, callables);

        return nestedCallables.All(reachableCallables.Contains);
    }

    private static HashSet<IMethodSymbol> GetReachableCallables(
        ImmutableArray<IInvocationOperation> invocations,
        ImmutableArray<IMethodSymbol> callables)
    {
        var reachable = new HashSet<IMethodSymbol>(SymbolEqualityComparer.Default);
        var pending = new Queue<IMethodSymbol>();

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
                     SymbolEqualityComparer.Default.Equals(
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

        if (invocation.Instance is not null
            && ValueContainsCallable(
                invocation.Instance,
                callable,
                [with(SymbolEqualityComparer.Default)]))
        {
            return true;
        }

        return IsKnownDelegateInvoker(invocation)
               && invocation.Arguments.Any(argument =>
                   ValueContainsCallable(
                       argument.Value,
                       callable,
                       [with(SymbolEqualityComparer.Default)]));
    }

    private static bool IsKnownDelegateInvoker(IInvocationOperation invocation)
    {
        var method = invocation.TargetMethod;
        var containingType = method.ContainingType.OriginalDefinition.ToDisplayString();
        return (method.Name == "Run"
                && containingType == "System.Threading.Tasks.Task")
               || (method.Name == "ContinueWith"
                   && containingType is
                       "System.Threading.Tasks.Task"
                       or "System.Threading.Tasks.Task<TResult>")
               || (method.Name == "StartNew"
                   && containingType is
                       "System.Threading.Tasks.TaskFactory"
                       or "System.Threading.Tasks.TaskFactory<TResult>")
               || (method.Name == "ForEach"
                   && containingType is
                       "System.Array"
                       or "System.Collections.Generic.List<T>"
                       or "System.Threading.Tasks.Parallel")
               || (containingType == "System.Threading.Tasks.Parallel"
                   && method.Name is "For" or "ForEachAsync" or "Invoke")
               || (containingType == "System.Linq.Enumerable"
                   && IsLinqCallbackInvoked(invocation));
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
            var analysisType = candidate.ContainingType.InheritsFrom(
                targetMethod.ContainingType)
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
                () => [.. GetNamedTypes(currentCompilation.Assembly.GlobalNamespace)
                    .SelectMany(type => type.GetMembers(
                        AnalyzerConstants.MethodNames.ExecuteAsync))
                    .OfType<IMethodSymbol>()
                    .Where(method => IsModuleExecuteAsync(method, currentCompilation))])).Value;
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
        return [.. GetModuleTypeHierarchy(analysisType, compilation)
            .SelectMany(static type => type.GetMembers())
            .OfType<IMethodSymbol>()
            .Where(static method =>
                method.MethodKind is MethodKind.Ordinary or MethodKind.PropertyGet
                && method.DeclaringSyntaxReferences.Length > 0)];
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
        var nestedCallables = operation.DescendantsAndSelf()
            .Select(GetCallableSymbol)
            .Where(static callable => callable is not null)
            .Cast<IMethodSymbol>()
            .Distinct<IMethodSymbol>(SymbolEqualityComparer.Default)
            .ToImmutableArray();
        var reachableNestedCallables = GetReachableCallables(
            invocations,
            nestedCallables);

        return GetInvokedMethods(
                invocations,
                memberMethods,
                reachableNestedCallables)
            .Concat(GetReferencedPropertyGetters(
                propertyReferences,
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
            if (!IsInsideReachableCallable(invocation, reachableNestedCallables))
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

    private static IEnumerable<IMethodSymbol> GetReferencedPropertyGetters(
        ImmutableArray<IPropertyReferenceOperation> propertyReferences,
        ImmutableArray<IMethodSymbol> memberMethods,
        HashSet<IMethodSymbol> reachableNestedCallables)
    {
        foreach (var propertyReference in propertyReferences)
        {
            if (!IsInsideReachableCallable(
                    propertyReference,
                    reachableNestedCallables))
            {
                continue;
            }

            foreach (var method in memberMethods)
            {
                if (PropertyReferenceTargetsGetter(propertyReference, method))
                {
                    yield return method;
                }
            }
        }
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
        return propertyReference.Property.GetMethod is { } getter
               && SymbolEqualityComparer.Default.Equals(
                   getter.OriginalDefinition,
                   method.OriginalDefinition)
               && (propertyReference.Parent is not ISimpleAssignmentOperation assignment
                   || !ReferenceEquals(assignment.Target, propertyReference));
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
        if (value.DescendantsAndSelf()
            .OfType<IAnonymousFunctionOperation>()
            .Any(candidate => SymbolEqualityComparer.Default.Equals(
                candidate.Symbol,
                callable))
            || value.DescendantsAndSelf()
                .OfType<IMethodReferenceOperation>()
                .Any(candidate => SymbolEqualityComparer.Default.Equals(
                    candidate.Method.OriginalDefinition,
                    callable.OriginalDefinition)))
        {
            return true;
        }

        foreach (var localReference in value.DescendantsAndSelf().OfType<ILocalReferenceOperation>())
        {
            if (visitedLocals.Add(localReference.Local)
                && FindReachingLocalValue(localReference, localReference.Local) is { } localValue
                && ValueContainsCallable(localValue, callable, visitedLocals))
            {
                return true;
            }
        }

        return false;
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
        HashSet<ILocalSymbol> visitedLocals)
    {
        return value switch
        {
            IConversionOperation conversion => FlowsFromCancellationToken(
                conversion.Operand,
                cancellationToken,
                visitedLocals),
            IParameterReferenceOperation parameterReference =>
                SymbolEqualityComparer.Default.Equals(
                    parameterReference.Parameter,
                    cancellationToken)
                || FlowsFromParallelCallbackToken(
                    parameterReference,
                    cancellationToken,
                    visitedLocals),
            ILocalReferenceOperation localReference =>
                FlowsFromCancellationTokenLocal(
                    localReference,
                    cancellationToken,
                    visitedLocals),
            IPropertyReferenceOperation propertyReference =>
                FlowsFromCancellationTokenProperty(
                    propertyReference,
                    cancellationToken,
                    visitedLocals),
            IInvocationOperation invocation =>
                FlowsFromCancellationTokenInvocation(
                    invocation,
                    cancellationToken,
                    visitedLocals),
            IArrayCreationOperation { Initializer: { } initializer } =>
                FlowsFromCancellationToken(
                    initializer,
                    cancellationToken,
                    visitedLocals),
            IArrayInitializerOperation initializer =>
                initializer.ElementValues.Any(element => FlowsFromCancellationToken(
                    element,
                    cancellationToken,
                    visitedLocals)),
            ICollectionExpressionOperation collection =>
                collection.Elements.Any(element => FlowsFromCancellationToken(
                    element,
                    cancellationToken,
                    visitedLocals)),
            ISpreadOperation spread =>
                FlowsFromCancellationToken(
                    spread.Operand,
                    cancellationToken,
                    visitedLocals),
            IConditionalOperation conditional =>
                FlowsFromCancellationTokenConditional(
                    conditional,
                    cancellationToken,
                    visitedLocals),
            ICoalesceOperation coalesce =>
                FlowsFromCancellationTokenCoalesce(
                    coalesce,
                    cancellationToken,
                    visitedLocals),
            _ => false,
        };
    }

    private static bool FlowsFromParallelCallbackToken(
        IParameterReferenceOperation parameterReference,
        IParameterSymbol cancellationToken,
        HashSet<ILocalSymbol> visitedLocals)
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
                cancellationToken,
                CloneVisitedLocals(visitedLocals)));
    }

    private static bool FlowsFromCancellationTokenProperty(
        IPropertyReferenceOperation propertyReference,
        IParameterSymbol cancellationToken,
        HashSet<ILocalSymbol> visitedLocals)
    {
        return IsCancellationTokenSourceToken(propertyReference)
               && propertyReference.Instance is not null
               && FlowsFromCancellationToken(
                   propertyReference.Instance,
                   cancellationToken,
                   visitedLocals);
    }

    private static bool FlowsFromCancellationTokenInvocation(
        IInvocationOperation invocation,
        IParameterSymbol cancellationToken,
        HashSet<ILocalSymbol> visitedLocals)
    {
        return IsCancellationCarrier(invocation.Type)
               && invocation.Arguments.Any(argument =>
                   argument.Parameter is not null
                   && IsCancellationInput(argument.Parameter)
                   && FlowsFromCancellationToken(
                       argument.Value,
                       cancellationToken,
                       visitedLocals));
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
        IParameterSymbol cancellationToken,
        HashSet<ILocalSymbol> visitedLocals)
    {
        return FlowsFromCancellationToken(
                   conditional.WhenTrue,
                   cancellationToken,
                   CloneVisitedLocals(visitedLocals))
               && conditional.WhenFalse is not null
               && FlowsFromCancellationToken(
                   conditional.WhenFalse,
                   cancellationToken,
                   CloneVisitedLocals(visitedLocals));
    }

    private static bool FlowsFromCancellationTokenCoalesce(
        ICoalesceOperation coalesce,
        IParameterSymbol cancellationToken,
        HashSet<ILocalSymbol> visitedLocals)
    {
        return FlowsFromCancellationToken(
                   coalesce.Value,
                   cancellationToken,
                   CloneVisitedLocals(visitedLocals))
               && FlowsFromCancellationToken(
                   coalesce.WhenNull,
                   cancellationToken,
                   CloneVisitedLocals(visitedLocals));
    }

    private static HashSet<ILocalSymbol> CloneVisitedLocals(
        HashSet<ILocalSymbol> visitedLocals) =>
        new(visitedLocals, visitedLocals.Comparer);

    private static bool FlowsFromCancellationTokenLocal(
        ILocalReferenceOperation localReference,
        IParameterSymbol cancellationToken,
        HashSet<ILocalSymbol> visitedLocals)
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
                       cancellationToken,
                       CloneVisitedLocals(visitedLocals)));
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
        var lowerBound = linearAssignment?.Syntax.SpanStart ?? int.MinValue;

        foreach (var assignment in assignments.Where(candidate =>
                     candidate.Syntax.SpanStart >= lowerBound))
        {
            yield return assignment.Value;
        }

        if (linearAssignment is not null)
        {
            yield break;
        }

        var declarator = FindLocalDeclarator(root, operation, local, callable);
        if (declarator?.Initializer?.Value is { } initialValue)
        {
            yield return initialValue;
        }
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
        IBlockOperation containingBlock)
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
