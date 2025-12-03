using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netapp", "active-directories", "list")]
public record GcloudNetappActiveDirectoriesListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}