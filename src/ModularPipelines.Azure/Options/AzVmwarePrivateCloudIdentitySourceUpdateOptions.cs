using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-cloud", "identity-source", "update")]
public record AzVmwarePrivateCloudIdentitySourceUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--private-cloud")] string PrivateCloud,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--alias")]
    public string? Alias { get; set; }

    [CliOption("--base-group-dn")]
    public string? BaseGroupDn { get; set; }

    [CliOption("--base-user-dn")]
    public string? BaseUserDn { get; set; }

    [CliOption("--domain")]
    public string? Domain { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--primary-server")]
    public string? PrimaryServer { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--secondary-server")]
    public string? SecondaryServer { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--ssl")]
    public string? Ssl { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }
}