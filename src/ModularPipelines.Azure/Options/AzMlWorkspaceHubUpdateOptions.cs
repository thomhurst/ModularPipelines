using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "workspace-hub", "update")]
public record AzMlWorkspaceHubUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--managed-network")]
    public string? ManagedNetwork { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--primary-user-assigned-identity")]
    public string? PrimaryUserAssignedIdentity { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliFlag("--update-dependent-resources")]
    public bool? UpdateDependentResources { get; set; }
}