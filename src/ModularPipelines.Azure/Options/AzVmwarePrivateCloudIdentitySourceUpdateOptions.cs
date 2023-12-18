using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-cloud", "identity-source", "update")]
public record AzVmwarePrivateCloudIdentitySourceUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--private-cloud")] string PrivateCloud,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--alias")]
    public string? Alias { get; set; }

    [CommandSwitch("--base-group-dn")]
    public string? BaseGroupDn { get; set; }

    [CommandSwitch("--base-user-dn")]
    public string? BaseUserDn { get; set; }

    [CommandSwitch("--domain")]
    public string? Domain { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--primary-server")]
    public string? PrimaryServer { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--secondary-server")]
    public string? SecondaryServer { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--ssl")]
    public string? Ssl { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }
}