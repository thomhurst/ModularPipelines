using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workflows", "executions", "list")]
public record GcloudWorkflowsExecutionsListOptions(
[property: CliArgument] string Workflow,
[property: CliArgument] string Location
) : GcloudOptions;