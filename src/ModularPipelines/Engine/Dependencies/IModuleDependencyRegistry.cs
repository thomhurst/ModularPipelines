namespace ModularPipelines.Engine.Dependencies;

/// <summary>
/// Registry for dynamic module dependencies added at registration time.
/// </summary>
internal interface IModuleDependencyRegistry
{
    void AddDynamicDependency(Type module, Type dependency);

    void RemoveDependency(Type module, Type dependency);

    IEnumerable<Type> GetDynamicDependencies(Type module);

    bool HasDynamicDependencies(Type module);
}
