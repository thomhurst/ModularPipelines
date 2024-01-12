using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml-engine", "jobs", "cancel")]
public record GcloudMlEngineJobsCancelOptions(
[property: PositionalArgument] string Job
) : GcloudOptions;