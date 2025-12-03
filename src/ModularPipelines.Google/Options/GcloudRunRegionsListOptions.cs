using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("run", "regions", "list")]
public record GcloudRunRegionsListOptions : GcloudOptions
{
    [CliOption("--platform")]
    public string? Platform { get; set; }
}