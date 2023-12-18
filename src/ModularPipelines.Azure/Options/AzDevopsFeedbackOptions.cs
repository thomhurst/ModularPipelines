using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "feedback")]
public record AzDevopsFeedbackOptions : AzOptions
{
    [CommandSwitch("--accept-media-type")]
    public string? AcceptMediaType { get; set; }

    [CommandSwitch("--api-version")]
    public string? ApiVersion { get; set; }

    [CommandSwitch("--area")]
    public string? Area { get; set; }

    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--encoding")]
    public string? Encoding { get; set; }

    [CommandSwitch("--http-method")]
    public string? HttpMethod { get; set; }

    [CommandSwitch("--in-file")]
    public string? InFile { get; set; }

    [CommandSwitch("--media-type")]
    public string? MediaType { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--out-file")]
    public string? OutFile { get; set; }

    [CommandSwitch("--query-parameters")]
    public string? QueryParameters { get; set; }

    [CommandSwitch("--resource")]
    public string? Resource { get; set; }

    [CommandSwitch("--route-parameters")]
    public string? RouteParameters { get; set; }
}