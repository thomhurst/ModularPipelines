using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workflows", "executions", "wait")]
public record GcloudWorkflowsExecutionsWaitOptions(
[property: PositionalArgument] string Execution,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Workflow
) : GcloudOptions;