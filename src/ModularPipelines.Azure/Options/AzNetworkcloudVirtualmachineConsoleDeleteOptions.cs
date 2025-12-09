using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkcloud", "virtualmachine", "console", "delete")]
public record AzNetworkcloudVirtualmachineConsoleDeleteOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--virtual-machine-name")]
    public string? VirtualMachineName { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}