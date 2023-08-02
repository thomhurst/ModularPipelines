using Azure.Core;

namespace ModularPipelines.Azure.Scopes;

public record AzureManagementGroupIdentifier
(
    string Name
) : AzureScope
{
    public ResourceIdentifier ToManagementGroupIdentifier() => new($"/providers/Microsoft.Management/managementGroups/{Name}");
}