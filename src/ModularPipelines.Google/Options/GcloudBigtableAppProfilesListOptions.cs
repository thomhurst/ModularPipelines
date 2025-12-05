using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "app-profiles", "list")]
public record GcloudBigtableAppProfilesListOptions(
[property: CliOption("--instance")] string Instance
) : GcloudOptions;