using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml-engine", "jobs", "cancel")]
public record GcloudMlEngineJobsCancelOptions(
[property: CliArgument] string Job
) : GcloudOptions;