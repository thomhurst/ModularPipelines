using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devops", "invoke")]
public record AzDevopsInvokeOptions : AzOptions
{
    [CliOption("--accept-media-type")]
    public string? AcceptMediaType { get; set; }

    [CliOption("--api-version")]
    public string? ApiVersion { get; set; }

    [CliOption("--area")]
    public string? Area { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--encoding")]
    public string? Encoding { get; set; }

    [CliOption("--http-method")]
    public string? HttpMethod { get; set; }

    [CliOption("--in-file")]
    public string? InFile { get; set; }

    [CliOption("--media-type")]
    public string? MediaType { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--out-file")]
    public string? OutFile { get; set; }

    [CliOption("--query-parameters")]
    public string? QueryParameters { get; set; }

    [CliOption("--resource")]
    public string? Resource { get; set; }

    [CliOption("--route-parameters")]
    public string? RouteParameters { get; set; }
}