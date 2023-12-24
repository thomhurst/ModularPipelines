using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisanalytics", "discover-input-schema")]
public record AwsKinesisanalyticsDiscoverInputSchemaOptions : AwsOptions
{
    [CommandSwitch("--resource-arn")]
    public string? ResourceArn { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--input-starting-position-configuration")]
    public string? InputStartingPositionConfiguration { get; set; }

    [CommandSwitch("--s3-configuration")]
    public string? S3Configuration { get; set; }

    [CommandSwitch("--input-processing-configuration")]
    public string? InputProcessingConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}