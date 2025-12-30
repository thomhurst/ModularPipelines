using System.Collections.Concurrent;
using System.Linq.Expressions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Factory for creating ModuleExecutionContext instances without reflection.
/// </summary>
/// <remarks>
/// Replaces Activator.CreateInstance with compiled expression trees for better performance.
/// </remarks>
internal static class ExecutionContextFactory
{
    /// <summary>
    /// Delegate signature for creating a ModuleExecutionContext.
    /// </summary>
    internal delegate ModuleExecutionContext CreateContextDelegate(IModule module, Type moduleType);

    private static readonly ConcurrentDictionary<Type, CreateContextDelegate> ContextFactoryCache = new();

    /// <summary>
    /// Creates a ModuleExecutionContext for the specified module.
    /// </summary>
    /// <param name="module">The module instance.</param>
    /// <param name="moduleType">The type of the module.</param>
    /// <returns>A typed ModuleExecutionContext.</returns>
    public static ModuleExecutionContext Create(IModule module, Type moduleType)
    {
        var resultType = module.ResultType;
        var factory = ContextFactoryCache.GetOrAdd(resultType, CreateFactory);
        return factory(module, moduleType);
    }

    private static CreateContextDelegate CreateFactory(Type resultType)
    {
        // Parameters
        var moduleParam = Expression.Parameter(typeof(IModule), "module");
        var moduleTypeParam = Expression.Parameter(typeof(Type), "moduleType");

        // Get the generic types
        var contextType = typeof(ModuleExecutionContext<>).MakeGenericType(resultType);
        var typedModuleType = typeof(Module<>).MakeGenericType(resultType);

        // Find the constructor: ModuleExecutionContext<T>(Module<T> module, Type moduleType)
        var constructor = contextType.GetConstructor(new[] { typedModuleType, typeof(Type) })
            ?? throw new InvalidOperationException(
                $"Could not find constructor for {contextType.Name} with (Module<{resultType.Name}>, Type) parameters.");

        // Cast module to Module<T>
        var castModule = Expression.Convert(moduleParam, typedModuleType);

        // Create new ModuleExecutionContext<T>((Module<T>)module, moduleType)
        var newContext = Expression.New(constructor, castModule, moduleTypeParam);

        // Cast to base type
        var castToBase = Expression.Convert(newContext, typeof(ModuleExecutionContext));

        // Create and compile the lambda
        var lambda = Expression.Lambda<CreateContextDelegate>(
            castToBase,
            moduleParam,
            moduleTypeParam);

        return lambda.Compile();
    }
}
