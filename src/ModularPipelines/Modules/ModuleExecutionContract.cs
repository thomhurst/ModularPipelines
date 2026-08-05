namespace ModularPipelines.Modules;

internal static class ModuleExecutionContract
{
    public static IInternalModule AsInternal(this IModule module)
    {
        return module as IInternalModule
               ?? throw CreateException(module.GetType());
    }

    public static void Validate(Type moduleType)
    {
        if (!typeof(IInternalModule).IsAssignableFrom(moduleType))
        {
            throw CreateException(moduleType);
        }
    }

    private static InvalidOperationException CreateException(Type moduleType)
    {
        return new InvalidOperationException(
            $"Module type '{moduleType.FullName}' must derive from Module<T> or SyncModule<T>; "
            + "direct IModule implementations cannot be executed.");
    }
}
