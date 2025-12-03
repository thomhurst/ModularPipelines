using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "record-sets", "delete")]
public record GcloudDnsRecordSetsDeleteOptions(
[property: CliArgument] string DnsName,
[property: CliOption("--type")] string Type,
[property: CliOption("--zone")] string Zone
) : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}