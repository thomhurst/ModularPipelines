using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "datasets", "list")]
public record GcloudHealthcareDatasetsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}