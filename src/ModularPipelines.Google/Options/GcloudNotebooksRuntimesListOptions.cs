using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("notebooks", "runtimes", "list")]
public record GcloudNotebooksRuntimesListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}