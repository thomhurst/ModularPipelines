using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class AssemblyLoadedTypesProvider : IAssemblyLoadedTypesProvider
{
    private static readonly Assembly ModularPipelinesAssembly = typeof(IModule).Assembly;
    private static readonly string ModularPipelinesAssemblyName = ModularPipelinesAssembly.GetName().Name!;

    public Type[] GetLoadedTypesAssignableTo(Type type)
    {
        return AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(ReferencesModularPipelines)
            .SelectMany(assembly => GetKnownTypes(assembly, type))
            .Where(t => t.IsAssignableTo(type))
            .Where(t => !t.IsAbstract)
            .ToArray();
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
        if (typeof(IModule).IsAssignableFrom(assignableTo)
            && GeneratedModuleMetadata.TryGetModuleTypes(
                assembly,
                out var generatedModuleTypes,
                out var generatedMetadataIsComplete)
            && generatedMetadataIsComplete)
        {
            return generatedModuleTypes;
        }

        return GetLoadableTypes(assembly);
    }

    [RequiresUnreferencedCode("Calls System.Reflection.Assembly.GetTypes()")]
    private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException e)
        {
            return e.Types.OfType<Type>();
        }
    }
}
