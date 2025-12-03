using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "app-profiles", "delete")]
public record GcloudBigtableAppProfilesDeleteOptions(
[property: CliArgument] string AppProfile,
[property: CliArgument] string Instance
) : GcloudOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }
}