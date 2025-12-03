using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "speech", "operations", "wait")]
public record GcloudMlSpeechOperationsWaitOptions(
[property: CliArgument] string Operation
) : GcloudOptions;