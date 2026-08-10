using System.Reflection;
using ModularPipelines.Context.Domains;

namespace ModularPipelines.Context;

internal sealed class ToolsContext(IServicesContext services) : IToolsContext
{
    public T Get<T>()
        where T : class
    {
        var toolType = typeof(T);
        return services.TryGet<T>() ?? throw ToolRegistrationExceptionFactory.Create(
            toolType,
            ToolRegistrationExceptionFactory.FindIntegrationAssemblyName(toolType));
    }
}

internal static class ToolRegistrationExceptionFactory
{
    private const string ToolTypeIdentityMetadataPrefix = "ModularPipelines.ToolTypeIdentity:";

    public static string? FindIntegrationAssemblyName(Type serviceType)
    {
        var typeIdentity = GetTypeIdentity(serviceType);
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (assembly.GetCustomAttributes<AssemblyMetadataAttribute>().Any(attribute =>
                    attribute.Key.StartsWith(ToolTypeIdentityMetadataPrefix, StringComparison.Ordinal)
                    && string.Equals(attribute.Value, typeIdentity, StringComparison.Ordinal)))
            {
                return assembly.GetName().Name;
            }
        }

        return null;
    }

    public static InvalidOperationException Create(Type toolType, string? integrationAssemblyName)
    {
        var registrationGuidance = integrationAssemblyName is null
            ? "Call the service collection extension marked with [ModularPipelinesIntegration]. "
            : $"Ensure the integration assembly '{integrationAssemblyName}' is referenced and call its " +
              "service collection extension marked " +
              "with [ModularPipelinesIntegration]. ";
        return new InvalidOperationException(
            $"Tool integration service '{toolType.FullName}' is not registered. " +
            registrationGuidance +
            "When using Native AOT, register tool integrations explicitly.");
    }

    private static string GetTypeIdentity(Type type)
    {
        if (type.IsArray)
        {
            return $"{GetTypeIdentity(type.GetElementType()!)}[{new string(',', type.GetArrayRank() - 1)}]";
        }

        var definition = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
        var assemblyName = NormalizeAssemblyName(definition.Assembly.GetName().Name!);
        var identity = $"{assemblyName}:{definition.FullName}";
        var typeArguments = type.IsGenericType
            ? type.GetGenericArguments()
            : Type.EmptyTypes;
        return typeArguments.Length == 0
            ? identity
            : $"{identity}[{string.Join(",", typeArguments.Select(GetTypeIdentity))}]";
    }

    private static string NormalizeAssemblyName(string assemblyName) =>
        assemblyName is "System" or "mscorlib" or "netstandard"
        || assemblyName.StartsWith("System.", StringComparison.Ordinal)
            ? "framework"
            : assemblyName;
}
