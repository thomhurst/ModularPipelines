using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("connectedmachine", "upgrade-extension")]
public record AzConnectedmachineUpgradeExtensionOptions : AzOptions
{
    [CliOption("--extension-targets")]
    public string? ExtensionTargets { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--machine-name")]
    public string? MachineName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}