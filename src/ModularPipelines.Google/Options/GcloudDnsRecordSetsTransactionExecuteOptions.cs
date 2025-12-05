using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "record-sets", "transaction", "execute")]
public record GcloudDnsRecordSetsTransactionExecuteOptions(
[property: CliOption("--zone")] string Zone
) : GcloudOptions
{
    [CliOption("--transaction-file")]
    public string? TransactionFile { get; set; }
}