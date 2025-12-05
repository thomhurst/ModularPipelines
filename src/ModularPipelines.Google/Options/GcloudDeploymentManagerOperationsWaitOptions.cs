using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deployment-manager", "operations", "wait")]
public record GcloudDeploymentManagerOperationsWaitOptions(
[property: CliArgument] string OperationName
) : GcloudOptions;