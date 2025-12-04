using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "setting", "show")]
public record AzSecuritySettingShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions;