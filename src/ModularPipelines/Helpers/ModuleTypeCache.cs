using System.Collections.Concurrent;
using ModularPipelines.Modules;

namespace ModularPipelines.Helpers;

/// <summary>
/// Cache for module result types to avoid repeated reflection overhead.
/// </summary>
/// <remarks>
/// When accessing <see cref="IModule.ResultType"/>, the underlying implementation
/// returns <c>typeof(T)</c> which involves runtime type lookup. This cache stores
/// the result type once per module type for efficient repeated access.
/// </remarks>
internal static class ModuleTypeCache
{
    private static readonly ConcurrentDictionary<Type, Type> ResultTypes = new();

    /// <summary>
    /// Gets the cached result type for the specified module type.
    /// </summary>
    /// <param name="moduleType">The concrete module type (e.g., <c>MyModule</c>).</param>
    /// <returns>The result type <c>T</c> from <c>Module&lt;T&gt;</c>.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the module type does not inherit from <c>Module&lt;T&gt;</c>.
    /// </exception>
    public static Type GetResultType(Type moduleType)
        => ResultTypes.GetOrAdd(moduleType, GetResultTypeFromModule);

    private static Type GetResultTypeFromModule(Type moduleType)
    {
        // Walk up the inheritance hierarchy to find Module<T>
        var current = moduleType;
        while (current != null)
        {
            if (current.IsGenericType && current.GetGenericTypeDefinition() == typeof(Module<>))
            {
                return current.GetGenericArguments()[0];
            }

            current = current.BaseType;
        }

        throw new InvalidOperationException(
            $"Type {moduleType.FullName} does not inherit from Module<T>. " +
            "Ensure your module class inherits from Module<T> where T is your result type.");
    }
}
