using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("infra-manager", "revisions", "describe")]
public record GcloudInfraManagerRevisionsDescribeOptions(
[property: PositionalArgument] string Revision,
[property: PositionalArgument] string Deployment,
[property: PositionalArgument] string Location
) : GcloudOptions;