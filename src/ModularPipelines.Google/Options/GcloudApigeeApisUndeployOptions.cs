using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigee", "apis", "undeploy")]
public record GcloudApigeeApisUndeployOptions(
[property: PositionalArgument] string Revision,
[property: PositionalArgument] string Api,
[property: PositionalArgument] string Environment,
[property: PositionalArgument] string Organization
) : GcloudOptions;