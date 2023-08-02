namespace ModularPipelines.Engine;

internal class AssemblyLoadedTypesProvider : IAssemblyLoadedTypesProvider
{
    public Type[] GetLoadedTypesAssignableTo(Type type)
    {
        return AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(t => t.IsAssignableTo(type))
            .Where(t => !t.IsAbstract)
            .ToArray();
    }
}