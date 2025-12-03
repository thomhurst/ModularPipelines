using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("anthos", "config", "controller", "describe")]
public record GcloudAnthosConfigControllerDescribeOptions(
[property: CliArgument] string Name,
[property: CliArgument] string Location
) : GcloudOptions;