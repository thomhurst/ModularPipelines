using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3outposts", "delete-endpoint")]
public record AwsS3outpostsDeleteEndpointOptions(
[property: CliOption("--endpoint-id")] string EndpointId,
[property: CliOption("--outpost-id")] string OutpostId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}