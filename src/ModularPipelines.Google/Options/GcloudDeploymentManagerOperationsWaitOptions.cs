using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment-manager", "operations", "wait")]
public record GcloudDeploymentManagerOperationsWaitOptions(
[property: PositionalArgument] string OperationName
) : GcloudOptions;