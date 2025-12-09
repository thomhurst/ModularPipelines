using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("xray", "put-trace-segments")]
public record AwsXrayPutTraceSegmentsOptions(
[property: CliOption("--trace-segment-documents")] string[] TraceSegmentDocuments
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}