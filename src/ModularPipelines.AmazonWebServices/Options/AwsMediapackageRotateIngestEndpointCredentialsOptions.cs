using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediapackage", "rotate-ingest-endpoint-credentials")]
public record AwsMediapackageRotateIngestEndpointCredentialsOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--ingest-endpoint-id")] string IngestEndpointId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}