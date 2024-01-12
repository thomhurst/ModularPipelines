using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "record-sets", "changes", "describe")]
public record GcloudDnsRecordSetsChangesDescribeOptions(
[property: PositionalArgument] string ChangeId,
[property: CommandSwitch("--zone")] string Zone
) : GcloudOptions;