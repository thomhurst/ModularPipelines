using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal static class ModuleResultRegistryExtensions
{
    public static IReadOnlyList<IModuleResult> GetCompletedResults(
        this IModuleResultRegistry resultRegistry,
        IEnumerable<IModule> modules)
    {
        ArgumentNullException.ThrowIfNull(resultRegistry);
        ArgumentNullException.ThrowIfNull(modules);

        return modules
            .Select(module => resultRegistry.GetResult(module.GetType()))
            .OfType<IModuleResult>()
            .ToArray();
    }

    /// <summary>
    /// Completes a module's awaitable, then registers the result it holds.
    /// </summary>
    /// <remarks>
    /// An execution backend may complete the awaitable first, for example when a distributed
    /// coordinator stops collecting a module it is still running locally during cancellation.
    /// The awaitable's first result wins, and registering only that result keeps the registry,
    /// including its completion signal, consistent with it whichever writer runs first.
    /// </remarks>
    /// <returns>The result the module's awaitable holds.</returns>
    public static IModuleResult CompleteAndRegister(
        this IModuleResultRegistry resultRegistry,
        IModule module,
        Type moduleType,
        IModuleResult result)
    {
        ArgumentNullException.ThrowIfNull(resultRegistry);
        ArgumentNullException.ThrowIfNull(module);
        ArgumentNullException.ThrowIfNull(result);

        var publishedResult = module.AsInternal().CompleteResult(result);
        resultRegistry.RegisterResult(moduleType, publishedResult);
        return publishedResult;
    }
}
