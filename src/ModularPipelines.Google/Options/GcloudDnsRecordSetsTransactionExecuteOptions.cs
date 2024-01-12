using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "record-sets", "transaction", "execute")]
public record GcloudDnsRecordSetsTransactionExecuteOptions(
[property: CommandSwitch("--zone")] string Zone
) : GcloudOptions
{
    [CommandSwitch("--transaction-file")]
    public string? TransactionFile { get; set; }
}