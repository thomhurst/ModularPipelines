using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "app-profiles", "list")]
public record GcloudBigtableAppProfilesListOptions(
[property: CommandSwitch("--instance")] string Instance
) : GcloudOptions;