using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "record-sets", "transaction", "add")]
public record GcloudDnsRecordSetsTransactionAddOptions(
[property: PositionalArgument] string Rrdatas,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--ttl")] string Ttl,
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--zone")] string Zone
) : GcloudOptions
{
    [CommandSwitch("--transaction-file")]
    public string? TransactionFile { get; set; }
}