using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("infra-manager", "revisions", "describe")]
public record GcloudInfraManagerRevisionsDescribeOptions(
[property: CliArgument] string Revision,
[property: CliArgument] string Deployment,
[property: CliArgument] string Location
) : GcloudOptions;