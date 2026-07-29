using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ModularPipelines;

internal static class ReferencedAssemblyTraversal
{
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Referenced integrations removed by trimming do not need runtime registration.")]
    public static void LoadModularPipelinesAssemblies(
        IEnumerable<Assembly> currentAssemblies,
        Func<AssemblyName, Assembly> loadAssembly)
    {
        var loadedAssemblies = currentAssemblies
            .Where(static assembly => !assembly.IsDynamic)
            .ToArray();
        var loadedAssemblyNames = loadedAssemblies
            .Select(static assembly => assembly.GetName().Name)
            .OfType<string>()
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var assembliesToVisit = new Queue<Assembly>(loadedAssemblies);

        while (assembliesToVisit.TryDequeue(out var assembly))
        {
            foreach (var referencedAssemblyName in assembly.GetReferencedAssemblies()
                         .Where(IsModularPipelinesAssembly))
            {
                if (referencedAssemblyName.Name is not { } name || !loadedAssemblyNames.Add(name))
                {
                    continue;
                }

                assembliesToVisit.Enqueue(loadAssembly(referencedAssemblyName));
            }
        }
    }

    private static bool IsModularPipelinesAssembly(AssemblyName assemblyName)
    {
        return assemblyName.Name?.Contains("ModularPipeline", StringComparison.OrdinalIgnoreCase) is true;
    }
}
