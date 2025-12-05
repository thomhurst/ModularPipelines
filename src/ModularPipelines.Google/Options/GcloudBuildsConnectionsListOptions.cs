using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "connections", "list")]
public record GcloudBuildsConnectionsListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}