using System.Reflection;

namespace ModularPipelines.Engine;

internal class AssemblyLoadedTypesProvider : IAssemblyLoadedTypesProvider
{
    public Type[] GetLoadedTypesAssignableTo(Type type)
    {
        return AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(GetLoadableTypes)
            .Where(t => t.IsAssignableTo(type))
            .Where(t => !t.IsAbstract)
            .ToArray();
    }

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