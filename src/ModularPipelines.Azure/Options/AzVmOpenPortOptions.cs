using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vm", "open-port")]
public record AzVmOpenPortOptions(
[property: CliOption("--port")] int Port
) : AzOptions
{
    [CliFlag("--apply-to-subnet")]
    public bool? ApplyToSubnet { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--nsg-name")]
    public string? NsgName { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}