using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "list")]
public record GcloudBuildsListOptions : GcloudOptions
{
    [CliFlag("--ongoing")]
    public bool? Ongoing { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}