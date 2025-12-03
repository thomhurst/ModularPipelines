using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "setting", "update")]
public record AzSecuritySettingUpdateOptions(
[property: CliFlag("--enabled")] bool Enabled,
[property: CliOption("--name")] string Name
) : AzOptions;