using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "feature-store", "create")]
public record AzMlFeatureStoreCreateOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--application-insights")]
    public string? ApplicationInsights { get; set; }

    [CliOption("--container-registry")]
    public string? ContainerRegistry { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }

    [CliOption("--image-build-compute")]
    public string? ImageBuildCompute { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--managed-network")]
    public string? ManagedNetwork { get; set; }

    [CliOption("--materialization-identity")]
    public string? MaterializationIdentity { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--not-grant-permissions")]
    public bool? NotGrantPermissions { get; set; }

    [CliOption("--offline-store")]
    public string? OfflineStore { get; set; }

    [CliOption("--primary-user-assigned-identity")]
    public string? PrimaryUserAssignedIdentity { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliFlag("--update-dependent-resources")]
    public bool? UpdateDependentResources { get; set; }
}