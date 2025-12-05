using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigee", "apis", "undeploy")]
public record GcloudApigeeApisUndeployOptions(
[property: CliArgument] string Revision,
[property: CliArgument] string Api,
[property: CliArgument] string Environment,
[property: CliArgument] string Organization
) : GcloudOptions;