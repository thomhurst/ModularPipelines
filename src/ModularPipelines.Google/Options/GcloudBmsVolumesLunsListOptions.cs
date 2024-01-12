using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bms", "volumes", "luns", "list")]
public record GcloudBmsVolumesLunsListOptions(
[property: CommandSwitch("--volume")] string Volume,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;