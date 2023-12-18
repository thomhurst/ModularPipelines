using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-cloud", "addidentitysource")]
public record AzVmwarePrivateCloudAddIdentitySourceOptions(
[property: CommandSwitch("--alias")] string Alias,
[property: CommandSwitch("--base-group-dn")] string BaseGroupDn,
[property: CommandSwitch("--base-user-dn")] string BaseUserDn,
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--password")] string Password,
[property: CommandSwitch("--primary-server")] string PrimaryServer,
[property: CommandSwitch("--private-cloud")] string PrivateCloud,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--username")] string Username
) : AzOptions
{
    [CommandSwitch("--secondary-server")]
    public string? SecondaryServer { get; set; }

    [CommandSwitch("--ssl")]
    public string? Ssl { get; set; }
}

