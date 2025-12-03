using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "operations", "describe")]
public record GcloudTransferOperationsDescribeOptions(
[property: CliArgument] string Name
) : GcloudOptions;