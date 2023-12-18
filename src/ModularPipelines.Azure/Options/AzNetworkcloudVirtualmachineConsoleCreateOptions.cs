using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud", "virtualmachine", "console", "create")]
public record AzNetworkcloudVirtualmachineConsoleCreateOptions(
[property: BooleanCommandSwitch("--enabled")] bool Enabled,
[property: CommandSwitch("--extended-location")] string ExtendedLocation,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--ssh-public-key")] string SshPublicKey,
[property: CommandSwitch("--virtual-machine-name")] string VirtualMachineName
) : AzOptions
{
    [CommandSwitch("--expiration")]
    public string? Expiration { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

