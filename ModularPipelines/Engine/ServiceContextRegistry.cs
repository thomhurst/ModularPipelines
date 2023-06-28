using Microsoft.Extensions.DependencyInjection;

namespace ModularPipelines.Engine;

public static class ServiceContextRegistry
{
    internal static readonly List<Action<IServiceCollection>> ContextRegistrationDelegates = new();

    public static void RegisterContext(Action<IServiceCollection> contextRegistrationDelegate)
    {
        ContextRegistrationDelegates.Add(contextRegistrationDelegate);
    }
}