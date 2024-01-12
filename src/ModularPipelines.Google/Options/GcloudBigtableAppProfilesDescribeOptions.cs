using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "app-profiles", "describe")]
public record GcloudBigtableAppProfilesDescribeOptions(
[property: PositionalArgument] string AppProfile,
[property: PositionalArgument] string Instance
) : GcloudOptions;