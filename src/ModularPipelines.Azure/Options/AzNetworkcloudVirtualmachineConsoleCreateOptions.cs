using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkcloud", "virtualmachine", "console", "create")]
public record AzNetworkcloudVirtualmachineConsoleCreateOptions(
[property: CliFlag("--enabled")] bool Enabled,
[property: CliOption("--extended-location")] string ExtendedLocation,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--ssh-public-key")] string SshPublicKey,
[property: CliOption("--virtual-machine-name")] string VirtualMachineName
) : AzOptions
{
    [CliOption("--expiration")]
    public string? Expiration { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}