using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workbench", "instances", "describe")]
public record GcloudWorkbenchInstancesDescribeOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Location
) : GcloudOptions;