using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment-manager", "operations", "describe")]
public record GcloudDeploymentManagerOperationsDescribeOptions(
[property: PositionalArgument] string OperationName
) : GcloudOptions;