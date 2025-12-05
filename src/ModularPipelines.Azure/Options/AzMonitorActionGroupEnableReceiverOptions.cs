using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "action-group", "enable-receiver")]
public record AzMonitorActionGroupEnableReceiverOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--action-group")]
    public string? ActionGroup { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}