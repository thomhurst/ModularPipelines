using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataflow", "jobs", "drain")]
public record GcloudDataflowJobsDrainOptions(
[property: CliArgument] string JobId
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}