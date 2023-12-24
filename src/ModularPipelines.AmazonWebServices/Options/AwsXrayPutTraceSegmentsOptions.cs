using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("xray", "put-trace-segments")]
public record AwsXrayPutTraceSegmentsOptions(
[property: CommandSwitch("--trace-segment-documents")] string[] TraceSegmentDocuments
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}