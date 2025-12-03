using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bms", "volumes", "luns", "list")]
public record GcloudBmsVolumesLunsListOptions(
[property: CliOption("--volume")] string Volume,
[property: CliOption("--region")] string Region
) : GcloudOptions;