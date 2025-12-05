using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("notebooks", "environments", "list")]
public record GcloudNotebooksEnvironmentsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}