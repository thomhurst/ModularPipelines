using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "feature-store", "update")]
public record AzMlFeatureStoreUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

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

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--image-build-compute")]
    public string? ImageBuildCompute { get; set; }

    [CommandSwitch("--managed-network")]
    public string? ManagedNetwork { get; set; }

    [CommandSwitch("--materialization-identity")]
    public string? MaterializationIdentity { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--not-grant-permissions")]
    public bool? NotGrantPermissions { get; set; }

    [CommandSwitch("--primary-user-assigned-identity")]
    public string? PrimaryUserAssignedIdentity { get; set; }

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [BooleanCommandSwitch("--update-dependent-resources")]
    public bool? UpdateDependentResources { get; set; }
}

