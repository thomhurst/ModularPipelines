using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "operations", "describe")]
public record GcloudDnsOperationsDescribeOptions(
[property: CliArgument] string OperationId,
[property: CliOption("--zone")] string Zone
) : GcloudOptions;