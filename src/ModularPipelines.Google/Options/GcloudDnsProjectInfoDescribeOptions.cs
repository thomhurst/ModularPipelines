using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "project-info", "describe")]
public record GcloudDnsProjectInfoDescribeOptions(
[property: CliArgument] string ProjectId
) : GcloudOptions;