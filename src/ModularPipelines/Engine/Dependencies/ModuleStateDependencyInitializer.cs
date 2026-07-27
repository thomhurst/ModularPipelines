using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Dependencies;

internal static class ModuleStateDependencyInitializer
{
    public static IReadOnlyList<(Type DependencyType, bool Optional)> Populate(
        ModuleState state,
        IReadOnlyList<Type> availableModuleTypes,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry)
    {
        var dependencies = ModuleDependencyResolver.GetAllDependencies(
                state.Module,
                availableModuleTypes,
                dependencyRegistry,
                metadataRegistry)
            .ToArray();

        foreach (var (dependencyType, optional) in dependencies)
        {
            Record(state, dependencyType, optional);
        }

        return dependencies;
    }

    public static void Record(ModuleState state, Type dependencyType, bool optional)
    {
        state.Dependencies[dependencyType] =
            state.Dependencies.TryGetValue(dependencyType, out var existingOptional)
                ? existingOptional && optional
                : optional;
    }
}
