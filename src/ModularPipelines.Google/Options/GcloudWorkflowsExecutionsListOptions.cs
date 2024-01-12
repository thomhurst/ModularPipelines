using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workflows", "executions", "list")]
public record GcloudWorkflowsExecutionsListOptions(
[property: PositionalArgument] string Workflow,
[property: PositionalArgument] string Location
) : GcloudOptions;