using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "speech", "operations", "wait")]
public record GcloudMlSpeechOperationsWaitOptions(
[property: PositionalArgument] string Operation
) : GcloudOptions;