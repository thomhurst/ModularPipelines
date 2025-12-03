using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "record-sets", "transaction", "remove")]
public record GcloudDnsRecordSetsTransactionRemoveOptions(
[property: CliArgument] string Rrdatas,
[property: CliOption("--name")] string Name,
[property: CliOption("--ttl")] string Ttl,
[property: CliOption("--type")] string Type,
[property: CliOption("--zone")] string Zone
) : GcloudOptions
{
    [CliOption("--transaction-file")]
    public string? TransactionFile { get; set; }
}