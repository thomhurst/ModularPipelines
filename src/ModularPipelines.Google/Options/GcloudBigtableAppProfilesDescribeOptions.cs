using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "app-profiles", "describe")]
public record GcloudBigtableAppProfilesDescribeOptions(
[property: CliArgument] string AppProfile,
[property: CliArgument] string Instance
) : GcloudOptions;