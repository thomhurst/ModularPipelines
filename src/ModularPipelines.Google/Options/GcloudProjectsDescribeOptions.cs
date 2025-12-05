using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("projects", "describe")]
public record GcloudProjectsDescribeOptions(
[property: CliArgument] string ProjectIdOrNumber
) : GcloudOptions;