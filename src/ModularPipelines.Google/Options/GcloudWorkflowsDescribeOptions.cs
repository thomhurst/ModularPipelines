using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workflows", "describe")]
public record GcloudWorkflowsDescribeOptions(
[property: CliArgument] string Workflow,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--revision-id")]
    public string? RevisionId { get; set; }
}