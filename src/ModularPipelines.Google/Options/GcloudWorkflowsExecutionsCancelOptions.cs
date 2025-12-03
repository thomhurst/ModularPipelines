using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workflows", "executions", "cancel")]
public record GcloudWorkflowsExecutionsCancelOptions(
[property: CliArgument] string Execution,
[property: CliArgument] string Location,
[property: CliArgument] string Workflow
) : GcloudOptions;