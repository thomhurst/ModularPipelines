using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigee", "deployments", "list")]
public record GcloudApigeeDeploymentsListOptions : GcloudOptions
{
    [CliOption("--api")]
    public string? Api { get; set; }

    [CliOption("--environment")]
    public string? Environment { get; set; }

    [CliOption("--organization")]
    public string? Organization { get; set; }

    [CliOption("--revision")]
    public string? Revision { get; set; }
}