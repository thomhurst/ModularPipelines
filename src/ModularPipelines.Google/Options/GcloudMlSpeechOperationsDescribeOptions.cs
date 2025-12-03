using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "speech", "operations", "describe")]
public record GcloudMlSpeechOperationsDescribeOptions(
[property: CliArgument] string Operation
) : GcloudOptions;