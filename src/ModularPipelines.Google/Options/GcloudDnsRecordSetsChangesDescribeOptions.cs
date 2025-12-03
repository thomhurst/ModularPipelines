using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "record-sets", "changes", "describe")]
public record GcloudDnsRecordSetsChangesDescribeOptions(
[property: CliArgument] string ChangeId,
[property: CliOption("--zone")] string Zone
) : GcloudOptions;