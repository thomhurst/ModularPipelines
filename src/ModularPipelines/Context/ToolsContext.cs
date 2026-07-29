using ModularPipelines.Context.Domains;

namespace ModularPipelines.Context;

internal sealed class ToolsContext(IServicesContext services) : IToolsContext
{
    public T Get<T>()
        where T : class => services.Get<T>();
}
