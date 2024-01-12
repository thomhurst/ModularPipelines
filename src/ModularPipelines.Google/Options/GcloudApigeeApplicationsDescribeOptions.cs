using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigee", "applications", "describe")]
public record GcloudApigeeApplicationsDescribeOptions(
[property: PositionalArgument] string Application,
[property: PositionalArgument] string Organization
) : GcloudOptions;