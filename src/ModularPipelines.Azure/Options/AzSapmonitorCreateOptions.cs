using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sapmonitor", "create")]
public record AzSapmonitorCreateOptions(
[property: CliOption("--hana-subnet")] string HanaSubnet,
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--region")] string Region,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--dca")]
    public bool? Dca { get; set; }

    [CliOption("--lawsid")]
    public string? Lawsid { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}