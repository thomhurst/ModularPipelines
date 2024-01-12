using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workflows", "describe")]
public record GcloudWorkflowsDescribeOptions(
[property: PositionalArgument] string Workflow,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--revision-id")]
    public string? RevisionId { get; set; }
}