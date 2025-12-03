using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workbench", "instances", "list")]
public record GcloudWorkbenchInstancesListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}