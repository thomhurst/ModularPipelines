using System.Collections.Concurrent;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using ModularPipelines.Analyzers.Extensions;

namespace ModularPipelines.Analyzers;

internal static class ModuleAuthoringAnalysis
{
    private const string CancellationTokenMetadataName = "System.Threading.CancellationToken";

    public static void InitializeRegistrationAnalysis(AnalysisContext context)
    {
        context.RegisterCompilationStartAction(StartRegistrationAnalysis);
    }

    public static void InitializeAsyncSafetyAnalysis(AnalysisContext context)
    {
        context.RegisterSymbolAction(AnalyzeMethod, SymbolKind.Method);
        context.RegisterOperationAction(AnalyzeInvocation, OperationKind.Invocation);
        context.RegisterOperationAction(AnalyzePropertyReference, OperationKind.PropertyReference);
        context.RegisterOperationAction(AnalyzeAwait, OperationKind.Await);
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

        context.RegisterSymbolAction(
            symbolContext => CollectModuleType(symbolContext, modules),
            SymbolKind.NamedType);
        context.RegisterOperationAction(
            operationContext => TrackRegistration(
                operationContext,
                registeredModules,
                instanceRegisteredModules,
                scannedAssemblies,
                unresolvedModuleRegistrations),
            OperationKind.Invocation);
        context.RegisterCompilationEndAction(endContext =>
            ReportModuleDiagnostics(
                endContext,
                modules,
                registeredModules,
                instanceRegisteredModules,
                scannedAssemblies,
                unresolvedModuleRegistrations));
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

    private static void AnalyzeMethod(SymbolAnalysisContext context)
    {
        var method = (IMethodSymbol) context.Symbol;
        if (!method.IsAsync
            || !method.ReturnsVoid
            || !method.ContainingType.IsModule(context.Compilation))
        {
            return;
        }

        var location = method.Locations.FirstOrDefault(static item => item.IsInSource);
        if (location is not null)
        {
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
        ConcurrentBag<IAssemblySymbol> scannedAssemblies,
        ConcurrentBag<byte> unresolvedModuleRegistrations)
    {
        var invocation = (IInvocationOperation) context.Operation;
        TrackRegistrationInvocation(
            invocation,
            context.Compilation.Assembly,
            registeredModules,
            instanceRegisteredModules,
            scannedAssemblies,
            unresolvedModuleRegistrations);
    }

    private static void AnalyzeInvocation(OperationAnalysisContext context)
    {
        var invocation = (IInvocationOperation) context.Operation;

        if (GetModuleExecuteAsync(context) is null)
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
        if (GetModuleExecuteAsync(context) is null
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
        if (GetModuleExecuteAsync(context) is not { } executeMethod
            || context.Operation is not IAwaitOperation awaitOperation)
        {
            return;
        }

        var cancellationToken = executeMethod.Parameters.FirstOrDefault(IsCancellationToken);
        if (cancellationToken is null)
        {
            return;
        }

        foreach (var invocation in GetAwaitedInvocations(awaitOperation))
        {
            if (InvocationUsesCancellation(invocation, cancellationToken)
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
        IAwaitOperation awaitOperation)
    {
        return GetAwaitedInvocations(
            awaitOperation.Operation,
            [with(SymbolEqualityComparer.Default)]);
    }

    private static IEnumerable<IInvocationOperation> GetAwaitedInvocations(
        IOperation operation,
        HashSet<ILocalSymbol> visitedLocals)
    {
        var pending = new Stack<(IOperation Operation, bool RequireTaskLike)>();
        pending.Push((operation, false));

        while (pending.Count > 0)
        {
            var (current, requireTaskLike) = pending.Pop();
            if (ProcessAwaitedOperation(
                    current,
                    requireTaskLike,
                    visitedLocals,
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
        Stack<(IOperation Operation, bool RequireTaskLike)> pending)
    {
        if (operation is IAnonymousFunctionOperation)
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
            return ProcessAwaitedInvocation(invocation, requireTaskLike, pending);
        }

        QueueChildOperations(operation, requireTaskLike, pending);
        return null;
    }

    private static void QueueLocalValue(
        ILocalReferenceOperation localReference,
        bool requireTaskLike,
        HashSet<ILocalSymbol> visitedLocals,
        Stack<(IOperation Operation, bool RequireTaskLike)> pending)
    {
        if (visitedLocals.Add(localReference.Local)
            && FindReachingLocalValue(localReference, localReference.Local) is { } localValue)
        {
            pending.Push((localValue, requireTaskLike));
        }
    }

    private static IInvocationOperation? ProcessAwaitedInvocation(
        IInvocationOperation invocation,
        bool requireTaskLike,
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

        foreach (var argument in invocation.Arguments.Reverse())
        {
            pending.Push((argument.Value, true));
        }

        return !requireTaskLike || IsTaskLike(invocation.Type)
            ? invocation
            : null;
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
        for (var current = type as INamedTypeSymbol;
             current is not null;
             current = current.BaseType)
        {
            if (current.OriginalDefinition.ToDisplayString() is
                "System.Threading.Tasks.Task"
                or "System.Threading.Tasks.Task<TResult>"
                or "System.Threading.Tasks.ValueTask"
                or "System.Threading.Tasks.ValueTask<TResult>")
            {
                return true;
            }
        }

        return false;
    }

    private static void TrackRegistrationInvocation(
        IInvocationOperation invocation,
        IAssemblySymbol currentAssembly,
        ConcurrentBag<INamedTypeSymbol> registeredModules,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        ConcurrentBag<IAssemblySymbol> scannedAssemblies,
        ConcurrentBag<byte> unresolvedModuleRegistrations)
    {
        var method = invocation.TargetMethod;
        if (!method.ContainingNamespace.ToDisplayString().StartsWith(
                "ModularPipelines",
                StringComparison.Ordinal)
            || method.Name is not (
                "AddModule"
                or "AddModules"
                or "AddModulesFromAssembly"
                or "AddModulesFromAssemblyContainingType"))
        {
            return;
        }

        if (method.Name.StartsWith("AddModulesFromAssembly", StringComparison.Ordinal))
        {
            TrackScannedAssemblies(invocation, currentAssembly, scannedAssemblies);
            return;
        }

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

        if (method.Name != "AddModules")
        {
            return;
        }

        foreach (var argument in invocation.Arguments)
        {
            if (!TryTrackModuleTypes(
                    argument.Value,
                    registeredModules,
                    [with(SymbolEqualityComparer.Default)]))
            {
                unresolvedModuleRegistrations.Add(0);
            }
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
                    TryTrackModuleTypes(element, registeredModules, visitedLocals));
            case ICollectionExpressionOperation collection:
                return collection.Elements.All(element =>
                    TryTrackModuleTypes(element, registeredModules, visitedLocals));
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
        ConcurrentBag<IAssemblySymbol> scannedAssemblies)
    {
        foreach (var typeArgument in invocation.TargetMethod.TypeArguments.OfType<INamedTypeSymbol>())
        {
            scannedAssemblies.Add(typeArgument.ContainingAssembly);
        }

        foreach (var value in invocation.Arguments.SelectMany(argument =>
                     GetValueAndReachingLocalValues(
                         argument.Value,
                         [with(SymbolEqualityComparer.Default)])))
        {
            foreach (var typeOfOperation in value.DescendantsAndSelf().OfType<ITypeOfOperation>())
            {
                scannedAssemblies.Add(typeOfOperation.TypeOperand.ContainingAssembly);
            }

            if (value.DescendantsAndSelf()
                .OfType<IInvocationOperation>()
                .Any(static operation =>
                    operation.TargetMethod.Name is "GetExecutingAssembly" or "GetEntryAssembly"
                    && operation.TargetMethod.ContainingType.ToDisplayString()
                    == "System.Reflection.Assembly"))
            {
                scannedAssemblies.Add(currentAssembly);
            }
        }
    }

    private static void ReportModuleDiagnostics(
        CompilationAnalysisContext context,
        ConcurrentBag<INamedTypeSymbol> modules,
        ConcurrentBag<INamedTypeSymbol> registeredModules,
        ConcurrentBag<INamedTypeSymbol> instanceRegisteredModules,
        ConcurrentBag<IAssemblySymbol> scannedAssemblies,
        ConcurrentBag<byte> unresolvedModuleRegistrations)
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
                     && !scanned.Contains(module.ContainingAssembly)))
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

        if (!unresolvedModuleRegistrations.IsEmpty)
        {
            return;
        }

        var registered = new HashSet<INamedTypeSymbol>(
            registeredModules,
            SymbolEqualityComparer.Default);
        foreach (var module in moduleSet.Where(module => scanned.Contains(module.ContainingAssembly)))
        {
            registered.Add(module);
        }

        AddRequiredDependencyClosure(registered, context.Compilation);
        foreach (var module in moduleSet)
        {
            if (registered.Contains(module))
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

    private static void AnalyzeDuplicateDependencies(SymbolAnalysisContext context)
    {
        var module = (INamedTypeSymbol) context.Symbol;
        if (module.IsAbstract || !module.IsModule(context.Compilation))
        {
            return;
        }

        var dependencies = GetInheritedAttributes(module)
            .Concat(module.GetAttributes())
            .Select(attribute => new
            {
                Attribute = attribute,
                Type = GetDependencyType(attribute, context.Compilation),
            })
            .Where(static item => item.Type is not null)
            .GroupBy(static item => item.Type!, SymbolEqualityComparer.Default);

        foreach (var duplicates in dependencies.Where(static group => group.Skip(1).Any()))
        {
            foreach (var duplicate in duplicates.Skip(1))
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

    private static IMethodSymbol? GetModuleExecuteAsync(OperationAnalysisContext context)
    {
        var nestedCallables = GetEnclosingNestedCallables(context.Operation);

        for (var method = context.ContainingSymbol as IMethodSymbol;
             method is not null;
             method = method.ContainingSymbol as IMethodSymbol)
        {
            if (method.Name == AnalyzerConstants.MethodNames.ExecuteAsync
                && method.IsOverride
                && method.OverriddenMethod?.ContainingType.IsModule(context.Compilation) == true)
            {
                return NestedCallablesAreInvoked(context.Operation, nestedCallables)
                    ? method
                    : null;
            }

            if (method.MethodKind is not (MethodKind.LocalFunction or MethodKind.AnonymousFunction))
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
        var invokedMethods = invocations
            .Select(static invocation => invocation.TargetMethod.OriginalDefinition)
            .ToImmutableHashSet<IMethodSymbol>(SymbolEqualityComparer.Default);

        return nestedCallables.All(callable =>
            callable.MethodKind == MethodKind.AnonymousFunction
                ? invocations.Any(invocation => InvocationTargetsAnonymousFunction(
                    invocation,
                    callable))
                : invokedMethods.Contains(callable.OriginalDefinition));
    }

    private static bool InvocationTargetsAnonymousFunction(
        IInvocationOperation invocation,
        IMethodSymbol anonymousFunction)
    {
        if (invocation.Instance is not null
            && ValueContainsAnonymousFunction(
                invocation.Instance,
                anonymousFunction,
                [with(SymbolEqualityComparer.Default)]))
        {
            return true;
        }

        return IsKnownDelegateInvoker(invocation.TargetMethod)
               && invocation.Arguments.Any(argument =>
                   ValueContainsAnonymousFunction(
                       argument.Value,
                       anonymousFunction,
                       [with(SymbolEqualityComparer.Default)]));
    }

    private static bool IsKnownDelegateInvoker(IMethodSymbol method)
    {
        var containingType = method.ContainingType.OriginalDefinition.ToDisplayString();
        return (method.Name == "Run"
                && containingType == "System.Threading.Tasks.Task")
               || (method.Name == "StartNew"
                   && containingType == "System.Threading.Tasks.TaskFactory");
    }

    private static bool ValueContainsAnonymousFunction(
        IOperation value,
        IMethodSymbol anonymousFunction,
        HashSet<ILocalSymbol> visitedLocals)
    {
        if (value.DescendantsAndSelf()
            .OfType<IAnonymousFunctionOperation>()
            .Any(candidate => SymbolEqualityComparer.Default.Equals(
                candidate.Symbol,
                anonymousFunction)))
        {
            return true;
        }

        foreach (var localReference in value.DescendantsAndSelf().OfType<ILocalReferenceOperation>())
        {
            if (visitedLocals.Add(localReference.Local)
                && FindReachingLocalValue(localReference, localReference.Local) is { } localValue
                && ValueContainsAnonymousFunction(localValue, anonymousFunction, visitedLocals))
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
        if (value.DescendantsAndSelf()
            .OfType<IParameterReferenceOperation>()
            .Any(reference => SymbolEqualityComparer.Default.Equals(
                reference.Parameter,
                cancellationToken)))
        {
            return true;
        }

        foreach (var localReference in value.DescendantsAndSelf().OfType<ILocalReferenceOperation>())
        {
            if (!visitedLocals.Add(localReference.Local))
            {
                continue;
            }

            var localValue = FindReachingLocalValue(value, localReference.Local);
            if (localValue is not null
                && FlowsFromCancellationToken(localValue, cancellationToken, visitedLocals))
            {
                return true;
            }
        }

        return false;
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
        var root = GetRoot(operation);
        var callable = GetEnclosingCallable(operation);
        var assignment = root.DescendantsAndSelf()
            .OfType<ISimpleAssignmentOperation>()
            .Where(candidate => candidate.Syntax.SpanStart < operation.Syntax.SpanStart)
            .Where(candidate => candidate.Target is ILocalReferenceOperation localReference
                && SymbolEqualityComparer.Default.Equals(localReference.Local, local))
            .Where(candidate => ReferenceEquals(GetEnclosingCallable(candidate), callable))
            .OrderByDescending(static candidate => candidate.Syntax.SpanStart)
            .FirstOrDefault();
        if (assignment is not null)
        {
            return assignment.Value;
        }

        return root.DescendantsAndSelf()
            .OfType<IVariableDeclaratorOperation>()
            .Where(declarator => declarator.Syntax.SpanStart < operation.Syntax.SpanStart)
            .Where(declarator => SymbolEqualityComparer.Default.Equals(declarator.Symbol, local))
            .Where(declarator => ReferenceEquals(GetEnclosingCallable(declarator), callable))
            .OrderByDescending(static declarator => declarator.Syntax.SpanStart)
            .FirstOrDefault()
            ?.Initializer?.Value;
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
            .Any(candidate => IsCancellationOverload(selectedMethod, candidate));
    }

    private static bool IsCancellationOverload(
        IMethodSymbol selectedMethod,
        IMethodSymbol candidate)
    {
        if (candidate.IsGenericMethod
            && candidate.Arity == selectedMethod.TypeArguments.Length)
        {
            candidate = candidate.Construct([.. selectedMethod.TypeArguments]);
        }

        var candidateParameters = candidate.Parameters
            .Where(parameter => !IsCancellationToken(parameter))
            .ToArray();
        if (candidateParameters.Length != selectedMethod.Parameters.Length
            || candidate.Parameters.Length == candidateParameters.Length)
        {
            return false;
        }

        return candidateParameters
            .Zip(
                selectedMethod.Parameters,
                static (left, right) => left.RefKind == right.RefKind
                    && SymbolEqualityComparer.Default.Equals(left.Type, right.Type))
            .All(static matches => matches);
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
