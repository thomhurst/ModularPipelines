using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-cloud", "identity-source", "create")]
public record AzVmwarePrivateCloudIdentitySourceCreateOptions(
[property: CliOption("--alias")] string Alias,
[property: CliOption("--base-group-dn")] string BaseGroupDn,
[property: CliOption("--base-user-dn")] string BaseUserDn,
[property: CliOption("--domain")] string Domain,
[property: CliOption("--name")] string Name,
[property: CliOption("--password")] string Password,
[property: CliOption("--primary-server")] string PrimaryServer,
[property: CliOption("--private-cloud")] string PrivateCloud,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--username")] string Username
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--secondary-server")]
    public string? SecondaryServer { get; set; }

    [CliOption("--ssl")]
    public string? Ssl { get; set; }
}