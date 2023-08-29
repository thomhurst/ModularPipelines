using Azure.Core;

namespace ModularPipelines.Examples;

public static class WellKnownRoleDefinitions
{
    public static ResourceIdentifier BlobStorageOwnerDefinitionId { get; } = new ResourceIdentifier("Blah");
}