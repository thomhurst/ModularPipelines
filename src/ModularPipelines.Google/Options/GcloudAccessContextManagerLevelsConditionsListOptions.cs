using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "levels", "conditions", "list")]
public record GcloudAccessContextManagerLevelsConditionsListOptions(
[property: CliOption("--level")] string Level,
[property: CliOption("--policy")] string Policy
) : GcloudOptions;