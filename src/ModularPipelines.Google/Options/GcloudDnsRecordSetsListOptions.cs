using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "record-sets", "list")]
public record GcloudDnsRecordSetsListOptions(
[property: CliOption("--zone")] string Zone
) : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }
}