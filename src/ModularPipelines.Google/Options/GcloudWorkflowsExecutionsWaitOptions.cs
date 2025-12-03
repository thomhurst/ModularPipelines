using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workflows", "executions", "wait")]
public record GcloudWorkflowsExecutionsWaitOptions(
[property: CliArgument] string Execution,
[property: CliArgument] string Location,
[property: CliArgument] string Workflow
) : GcloudOptions;