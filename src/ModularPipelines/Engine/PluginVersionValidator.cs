using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Exceptions;

namespace ModularPipelines.Engine;

/// <summary>
/// Validates plugin assembly compatibility with the current ModularPipelines version.
/// </summary>
internal static class PluginVersionValidator
{
    /// <summary>
    /// Validates that a plugin assembly is compatible with the current ModularPipelines version.
    /// </summary>
    /// <param name="assembly">The plugin assembly to validate.</param>
    /// <param name="coreVersion">The current ModularPipelines version.</param>
    /// <exception cref="PluginVersionMismatchException">
    /// Thrown when the plugin declares an incompatible major version.
    /// </exception>
    public static void Validate(Assembly assembly, Version? coreVersion)
    {
        var pluginAttr = assembly.GetCustomAttribute<ModularPipelinesPluginAttribute>();

        if (pluginAttr is null)
        {
            return; // No attribute = no version requirement (backward compatible)
        }

        if (coreVersion is null || pluginAttr.CompatibleMajorVersion != coreVersion.Major)
        {
            throw new PluginVersionMismatchException(
                assembly.GetName().Name ?? assembly.FullName ?? "Unknown",
                pluginAttr.CompatibleMajorVersion,
                coreVersion);
        }
    }

    /// <summary>
    /// Checks if a plugin assembly is compatible without throwing.
    /// </summary>
    /// <param name="assembly">The plugin assembly to check.</param>
    /// <param name="coreVersion">The current ModularPipelines version.</param>
    /// <returns>True if compatible or no version requirement; false if incompatible.</returns>
    public static bool IsCompatible(Assembly assembly, Version? coreVersion)
    {
        var pluginAttr = assembly.GetCustomAttribute<ModularPipelinesPluginAttribute>();

        if (pluginAttr is null)
        {
            return true; // No attribute = compatible
        }

        return coreVersion is not null && pluginAttr.CompatibleMajorVersion == coreVersion.Major;
    }
}
