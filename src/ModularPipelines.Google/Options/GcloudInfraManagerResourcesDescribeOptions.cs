using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("infra-manager", "resources", "describe")]
public record GcloudInfraManagerResourcesDescribeOptions(
[property: CliArgument] string Resource,
[property: CliArgument] string Deployment,
[property: CliArgument] string Location,
[property: CliArgument] string Revision
) : GcloudOptions;