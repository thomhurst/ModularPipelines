using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigee", "applications", "describe")]
public record GcloudApigeeApplicationsDescribeOptions(
[property: CliArgument] string Application,
[property: CliArgument] string Organization
) : GcloudOptions;