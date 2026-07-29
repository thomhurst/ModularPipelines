using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Threading;
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
        var assemblyRegistrationUsed = 0;

        context.RegisterSymbolAction(
            symbolContext => AnalyzeRegisteredModuleType(symbolContext, modules),
            SymbolKind.NamedType);
        context.RegisterOperationAction(
            operationContext => TrackRegistration(
                operationContext,
                registeredModules,
                ref assemblyRegistrationUsed),
            OperationKind.Invocation);
        context.RegisterCompilationEndAction(endContext =>
            ReportUnregisteredModules(
                endContext,
                modules,
                registeredModules,
                Volatile.Read(ref assemblyRegistrationUsed) != 0));
    }

    private static void AnalyzeRegisteredModuleType(
        SymbolAnalysisContext context,
        ConcurrentBag<INamedTypeSymbol> modules)
    {
        var type = (INamedTypeSymbol) context.Symbol;
        if (type.IsAbstract || !type.IsModule(context.Compilation))
        {
            return;
        }

        modules.Add(type);
        var location = type.Locations.FirstOrDefault(static item => item.IsInSource);
        if (location is not null && !IsPublic(type))
        {
            context.ReportDiagnostic(Diagnostic.Create(
                ModuleRegistrationAnalyzer.NonPublicModuleRule,
                location,
                type.Name));
        }
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
        ref int assemblyRegistrationUsed)
    {
        var invocation = (IInvocationOperation) context.Operation;
        TrackRegistrationInvocation(invocation, registeredModules, ref assemblyRegistrationUsed);
    }

    private static void AnalyzeInvocation(OperationAnalysisContext context)
    {
        var invocation = (IInvocationOperation) context.Operation;

        if (!IsInsideModuleExecuteAsync(context))
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

        if ((method.Name == "Wait" && method.ContainingType.InheritsFrom(
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
        if (!IsInsideModuleExecuteAsync(context)
            || propertyReference.Property.Name != "Result"
            || !propertyReference.Property.ContainingType.InheritsFrom(
                context.Compilation.GetTypeByMetadataName("System.Threading.Tasks.Task")))
        {
            return;
        }

        context.ReportDiagnostic(Diagnostic.Create(
            ModuleAsyncSafetyAnalyzer.BlockingCallRule,
            propertyReference.Syntax.GetLocation(),
            propertyReference.Property.Name));
    }

    private static void AnalyzeAwait(OperationAnalysisContext context)
    {
        if (!IsInsideModuleExecuteAsync(context)
            || context.Operation is not IAwaitOperation awaitOperation
            || GetAwaitedInvocation(awaitOperation) is not { } invocation
            || context.ContainingSymbol is not IMethodSymbol executeMethod)
        {
            return;
        }

        var cancellationToken = executeMethod.Parameters.FirstOrDefault(IsCancellationToken);
        if (cancellationToken is null
            || InvocationUsesCancellation(invocation, cancellationToken)
            || !InvocationAcceptsCancellationToken(invocation.TargetMethod))
        {
            return;
        }

        context.ReportDiagnostic(Diagnostic.Create(
            ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenRule,
            invocation.Syntax.GetLocation(),
            invocation.TargetMethod.Name));
    }

    private static IInvocationOperation? GetAwaitedInvocation(IAwaitOperation awaitOperation)
    {
        if (awaitOperation.Operation is not IInvocationOperation invocation)
        {
            return null;
        }

        while (invocation.TargetMethod.Name == "ConfigureAwait"
               && invocation.Instance is IInvocationOperation configuredInvocation)
        {
            invocation = configuredInvocation;
        }

        return invocation;
    }

    private static void TrackRegistrationInvocation(
        IInvocationOperation invocation,
        ConcurrentBag<INamedTypeSymbol> registeredModules,
        ref int assemblyRegistrationUsed)
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
            Interlocked.Exchange(ref assemblyRegistrationUsed, 1);
            return;
        }

        foreach (var typeArgument in method.TypeArguments.OfType<INamedTypeSymbol>())
        {
            registeredModules.Add(typeArgument);
        }

        foreach (var typeOfOperation in invocation.Arguments
                     .SelectMany(static argument => argument.Value.DescendantsAndSelf())
                     .OfType<ITypeOfOperation>())
        {
            if (typeOfOperation.TypeOperand is INamedTypeSymbol moduleType)
            {
                registeredModules.Add(moduleType);
            }
        }
    }

    private static void ReportUnregisteredModules(
        CompilationAnalysisContext context,
        ConcurrentBag<INamedTypeSymbol> modules,
        ConcurrentBag<INamedTypeSymbol> registeredModules,
        bool assemblyRegistrationUsed)
    {
        if (assemblyRegistrationUsed)
        {
            return;
        }

        var registered = registeredModules.ToImmutableHashSet<INamedTypeSymbol>(
            SymbolEqualityComparer.Default);
        foreach (var module in modules.Distinct<INamedTypeSymbol>(SymbolEqualityComparer.Default))
        {
            if (registered.Contains(module))
            {
                continue;
            }

            var location = module.Locations.FirstOrDefault(static item => item.IsInSource);
            if (location is not null)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    ModuleRegistrationAnalyzer.UnregisteredModuleRule,
                    location,
                    module.Name));
            }
        }
    }

    private static void AnalyzeDuplicateDependencies(SymbolAnalysisContext context)
    {
        var module = (INamedTypeSymbol) context.Symbol;
        if (module.IsAbstract || !module.IsModule(context.Compilation))
        {
            return;
        }

        var dependencies = module.GetAttributes()
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

    private static bool IsInsideModuleExecuteAsync(OperationAnalysisContext context)
    {
        return context.ContainingSymbol is IMethodSymbol method
            && method.Name == AnalyzerConstants.MethodNames.ExecuteAsync
            && method.ContainingType.IsModule(context.Compilation);
    }

    private static bool IsAwaiterGetResult(IInvocationOperation invocation)
    {
        return invocation.Instance is IInvocationOperation
        {
            TargetMethod.Name: "GetAwaiter",
        };
    }

    private static bool InvocationUsesCancellation(
        IInvocationOperation invocation,
        IParameterSymbol cancellationToken)
    {
        foreach (var value in invocation.Arguments
            .Where(static argument => argument.Parameter is not null)
            .Where(argument => IsCancellationToken(argument.Parameter!))
            .Select(static argument => argument.Value))
        {
            if (value.DescendantsAndSelf()
                .OfType<IParameterReferenceOperation>()
                .Any(reference => SymbolEqualityComparer.Default.Equals(
                    reference.Parameter,
                    cancellationToken)))
            {
                return true;
            }

            if (value is not IDefaultValueOperation
                && value is not IPropertyReferenceOperation
                {
                    Property.Name: "None",
                    Property.ContainingType.Name: "CancellationToken",
                })
            {
                return true;
            }
        }

        return false;
    }

    private static bool InvocationAcceptsCancellationToken(IMethodSymbol method)
    {
        if (method.Parameters.Any(IsCancellationToken))
        {
            return true;
        }

        return method.ContainingType.GetMembers(method.Name)
            .OfType<IMethodSymbol>()
            .Any(candidate => IsCancellationOverload(method, candidate));
    }

    private static bool IsCancellationOverload(
        IMethodSymbol selectedMethod,
        IMethodSymbol candidate)
    {
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
