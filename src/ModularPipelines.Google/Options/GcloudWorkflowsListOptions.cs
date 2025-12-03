using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workflows", "list")]
public record GcloudWorkflowsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}