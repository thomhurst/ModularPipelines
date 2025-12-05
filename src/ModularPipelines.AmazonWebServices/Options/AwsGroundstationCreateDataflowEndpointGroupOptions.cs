using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("groundstation", "create-dataflow-endpoint-group")]
public record AwsGroundstationCreateDataflowEndpointGroupOptions(
[property: CliOption("--endpoint-details")] string[] EndpointDetails
) : AwsOptions
{
    [CliOption("--contact-post-pass-duration-seconds")]
    public int? ContactPostPassDurationSeconds { get; set; }

    [CliOption("--contact-pre-pass-duration-seconds")]
    public int? ContactPrePassDurationSeconds { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}