using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("functions", "list")]
public record GcloudFunctionsListOptions : GcloudOptions
{
    [CliOption("--regions")]
    public string[]? Regions { get; set; }
}