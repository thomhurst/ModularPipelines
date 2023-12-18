using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "feature-store", "create")]
public record AzMlFeatureStoreCreateOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--application-insights")]
    public string? ApplicationInsights { get; set; }

    [CommandSwitch("--container-registry")]
    public string? ContainerRegistry { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--file")]
    public string? File { get; set; }

    [CommandSwitch("--image-build-compute")]
    public string? ImageBuildCompute { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--managed-network")]
    public string? ManagedNetwork { get; set; }

    [CommandSwitch("--materialization-identity")]
    public string? MaterializationIdentity { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--not-grant-permissions")]
    public bool? NotGrantPermissions { get; set; }

    [CommandSwitch("--offline-store")]
    public string? OfflineStore { get; set; }

    [CommandSwitch("--primary-user-assigned-identity")]
    public string? PrimaryUserAssignedIdentity { get; set; }

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [BooleanCommandSwitch("--update-dependent-resources")]
    public bool? UpdateDependentResources { get; set; }
}