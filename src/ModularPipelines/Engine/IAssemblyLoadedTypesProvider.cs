namespace ModularPipelines.Engine;

internal interface IAssemblyLoadedTypesProvider
{
    Type[] GetLoadedTypesAssignableTo(Type type);
}