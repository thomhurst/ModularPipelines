using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("functions", "runtimes", "list")]
public record GcloudFunctionsRuntimesListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}