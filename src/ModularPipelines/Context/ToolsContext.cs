using System.Reflection;
using ModularPipelines.Context.Domains;

namespace ModularPipelines.Context;

internal sealed class ToolsContext(IServicesContext services) : IToolsContext
{
    public T Get<T>()
        where T : class => services.TryGet<T>() ?? throw ToolRegistrationExceptionFactory.Create(typeof(T));
}

internal static class ToolRegistrationExceptionFactory
{
    private const string ToolPropertyMetadataPrefix = "ModularPipelines.ToolProperty:";

    public static bool IsToolIntegration(Type serviceType)
    {
        if (serviceType.FullName is not { } fullName)
        {
            return false;
        }

        var metadataTypeName = $"global::{fullName.Replace('+', '.')}";
        return serviceType.Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .Any(attribute =>
                attribute.Key.StartsWith(ToolPropertyMetadataPrefix, StringComparison.Ordinal)
                && string.Equals(attribute.Value, metadataTypeName, StringComparison.Ordinal));
    }

    public static InvalidOperationException Create(Type toolType)
    {
        var assemblyName = toolType.Assembly.GetName().Name;
        return new InvalidOperationException(
            $"Tool integration service '{toolType.FullName}' is not registered. " +
            $"Reference the {assemblyName} package and call its service collection extension marked " +
            "with [ModularPipelinesIntegration]. " +
            "When using Native AOT, register tool integrations explicitly.");
    }
}
