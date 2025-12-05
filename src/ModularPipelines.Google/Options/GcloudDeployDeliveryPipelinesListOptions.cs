using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "delivery-pipelines", "list")]
public record GcloudDeployDeliveryPipelinesListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}