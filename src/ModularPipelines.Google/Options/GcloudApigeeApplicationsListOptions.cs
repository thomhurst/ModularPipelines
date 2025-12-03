using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigee", "applications", "list")]
public record GcloudApigeeApplicationsListOptions : GcloudOptions
{
    [CliOption("--developer")]
    public string? Developer { get; set; }

    [CliOption("--organization")]
    public string? Organization { get; set; }
}