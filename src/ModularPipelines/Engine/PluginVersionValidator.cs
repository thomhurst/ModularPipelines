using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Exceptions;
using ModularPipelines.Generated;

namespace ModularPipelines.Engine;

/// <summary>
/// Validates plugin assembly compatibility with the current ModularPipelines version.
/// </summary>
internal static class PluginVersionValidator
{
    private const string RuntimeMetadataRegistrationTypeName =
        "ModularPipelines.Generated.RuntimeMetadataRegistration";

    /// <summary>
    /// Validates that a plugin assembly is compatible with the current ModularPipelines version.
    /// </summary>
    /// <param name="assembly">The plugin assembly to validate.</param>
    /// <param name="coreVersion">The current ModularPipelines version.</param>
    /// <exception cref="PluginVersionMismatchException">
    /// Thrown when the plugin declares an incompatible major version.
    /// </exception>
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Dynamic plugin loading is disabled when dynamic code is unavailable.")]
    public static void Validate(Assembly assembly, Version? coreVersion)
    {
        var pluginName = assembly.GetName().Name ?? assembly.FullName ?? "Unknown";
        var pluginAttr = assembly.GetCustomAttribute<ModularPipelinesPluginAttribute>();

        if (pluginAttr is not null
            && (coreVersion is null || pluginAttr.CompatibleMajorVersion != coreVersion.Major))
        {
            throw new PluginVersionMismatchException(
                pluginName,
                pluginAttr.CompatibleMajorVersion,
                coreVersion);
        }

        var metadataRegistration = assembly.GetType(
            RuntimeMetadataRegistrationTypeName,
            throwOnError: false,
            ignoreCase: false);
        if (metadataRegistration is null)
        {
            // Referenced assemblies are traversed alongside plugins. Without either marker,
            // there is no deterministic evidence that the assembly is a plugin.
            return;
        }

        var runtimeSchemaVersion = GetSchemaVersion(metadataRegistration, "SchemaVersion");
        var commandSchemaVersion = GetSchemaVersion(metadataRegistration, "CommandSchemaVersion");
        if (runtimeSchemaVersion != GeneratedSecretMetadata.CurrentSchemaVersion
            || commandSchemaVersion != GeneratedCommandMetadata.CurrentSchemaVersion)
        {
            throw new PluginInitializationException(
                $"Assembly '{pluginName}' uses runtime metadata schemas "
                + $"{runtimeSchemaVersion?.ToString() ?? "missing"}/"
                + $"{commandSchemaVersion?.ToString() ?? "missing"}, but this "
                + "ModularPipelines runtime requires "
                + $"{GeneratedSecretMetadata.CurrentSchemaVersion}/"
                + $"{GeneratedCommandMetadata.CurrentSchemaVersion}. "
                + "Rebuild the assembly against ModularPipelines v4.",
                pluginName);
        }
    }

    /// <summary>
    /// Checks if a plugin assembly is compatible without throwing.
    /// </summary>
    /// <param name="assembly">The plugin assembly to check.</param>
    /// <param name="coreVersion">The current ModularPipelines version.</param>
    /// <returns>True if compatible or no version requirement; false if incompatible.</returns>
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Dynamic plugin loading is disabled when dynamic code is unavailable.")]
    public static bool IsCompatible(Assembly assembly, Version? coreVersion)
    {
        var pluginAttr = assembly.GetCustomAttribute<ModularPipelinesPluginAttribute>();

        if (pluginAttr is not null
            && (coreVersion is null || pluginAttr.CompatibleMajorVersion != coreVersion.Major))
        {
            return false;
        }

        var metadataRegistration = assembly.GetType(
            RuntimeMetadataRegistrationTypeName,
            throwOnError: false,
            ignoreCase: false);
        if (metadataRegistration is null)
        {
            return true;
        }

        return GetSchemaVersion(metadataRegistration, "SchemaVersion")
               == GeneratedSecretMetadata.CurrentSchemaVersion
               && GetSchemaVersion(metadataRegistration, "CommandSchemaVersion")
               == GeneratedCommandMetadata.CurrentSchemaVersion;
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2070",
        Justification = "Dynamic plugin loading is disabled when dynamic code is unavailable.")]
    private static int? GetSchemaVersion(Type metadataRegistration, string fieldName)
    {
        var field = metadataRegistration.GetField(
            fieldName,
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        return field is { IsLiteral: true }
               && field.GetRawConstantValue() is int schemaVersion
            ? schemaVersion
            : null;
    }
}
