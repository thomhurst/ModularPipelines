using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "custom-target-types", "describe")]
public record GcloudDeployCustomTargetTypesDescribeOptions(
[property: CliArgument] string CustomTargetType,
[property: CliArgument] string Region
) : GcloudOptions;