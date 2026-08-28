using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using ModularPipelines.Modules;

using ModularPipelines.Generated;

namespace ModularPipelines.Engine;

internal class AssemblyLoadedTypesProvider : IAssemblyLoadedTypesProvider
{
    private static readonly Assembly ModularPipelinesAssembly = typeof(IModule).Assembly;
    private static readonly string ModularPipelinesAssemblyName = ModularPipelinesAssembly.GetName().Name!;

    public Type[] GetLoadedTypesAssignableTo(Type type)
    {
        var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        var knownTypes = RuntimeFeature.IsDynamicCodeSupported
            ? loadedAssemblies
                .Where(ReferencesModularPipelines)
                .SelectMany(assembly => GetKnownTypes(assembly, type))
            : loadedAssemblies
                .SelectMany(assembly => GetGeneratedKnownTypes(assembly, type));

        return
        [
            .. knownTypes
                .Where(t => t.IsAssignableTo(type))
                .Where(t => !t.IsAbstract),
        ];
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Unused-module diagnostics tolerate assembly references removed by trimming.")]
    internal static bool ReferencesModularPipelines(Assembly assembly)
    {
        return assembly == ModularPipelinesAssembly
               || assembly.GetReferencedAssemblies().Any(reference =>
                   string.Equals(reference.Name, ModularPipelinesAssemblyName, StringComparison.Ordinal));
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Generated metadata is used when complete; reflection is the explicit compatibility fallback for dynamic assemblies.")]
    internal static IEnumerable<Type> GetKnownTypes(Assembly assembly, Type assignableTo)
    {
        if (TryGetGeneratedKnownTypes(assembly, assignableTo, out var generatedModuleTypes))
        {
            return generatedModuleTypes;
        }

        return AssemblyTypeLoader.GetLoadableTypes(assembly);
    }

    internal static IEnumerable<Type> GetGeneratedKnownTypes(Assembly assembly, Type assignableTo)
    {
        return TryGetGeneratedKnownTypes(assembly, assignableTo, out var generatedModuleTypes)
            ? generatedModuleTypes
            : [];
    }

    private static bool TryGetGeneratedKnownTypes(
        Assembly assembly,
        Type assignableTo,
        out IReadOnlyList<Type> generatedModuleTypes)
    {
        if (typeof(IModule).IsAssignableFrom(assignableTo)
            && GeneratedModuleMetadata.TryGetModuleTypes(
                assembly,
                out generatedModuleTypes,
                out var generatedMetadataIsComplete)
            && generatedMetadataIsComplete)
        {
            return true;
        }

        generatedModuleTypes = [];
        return false;
    }
}
