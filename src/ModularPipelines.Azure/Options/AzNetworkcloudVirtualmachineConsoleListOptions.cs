using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkcloud", "virtualmachine", "console", "list")]
public record AzNetworkcloudVirtualmachineConsoleListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--virtual-machine-name")] string VirtualMachineName
) : AzOptions
{
    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }
}