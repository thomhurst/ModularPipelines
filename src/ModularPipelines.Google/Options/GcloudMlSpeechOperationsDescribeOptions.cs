using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "speech", "operations", "describe")]
public record GcloudMlSpeechOperationsDescribeOptions(
[property: PositionalArgument] string Operation
) : GcloudOptions;