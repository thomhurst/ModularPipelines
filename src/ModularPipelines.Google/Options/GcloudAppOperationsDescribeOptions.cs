using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "operations", "describe")]
public record GcloudAppOperationsDescribeOptions(
[property: CliArgument] string Operation
) : GcloudOptions;