using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataflow", "jobs", "cancel")]
public record GcloudDataflowJobsCancelOptions(
[property: CliArgument] string JobId
) : GcloudOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}