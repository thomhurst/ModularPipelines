using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("notebooks", "instances", "list")]
public record GcloudNotebooksInstancesListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}