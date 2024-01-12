using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("infra-manager", "resources", "describe")]
public record GcloudInfraManagerResourcesDescribeOptions(
[property: PositionalArgument] string Resource,
[property: PositionalArgument] string Deployment,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Revision
) : GcloudOptions;