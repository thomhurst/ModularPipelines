using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

/// <summary>
/// Cancels typed module completion sources when modules terminate before execution.
/// </summary>
internal static class ModuleCompletionSourceCanceller
{
    private static readonly ConcurrentDictionary<Type, Action<IModule>> Cache = new();

    /// <summary>
    /// Cancels the module's typed completion source.
    /// </summary>
    /// <param name="module">The module instance to cancel.</param>
    /// <param name="moduleType">The concrete module type.</param>
    public static void Cancel(IModule module, Type moduleType)
    {
        if (GeneratedModuleMetadata.TryGetRuntime(moduleType, out var runtime))
        {
            runtime.CancelCompletionSource(module);
            return;
        }

        var resultType = GetResultType(moduleType);
        if (resultType is not null)
        {
            Cache.GetOrAdd(resultType, CreateCanceller)(module);
        }
    }

    private static Type? GetResultType(Type moduleType)
    {
        for (var currentType = moduleType; currentType is not null; currentType = currentType.BaseType)
        {
            if (currentType.IsGenericType
                && currentType.GetGenericTypeDefinition() == typeof(Module<>))
            {
                return currentType.GetGenericArguments()[0];
            }
        }

        return null;
    }

    [UnconditionalSuppressMessage(
        "AOT",
        "IL3050",
        Justification = "The dynamic completion-source canceller is a fallback for modules without generated runtime metadata.")]
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2075",
        Justification = "The dynamic completion-source canceller is a fallback for modules without generated runtime metadata.")]
    private static Action<IModule> CreateCanceller(Type resultType)
    {
        var moduleType = typeof(Module<>).MakeGenericType(resultType);
        var moduleParameter = Expression.Parameter(typeof(IModule), "module");
        var typedModule = Expression.Convert(moduleParameter, moduleType);
        var completionSourceProperty = moduleType.GetProperty(
            "CompletionSource",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
            ?? throw new InvalidOperationException($"CompletionSource property not found on {moduleType.Name}");
        var completionSource = Expression.Property(typedModule, completionSourceProperty);
        var trySetCanceledMethod = completionSourceProperty.PropertyType.GetMethod(
            "TrySetCanceled",
            Type.EmptyTypes)
            ?? throw new InvalidOperationException(
                $"TrySetCanceled method not found on {completionSourceProperty.PropertyType.Name}");
        var cancel = Expression.Call(completionSource, trySetCanceledMethod);

        return Expression.Lambda<Action<IModule>>(cancel, moduleParameter).Compile();
    }
}
