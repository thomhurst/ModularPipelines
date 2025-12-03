using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deployment-manager", "operations", "describe")]
public record GcloudDeploymentManagerOperationsDescribeOptions(
[property: CliArgument] string OperationName
) : GcloudOptions;