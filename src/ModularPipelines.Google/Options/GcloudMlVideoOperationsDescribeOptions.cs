using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "video", "operations", "describe")]
public record GcloudMlVideoOperationsDescribeOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Location
) : GcloudOptions;