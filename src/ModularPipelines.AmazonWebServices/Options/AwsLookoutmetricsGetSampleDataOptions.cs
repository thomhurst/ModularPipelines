using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutmetrics", "get-sample-data")]
public record AwsLookoutmetricsGetSampleDataOptions : AwsOptions
{
    [CommandSwitch("--s3-source-config")]
    public string? S3SourceConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}