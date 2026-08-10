using ModularPipelines.Context.Domains;

namespace ModularPipelines.Context;

internal sealed class ToolsContext(IServicesContext services) : IToolsContext
{
    public T Get<T>()
        where T : class => services.TryGet<T>() ?? throw CreateMissingToolException<T>();

    private static InvalidOperationException CreateMissingToolException<T>()
    {
        var toolType = typeof(T);
        var assemblyName = toolType.Assembly.GetName().Name;
        return new InvalidOperationException(
            $"Tool integration service '{toolType.FullName}' is not registered. " +
            $"Reference the {assemblyName} package and call its Register*Context() service collection extension. " +
            "When using Native AOT, register tool integrations explicitly.");
    }
}
