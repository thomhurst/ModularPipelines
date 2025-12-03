using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "du", "device", "log", "collect")]
public record AzIotDuDeviceLogCollectOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--agent-id")] string AgentId,
[property: CliOption("--instance")] string Instance,
[property: CliOption("--lcid")] string Lcid
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}