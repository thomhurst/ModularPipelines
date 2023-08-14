using Azure.Core;

namespace ModularPipelines.Examples;

public class WellKnownRoleDefinitions
{
    public static ResourceIdentifier BlobStorageOwnerDefinitionId { get; } = new ResourceIdentifier("Blah");
}